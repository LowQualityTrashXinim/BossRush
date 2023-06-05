using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;
using BossRush.Contents.Items.Weapon.MagicSynergyWeapon.AmethystSwotaff;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.TopazSwotaff
{
    internal class TopazSwotaff : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(60, 58, 20, 3f, 20, 20, ItemUseStyleID.Swing, ModContent.ProjectileType<TopazSwotaffP>(), 7, 10, false);
            Item.crit = 10;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.noUseGraphic = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<TopazSwotaffP>()] < 1;
        }
        public override void OnConsumeMana(Player player, int manaConsumed)
        {
            if (player.altFunctionUse == 2)
            {
                player.statMana += manaConsumed;
            }
        }
        public override void OnMissingMana(Player player, int neededMana)
        {
            if (player.statMana <= player.GetManaCost(Item))
            {
                CanShootProjectile = -1;
            }
            player.statMana += neededMana;
        }
        public override bool AltFunctionUse(Player player)
        {
            CanShootProjectile = -1;
            return true;
        }
        int CanShootProjectile = 1;
        int countIndex = 1;
        int time = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.statMana >= player.GetManaCost(Item) && player.altFunctionUse != 2)
            {
                CanShootProjectile = 1;
            }
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, countIndex, CanShootProjectile);
            if (CanShootProjectile == 1)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.TopazBolt, damage, knockback, player.whoAmI);
            }
            if (player.altFunctionUse != 2)
            {
                SwingComboHandle();
            }
            return false;
        }
        private void SwingComboHandle()
        {
            countIndex = countIndex != 0 ? countIndex * -1 : 1;
            time++;
            if (time >= 3)
            {
                countIndex = 0;
                time = 0;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TinBroadsword)
                .AddIngredient(ItemID.TopazStaff)
                .Register();
        }
    }
    public class TopazSwotaffP : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<TopazSwotaff>();
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<TopazGemP>();
        protected override int NormalBoltProjectile() => ProjectileID.TopazBolt;
        protected override int DustType() => DustID.GemTopaz;
    }
}