using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.DarkCactus
{
    internal class DarkCactus : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("uhhhh, this make no sense at all");
        }
        public override void SetDefaults()
        {
            Item.BossRushSetDefaultMelee(58, 78, 29, 5f, 60, 20, ItemUseStyleID.Swing, true);

            Item.shoot = ModContent.ProjectileType<CactusBall>();
            Item.shootSpeed = 15;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 getPos2 = new Vector2(40 * player.direction + Main.rand.Next(-50, 50), -700) + player.Center;
                Vector2 aimto2 = new Vector2(player.Center.X + 60 * player.direction, player.Center.Y) - getPos2;
                Vector2 safeAim = aimto2.SafeNormalize(Vector2.UnitX) * 10f;
                Projectile.NewProjectile(Item.GetSource_FromThis(), getPos2, safeAim, ProjectileID.Bat, (int)(damage * 0.75), knockBack, player.whoAmI);
            }
            // prevent heal from applying when damaging critters or target dummy
            if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy)
            {
                int healAmount = Main.rand.Next(1, 7);

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
                .AddIngredient(ItemID.CactusSword)
                .AddIngredient(ItemID.BatBat)
                .Register();
        }
    }
}
