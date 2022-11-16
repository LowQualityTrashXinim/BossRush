using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.FrozenSwordFish
{
    internal class FrostSwordFish : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost SwordFish");
            Tooltip.SetDefault("The cursed cold cousin");
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 64;
            Item.rare = 2;

            Item.damage = 27;
            Item.crit = 5;
            Item.knockBack = 1f;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(gold: 50);
            Item.autoReuse = true;
            Item.useTurn = false;

            Item.scale += 0.25f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            Projectile.NewProjectile(new EntitySource_ItemUse(player, Item), player.Center + Main.rand.NextVector2Circular(400f, 400f), Vector2.Zero, ModContent.ProjectileType<FrostDaggerFishP>(), damage, knockBack, player.whoAmI);
            target.AddBuff(BuffID.Frostburn, 180);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ItemID.FrostDaggerfish, 100)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
