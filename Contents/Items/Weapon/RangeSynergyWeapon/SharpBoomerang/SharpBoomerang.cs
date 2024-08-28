using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SharpBoomerang {
	internal class SharpBoomerang : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(38, 72, 17, 5f, 15, 15, ItemUseStyleID.Swing, ModContent.ProjectileType<SharpBoomerangP>(), 40, false);
			Item.crit = 6;
			Item.scale = 0.5f;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(platinum: 5);
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (modplayer.SharpBoomerang_EnchantedBoomerang)
				tooltips.Add(new TooltipLine(Mod, "SharpBoomerang_EnchantedBoomerang", $"[i:{ItemID.EnchantedBoomerang}] You throw out additional boomerang"));
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.HasItem(ItemID.EnchantedBoomerang)) {
				modplayer.SharpBoomerang_EnchantedBoomerang = true;
				modplayer.SynergyBonus++;
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			CanShootItem = true;
			if (modplayer.SharpBoomerang_EnchantedBoomerang) {
				for (int i = -1; i < 2; i++) {
					if (i == 0)
						continue;
					Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, i);
				}
				CanShootItem = false;
			}
		}
		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[ModContent.ProjectileType<SharpBoomerangP>()] < 1;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.WoodenBoomerang)
				.AddRecipeGroup("OreShortSword")
				.Register();
		}
	}
	internal class SharpBoomerangP : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<SharpBoomerang>();

		public override void SetDefaults() {
			Projectile.width = Projectile.height = 72;
			Projectile.scale = .5f;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 999;
			DrawOriginOffsetX = 5;
			DrawOriginOffsetY = -10;
			Projectile.usesLocalNPCImmunity = true;
		}
		float MaxLengthX = 0;
		float MaxLengthY = 0;

		int MouseXPosDirection;
		int maxProgress = 25;
		int progression = 0;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (Projectile.timeLeft == 999) {
				if (Projectile.ai[0] == 0)
					Projectile.ai[0] = Main.rand.NextBool().ToDirectionInt();
				MaxLengthX = (Main.MouseWorld - player.Center).Length();
				maxProgress += (int)(MaxLengthX * .05f);
				progression = maxProgress;
				MouseXPosDirection = (int)Projectile.ai[0] * (Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1);
				MaxLengthY = -(MaxLengthX + Main.rand.NextFloat(-10, 80)) * .25f * MouseXPosDirection;
			}
			if (player.dead || !player.active || progression <= 0) {
				Projectile.Kill();
			}
			int halfmaxProgress = (int)(maxProgress * .5f);
			int quadmaxProgress = (int)(maxProgress * .25f);
			float progress;
			if (progression > halfmaxProgress) {
				progress = (maxProgress - progression) / (float)halfmaxProgress;
			}
			else {
				progress = progression / (float)halfmaxProgress;
			}
			float X = MathHelper.SmoothStep(-30, MaxLengthX, progress);
			ProgressYHandle(progression, halfmaxProgress, quadmaxProgress, out float Y);
			Vector2 VelocityPosition = new Vector2(X, Y).RotatedBy(Projectile.velocity.ToRotation());
			Projectile.Center = player.Center + VelocityPosition;
			progression--;
			Projectile.rotation += MathHelper.ToRadians(55);
		}
		private void ProgressYHandle(int timeleft, float progressMaxHalf, float progressMaxQuad, out float Y) {
			if (timeleft > progressMaxHalf + progressMaxQuad) {
				float progressY = 1 - (timeleft - (progressMaxHalf + progressMaxQuad)) / progressMaxQuad;
				Y = MathHelper.SmoothStep(0, MaxLengthY, progressY);
				return;
			}
			if (timeleft > progressMaxQuad) {
				float progressY = 1 - (timeleft - progressMaxQuad) / progressMaxHalf;
				Y = MathHelper.SmoothStep(MaxLengthY, -MaxLengthY, progressY);
				return;
			}
			else {
				float progressY = 1 - timeleft / progressMaxQuad;
				Y = MathHelper.SmoothStep(-MaxLengthY, 0, progressY);
				return;
			}
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			Vector2 RandomPos = Projectile.Center + Main.rand.NextVector2CircularEdge(50, 50);
			Vector2 DistanceToAim = (npc.Center - RandomPos).SafeNormalize(Vector2.UnitX) * 4f;
			int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), RandomPos, DistanceToAim, ProjectileID.SuperStarSlash, Projectile.damage, 0, Projectile.owner);
			Main.projectile[proj].usesIDStaticNPCImmunity = true;
			npc.immune[Projectile.owner] = 3;
		}
	}
}
