using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MagicSynergyWeapon
{
    internal class AmethystSwotaff : ModItem,ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You know, if this is all what it do\nthen it would be pretty disappointing\nluckily, you can yeet the thing");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 58;

            Item.damage = 20;
            Item.crit = 10;
            Item.knockBack = 3f;

            Item.useTime = 4;
            Item.useAnimation = 40;
            Item.reuseDelay = 20;

            Item.shootSpeed = 7;
            Item.mana = 30;

            Item.value = Item.buyPrice(gold: 50);
            Item.shoot = ProjectileID.AmethystBolt;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.rare = 2;

            Item.UseSound = SoundID.Item8;
        }

        int i = 0;
        int countChange = 0;

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
            {
                mult = 2.5f;
            }
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<AmethystSwotaffP>()] < 1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Item.noUseGraphic = false;
                i++;
                float rotation = MathHelper.ToRadians(30);
                if (countChange == 0)
                {
                    velocity = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)(i / 9f)));
                }
                else
                {
                    velocity = velocity.RotatedBy(MathHelper.Lerp(rotation, -rotation, (float)(i / 9f)));
                }
                if (i > 9)
                {
                    i = 0;
                    countChange++;
                    if (countChange > 1)
                    {
                        countChange = 0;
                    }
                }
                Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ItemAnimationJustStarted)
                {
                    Item.noUseGraphic = true;
                    Projectile.NewProjectile(source, position, velocity * 4, ModContent.ProjectileType<AmethystSwotaffP>(), damage, knockback, player.whoAmI);
                }
                return false;
            }
            else
            {
                return true;
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.CopperBroadsword)
                .AddIngredient(ItemID.AmethystStaff)
                .Register();
        }
    }
}
