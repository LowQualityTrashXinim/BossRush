using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.ParadoxPistol
{
    class UltimatePistol : ModItem
    {
        int Counter = 0;

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
        int[] Arrow = new int[] { ProjectileID.MoonlordArrow, ProjectileID.ShadowFlameArrow, ProjectileID.BeeArrow, ProjectileID.ChlorophyteArrow, ProjectileID.VenomArrow, ProjectileID.IchorArrow, ProjectileID.FrostburnArrow, ProjectileID.FrostArrow, ProjectileID.BoneArrow, ProjectileID.CursedArrow, ProjectileID.HolyArrow, ProjectileID.HellfireArrow, ProjectileID.JestersArrow, ProjectileID.UnholyArrow, ProjectileID.FireArrow, ProjectileID.WoodenArrowFriendly };
        int[] Bullet = new int[] { ProjectileID.MoonlordBullet, ProjectileID.BulletHighVelocity, ProjectileID.IchorBullet, ProjectileID.PartyBullet, ProjectileID.VenomBullet, ProjectileID.ExplosiveBullet, ProjectileID.NanoBullet, ProjectileID.ChlorophyteBullet, ProjectileID.CursedBullet, ProjectileID.GoldenBullet, ProjectileID.MeteorShot, ProjectileID.CrystalBullet };
        int[] Boomerang = new int[] { ProjectileID.FruitcakeChakram, ProjectileID.BloodyMachete, ProjectileID.Bananarang, ProjectileID.PaladinsHammerFriendly, ProjectileID.PossessedHatchet, ProjectileID.LightDisc, ProjectileID.Flamarang, ProjectileID.ThornChakram, ProjectileID.IceBoomerang, ProjectileID.WoodenBoomerang, ProjectileID.EnchantedBoomerang };
        int Star = Main.rand.Next(new int[] { ProjectileID.Starfury, ProjectileID.HallowStar, ProjectileID.StarWrath, ProjectileID.FallingStar });
        int BallofMagic = Main.rand.Next(new int[] { ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost });
        int Grenade = Main.rand.Next(new int[] { ProjectileID.ExplosiveBunny, ProjectileID.BouncyGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail, ProjectileID.PartyGirlGrenade });
        int ThrowingKnife = Main.rand.Next(new int[] { ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife, ProjectileID.MagicDagger, ProjectileID.VampireKnife, ProjectileID.ShadowFlameKnife });
        int MusicNote = Main.rand.Next(new int[] { ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote });
        int MagicalBolt = Main.rand.Next(new int[] { ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt });
        int SwordBeam = Main.rand.Next(new int[] { ProjectileID.SwordBeam, ProjectileID.FrostBoltSword, ProjectileID.TerraBeam, ProjectileID.LightBeam, ProjectileID.NightBeam, ProjectileID.EnchantedBeam, ProjectileID.InfluxWaver });
        int Dart = Main.rand.Next(new int[] { ProjectileID.CrystalDart, ProjectileID.CursedDart, ProjectileID.IchorDart });
        int[] Coin = new int[] { ProjectileID.CopperCoin, ProjectileID.SilverCoin, ProjectileID.GoldCoin, ProjectileID.PlatinumCoin };
        int HalloweenPack = Main.rand.Next(new int[] { ProjectileID.JackOLantern, ProjectileID.CandyCorn, ProjectileID.Bat, ProjectileID.RottenEgg, ProjectileID.Stake });
        int[] DesertFossil = new int[] { ProjectileID.BoneDagger, ProjectileID.BoneJavelin };
        int HappyChristmasMF = Main.rand.Next(new int[] { ProjectileID.SnowBallFriendly, ProjectileID.FrostBlastFriendly, ProjectileID.OrnamentFriendly, ProjectileID.PineNeedleFriendly, ProjectileID.RocketSnowmanI, ProjectileID.NorthPoleSnowflake, ProjectileID.NorthPoleWeapon, ProjectileID.IceSickle, ProjectileID.FrostBoltStaff, ProjectileID.FrostDaggerfish });
        int[] DevilPack = new int[] { ProjectileID.DemonScythe, ProjectileID.UnholyTridentFriendly, ProjectileID.DeathSickle };
        int Nature = Main.rand.Next(new int[] { ProjectileID.Leaf, ProjectileID.FlowerPetal, ProjectileID.CrystalLeafShot, ProjectileID.SporeCloud, ProjectileID.ChlorophyteOrb, ProjectileID.FlowerPowPetal });
        int alienShooter = Main.rand.Next(new int[] { ProjectileID.ScutlixLaserFriendly, ProjectileID.LaserMachinegunLaser, ProjectileID.ElectrosphereMissile, ProjectileID.Xenopopper, ProjectileID.ChargedBlasterOrb });
        int[] Fang = new int[] { ProjectileID.PoisonFang, ProjectileID.VenomFang };
        int JungleTemple = Main.rand.Next(new int[] { ProjectileID.BoulderStaffOfEarth, ProjectileID.HeatRay, ProjectileID.Stynger });
        int[] UltimateProjPack = new int[] { ProjectileID.IceSickle, ProjectileID.DeathSickle, ProjectileID.DemonScythe, ProjectileID.UnholyTridentFriendly, ProjectileID.MoonlordArrow, ProjectileID.ShadowFlameArrow, ProjectileID.BeeArrow, ProjectileID.ChlorophyteArrow, ProjectileID.Hellwing, ProjectileID.VenomArrow, ProjectileID.IchorArrow, ProjectileID.FrostburnArrow, ProjectileID.FrostArrow, ProjectileID.BoneArrow, ProjectileID.CursedArrow, ProjectileID.HolyArrow, ProjectileID.HellfireArrow, ProjectileID.JestersArrow, ProjectileID.UnholyArrow, ProjectileID.FireArrow, ProjectileID.WoodenArrowFriendly, ProjectileID.MoonlordBullet, ProjectileID.BulletHighVelocity, ProjectileID.IchorBullet, ProjectileID.PartyBullet, ProjectileID.VenomBullet, ProjectileID.ExplosiveBullet, ProjectileID.NanoBullet, ProjectileID.ChlorophyteBullet, ProjectileID.CursedBullet, ProjectileID.GoldenBullet, ProjectileID.MeteorShot, ProjectileID.CrystalBullet, ProjectileID.FruitcakeChakram, ProjectileID.BloodyMachete, ProjectileID.Bananarang, ProjectileID.PaladinsHammerFriendly, ProjectileID.PossessedHatchet, ProjectileID.LightDisc, ProjectileID.Flamarang, ProjectileID.ThornChakram, ProjectileID.IceBoomerang, ProjectileID.WoodenBoomerang, ProjectileID.EnchantedBoomerang, ProjectileID.Starfury, ProjectileID.HallowStar, ProjectileID.StarWrath, ProjectileID.FallingStar, ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost, ProjectileID.BouncyGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail, ProjectileID.PartyGirlGrenade, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife, ProjectileID.MagicDagger, ProjectileID.VampireKnife, ProjectileID.ShadowFlameKnife, ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote, ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt, ProjectileID.InfernoFriendlyBolt, ProjectileID.PulseBolt, ProjectileID.BlackBolt, ProjectileID.SwordBeam, ProjectileID.FrostBoltSword, ProjectileID.TerraBeam, ProjectileID.LightBeam, ProjectileID.NightBeam, ProjectileID.EnchantedBeam, ProjectileID.InfluxWaver, ProjectileID.CrystalDart, ProjectileID.CursedDart, ProjectileID.IchorDart, ProjectileID.GiantBee, ProjectileID.Wasp, ProjectileID.Bee, ProjectileID.CopperCoin, ProjectileID.SilverCoin, ProjectileID.GoldCoin, ProjectileID.PlatinumCoin, ProjectileID.JackOLantern, ProjectileID.CandyCorn, ProjectileID.Bat, ProjectileID.RottenEgg, ProjectileID.Stake };

        //type = Main.rand.Next(new int[] { ProjectileID.Flare, ProjectileID.PoisonDartBlowgun, ProjectileID.GoldenShowerFriendly, ProjectileID.ShadowBeamFriendly, ProjectileID.LostSoulFriendly, ProjectileID.EatersBite, ProjectileID.Flairon, ProjectileID.MiniSharkron, ProjectileID.NailFriendly, ProjectileID.Meowmere, ProjectileID.JavelinFriendly, ProjectileID.ToxicFlask, ProjectileID.ToxicBubble, ProjectileID.ClothiersCurse, ProjectileID.PainterPaintball, ProjectileID.VortexBeaterRocket, ProjectileID.NebulaArcanum, ProjectileID.TowerDamageBolt, ProjectileID.NebulaBlaze1, ProjectileID.NebulaBlaze2, ProjectileID.Daybreak, ProjectileID.LunarFlare, ProjectileID.SandnadoFriendly, ProjectileID.SkyFracture, ProjectileID.SpiritFlame, ProjectileID.DD2FlameBurstTowerT1Shot, ProjectileID.DD2FlameBurstTowerT2Shot, ProjectileID.DD2FlameBurstTowerT3Shot, ProjectileID.Ale, ProjectileID.DD2BallistraProj, ProjectileID.MonkStaffT2Ghast, ProjectileID.DD2ApprenticeStorm, ProjectileID.DD2PhoenixBowShot, ProjectileID.MonkStaffT3_AltShot, ProjectileID.ApprenticeStaffT3Shot, ProjectileID.DD2BetsyArrow, ProjectileID.BookStaffShot });
        //Todo : try and make a global function for just Projectile.NewProjectile
        //use ProjectileID to choose what behavoir to do
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
                Counter += 1;
                float SpeedMultiplier;
                switch (Counter)
                {
                    case 1://Arrow
                        RangeWeaponOverhaul.NumOfProjectile = 5;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(10, i);
                            for (int a = 0; a < Arrow.Length; a++)
                            {
                                SpeedMultiplier = 0.5f + a * 0.1f;
                                Projectile.NewProjectile(source, position, velocity * SpeedMultiplier, Arrow[a], damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 2://BulletHell
                        for (int i = 0; i < Bullet.Length; i++)
                        {
                            Projectile.NewProjectile(source, position, velocity, Bullet[i], damage, knockback, player.whoAmI);
                        }
                        for (int c = 0; c < Bullet.Length; c++)
                        {
                            SpeedMultiplier = 0.4f + c * 0.05f;
                            RangeWeaponOverhaul.NumOfProjectile = c + 6;
                            for (int i = 0; i < c + 6; i++)
                            {
                                velocity = velocity.RotateCode(60, i);
                                Projectile.NewProjectile(source, position, velocity * SpeedMultiplier, Bullet[c], damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 3://Shuriken
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(60, i);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.Shuriken, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 4://Boomerang
                        RangeWeaponOverhaul.NumOfProjectile = 11;
                        for (int i = 0; i < Boomerang.Length; i++)
                        {
                            velocity = velocity.RotateCode(80, i);
                            Projectile.NewProjectile(source, position, velocity, Boomerang[i], damage, knockback, player.whoAmI);
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
                            velocity = velocity.RotateCode(360, i);
                            Projectile.NewProjectile(source, position, velocity * 0.7f, ProjectileID.FallingStar, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 0.8f, ProjectileID.Starfury, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 0.9f, ProjectileID.HallowStar, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.StarWrath, damage * 3, knockback, player.whoAmI);
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
                            velocity = velocity.RotateCode(360, i);
                            int FireBallColor = Main.rand.Next(new int[] { ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost });
                            Projectile.NewProjectile(source, position, velocity, FireBallColor, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 7://WaterBolt+WaterSpray
                        RangeWeaponOverhaul.NumOfProjectile = 7;
                        for (int i = 0; i < 7; i++)
                        {
                            velocity = velocity.RotateCode(14, i);
                            Projectile.NewProjectile(source, position, velocity * 0.6f, ProjectileID.WaterBolt, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.WaterStream, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 8://Grenade
                        RangeWeaponOverhaul.NumOfProjectile = 25;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            int Grenade2 = Main.rand.Next(new int[] { ProjectileID.ExplosiveBunny, ProjectileID.BouncyGrenade, ProjectileID.PartyGirlGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail });
                            velocity = velocity.RotateCode(80,i).RandomSpread(5, 0.75f);
                            Projectile.NewProjectile(source, position, velocity, Grenade2, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 9://SwordBeam
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(80, i);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.EnchantedBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.1f, ProjectileID.SwordBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.2f, ProjectileID.FrostBoltSword, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.3f, ProjectileID.LightBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.4f, ProjectileID.NightBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileID.TerraBeam, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.6f, ProjectileID.InfluxWaver, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 10://MusicalNote
                        SpeedMultiplier = 1f;
                        RangeWeaponOverhaul.NumOfProjectile = 15;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            int MusicNote2 = Main.rand.Next(new int[] { ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote });
                            velocity = velocity.RotateCode(40,i).RandomSpread(0, 7) * SpeedMultiplier;
                            Projectile.NewProjectile(source, position, velocity, MusicNote2, damage, knockback, player.whoAmI);
                            SpeedMultiplier -= 0.05f;
                        }
                        break;
                    case 11://Magicalbolt
                        RangeWeaponOverhaul.NumOfProjectile = 16;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(32, i);
                            int[] MagicalBoltv2 = new int[] { ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt };
                            Projectile.NewProjectile(source, position, velocity, MagicalBoltv2[i % 8], damage, knockback, player.whoAmI);
                        }
                        break;
                    case 12://knife
                        RangeWeaponOverhaul.NumOfProjectile = 12;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(40, i);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.ThrowingKnife, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.15f, ProjectileID.PoisonedKnife, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.25f, ProjectileID.MagicDagger, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.35f, ProjectileID.ShadowFlameKnife, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 1.45f, ProjectileID.VampireKnife, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 13://dart
                        RangeWeaponOverhaul.NumOfProjectile = 5;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(60, i);
                            Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileID.CrystalDart, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.CursedDart, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity * 2f, ProjectileID.IchorDart, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 14://bee
                        for (int a = 0; a < 20; a++)
                        {
                            velocity = velocity.RandomSpread( 0, Main.rand.NextFloat(0.4f, 1.25f));
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.Bee, damage, knockback, player.whoAmI);
                            if (a < 14) { velocity = velocity.RandomSpread( 0, Main.rand.NextFloat(0.5f, 1.35f)); }
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.Bee, damage, knockback, player.whoAmI);
                            if (a < 10) { velocity = velocity.RandomSpread(0, Main.rand.NextFloat(0.6f, 1.45f)); }
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.Bee, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 15://coin
                        RangeWeaponOverhaul.NumOfProjectile = 5;
                        for (int i = 0; i < Coin.Length; i++)
                        {
                            for (int l = 0; l < RangeWeaponOverhaul.NumOfProjectile; l++)
                            {
                                velocity = velocity.RotateCode(5, l);
                                Projectile.NewProjectile(source, position, velocity * .5f * i, Coin[i], damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 16://CrystalStorm
                        RangeWeaponOverhaul.NumOfProjectile = 45;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateRandom(40).RandomSpread( 7, 1.5f);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.CrystalStorm, damage, knockback, player.whoAmI);
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
                                velocity = velocity.RotateCode(20,i).RandomSpread( 7, .9f);
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.CandyCorn, damage, knockback, player.whoAmI);
                            }
                            if (i < 14)
                            {
                                RangeWeaponOverhaul.NumOfProjectile = 14;
                                velocity = velocity.RotateCode(25,i).RandomSpread(6);
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.Bat, damage, knockback, player.whoAmI);
                            }
                            if (i < 5)
                            {
                                RangeWeaponOverhaul.NumOfProjectile = 5;
                                velocity = velocity.RotateCode(25,i).RandomSpread(4);
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.JackOLantern, damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 19://Blizzard
                        for (int i = 0; i < 40; i++)
                        {
                            if (i < 36)
                            {
                                RangeWeaponOverhaul.NumOfProjectile = 36;
                                velocity = velocity.RotateCode(360, i);
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.IceSickle, damage, knockback, player.whoAmI);
                            }
                            if (i < 20)
                            {
                                velocity = velocity.RotateRandom(40).RandomSpread( 7);
                                Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
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
                            velocity = velocity.RotateCode(360, i);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.ChargedBlasterOrb, damage, knockback, player.whoAmI);
                            if (i < 20)
                            {
                                velocity = velocity.RotateRandom(25).RandomSpread( 15, 1.5f);
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.LaserMachinegunLaser, damage, knockback, player.whoAmI);
                            }
                            if (i < 10)
                            {
                                velocity = velocity.RotateRandom(17).RandomSpread( 11);
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.ElectrosphereMissile, damage, knockback, player.whoAmI);
                            }
                            if (i < 15)
                            {
                                velocity = velocity.RotateRandom(15).RandomSpread( 9);
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.Xenopopper, damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 21://DesertFossil
                        for (int i = 0; i < 20; i++)
                        {
                            velocity = velocity.RotateRandom(35).RandomSpread(5);
                            Projectile.NewProjectile(source, position, velocity, DesertFossil[i % 2], damage, knockback, player.whoAmI);
                        }
                        break;
                    case 22://PulseBolt
                        RangeWeaponOverhaul.NumOfProjectile = 8;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(48, i);
                            Projectile.NewProjectile(source, position, velocity * 0.45f, ProjectileID.PulseBolt, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 23://InfernoFriendlyBolt
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(40, i);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.InfernoFriendlyBolt, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 24://BlackBolt or OnyxBlaster + bullet
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile * 3; i++)
                        {
                            velocity = velocity.RotateRandom(30).RandomSpread(0, Main.rand.NextFloat(0.3f, 1.1f));
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.Bullet, damage, knockback);
                            if (i < 10)
                            {
                                velocity = velocity.RotateCode(30, i);
                                Projectile.NewProjectile(source, position, velocity * 3, ProjectileID.BlackBolt, (int)(damage * 2f), knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 25://HappyChristmasMF
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.NorthPoleWeapon, damage, knockback, player.whoAmI);
                        for (int i = 0; i < 35; i++)
                        {
                            velocity = velocity.RotateRandom(50).RandomSpread(0, Main.rand.NextFloat(0.3f, 1.5f));
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.PineNeedleFriendly, damage, knockback, player.whoAmI);
                            if (i < 20)
                            {
                                SpeedMultiplier = +0.1f + i * 0.1f;
                                Projectile.NewProjectile(source, position, velocity * SpeedMultiplier, ProjectileID.NorthPoleSnowflake, damage, knockback, player.whoAmI);
                            }
                            if (i < 17)
                            {
                                velocity = velocity.RotateRandom(40).RandomSpread( 0, Main.rand.NextFloat(0.6f, 1.4f));
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.FrostDaggerfish, damage, knockback, player.whoAmI);
                            }
                            if (i < 15)
                            {
                                velocity = velocity.RotateRandom(30).RandomSpread(0, Main.rand.NextFloat(.65f, 1.35f));
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.SnowBallFriendly, damage, knockback, player.whoAmI);
                            }
                            if (i < 6)
                            {
                                velocity = velocity.RotateRandom(20).RandomSpread( 0, Main.rand.NextFloat(.8f, 1.3f));
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.OrnamentFriendly, damage, knockback, player.whoAmI);
                            }
                            if (i < 5)
                            {
                                velocity = velocity.RotateRandom(9).RandomSpread( 0, Main.rand.NextFloat(.84f, 1.25f));
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.FrostBlastFriendly, damage, knockback, player.whoAmI);
                            }
                            if (i < 4)
                            {
                                velocity = velocity.RotateRandom(8).RandomSpread( 0, Main.rand.NextFloat(0.89f, 1.17f));
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.FrostBoltStaff, damage, knockback, player.whoAmI);
                                velocity = velocity.RotateRandom(15).RandomSpread( 0, Main.rand.NextFloat(0.91f, 1.1f));
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.IceSickle, damage, knockback, player.whoAmI);
                                velocity = velocity.RotateRandom(6).RandomSpread( 0, Main.rand.NextFloat(0.95f, 1.1f));
                                Projectile.NewProjectile(source, position, velocity, ProjectileID.RocketSnowmanI, damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 26://DevilPack
                        RangeWeaponOverhaul.NumOfProjectile = 36;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(360, i);
                            for (int l = 0; l < DevilPack.Length; l++)
                            {
                                Projectile.NewProjectile(source, position, velocity * (.5f + l * .25f), DevilPack[l], damage, knockback, player.whoAmI);
                            }
                        }
                        break;
                    case 27://CannonballFriendly+GoldenBullet
                        for (int i = 0; i < 30; i++)
                        {
                            velocity = velocity.RotateRandom(40).RandomSpread( 1, Main.rand.NextFloat(.4f, 1f));
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.GoldenBullet, damage, knockback, player.whoAmI);
                            if (i < 10) { velocity = velocity.RotateRandom(20).RandomSpread( 1, Main.rand.NextFloat(.9f, 1.6f)); }
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.CannonballFriendly, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 28://Nature
                        for (int i = 0; i < 80; i++)
                        {
                            int Nature2 = Main.rand.Next(new int[] { ProjectileID.Leaf, ProjectileID.FlowerPetal, ProjectileID.SporeCloud, ProjectileID.ChlorophyteOrb, ProjectileID.FlowerPowPetal, ProjectileID.CrystalLeafShot });
                            velocity = velocity.RotateRandom(40).RandomSpread( 0, Main.rand.NextFloat(.5f, 1.2f));
                            Projectile.NewProjectile(source, position, velocity, Nature2, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 29://Rocket package
                        RangeWeaponOverhaul.NumOfProjectile = 30;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            int Rocket = Main.rand.Next(new int[] { ProjectileID.RocketI, ProjectileID.ElectrosphereMissile, ProjectileID.RocketSnowmanI });
                            velocity = velocity.RotateCode(40, i);
                            Projectile.NewProjectile(source, position, velocity, Rocket, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 30://Fang
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            int Chooser = i % 2;
                            velocity = velocity.RotateCode(60, i);
                            Projectile.NewProjectile(source, position, velocity * 0.5f, Fang[Chooser], damage, knockback, player.whoAmI);
                        }
                        break;
                    case 31://ProjectileID.VortexBeaterRocket
                        RangeWeaponOverhaul.NumOfProjectile = 15;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(30, i);
                            Projectile.NewProjectile(source, position, velocity * 0.5f, ProjectileID.VortexBeaterRocket, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 32://JungleTemple
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.BoulderStaffOfEarth, damage * 10, knockback, player.whoAmI);
                        RangeWeaponOverhaul.NumOfProjectile = 10;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(40, i);
                            Projectile.NewProjectile(source, position, velocity * 0.75f, ProjectileID.Stynger, damage, knockback, player.whoAmI);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.HeatRay, damage, knockback, player.whoAmI);
                        }
                        break;
                    case 33://ProjectileID.EaterBite
                        RangeWeaponOverhaul.NumOfProjectile = 18;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(360, i);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.EatersBite, damage, knockback, player.whoAmI);
                        }
                        RangeWeaponOverhaul.NumOfProjectile = 6;
                        for (int i = 0; i < RangeWeaponOverhaul.NumOfProjectile; i++)
                        {
                            velocity = velocity.RotateCode(24, i);
                            Projectile.NewProjectile(source, position, velocity, ProjectileID.EatersBite, damage, knockback, player.whoAmI);
                        }
                        break;
                    default://UltimateProjectilePack
                        for (int i = 0; i < UltimateProjPack.Length; i++)
                        {
                            Projectile.NewProjectile(source, position, velocity, UltimateProjPack[i], damage, knockback, player.whoAmI);
                        }
                        break;
                }
                //Reset Counter
                if (Counter > 34)
                {
                    Counter = 0;
                }
            }
            return true;
        }
    }
}