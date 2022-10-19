using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.RangeSynergyWeapon.KnifeRevolver
{
    internal class KnifeRevolverSpearProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 2;
		}
        protected virtual float HoldoutRangeMin => 30f;
		protected virtual float HoldoutRangeMax => 46f;

		public float CollisionWidth => 10f * Projectile.scale;
		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(30);
			Projectile.scale = 0.85f;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.hide = true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 50f;
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			SelectFrame(player);
			Projectile.Center = player.Center;
			int duration = player.itemAnimationMax;
			DrawOffsetX = -30;
			player.heldProj = Projectile.whoAmI;

			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}
			
			Projectile.velocity = Vector2.Normalize(Projectile.velocity);
			float halfDuration = duration * 0.5f;
			float progress;
			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;
			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
			Projectile.rotation = Projectile.velocity.ToRotation() + (player.direction != 1 ? MathHelper.Pi : 0);
			return false;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			if(!player.GetModPlayer<KnifeRevolverPlayer>().SpecialShotReady)
            {
				player.GetModPlayer<KnifeRevolverPlayer>().SpecialShotReady = true;

			}
        }
		public void SelectFrame(Player player)
		{
			if (player.direction == 1)
			{
				Projectile.frame = 1;
			}
			else
			{
				Projectile.frame = 0;
			}
		}
	}

	public class KnifeRevolverPlayer : ModPlayer
    {
		public bool SpecialShotReady = false;

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if(SpecialShotReady && Player.HeldItem.type == ModContent.ItemType<KnifeRevolver>() && Player.altFunctionUse != 2)
            {
				type = ModContent.ProjectileType<KnifeRevolverP>();
				damage *= 5;
				SpecialShotReady = false;
            }
        }
    }
}
