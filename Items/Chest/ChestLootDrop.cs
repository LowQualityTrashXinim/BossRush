using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Items.Artifact;
using System;
using BossRush.Items.Potion;
using System.Linq;
using System.Xml.Serialization;

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
            if (FlagNumber.Count > 1)
            {
                FlagNumber = FlagNumber.OrderFromSmallest();
            }
            List<int> RNGchooseWhichTierToGet = FlagNumber.SetUpRNGTier();
            List<int> newList = RNGchooseWhichTierToGet.RemoveDupeInArray();
            newList = newList.OrderFromSmallest();
            for (int i = 0; i < newList.Count; ++i)
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
            ReturnWeapon = ChooseWeapon(rng, player);
        }

        public int ChooseWeapon(int rng, Player player)
        {
            switch (rng)
            {
                case 0:
                    return ItemID.None;
                case 1:
                    return Main.rand.NextFromCollection(DropItemMelee);
                case 2:
                    return Main.rand.NextFromCollection(DropItemRange);
                case 3:
                    return Main.rand.NextFromCollection(DropItemMagic);
                case 4:
                    return Main.rand.NextFromCollection(DropItemSummon);
                case 5:
                    if (DropItemMisc.Count < 1)
                    {
                        int rngM = ModifyRNG(Main.rand.Next(1,5), player);
                        return ChooseWeapon(rngM, player);
                    }
                    return Main.rand.NextFromCollection(DropItemMisc);
                case 6:
                    return ModContent.ItemType<WonderDrug>();
                case 7:
                    return ModContent.ItemType<RainbowTreasureChest>();
            }
            return ItemID.None;
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
    static class ChestLootDropUtils
    {
        public static List<int> OrderFromSmallest(this List<int> flag)
        {
            List<int> finalflag = flag;
            for (int i = 0; i < flag.Count; ++i)
            {
                for (int l = i + 1; l < flag.Count; ++l)
                {
                    if (flag[i] > flag[l])
                    {
                        int CurrentIndexNum = finalflag[i];
                        finalflag[i] = flag[l];
                        finalflag[l] = CurrentIndexNum;
                    }
                }
            }
            return finalflag;
        }
        public static List<int> SetUpRNGTier(this List<int> FlagNum)
        {
            if (FlagNum.Count < 2)
            {
                return FlagNum;
            }
            List<int> FlagNumNew = new List<int> { FlagNum[0] };
            float GetOnePercentChance = 100 / (float)FlagNum.Count;
            for (int i = 1; i < FlagNum.Count; ++i)
            {
                if (Main.rand.Next(101) < GetOnePercentChance * i)
                {
                    FlagNumNew.Add(FlagNum[FlagNum.Count - i]);
                }
            }
            return FlagNumNew;
        }
        public static List<int> RemoveDupeInArray(this List<int> flag)
        {
            List<int> listArray = new List<int>();
            listArray.AddRange(flag);
            List<int> listofIndexWhereDupe = new List<int>();
            for (int i = 0; i < flag.Count; ++i)
            {
                for (int l = i + 1; l < flag.Count; ++l)
                {
                    if (listArray[i] == flag[l])
                    {
                        listofIndexWhereDupe.Add(i);
                    }
                }
            }
            for (int i = listofIndexWhereDupe.Count - 1; i > -1; --i)
            {
                listArray.RemoveAt(listofIndexWhereDupe[i]);
            }
            return listArray;
        }
    }
    public static class TerrariaArrayID
    {
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
        public readonly static int[] MagicPreBoss = { ItemID.AmethystStaff, ItemID.TopazStaff, ItemID.SapphireStaff, ItemID.EmeraldStaff, ItemID.RubyStaff, ItemID.DiamondStaff };
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
                ItemID.WandofSparking,ItemID.ThunderStaff, ItemID.AmberStaff
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
        public readonly static int[] RangePostPlant = { ItemID.ChainGun, ItemID.SnowmanCannon, ItemID.EldMelter, ItemID.PulseBow, ItemID.VenusMagnum, ItemID.TacticalShotgun, ItemID.SniperRifle, ItemID.GrenadeLauncher, ItemID.ProximityMineLauncher, ItemID.RocketLauncher, ItemID.NailGun, ItemID.PiranhaGun, ItemID.JackOLanternLauncher, ItemID.StakeLauncher, ItemID.CandyCornRifle };
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
        public readonly static int[] T1MovementAccessory =  { ItemID.Aglet, ItemID.FlyingCarpet, ItemID.FrogLeg, ItemID.IceSkates, ItemID.ShoeSpikes, ItemID.ClimbingClaws, ItemID.HermesBoots, ItemID.AmphibianBoots, ItemID.FlurryBoots, ItemID.CloudinaBottle, ItemID.SandstorminaBottle, ItemID.BlizzardinaBottle, ItemID.Flipper, ItemID.AnkletoftheWind, ItemID.BalloonPufferfish, ItemID.TsunamiInABottle, ItemID.LuckyHorseshoe, ItemID.ShinyRedBalloon };
        public readonly static int[] T1HealthAndManaAccessory = { ItemID.BandofRegeneration, ItemID.NaturesGift };

        public readonly static int[] PostEvilCombatAccessory = { ItemID.MagmaStone, ItemID.ObsidianRose };
        public readonly static int[] PostEvilMovementAccessory =  { ItemID.LavaCharm, ItemID.Magiluminescence, ItemID.RocketBoots };
        public readonly static int[] PostEvilHealthManaAccessory = { ItemID.BandofStarpower, ItemID.CelestialMagnet };

        public readonly static int[] QueenBeeCombatAccessory = { ItemID.PygmyNecklace, ItemID.HoneyComb };

        public readonly static int[] AnhkCharm = { ItemID.AdhesiveBandage, ItemID.Bezoar, ItemID.Vitamins, ItemID.ArmorPolish, ItemID.Blindfold, ItemID.PocketMirror, ItemID.Nazar, ItemID.Megaphone, ItemID.FastClock, ItemID.TrifoldMap };
        public readonly static int[] HMAccessory = { ItemID.RangerEmblem, ItemID.SorcererEmblem, ItemID.SummonerEmblem, ItemID.WarriorEmblem, ItemID.StarCloak, ItemID.CrossNecklace, ItemID.YoYoGlove, ItemID.TitanGlove, ItemID.PutridScent, ItemID.FleshKnuckles };

        public readonly static int[] DustGem = { DustID.GemDiamond, DustID.GemAmber, DustID.GemAmethyst, DustID.GemEmerald, DustID.GemRuby, DustID.GemSapphire, DustID.GemTopaz };
    }
}