using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.SuperShortSword
{
    class SuperShortSword : ModItem, ISynergyItem
    {
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

            Item.useTime = 36;
            Item.useAnimation = 36;

            Item.damage = 93;
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
            player.AddBuff(ModContent.BuffType<SuperShortSwordPower>(), 2);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SuperShortSwordOrbitShortSword>()] < 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    Projectile.NewProjectile(
                        player.GetSource_FromThis(),
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<SuperShortSwordOrbitShortSword>(),
                        (int)(Item.damage * 0.25f),
                        0,
                        player.whoAmI,
                        i, i);
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CopperShortsword)
            .AddIngredient(ItemID.TinShortsword)
            .AddIngredient(ItemID.IronShortsword)
            .AddIngredient(ItemID.LeadShortsword)
            .AddIngredient(ItemID.SilverShortsword)
            .AddIngredient(ItemID.TungstenShortsword)
            .AddIngredient(ItemID.GoldShortsword)
            .AddIngredient(ItemID.PlatinumShortsword)
            .Register();
        }
    }
}
