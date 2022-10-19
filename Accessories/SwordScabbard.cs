using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace BossRush.Accessories
{
    internal class SwordScabbard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Made for people who complain their sword isn't special" +
                "\nIncrease melee speed by 5%" +
                "\nRlease a sword slash upon swing");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 42;
            Item.width = 66;
            Item.rare = 3;
            Item.value = 10000000;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<SwordPlayer>().SwordSlash = true;
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood Sword")
                .AddRecipeGroup("OreBroadSword")
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }

    public class SwordPlayer : ModPlayer
    {
        public bool SwordSlash;
        public override void ResetEffects()
        {
            SwordSlash = false;
        }
        public override void PostUpdate()
        {
            if (Player.HeldItem.DamageType == DamageClass.Melee && Player.HeldItem.useStyle == ItemUseStyleID.Swing && SwordSlash && Main.mouseLeft && Player.ItemAnimationJustStarted)
            {
                Vector2 DistanceFromProjToAim = Main.MouseWorld - Player.Center;
                Vector2 DirectionFromProjToAim = DistanceFromProjToAim.SafeNormalize(Vector2.UnitX);
                Vector2 speed = DirectionFromProjToAim * 12f;
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, speed, ModContent.ProjectileType<SwordSlash>(), 40, 2f, Player.whoAmI);
            }
        }
    }

    public class SwordSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 50;
        }
        public override void AI()
        {
            Projectile.alpha += 255/50;
            Projectile.scale -= 0.02f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
