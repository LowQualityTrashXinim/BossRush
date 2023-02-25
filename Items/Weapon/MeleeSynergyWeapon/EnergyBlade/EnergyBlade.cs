using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.EnergyBlade
{
    internal class EnergyBlade : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("High energy vibration around the sword" +
                "\nmaking it sharp enough to even slash through the strongest mental like it is nothing");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.height = 62;
            Item.width = 64;

            Item.damage = 25;
            Item.knockBack = 5f;
            Item.useTime = 30;
            Item.useAnimation = 15;

            Item.rare = ItemRarityID.Orange;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = BossRushUseStyle.GenericSwingDownImprove;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }
        //public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        //{
        //    Vector2 direction = Main.MouseWorld.X - player.Center.X > 0 ? Vector2.UnitX : -Vector2.UnitX; 
        //    int proj = Projectile.NewProjectile(
        //        Item.GetSource_ItemUse(Item),
        //        player.itemLocation,
        //        direction,
        //        ModContent.ProjectileType<GhostHitBox>(),
        //        player.GetWeaponDamage(Item),
        //        player.HeldItem.knockBack,
        //        player.whoAmI);
        //    Projectile projectile = Main.projectile[proj];
        //    projectile.DamageType = DamageClass.Melee;
        //    projectile.Hitbox = hitbox;
        //    noHitbox = true;
        //}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EnchantedSword, 2)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
