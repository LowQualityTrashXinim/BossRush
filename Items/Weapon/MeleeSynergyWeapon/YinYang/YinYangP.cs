using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.YinYang
{
    internal class YinYangP : ModProjectile
    {
        int charge = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 25f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 99;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 24;
            Projectile.height = 24;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            EntitySource_ItemUse entity = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<YinYang>()));
            charge++;
            if (charge == 20 && player.yoyoGlove == false)
            {
                Projectile.NewProjectile(entity, Projectile.position, Vector2.Zero, ModContent.ProjectileType<YinYangShockWave>(), 0, 0, player.whoAmI);
                Projectile.damage = (int)(Projectile.damage * 3.5f);
                ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 24;
                ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 600f;
            }
            if (charge > 20 || player.yoyoGlove == true)
            {
                Vector2 RotatePos = Main.rand.NextVector2CircularEdge(75f, 75f) * 5 + Projectile.position;
                Vector2 RotatePos2 = Main.rand.NextVector2CircularEdge(75f, 75f) * 5 + Projectile.position;
                Vector2 Aimto = (target.Center - RotatePos).SafeNormalize(Vector2.UnitX) * 3;
                Vector2 Aimto2 = (target.Center - RotatePos2).SafeNormalize(Vector2.UnitX) * 3;
                Projectile.NewProjectile(entity, RotatePos, Aimto, ModContent.ProjectileType<YinLight>(), (int)(Projectile.damage * 0.75f), 2f, player.whoAmI);
                Projectile.NewProjectile(entity, RotatePos2, Aimto2, ModContent.ProjectileType<YangDark>(), (int)(Projectile.damage * 0.75f), 2f, player.whoAmI);
            }
        }

        public override void PostAI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<YinYangP>()] < 1)
            {
                ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400f;
                ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12;
            }
        }
    }
}
