using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.Net;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.IceStorm
{
    internal class IceStorm : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An Ice Queen's most reliable defense" +
                "\nHave 75% chance to not consume ammo");
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 98;

            Item.damage = 50;
            Item.knockBack = 1f;
            Item.crit = 5;

            Item.shoot = ProjectileID.FrostArrow;
            Item.shootSpeed = 10f;

            Item.useTime = 50;
            Item.useAnimation = 50;

            Item.rare = 3;
            Item.useAmmo = AmmoID.Arrow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(gold: 50);
            Item.scale = 0.7f;

            Item.UseSound = SoundID.Item5;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            if (player.HasItem(ItemID.SnowballCannon))
            {
                tooltips.Add(new TooltipLine(Mod, "smth", $"[i:{ItemID.SnowballCannon}] Charge attack up can shoot snowballs and a certain minion now follow you"));
            }
            if (player.HasItem(ItemID.FlowerofFrost))
            {
                tooltips.Add(new TooltipLine(Mod, "smth", $"[i:{ItemID.FlowerofFrost}] Charge attack up can shoot ball of frost"));
            }
            if (player.HasItem(ItemID.BlizzardStaff))
            {
                tooltips.Add(new TooltipLine(Mod, "smth", $"[i:{ItemID.BlizzardStaff}] Max charge can now rain down frost spike"));
            }
        }
        public override void HoldItem(Player player)
        {
            if (!Main.mouseLeft && player.GetModPlayer<IceStormPlayer>().SpeedMultiplier >= 1)
            {
                player.GetModPlayer<IceStormPlayer>().SpeedMultiplier -= 0.025f;
            }
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.7f;
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
                Projectile.NewProjectile(source, position, velocity.NextVector2RotatedByRandom(5).NextVector2Square(4, Main.rand.NextFloat(0.5f, 1f)), ProjectileID.IceBolt, damage, knockback, player.whoAmI);
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
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ItemID.IceBow)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
    public class IceStormPlayer : ModPlayer
    {
        public float SpeedMultiplier = 1;
        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            float? Modify = SpeedMultiplier <= 3 ? 1f : SpeedMultiplier - 2f;
            SpeedMultiplier = Modify.Value;
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
        public override string Texture => "BossRush/VanillaSprite/Snowball_Cannon";
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 50;
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
            if (player.active && player.HeldItem.type == ModContent.ItemType<IceStorm>())
            {
                Projectile.timeLeft = 2;
            }
            Projectile.IdleFloatMovement(player, out Vector2 vec, out float dis);
            Projectile.MoveToIdle(vec, dis, 20, 90);
            Projectile.rotation = (Projectile.Center - Main.MouseWorld).ToRotation();
            Projectile.spriteDirection = Projectile.Center.X < Main.MouseWorld.X ? 1 : -1;
            Projectile.rotation += Projectile.spriteDirection == -1 ? 0 : MathHelper.Pi;
            if (player.Center.LookForHostileNPC(out NPC npc, 500) && npc != null)
            {
                Vector2 velocityToNpc = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.spriteDirection = Projectile.Center.X < npc.Center.X ? 1 : -1;
                Projectile.rotation = velocityToNpc.ToRotation();
                Projectile.rotation += Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi;
                if (timer == 0)
                {
                    timer = 20;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 10), velocityToNpc * 20f, ProjectileID.SnowBallFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    return;
                }
            }
            timer = BossRushUtils.CoolDown(timer);
        }
    }
    class IceStormFrostFlowerMinion : ModProjectile
    {
        public override string Texture => "BossRush/VanillaSprite/Flower_of_Frost";
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
            Player player = Main.player[Projectile.owner];
            if (player.active && player.HeldItem.type == ModContent.ItemType<IceStorm>())
            {
                Projectile.timeLeft = 2;
            }
            Vector2 positionToIdle = player.Center + new Vector2(0, -50);
            
            Projectile.MoveToIdle(Projectile.NormalizedVectoDis(positionToIdle), Projectile.NormalizedVectoDis(positionToIdle).Length(), 20, 90);
            if (player.Center.LookForHostileNPC(out NPC npc, 500) && npc != null)
            {
                if (timer == 0)
                {
                    timer = 20;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(), 
                        Projectile.Center - new Vector2(0,20), 
                        Main.rand.NextVector2Unit(-MathHelper.PiOver2 - MathHelper.PiOver4, MathHelper.PiOver4) * 15f, 
                        ProjectileID.BallofFrost, 
                        Projectile.damage, Projectile.knockBack, Projectile.owner);
                    return;
                }
            }
            timer = BossRushUtils.CoolDown(timer);
        }
    }
}
