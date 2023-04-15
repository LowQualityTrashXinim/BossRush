using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.AmethystSwotaff
{
    internal class AmethystSwotaff : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You know, if this is all what it do\nthen it would be pretty disappointing\nluckily, you can yeet the thing");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(60, 58, 20, 3f, 4, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<AmethystSwotaffP>(), 7, 30, false);
            Item.crit = 10;
            Item.reuseDelay = 20;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;
            Item.rare = 2;
            Item.UseSound = SoundID.Item8;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<AmethystSwotaffP>()] < 1;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBroadsword)
                .AddIngredient(ItemID.AmethystStaff)
                .Register();
        }
    }
    public class AmethystSwotaffP : ModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<AmethystSwotaff>();
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
        }
        Vector2 DirectionToSwing;
        int FirstFrame = 0;
        public override void AI()
        {
            BossRushUtils.ProjectileSwordSwingAI(Projectile, ref DirectionToSwing, ref FirstFrame, (int)Projectile.ai[0]);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 6;
        }
    }
}
