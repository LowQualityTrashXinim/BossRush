using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Global;
using BossRush.Common.Utils;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ParadoxPistol
{
    class UltimatePistol : ModItem
    {
        int Counter = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("? Pistol");
            Tooltip.SetDefault("it can't decide what it want to be, so it decide to become everything" +
                "\nShoot out many thing" +
                "\nAlt click to shoot down a copy of itself onto the screen");
        }

        public override void SetDefaults()
        {
            Item.damage = 400;
            Item.DamageType = DamageClass.Generic;
            Item.width = 40;
            Item.height = 20;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = 10000;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = 5;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        //type = Main.rand.Next(new int[] { ProjectileID.Flare, ProjectileID.PoisonDartBlowgun, ProjectileID.GoldenShowerFriendly, ProjectileID.ShadowBeamFriendly, ProjectileID.LostSoulFriendly, ProjectileID.EatersBite, ProjectileID.Flairon, ProjectileID.MiniSharkron, ProjectileID.NailFriendly, ProjectileID.Meowmere, ProjectileID.JavelinFriendly, ProjectileID.ToxicFlask, ProjectileID.ToxicBubble, ProjectileID.ClothiersCurse, ProjectileID.PainterPaintball, ProjectileID.VortexBeaterRocket, ProjectileID.NebulaArcanum, ProjectileID.TowerDamageBolt, ProjectileID.NebulaBlaze1, ProjectileID.NebulaBlaze2, ProjectileID.Daybreak, ProjectileID.LunarFlare, ProjectileID.SandnadoFriendly, ProjectileID.SkyFracture, ProjectileID.SpiritFlame, ProjectileID.DD2FlameBurstTowerT1Shot, ProjectileID.DD2FlameBurstTowerT2Shot, ProjectileID.DD2FlameBurstTowerT3Shot, ProjectileID.Ale, ProjectileID.DD2BallistraProj, ProjectileID.MonkStaffT2Ghast, ProjectileID.DD2ApprenticeStorm, ProjectileID.DD2PhoenixBowShot, ProjectileID.MonkStaffT3_AltShot, ProjectileID.ApprenticeStaffT3Shot, ProjectileID.DD2BetsyArrow, ProjectileID.BookStaffShot });
        //Todo : try and make a global function for just Projectile.NewProjectile
        //use ProjectileID to choose what behavoir to do
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<UltimatePistolMinion>()] < 10)
                {
                    Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<UltimatePistolMinion>(), damage, knockback, player.whoAmI);
                }
            }
            else
            {
                Counter += 1;
                float SpeedMultiplier;
                Vector2 newVelocity;
                switch (Counter)
                {
                    case 1://Arrow
                        RangeWeaponOverhaul.NumOfProjectile = 5;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(10, i);
                            for (int a = TerrariaArrayID.Arrow.Length - 1; a >= 0; --a)
                            {
                                SpeedMultiplier = 0.5f + a * 0.15f;
                                Projectile.NewProjectile(source, position, newVelocity * SpeedMultiplier, TerrariaArrayID.Arrow[a], damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 2://BulletHell
                        for (int i = 0; i < TerrariaArrayID.Bullet.Length; i++)
                        {
                            Projectile.NewProjectile(source, position, velocity, TerrariaArrayID.Bullet[i], damage, knockback, player.whoAmI);
                        }
                        for (int c = 0; c < TerrariaArrayID.Bullet.Length; c++)
                        {
                            SpeedMultiplier = 0.4f + c * 0.05f;
                            RangeWeaponOverhaul.NumOfProjectile = c + 6;
                            for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                            {
                                newVelocity = velocity.RotateCode(60, i);
                                Projectile.NewProjectile(source, position, newVelocity * SpeedMultiplier, TerrariaArrayID.Bullet[c], damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 3://Shuriken
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(60, i);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.Shuriken, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 4://Boomerang
                        RangeWeaponOverhaul.NumOfProjectile = 11;
                        for (int i = 0; i < TerrariaArrayID.Boomerang.Length; i++)
                        {
                            newVelocity = velocity.RotateCode(80, i);
                            Projectile.NewProjectile(source, position, newVelocity, TerrariaArrayID.Boomerang[i], damage, knockback, player.whoAmI);
                        }
                        break;
                    case 5://Star
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.Starfury, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.HallowStar, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.StarWrath, damage * 3, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.FallingStar, damage, knockback, player.whoAmI);
                        Vector2 StarPosition = new Vector2(Main.MouseWorld.X - Main.rand.Next(-100, 100), -950);
                        Vector2 StarToMouse = (Main.MouseWorld - StarPosition).SafeNormalize(Vector2.UnitX) * 10f;
                        RangeWeaponOverhaul.NumOfProjectile = 36;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(360, i);
                            Projectile.NewProjectile(source, position, newVelocity * 0.7f, ProjectileID.FallingStar, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 0.8f, ProjectileID.Starfury, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 0.9f, ProjectileID.HallowStar, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.StarWrath, damage * 3, knockback, player.whoAmI);
                        }
                        for (int i = 0; i < 35; i++)
                        {
                            int randomPos = Main.rand.Next(-250, 250);
                            Projectile.NewProjectile(source, StarPosition.X + randomPos, StarPosition.Y + randomPos, (StarToMouse.X + Main.rand.Next(-15, 15)) * 1.35f, StarToMouse.Y * 1.35f, ProjectileID.FallingStar, damage, knockback, player.whoAmI);
                            if (i < 20)
                            {
                                randomPos = Main.rand.Next(-125, 125);
                                Projectile.NewProjectile(source, StarPosition.X + randomPos, StarPosition.Y + randomPos, (StarToMouse.X + Main.rand.Next(-15, 15)) * 1.3f, StarToMouse.Y * 1.3f, ProjectileID.Starfury, damage, knockback, player.whoAmI);
                            }
                            if (i < 15)
                            {
                                randomPos = Main.rand.Next(-50, 50);
                                Projectile.NewProjectile(source, StarPosition.X + randomPos, StarPosition.Y + randomPos, (StarToMouse.X + Main.rand.Next(-5, 5)) * 1.25f, StarToMouse.Y * 1.25f, ProjectileID.HallowStar, damage, knockback, player.whoAmI);
                            }
                            if (i < 4)
                                Projectile.NewProjectile(source, StarPosition.X, StarPosition.Y, StarToMouse.X + Main.rand.Next(-2, 2), StarToMouse.Y, ProjectileID.StarWrath, damage * 3, knockback, player.whoAmI);
                        }
                        break;
                    case 6://ColorFireBall	
                        RangeWeaponOverhaul.NumOfProjectile = 36;
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.BallofFire, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.CursedFlameFriendly, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(360, i);
                            int FireBallColor = Main.rand.Next(new int[] { ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost });
                            Projectile.NewProjectile(source, position, newVelocity, FireBallColor, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 7://WaterBolt+WaterSpray
                        RangeWeaponOverhaul.NumOfProjectile = 7;
                        for (int i = 0; i < 7; i++)
                        {
                            newVelocity = velocity.RotateCode(14, i);
                            Projectile.NewProjectile(source, position, newVelocity * 0.6f, ProjectileID.WaterBolt, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.WaterStream, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 8://Grenade
                        RangeWeaponOverhaul.NumOfProjectile = 25;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            int Grenade2 = Main.rand.Next(new int[] { ProjectileID.ExplosiveBunny, ProjectileID.BouncyGrenade, ProjectileID.PartyGirlGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail });
                            newVelocity = velocity.RotateCode(80, i).RandomSpread(5, 0.75f);
                            Projectile.NewProjectile(source, position, newVelocity, Grenade2, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 9://SwordBeam
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(80, i);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.EnchantedBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.1f, ProjectileID.SwordBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.2f, ProjectileID.FrostBoltSword, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.3f, ProjectileID.LightBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.4f, ProjectileID.NightBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.5f, ProjectileID.TerraBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.6f, ProjectileID.InfluxWaver, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 10://MusicalNote
                        SpeedMultiplier = 1f;
                        RangeWeaponOverhaul.NumOfProjectile = 15;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            int MusicNote2 = Main.rand.Next(new int[] { ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote });
                            newVelocity = velocity.RotateCode(40, i).RandomSpread(0, 7) * SpeedMultiplier;
                            Projectile.NewProjectile(source, position, velocity, MusicNote2, damage, knockback, player.whoAmI);
                            SpeedMultiplier -= 0.05f;
                        }
                        break;
                    case 11://Magicalbolt
                        RangeWeaponOverhaul.NumOfProjectile = 16;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(32, i);
                            int[] MagicalBoltv2 = new int[] { ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt };
                            Projectile.NewProjectile(source, position, newVelocity, MagicalBoltv2[i % 8], damage, knockback, player.whoAmI);
                        }
                        break;
                    case 12://knife
                        RangeWeaponOverhaul.NumOfProjectile = 12;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(40, i);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.ThrowingKnife, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.15f, ProjectileID.PoisonedKnife, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.25f, ProjectileID.MagicDagger, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.35f, ProjectileID.ShadowFlameKnife, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 1.45f, ProjectileID.VampireKnife, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 13://dart
                        RangeWeaponOverhaul.NumOfProjectile = 5;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(60, i);
                            Projectile.NewProjectile(source, position, newVelocity * 1.5f, ProjectileID.CrystalDart, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.CursedDart, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity * 2f, ProjectileID.IchorDart, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 14://bee
                        for (int a = 0; a < 20; a++)
                        {
                            newVelocity = velocity.RandomSpread(0, Main.rand.NextFloat(0.4f, 1.25f));
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.Bee, damage, knockback, player.whoAmI);
                            if (a < 14) { newVelocity = velocity.RandomSpread(0, Main.rand.NextFloat(0.5f, 1.35f)); }
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.Bee, damage, knockback, player.whoAmI);
                            if (a < 10) { newVelocity = velocity.RandomSpread(0, Main.rand.NextFloat(0.6f, 1.45f)); }
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.Bee, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 15://coin
                        RangeWeaponOverhaul.NumOfProjectile = 5;
                        for (int i = 0; i < TerrariaArrayID.Coin.Length; i++)
                        {
                            for (int l = 0; l < RangeWeaponOverhaul.NumOfProjectile; l++)
                            {
                                newVelocity = velocity.RotateCode(5, l);
                                Projectile.NewProjectile(source, position, newVelocity * .85f * i, TerrariaArrayID.Coin[i], damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 16://CrystalStorm
                        RangeWeaponOverhaul.NumOfProjectile = 45;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateRandom(40).RandomSpread(7, 1.5f);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.CrystalStorm, damage, knockback, player.whoAmI);
                            if (i < 20)
                            {
                                Vector2 SkyPosition = new Vector2(Main.MouseWorld.X + Main.rand.Next(-75, 75), Main.MouseWorld.Y - 600 + Main.rand.Next(-100, 100));
                                Vector2 FallingDirection = (Main.MouseWorld - SkyPosition).SafeNormalize(Vector2.UnitX) * 30;
                                Projectile.NewProjectile(source, SkyPosition, FallingDirection, ProjectileID.CrystalStorm, damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 17://MagnetSphereBall
                        Vector2 SafeMovement = (player.Center - Main.MouseWorld).SafeNormalize(Vector2.UnitX) * 10;
                        Projectile.NewProjectile(source, Main.MouseWorld, SafeMovement, ProjectileID.MagnetSphereBall, damage * 10, knockback, player.whoAmI);
                        break;
                    case 18://HallowWeedPackage
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.Stake, damage, knockback, player.whoAmI);
                        for (int i = 0; i < 35; i++)
                        {
                            float DelaySpeed = i * 0.05f;
                            Projectile.NewProjectile(source, position, velocity * DelaySpeed, ProjectileID.RottenEgg, damage, knockback, player.whoAmI);
                            if (i < 20)
                            {
                                RangeWeaponOverhaul.NumOfProjectile = 20;
                                newVelocity = velocity.RotateCode(20, i).RandomSpread(7, .9f);
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.CandyCorn, damage, knockback, player.whoAmI);
                            }
                            if (i < 14)
                            {
                                RangeWeaponOverhaul.NumOfProjectile = 14;
                                newVelocity = velocity.RotateCode(25, i).RandomSpread(6);
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.Bat, damage, knockback, player.whoAmI);
                            }
                            if (i < 5)
                            {
                                RangeWeaponOverhaul.NumOfProjectile = 5;
                                newVelocity = velocity.RotateCode(25, i).RandomSpread(4);
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.JackOLantern, damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 19://Blizzard
                        for (int i = 0; i < 40; i++)
                        {
                            if (i < 36)
                            {
                                RangeWeaponOverhaul.NumOfProjectile = 36;
                                newVelocity = velocity.RotateCode(360, i);
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.IceSickle, damage, knockback, player.whoAmI);
                            }
                            if (i < 20)
                            {
                                newVelocity = velocity.RotateRandom(40).RandomSpread(7);
                                Projectile.NewProjectile(source, position, newVelocity * 1.5f, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
                            }
                            Vector2 SkyPosition = new Vector2(Main.MouseWorld.X + Main.rand.Next(-75, 75), Main.MouseWorld.Y - 600 + Main.rand.Next(-100, 100));
                            Vector2 FallingDirection = (Main.MouseWorld - SkyPosition).SafeNormalize(Vector2.UnitX) * 30;
                            Projectile.NewProjectile(source, SkyPosition, FallingDirection, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 20://alienShooter
                        RangeWeaponOverhaul.NumOfProjectile = 36;
                        for (int i = 0; i < 36; i++)
                        {
                            newVelocity = velocity.RotateCode(360, i);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.ChargedBlasterOrb, damage, knockback, player.whoAmI);
                            if (i < 20)
                            {
                                newVelocity = velocity.RotateRandom(25).RandomSpread(8, 1.5f);
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.LaserMachinegunLaser, damage, knockback, player.whoAmI);
                            }
                            if (i < 10)
                            {
                                newVelocity = velocity.RotateRandom(17).RandomSpread(11);
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.ElectrosphereMissile, damage, knockback, player.whoAmI);
                            }
                            if (i < 15)
                            {
                                newVelocity = velocity.RotateRandom(15).RandomSpread(9);
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.Xenopopper, damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 21://DesertFossil
                        for (int i = 0; i < 20; i++)
                        {
                            newVelocity = velocity.RotateRandom(35).RandomSpread(5);
                            Projectile.NewProjectile(source, position, newVelocity, TerrariaArrayID.DesertFossil[i % 2], damage, knockback, player.whoAmI);
                        }
                        break;
                    case 22://PulseBolt
                        RangeWeaponOverhaul.NumOfProjectile = 8;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(48, i);
                            Projectile.NewProjectile(source, position, newVelocity * 0.45f, ProjectileID.PulseBolt, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 23://InfernoFriendlyBolt
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(40, i);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.InfernoFriendlyBolt, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 24://BlackBolt or OnyxBlaster + bullet
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile * 3; i++)
                        {
                            newVelocity = velocity.RotateRandom(30).RandomSpread(0, Main.rand.NextFloat(0.3f, 1.1f));
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.Bullet, damage, knockback);
                            if (i < 10)
                            {
                                newVelocity = velocity.RotateCode(30, i);
                                Projectile.NewProjectile(source, position, newVelocity * 3, ProjectileID.BlackBolt, (int)(damage * 2f), knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 25://HappyChristmasMF
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.NorthPoleWeapon, damage, knockback, player.whoAmI);
                        for (int i = 0; i < 35; i++)
                        {
                            newVelocity = velocity.RotateRandom(50).RandomSpread(0, Main.rand.NextFloat(0.3f, 1.5f));
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.PineNeedleFriendly, damage, knockback, player.whoAmI);
                            if (i < 20)
                            {
                                SpeedMultiplier = +0.1f + i * 0.1f;
                                Projectile.NewProjectile(source, position, newVelocity * SpeedMultiplier, ProjectileID.NorthPoleSnowflake, damage, knockback, player.whoAmI);
                            }
                            if (i < 17)
                            {
                                newVelocity = velocity.RotateRandom(40).RandomSpread(0, Main.rand.NextFloat(0.6f, 1.4f));
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.FrostDaggerfish, damage, knockback, player.whoAmI);
                            }
                            if (i < 15)
                            {
                                newVelocity = velocity.RotateRandom(30).RandomSpread(0, Main.rand.NextFloat(.65f, 1.35f));
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.SnowBallFriendly, damage, knockback, player.whoAmI);
                            }
                            if (i < 6)
                            {
                                newVelocity = velocity.RotateRandom(20).RandomSpread(0, Main.rand.NextFloat(.8f, 1.3f));
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.OrnamentFriendly, damage, knockback, player.whoAmI);
                            }
                            if (i < 5)
                            {
                                newVelocity = velocity.RotateRandom(9).RandomSpread(0, Main.rand.NextFloat(.84f, 1.25f));
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.FrostBlastFriendly, damage, knockback, player.whoAmI);
                            }
                            if (i < 4)
                            {
                                newVelocity = velocity.RotateRandom(8).RandomSpread(0, Main.rand.NextFloat(0.89f, 1.17f));
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.FrostBoltStaff, damage, knockback, player.whoAmI);
                                newVelocity = velocity.RotateRandom(15).RandomSpread(0, Main.rand.NextFloat(0.91f, 1.1f));
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.IceSickle, damage, knockback, player.whoAmI);
                                newVelocity = velocity.RotateRandom(6).RandomSpread(0, Main.rand.NextFloat(0.95f, 1.1f));
                                Projectile.NewProjectile(source, position, newVelocity, ProjectileID.RocketSnowmanI, damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 26://DevilPack
                        RangeWeaponOverhaul.NumOfProjectile = 36;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(360, i);
                            for (int l = 0; l < TerrariaArrayID.DevilPack.Length; l++)
                            {
                                Projectile.NewProjectile(source, position, newVelocity * (.5f + l * .25f), TerrariaArrayID.DevilPack[l], damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 27://CannonballFriendly+GoldenBullet
                        for (int i = 0; i < 30; i++)
                        {
                            newVelocity = velocity.RotateRandom(40).RandomSpread(1, Main.rand.NextFloat(.4f, 1f));
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.GoldenBullet, damage, knockback, player.whoAmI);
                            if (i < 10) { newVelocity = velocity.RotateRandom(20).RandomSpread(1, Main.rand.NextFloat(.9f, 1.6f)); }
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.CannonballFriendly, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 28://Nature
                        for (int i = 0; i < 80; i++)
                        {
                            int Nature2 = Main.rand.Next(new int[] { ProjectileID.Leaf, ProjectileID.FlowerPetal, ProjectileID.SporeCloud, ProjectileID.ChlorophyteOrb, ProjectileID.FlowerPowPetal, ProjectileID.CrystalLeafShot });
                            newVelocity = velocity.RotateRandom(40).RandomSpread(0, Main.rand.NextFloat(.5f, 1.2f));
                            Projectile.NewProjectile(source, position, newVelocity, Nature2, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 29://Rocket package
                        RangeWeaponOverhaul.NumOfProjectile = 30;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            int Rocket = Main.rand.Next(new int[] { ProjectileID.RocketI, ProjectileID.ElectrosphereMissile, ProjectileID.RocketSnowmanI });
                            newVelocity = velocity.RotateCode(40, i);
                            Projectile.NewProjectile(source, position, newVelocity, Rocket, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 30://Fang
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            int Chooser = i % 2;
                            newVelocity = velocity.RotateCode(60, i);
                            Projectile.NewProjectile(source, position, newVelocity * 0.5f, TerrariaArrayID.Fang[Chooser], damage, knockback, player.whoAmI);
                        }
                        break;
                    case 31://ProjectileID.VortexBeaterRocket
                        RangeWeaponOverhaul.NumOfProjectile = 15;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(30, i);
                            Projectile.NewProjectile(source, position, newVelocity * 0.5f, ProjectileID.VortexBeaterRocket, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 32://JungleTemple
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.BoulderStaffOfEarth, damage * 10, knockback, player.whoAmI);
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(40, i);
                            Projectile.NewProjectile(source, position, newVelocity * 0.75f, ProjectileID.Stynger, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.HeatRay, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 33://ProjectileID.EaterBite
                        RangeWeaponOverhaul.NumOfProjectile = 18;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(360, i);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.EatersBite, damage, knockback, player.whoAmI);
                        }
                        RangeWeaponOverhaul.NumOfProjectile = 6;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            newVelocity = velocity.RotateCode(24, i);
                            Projectile.NewProjectile(source, position, newVelocity, ProjectileID.EatersBite, damage, knockback, player.whoAmI);
                        }
                        break;
                    default://UltimateProjectilePack
                        for (int i = 0; i < TerrariaArrayID.UltimateProjPack.Length; i++)
                        {
                            Projectile.NewProjectile(source, position, velocity, TerrariaArrayID.UltimateProjPack[i], damage, knockback, player.whoAmI);
                        }
                        break;
                }
                //Reset Counter
                if (Counter > 34)
                {
                    Counter = 0;
                }
            }
            return false;
        }
    }
}