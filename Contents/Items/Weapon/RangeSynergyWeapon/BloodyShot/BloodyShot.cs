using BossRush.Common.RoguelikeChange;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.BloodyShot {
	internal class BloodyShot : SynergyModItem, IRogueLikeRangeGun {
		public float OffSetPosition => 30f;
		public float Spread { get; set; }
		public override void SetDefaults() {
			Item.BossRushDefaultRange(42, 36, 25, 1f, 20, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<BloodBullet>(), 1, false, AmmoID.Bullet);
			Item.scale = 0.9f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item11;
			Spread = 5;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (modplayer.BloodyShoot_AquaScepter)
				tooltips.Add(new TooltipLine(Mod, "BloodyShoot_AquaScepter", $"[i:{ItemID.AquaScepter}] Your gun now shoot out damaging blood"));
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.HasItem(ItemID.AquaScepter)) {
				modplayer.BloodyShoot_AquaScepter = true;
				modplayer.SynergyBonus++;
			}

		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ModContent.ProjectileType<BloodBullet>();
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (modplayer.BloodyShoot_AquaScepter) {
				for (int i = 0; i < Main.rand.Next(3, 6); i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(20).Vector2RandomSpread(4, Main.rand.NextFloat(.9f, 1.1f)) * .5f, ModContent.ProjectileType<BloodWater>(), damage, knockback, player.whoAmI);
				}
			}
			CanShootItem = true;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(4, 2);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Handgun)
				.AddIngredient(ItemID.BloodRainBow)
				.Register();
		}
	}
	public class BloodWater : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<BloodBullet>();
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.extraUpdates = 6;
			Projectile.tileCollide = true;
			Projectile.hide = true;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			int dust = Dust.NewDust(Projectile.Center, 10, 10, DustID.Blood);
			Main.dust[dust].fadeIn = .5f;
			Main.dust[dust].velocity = Vector2.Zero;
			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 100) {
				Projectile.velocity.Y += .01f;
			}
		}
	}
	internal class BloodBullet : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.extraUpdates = 20;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			if (!npc.HasBuff(ModContent.BuffType<BoilingBlood>()) && Main.rand.NextBool(10)) {
				npc.AddBuff(ModContent.BuffType<BoilingBlood>(), 90);
			}
			else {
				hit.Damage += (int)(Projectile.damage * .25f);
				int randNum2 = 1 + Main.rand.Next(4, 6);
				for (int i = 0; i < randNum2; i++) {
					Vector2 newPos = npc.Center + Main.rand.NextVector2CircularEdge(npc.width, npc.height) * 1.1f;
					Vector2 vel = (newPos - npc.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(4, 7);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos, vel, ProjectileID.BloodArrow, (int)(hit.Damage * 0.75f), hit.Knockback, player.whoAmI);
				}
			}
			int randNum = 1 + Main.rand.Next(3, 6);
			for (int i = 0; i < randNum; i++) {
				Vector2 newPos = new Vector2(Projectile.position.X + Main.rand.Next(-200, 200) + 5, Projectile.position.Y - (600 + Main.rand.Next(1, 200)) + 5);
				Projectile.position.X += Main.rand.Next(-50, 50);
				Vector2 safeAimto = (Projectile.position - newPos).SafeNormalize(Vector2.UnitX);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos, safeAimto * 25, ProjectileID.BloodArrow, (int)(hit.Damage * 0.75f), hit.Knockback, player.whoAmI);
			}
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Blood);
			Main.dust[dust].noGravity = true;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrail(lightColor);
			return true;
		}
	}
	public class BoilingBlood : SynergyBuff {
		public override void SynergySetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void UpdateNPC(NPC npc, ref int buffIndex) {
			npc.lifeRegen -= 50;
			for (int i = 0; i < 3; i++) {
				int dust = Dust.NewDust(npc.Center + Main.rand.NextVector2Circular(npc.width, npc.height) * .75f, 0, 0, DustID.Blood);
				Main.dust[dust].velocity = Vector2.Zero;
			}
		}
	}
}
