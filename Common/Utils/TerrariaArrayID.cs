using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnchantedOreSword;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ForceOfEarth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Utils
{
    public static class TerrariaArrayID
    {
        public readonly static int[] AllOreShortSword = {
            ItemID.CopperShortsword, ItemID.TinShortsword, ItemID.IronShortsword, ItemID.LeadShortsword,
            ItemID.SilverShortsword, ItemID.TungstenShortsword, ItemID.GoldShortsword, ItemID.PlatinumShortsword };
        public readonly static int[] AllOreBroadSword = {
            ItemID.CopperBroadsword, ItemID.TinBroadsword, ItemID.IronBroadsword,ItemID.LeadBroadsword,
            ItemID.SilverBroadsword, ItemID.TungstenBroadsword, ItemID.GoldBroadsword, ItemID.PlatinumBroadsword };
        public readonly static int[] AllWoodBowPHM = {
            ItemID.WoodenBow, ItemID.BorealWoodBow, ItemID.RichMahoganyBow,ItemID.PalmWoodBow, ItemID.EbonwoodBow, ItemID.ShadewoodBow };
        public readonly static int[] AllGemStaffPHM = {
            ItemID.AmethystStaff, ItemID.TopazStaff, ItemID.SapphireStaff, ItemID.EmeraldStaff,ItemID.RubyStaff, ItemID.DiamondStaff
        };
        public readonly static int[] AllGemStafProjectilePHM = {
            ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt,ProjectileID.RubyBolt, ProjectileID.DiamondBolt
        };
        //Pre boss and Pre EoC is the same PreBoss
        public readonly static int[] MeleePreBoss = {
            ItemID.CopperShortsword, ItemID.TinShortsword, ItemID.IronShortsword, ItemID.LeadShortsword,
            ItemID.SilverShortsword, ItemID.TungstenShortsword, ItemID.GoldShortsword, ItemID.PlatinumShortsword,
            ItemID.WoodenSword, ItemID.BorealWoodSword, ItemID.PalmWoodSword, ItemID.RichMahoganySword, ItemID.CactusSword,
            ItemID.EbonwoodSword, ItemID.ShadewoodSword, ItemID.CopperBroadsword, ItemID.TinBroadsword, ItemID.IronBroadsword,
            ItemID.LeadBroadsword, ItemID.SilverBroadsword, ItemID.TungstenBroadsword, ItemID.GoldBroadsword, ItemID.PlatinumBroadsword,
        };
        public readonly static int[] RangePreBoss = {
                ItemID.WoodenBow, ItemID.BorealWoodBow, ItemID.RichMahoganyBow,ItemID.PalmWoodBow, ItemID.EbonwoodBow, ItemID.ShadewoodBow,
                ItemID.CopperBow, ItemID.TinBow, ItemID.IronBow, ItemID.LeadBow, ItemID.SilverBow,ItemID.TungstenBow, ItemID.GoldBow, ItemID.PlatinumBow,
        };
        public readonly static int[] MagicPreBoss = { ItemID.WandofFrosting, ItemID.WandofSparking, ItemID.AmethystStaff, ItemID.TopazStaff, ItemID.SapphireStaff, ItemID.EmeraldStaff };
        public readonly static int[] SummonPreBoss = { ItemID.BabyBirdStaff, ItemID.SlimeStaff, ItemID.FlinxStaff };
        public readonly static int[] SpecialPreBoss = { ItemID.Shuriken, ItemID.ThrowingKnife, ItemID.PoisonedKnife, ItemID.RottenEgg, ItemID.StarAnise, };

        public readonly static int[] MeleePreEoC = {
                ItemID.BladedGlove, ItemID.ZombieArm, ItemID.AntlionClaw, ItemID.StylistKilLaKillScissorsIWish, ItemID.Ruler, ItemID.Umbrella,
                ItemID.TragicUmbrella, ItemID.BreathingReed, ItemID.Gladius, ItemID.BoneSword, ItemID.BatBat, ItemID.TentacleSpike,
                ItemID.CandyCaneSword, ItemID.Katana, ItemID.IceBlade, ItemID.LightsBane, ItemID.BloodButcherer, ItemID.Starfury, ItemID.EnchantedSword,
                ItemID.PurpleClubberfish, ItemID.FalconBlade, ItemID.BladeofGrass, ItemID.WoodYoyo, ItemID.Rally, ItemID.CorruptYoyo, ItemID.CrimsonYoyo,
                ItemID.JungleYoyo, ItemID.Spear, ItemID.Trident, ItemID.ThunderSpear, ItemID.TheRottedFork, ItemID.Swordfish, ItemID.WoodenBoomerang,
                ItemID.EnchantedBoomerang, ItemID.FruitcakeChakram, ItemID.BloodyMachete, ItemID.Shroomerang, ItemID.IceBoomerang, ItemID.ThornChakram,
                ItemID.ChainKnife, ItemID.Mace, ItemID.FlamingMace, ItemID.BallOHurt,ItemID.Terragrim, ItemID.DyeTradersScimitar
            };
        public readonly static int[] RangePreEoC = {
                ItemID.RedRyder, ItemID.FlintlockPistol,ItemID.Musket, ItemID.TheUndertaker, ItemID.Revolver, ItemID.Minishark, ItemID.Boomstick,
                ItemID.Sandgun, ItemID.FlareGun, ItemID.Blowpipe, ItemID.SnowballCannon, ItemID.PainterPaintballGun
        };
        public readonly static int[] MagicPreEoC = {
                ItemID.ThunderStaff, ItemID.AmberStaff,ItemID.RubyStaff, ItemID.DiamondStaff
        };
        public readonly static int[] SummonerPreEoC = {
                ItemID.AbigailsFlower, ItemID.VampireFrogStaff,
                ItemID.BlandWhip, ItemID.ThornWhip
        };
        public readonly static int[] Special = { ItemID.MolotovCocktail,ItemID.FrostDaggerfish, ItemID.Javelin, ItemID.BoneJavelin, ItemID.BoneDagger, ItemID.Grenade, ItemID.StickyGrenade, ItemID.BouncyGrenade,ItemID.PartyGirlGrenade
        };
        public readonly static int[] MeleeEvilBoss = {
            ItemID.FieryGreatsword, ItemID.PurplePhaseblade, ItemID.YellowPhaseblade, ItemID.BluePhaseblade, ItemID.GreenPhaseblade, ItemID.RedPhaseblade, ItemID.WhitePhaseblade,
            ItemID.OrangePhaseblade, ItemID.Flamarang, ItemID.TheMeatball
        };
        public readonly static int[] MagicEvilBoss = { ItemID.Vilethorn, ItemID.CrimsonRod, ItemID.DemonScythe, ItemID.SpaceGun, ItemID.ImpStaff };
        public readonly static int[] MeleeSkel = { ItemID.Muramasa, ItemID.NightsEdge, ItemID.Valor, ItemID.Cascade, ItemID.DarkLance, ItemID.CombatWrench, ItemID.BlueMoon, ItemID.Sunfury };
        public readonly static int[] RangeSkele = { ItemID.HellwingBow, ItemID.QuadBarrelShotgun, ItemID.Handgun, ItemID.PhoenixBlaster };
        public readonly static int[] MagicSkele = { ItemID.MagicMissile, ItemID.AquaScepter, ItemID.FlowerofFire, ItemID.Flamelash, ItemID.WaterBolt, ItemID.BookofSkulls };
        public readonly static int[] SummonSkele = { ItemID.DD2LightningAuraT1Popper, ItemID.DD2FlameburstTowerT1Popper, ItemID.DD2ExplosiveTrapT1Popper, ItemID.DD2BallistraTowerT1Popper, ItemID.BoneWhip };

        public readonly static int[] MeleeHM = {
            ItemID.PearlwoodSword, ItemID.TaxCollectorsStickOfDoom, ItemID.SlapHand, ItemID.CobaltSword, ItemID.PalladiumSword, ItemID.MythrilSword,
            ItemID.OrichalcumSword, ItemID.AdamantiteSword, ItemID.TitaniumSword, ItemID.BreakerBlade, ItemID.IceSickle, ItemID.Cutlass, ItemID.Frostbrand,
            ItemID.BeamSword, ItemID.FetidBaghnakhs, ItemID.Bladetongue, ItemID.HamBat, ItemID.FormatC, ItemID.Gradient, ItemID.Chik, ItemID.HelFire,
            ItemID.Amarok, ItemID.CobaltNaginata, ItemID.PalladiumPike, ItemID.MythrilHalberd, ItemID.OrichalcumHalberd, ItemID.AdamantiteGlaive,
            ItemID.TitaniumTrident, ItemID.FlyingKnife, ItemID.BouncingShield, ItemID.Bananarang, ItemID.Anchor, ItemID.KOCannon, ItemID.DripplerFlail,
            ItemID.ChainGuillotines, ItemID.DaoofPow, ItemID.JoustingLance, ItemID.ShadowFlameKnife,ItemID.ObsidianSwordfish
        };
        public readonly static int[] RangeHM = {
            ItemID.PearlwoodBow, ItemID.Marrow, ItemID.IceBow,ItemID.DaedalusStormbow, ItemID.ShadowFlameBow, ItemID.CobaltRepeater, ItemID.PalladiumRepeater, ItemID.MythrilRepeater, ItemID.OrichalcumRepeater,
            ItemID.AdamantiteRepeater, ItemID.TitaniumRepeater, ItemID.ClockworkAssaultRifle, ItemID.Gatligator, ItemID.Shotgun, ItemID.OnyxBlaster, ItemID.OnyxBlaster,
            ItemID.CoinGun, ItemID.Uzi, ItemID.Toxikarp, ItemID.DartPistol, ItemID.DartRifle
        };
        public readonly static int[] MagicHM = {
            ItemID.SkyFracture, ItemID.CrystalSerpent, ItemID.FlowerofFrost,ItemID.FrostStaff, ItemID.CrystalVileShard, ItemID.SoulDrain, ItemID.MeteorStaff, ItemID.PoisonStaff, ItemID.LaserRifle, ItemID.ZapinatorOrange,
            ItemID.CursedFlames, ItemID.GoldenShower, ItemID.CrystalStorm, ItemID.IceRod, ItemID.ClingerStaff, ItemID.NimbusRod, ItemID.MagicDagger, ItemID.MedusaHead,
            ItemID.SpiritFlame, ItemID.SharpTears
        };
        public readonly static int[] SummonHM = { ItemID.SpiderStaff, ItemID.PirateStaff, ItemID.SanguineStaff, ItemID.QueenSpiderStaff, ItemID.FireWhip, ItemID.CoolWhip };

        public readonly static int[] MeleeQS = { ItemID.RedsYoyo, ItemID.ValkyrieYoyo, ItemID.Arkhalis };
        public readonly static int[] MeleeMech = { ItemID.Code2, ItemID.Yelets, ItemID.MushroomSpear };

        public readonly static int[] MeleePostAllMechs = { ItemID.TrueExcalibur, ItemID.ChlorophyteSaber, ItemID.DeathSickle, ItemID.ChlorophyteClaymore, ItemID.TrueNightsEdge, ItemID.ChlorophytePartisan, ItemID.DD2SquireDemonSword, ItemID.MonkStaffT2, ItemID.MonkStaffT1 };
        public readonly static int[] RangePostAllMech = { ItemID.ChlorophyteShotbow, ItemID.Megashark, ItemID.Flamethrower };
        public readonly static int[] MagicPostAllMech = { ItemID.VenomStaff, ItemID.BookStaff, ItemID.RainbowRod, ItemID.MagicalHarp };
        public readonly static int[] SummonPostAllMech = { ItemID.OpticStaff, ItemID.DD2LightningAuraT2Popper, ItemID.DD2FlameburstTowerT2Popper, ItemID.DD2ExplosiveTrapT2Popper, ItemID.DD2BallistraTowerT2Popper };

        public readonly static int[] MeleePostPlant = { ItemID.ChristmasTreeSword, ItemID.NorthPole, ItemID.PsychoKnife, ItemID.Keybrand, ItemID.Seedler, ItemID.TerraBlade, ItemID.PaladinsHammer, ItemID.FlowerPow, ItemID.ScourgeoftheCorruptor, ItemID.Kraken, ItemID.TheEyeOfCthulhu, ItemID.ShadowJoustingLance, ItemID.VampireKnives, ItemID.TheHorsemansBlade };
        public readonly static int[] RangePostPlant = { ItemID.ChainGun, ItemID.SnowmanCannon, ItemID.ElfMelter, ItemID.PulseBow, ItemID.VenusMagnum, ItemID.TacticalShotgun, ItemID.SniperRifle, ItemID.GrenadeLauncher, ItemID.ProximityMineLauncher, ItemID.RocketLauncher, ItemID.NailGun, ItemID.PiranhaGun, ItemID.JackOLanternLauncher, ItemID.StakeLauncher, ItemID.CandyCornRifle };
        public readonly static int[] MagicPostPlant = { ItemID.InfernoFork, ItemID.SpectreStaff, ItemID.PrincessWeapon, ItemID.WaspGun, ItemID.LeafBlower, ItemID.BatScepter, ItemID.BlizzardStaff, ItemID.Razorpine, ItemID.RainbowGun, ItemID.ToxicFlask, ItemID.NettleBurst };
        public readonly static int[] SummonPostPlant = { ItemID.RavenStaff, ItemID.DeadlySphereStaff, ItemID.PygmyStaff, ItemID.StormTigerStaff, ItemID.StaffoftheFrostHydra, ItemID.MaceWhip, ItemID.ScytheWhip };

        public readonly static int[] MeleePostGolem = { ItemID.PossessedHatchet, ItemID.GolemFist, ItemID.DD2SquireBetsySword, ItemID.MonkStaffT3, ItemID.InfluxWaver };
        public readonly static int[] RangePostGolem = { ItemID.Stynger, ItemID.FireworksLauncher, ItemID.DD2BetsyBow, ItemID.ElectrosphereLauncher, ItemID.Xenopopper };
        public readonly static int[] MagicPostGolem = { ItemID.StaffofEarth, ItemID.HeatRay, ItemID.ApprenticeStaffT3, ItemID.ChargedBlasterCannon, ItemID.LaserMachinegun };
        public readonly static int[] SummonPostGolem = { ItemID.DD2BallistraTowerT3Popper, ItemID.DD2LightningAuraT3Popper, ItemID.DD2FlameburstTowerT3Popper, ItemID.DD2ExplosiveTrapT3Popper, ItemID.XenoStaff };

        public readonly static int[] MeleePreLuna = { ItemID.Flairon, ItemID.PiercingStarlight };
        public readonly static int[] RangePreLuna = { ItemID.Tsunami, ItemID.FairyQueenRangedItem };
        public readonly static int[] MagicPreLuna = { ItemID.RazorbladeTyphoon, ItemID.BubbleGun, ItemID.FairyQueenMagicItem, ItemID.SparkleGuitar };
        public readonly static int[] SummonPreLuna = { ItemID.TempestStaff, ItemID.RainbowWhip };

        public readonly static int[] NonMovementPotion = { ItemID.ArcheryPotion, ItemID.AmmoReservationPotion, ItemID.EndurancePotion, ItemID.HeartreachPotion, ItemID.IronskinPotion, ItemID.MagicPowerPotion, ItemID.RagePotion, ItemID.SummoningPotion, ItemID.WrathPotion, ItemID.RegenerationPotion, ItemID.TitanPotion, ItemID.ThornsPotion, ItemID.ManaRegenerationPotion };
        public readonly static int[] MovementPotion = { ItemID.SwiftnessPotion, ItemID.FeatherfallPotion, ItemID.GravitationPotion, ItemID.WaterWalkingPotion };

        public readonly static int[] defaultArrow = { ItemID.WoodenArrow, ItemID.FlamingArrow, ItemID.FrostburnArrow, ItemID.JestersArrow, ItemID.UnholyArrow, ItemID.BoneArrow, ItemID.HellfireArrow };
        public readonly static int[] ArrowHM = { ItemID.HolyArrow, ItemID.CursedArrow, ItemID.IchorArrow };

        public readonly static int[] defaultBullet = { ItemID.MusketBall, ItemID.SilverBullet, ItemID.TungstenBullet };
        public readonly static int[] BulletHM = { ItemID.CursedBullet, ItemID.IchorBullet, ItemID.GoldenBullet, ItemID.CrystalBullet, ItemID.HighVelocityBullet, ItemID.PartyBullet, ItemID.ExplodingBullet };

        public readonly static int[] defaultDart = { ItemID.PoisonDart, ItemID.Seed };
        public readonly static int[] DartHM = { ItemID.IchorDart, ItemID.CursedDart, ItemID.CrystalDart };

        public readonly static int[] T1CombatAccessory = { ItemID.FeralClaws, ItemID.ObsidianSkull, ItemID.SharkToothNecklace, ItemID.WhiteString, ItemID.BlackCounterweight };
        public readonly static int[] T1MovementAccessory = { ItemID.Aglet, ItemID.FlyingCarpet, ItemID.FrogLeg, ItemID.IceSkates, ItemID.ShoeSpikes, ItemID.ClimbingClaws, ItemID.HermesBoots, ItemID.AmphibianBoots, ItemID.FlurryBoots, ItemID.CloudinaBottle, ItemID.SandstorminaBottle, ItemID.BlizzardinaBottle, ItemID.Flipper, ItemID.AnkletoftheWind, ItemID.BalloonPufferfish, ItemID.TsunamiInABottle, ItemID.LuckyHorseshoe, ItemID.ShinyRedBalloon };
        public readonly static int[] T1HealthAndManaAccessory = { ItemID.BandofRegeneration, ItemID.NaturesGift };

        public readonly static int[] PostEvilCombatAccessory = { ItemID.MagmaStone, ItemID.ObsidianRose };
        public readonly static int[] PostEvilMovementAccessory = { ItemID.LavaCharm, ItemID.Magiluminescence, ItemID.RocketBoots };
        public readonly static int[] PostEvilHealthManaAccessory = { ItemID.BandofStarpower, ItemID.CelestialMagnet };

        public readonly static int[] QueenBeeCombatAccessory = { ItemID.PygmyNecklace, ItemID.HoneyComb };

        public readonly static int[] AnhkCharm = { ItemID.AdhesiveBandage, ItemID.Bezoar, ItemID.Vitamins, ItemID.ArmorPolish, ItemID.Blindfold, ItemID.PocketMirror, ItemID.Nazar, ItemID.Megaphone, ItemID.FastClock, ItemID.TrifoldMap };
        public readonly static int[] HMAccessory = { ItemID.RangerEmblem, ItemID.SorcererEmblem, ItemID.SummonerEmblem, ItemID.WarriorEmblem, ItemID.StarCloak, ItemID.CrossNecklace, ItemID.YoYoGlove, ItemID.TitanGlove, ItemID.PutridScent, ItemID.FleshKnuckles };

        public readonly static int[] Phaseblade = { ItemID.BluePhaseblade, ItemID.RedPhaseblade, ItemID.GreenPhaseblade, ItemID.OrangePhaseblade, ItemID.YellowPhaseblade, ItemID.PurplePhaseblade, ItemID.WhitePhaseblade };

        //DustID
        public readonly static int[] DustGem = { DustID.GemDiamond, DustID.GemAmber, DustID.GemAmethyst, DustID.GemEmerald, DustID.GemRuby, DustID.GemSapphire, DustID.GemTopaz };
        //ProjectileID
        public readonly static int[] Arrow = { ProjectileID.MoonlordArrow, ProjectileID.ShadowFlameArrow, ProjectileID.BeeArrow, ProjectileID.ChlorophyteArrow, ProjectileID.VenomArrow, ProjectileID.IchorArrow, ProjectileID.FrostburnArrow, ProjectileID.FrostArrow, ProjectileID.BoneArrow, ProjectileID.CursedArrow, ProjectileID.HolyArrow, ProjectileID.HellfireArrow, ProjectileID.JestersArrow, ProjectileID.UnholyArrow, ProjectileID.FireArrow, ProjectileID.WoodenArrowFriendly };
        public readonly static int[] Bullet = { ProjectileID.MoonlordBullet, ProjectileID.BulletHighVelocity, ProjectileID.IchorBullet, ProjectileID.PartyBullet, ProjectileID.VenomBullet, ProjectileID.ExplosiveBullet, ProjectileID.NanoBullet, ProjectileID.ChlorophyteBullet, ProjectileID.CursedBullet, ProjectileID.GoldenBullet, ProjectileID.MeteorShot, ProjectileID.CrystalBullet };
        public readonly static int[] Boomerang = { ProjectileID.FruitcakeChakram, ProjectileID.BloodyMachete, ProjectileID.Bananarang, ProjectileID.PaladinsHammerFriendly, ProjectileID.PossessedHatchet, ProjectileID.LightDisc, ProjectileID.Flamarang, ProjectileID.ThornChakram, ProjectileID.IceBoomerang, ProjectileID.WoodenBoomerang, ProjectileID.EnchantedBoomerang };
        public readonly static int[] Star = { ProjectileID.Starfury, ProjectileID.HallowStar, ProjectileID.StarWrath, ProjectileID.FallingStar };
        public readonly static int[] BallofMagic = { ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost };
        public readonly static int[] Grenade = { ProjectileID.ExplosiveBunny, ProjectileID.BouncyGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail, ProjectileID.PartyGirlGrenade };
        public readonly static int[] ThrowingKnife = { ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife, ProjectileID.MagicDagger, ProjectileID.VampireKnife, ProjectileID.ShadowFlameKnife };
        public readonly static int[] MusicNote = { ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote };
        public readonly static int[] MagicalBolt = { ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt };
        public readonly static int[] SwordBeam = { ProjectileID.SwordBeam, ProjectileID.FrostBoltSword, ProjectileID.TerraBeam, ProjectileID.LightBeam, ProjectileID.NightBeam, ProjectileID.EnchantedBeam, ProjectileID.InfluxWaver };
        public readonly static int[] Dart = { ProjectileID.CrystalDart, ProjectileID.CursedDart, ProjectileID.IchorDart };
        public readonly static int[] Coin = { ProjectileID.CopperCoin, ProjectileID.SilverCoin, ProjectileID.GoldCoin, ProjectileID.PlatinumCoin };
        public readonly static int[] HalloweenPack = { ProjectileID.JackOLantern, ProjectileID.CandyCorn, ProjectileID.Bat, ProjectileID.RottenEgg, ProjectileID.Stake };
        public readonly static int[] DesertFossil = { ProjectileID.BoneDagger, ProjectileID.BoneJavelin };
        public readonly static int[] HappyChristmasMF = { ProjectileID.SnowBallFriendly, ProjectileID.FrostBlastFriendly, ProjectileID.OrnamentFriendly, ProjectileID.PineNeedleFriendly, ProjectileID.RocketSnowmanI, ProjectileID.NorthPoleSnowflake, ProjectileID.NorthPoleWeapon, ProjectileID.IceSickle, ProjectileID.FrostBoltStaff, ProjectileID.FrostDaggerfish };
        public readonly static int[] DevilPack = { ProjectileID.DemonScythe, ProjectileID.UnholyTridentFriendly, ProjectileID.DeathSickle };
        public readonly static int[] Nature = { ProjectileID.Leaf, ProjectileID.FlowerPetal, ProjectileID.CrystalLeafShot, ProjectileID.SporeCloud, ProjectileID.ChlorophyteOrb, ProjectileID.FlowerPowPetal };
        public readonly static int[] AlienShooter = { ProjectileID.ScutlixLaserFriendly, ProjectileID.LaserMachinegunLaser, ProjectileID.ElectrosphereMissile, ProjectileID.Xenopopper, ProjectileID.ChargedBlasterOrb };
        public readonly static int[] Fang = { ProjectileID.PoisonFang, ProjectileID.VenomFang };
        public readonly static int[] JungleTemple = { ProjectileID.BoulderStaffOfEarth, ProjectileID.HeatRay, ProjectileID.Stynger };
        public readonly static int[] UltimateProjPack = { ProjectileID.IceSickle, ProjectileID.DeathSickle, ProjectileID.DemonScythe, ProjectileID.UnholyTridentFriendly, ProjectileID.MoonlordArrow, ProjectileID.ShadowFlameArrow, ProjectileID.BeeArrow, ProjectileID.ChlorophyteArrow, ProjectileID.Hellwing, ProjectileID.VenomArrow, ProjectileID.IchorArrow, ProjectileID.FrostburnArrow, ProjectileID.FrostArrow, ProjectileID.BoneArrow, ProjectileID.CursedArrow, ProjectileID.HolyArrow, ProjectileID.HellfireArrow, ProjectileID.JestersArrow, ProjectileID.UnholyArrow, ProjectileID.FireArrow, ProjectileID.WoodenArrowFriendly, ProjectileID.MoonlordBullet, ProjectileID.BulletHighVelocity, ProjectileID.IchorBullet, ProjectileID.PartyBullet, ProjectileID.VenomBullet, ProjectileID.ExplosiveBullet, ProjectileID.NanoBullet, ProjectileID.ChlorophyteBullet, ProjectileID.CursedBullet, ProjectileID.GoldenBullet, ProjectileID.MeteorShot, ProjectileID.CrystalBullet, ProjectileID.FruitcakeChakram, ProjectileID.BloodyMachete, ProjectileID.Bananarang, ProjectileID.PaladinsHammerFriendly, ProjectileID.PossessedHatchet, ProjectileID.LightDisc, ProjectileID.Flamarang, ProjectileID.ThornChakram, ProjectileID.IceBoomerang, ProjectileID.WoodenBoomerang, ProjectileID.EnchantedBoomerang, ProjectileID.Starfury, ProjectileID.HallowStar, ProjectileID.StarWrath, ProjectileID.FallingStar, ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost, ProjectileID.BouncyGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail, ProjectileID.PartyGirlGrenade, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife, ProjectileID.MagicDagger, ProjectileID.VampireKnife, ProjectileID.ShadowFlameKnife, ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote, ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt, ProjectileID.InfernoFriendlyBolt, ProjectileID.PulseBolt, ProjectileID.BlackBolt, ProjectileID.SwordBeam, ProjectileID.FrostBoltSword, ProjectileID.TerraBeam, ProjectileID.LightBeam, ProjectileID.NightBeam, ProjectileID.EnchantedBeam, ProjectileID.InfluxWaver, ProjectileID.CrystalDart, ProjectileID.CursedDart, ProjectileID.IchorDart, ProjectileID.GiantBee, ProjectileID.Wasp, ProjectileID.Bee, ProjectileID.CopperCoin, ProjectileID.SilverCoin, ProjectileID.GoldCoin, ProjectileID.PlatinumCoin, ProjectileID.JackOLantern, ProjectileID.CandyCorn, ProjectileID.Bat, ProjectileID.RottenEgg, ProjectileID.Stake };

        public readonly static int[] FoEProjectileCustom = new int[] {
            ModContent.ProjectileType<CopperBowP>(),
            ModContent.ProjectileType<TinBowP>(),
            ModContent.ProjectileType<IronBowP>(),
            ModContent.ProjectileType<LeadBowP>(),
            ModContent.ProjectileType<SilverBowP>(),
            ModContent.ProjectileType<TungstenBowP>(),
            ModContent.ProjectileType<GoldBowP>(),
            ModContent.ProjectileType<PlatinumBowP>()
        };

        public readonly static int[] EnchantedOreSwordProjectile = new int[] {
                ModContent.ProjectileType<EnchantedCopperSwordP>(),
                ModContent.ProjectileType<EnchantedTinSwordP>(),
                ModContent.ProjectileType<EnchantedLeadSwordP>(),
                ModContent.ProjectileType<EnchantedIronSwordP>(),
                ModContent.ProjectileType<EnchantedSilverSwordP>(),
                ModContent.ProjectileType<EnchantedTungstenSwordP>(),
                ModContent.ProjectileType<EnchantedGoldSwordP>(),
                ModContent.ProjectileType<EnchantedPlatinumSwordP>() };

        public readonly static int[] WeakDrink = new int[] { ItemID.Teacup, ItemID.TropicalSmoothie, ItemID.SmoothieofDarkness, ItemID.PinaColada, ItemID.PeachSangria, ItemID.Lemonade, ItemID.FruitJuice, ItemID.BananaDaiquiri, ItemID.AppleJuice, ItemID.BloodyMoscato, ItemID.MilkCarton };
        public readonly static int[] Smallmeal = new int[] { ItemID.CookedMarshmallow, ItemID.RoastedBird, ItemID.SauteedFrogLegs, ItemID.GrilledSquirrel, ItemID.FruitSalad, ItemID.CookedFish, ItemID.BunnyStew, ItemID.PotatoChips, ItemID.ShuckedOyster };
        public readonly static int[] fruit = new int[] { ItemID.Lemon, ItemID.Peach, ItemID.Apple, ItemID.Apricot, ItemID.BlackCurrant, ItemID.Elderberry, ItemID.Grapefruit, ItemID.Mango, ItemID.Mango, ItemID.Plum, ItemID.Rambutan, ItemID.Coconut, ItemID.BloodOrange, ItemID.Grapes, ItemID.Dragonfruit, ItemID.Starfruit };

        public readonly static int[] MediumMeal = new int[] { ItemID.Sashimi, ItemID.PumpkinPie, ItemID.GrubSoup, ItemID.CookedShrimp, ItemID.BowlofSoup, ItemID.RoastedDuck, ItemID.MonsterLasagna, ItemID.LobsterTail, ItemID.FroggleBunwich, ItemID.Escargot, ItemID.Nachos, ItemID.ShrimpPoBoy, ItemID.Pho, ItemID.PadThai, ItemID.Fries, ItemID.Hotdog, ItemID.FriedEgg, ItemID.BananaSplit, ItemID.ChickenNugget, ItemID.ChocolateChipCookie };
        public readonly static int[] MediumDrink = new int[] { ItemID.PrismaticPunch, ItemID.IceCream, ItemID.CreamSoda, ItemID.CoffeeCup };

        public readonly static int[] BigMeal = new int[] { ItemID.SeafoodDinner, ItemID.GoldenDelight, ItemID.ApplePie, ItemID.BBQRibs, ItemID.Burger, ItemID.Pizza, ItemID.Spaghetti, ItemID.Steak, ItemID.Bacon, ItemID.ChristmasPudding, ItemID.GingerbreadCookie, ItemID.SugarCookie };
        public readonly static int[] StrongDrink = new int[] { ItemID.Milkshake, ItemID.Grapefruit };

        //NPC
        public readonly static int[] BAT = { NPCID.CaveBat, NPCID.GiantBat, NPCID.IceBat, NPCID.IlluminantBat, NPCID.JungleBat, NPCID.SporeBat, NPCID.Lavabat, NPCID.Hellbat };

        //Armor
        public readonly static int[] HeadArmorPreBoss = { ItemID.WoodHelmet, ItemID.BorealWoodHelmet, ItemID.RichMahoganyHelmet, ItemID.EbonwoodHelmet, ItemID.PalmWoodHelmet, ItemID.ShadewoodHelmet, ItemID.CactusHelmet, ItemID.VikingHelmet, ItemID.EmptyBucket, ItemID.NightVisionHelmet, ItemID.DivingHelmet, ItemID.Goggles, ItemID.CopperHelmet, ItemID.TinHelmet, ItemID.IronHelmet, ItemID.LeadHelmet, ItemID.JungleHat, ItemID.MagicHat, ItemID.WizardHat,ItemID.SilverHelmet, ItemID.TungstenHelmet, ItemID.GoldHelmet, ItemID.PlatinumHelmet, ItemID.PumpkinHelmet, ItemID.GladiatorHelmet
        };
        public readonly static int[] BodyArmorPreBoss = { ItemID.WoodBreastplate, ItemID.BorealWoodBreastplate, ItemID.RichMahoganyBreastplate, ItemID.EbonwoodBreastplate, ItemID.PalmWoodBreastplate, ItemID.ShadewoodBreastplate, ItemID.CactusBreastplate, ItemID.FlinxFurCoat, ItemID.Gi, ItemID.CopperChainmail, ItemID.TinChainmail, ItemID.IronChainmail, ItemID.LeadChainmail, ItemID.JungleShirt, ItemID.AmethystRobe, ItemID.DiamondRobe, ItemID.RubyRobe, ItemID.SapphireRobe, ItemID.EmeraldRobe, ItemID.TopazRobe, ItemID.GypsyRobe, ItemID.SilverChainmail, ItemID.TungstenChainmail, ItemID.GoldChainmail, ItemID.PlatinumChainmail, ItemID.PumpkinBreastplate, ItemID.GladiatorBreastplate
        };
        public readonly static int[] LegArmorPreBoss = { ItemID.WoodGreaves, ItemID.BorealWoodGreaves, ItemID.RichMahoganyGreaves, ItemID.EbonwoodGreaves, ItemID.PalmWoodGreaves, ItemID.ShadewoodGreaves, ItemID.CactusLeggings, ItemID.CopperGreaves,ItemID.TinGreaves, ItemID.IronGreaves, ItemID.LeadGreaves, ItemID.JunglePants, ItemID.SilverGreaves, ItemID.TungstenGreaves, ItemID.GoldGreaves, ItemID.PlatinumGreaves, ItemID.PumpkinLeggings, ItemID.GladiatorLeggings
        };
        public readonly static int[] HeadArmorPostEvil ={ItemID.ShadowHelmet,ItemID.MeteorHelmet,ItemID.FossilHelm,ItemID.MoltenHelmet,ItemID.ObsidianHelm,ItemID.CrimsonHelmet
        };
        public readonly static int[] BodyArmorPostEvil ={ItemID.ShadowScalemail,ItemID.MeteorSuit,ItemID.FossilShirt,ItemID.MoltenBreastplate,ItemID.ObsidianShirt,ItemID.CrimsonScalemail
        };
        public readonly static int[] LegArmorPostEvil ={ItemID.ShadowGreaves,ItemID.MeteorLeggings,ItemID.FossilPants,ItemID.MoltenGreaves,ItemID.ObsidianPants,ItemID.CrimsonGreaves
        };
    }
}