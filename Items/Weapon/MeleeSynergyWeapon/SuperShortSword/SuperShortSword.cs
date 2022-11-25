using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.SuperShortSword
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

            Item.UseSound = SoundID.Item1;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override void HoldItem(Player player)
        {
            int[] ArrayOfWeaponProjectile = new int[] { ModContent.ProjectileType<SpeCopper>(), ModContent.ProjectileType<SpeTin>(), ModContent.ProjectileType<SpeIron>(), ModContent.ProjectileType<SpeLead>(), ModContent.ProjectileType<SpeSilver>(), ModContent.ProjectileType<SpeTungsten>(), ModContent.ProjectileType<SpeGold>(), ModContent.ProjectileType<SpePlatinum>() };
            player.AddBuff(ModContent.BuffType<SuperShortSwordPower>(), 2);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SpeCopper>()] < 1)
            {
                for (int i = 0; i < ArrayOfWeaponProjectile.Length; i++)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(0, 0), ArrayOfWeaponProjectile[i], (int)(Item.damage * 0.25f), 0, player.whoAmI);
                }
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
