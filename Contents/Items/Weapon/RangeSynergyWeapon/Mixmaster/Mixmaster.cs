using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Mixmaster {
	public class Mixmaster : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(130, 34, 35, 1f, 5, 5, ItemUseStyleID.Shoot, ModContent.ProjectileType<PlasmaProjectile>(), 10, true, AmmoID.Bullet);
			Item.value = 10000;
			Item.UseSound = SoundID.Item12 with { Pitch = 1 };
			Item.scale = .75f;
			Item.ArmorPenetration = 30;
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			position = position.PositionOFFSET(velocity, 50);
			for (int i = 0; i < 2; i++) {
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(3));
				float scale = 1f - Main.rand.NextFloat() * .10f;
				perturbedSpeed = perturbedSpeed * scale;
				Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<PlasmaProjectile>(), damage, knockback, player.whoAmI);
			}
			CanShootItem = false;

		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-30, 2);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Megashark)
				.AddIngredient(ItemID.LaserRifle)
				.Register();
		}
	}
	public class PlasmaProjectile : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.light = 0.5f;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 3;
			Projectile.scale = .5f;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0, 0, ModContent.ProjectileType<OnHitRangeP>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * .5f;
			Vector2 drawPos = Projectile.Center.IgnoreTilePositionOFFSET(Projectile.velocity, -10) - Main.screenPosition;
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
			return false;
		}
	}
	public class OnHitRangeP : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 10;
			Projectile.aiStyle = 1;
			Projectile.tileCollide = false;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (Projectile.timeLeft > 0) {
				Projectile.scale -= 0.25f;
			}
		}
	}
}
