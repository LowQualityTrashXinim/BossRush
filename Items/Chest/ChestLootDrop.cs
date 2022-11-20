using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Items.Artifact;
using BossRush.Items.CustomPotion;

namespace BossRush.Items.Chest
{
    public abstract class ChestLootDrop : ModItem
    {
        private List<int> DropItemMelee = new List<int>();
        private List<int> DropItemRange = new List<int>();
        private List<int> DropItemMagic = new List<int>();
        private List<int> DropItemSummon = new List<int>();
        private List<int> DropItemMisc = new List<int>();

        readonly int[] MeleePreEoC = new int[] {
                ItemID.CopperShortsword, ItemID.TinShortsword, ItemID.IronShortsword, ItemID.LeadShortsword,
                ItemID.SilverShortsword, ItemID.TungstenShortsword, ItemID.GoldShortsword, ItemID.PlatinumShortsword,
                ItemID.WoodenSword, ItemID.BorealWoodSword, ItemID.PalmWoodSword, ItemID.RichMahoganySword, ItemID.CactusSword,
                ItemID.EbonwoodSword, ItemID.ShadewoodSword, ItemID.CopperBroadsword, ItemID.TinBroadsword, ItemID.IronBroadsword,
                ItemID.LeadBroadsword, ItemID.SilverBroadsword, ItemID.TungstenBroadsword, ItemID.GoldBroadsword, ItemID.PlatinumBroadsword,
                ItemID.BladedGlove, ItemID.ZombieArm, ItemID.AntlionClaw, ItemID.StylistKilLaKillScissorsIWish, ItemID.Ruler, ItemID.Umbrella,
                ItemID.TragicUmbrella, ItemID.BreathingReed, ItemID.Gladius, ItemID.BoneSword, ItemID.BatBat, ItemID.TentacleSpike,
                ItemID.CandyCaneSword, ItemID.Katana, ItemID.IceBlade, ItemID.LightsBane, ItemID.BloodButcherer, ItemID.Starfury, ItemID.EnchantedSword,
                ItemID.PurpleClubberfish, ItemID.FalconBlade, ItemID.BladeofGrass, ItemID.WoodYoyo, ItemID.Rally, ItemID.CorruptYoyo, ItemID.CrimsonYoyo,
                ItemID.JungleYoyo, ItemID.Spear, ItemID.Trident, ItemID.ThunderSpear, ItemID.TheRottedFork, ItemID.Swordfish, ItemID.WoodenBoomerang,
                ItemID.EnchantedBoomerang, ItemID.FruitcakeChakram, ItemID.BloodyMachete, ItemID.Shroomerang, ItemID.IceBoomerang, ItemID.ThornChakram,
                ItemID.ChainKnife, ItemID.Mace, ItemID.FlamingMace, ItemID.BallOHurt,ItemID.Terragrim, ItemID.DyeTradersScimitar
            };
        readonly int[] RangePreEoC = new int[] {
                ItemID.WoodenBow, ItemID.BorealWoodBow, ItemID.RichMahoganyBow,ItemID.PalmWoodBow, ItemID.EbonwoodBow, ItemID.ShadewoodBow,
                ItemID.CopperBow, ItemID.TinBow, ItemID.IronBow, ItemID.LeadBow, ItemID.SilverBow,ItemID.TungstenBow, ItemID.GoldBow, ItemID.PlatinumBow,
                ItemID.RedRyder, ItemID.FlintlockPistol,ItemID.Musket, ItemID.TheUndertaker, ItemID.Revolver, ItemID.Minishark, ItemID.Boomstick,
                ItemID.Sandgun, ItemID.FlareGun, ItemID.Blowpipe, ItemID.SnowballCannon, ItemID.PainterPaintballGun
        };
        readonly int[] MagicPreEoC = new int[] {
                ItemID.WandofSparking,ItemID.ThunderStaff, ItemID.AmethystStaff,
                ItemID.TopazStaff, ItemID.SapphireStaff, ItemID.EmeraldStaff,
                ItemID.RubyStaff, ItemID.DiamondStaff, ItemID.AmberStaff,
        };
        readonly int[] SummonerPreEoC = new int[]{
                ItemID.BabyBirdStaff, ItemID.SlimeStaff, ItemID.FlinxStaff, ItemID.AbigailsFlower, ItemID.VampireFrogStaff,
                ItemID.BlandWhip, ItemID.ThornWhip,
        };
        readonly int[] Special = new int[] {
                ItemID.Shuriken, ItemID.ThrowingKnife, ItemID.PoisonedKnife, ItemID.RottenEgg, ItemID.StarAnise, ItemID.MolotovCocktail,
                ItemID.FrostDaggerfish, ItemID.Javelin, ItemID.BoneJavelin, ItemID.BoneDagger,
                ItemID.Grenade, ItemID.StickyGrenade, ItemID.BouncyGrenade,ItemID.PartyGirlGrenade
        };
        readonly int[] MeleeEvilBoss = new int[]
        {
            ItemID.FieryGreatsword, ItemID.PurplePhaseblade, ItemID.YellowPhaseblade, ItemID.BluePhaseblade, ItemID.GreenPhaseblade, ItemID.RedPhaseblade, ItemID.WhitePhaseblade,
            ItemID.OrangePhaseblade, ItemID.Flamarang, ItemID.TheMeatball
        };
        readonly int[] MagicEvilBoss = new int[] { ItemID.Vilethorn, ItemID.CrimsonRod, ItemID.DemonScythe, ItemID.SpaceGun, ItemID.ImpStaff };
        readonly int[] MeleeSkel = new int[] { ItemID.Muramasa, ItemID.NightsEdge, ItemID.Valor, ItemID.Cascade, ItemID.DarkLance, ItemID.CombatWrench, ItemID.BlueMoon, ItemID.Sunfury };
        readonly int[] RangeSkele = new int[] { ItemID.HellwingBow, ItemID.QuadBarrelShotgun, ItemID.Handgun, ItemID.PhoenixBlaster };
        readonly int[] MagicSkele = new int[] { ItemID.MagicMissile, ItemID.AquaScepter, ItemID.FlowerofFire, ItemID.Flamelash, ItemID.WaterBolt, ItemID.BookofSkulls };
        readonly int[] SummonSkele = new int[] { ItemID.DD2LightningAuraT1Popper, ItemID.DD2FlameburstTowerT1Popper, ItemID.DD2ExplosiveTrapT1Popper, ItemID.DD2BallistraTowerT1Popper, ItemID.BoneWhip };

        readonly int[] MeleeHM = new int[]
        {
            ItemID.PearlwoodSword, ItemID.TaxCollectorsStickOfDoom, ItemID.SlapHand, ItemID.CobaltSword, ItemID.PalladiumSword, ItemID.MythrilSword,
            ItemID.OrichalcumSword, ItemID.AdamantiteSword, ItemID.TitaniumSword, ItemID.BreakerBlade, ItemID.IceSickle, ItemID.Cutlass, ItemID.Frostbrand,
            ItemID.BeamSword, ItemID.FetidBaghnakhs, ItemID.Bladetongue, ItemID.HamBat, ItemID.FormatC, ItemID.Gradient, ItemID.Chik, ItemID.HelFire,
            ItemID.Amarok, ItemID.CobaltNaginata, ItemID.PalladiumPike, ItemID.MythrilHalberd, ItemID.OrichalcumHalberd, ItemID.AdamantiteGlaive,
            ItemID.TitaniumTrident, ItemID.FlyingKnife, ItemID.BouncingShield, ItemID.Bananarang, ItemID.Anchor, ItemID.KOCannon, ItemID.DripplerFlail,
            ItemID.ChainGuillotines, ItemID.DaoofPow, ItemID.JoustingLance, ItemID.ShadowFlameKnife,ItemID.ObsidianSwordfish
        };
        readonly int[] RangeHM = new int[]
        {
            ItemID.PearlwoodBow, ItemID.Marrow, ItemID.IceBow,ItemID.DaedalusStormbow, ItemID.ShadowFlameBow, ItemID.CobaltRepeater, ItemID.PalladiumRepeater, ItemID.MythrilRepeater, ItemID.OrichalcumRepeater,
            ItemID.AdamantiteRepeater, ItemID.TitaniumRepeater, ItemID.ClockworkAssaultRifle, ItemID.Gatligator, ItemID.Shotgun, ItemID.OnyxBlaster, ItemID.OnyxBlaster,
            ItemID.CoinGun, ItemID.Uzi, ItemID.Toxikarp, ItemID.DartPistol, ItemID.DartRifle
        };
        readonly int[] MagicHM = new int[]
        {
            ItemID.SkyFracture, ItemID.CrystalSerpent, ItemID.FlowerofFrost,ItemID.FrostStaff, ItemID.CrystalVileShard, ItemID.SoulDrain, ItemID.MeteorStaff, ItemID.PoisonStaff, ItemID.LaserRifle, ItemID.ZapinatorOrange,
            ItemID.CursedFlames, ItemID.GoldenShower, ItemID.CrystalStorm, ItemID.IceRod, ItemID.ClingerStaff, ItemID.NimbusRod, ItemID.MagicDagger, ItemID.MedusaHead,
            ItemID.SpiritFlame, ItemID.SharpTears,
        };
        readonly int[] SummonHM = new int[] { ItemID.SpiderStaff, ItemID.PirateStaff, ItemID.SanguineStaff, ItemID.QueenSpiderStaff, ItemID.FireWhip, ItemID.CoolWhip };

        readonly int[] MeleeQS = new int[] { ItemID.RedsYoyo, ItemID.ValkyrieYoyo, ItemID.Arkhalis };
        readonly int[] MeleeMech = new int[] { ItemID.Code2, ItemID.Yelets, ItemID.MushroomSpear };

        readonly int[] MeleePostAllMechs = new int[] { ItemID.TrueExcalibur, ItemID.ChlorophyteSaber, ItemID.DeathSickle, ItemID.ChlorophyteClaymore, ItemID.TrueNightsEdge, ItemID.ChlorophytePartisan, ItemID.DD2SquireDemonSword, ItemID.MonkStaffT2, ItemID.MonkStaffT1 };
        readonly int[] RangePostAllMech = new int[] { ItemID.ChlorophyteShotbow, ItemID.Megashark, ItemID.Flamethrower };
        readonly int[] MagicPostAllMech = new int[] { ItemID.VenomStaff, ItemID.BookStaff, ItemID.RainbowRod, ItemID.MagicalHarp };
        readonly int[] SummonPostAllMech = new int[] { ItemID.OpticStaff, ItemID.DD2LightningAuraT2Popper, ItemID.DD2FlameburstTowerT2Popper, ItemID.DD2ExplosiveTrapT2Popper, ItemID.DD2BallistraTowerT2Popper };

        readonly int[] MeleePostPlant = new int[] { ItemID.ChristmasTreeSword, ItemID.NorthPole, ItemID.PsychoKnife, ItemID.Keybrand, ItemID.Seedler, ItemID.TerraBlade, ItemID.PaladinsHammer, ItemID.FlowerPow, ItemID.ScourgeoftheCorruptor, ItemID.Kraken, ItemID.TheEyeOfCthulhu, ItemID.ShadowJoustingLance, ItemID.VampireKnives, ItemID.TheHorsemansBlade };
        readonly int[] RangePostPlant = new int[] { ItemID.ChainGun, ItemID.SnowmanCannon, ItemID.EldMelter, ItemID.PulseBow, ItemID.VenusMagnum, ItemID.TacticalShotgun, ItemID.SniperRifle, ItemID.GrenadeLauncher, ItemID.ProximityMineLauncher, ItemID.RocketLauncher, ItemID.NailGun, ItemID.PiranhaGun, ItemID.JackOLanternLauncher, ItemID.StakeLauncher, ItemID.CandyCornRifle };
        readonly int[] MagicPostPlant = new int[] { ItemID.InfernoFork, ItemID.SpectreStaff, ItemID.PrincessWeapon, ItemID.WaspGun, ItemID.LeafBlower, ItemID.BatScepter, ItemID.BlizzardStaff, ItemID.Razorpine, ItemID.RainbowGun, ItemID.ToxicFlask, ItemID.NettleBurst };
        readonly int[] SummonPostPlant = new int[] { ItemID.RavenStaff, ItemID.DeadlySphereStaff, ItemID.PygmyStaff, ItemID.StormTigerStaff, ItemID.StaffoftheFrostHydra, ItemID.MaceWhip, ItemID.ScytheWhip };

        readonly int[] MeleePostGolem = new int[] { ItemID.PossessedHatchet, ItemID.GolemFist, ItemID.DD2SquireBetsySword, ItemID.MonkStaffT3, ItemID.InfluxWaver };
        readonly int[] RangePostGolem = new int[] { ItemID.Stynger, ItemID.FireworksLauncher, ItemID.DD2BetsyBow, ItemID.ElectrosphereLauncher, ItemID.Xenopopper };
        readonly int[] MagicPostGolem = new int[] { ItemID.StaffofEarth, ItemID.HeatRay, ItemID.ApprenticeStaffT3, ItemID.ChargedBlasterCannon, ItemID.LaserMachinegun };
        readonly int[] SummonPostGolem = new int[] { ItemID.DD2BallistraTowerT3Popper, ItemID.DD2LightningAuraT3Popper, ItemID.DD2FlameburstTowerT3Popper, ItemID.DD2ExplosiveTrapT3Popper, ItemID.XenoStaff };

        readonly int[] MeleePreLuna = new int[] { ItemID.Flairon, ItemID.PiercingStarlight };
        readonly int[] RangePreLuna = new int[] { ItemID.Tsunami, ItemID.FairyQueenRangedItem };
        readonly int[] MagicPreLuna = new int[] { ItemID.RazorbladeTyphoon, ItemID.BubbleGun, ItemID.FairyQueenMagicItem, ItemID.SparkleGuitar };
        readonly int[] SummonPreLuna = new int[] { ItemID.TempestStaff, ItemID.RainbowWhip };

        int meleeChance;
        int rangeChance;
        int magicChance;
        int summonChance;
        int specialChance;

        int amountModifier = 0;

        private int ModifyGetAmount(int ValueToModify, float amountToModify, Player player, bool multiplier = false)
        {
            //Modifier
            if (player.GetModPlayer<ModdedPlayer>().ArtifactAllowance)
            {
                if (player.HasItem(ModContent.ItemType<TokenofGreed>())) { amountToModify += 4; }
                if (ModContent.GetInstance<BossRushModConfig>().EasyMode) { amountToModify += 2; }
                if (player.HasItem(ModContent.ItemType<TokenofPride>())) { amountToModify = 0.5f; multiplier = true; }
            }
            //Change
            if (multiplier)
            {
                return amountModifier > 0 ? (int)(amountToModify * ValueToModify) : 0;
            }
            else
            {
                return ValueToModify + amountToModify > 0 ? (int)(amountToModify + ValueToModify) : 0;
            }
        }
        protected void GetAmount(out int amountForWeapon, out int amountForPotionType, out int amountForPotionNum, Player player)
        {
            amountForWeapon = 2;
            amountForPotionType = 1;
            amountForPotionNum = 2;
            if (Main.getGoodWorld)
            {
                amountForWeapon = 1;
                amountForPotionType = 1;
                amountForPotionNum = 1;
            }
            if (Main.hardMode)
            {
                amountForWeapon += 1;
                amountForPotionType += 1;
                amountForPotionNum += 1;
            }
            amountForWeapon = ModifyGetAmount(amountForWeapon, amountModifier, player);
            amountForPotionType = ModifyGetAmount(amountForPotionType, amountModifier, player);
            amountForPotionNum = ModifyGetAmount(amountForPotionNum, amountModifier, player);
        }
        private int ModifyRNG(int rng, Player player)
        {
            if (Main.rand.NextBool(1000))
            {
                return 7;
            }
            if (player.GetModPlayer<WonderDrugPlayer>().DrugDealer > 0)
            {
                if (Main.rand.Next(100 + player.GetModPlayer<WonderDrugPlayer>().DrugDealer * 5) <= player.GetModPlayer<WonderDrugPlayer>().DrugDealer * 10)
                {
                    return 6;
                }
                else
                {
                    return rng;
                }
            }
            else
            {
                return rng;
            }
        }

        protected int RNGManage(int meleeChance = 25, int rangeChance = 25, int magicChance = 25, int summonChance = 10, int specialChance = 15)
        {
            rangeChance += meleeChance;
            magicChance += rangeChance;
            summonChance += magicChance;
            specialChance += summonChance;
            int chooser = Main.rand.Next(specialChance);
            if (chooser <= meleeChance)
            {
                return 1;
            }
            else if (chooser > meleeChance && chooser <= rangeChance)
            {
                return 2;
            }
            else if (chooser > rangeChance && chooser <= magicChance)
            {
                return 3;
            }
            else if (chooser > magicChance && chooser <= summonChance)
            {
                return 4;
            }
            else if (chooser > summonChance && chooser <= specialChance)
            {
                return 5;
            }
            return 0;
        }

        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootMelee()
        {
            return new List<int> { };
        }
        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootRange()
        {
            return new List<int> { };
        }
        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootMagic()
        {
            return new List<int> { };
        }
        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootSummon()
        {
            return new List<int> { };
        }
        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootMisc()
        {
            return new List<int> { };
        }
        private void AddLoot(List<int> FlagNumber)
        {
            for (int i = 0; i < FlagNumber.Count; i++)
            {
                switch (FlagNumber[i])
                {
                    case 0://PreBoss
                        DropItemMelee.AddRange(MeleePreEoC);
                        DropItemRange.AddRange(RangePreEoC);
                        DropItemMagic.AddRange(MagicPreEoC);
                        DropItemSummon.AddRange(SummonerPreEoC);
                        DropItemMisc.AddRange(Special);
                        break;
                    case 1://EoC
                        DropItemMelee.Add(ItemID.Code1);
                        DropItemMagic.Add(ItemID.ZapinatorGray);
                        break;
                    case 2://Evil boss
                        DropItemMelee.AddRange(MeleeEvilBoss);
                        DropItemRange.Add(ItemID.MoltenFury);
                        DropItemRange.Add(ItemID.StarCannon);
                        DropItemRange.Add(ItemID.AleThrowingGlove);
                        DropItemRange.Add(ItemID.Harpoon);
                        DropItemMagic.AddRange(MagicEvilBoss);
                        DropItemSummon.Add(ItemID.ImpStaff);
                        break;
                    case 3://Skeletron
                        DropItemMelee.AddRange(MeleeSkel);
                        DropItemRange.AddRange(RangeSkele);
                        DropItemMagic.AddRange(MagicSkele);
                        DropItemSummon.AddRange(SummonSkele);
                        break;
                    case 4://Queen bee
                        DropItemMelee.Add(ItemID.BeeKeeper);
                        DropItemRange.Add(ItemID.BeesKnees); DropItemRange.Add(ItemID.Blowgun);
                        DropItemMagic.Add(ItemID.BeeGun);
                        DropItemSummon.Add(ItemID.HornetStaff);
                        DropItemMisc.Add(ItemID.Beenade);
                        break;
                    case 5://Deerclop
                        DropItemRange.Add(ItemID.PewMaticHorn);
                        DropItemMagic.Add(ItemID.WeatherPain);
                        DropItemSummon.Add(ItemID.HoundiusShootius);
                        break;
                    case 6://Wall of flesh
                        DropItemMelee.AddRange(MeleeHM);
                        DropItemRange.AddRange(RangeHM);
                        DropItemMagic.AddRange(MagicHM);
                        DropItemSummon.AddRange(SummonHM);
                        break;
                    case 7://Queen slime
                        DropItemMelee.AddRange(MeleeQS);
                        DropItemSummon.Add(ItemID.Smolstar);
                        break;
                    case 8://First mech
                        DropItemMelee.AddRange(MeleeMech);
                        DropItemRange.Add(ItemID.SuperStarCannon);
                        DropItemRange.Add(ItemID.DD2PhoenixBow);
                        DropItemMagic.Add(ItemID.UnholyTrident);
                        break;
                    case 9://All three mech
                        DropItemMelee.AddRange(MeleePostAllMechs);
                        DropItemRange.AddRange(RangePostAllMech);
                        DropItemMagic.AddRange(MagicPostAllMech);
                        DropItemSummon.AddRange(SummonPostAllMech);
                        break;
                    case 10://Plantera
                        DropItemMelee.AddRange(MeleePostPlant);
                        DropItemRange.AddRange(RangePostPlant);
                        DropItemMagic.AddRange(MagicPostPlant);
                        DropItemSummon.AddRange(SummonPostPlant);
                        break;
                    case 11://Golem
                        DropItemMelee.AddRange(MeleePostGolem);
                        DropItemRange.AddRange(RangePostGolem);
                        DropItemMagic.AddRange(MagicPostGolem);
                        DropItemSummon.AddRange(SummonPostGolem);
                        break;
                    case 12://Pre lunatic (Duke fishron, EoL, ect)
                        DropItemMelee.AddRange(MeleePreLuna);
                        DropItemRange.AddRange(RangePreLuna);
                        DropItemMagic.AddRange(MagicPreLuna);
                        DropItemSummon.AddRange(SummonPreLuna);
                        break;
                    case 13://Lunatic Cultist
                        DropItemMelee.Add(ItemID.DayBreak);
                        DropItemMelee.Add(ItemID.SolarEruption);
                        DropItemRange.Add(ItemID.Phantasm);
                        DropItemRange.Add(ItemID.VortexBeater);
                        DropItemMagic.Add(ItemID.NebulaArcanum);
                        DropItemMagic.Add(ItemID.NebulaBlaze);
                        DropItemSummon.Add(ItemID.StardustCellStaff);
                        DropItemSummon.Add(ItemID.StardustDragonStaff);
                        break;
                    case 14://MoonLord
                        DropItemMelee.Add(ItemID.StarWrath);
                        DropItemMelee.Add(ItemID.Meowmere);
                        DropItemMelee.Add(ItemID.Terrarian);
                        DropItemRange.Add(ItemID.SDMG);
                        DropItemRange.Add(ItemID.Celeb2);
                        DropItemMagic.Add(ItemID.LunarFlareBook);
                        DropItemMagic.Add(ItemID.LastPrism);
                        DropItemSummon.Add(ItemID.RainbowCrystalStaff);
                        DropItemSummon.Add(ItemID.MoonlordTurretStaff);
                        break;
                }
                if (SafePostAddLootMelee().Count > 0 && SafePostAddLootRange().Count > 0 && SafePostAddLootMagic().Count > 0 && SafePostAddLootSummon().Count > 0 && SafePostAddLootMisc().Count > 0)
                {
                    DropItemMelee.AddRange(SafePostAddLootMelee());
                    DropItemRange.AddRange(SafePostAddLootRange());
                    DropItemMagic.AddRange(SafePostAddLootMagic());
                    DropItemSummon.AddRange(SafePostAddLootSummon());
                    DropItemMisc.AddRange(SafePostAddLootMisc());
                }
            }
        }
        /// <summary>
        ///      Allow user to return a list of number that contain different data to insert into chest <br/>
        ///      0 : Pre Boss <br/>
        ///      1 : Post EoC <br/>
        ///      2 : Post Evil boss <br/>
        ///      3 : Post Skeletron <br/>
        ///      4 : Post Queen bee <br/>
        ///      5 : Post Deerclop <br/>
        ///      6 : Post WoF <br/>
        ///      7 : Post Queen slime <br/>
        ///      8 : 1 mech boss loot <br/>
        ///      9 : Post all mech <br/>
        ///      10 : Post Plantera <br/>
        ///      11 : Post Golem <br/>
        ///      12 : Pre lunatic cultist ( EoL, Duke Fishron ) <br/>
        ///      13 : Post lunatic cultist <br/>
        ///      14 : Post Moon lord <br/>
        /// </summary>
        public virtual List<int> FlagNumber() => new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        /// <summary>
        /// Return a random weapon and if the weapon consumable then return many ammount
        /// </summary>
        /// <param name="player">Player player</param>
        /// <param name="ReturnWeapon">Weapon get return</param>
        /// <param name="specialAmount">Ammount of that weapon get return</param>
        /// <param name="rng">Set the rng number to return a specific type of weapon
        /// <br/>1 : Melee weapon
        /// <br/>2 : Range weapon
        /// <br/>3 : Magic weapon
        /// <br/> 4 : Summon weapon
        /// <br/>5 : Misc weapon
        /// <br/> 6 : Drug
        /// <br/> 7 : Rainbow Chest</param>
        public void GetWeapon(Player player, out int ReturnWeapon, out int specialAmount, int rng = 0)
        {
            specialAmount = 1;
            ReturnWeapon = 0;
            if (rng == 0)
            {
                if (meleeChance + rangeChance + magicChance + summonChance + specialChance >= 1)
                {
                    rng = RNGManage(meleeChance, rangeChance, magicChance, summonChance, specialChance);
                }
                else
                {
                    rng = RNGManage();
                }
                rng = ModifyRNG(rng, player);
            }
            //adding stuff here
            if (rng < 6 && rng > 0)
            {
                List<int> list = FlagNumber();
                AddLoot(list);
            }
            //actual choosing item
            switch (rng)
            {
                case 0:
                    ReturnWeapon = ItemID.None;
                    break;
                case 1:
                    ReturnWeapon = Main.rand.NextFromCollection(DropItemMelee);
                    break;
                case 2:
                    ReturnWeapon = Main.rand.NextFromCollection(DropItemRange);
                    break;
                case 3:
                    ReturnWeapon = Main.rand.NextFromCollection(DropItemMagic);
                    break;
                case 4:
                    ReturnWeapon = Main.rand.NextFromCollection(DropItemSummon);
                    break;
                case 5:
                    ReturnWeapon = Main.rand.NextFromCollection(DropItemMisc);
                    specialAmount += 199;
                    break;
                case 6:
                    ReturnWeapon = ModContent.ItemType<WonderDrug>();
                    break;
                case 7:
                    ReturnWeapon = ModContent.ItemType<RainbowTreasureChest>();
                    break;
            }
        }
        List<int> DropArrowAmmo = new List<int>();
        List<int> DropBulletAmmo = new List<int>();
        List<int> DropDartAmmo = new List<int>();

        int[] defaultArrow = new int[] { ItemID.WoodenArrow, ItemID.FlamingArrow, ItemID.FrostburnArrow, ItemID.JestersArrow, ItemID.UnholyArrow, ItemID.BoneArrow, ItemID.HellfireArrow };
        int[] ArrowHM = new int[] { ItemID.HolyArrow, ItemID.CursedArrow, ItemID.IchorArrow };

        int[] defaultBullet = new int[] { ItemID.MusketBall, ItemID.SilverBullet, ItemID.TungstenBullet };
        int[] BulletHM = new int[] { ItemID.CursedBullet, ItemID.IchorBullet, ItemID.GoldenBullet, ItemID.CrystalBullet, ItemID.HighVelocityBullet, ItemID.PartyBullet, ItemID.ExplodingBullet };

        int[] defaultDart = new int[] { ItemID.PoisonDart, ItemID.Seed };
        int[] DartHM = new int[] { ItemID.IchorDart, ItemID.CursedDart, ItemID.CrystalDart };

        /// <summary>
        /// Return ammo of weapon accordingly to weapon parameter
        /// </summary>
        /// <param name="Ammo">Return ammo type accordingly to weapon type</param>
        /// <param name="Amount">Return the ammount of ammo</param>
        /// <param name="weapon">Weapon need to be checked</param>
        /// <param name="AmountModifier">Modify the ammount of ammo will be given</param>
        public void AmmoForWeapon(out int Ammo, out int Amount, int weapon, float AmountModifier = 1)
        {
            Amount = (int)(200 * AmountModifier);
            Item weapontoCheck = new Item(weapon);

            DropArrowAmmo.Clear();
            DropBulletAmmo.Clear();
            DropDartAmmo.Clear();

            DropArrowAmmo.AddRange(defaultArrow);
            DropBulletAmmo.AddRange(defaultBullet);
            DropDartAmmo.AddRange(defaultDart);

            if (Main.hardMode)
            {
                DropArrowAmmo.AddRange(ArrowHM);
                DropBulletAmmo.AddRange(BulletHM);
                DropDartAmmo.AddRange(DartHM);
            }
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                DropArrowAmmo.Add(ItemID.ChlorophyteArrow);
                DropBulletAmmo.Add(ItemID.ChlorophyteBullet);
            }
            if (NPC.downedPlantBoss)
            {
                DropArrowAmmo.Add(ItemID.VenomArrow);
                DropBulletAmmo.Add(ItemID.NanoBullet);
                DropBulletAmmo.Add(ItemID.VenomBullet);
            }
            if (weapontoCheck.useAmmo == AmmoID.Arrow)
            {
                Ammo = Main.rand.NextFromCollection(DropArrowAmmo);
            }
            else if (weapontoCheck.useAmmo == AmmoID.Bullet)
            {
                Ammo = Main.rand.NextFromCollection(DropBulletAmmo);
            }
            else if (weapontoCheck.useAmmo == AmmoID.Dart)
            {
                Ammo = Main.rand.NextFromCollection(DropDartAmmo);
            }
            else if (weapontoCheck.mana > 0)
            {
                Ammo = ItemID.LesserManaPotion;
                Amount = (int)(10 * AmountModifier);
            }
            else if (weapontoCheck.useAmmo == AmmoID.Rocket)
            {
                Ammo = ItemID.RocketI;
            }
            else if (weapontoCheck.useAmmo == AmmoID.Snowball)
            {
                Ammo = ItemID.Snowball;
            }
            else if (weapontoCheck.useAmmo == AmmoID.CandyCorn)
            {
                Ammo = ItemID.CandyCorn;
            }
            else if (weapontoCheck.useAmmo == AmmoID.JackOLantern)
            {
                Ammo = ItemID.JackOLantern;
            }
            else if (weapontoCheck.useAmmo == AmmoID.Flare)
            {
                Ammo = ItemID.Flare;
            }
            else if (weapontoCheck.useAmmo == AmmoID.Stake)
            {
                Ammo = ItemID.Stake;
            }
            else if (weapontoCheck.useAmmo == AmmoID.StyngerBolt)
            {
                Ammo = ItemID.StyngerBolt;
            }
            else if (weapontoCheck.useAmmo == AmmoID.NailFriendly)
            {
                Ammo = ItemID.Nail;
            }
            else if (weapontoCheck.useAmmo == AmmoID.Gel)
            {
                Ammo = ItemID.Gel;
            }
            else if (weapontoCheck.useAmmo == AmmoID.FallenStar)
            {
                Ammo = ModContent.ItemType<StarTreasureChest>();
                if (Amount < 1)
                {
                    Amount = 1;
                }
                else if (Amount > 1)
                {
                    Amount = (int)(1 * AmountModifier);
                }
            }
            else
            {
                Ammo = ItemID.None;
            }
        }

        List<int> Accessories = new List<int>();

        int[] T1CombatAccessory = new int[] { ItemID.FeralClaws, ItemID.ObsidianSkull, ItemID.SharkToothNecklace, ItemID.WhiteString, ItemID.BlackCounterweight };
        int[] T1MovementAccessory = new int[] { ItemID.Aglet, ItemID.FlyingCarpet, ItemID.FrogLeg, ItemID.IceSkates, ItemID.ShoeSpikes, ItemID.ClimbingClaws, ItemID.FlurryBoots, ItemID.CloudinaBottle, ItemID.SandstorminaBottle, ItemID.BlizzardinaBottle, ItemID.Flipper, ItemID.AnkletoftheWind, ItemID.BalloonPufferfish, ItemID.TsunamiInABottle, ItemID.LuckyHorseshoe, ItemID.ShinyRedBalloon };
        int[] T1HealthAndManaAccessory = new int[] { ItemID.BandofRegeneration, ItemID.NaturesGift };

        int[] PostEvilCombatAccessory = new int[] { ItemID.MagmaStone, ItemID.ObsidianRose };
        int[] PostEvilMovementAccessory = new int[] { ItemID.LavaCharm, ItemID.Magiluminescence, ItemID.RocketBoots };
        int[] PostEvilHealthManaAccessory = new int[] { ItemID.BandofStarpower, ItemID.CelestialMagnet };

        int[] QueenBeeCombatAccessory = new int[] { ItemID.PygmyNecklace, ItemID.HoneyComb };

        int[] AnhkCharm = new int[] { ItemID.AdhesiveBandage, ItemID.Bezoar, ItemID.Vitamins, ItemID.ArmorPolish, ItemID.Blindfold, ItemID.PocketMirror, ItemID.Nazar, ItemID.Megaphone, ItemID.FastClock, ItemID.TrifoldMap };
        int[] HMAccessory = new int[] { ItemID.RangerEmblem, ItemID.SorcererEmblem, ItemID.SummonerEmblem, ItemID.WarriorEmblem, ItemID.StarCloak, ItemID.CrossNecklace, ItemID.YoYoGlove, ItemID.TitanGlove, ItemID.PutridScent, ItemID.FleshKnuckles };
        /// <summary>
        /// Return a random accessory 
        /// </summary>
        /// <param name="Accessory">Return a accessory</param>
        /// <param name="MovementAcc">allow for movement accessory to be include</param>
        /// <param name="CombatAcc">allow for accessory that combat oriented to be include</param>
        /// <param name="HealthManaAcc">allow for accessory that increase survivability to be include</param>
        /// <param name="AllowPreHMAcc">W.I.P</param>
        /// <param name="PriorityAnhkShield">only drop AnhkShield material</param>
        public void GetAccessory(out int Accessory, bool MovementAcc = true, bool CombatAcc = true, bool HealthManaAcc = true, bool AllowPreHMAcc = true, bool PriorityAnhkShield = false)
        {
            if (MovementAcc)
            {
                if (AllowPreHMAcc)
                {
                    Accessories.AddRange(T1MovementAccessory);
                    if (NPC.downedBoss2)
                    {
                        Accessories.AddRange(PostEvilMovementAccessory);
                    }
                }
            }
            if (CombatAcc)
            {
                if (AllowPreHMAcc)
                {
                    Accessories.AddRange(T1CombatAccessory);
                    if (NPC.downedBoss2)
                    {
                        Accessories.AddRange(PostEvilCombatAccessory);
                    }
                    if (NPC.downedBoss3)
                    {
                        Accessories.Add(ItemID.CobaltShield);
                    }
                    if (NPC.downedQueenBee)
                    {
                        Accessories.AddRange(QueenBeeCombatAccessory);
                    }
                }
                if (Main.hardMode)
                {
                    Accessories.AddRange(HMAccessory);
                }
            }
            if (HealthManaAcc)
            {
                if (AllowPreHMAcc)
                {
                    Accessories.AddRange(T1HealthAndManaAccessory);
                    if (NPC.downedBoss2)
                    {
                        Accessories.AddRange(PostEvilHealthManaAccessory);
                    }
                }
                if (Main.hardMode)
                {
                    Accessories.Add(ItemID.PhilosophersStone);
                }
            }
            if (Main.hardMode)
            {
                if (PriorityAnhkShield)
                {
                    Accessories.Clear();
                    Accessories.AddRange(AnhkCharm);
                }
            }
            Accessory = Main.rand.NextFromCollection(Accessories);
        }
        List<int> DropItemPotion = new List<int>();
        int[] NonMovementPotion = new int[] { ItemID.ArcheryPotion, ItemID.AmmoReservationPotion, ItemID.EndurancePotion, ItemID.HeartreachPotion, ItemID.IronskinPotion, ItemID.MagicPowerPotion, ItemID.RagePotion, ItemID.SummoningPotion, ItemID.WrathPotion, ItemID.RegenerationPotion, ItemID.TitanPotion, ItemID.ThornsPotion, ItemID.ManaRegenerationPotion };
        int[] MovementPotion = new int[] { ItemID.SwiftnessPotion, ItemID.FeatherfallPotion, ItemID.GravitationPotion, ItemID.WaterWalkingPotion };
        /// <summary>
        /// Return random potion
        /// </summary>
        /// <param name="Potion">Return potion type</param>
        /// <param name="AllowMovementPotion">Allow potion that enhance movement to be drop</param>
        public void GetPotion(out int Potion, bool AllowMovementPotion = false)
        {
            if (AllowMovementPotion)
            {
                DropItemPotion.AddRange(MovementPotion);
            }
            DropItemPotion.AddRange(NonMovementPotion);
            if (Main.hardMode)
            {
                DropItemPotion.Add(ItemID.LifeforcePotion);
                DropItemPotion.Add(ItemID.InfernoPotion);
            }
            Potion = Main.rand.NextFromCollection(DropItemPotion);
        }
    }
}