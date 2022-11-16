using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Items.Weapon.RangeSynergyWeapon.HeartPistol;
using BossRush.Items.Weapon.RangeSynergyWeapon.NatureSelection;

namespace BossRush.Items.Accessories
{
    class NatureCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A Mana and a Life Crystal, not much to say tbh\nIncrease health and mana by 30");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 28;
            Item.rare = 2;
            Item.value = 1000000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 30;
            player.statManaMax2 += 30;
            if (player.HeldItem.type == ModContent.ItemType<NatureSelection>()) player.GetModPlayer<NatureSelectionPlayer>().PowerUP = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            if (player.GetModPlayer<NatureSelectionPlayer>().PowerUP)
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ModContent.ItemType<NatureSelection>()}] Nature Selection will also shoot out star and heart"));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.LifeCrystal, 1)
           .AddIngredient(ItemID.ManaCrystal, 1)
           .Register();
        }
    }
    class NatureSelectionPlayer : ModPlayer
    {
        public bool PowerUP;

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (PowerUP)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 RandomPos = position + Main.rand.NextVector2Circular(100f, 100f);
                    Vector2 Aimto = (Main.MouseWorld - RandomPos).SafeNormalize(Vector2.UnitX) * 15;
                    if (i == 1)
                    {
                        Projectile.NewProjectile(source, RandomPos, Aimto, ModContent.ProjectileType<HeartP>(), damage, knockback, Player.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectile(source, RandomPos, Aimto, ProjectileID.StarCannonStar, damage, knockback, Player.whoAmI);
                    }
                }
            }
            return true;
        }
        public override void ResetEffects()
        {
            PowerUP = false;
        }
    }
}
