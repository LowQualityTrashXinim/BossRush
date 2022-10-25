using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BossRush.Accessories
{
    internal class GuideToMasterNinja2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guide To Master Ninja 2");
            Tooltip.SetDefault("The final chapeter and possibly the prequel, they just really add in another I into the book" +
                "\nGain jump speed and jump height by 150%" +
                "\nMelee attack speed increase by 10%");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 32;
            Item.rare = 3;
            Item.value = 10000000;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player  = Main.player[Main.myPlayer];
            if (player.GetModPlayer<WeebMasterPlayer2>().GuidetoMasterNinja && (player.HasItem(ItemID.Shuriken) || player.HasItem(ItemID.ThrowingKnife) || player.HasItem(ItemID.PoisonedKnife)))
            {
                tooltips.Add(new TooltipLine(Mod, "Weeb2", $"[i:{ ItemID.Shuriken }][i:{ ItemID.ThrowingKnife}][i:{ ItemID.PoisonedKnife}] gain 50% damage and have a chance to spawn itself ontop of the enemy"));
                if (player.HasItem(ItemID.BoneDagger))
                {
                    tooltips.Add(new TooltipLine(Mod, "BoneDaggerGTMN2", $"[i:{ItemID.BoneDagger}] Gain 10% damage increase"));
                }
                if (player.HasItem(ItemID.BoneDagger))
                {
                    tooltips.Add(new TooltipLine(Mod, "BoneDaggerGTMN2", $"[i:{ItemID.FrostDaggerfish}] Gain 10% damage increase"));
                }
            }
            if (player.HasItem(ItemID.Katana))
            {
                tooltips.Add(new TooltipLine(Mod,"KatanaPower",$"[i:{ItemID.Katana}] Increase sword size and damage by 50%"));
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WeebMasterPlayer2>().GuidetoMasterNinja = true;
            player.jumpSpeedBoost += 1.5f;
            player.GetAttackSpeed(DamageClass.Melee) += .1f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ThrowingKnife)
                .AddIngredient(ItemID.Gi)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
    public class WeebMasterPlayer2 : ModPlayer
    {
        public bool GuidetoMasterNinja;
        public override void ResetEffects()
        {
            GuidetoMasterNinja = false;
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if(GuidetoMasterNinja && item.type == ItemID.Katana)
            {
                item.Size += new Vector2(50,50);
                item.scale += .5f;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
           if(GuidetoMasterNinja)
            {
                if (item.type == ItemID.Shuriken || item.type == ItemID.ThrowingKnife || item.type == ItemID.PoisonedKnife)
                {
                    damage += .5f;
                    if (Player.HasItem(ItemID.BoneDagger))
                    {
                        damage += .1f;
                    }
                    if (Player.HasItem(ItemID.FrostDaggerfish))
                    {
                        damage += .1f;
                    }
                }
                if(item.type == ItemID.Katana)
                {
                    damage += .5f;
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (GuidetoMasterNinja)
            {
                if ((Player.HasItem(ItemID.Shuriken) || Player.HasItem(ItemID.ThrowingKnife) || Player.HasItem(ItemID.PoisonedKnife)) && Main.rand.NextBool(10))
                {
                    List<int> NinjaBag = new List<int>();
                    int[] RandomThrow = new int[] { ProjectileID.Shuriken, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife };
                    NinjaBag.AddRange(RandomThrow);
                    if (Player.HasItem(ItemID.BoneDagger))
                    {
                        NinjaBag.Add(ItemID.BoneDagger);
                    }
                    if (Player.HasItem(ItemID.FrostDaggerfish))
                    {
                        NinjaBag.Add(ItemID.FrostDaggerfish);
                    }
                    Vector2 SpawnProjPos = target.Center + new Vector2(0, -200);
                    for (int i = 0; i < 12; i++)
                    { 
                        Vector2 randomSpeed = Main.rand.NextVector2Circular(2, 2);
                        Dust.NewDust(SpawnProjPos, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0,default, Main.rand.NextFloat(2f, 3.5f));
                    }
                    Projectile.NewProjectile(Player.GetSource_FromThis(), SpawnProjPos, Vector2.Zero,Main.rand.NextFromCollection(NinjaBag) , damage, knockback, Player.whoAmI);
                }
            }
        }
    }
}
