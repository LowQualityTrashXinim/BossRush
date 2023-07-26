using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.OrbOfEnergy
{
    internal class OrbOfEnergy : SynergyModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(1, 1, 100, 10, 5, 5, ItemUseStyleID.HoldUp, ModContent.ProjectileType<OrbOfEnergyBolt>(), 5, 20, true);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = position.PositionOFFSET(velocity, 20);
            position.Y -= 20;
            velocity = velocity.NextVector2RotatedByRandom(10);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //for (int i = 0; i < 3; i++)
            //{
            //    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            //}
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
    class OrbOfEnergyBolt : SynergyModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hide = true;
            Projectile.extraUpdates = 10;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, Main.rand.Next(new int[] { DustID.Electric, DustID.GemSapphire }));
            Main.dust[dust].velocity = Vector2.Zero;
            if (Projectile.timeLeft % 10 == 0)
            {
                Projectile.velocity = Projectile.velocity.NextVector2RotatedByRandom(90);
                Projectile.damage += 5;
            }
        }
    }
}