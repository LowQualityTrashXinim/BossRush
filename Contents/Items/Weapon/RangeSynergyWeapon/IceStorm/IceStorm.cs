using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.IceStorm
{
    internal class IceStorm : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(42, 98, 50, 1f, 75, 75, ItemUseStyleID.Shoot, ProjectileID.FrostArrow, 10f, true, AmmoID.Arrow);
            Item.crit = 5;
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 50);
            Item.scale = 0.7f;
            Item.UseSound = SoundID.Item5;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (player.HasItem(ItemID.SnowballCannon))
            {
                tooltips.Add(new TooltipLine(Mod, "smth", $"[i:{ItemID.SnowballCannon}] Charge attack up can shoot snowballs and summon itself"));
            }
            if (player.HasItem(ItemID.FlowerofFrost))
            {
                tooltips.Add(new TooltipLine(Mod, "smth", $"[i:{ItemID.FlowerofFrost}] Charge attack up can shoot ball of frost and summon itself"));
            }
            if (player.HasItem(ItemID.BlizzardStaff))
            {
                tooltips.Add(new TooltipLine(Mod, "smth", $"[i:{ItemID.BlizzardStaff}] Max charge can now rain down frost spike"));
            }
        }
        public override void HoldItem(Player player)
        {
            if (player.HasItem(ItemID.SnowballCannon))
            {
                int type = ModContent.ProjectileType<IceStormSnowBallCannonMinion>();
                if (player.ownedProjectileCounts[type] < 1)
                {
                    Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.Zero, type, Item.damage, Item.knockBack, player.whoAmI);
                }
            }
            if (player.HasItem(ItemID.FlowerofFrost))
            {
                int type = ModContent.ProjectileType<IceStormFrostFlowerMinion>();
                if (player.ownedProjectileCounts[type] < 1)
                {
                    Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.Zero, type, Item.damage, Item.knockBack, player.whoAmI);
                }
            }
            if (!Main.mouseLeft && player.GetModPlayer<IceStormPlayer>().SpeedMultiplier >= 1)
            {
                player.GetModPlayer<IceStormPlayer>().SpeedMultiplier -= 0.025f;
            }
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.2f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        public override float UseSpeedMultiplier(Player player)
        {
            return player.GetModPlayer<IceStormPlayer>().SpeedMultiplier;
        }
        int count = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ChargeUpHandle(player);
            IceStormSynergy(player, source, position, velocity, damage, knockback);
            int projectile2 = 1 + (int)(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .2f);
            for (int i = 0; i < projectile2; ++i)
            {
                float ToRa = 0;
                if (projectile2 != 1)
                {
                    ToRa = projectile2 * 3;
                }
                Projectile.NewProjectile(source, position, velocity.NextVector2RotatedByRandom(ToRa) * 2f, ProjectileID.FrostburnArrow, damage, knockback, player.whoAmI);
            }
            int projectile = (int)(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .2f);
            for (int i = 0; i < projectile; i++)
            {
                Projectile.NewProjectile(source, position, velocity.NextVector2RotatedByRandom(5).NextVector2Spread(4, Main.rand.NextFloat(0.5f, 1f)), ProjectileID.IceBolt, damage, knockback, player.whoAmI);
            }
            float projectile3 = player.GetModPlayer<IceStormPlayer>().SpeedMultiplier / 7f;
            if (projectile3 >= 1)
            {
                Projectile.NewProjectile(source, position, velocity * 2f, ProjectileID.FrostArrow, damage, knockback, player.whoAmI);
            }

            if (player.GetModPlayer<IceStormPlayer>().SpeedMultiplier >= 8)
            {
                if (count == 0)
                {
                    DustExplosion(player.Center);
                    count++;
                }
            }
            if (player.GetModPlayer<IceStormPlayer>().SpeedMultiplier <= 7)
            {
                count = 0;
            }
            return false;
        }
        private void ChargeUpHandle(Player player)
        {
            if (Main.mouseLeft && player.GetModPlayer<IceStormPlayer>().SpeedMultiplier <= 8)
            {
                if (player.GetModPlayer<IceStormPlayer>().SpeedMultiplier <= 2)
                {
                    player.GetModPlayer<IceStormPlayer>().SpeedMultiplier += 0.1f;
                }
                if (player.GetModPlayer<IceStormPlayer>().SpeedMultiplier <= 6)
                {
                    player.GetModPlayer<IceStormPlayer>().SpeedMultiplier += 0.1f;
                }
                else
                {
                    player.GetModPlayer<IceStormPlayer>().SpeedMultiplier += 0.02f;
                }
            }
        }
        private void DustExplosion(Vector2 center)
        {
            for (int i = 0; i < 400; i++)
            {
                Vector2 circular = Main.rand.NextVector2CircularEdge(25, 25);
                int dustNum = Dust.NewDust(center, 0, 0, DustID.FrostHydra, 0, 0, 0, default, Main.rand.NextFloat(0.75f, 1.5f));
                Main.dust[dustNum].noGravity = true;
                Main.dust[dustNum].noLight = false;
                Main.dust[dustNum].noLightEmittence = false;
                Main.dust[dustNum].velocity = circular;
            }
        }
        private void IceStormSynergy(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            if (player.HasItem(ItemID.SnowballCannon))
            {
                HasSnowBall(player, source, position, velocity, damage, knockback);
            }
            if (player.HasItem(ItemID.FlowerofFrost))
            {
                HasFrostFlower(player, source, position, velocity, damage, knockback);
            }
            if (player.HasItem(ItemID.BlizzardStaff))
            {
                HasBlizzardStaff(player, source, damage, knockback);
            }
        }
        private void HasSnowBall(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            int projectile4 = (int)(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .5f);
            for (int i = 0; i < projectile4; i++)
            {
                float ToRa = 0;
                if (projectile4 != 1)
                {
                    ToRa = projectile4 * 7;
                }
                Projectile.NewProjectile(source, position, velocity.RotateRandom(ToRa).RandomSpread(7) * 1.5f, ProjectileID.SnowBallFriendly, damage, knockback, player.whoAmI);
            }
        }
        private void HasFrostFlower(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            int projectile5 = (int)(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .1666667f);
            for (int i = 0; i < projectile5; i++)
            {
                float ToRa = 0;
                if (projectile5 != 1)
                {
                    ToRa = projectile5 * 5;
                }
                Projectile.NewProjectile(source, position, velocity.RotateRandom(ToRa).RandomSpread(12), ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
            }
        }
        private void HasBlizzardStaff(Player player, EntitySource_ItemUse_WithAmmo source, int damage, float knockback)
        {
            if (player.GetModPlayer<IceStormPlayer>().SpeedMultiplier < 8)
            {
                return;
            }
            Vector2 SkyPos = new Vector2(player.Center.X, player.Center.Y - 800);
            Vector2 SkyVelocity = (Main.MouseWorld - SkyPos).SafeNormalize(Vector2.UnitX);
            for (int i = 0; i < 5; i++)
            {
                SkyPos += Main.rand.NextVector2Circular(200, 200);
                int FinalCharge = Projectile.NewProjectile(source, SkyPos, SkyVelocity * 30, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
                Main.projectile[FinalCharge].tileCollide = false;
                Main.projectile[FinalCharge].timeLeft = 100;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ItemID.IceBow)
                .Register();
        }
    }
    public class IceStormPlayer : ModPlayer
    {
        public float SpeedMultiplier = 1;
        public override void PostHurt(Player.HurtInfo info)
        {
            float Modify = SpeedMultiplier <= 3 ? 1f : SpeedMultiplier - 2f;
            SpeedMultiplier = Modify;
            base.PostHurt(info);
        }

        public override void PostUpdate()
        {
            if (Player.HeldItem.type != ModContent.ItemType<IceStorm>())
            {
                SpeedMultiplier = 1;
            }
        }
    }
    class IceStormSnowBallCannonMinion : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SnowballCannon);
        public override void SetDefaults()
        {
            Projectile.height = 26;
            Projectile.width = 50;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 9000;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        int timer = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.active && player.HeldItem.type == ModContent.ItemType<IceStorm>() && player.HasItem(ItemID.SnowballCannon))
            {
                Projectile.timeLeft = 2;
            }
            Projectile.IdleFloatMovement(player, out Vector2 vec, out float dis);
            Projectile.MoveToIdle(vec, dis, 10, 10);
            Projectile.rotation = (Projectile.Center - Main.MouseWorld).ToRotation();
            Projectile.spriteDirection = Projectile.Center.X < Main.MouseWorld.X ? 1 : -1;
            Projectile.rotation += Projectile.spriteDirection == -1 ? 0 : MathHelper.Pi;
            if (player.Center.LookForHostileNPC(out NPC npc, 500) && npc != null)
            {
                Vector2 velocityToNpc = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.spriteDirection = Projectile.Center.X < npc.Center.X ? 1 : -1;
                Projectile.rotation = velocityToNpc.ToRotation();
                Projectile.rotation += Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi;
                if (timer <= 0)
                {
                    timer = (int)(20 * Math.Clamp(1 - player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .125f, .1f, 1f));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.PositionOFFSET(velocityToNpc, 45f), velocityToNpc * 20f, ProjectileID.SnowBallFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    return;
                }
            }
            timer = BossRushUtils.CoolDown(timer);
        }
    }
    class IceStormFrostFlowerMinion : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FlowerofFrost);
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 9000;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        int timer = 0;
        public override void AI()
        {
            Dust.NewDust(Projectile.Center, 0, 0, DustID.Frost, 0, 0, 0, default, Main.rand.NextFloat(.5f, .75f));
            Player player = Main.player[Projectile.owner];
            if (player.active && player.HeldItem.type == ModContent.ItemType<IceStorm>() && player.HasItem(ItemID.FlowerofFrost))
            {
                Projectile.timeLeft = 2;
            }
            Vector2 positionToIdle = player.Center + new Vector2(0, -50);
            Vector2 VelocityRaw = positionToIdle - Projectile.Center;
            Projectile.rotation = -MathHelper.PiOver4;
            Projectile.MoveToIdle(VelocityRaw, 11, 10, true);
            Projectile.ResetMinion(positionToIdle, 1500);
            if (player.Center.LookForHostileNPC(out NPC npc, 500) && npc != null)
            {
                if (timer <= 0)
                {
                    Vector2 velocityToNpc = (npc.Center - player.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(5f, 8f);
                    timer = (int)(30 * Math.Clamp(1 - player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .125f, .1f, 1f));
                    int proj = Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center - new Vector2(0, 20),
                        velocityToNpc,
                        ProjectileID.BallofFrost,
                        Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[proj].timeLeft = 250;
                    return;
                }
            }
            timer = BossRushUtils.CoolDown(timer);
        }
    }
}