using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.BloodyShot
{
    internal class BloodyShot : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(42, 36, 25, 1f, 20, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<BloodBullet>(), 5, false, AmmoID.Bullet);
            Item.scale = 0.7f;
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item11;
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            if (modplayer.BloodyShoot_AquaScepter)
                tooltips.Add(new TooltipLine(Mod, "BloodyShoot_AquaScepter", $"{ItemID.AquaScepter} Your gun now shoot out damaging blood"));
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.AquaScepter))
                modplayer.BloodyShoot_AquaScepter = true;

        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            position = position.PositionOFFSET(velocity, 30);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BloodBullet>(), damage, knockback, player.whoAmI);
            if (modplayer.BloodyShoot_AquaScepter)
            {
                for (int i = 0; i < Main.rand.Next(3, 6); i++)
                {
                    Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(20), ModContent.ProjectileType<BloodWater>(), damage, knockback, player.whoAmI);
                }
            }
            CanShootItem = false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, 2);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Handgun)
                .AddIngredient(ItemID.BloodRainBow)
                .Register();
        }
    }
    public class BloodWater : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<BloodBullet>();
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.extraUpdates = 6;
            Projectile.tileCollide = true;
            Projectile.hide = true;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Dust.NewDust(Projectile.Center, 10, 10, DustID.BloodWater);
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 50)
            {
                Projectile.velocity.Y += .1f;
            }
        }
    }
    internal class BloodBullet : SynergyModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            int ran = Main.rand.Next(7);
            if (ran == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 newPos = new Vector2(Projectile.position.X + Main.rand.Next(-500, 500) + 5, Projectile.position.Y - (600 + Main.rand.Next(1, 100)) + 5);
                    Vector2 safeAimto = (Projectile.position - newPos).SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos, safeAimto * 5, ModContent.ProjectileType<BloodBullet>(), hit.Damage, hit.Knockback, player.whoAmI);
                }
            }
            int randNum = 1 + Main.rand.Next(3, 6);
            for (int i = 0; i < randNum; i++)
            {
                Vector2 newPos = new Vector2(Projectile.position.X + Main.rand.Next(-200, 200) + 5, Projectile.position.Y - (600 + Main.rand.Next(1, 200)) + 5);
                Projectile.position.X += Main.rand.Next(-50, 50);
                Vector2 safeAimto = (Projectile.position - newPos).SafeNormalize(Vector2.UnitX);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos, safeAimto * 25, ProjectileID.BloodArrow, (int)(hit.Damage * 0.75f), hit.Knockback, player.whoAmI);
            }
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Blood);
            Main.dust[dust].noGravity = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return true;
        }
    }
}