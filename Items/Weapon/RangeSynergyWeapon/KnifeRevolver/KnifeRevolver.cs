using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.KnifeRevolver
{
    internal class KnifeRevolver : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Self defense at it finest");
        }
        public override void SetDefaults()
        {
            Item.width = 84;
            Item.height = 24;

            Item.damage = 35;
            Item.crit = 10;
            Item.knockBack = 3f;
            Item.scale = 0.85f;

            Item.useTime = 10;
            Item.useAnimation = 10;

            Item.shootSpeed = 10;

            Item.rare = 4;
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.value = Item.buyPrice(gold: 50);
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.UseSound = SoundID.Item40;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 4);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.altFunctionUse != 2;
        }

        public override bool? UseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<KnifeRevolverSpearProjectile>()] < 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Main.mouseRight)
            {
                type = ModContent.ProjectileType<KnifeRevolverSpearProjectile>();
                Item.noUseGraphic = true;
            }
            else
            {
                type = ProjectileID.Bullet;
                Item.noUseGraphic = false;
            }
            player.GetArmorPenetration(DamageClass.Ranged) += 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Revolver)
                .AddIngredient(ItemID.Gladius)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
