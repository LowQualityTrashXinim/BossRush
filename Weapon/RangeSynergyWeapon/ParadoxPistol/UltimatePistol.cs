using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace BossRush.Weapon.RangeSynergyWeapon.ParadoxPistol
{
    class UltimatePistol : WeaponTemplate
    {
        float Counter = 0f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("? Pistol");
            Tooltip.SetDefault("it can't decide what it want to be, so it decide to become everything\nShoot out many thing\nAlt click to shoot down a copy of itself onto the screen");
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

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ModContent.ProjectileType<UltimatePistolMinion>();
            }
            else
            {
                Item.shoot = 5;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<UltimatePistolMinion>()] < 10)
                {
                    Projectile.NewProjectile(source, Main.MouseWorld.X, Main.MouseWorld.Y, 0, 0, ModContent.ProjectileType<UltimatePistolMinion>(), damage, knockback, player.whoAmI);
                }
            }
            else
            {
                Counter += 1f;

                int Arrow = Main.rand.Next(new int[] { ProjectileID.MoonlordArrow, ProjectileID.ShadowFlameArrow, ProjectileID.BeeArrow, ProjectileID.ChlorophyteArrow, ProjectileID.VenomArrow, ProjectileID.IchorArrow, ProjectileID.FrostburnArrow, ProjectileID.FrostArrow, ProjectileID.BoneArrow, ProjectileID.CursedArrow, ProjectileID.HolyArrow, ProjectileID.HellfireArrow, ProjectileID.JestersArrow, ProjectileID.UnholyArrow, ProjectileID.FireArrow, ProjectileID.WoodenArrowFriendly });
                int Bullet = Main.rand.Next(new int[] { ProjectileID.MoonlordBullet, ProjectileID.BulletHighVelocity, ProjectileID.IchorBullet, ProjectileID.PartyBullet, ProjectileID.VenomBullet, ProjectileID.ExplosiveBullet, ProjectileID.NanoBullet, ProjectileID.ChlorophyteBullet, ProjectileID.CursedBullet, ProjectileID.GoldenBullet, ProjectileID.MeteorShot, ProjectileID.CrystalBullet });
                int Boomerang = Main.rand.Next(new int[] { ProjectileID.FruitcakeChakram, ProjectileID.BloodyMachete, ProjectileID.Bananarang, ProjectileID.PaladinsHammerFriendly, ProjectileID.PossessedHatchet, ProjectileID.LightDisc, ProjectileID.Flamarang, ProjectileID.ThornChakram, ProjectileID.IceBoomerang, ProjectileID.WoodenBoomerang, ProjectileID.EnchantedBoomerang });
                int Star = Main.rand.Next(new int[] { ProjectileID.Starfury, ProjectileID.HallowStar, ProjectileID.StarWrath, ProjectileID.FallingStar });
                int BallofMagic = Main.rand.Next(new int[] { ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost });
                int Grenade = Main.rand.Next(new int[] { ProjectileID.ExplosiveBunny, ProjectileID.BouncyGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail, ProjectileID.PartyGirlGrenade });
                int ThrowingKnife = Main.rand.Next(new int[] { ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife, ProjectileID.MagicDagger, ProjectileID.VampireKnife, ProjectileID.ShadowFlameKnife });
                int MusicNote = Main.rand.Next(new int[] { ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote });
                int MagicalBolt = Main.rand.Next(new int[] { ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt });
                int SwordBeam = Main.rand.Next(new int[] { ProjectileID.SwordBeam, ProjectileID.FrostBoltSword, ProjectileID.TerraBeam, ProjectileID.LightBeam, ProjectileID.NightBeam, ProjectileID.EnchantedBeam, ProjectileID.InfluxWaver });
                int Dart = Main.rand.Next(new int[] { ProjectileID.CrystalDart, ProjectileID.CursedDart, ProjectileID.IchorDart });
                int Bee = Main.rand.Next(new int[] { ProjectileID.GiantBee, ProjectileID.Wasp, ProjectileID.Bee });
                int Coin = Main.rand.Next(new int[] { ProjectileID.CopperCoin, ProjectileID.SilverCoin, ProjectileID.GoldCoin, ProjectileID.PlatinumCoin });
                int HalloweenPack = Main.rand.Next(new int[] { ProjectileID.JackOLantern, ProjectileID.CandyCorn, ProjectileID.Bat, ProjectileID.RottenEgg, ProjectileID.Stake });
                int DesertFossil = Main.rand.Next(new int[] { ProjectileID.BoneDagger, ProjectileID.BoneJavelin });
                int HappyChristmasMF = Main.rand.Next(new int[] { ProjectileID.SnowBallFriendly, ProjectileID.FrostBlastFriendly, ProjectileID.OrnamentFriendly, ProjectileID.PineNeedleFriendly, ProjectileID.RocketSnowmanI, ProjectileID.NorthPoleSnowflake, ProjectileID.NorthPoleWeapon, ProjectileID.IceSickle, ProjectileID.FrostBoltStaff, ProjectileID.FrostDaggerfish });
                int DevilPack = Main.rand.Next(new int[] { ProjectileID.DemonScythe, ProjectileID.UnholyTridentFriendly, ProjectileID.DeathSickle });
                int Nature = Main.rand.Next(new int[] { ProjectileID.Leaf, ProjectileID.FlowerPetal, ProjectileID.CrystalLeafShot, ProjectileID.SporeCloud, ProjectileID.ChlorophyteOrb, ProjectileID.FlowerPowPetal });
                int alienShooter = Main.rand.Next(new int[] { ProjectileID.ScutlixLaserFriendly, ProjectileID.LaserMachinegunLaser, ProjectileID.ElectrosphereMissile, ProjectileID.Xenopopper, ProjectileID.ChargedBlasterOrb });
                int Fang = Main.rand.Next(new int[] { ProjectileID.PoisonFang, ProjectileID.VenomFang });
                int JungleTemple = Main.rand.Next(new int[] { ProjectileID.BoulderStaffOfEarth, ProjectileID.HeatRay, ProjectileID.Stynger });

                int[] UltimateProjPack = new int[] { ProjectileID.IceSickle, ProjectileID.DeathSickle, ProjectileID.DemonScythe, ProjectileID.UnholyTridentFriendly, ProjectileID.MoonlordArrow, ProjectileID.ShadowFlameArrow, ProjectileID.BeeArrow, ProjectileID.ChlorophyteArrow, ProjectileID.Hellwing, ProjectileID.VenomArrow, ProjectileID.IchorArrow, ProjectileID.FrostburnArrow, ProjectileID.FrostArrow, ProjectileID.BoneArrow, ProjectileID.CursedArrow, ProjectileID.HolyArrow, ProjectileID.HellfireArrow, ProjectileID.JestersArrow, ProjectileID.UnholyArrow, ProjectileID.FireArrow, ProjectileID.WoodenArrowFriendly, ProjectileID.MoonlordBullet, ProjectileID.BulletHighVelocity, ProjectileID.IchorBullet, ProjectileID.PartyBullet, ProjectileID.VenomBullet, ProjectileID.ExplosiveBullet, ProjectileID.NanoBullet, ProjectileID.ChlorophyteBullet, ProjectileID.CursedBullet, ProjectileID.GoldenBullet, ProjectileID.MeteorShot, ProjectileID.CrystalBullet, ProjectileID.FruitcakeChakram, ProjectileID.BloodyMachete, ProjectileID.Bananarang, ProjectileID.PaladinsHammerFriendly, ProjectileID.PossessedHatchet, ProjectileID.LightDisc, ProjectileID.Flamarang, ProjectileID.ThornChakram, ProjectileID.IceBoomerang, ProjectileID.WoodenBoomerang, ProjectileID.EnchantedBoomerang, ProjectileID.Starfury, ProjectileID.HallowStar, ProjectileID.StarWrath, ProjectileID.FallingStar, ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost, ProjectileID.BouncyGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail, ProjectileID.PartyGirlGrenade, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife, ProjectileID.MagicDagger, ProjectileID.VampireKnife, ProjectileID.ShadowFlameKnife, ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote, ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt, ProjectileID.InfernoFriendlyBolt, ProjectileID.PulseBolt, ProjectileID.BlackBolt, ProjectileID.SwordBeam, ProjectileID.FrostBoltSword, ProjectileID.TerraBeam, ProjectileID.LightBeam, ProjectileID.NightBeam, ProjectileID.EnchantedBeam, ProjectileID.InfluxWaver, ProjectileID.CrystalDart, ProjectileID.CursedDart, ProjectileID.IchorDart, ProjectileID.GiantBee, ProjectileID.Wasp, ProjectileID.Bee, ProjectileID.CopperCoin, ProjectileID.SilverCoin, ProjectileID.GoldCoin, ProjectileID.PlatinumCoin, ProjectileID.JackOLantern, ProjectileID.CandyCorn, ProjectileID.Bat, ProjectileID.RottenEgg, ProjectileID.Stake };

                //type = Main.rand.Next(new int[] { ProjectileID.Flare, ProjectileID.PoisonDartBlowgun, ProjectileID.GoldenShowerFriendly, ProjectileID.ShadowBeamFriendly, ProjectileID.LostSoulFriendly, ProjectileID.EatersBite, ProjectileID.Flairon, ProjectileID.MiniSharkron, ProjectileID.NailFriendly, ProjectileID.Meowmere, ProjectileID.JavelinFriendly, ProjectileID.ToxicFlask, ProjectileID.ToxicBubble, ProjectileID.ClothiersCurse, ProjectileID.PainterPaintball, ProjectileID.VortexBeaterRocket, ProjectileID.NebulaArcanum, ProjectileID.TowerDamageBolt, ProjectileID.NebulaBlaze1, ProjectileID.NebulaBlaze2, ProjectileID.Daybreak, ProjectileID.LunarFlare, ProjectileID.SandnadoFriendly, ProjectileID.SkyFracture, ProjectileID.SpiritFlame, ProjectileID.DD2FlameBurstTowerT1Shot, ProjectileID.DD2FlameBurstTowerT2Shot, ProjectileID.DD2FlameBurstTowerT3Shot, ProjectileID.Ale, ProjectileID.DD2BallistraProj, ProjectileID.MonkStaffT2Ghast, ProjectileID.DD2ApprenticeStorm, ProjectileID.DD2PhoenixBowShot, ProjectileID.MonkStaffT3_AltShot, ProjectileID.ApprenticeStaffT3Shot, ProjectileID.DD2BetsyArrow, ProjectileID.BookStaffShot });

                int newtype = Main.rand.Next(new int[] { ProjectileID.CannonballFriendly, ProjectileID.InfernoFriendlyBolt, ProjectileID.PulseBolt, ProjectileID.BlackBolt, ProjectileID.Blizzard, ProjectileID.MagnetSphereBall, ProjectileID.Shuriken, ProjectileID.WaterBolt, ProjectileID.WaterStream, ProjectileID.CrystalStorm, JungleTemple, Arrow, Bullet, Boomerang, Star, BallofMagic, Grenade, MagicalBolt, MusicNote, ThrowingKnife, SwordBeam, Bee, Dart, Coin, HalloweenPack, DesertFossil, HappyChristmasMF, DevilPack, Nature, alienShooter });
                //Note : ProjectileID.HellfireArrow won't work for some reason, i try making it multiply speed by 10 but it just stop mid air, idk how to fix so yea
                float speedX = velocity.X;
                float speedY = velocity.Y;

                //Arrow
                if (Counter == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int[] Arrow2 = new int[] { ProjectileID.WoodenArrowFriendly, ProjectileID.FireArrow, ProjectileID.FrostburnArrow, ProjectileID.FrostArrow, ProjectileID.BeeArrow, ProjectileID.BoneArrow, ProjectileID.JestersArrow, ProjectileID.UnholyArrow, ProjectileID.HellfireArrow, ProjectileID.IchorArrow, ProjectileID.CursedArrow, ProjectileID.HolyArrow, ProjectileID.VenomArrow, ProjectileID.ShadowFlameArrow, ProjectileID.ChlorophyteArrow, ProjectileID.MoonlordArrow };
                        NumOfProjectile = 5;
                        Vector2 Newspeed = RotateCode(10, i);
                        for (int a = 0; a < Arrow2.Length; a++)
                        {
                            float SpeedMultiplier = 0.5f + a * 0.1f;
                            Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X * SpeedMultiplier, Newspeed.Y * SpeedMultiplier, Arrow2[a], damage, knockback, player.whoAmI);
                        }
                    }
                }
                //BulletHell
                if (Counter == 2)
                {
                    int[] Bullet2 = new int[] { ProjectileID.Bullet, ProjectileID.PartyBullet, ProjectileID.MeteorShot, ProjectileID.BulletHighVelocity, ProjectileID.CursedBullet, ProjectileID.IchorBullet, ProjectileID.GoldenBullet, ProjectileID.CrystalBullet, ProjectileID.ExplosiveBullet, ProjectileID.VenomBullet, ProjectileID.NanoBullet, ProjectileID.ChlorophyteBullet, ProjectileID.MoonlordBullet };
                    for (int i = 0; i < Bullet2.Length; i++)
                    {
                        Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, Bullet2[i], damage, knockback, player.whoAmI);
                    }
                    for (int c = 0; c < Bullet2.Length; c++)
                    {
                        float speedMultiplier = 0.4f + c * 0.05f;
                        NumOfProjectile = c+6;
                        for (int i = 0; i < c + 6; i++)
                        {
                            Vector2 Newspeed = RotateCode(60,i);
                            Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X * speedMultiplier, Newspeed.Y * speedMultiplier, Bullet2[c], damage, knockback, player.whoAmI);
                        }
                    }
                }
                //Shuriken
                if (Counter == 3)
                {
                    NumOfProjectile = 10;
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 NewSpeed = RotateCode(60, i);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.Shuriken, damage, knockback, player.whoAmI);
                    }
                }
                //Boomerang
                if (Counter == 4)
                {
                    NumOfProjectile = 11;
                    for (int i = 0; i < 11; i++)
                    {
                        Vector2 NewSpeed = RotateCode(80,i);
                        switch (i)
                        {
                            case 1:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.WoodenBoomerang, damage, knockback, player.whoAmI);
                                break;
                            case 2:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.EnchantedBoomerang, damage, knockback, player.whoAmI);
                                break;
                            case 3:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.IceBoomerang, damage, knockback, player.whoAmI);
                                break;
                            case 4:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.ThornChakram, damage, knockback, player.whoAmI);
                                break;
                            case 5:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.Flamarang, damage, knockback, player.whoAmI);
                                break;
                            case 6:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.LightDisc, damage, knockback, player.whoAmI);
                                break;
                            case 7:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.PossessedHatchet, damage, knockback, player.whoAmI);
                                break;
                            case 8:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.PaladinsHammerFriendly, damage, knockback, player.whoAmI);
                                break;
                            case 9:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.Bananarang, damage, knockback, player.whoAmI);
                                break;
                            case 10:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.BloodyMachete, damage, knockback, player.whoAmI);
                                break;
                            case 11:
                                Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.FruitcakeChakram, damage, knockback, player.whoAmI);
                                break;
                        }
                    }
                }
                //Star
                if (Counter == 5)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.Starfury, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.HallowStar, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.StarWrath, damage * 3, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.FallingStar, damage, knockback, player.whoAmI);

                    Vector2 StarPosition = Main.MouseWorld;
                    StarPosition.Y -= 950;
                    StarPosition.X -= Main.rand.Next(-100, 100);

                    Vector2 StarPositionToMouse = Main.MouseWorld - StarPosition;
                    Vector2 StarToMouse = StarPositionToMouse.SafeNormalize(Vector2.UnitX);
                    Vector2 StarSpeed = StarToMouse * 10f;

                    float NumberOfProjectile = 36;
                    float rotation = MathHelper.ToRadians(180);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
                    for (int i = 0; i < NumberOfProjectile; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (NumberOfProjectile - 1)));

                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 0.7f, NewSpeed.Y * 0.7f, ProjectileID.FallingStar, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 0.8f, NewSpeed.Y * 0.8f, ProjectileID.Starfury, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 0.9f, NewSpeed.Y * 0.9f, ProjectileID.HallowStar, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.StarWrath, damage * 3, knockback, player.whoAmI);
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                for (int t = 0; t < 25; t++)
                                {
                                    int randomPos = Main.rand.Next(-125, 125);
                                    Projectile.NewProjectile(source, StarPosition.X + randomPos, StarPosition.Y + randomPos, (StarSpeed.X + Main.rand.Next(-15, 15)) * 1.3f, StarSpeed.Y * 1.3f, ProjectileID.Starfury, damage, knockback, player.whoAmI);
                                }
                                break;
                            case 2:
                                for (int t = 0; t < 15; t++)
                                {
                                    int randomPos = Main.rand.Next(-50, 50);
                                    Projectile.NewProjectile(source, StarPosition.X + randomPos, StarPosition.Y + randomPos, (StarSpeed.X + Main.rand.Next(-5, 5)) * 1.25f, StarSpeed.Y * 1.25f, ProjectileID.HallowStar, damage, knockback, player.whoAmI);
                                }
                                break;
                            case 3:
                                for (int t = 0; t < 4; t++)
                                {
                                    Projectile.NewProjectile(source, StarPosition.X, StarPosition.Y, StarSpeed.X + Main.rand.Next(-2, 2), StarSpeed.Y, ProjectileID.StarWrath, damage * 3, knockback, player.whoAmI);
                                }
                                break;
                            case 4:
                                for (int t = 0; t < 35; t++)
                                {
                                    int randomPos = Main.rand.Next(-250, 250);
                                    Projectile.NewProjectile(source, StarPosition.X + randomPos, StarPosition.Y + randomPos, (StarSpeed.X + Main.rand.Next(-15, 15)) * 1.35f, StarSpeed.Y * 1.35f, ProjectileID.FallingStar, damage, knockback, player.whoAmI);
                                }
                                break;
                        }
                    }
                }
                //ColorFireBall	
                if (Counter == 6)
                {
                    NumOfProjectile = 36;
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.BallofFire, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.CursedFlameFriendly, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 NewSpeed = RotateCode(360, i);
                        int FireBallColor = Main.rand.Next(new int[] { ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost });
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, FireBallColor, damage, knockback, player.whoAmI);
                    }
                }
                //WaterBolt+WaterSpray
                if (Counter == 7)
                {
                    NumOfProjectile = 7;
                    for (int i = 0; i < 7; i++)
                    {
                        Vector2 NewSpeed = RotateCode(14, i);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 0.6f, NewSpeed.Y * 0.6f, ProjectileID.WaterBolt, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.WaterStream, damage, knockback, player.whoAmI);
                    }
                }
                //Grenade
                if (Counter == 8)
                {
                    NumOfProjectile = 25;
                    for (int i = 0; i < 25; i++)
                    {
                        int Grenade2 = Main.rand.Next(new int[] { ProjectileID.ExplosiveBunny, ProjectileID.BouncyGrenade, ProjectileID.PartyGirlGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail });
                        Vector2 NewSpeed1 = RotateCode(80,i);
                        Vector2 NewSpeed = RandomSpread(NewSpeed1, 5, 0.75f);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, Grenade2, damage, knockback, player.whoAmI);
                    }
                }
                //SwordBeam
                if (Counter == 9)
                {
                    float NumberOfProjectile = 10;
                    float rotation = MathHelper.ToRadians(20);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 5f;
                    for (int i = 0; i < NumberOfProjectile; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (NumberOfProjectile - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.EnchantedBeam, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.1f, NewSpeed.Y * 1.1f, ProjectileID.SwordBeam, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.2f, NewSpeed.Y * 1.2f, ProjectileID.FrostBoltSword, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.3f, NewSpeed.Y * 1.3f, ProjectileID.LightBeam, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.4f, NewSpeed.Y * 1.4f, ProjectileID.NightBeam, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.5f, NewSpeed.Y * 1.5f, ProjectileID.TerraBeam, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.6f, NewSpeed.Y * 1.6f, ProjectileID.InfluxWaver, damage, knockback, player.whoAmI);
                    }
                }
                //MusicalNote
                if (Counter == 10)
                {
                    float Speedmodifier = 1f;
                    float ProjectileAmount = 15;
                    float rotation = MathHelper.ToRadians(20);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
                    for (int i = 0; i < ProjectileAmount; i++)
                    {
                        int MusicNote2 = Main.rand.Next(new int[] { ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote });
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileAmount - 1)));
                        NewSpeed.X += Main.rand.Next(-7, 7);
                        NewSpeed.Y += Main.rand.Next(-2, 2);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * Speedmodifier, NewSpeed.Y * Speedmodifier, MusicNote2, damage, knockback, player.whoAmI);
                        Speedmodifier -= 0.05f;
                    }
                }
                //Magicalbolt
                if (Counter == 11)
                {
                    float ProjectileAmount = 16;
                    float rotation = MathHelper.ToRadians(16);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
                    for (int i = 0; i < ProjectileAmount; i++)
                    {
                        Vector2 Newspeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileAmount - 1)));
                        int ProjectileChooser = i % 8;
                        int[] MagicalBoltv2 = new int[] { ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt };

                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X, Newspeed.Y, MagicalBoltv2[ProjectileChooser], damage, knockback, player.whoAmI);
                    }
                }
                //knife
                if (Counter == 12)
                {
                    float NumberOfProjectile = 10;
                    float rotation = MathHelper.ToRadians(20);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
                    for (int i = 0; i < NumberOfProjectile; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (NumberOfProjectile - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.ThrowingKnife, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.15f, NewSpeed.Y * 1.15f, ProjectileID.PoisonedKnife, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.25f, NewSpeed.Y * 1.25f, ProjectileID.MagicDagger, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.5f, NewSpeed.Y * 1.5f, ProjectileID.ShadowFlameKnife, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.75f, NewSpeed.Y * 1.75f, ProjectileID.VampireKnife, damage, knockback, player.whoAmI);
                    }

                }
                //dart
                if (Counter == 13)
                {
                    float ProjectileAmount = 5;
                    float rotation = MathHelper.ToRadians(30);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
                    for (int i = 0; i < ProjectileAmount; i++)
                    {
                        Vector2 Newspeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileAmount - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X * 1.5f, Newspeed.Y * 1.5f, ProjectileID.CrystalDart, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X, Newspeed.Y, ProjectileID.CursedDart, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X * 2f, Newspeed.Y * 2f, ProjectileID.IchorDart, damage, knockback, player.whoAmI);
                    }
                }
                //bee
                if (Counter == 14)
                {
                    for (int a = 0; a < 20; a++)
                    {
                        float randomSpeed = Main.rand.NextFloat(0.4f, 1.25f);
                        Projectile.NewProjectile(source, position.X, position.Y, speedX * randomSpeed, speedY * randomSpeed, ProjectileID.Bee, damage, knockback, player.whoAmI);
                    }
                    for (int b = 0; b < 14; b++)
                    {
                        float randomSpeed = Main.rand.NextFloat(0.5f, 1.35f);
                        Projectile.NewProjectile(source, position.X, position.Y, speedX * randomSpeed, speedY * randomSpeed, ProjectileID.GiantBee, damage, knockback, player.whoAmI);
                    }
                    for (int c = 0; c < 10; c++)
                    {
                        float randomSpeed = Main.rand.NextFloat(0.6f, 1.45f);
                        Projectile.NewProjectile(source, position.X, position.Y, speedX * randomSpeed, speedY * randomSpeed, ProjectileID.Wasp, damage, knockback, player.whoAmI);
                    }
                }
                //coin
                if (Counter == 15)
                {
                    float ProjectileAmount = 5;
                    float rotation = MathHelper.ToRadians(5);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 5f;
                    for (int i = 0; i < ProjectileAmount; i++)
                    {
                        Vector2 Newspeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileAmount - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X * 2f, Newspeed.Y * 2f, ProjectileID.PlatinumCoin, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X * 1.5f, Newspeed.Y * 1.5f, ProjectileID.GoldCoin, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X, Newspeed.Y, ProjectileID.SilverCoin, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X * 0.5f, Newspeed.Y * 0.5f, ProjectileID.CopperCoin, damage, knockback, player.whoAmI);
                    }
                }
                //CrystalStorm
                if (Counter == 16)
                {
                    Vector2 SkyPosition = Main.MouseWorld;
                    SkyPosition.Y -= 600f;

                    for (int t = 0; t < 20; t++)
                    {
                        SkyPosition.Y += Main.rand.Next(-100, 100);
                        SkyPosition.X += Main.rand.Next(-75, 75);
                        Vector2 FallingDistance = Main.MouseWorld - SkyPosition;
                        Vector2 FallingDirection = FallingDistance.SafeNormalize(Vector2.UnitX);
                        Vector2 FallingSpeed = FallingDirection * 30f;
                        Projectile.NewProjectile(source, SkyPosition.X, SkyPosition.Y, FallingSpeed.X, FallingSpeed.Y, ProjectileID.CrystalStorm, damage, knockback, player.whoAmI);
                    }
                    float ProjectileAmount = 45;
                    float rotation = MathHelper.ToRadians(40);
                    for (int i = 0; i < ProjectileAmount; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
                        NewSpeed.X += Main.rand.Next(-7, 7);
                        NewSpeed.Y += Main.rand.Next(-7, 7);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.5f, NewSpeed.Y * 1.5f, ProjectileID.CrystalStorm, damage, knockback, player.whoAmI);
                    }
                }
                //MagnetSphereBall
                if (Counter == 17)
                {
                    Vector2 MouseToPlayer = player.Center - Main.MouseWorld;
                    Vector2 SafeMovement = MouseToPlayer.SafeNormalize(Vector2.UnitX);
                    Vector2 MovementTo = SafeMovement * 10f;
                    Projectile.NewProjectile(source, Main.MouseWorld.X, Main.MouseWorld.Y, MovementTo.X, MovementTo.Y, ProjectileID.MagnetSphereBall, damage * 10, knockback, player.whoAmI);
                }
                //HallowWeedPackage
                if (Counter == 18)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.Stake, damage, knockback, player.whoAmI);

                    int num1 = 20;
                    int num2 = 35;
                    int num3 = 5;
                    int num4 = 14;

                    float rotation = MathHelper.ToRadians(20);
                    float rotation2 = MathHelper.ToRadians(25);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                    for (int a = 0; a < num1; a++)
                    {
                        Vector2 Newspeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, a / (num1 - 1)));
                        int RandomNumber = Main.rand.Next(-7, 7);
                        Projectile.NewProjectile(source, position.X, position.Y, (Newspeed.X + RandomNumber) * 0.9f, (Newspeed.Y + RandomNumber) * 0.9f, ProjectileID.CandyCorn, damage, knockback, player.whoAmI);
                    }
                    for (int b = 0; b < num2; b++)
                    {
                        float DelaySpeed = b * 0.1f;
                        Projectile.NewProjectile(source, position.X, position.Y, speedX * DelaySpeed, speedY * DelaySpeed, ProjectileID.RottenEgg, damage, knockback, player.whoAmI);
                    }
                    for (int c = 0; c < num3; c++)
                    {
                        Vector2 Newspeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, c / (num3 - 1)));
                        int RandomNumber = Main.rand.Next(-4, 4);
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X + RandomNumber, Newspeed.Y + RandomNumber, ProjectileID.JackOLantern, damage, knockback, player.whoAmI);
                    }
                    for (int d = 0; d < num4; d++)
                    {
                        Vector2 Newspeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, d / (num4 - 1)));
                        int RandomNumber = Main.rand.Next(-6, 6);
                        Projectile.NewProjectile(source, position.X, position.Y, (Newspeed.X + RandomNumber) * 0.7f, (Newspeed.Y + RandomNumber) * 0.7f, ProjectileID.Bat, damage, knockback, player.whoAmI);
                    }
                }
                //Blizzard
                if (Counter == 19)
                {
                    Vector2 SkyPosition = Main.MouseWorld;
                    SkyPosition.Y -= 600f;

                    float ProjectileNum = 36;
                    float rotation = MathHelper.ToRadians(180);
                    for (int i = 0; i < ProjectileNum; i++)
                    {
                        Vector2 Rotate = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileNum - 1)));
                        Projectile.NewProjectile(source, position, Rotate, ProjectileID.IceSickle, damage, knockback, player.whoAmI);
                    }

                    for (int t = 0; t < 40; t++)
                    {
                        SkyPosition.Y += Main.rand.Next(-100, 100);
                        SkyPosition.X += Main.rand.Next(-75, 75);
                        Vector2 FallingDistance = Main.MouseWorld - SkyPosition;
                        Vector2 FallingDirection = FallingDistance.SafeNormalize(Vector2.UnitX);
                        Vector2 FallingSpeed = FallingDirection * 30f;
                        Projectile.NewProjectile(source, SkyPosition.X, SkyPosition.Y, FallingSpeed.X, FallingSpeed.Y, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
                    }
                    float ProjectileAmount = 20;
                    for (int i = 0; i < ProjectileAmount; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
                        NewSpeed.X += Main.rand.Next(-7, 7);
                        NewSpeed.Y += Main.rand.Next(-7, 7);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.5f, NewSpeed.Y * 1.5f, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
                    }
                }
                //alienShooter
                if (Counter == 20)
                {
                    float ProjectileNum = 36;
                    float rotation2 = MathHelper.ToRadians(180);
                    for (int i = 0; i < ProjectileNum; i++)
                    {
                        Vector2 Speed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation2, -rotation2, i / (ProjectileNum - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, Speed.X * 0.9f, Speed.Y * 0.9f, ProjectileID.ChargedBlasterOrb, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                        NewSpeed.X += Main.rand.Next(-14, 14);
                        NewSpeed.Y += Main.rand.Next(-14, 14);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.ElectrosphereMissile, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                        NewSpeed.X += Main.rand.Next(-9, 9);
                        NewSpeed.Y += Main.rand.Next(-9, 9);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.Xenopopper, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
                        NewSpeed.X += Main.rand.Next(-7, 7);
                        NewSpeed.Y += Main.rand.Next(-7, 7);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 1.5f, NewSpeed.Y * 1.5f, ProjectileID.LaserMachinegunLaser, damage, knockback, player.whoAmI);
                    }
                    float ProjNum = 18;
                    float rotation = MathHelper.ToRadians(90);
                    for (int i = 0; i < ProjNum; i++)
                    {
                        Vector2 Rotate180 = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjNum - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, Rotate180.X, Rotate180.Y, ProjectileID.ScutlixLaserFriendly, damage, knockback, player.whoAmI);
                    }
                }
                //DesertFossil
                if (Counter == 21)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                        int RandomSpeed = Main.rand.Next(-5, 5);
                        int typeDesert = Main.rand.Next(new int[] { ProjectileID.BoneDagger, ProjectileID.BoneJavelin });
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X + RandomSpeed, RotateRandom.Y + RandomSpeed, typeDesert, damage, knockback, player.whoAmI);
                    }
                }
                //PulseBolt
                if (Counter == 22)
                {
                    float NumberOfProjectile = 8;
                    float rotation = MathHelper.ToRadians(24);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
                    for (int i = 0; i < NumberOfProjectile; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (NumberOfProjectile - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 0.35f, NewSpeed.Y * 0.35f, ProjectileID.PulseBolt, damage, knockback, player.whoAmI);
                    }
                }
                //InfernoFriendlyBolt
                if (Counter == 23)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 Newspeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X, Newspeed.Y, ProjectileID.InfernoFriendlyBolt, damage, knockback, player.whoAmI);
                    }
                }
                //BlackBolt or OnyxBlaster + bullet
                if (Counter == 24)
                {
                    float NumberOfProjectile = 10;
                    float rotation = MathHelper.ToRadians(15);
                    for (int i = 0; i < NumberOfProjectile; i++)
                    {
                        Vector2 Newspeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (NumberOfProjectile - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, Newspeed.X * 3f, Newspeed.Y * 3f, ProjectileID.BlackBolt, (int)(damage * 2f), knockback, player.whoAmI);
                    }
                    for (int i = 0; i < NumberOfProjectile * 3; i++)
                    {
                        float SpeedMulti = Main.rand.NextFloat(0.3f, 1.1f);
                        Vector2 RotateRan = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRan.X * SpeedMulti, RotateRan.Y * SpeedMulti, ProjectileID.Bullet, damage, knockback);
                    }
                }
                //HappyChristmasMF
                if (Counter == 25)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.NorthPoleWeapon, damage, knockback, player.whoAmI);
                    for (int i = 0; i < 20; i++)
                    {
                        float SpeedMultiplier = +0.1f + i * 0.1f;
                        Projectile.NewProjectile(source, position.X, position.Y, speedX * SpeedMultiplier, speedY * SpeedMultiplier, ProjectileID.NorthPoleSnowflake, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 35; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(50));
                        float RandomSpeedMultiplier = Main.rand.NextFloat(0.3f, 1.5f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X * RandomSpeedMultiplier, RotateRandom.Y * RandomSpeedMultiplier, ProjectileID.PineNeedleFriendly, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 17; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
                        float RandomSpeedMultiplier = Main.rand.NextFloat(0.6f, 1.4f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X * RandomSpeedMultiplier, RotateRandom.Y * RandomSpeedMultiplier, ProjectileID.FrostDaggerfish, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
                        float RandomSpeedMultiplier = Main.rand.NextFloat(0.65f, 1.35f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X * RandomSpeedMultiplier, RotateRandom.Y * RandomSpeedMultiplier, ProjectileID.SnowBallFriendly, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                        float RandomSpeedMultiplier = Main.rand.NextFloat(0.8f, 1.3f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X * RandomSpeedMultiplier, RotateRandom.Y * RandomSpeedMultiplier, ProjectileID.OrnamentFriendly, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(9));
                        float RandomSpeedMultiplier = Main.rand.NextFloat(0.84f, 1.25f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X * RandomSpeedMultiplier, RotateRandom.Y * RandomSpeedMultiplier, ProjectileID.FrostBlastFriendly, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
                        float RandomSpeedMultiplier = Main.rand.NextFloat(0.89f, 1.17f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X * RandomSpeedMultiplier, RotateRandom.Y * RandomSpeedMultiplier, ProjectileID.FrostBoltStaff, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                        float RandomSpeedMultiplier = Main.rand.NextFloat(0.91f, 1.1f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X * RandomSpeedMultiplier, RotateRandom.Y * RandomSpeedMultiplier, ProjectileID.IceSickle, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 RotateRandom = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6));
                        float RandomSpeedMultiplier = Main.rand.NextFloat(0.95f, 1.1f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRandom.X * RandomSpeedMultiplier, RotateRandom.Y * RandomSpeedMultiplier, ProjectileID.RocketSnowmanI, damage, knockback, player.whoAmI);
                    }
                }
                //DevilPack
                if (Counter == 26)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, speedX * 0.5f, speedX * 0.5f, ProjectileID.UnholyTridentFriendly, damage * 5, knockback, player.whoAmI);
                    float NumProj = 36;
                    float rotate = MathHelper.ToRadians(180);
                    position += Vector2.Normalize(new Vector2(speedX, speedY)) * 5f;
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 NewSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotate, -rotate, i / (NumProj - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X * 0.5f, NewSpeed.Y * 0.5f, ProjectileID.DemonScythe, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, NewSpeed.X, NewSpeed.Y, ProjectileID.DeathSickle, damage, knockback, player.whoAmI);
                    }
                }
                //CannonballFriendly+GoldenBullet
                if (Counter == 27)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        float SpeedMulti = Main.rand.NextFloat(0.9f, 1.6f);
                        Vector2 RotateRan = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRan.X * SpeedMulti, RotateRan.Y * SpeedMulti, ProjectileID.CannonballFriendly, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < 30; i++)
                    {
                        float SpeedMulti = Main.rand.NextFloat(0.4f, 1f);
                        Vector2 RotateRan = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRan.X * SpeedMulti, RotateRan.Y * SpeedMulti, ProjectileID.GoldenBullet, damage, knockback, player.whoAmI);
                    }
                }
                //Nature
                if (Counter == 28)
                {
                    for (int i = 0; i < 80; i++)
                    {
                        int Nature2 = Main.rand.Next(new int[] { ProjectileID.Leaf, ProjectileID.FlowerPetal, ProjectileID.SporeCloud, ProjectileID.ChlorophyteOrb, ProjectileID.FlowerPowPetal, ProjectileID.CrystalLeafShot });
                        Vector2 RotateRan = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(40));
                        float SpeedMultiplier = Main.rand.NextFloat(0.5f, 1.2f);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateRan.X * SpeedMultiplier, RotateRan.Y * SpeedMultiplier, Nature2, damage, knockback, player.whoAmI);
                    }
                }
                //Rocket package
                if (Counter == 29)
                {
                    float ProjectileNum = 30;
                    float rotation = MathHelper.ToRadians(20);
                    for (int i = 0; i < ProjectileNum; i++)
                    {
                        int Rocket = Main.rand.Next(new int[] { ProjectileID.RocketI, ProjectileID.ElectrosphereMissile, ProjectileID.RocketSnowmanI });
                        Vector2 Rotate = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileNum - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, Rotate.X, Rotate.Y, Rocket, damage, knockback, player.whoAmI);
                    }
                }
                //Fang
                if (Counter == 30)
                {
                    float ProjectileNum = 10;
                    float rotation = MathHelper.ToRadians(30);
                    for (int i = 0; i < ProjectileNum; i++)
                    {
                        int Chooser = i % 2;
                        int[] SpiderFang = new int[] { ProjectileID.VenomFang, ProjectileID.PoisonFang };
                        Vector2 Rotate = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileNum - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, Rotate.X * 0.5f, Rotate.Y * 0.5f, SpiderFang[Chooser], damage, knockback, player.whoAmI);
                    }
                }
                //ProjectileID.VortexBeaterRocket
                if (Counter == 31)
                {
                    float projectileNum = 15;
                    float rotation = MathHelper.ToRadians(15);
                    for (int i = 0; i < projectileNum; i++)
                    {
                        Vector2 Rotate = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (projectileNum - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, Rotate.X * 0.5f, Rotate.Y * 0.5f, ProjectileID.VortexBeaterRocket, damage, knockback, player.whoAmI);
                    }
                }
                //JungleTemple
                if (Counter == 32)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ProjectileID.BoulderStaffOfEarth, damage * 10, knockback, player.whoAmI);

                    float ProjNum = 10f;
                    float rotation = MathHelper.ToRadians(20);
                    for (int i = 0; i < ProjNum; i++)
                    {
                        Vector2 RotateSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjNum - 1)));
                        Projectile.NewProjectile(source, position.X, position.Y, RotateSpeed.X * 0.75f, RotateSpeed.Y * 0.75f, ProjectileID.Stynger, damage, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, position.X, position.Y, RotateSpeed.X, RotateSpeed.Y, ProjectileID.HeatRay, damage, knockback, player.whoAmI);
                    }

                }
                //ProjectileID.EaterBite
                if (Counter == 33)
                {
                    float projnum = 18;
                    float projnum2 = 6;
                    float rotation = MathHelper.ToRadians(180);
                    float rotation2 = MathHelper.ToRadians(12);
                    for (int i = 0; i < projnum; i++)
                    {
                        Vector2 Speed1 = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (projnum - 1)));
                        Projectile.NewProjectile(source, position, Speed1, ProjectileID.EatersBite, damage, knockback, player.whoAmI);
                    }
                    for (int i = 0; i < projnum2; i++)
                    {
                        Vector2 Speed2 = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(rotation2, -rotation2, i / (projnum2 - 1)));
                        Projectile.NewProjectile(source, position, Speed2, ProjectileID.EatersBite, damage, knockback, player.whoAmI);
                    }
                }

                //Reset Counter
                if (Counter > 34f)
                {
                    Counter = 0f;
                }
                //UltimateProjectilePack
                if (Counter == 0)
                {
                    for (int i = 0; i < UltimateProjPack.Length; i++)
                    {
                        Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, UltimateProjPack[i], damage, knockback, player.whoAmI);
                    }
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 200000000)
                .Register();
        }
    }
}