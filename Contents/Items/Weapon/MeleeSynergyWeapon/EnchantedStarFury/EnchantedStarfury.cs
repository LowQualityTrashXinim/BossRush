using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Global;
using System.Collections.Generic;
using System;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnchantedStarFury
{
    internal class EnchantedStarfury : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(66, 66, 24, 4f, 60, 20, BossRushUseStyle.GenericSwingDownImprove, true);
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ProjectileID.EnchantedBeam;
            Item.shootSpeed = 20f;

            Item.rare = 4;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item1;
        }
        int switchProj = 0;
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            if (modplayer.EnchantedStarfury_SkyFacture)
            {
                tooltips.Add(new TooltipLine(Mod, "EnchantedStarfury_SkyFacture", $"[i:{ItemID.SkyFracture}] Shower down StarFury regardless of attack and with additional sky facture"));
            }
            if (modplayer.EnchantedStarfury_BreakerBlade)
            {
                tooltips.Add(new TooltipLine(Mod, "EnchantedStarfury_BreakerBlade", $"[i:{ItemID.BreakerBlade}] On swing, swing out a living breaker blade that deal 250% of your weapon damage"));
            }
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.SkyFracture))
            {
                modplayer.EnchantedStarfury_SkyFacture = true;
                modplayer.SynergyBonus++;
            }
            if (player.HasItem(ItemID.BreakerBlade))
            {
                modplayer.EnchantedStarfury_BreakerBlade = true;
                modplayer.SynergyBonus++;
            }
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            for (int i = 0; i < 5; i++)
            {
                if (switchProj % 2 == 1 || modplayer.EnchantedStarfury_SkyFacture)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        Vector2 customPos = new Vector2(position.X + Main.rand.Next(-100, 100), position.Y - 900 + Main.rand.Next(-200, 200));
                        Vector2 aimSpread = Main.MouseWorld + Main.rand.NextVector2Circular(200f, 200f);
                        Vector2 velocityTo = (aimSpread - customPos).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                        int typeChoose = l == 0 ? ProjectileID.StarCannonStar : ProjectileID.SkyFracture;
                        Projectile.NewProjectile(source, customPos, velocityTo, typeChoose, damage * 3, knockback, player.whoAmI, i);
                        if (!modplayer.EnchantedStarfury_SkyFacture)
                        {
                            break;
                        }
                    }
                }
                if (switchProj % 2 == 0)
                {
                    Vector2 rotate = velocity.Vector2DistributeEvenly(5, player.direction == 1 ? -60 : 60, i);
                    Projectile.NewProjectile(source, position, rotate * .5f, ModContent.ProjectileType<EnchantedSwordProjectile>(), damage, knockback, player.whoAmI, i);
                }
            }
            switchProj++;
            if (modplayer.EnchantedStarfury_BreakerBlade)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LivingBreakerBladeProjectile>(), (int    )(damage * 2.5f), knockback, player.whoAmI);
            }
            CanShootItem = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.Starfury)
                .Register();
        }
    }
    class EnchantedSwordProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.EnchantedBeam);
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.penetrate = 1;
            Projectile.light = 1f;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        int timer = 0;
        Vector2 localOriginalvelocity;
        public override void AI()
        {
            int dustPar = Main.rand.Next(new int[] { DustID.TintableDustLighted, DustID.YellowStarDust, 57, 58 });
            int dust = Dust.NewDust(Projectile.Center, 0, 0, dustPar, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.1f));
            Main.dust[dust].noGravity = true;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (timer == 0)
            {
                localOriginalvelocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            }
            if (timer <= 20 + Projectile.ai[0] * 2)
            {
                Projectile.timeLeft = 200;
                Projectile.velocity -= Projectile.velocity * .1f;
                timer++;
            }
            else
            {
                if (!Projectile.velocity.IsLimitReached(20)) Projectile.velocity += localOriginalvelocity;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                int dustPar = Main.rand.Next(new int[] { DustID.TintableDustLighted, DustID.YellowStarDust, 57, 58 });
                int dust = Dust.NewDust(Projectile.Center, 0, 0, dustPar, 0, 0, 0, default, Main.rand.NextFloat(1, 1.5f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(10f, 10f);
            }
        }
    }
    class LivingBreakerBladeProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BreakerBlade);
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 92;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 999;
            Projectile.DamageType = DamageClass.Melee;
        }
        int firstframe = 0;
        float MaxLengthX = 0;
        float MaxLengthY = 0;
        float MouseXPosDirection;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (firstframe == 0)
            {
                MouseXPosDirection = Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1;
                MaxLengthX = (Main.MouseWorld - player.Center).Length();
                MaxLengthY = MaxLengthX * .3f * -MouseXPosDirection;
                firstframe++;
            }
            int duration = player.itemAnimationMax;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            float timeleftcountbackward = duration - Projectile.timeLeft;
            float HalfDuration = duration * 0.5f;
            float progressX = BossRushUtils.InOutSine(timeleftcountbackward / duration);
            float progressY;
            if (Projectile.timeLeft < HalfDuration)
            {
                progressY = Projectile.timeLeft / HalfDuration;
            }
            else
            {
                progressY = timeleftcountbackward / HalfDuration;
            }
            progressY = BossRushUtils.InOutSine(progressY);
            float X = MathHelper.Lerp(-20, MaxLengthX + 40, progressX);
            float Y = MathHelper.Lerp(0, MaxLengthY, progressY);
            Vector2 VelocityPosition = new Vector2(X, Y).RotatedBy(Projectile.velocity.ToRotation());
            Projectile.Center = player.MountedCenter + VelocityPosition;
            Projectile.spriteDirection = (int)MouseXPosDirection;
            float value;
            if (MouseXPosDirection == 1)
            {
                value = MathHelper.Lerp(-135, 30, progressX) + 10;
            }
            else
            {
                value = MathHelper.Lerp(30, -135, progressX) - 90;
            }
            Projectile.rotation = MathHelper.ToRadians(value) + Projectile.velocity.ToRotation();
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            npc.immune[player.whoAmI] = 6;
        }
        public override void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.life > (int)(npc.lifeMax * .9f))
            {
                modifiers.SourceDamage.Base += 2.5f * Projectile.damage;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 60; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(6f,6f);
                Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 2.5f);
                Main.dust[dust].rotation = MathHelper.ToRadians(20f);
            }
        }
    }
}