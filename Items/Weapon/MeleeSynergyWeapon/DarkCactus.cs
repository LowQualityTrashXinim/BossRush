using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class DarkCactus : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("uhhhh, this make no sense at all");
        }
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 78;

            Item.damage = 27;
            Item.knockBack = 4f;
            Item.useTime = 80;
            Item.useAnimation = 20;

            Item.shoot = ModContent.ProjectileType<CactusBall>();
            Item.shootSpeed = 15;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = false;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            EntitySource_ItemUse source = new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<DarkCactus>()), 0);

            if (player.direction == 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 getPos1 = new Vector2(40 + Main.rand.Next(-50, 50), -700) + player.Center;
                    Vector2 aimto1 = new Vector2(player.Center.X + 60, player.Center.Y) - getPos1;
                    Vector2 safeAim = aimto1.SafeNormalize(Vector2.UnitX) * 10f;
                    Projectile.NewProjectile(source, getPos1, safeAim, ProjectileID.Bat, (int)(damage * 0.75), knockBack, player.whoAmI);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 getPos2 = new Vector2(-40 + Main.rand.Next(-50, 50), -700) + player.Center;
                    Vector2 aimto2 = new Vector2(player.Center.X - 60, player.Center.Y) - getPos2;
                    Vector2 safeAim = aimto2.SafeNormalize(Vector2.UnitX) * 10f;
                    Projectile.NewProjectile(source, getPos2, safeAim, ProjectileID.Bat, (int)(damage * 0.75), knockBack, player.whoAmI);
                }
            }

            // prevent heal from applying when damaging critters or target dummy
            if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy)
            {
                int healAmount = 2;

                player.statLife += healAmount;
                // this part here prevents health from going above max
                if (player.statLife > player.statLifeMax2)
                {
                    player.statLife = player.statLifeMax2;
                }

                // the heal popup text
                player.HealEffect(healAmount, true);

            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.CactusSword)
                .AddIngredient(ItemID.BatBat)
                .Register();
        }
    }
}
