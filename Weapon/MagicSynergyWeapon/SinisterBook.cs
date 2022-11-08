using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Weapon.MagicSynergyWeapon
{
    internal class SinisterBook : ModItem
    {
        public override string Texture => "BossRush/MissingTexture";
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;

            Item.damage = 26;
            Item.knockBack = 1f;
            Item.mana = 7;

            Item.useTime = 3;
            Item.useAnimation = 3;

            Item.shoot = ModContent.ProjectileType<SinisterBolt>();
            Item.shootSpeed = 2;

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(platinum: 5);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            velocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextBool(2) ? Main.rand.Next(70, 90) : -Main.rand.Next(70, 90)));
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SinisterBolt>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            base.AddRecipes();
        }
    }
}
