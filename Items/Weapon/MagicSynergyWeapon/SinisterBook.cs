using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MagicSynergyWeapon
{
    internal class SinisterBook : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;

            Item.damage = 43;
            Item.knockBack = 1f;
            Item.mana = 7;

            Item.useTime = 9;
            Item.useAnimation = 9;

            Item.shoot = ModContent.ProjectileType<SinisterBolt>();
            Item.shootSpeed = 2.5f;

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(platinum: 5);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;

            Item.UseSound = SoundID.Item8;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = position.PositionOFFSET(velocity, 30);
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                velocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextBool(2) ? Main.rand.Next(40, 90) : -Main.rand.Next(40, 90)));
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SinisterBolt>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BookofSkulls)
                .AddIngredient(ItemID.WaterBolt)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
