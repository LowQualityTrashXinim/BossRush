using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Weapon.RangeSynergyWeapon.NatureSelection
{
    internal class ShadewoodBowP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.timeLeft = 70;
            Projectile.width = 16;
            Projectile.height = 32;
        }

        public override void AI()
        {
            Vector2 AimPos = Main.MouseWorld - Projectile.position;
            Vector2 safeAim = AimPos.SafeNormalize(Vector2.UnitX);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] < 19)
            {
                Projectile.rotation = safeAim.ToRotation();
            }
            if (Projectile.ai[0] == 30)
            {
                Projectile.velocity = safeAim * 15f;
            }
            if (Projectile.ai[0] > 30)
            {
                Projectile.rotation += 0.5f;
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            EntitySource_ItemUse_WithAmmo source = new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<NatureSelection>()), AmmoID.Arrow);
            float numProj = 5 + Main.rand.Next(3);
            float rotation = MathHelper.ToRadians(180 + Main.rand.Next(90));
            for (int i = 0; i < numProj; i++)
            {
                Vector2 Star = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (numProj - 1)));
                Projectile.NewProjectile(source, new Vector2(Projectile.position.X + 8, Projectile.position.Y + 16), Star, ProjectileID.WoodenArrowFriendly, (int)(Projectile.damage * 0.5f), (float)(Projectile.knockBack * 0.5f), player.whoAmI);
            }
        }
    }
}
