using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace BossRush.Accessories
{
    internal class GuideToMasterNinja : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("To Master the ninja technique, you must master the way of the weeb first\nshuriken will be thrown when you shoot a enemy 20 times\nIncrease movement speed by 150%\nIncrease crits chance by 5%");
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
            if (player.GetModPlayer<WeebMasterPlayer2>().GuidetoMasterNinja)
            {
                tooltips.Add(new TooltipLine(Mod, "FinalMaster", $"[i:{ModContent.ItemType<GuideToMasterNinja2>()}] You summon a ring of shuriken and knife"));
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WeebMasterPlayer>().GuidetoMasterNinja = true;
            player.moveSpeed *= 1.5f;
            player.GetCritChance(DamageClass.Generic) += 5;
            if(player.head == 22 && player.body == 14 && player.legs == 14)
            {
                player.GetAttackSpeed(DamageClass.Melee) += .15f;
                player.GetDamage(DamageClass.Melee) += .25f;
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
    public class ThrowingKnifeCustom : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_"+ItemID.ThrowingKnife;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public void Behavior(Player player, float offSet, int Counter, float Distance = 100)
        {
            Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
            Vector2 NewCenter = player.Center + Rotate.RotatedBy(Counter * 0.1f) * Distance;
            Projectile.Center = NewCenter;
            if (Counter == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 randomSpeed = Main.rand.NextVector2Circular(1, 1);
                    Dust.NewDust(NewCenter, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0, default, Main.rand.NextFloat(2f, 2.5f));
                }
            }
        }
        int Counter = 0;
        int Multiplier = 1;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.timeLeft = 2;
            if (player.dead || !player.active)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0]++;
                switch (Projectile.velocity.X)
                {
                    case 1:
                        Multiplier = 1;
                        break;
                    case 2:
                        Multiplier = 2;
                        break;
                    case 3:
                        Multiplier = 3;
                        break;
                    case 4:
                        Multiplier = 4;
                        break;
                    case 5:
                        Multiplier = 5;
                        break;
                    case 6:
                        Multiplier = 6;
                        break;
                    case 7:
                        Multiplier = 7;
                        break;
                    case 8:
                        Multiplier = 8;
                        break;
                }
                Projectile.velocity = Vector2.Zero;
            }
            Behavior(player,45 * Multiplier, Counter);
            if (Counter == -MathHelper.TwoPi * 100 - 1) { Counter = -1; }
            Counter--;
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
        int TimerForUltimate = 0;
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
        public override void PostUpdate()
        {
            if (Player.GetModPlayer<WeebMasterPlayer2>().GuidetoMasterNinja)
            {
                if (TimerForUltimate >= 40)
                {
                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<ThrowingKnifeCustom>()] < 1)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 1 + i), ModContent.ProjectileType<ThrowingKnifeCustom>(), 30, 2f, Player.whoAmI);
                        }
                    }
                    else
                    {
                        TimerForUltimate = 0;
                    }
                }
                else 
                {
                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<ThrowingKnifeCustom>()] > 0)
                    {
                        TimerForUltimate = 0;
                    }
                    TimerForUltimate++;
                }
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
