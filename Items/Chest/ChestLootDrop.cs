using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Items.Potion;
using BossRush.Items.Artifact;
using System.Collections.Generic;
using BossRush.Common;

namespace BossRush.Items.Chest
{
    public abstract class ChestLootDrop : ModItem
    {
        private List<int> DropItemMelee = new List<int>();
        private List<int> DropItemRange = new List<int>();
        private List<int> DropItemMagic = new List<int>();
        private List<int> DropItemSummon = new List<int>();
        private List<int> DropItemMisc = new List<int>();

        private int meleeChance;
        private int rangeChance;
        private int magicChance;
        private int summonChance;
        private int specialChance;

        private int ModifyGetAmount(int ValueToModify, Player player, bool multiplier = false)
        {
            //Modifier
            float amountToModify = player.GetModPlayer<ChestLootDropPlayer>().amountModifier;
            if (player.GetModPlayer<ModdedPlayer>().ArtifactAllowance)
            {
                if (player.HasItem(ModContent.ItemType<TokenofGreed>())) { amountToModify += 4; }
                if (ModContent.GetInstance<BossRushModConfig>().EasyMode) { amountToModify += 2; }
                if (player.HasItem(ModContent.ItemType<TokenofPride>())) { amountToModify = 0.5f; multiplier = true; }
            }
            //Change
            if (multiplier)
            {
                return amountToModify > 0 ? (int)Math.Ceiling(amountToModify * ValueToModify) : 0;
            }
            else
            {
                return ValueToModify + amountToModify > 0 ? (int)(amountToModify + ValueToModify) : 0;
            }
        }
        protected void GetAmount(out int amountForWeapon, out int amountForPotionType, out int amountForPotionNum, Player player)
        {
            amountForWeapon = 3;
            amountForPotionType = 1;
            amountForPotionNum = 2;
            if (Main.getGoodWorld)
            {
                amountForWeapon = 2;
                amountForPotionType = 1;
                amountForPotionNum = 1;
            }
            if (Main.hardMode)
            {
                amountForWeapon += 1;
                amountForPotionType += 1;
                amountForPotionNum += 1;
            }
            amountForWeapon = ModifyGetAmount(amountForWeapon, player);
            amountForPotionType = ModifyGetAmount(amountForPotionType, player);
            amountForPotionNum = ModifyGetAmount(amountForPotionNum, player);
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
            }
            return rng;
        }

        protected int RNGManage(int meleeChance = 20, int rangeChance = 25, int magicChance = 25, int summonChance = 15, int specialChance = 15)
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
        protected virtual List<int> SafePostAddLootMelee() => new List<int> { };

        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootRange() => new List<int> { };

        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootMagic() => new List<int> { };

        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootSummon() => new List<int> { };

        /// <summary>
        /// Allow for safely add extra loot, this will be called After all the loot is added
        /// </summary>
        protected virtual List<int> SafePostAddLootMisc() => new List<int> { };
        private void AddLoot(List<int> FlagNumber)
        {
            List<int> RNGchooseWhichTierToGet = FlagNumber.SetUpRNGTier();
            RNGchooseWhichTierToGet = RNGchooseWhichTierToGet.RemoveDupeInArray();
            RNGchooseWhichTierToGet = RNGchooseWhichTierToGet.OrderFromSmallest();
            for (int i = 0; i < RNGchooseWhichTierToGet.Count; ++i)
            {
                switch (FlagNumber[i])
                {
                    case 0://PreBoss
                        DropItemMelee.AddRange(TerrariaArrayID.MeleePreBoss);
                        DropItemRange.AddRange(TerrariaArrayID.RangePreBoss);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicPreBoss);
                        DropItemSummon.AddRange(TerrariaArrayID.SummonPreBoss);
                        DropItemMisc.AddRange(TerrariaArrayID.SpecialPreBoss);
                        break;
                    case 1://PreEoC
                        DropItemMelee.AddRange(TerrariaArrayID.MeleePreEoC);
                        DropItemRange.AddRange(TerrariaArrayID.RangePreEoC);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicPreEoC);
                        DropItemSummon.AddRange(TerrariaArrayID.SummonerPreEoC);
                        DropItemMisc.AddRange(TerrariaArrayID.Special);
                        break;
                    case 2://EoC
                        DropItemMelee.Add(ItemID.Code1);
                        DropItemMagic.Add(ItemID.ZapinatorGray);
                        break;
                    case 3://Evil boss
                        DropItemMelee.AddRange(TerrariaArrayID.MeleeEvilBoss);
                        DropItemRange.Add(ItemID.MoltenFury);
                        DropItemRange.Add(ItemID.StarCannon);
                        DropItemRange.Add(ItemID.AleThrowingGlove);
                        DropItemRange.Add(ItemID.Harpoon);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicEvilBoss);
                        DropItemSummon.Add(ItemID.ImpStaff);
                        break;
                    case 4://Skeletron
                        DropItemMelee.AddRange(TerrariaArrayID.MeleeSkel);
                        DropItemRange.AddRange(TerrariaArrayID.RangeSkele);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicSkele);
                        DropItemSummon.AddRange(TerrariaArrayID.SummonSkele);
                        break;
                    case 5://Queen bee
                        DropItemMelee.Add(ItemID.BeeKeeper);
                        DropItemRange.Add(ItemID.BeesKnees); DropItemRange.Add(ItemID.Blowgun);
                        DropItemMagic.Add(ItemID.BeeGun);
                        DropItemSummon.Add(ItemID.HornetStaff);
                        DropItemMisc.Add(ItemID.Beenade);
                        break;
                    case 6://Deerclop
                        DropItemRange.Add(ItemID.PewMaticHorn);
                        DropItemMagic.Add(ItemID.WeatherPain);
                        DropItemSummon.Add(ItemID.HoundiusShootius);
                        break;
                    case 7://Wall of flesh
                        DropItemMelee.AddRange(TerrariaArrayID.MeleeHM);
                        DropItemRange.AddRange(TerrariaArrayID.RangeHM);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicHM);
                        DropItemSummon.AddRange(TerrariaArrayID.SummonHM);
                        break;
                    case 8://Queen slime
                        DropItemMelee.AddRange(TerrariaArrayID.MeleeQS);
                        DropItemSummon.Add(ItemID.Smolstar);
                        break;
                    case 9://First mech
                        DropItemMelee.AddRange(TerrariaArrayID.MeleeMech);
                        DropItemRange.Add(ItemID.SuperStarCannon);
                        DropItemRange.Add(ItemID.DD2PhoenixBow);
                        DropItemMagic.Add(ItemID.UnholyTrident);
                        break;
                    case 10://All three mech
                        DropItemMelee.AddRange(TerrariaArrayID.MeleePostAllMechs);
                        DropItemRange.AddRange(TerrariaArrayID.RangePostAllMech);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicPostAllMech);
                        DropItemSummon.AddRange(TerrariaArrayID.SummonPostAllMech);
                        break;
                    case 11://Plantera
                        DropItemMelee.AddRange(TerrariaArrayID.MeleePostPlant);
                        DropItemRange.AddRange(TerrariaArrayID.RangePostPlant);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicPostPlant);
                        DropItemSummon.AddRange(TerrariaArrayID.SummonPostPlant);
                        break;
                    case 12://Golem
                        DropItemMelee.AddRange(TerrariaArrayID.MeleePostGolem);
                        DropItemRange.AddRange(TerrariaArrayID.RangePostGolem);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicPostGolem);
                        DropItemSummon.AddRange(TerrariaArrayID.SummonPostGolem);
                        break;
                    case 13://Pre lunatic (Duke fishron, EoL, ect)
                        DropItemMelee.AddRange(TerrariaArrayID.MeleePreLuna);
                        DropItemRange.AddRange(TerrariaArrayID.RangePreLuna);
                        DropItemMagic.AddRange(TerrariaArrayID.MagicPreLuna);
                        DropItemSummon.AddRange(TerrariaArrayID.SummonPreLuna);
                        break;
                    case 14://Lunatic Cultist
                        DropItemMelee.Add(ItemID.DayBreak);
                        DropItemMelee.Add(ItemID.SolarEruption);
                        DropItemRange.Add(ItemID.Phantasm);
                        DropItemRange.Add(ItemID.VortexBeater);
                        DropItemMagic.Add(ItemID.NebulaArcanum);
                        DropItemMagic.Add(ItemID.NebulaBlaze);
                        DropItemSummon.Add(ItemID.StardustCellStaff);
                        DropItemSummon.Add(ItemID.StardustDragonStaff);
                        break;
                    case 15://MoonLord
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
                if (SafePostAddLootMelee().Count > 0) DropItemMelee.AddRange(SafePostAddLootMelee());
                if (SafePostAddLootRange().Count > 0) DropItemRange.AddRange(SafePostAddLootRange());
                if (SafePostAddLootMagic().Count > 0) DropItemMagic.AddRange(SafePostAddLootMagic());
                if (SafePostAddLootSummon().Count > 0) DropItemSummon.AddRange(SafePostAddLootSummon());
                if (SafePostAddLootMisc().Count > 0) DropItemMisc.AddRange(SafePostAddLootMisc());
            }
        }
        /// <summary>
        ///      Allow user to return a list of number that contain different data to insert into chest <br/>
        ///      0 : Pre Boss <br/>
        ///      1 : Pre EoC <br/>
        ///      2 : Post EoC <br/>
        ///      3 : Post Evil boss <br/>
        ///      4 : Post Skeletron <br/>
        ///      5 : Post Queen bee <br/>
        ///      6 : Post Deerclop <br/>
        ///      7 : Post WoF <br/>
        ///      8 : Post Queen slime <br/>
        ///      9 : 1 mech boss loot <br/>
        ///      10 : Post all mech <br/>
        ///      11 : Post Plantera <br/>
        ///      12 : Post Golem <br/>
        ///      13 : Pre lunatic cultist ( EoL, Duke Fishron ) <br/>
        ///      14 : Post lunatic cultist <br/>
        ///      15 : Post Moon lord <br/>
        /// </summary>
        public virtual List<int> FlagNumber() => new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
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
            ReturnWeapon = ItemID.None;
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
                AddLoot(FlagNumber());
            }
            //actual choosing item
            ChooseWeapon(rng, player,ref ReturnWeapon,ref specialAmount);
        }

        public void ChooseWeapon(int rng, Player player,ref int weapon, ref int amount)
        {
            switch (rng)
            {
                case 0:
                    weapon = ItemID.None;
                    break;
                case 1:
                    weapon = Main.rand.NextFromCollection(DropItemMelee);
                    break;
                case 2:
                    weapon = Main.rand.NextFromCollection(DropItemRange);
                    break;
                case 3:
                    weapon = Main.rand.NextFromCollection(DropItemMagic);
                    break;
                case 4:
                    weapon = Main.rand.NextFromCollection(DropItemSummon);
                    break;
                case 5:
                    if (DropItemMisc.Count < 1)
                    {
                        int rngM = ModifyRNG(Main.rand.Next(1, 5), player);
                        ChooseWeapon(rngM, player, ref weapon,ref amount);
                        break;
                    }
                    amount += 199;
                    weapon = Main.rand.NextFromCollection(DropItemMisc);
                    break;
                case 6:
                    weapon = ModContent.ItemType<WonderDrug>();
                    break;
                case 7:
                    weapon = ModContent.ItemType<RainbowTreasureChest>();
                    break;
            }
        }

        List<int> DropArrowAmmo = new List<int>();
        List<int> DropBulletAmmo = new List<int>();
        List<int> DropDartAmmo = new List<int>();

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

            DropArrowAmmo.AddRange(TerrariaArrayID.defaultArrow);
            DropBulletAmmo.AddRange(TerrariaArrayID.defaultBullet);
            DropDartAmmo.AddRange(TerrariaArrayID.defaultDart);

            if (Main.hardMode)
            {
                DropArrowAmmo.AddRange(TerrariaArrayID.ArrowHM);
                DropBulletAmmo.AddRange(TerrariaArrayID.BulletHM);
                DropDartAmmo.AddRange(TerrariaArrayID.DartHM);
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
        /// <summary>
        ///      Allow user to return a list of number that contain different data to insert into chest <br/>
        ///      0 : Tier 1 Combat acc <br/>
        ///      1 : Tier 1 Health and Mana acc<br/>
        ///      2 : Tier 1 Movement acc<br/>
        ///      3 : Post evil Combat acc<br/>
        ///      4 : Post evil Health and Mana acc<br/>
        ///      5 : Post evil Movement acc<br/>
        ///      6 : Queen bee acc<br/>
        ///      7 : Cobalt Shield<br/>
        ///      8 : Anhk shield sub acc (not include the shield itself)<br/>
        ///      9 : Hardmode acc<br/>
        ///      10 : PhilosophersStone<br/>
        /// </summary>
        public virtual List<int> FlagNumAcc() => new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        /// <summary>
        ///      Allow user to return a list of number that contain different data to insert into chest <br/>
        ///      0 : Tier 1 Combat acc <br/>
        ///      1 : Tier 1 Health and Mana acc<br/>
        ///      2 : Tier 1 Movement acc<br/>
        ///      3 : Post evil Combat acc<br/>
        ///      4 : Post evil Health and Mana acc<br/>
        ///      5 : Post evil Movement acc<br/>
        ///      6 : Queen bee acc<br/>
        ///      7 : Cobalt Shield<br/>
        ///      8 : Anhk shield sub acc (not include the shield itself)<br/>
        ///      9 : Hardmode acc<br/>
        ///      10 : PhilosophersStone<br/>
        /// </summary>
        public virtual List<int> FlagNumAcc(List<int> listofNum) => listofNum;

        /// <summary>
        /// Allow for safely add in a list of accessory that specific to a situation
        /// </summary>
        /// <returns></returns>
        public virtual List<int> SafePostAddAcc() => new List<int>() { };

        private void addAcc(List<int> flag)
        {
            for (int i = 0; i < flag.Count; i++)
            {
                switch (flag[i])
                {
                    case 0:
                        Accessories.AddRange(TerrariaArrayID.T1CombatAccessory);
                        break;
                    case 1:
                        Accessories.AddRange(TerrariaArrayID.T1HealthAndManaAccessory);
                        break;
                    case 2:
                        Accessories.AddRange(TerrariaArrayID.T1MovementAccessory);
                        break;
                    case 3:
                        Accessories.AddRange(TerrariaArrayID.PostEvilCombatAccessory);
                        break;
                    case 4:
                        Accessories.AddRange(TerrariaArrayID.PostEvilHealthManaAccessory);
                        break;
                    case 5:
                        Accessories.AddRange(TerrariaArrayID.PostEvilMovementAccessory);
                        break;
                    case 6:
                        Accessories.AddRange(TerrariaArrayID.QueenBeeCombatAccessory);
                        break;
                    case 7:
                        Accessories.Add(ItemID.CobaltShield);
                        break;
                    case 8:
                        Accessories.AddRange(TerrariaArrayID.AnhkCharm);
                        break;
                    case 9:
                        Accessories.AddRange(TerrariaArrayID.HMAccessory);
                        break;
                    case 10:
                        Accessories.Add(ItemID.PhilosophersStone);
                        break;
                }
                if (SafePostAddAcc().Count > 0) Accessories.AddRange(SafePostAddAcc());
            }
        }

        /// <summary>
        /// Return a random accessory 
        /// </summary>
        public int GetAccessory()
        {
            addAcc(FlagNumAcc());
            return Main.rand.NextFromCollection(Accessories);
        }
        List<int> DropItemPotion = new List<int>();
        /// <summary>
        /// Return random potion
        /// </summary>
        /// <param name="MovementPotionOnly">Allow potion that enhance movement to be drop</param>
        public int GetPotion(bool MovementPotionOnly = false)
        {
            DropItemPotion.AddRange(TerrariaArrayID.NonMovementPotion);
            if (Main.hardMode)
            {
                DropItemPotion.Add(ItemID.LifeforcePotion);
                DropItemPotion.Add(ItemID.InfernoPotion);
            }
            if (MovementPotionOnly)
            {
                DropItemPotion.Clear();
            }
            DropItemPotion.AddRange(TerrariaArrayID.MovementPotion);
            return Main.rand.NextFromCollection(DropItemPotion);
        }
    }
    public class ChestLootDropPlayer : ModPlayer
    {
        public float amountModifier = 0;
        public override void ResetEffects()
        {
            amountModifier = 0;
            base.ResetEffects();
        }
    }
}