using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.RectangleShotgun {
	class RectangleShotgun : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(12, 74, 50, 4f, 10, 10, ItemUseStyleID.Shoot, ModContent.ProjectileType<RectangleBullet>(), 100f, true, AmmoID.Bullet);
			Item.value = Item.buyPrice(gold: 50);
			Item.rare = ItemRarityID.LightRed;
			Item.reuseDelay = 30;
			Item.UseSound = SoundID.Item38;
			if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
				weapon.SpreadAmount = 0;
				weapon.OffSetPost = 40;
			}
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (modplayer.RectangleShotgun_QuadBarrelShotgun)
				tooltips.Add(new TooltipLine(Mod, "RectangleShotgun_QuadBarrelShotgun", $"[i:{ItemID.QuadBarrelShotgun}] You shoot out burst of rectangle bullets"));
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.HasItem(ItemID.QuadBarrelShotgun)) {
				modplayer.RectangleShotgun_QuadBarrelShotgun = true;
				modplayer.SynergyBonus++;
			}
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ModContent.ProjectileType<RectangleBullet>();
			velocity *= .1f;
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			CanShootItem = true;
			if (modplayer.RectangleShotgun_QuadBarrelShotgun) {
				for (int i = 0; i < Main.rand.Next(3, 5); i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(40), type, damage, knockback, player.whoAmI);
				}
				CanShootItem = false;
			}
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-19, 0);
		}

		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.Boomstick, 2)
			.Register();
		}
	}
	class RectangleBullet : ModProjectile {
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.width = 70;
			Projectile.height = 18;
			Projectile.light = 0.7f;
			Projectile.timeLeft = 400;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
		}
		public override void AI() {
			if (Projectile.ai[0] == 0) {
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.ai[0]++;
			}
			Projectile.velocity *= .97f;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 Direction = Projectile.rotation.ToRotationVector2() * 35;
			Vector2 Head = Projectile.Center + Direction;
			Vector2 End = Projectile.Center - Direction;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Head, End, 22, ref point);
		}
	}
}
