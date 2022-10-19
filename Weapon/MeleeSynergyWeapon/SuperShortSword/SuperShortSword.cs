using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Weapon.MeleeSynergyWeapon.SuperShortSword
{
    class SuperShortSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Super meaning better right ?");
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<SuperShortSwordP>();

            Item.useStyle = 5;
            Item.rare = 2;

            Item.height = 68;
            Item.width = 68;


            Item.useTime = 8;
            Item.useAnimation = 14;

            Item.damage = 112;
            Item.knockBack = 4f;
            Item.shootSpeed = 2.4f;
            Item.value = Item.buyPrice(platinum: 5);
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override void HoldItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SuperShortSwordPower>(), 2);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SpeCopper>()] < 1)
            {
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<SuperShortSword>()), 0), player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpeCopper>(), (int)(Item.damage * 0.25f), 0, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<SuperShortSword>()), 0), player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpeTin>(), (int)(Item.damage * 0.25f), 0, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<SuperShortSword>()), 0), player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpeIron>(), (int)(Item.damage * 0.25f), 0, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<SuperShortSword>()), 0), player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpeLead>(), (int)(Item.damage * 0.25f), 0, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<SuperShortSword>()), 0), player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpeSilver>(), (int)(Item.damage * 0.25f), 0, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<SuperShortSword>()), 0), player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpeTungsten>(), (int)(Item.damage * 0.25f), 0, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<SuperShortSword>()), 0), player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpeGold>(), (int)(Item.damage * 0.25f), 0, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<SuperShortSword>()), 0), player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpePlatinum>(), (int)(Item.damage * 0.25f), 0, player.whoAmI);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SynergyEnergy>())
            .AddIngredient(ItemID.CopperShortsword, 1)
            .AddIngredient(ItemID.TinShortsword, 1)
            .AddIngredient(ItemID.IronShortsword, 1)
            .AddIngredient(ItemID.LeadShortsword, 1)
            .AddIngredient(ItemID.SilverShortsword, 1)
            .AddIngredient(ItemID.TungstenShortsword, 1)
            .AddIngredient(ItemID.GoldShortsword, 1)
            .AddIngredient(ItemID.PlatinumShortsword, 1)
            .Register();
        }
    }
}
