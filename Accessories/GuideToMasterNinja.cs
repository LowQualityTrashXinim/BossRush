using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Accessories
{
    internal class GuideToMasterNinja : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("To Master the ninja technique, you must master the way of the weeb first\nshuriken will be thrown when you shoot a enemy 20 times\nIncrease movement speed by 25%\nIncrease crits chance by 5%");
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
            if(player.GetModPlayer<WeebMasterPlayer>().NinjaWeeb)
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.NinjaHood}][i:{ItemID.NinjaShirt}][i:{ItemID.NinjaPants}]Increase thrown damage by 20%, Melee attack is faster by 15% and increase melee damage by 25%"));
            }
            if(player.HasItem(ItemID.ThrowingKnife) && player.HasItem(ItemID.PoisonedKnife) && player.HasItem(ItemID.FrostDaggerfish) && player.HasItem(ItemID.BoneDagger))
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.ThrowingKnife}][i:{ItemID.PoisonedKnife}][i:{ItemID.FrostDaggerfish}][i:{ItemID.BoneDagger}] You can throw knife/dagger/shuriken faster"));
            }
            if (player.HasItem(ItemID.ThrowingKnife) && player.HasItem(ItemID.PoisonedKnife))
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.ThrowingKnife}][i:{ItemID.PoisonedKnife}] the 2 knife is added into bag, +10 damage"));
            }
            else if (player.HasItem(ItemID.ThrowingKnife) || player.HasItem(ItemID.PoisonedKnife))
            {
                if (player.HasItem(ItemID.ThrowingKnife))
                {
                    tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.ThrowingKnife}]the knife is added into bag, +5 damage"));
                }
                else
                {
                    tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.PoisonedKnife}]the knife is added into bag, +5 damage"));
                }
            }
            if(player.HasItem(ItemID.FrostDaggerfish))
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.FrostDaggerfish}] Attack now inflict FrostBurn and FrostDaggerFish is added into bag, +5 damage"));
            }
            if(player.HasItem(ItemID.BoneDagger))
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.BoneDagger}] Attack now inflict OnFire! and BoneDagger is added into bag, +5 damage"));
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WeebMasterPlayer>().GuidetoMasterNinja = true;
            player.moveSpeed *= 1.25f;
            player.GetCritChance(DamageClass.Generic) += 5;
            if(player.head == 22 && player.body == 14 && player.legs == 14)
            {
                player.GetAttackSpeed(DamageClass.Melee) += 1.15f;
                player.GetDamage(DamageClass.Melee) += 1.25f;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Shuriken)
                .AddIngredient(ItemID.NinjaHood)
                .AddIngredient(ItemID.NinjaShirt)
                .AddIngredient(ItemID.NinjaPants)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
    public class WeebMasterPlayer : ModPlayer
    {
        public bool GuidetoMasterNinja;
        public bool NinjaWeeb;
        //counter for accessory
        //GuidetoMasterNinja
        int GTMNcount = 0;
        int GTMNlimitCount = 15;
        public override void ResetEffects()
        {
            GuidetoMasterNinja = false;
            NinjaWeeb = false;
        }
        public override void UpdateEquips()
        {
            if (Player.head == 22 && Player.body == 14 && Player.legs == 14)
            {
                NinjaWeeb = true;
            }
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            List<int> GTMNcontain = new List<int>();

            if (GuidetoMasterNinja)
            {
                //Independant damage
                int StaticDamage = 10;
                if (NinjaWeeb)
                {
                    StaticDamage = (int)(StaticDamage * 1.2f);
                }
                if (Player.HasItem(ItemID.ThrowingKnife) && Player.HasItem(ItemID.PoisonedKnife) && Player.HasItem(ItemID.FrostDaggerfish) && Player.HasItem(ItemID.BoneDagger))
                {
                    GTMNcount++;
                }
                if (Player.HasItem(ItemID.ThrowingKnife))
                {
                    StaticDamage += 5;
                }
                if (Player.HasItem(ItemID.PoisonedKnife))
                {
                    StaticDamage += 5;
                }
                if (Player.HasItem(ItemID.FrostDaggerfish))
                {
                    StaticDamage += 5;
                }
                if (Player.HasItem(ItemID.BoneDagger))
                {
                    StaticDamage += 5;
                }
                GTMNcount++;
                if (GTMNcount >= GTMNlimitCount)
                {
                    Vector2 Aimto = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX);

                    EntitySource_ItemUse entity = new EntitySource_ItemUse(Player, new Item(ModContent.ItemType<GuideToMasterNinja>()));
                    //The actual secret here
                    GTMNcontain.Clear();
                    GTMNcontain.Add(ProjectileID.Shuriken);
                    if (Player.HasItem(ItemID.PoisonedKnife))
                    {
                        GTMNcontain.Add(ProjectileID.PoisonedKnife);
                    }
                    if (Player.HasItem(ItemID.ThrowingKnife))
                    {
                        GTMNcontain.Add(ProjectileID.ThrowingKnife);
                    }
                    if (Player.HasItem(ItemID.FrostDaggerfish))
                    {
                        GTMNcontain.Add(ProjectileID.FrostDaggerfish);
                    }
                    if (Player.HasItem(ItemID.BoneDagger))
                    {
                        GTMNcontain.Add(ProjectileID.BoneDagger);
                    }
                    Projectile.NewProjectile(entity, Player.Center, Aimto * 20, Main.rand.NextFromCollection(GTMNcontain), StaticDamage, 1f, Player.whoAmI);
                    GTMNcount = 0;
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (GuidetoMasterNinja)
            {
                if (Player.HasItem(ItemID.FrostDaggerfish))
                {
                    target.AddBuff(BuffID.Frostburn, 150);
                }
                if (Player.HasItem(ItemID.BoneDagger))
                {
                    target.AddBuff(BuffID.OnFire, 150);
                }
            }
        }
    }
}
