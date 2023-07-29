using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.GuideToMasterNinja
{
    internal class GuideToMasterNinja : SynergyModItem
    {
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
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<PlayerNinjaBook>().NinjaWeeb)
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.NinjaHood}][i:{ItemID.NinjaShirt}][i:{ItemID.NinjaPants}]Increase thrown damage by 20%, Melee attack is faster by 15% and increase melee damage by 25%"));
            }
            if (player.HasItem(ItemID.ThrowingKnife) && player.HasItem(ItemID.PoisonedKnife) && player.HasItem(ItemID.FrostDaggerfish) && player.HasItem(ItemID.BoneDagger))
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.ThrowingKnife}][i:{ItemID.PoisonedKnife}][i:{ItemID.FrostDaggerfish}][i:{ItemID.BoneDagger}] You can throw additional knife/dagger/shuriken faster"));
            }
            if (player.HasItem(ItemID.ThrowingKnife) && player.HasItem(ItemID.PoisonedKnife))
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.ThrowingKnife}][i:{ItemID.PoisonedKnife}] You will sometime throw 1 of 2 knife, fixed damage +10"));
            }
            else if (player.HasItem(ItemID.ThrowingKnife) || player.HasItem(ItemID.PoisonedKnife))
            {
                if (player.HasItem(ItemID.ThrowingKnife))
                {
                    tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.ThrowingKnife}] You will sometime throw throwing knife, fixed damage +5"));
                }
                else
                {
                    tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.PoisonedKnife}] You will sometime throw poisoned throwing knife, fixed damage +5"));
                }
            }
            if (player.HasItem(ItemID.FrostDaggerfish))
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.FrostDaggerfish}] Attack now inflict FrostBurn and you sometime throw FrostDaggerFish, fixed damage +5"));
            }
            if (player.HasItem(ItemID.BoneDagger))
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.BoneDagger}] Attack now inflict OnFire! and you sometime throw BoneDagger , fixed damage +5"));
            }
            if (player.GetModPlayer<PlayerNinjaBook>().GuidetoMasterNinja)
            {
                tooltips.Add(new TooltipLine(Mod, "FinalMaster", $"[i:{ModContent.ItemType<GuideToMasterNinja2>()}] You summon a ring of shuriken and knife"));
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlayerNinjaBook>().GuidetoMasterNinja = true;
            player.moveSpeed += .15f;
            player.GetCritChance(DamageClass.Generic) += 5;
            if (player.head == 22 && player.body == 14 && player.legs == 14)
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
                .Register();
        }
    }
    public class ThrowingKnifeCustom : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.ThrowingKnife);
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public void Behavior(Player player, float offSet, int Counter, float Distance = 125)
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
            if (Projectile.ai[0] >= 60)
            {
                Projectile.penetrate = 1;
            }
            Projectile.ai[0]++;
            Behavior(player, 45 * Multiplier, Counter);
            if (Counter == -MathHelper.TwoPi * 100 - 1) { Counter = -1; }
            Counter--;
        }
    }
}
