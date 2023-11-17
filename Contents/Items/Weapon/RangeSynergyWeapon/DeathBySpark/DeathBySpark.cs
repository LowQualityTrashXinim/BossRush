using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.DeathBySpark {
	internal class DeathBySpark : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(34, 24, 15, 1f, 84, 84, ItemUseStyleID.Shoot, ModContent.ProjectileType<SparkFlare>(), 12, false, AmmoID.Flare);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item11;
			Item.value = Item.buyPrice(gold: 50);
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if(modplayer.DeathBySpark_AleThrowingGlove) {
				tooltips.Add(new TooltipLine(Mod, "DeathBySpark_AleThrowingGlove", $"[i:{ItemID.AleThrowingGlove}] Flare will shoot out ale that deal 25% more damage"));
			}
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.HasItem(ItemID.AleThrowingGlove)) {
				modplayer.SynergyBonus++;
				modplayer.DeathBySpark_AleThrowingGlove = true;
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ModContent.ProjectileType<SparkFlare>();
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.FlareGun)
				.AddIngredient(ItemID.WandofSparking)
				.Register();
		}
	}
	internal class SparkFlare : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = 22;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.light = 2f;
		}
		bool hittile = false;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.ai[0]++;
			if (!hittile) {
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Projectile.ai[0] >= 40) {
					if (Projectile.velocity.Y < 16) Projectile.velocity.Y += 0.1f;
				}
			}
			Vector2 OppositeVelocity = Projectile.rotation.ToRotationVector2() * -4.5f;
			int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, OppositeVelocity + Main.rand.NextVector2Circular(1f, 1f), ProjectileID.WandOfSparkingSpark, (int)(Projectile.damage * 0.65f), 1f, player.whoAmI);
			Main.projectile[proj].usesLocalNPCImmunity = true;
			Main.projectile[proj].localNPCHitCooldown = 20;
			Main.projectile[proj].timeLeft = 12;
			if (modplayer.DeathBySpark_AleThrowingGlove && ++Projectile.ai[1] >= 30) {
				Projectile.ai[1] = 0;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, OppositeVelocity.Vector2RotateByRandom(15) * 4f, ProjectileID.Ale, (int)(Projectile.damage * 1.25f), 3f, player.whoAmI);
			}
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			npc.AddBuff(BuffID.OnFire, 300);
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (!hittile) {
				Projectile.position += Projectile.velocity;
			}
			hittile = true;
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			Projectile.velocity = Vector2.Zero;
			return false;
		}
	}
}
