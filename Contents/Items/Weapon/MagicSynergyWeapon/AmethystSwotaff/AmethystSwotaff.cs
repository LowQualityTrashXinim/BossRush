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
            Item.BossRushDefaultMagic(60, 58, 20, 3f, 4, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<AmethystSwotaffP>(), 7, 10, false);
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
        public override void OnMissingMana(Player player, int neededMana)
        {
            if (player.statMana <= player.GetManaCost(Item))
            {
                CanShootProjectile = false;
            }
            player.statMana += neededMana;
        }
        bool CanShootProjectile = true;
        int countIndex = 1;
        int time = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, countIndex);
            if(CanShootProjectile)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.AmethystBolt, damage, knockback, player.whoAmI);
            }
            if (countIndex == 1)
            {
                countIndex = -1;
            }
            else
            {
                countIndex = 1;
            }
            time++;
            if (time >= 3)
            {
                countIndex = 2;
                time = 0;
            }
            return false;
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
            Projectile.DamageType = DamageClass.Melee;
        }
        Vector2 DirectionToSwing;
        int FirstFrame = 0;
        public override void AI()
        {
            if (Projectile.ai[0] != 1 || Projectile.ai[0] != -1)
            {
                return;
            }
            BossRushUtils.ProjectileSwordSwingAI(Projectile, ref DirectionToSwing, ref FirstFrame, (int)Projectile.ai[0]);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 6;
        }
    }
}
