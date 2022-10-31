using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Weapon.RangeSynergyWeapon.IceStorm
{
    internal class IceStorm : WeaponTemplate
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
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            if (player.HasItem(ItemID.SnowballCannon))
            {
                tooltips.Add(new TooltipLine(Mod, "smth", $"[i:{ItemID.SnowballCannon}] Charge attack up can shoot snowballs"));
            }
            if (player.HasItem(ItemID.FlowerofFrost))
            {
                tooltips.Add(new TooltipLine(Mod, "smth", $"[i:{ItemID.FlowerofFrost}] Charge attack up can shoot ball of frost"));
            }
            if(player.HasItem(ItemID.BlizzardStaff))
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
            if (Main.mouseLeft && player.GetModPlayer<IceStormPlayer>().SpeedMultiplier <= 8)
            {
                if(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier <= 2)
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
            int projectile2 = 1+((int)player.GetModPlayer<IceStormPlayer>().SpeedMultiplier) / 5;
            for (int i = 0; i < projectile2; i++)
            {
                float ToRa = 0;
                if (projectile2 != 1)
                {
                    ToRa = projectile2 * 3;
                }
                Projectile.NewProjectile(source, position, RotateRandom(ToRa)*2f, ProjectileID.FrostburnArrow, damage, knockback, player.whoAmI);
            }
            if(player.HasItem(ItemID.SnowballCannon))
            {
                int projectile4 = ((int)player.GetModPlayer<IceStormPlayer>().SpeedMultiplier) / 2;
                for (int i = 0; i < projectile4; i++)
                {
                    float ToRa = 0;
                    if (projectile4 != 1)
                    {
                        ToRa = projectile4 * 7;
                    }
                    Projectile.NewProjectile(source, position, RandomSpread(RotateCode(ToRa),7)*1.5f, ProjectileID.SnowBallFriendly, damage, knockback, player.whoAmI);
                }
            }
            int projectile = ((int)player.GetModPlayer<IceStormPlayer>().SpeedMultiplier) / 5;
            for (int i = 0; i < projectile; i++)
            {
                Projectile.NewProjectile(source, position, RandomSpread(RotateCode(5), 4, Main.rand.NextFloat(0.5f, 1f)), ProjectileID.IceBolt, damage, knockback, player.whoAmI);
            }
            if(player.HasItem(ItemID.FlowerofFrost))
            {
                int projectile5 = ((int)player.GetModPlayer<IceStormPlayer>().SpeedMultiplier) / 6;
                for (int i = 0; i < projectile5; i++)
                {
                    float ToRa = 0;
                    if (projectile5 != 1)
                    {
                        ToRa = projectile5 * 5;
                    }
                    Projectile.NewProjectile(source, position, RandomSpread(RotateCode(ToRa), 12), ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
                }
            }
            float projectile3 = player.GetModPlayer<IceStormPlayer>().SpeedMultiplier/ 7f;
            if(projectile3 >= 1)
            {
                Projectile.NewProjectile(source, position, velocity*2f, ProjectileID.FrostArrow, damage, knockback, player.whoAmI);
            }

            if (player.GetModPlayer<IceStormPlayer>().SpeedMultiplier >= 8)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 SkyPos = new Vector2(player.Center.X, player.Center.Y - 800);
                    Vector2 SkyVelocity = (Main.MouseWorld - SkyPos).SafeNormalize(Vector2.UnitX);
                    SkyPos+= Main.rand.NextVector2Circular(200,200);
                    int FinalCharge = Projectile.NewProjectile(source, SkyPos, SkyVelocity * 30, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
                    Main.projectile[FinalCharge].tileCollide = false;
                    Main.projectile[FinalCharge].timeLeft = 100;
                }
                if (count == 0)
                {
                    for (int i = 0; i < 400; i++)
                    {
                        Vector2 circular = Main.rand.NextVector2CircularEdge(25, 25);
                        int dustNum = Dust.NewDust(player.Center, 0, 0, DustID.FrostHydra, circular.X, circular.Y, 0, default, Main.rand.NextFloat(0.75f, 1.5f));
                        Main.dust[dustNum].noGravity = true;
                        Main.dust[dustNum].noLight = false;
                        Main.dust[dustNum].noLightEmittence = false;
                    }
                    count++;
                }
            }
            if(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier <= 7)
            {
                count = 0;
            }
            return false;
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
            float? Modify = SpeedMultiplier <= 3 ? 1f : SpeedMultiplier -2f;
            SpeedMultiplier = Modify.Value;
        }

        public override void PostUpdate()
        {
            if(Player.HeldItem.type != ModContent.ItemType<IceStorm>())
            {
                SpeedMultiplier = 1;
            }
        }
    }
}
