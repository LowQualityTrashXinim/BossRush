using System;
using Terraria;
using System.IO;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Common.Utils;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Artifact;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.Accessories;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Chest
{
    public abstract class LootBoxBase : ModItem
    {
        private List<int> DropItemMelee = new List<int>();
        private List<int> DropItemRange = new List<int>();
        private List<int> DropItemMagic = new List<int>();
        private List<int> DropItemSummon = new List<int>();
        private List<int> DropItemMisc = new List<int>();
        private List<int> AllOfLootPossiblity = new List<int>();
        public virtual bool CanLootRNGbeRandomize() => true;
        public virtual bool ChestUseOwnLogic() => false;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
            if (!ChestUseOwnLogic())
            {
                if (AllOfLootPossiblity.Count < 1)
                {
                    AddLoot(FlagNumber());
                }
                chestplayer.GetAmount();
                List<int> potiontotal = new List<int>();
                potiontotal.AddRange(TerrariaArrayID.NonMovementPotion);
                potiontotal.AddRange(TerrariaArrayID.MovementPotion);
                TooltipLine chestline = new TooltipLine(Mod, "ChestLoot",
                    $"Weapon : [i:{Main.rand.Next(AllOfLootPossiblity)}] x {chestplayer.weaponAmount}\n" +
                    $"Potion type : [i:{Main.rand.Next(potiontotal)}] x {chestplayer.potionTypeAmount}\n" +
                    $"Amount of potion : [i:{ItemID.RegenerationPotion}][i:{ItemID.SwiftnessPotion}][i:{ItemID.IronskinPotion}] x {chestplayer.potionNumAmount}");
                tooltips.Add(chestline);
            }
            if (Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == ArtifactPlayerHandleLogic.ArtifactDefaultID)
            {
                TooltipLine line = new TooltipLine(Mod, "ArtifactBlock", "You must uses at least 1 artifact to open the chest");
                line.OverrideColor = BossRushUtils.MultiColor(new List<Color> { Color.DarkRed, Color.Red }, 10);
                tooltips.Add(line);
            }
            PostModifyTooltips(ref tooltips);
        }
        public virtual void PostModifyTooltips(ref List<TooltipLine> tooltips) { }
        private int ModifyRNG(int rng, Player player)
        {
            int DrugValue = player.GetModPlayer<WonderDrugPlayer>().DrugDealer;
            if (DrugValue > 0)
            {
                if (Main.rand.Next(100 + DrugValue * 5) <= DrugValue * 10)
                {
                    return 6;
                }
            }
            return rng;
        }
        protected int RNGManage(Player player, int meleeChance = 20, int rangeChance = 25, int magicChance = 25, int summonChance = 15, int specialChance = 15)
        {
            ChestLootDropPlayer modPlayer = player.GetModPlayer<ChestLootDropPlayer>();
            meleeChance = (int)(modPlayer.MeleeChanceMutilplier * meleeChance);
            rangeChance = (int)(modPlayer.RangeChanceMutilplier * rangeChance);
            magicChance = (int)(modPlayer.MagicChanceMutilplier * magicChance);
            summonChance = (int)(modPlayer.SummonChanceMutilplier * summonChance);
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
        public static List<int> SetUpRNGTier(List<int> FlagNum)
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
                    FlagNumNew.Add(FlagNum[^i]);//get element from the last of the list
                }
            }
            return FlagNumNew;
        }
        private void AddLoot(List<int> flagNumber)
        {
            if (CanLootRNGbeRandomize())
            {
                flagNumber = SetUpRNGTier(flagNumber);
                flagNumber = flagNumber.RemoveDupeInList();
                flagNumber.Sort();
            }
            for (int i = 0; i < flagNumber.Count; ++i)
            {
                switch (flagNumber[i])
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
            }
            if (SafePostAddLootMelee().Count > 0) DropItemMelee.AddRange(SafePostAddLootMelee());
            if (SafePostAddLootRange().Count > 0) DropItemRange.AddRange(SafePostAddLootRange());
            if (SafePostAddLootMagic().Count > 0) DropItemMagic.AddRange(SafePostAddLootMagic());
            if (SafePostAddLootSummon().Count > 0) DropItemSummon.AddRange(SafePostAddLootSummon());
            if (SafePostAddLootMisc().Count > 0) DropItemMisc.AddRange(SafePostAddLootMisc());
            AllOfLootPossiblity.AddRange(DropItemMagic);
            AllOfLootPossiblity.AddRange(DropItemRange);
            AllOfLootPossiblity.AddRange(DropItemMagic);
            AllOfLootPossiblity.AddRange(DropItemSummon);
            AllOfLootPossiblity.AddRange(DropItemMisc);
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
                rng = RNGManage(player);
            }
            rng = ModifyRNG(rng, player);
            //adding stuff here
            if (rng < 6 && rng > 0)
            {
                AddLoot(FlagNumber());
            }
            //actual choosing item
            ChooseWeapon(rng, ref ReturnWeapon, ref specialAmount);
        }
        /// <summary>
        /// Use this to add condition to chest
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBeRightClick() => false;
        public override bool CanRightClick() => Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID != ArtifactPlayerHandleLogic.ArtifactDefaultID || CanBeRightClick();

        public override void RightClick(Player player)
        {
            player.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen++;
            base.RightClick(player);
            OnRightClick(player, player.GetModPlayer<ChestLootDropPlayer>());
            var entitySource = player.GetSource_OpenItem(Type);
            if (ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                if (Main.rand.NextBool(1000))
                {
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<RainbowTreasureChest>());
                }
                if (Main.rand.NextBool(1000))
                {
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<EmblemofProgress>());
                }
            }
            player.QuickSpawnItem(entitySource, ModContent.ItemType<EmptyCard>());
            //Card dropping
            if (!ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                return;
            }
            if (player.IsDebugPlayer() && player.GetModPlayer<ChestLootDropPlayer>().potionNumAmount > 0)
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<MysteriousPotion>(), player.GetModPlayer<ChestLootDropPlayer>().potionNumAmount);
            }
            if (Main.rand.NextBool(25) || (player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == 7 && Main.rand.NextBool(7)))
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<BigCardPacket>());
                return;
            }
            if (Main.rand.NextBool(15) || (player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == 7 && Main.rand.NextBool(4)))
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<BigCardPacket>());
                return;
            }
            if (Main.rand.NextBool(7) || (player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == 7))
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<CardPacket>());
                return;
            }
        }
        public virtual void OnRightClick(Player player, ChestLootDropPlayer modplayer) { }
        public void ChooseWeapon(int rng, ref int weapon, ref int amount)
        {
            weapon = ItemID.None;
            amount = 1;
            switch (rng)
            {
                case 0:
                    weapon = ItemID.None;
                    return;
                case 1:
                    weapon = Main.rand.NextFromCollection(DropItemMelee);
                    return;
                case 2:
                    weapon = Main.rand.NextFromCollection(DropItemRange);
                    return;
                case 3:
                    weapon = Main.rand.NextFromCollection(DropItemMagic);
                    return;
                case 4:
                    weapon = Main.rand.NextFromCollection(DropItemSummon);
                    return;
                case 5:
                    if (DropItemMisc.Count < 1)
                    {
                        ChooseWeapon(Main.rand.Next(1, 5), ref weapon, ref amount);
                        break;
                    }
                    amount += 199;
                    if (Main.masterMode)
                    {
                        amount += 300;
                    }
                    weapon = Main.rand.NextFromCollection(DropItemMisc);
                    return;
                case 6:
                    weapon = ModContent.ItemType<WonderDrug>();
                    return;
            }
        }
        public static void GetWeapon(out int ReturnWeapon, out int Amount, int rng = 0)
        {
            if (rng > 6 || rng <= 0)
            {
                rng = Main.rand.Next(1, 6);
            }
            ReturnWeapon = 0;
            Amount = 1;
            List<int> DropItemMelee = new List<int>();
            List<int> DropItemRange = new List<int>();
            List<int> DropItemMagic = new List<int>();
            List<int> DropItemSummon = new List<int>();
            List<int> DropItemMisc = new List<int>();
            List<int> list = new() { 0 };
            if (NPC.downedSlimeKing)
            {
                list.Add(1);
            }
            if (NPC.downedBoss1)
            {
                list.Add(2);
            }
            if (NPC.downedBoss2)
            {
                list.Add(3);
            }
            if (NPC.downedBoss3)
            {
                list.Add(4);
            }
            if (NPC.downedQueenBee)
            {
                list.Add(5);
            }
            if (NPC.downedDeerclops)
            {
                list.Add(6);
            }
            if (Main.hardMode)
            {
                list.Add(7);
            }
            if (NPC.downedQueenSlime)
            {
                list.Add(8);
            }
            if (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3)
            {
                list.Add(9);
            }
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                list.Add(10);
            }
            if (NPC.downedPlantBoss)
            {
                list.Add(11);
            }
            if (NPC.downedGolemBoss)
            {
                list.Add(12);
            }
            if (NPC.downedEmpressOfLight)
            {
                list.Add(13);
            }
            if (NPC.downedAncientCultist)
            {
                list.Add(14);
            }
            if (NPC.downedMoonlord)
            {
                list.Add(15);
            }
            if (rng < 6 && rng > 0)
            {
                AddLoot(list, DropItemMelee, DropItemRange, DropItemMagic, DropItemSummon, DropItemMisc);
            }
            ChooseWeapon(rng, ref ReturnWeapon, ref Amount, DropItemMelee, DropItemRange, DropItemMagic, DropItemSummon, DropItemMisc);
        }
        private static void AddLoot(List<int> FlagNumber, List<int> DropItemMelee, List<int> DropItemRange, List<int> DropItemMagic, List<int> DropItemSummon, List<int> DropItemMisc)
        {
            for (int i = 0; i < FlagNumber.Count; ++i)
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
            }
        }
        private static void ChooseWeapon(int rng, ref int weapon, ref int amount, List<int> DropItemMelee, List<int> DropItemRange, List<int> DropItemMagic, List<int> DropItemSummon, List<int> DropItemMisc)
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
                        int rngM = Main.rand.Next(1, 5);
                        ChooseWeapon(rngM, ref weapon, ref amount, DropItemMelee, DropItemRange, DropItemMagic, DropItemSummon, DropItemMisc);
                        break;
                    }
                    amount += 199;
                    weapon = Main.rand.NextFromCollection(DropItemMisc);
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
            if (Main.masterMode)
            {
                Amount = (int)(Amount * 2.5f);
            }
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
            else if (weapontoCheck.useAmmo == AmmoID.Sand)
            {
                Ammo = ItemID.SandBlock;
            }
            else
            {
                Ammo = ItemID.WoodenArrow;
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

        private void AddAcc(List<int> flag)
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
        /// This happen automatically for the sake of prototype
        /// </summary>
        public void GetArmorForPlayer(IEntitySource entitySource, Player player)
        {
            List<int> HeadArmor = new List<int>();
            List<int> BodyArmor = new List<int>();
            List<int> LegArmor = new List<int>();
            HeadArmor.AddRange(TerrariaArrayID.HeadArmorPreBoss);
            BodyArmor.AddRange(TerrariaArrayID.BodyArmorPreBoss);
            LegArmor.AddRange(TerrariaArrayID.LegArmorPreBoss);
            if (NPC.downedBoss2)
            {
                HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostEvil);
                BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostEvil);
                LegArmor.AddRange(TerrariaArrayID.LegArmorPostEvil);
            }
            if (NPC.downedBoss3)
            {
                HeadArmor.Add(ItemID.NecroHelmet);
                BodyArmor.Add(ItemID.NecroBreastplate);
                LegArmor.Add(ItemID.NecroGreaves);
            }
            if (NPC.downedQueenBee)
            {
                HeadArmor.Add(ItemID.BeeHeadgear);
                BodyArmor.Add(ItemID.BeeBreastplate);
                LegArmor.Add(ItemID.BeeGreaves);
            }
            int[] fullbodyarmor = new int[]{
                Main.rand.Next(HeadArmor),
                Main.rand.Next(BodyArmor),
                Main.rand.Next(LegArmor) };
            for (int i = 0; i < fullbodyarmor.Length; i++)
            {
                player.QuickSpawnItem(entitySource, fullbodyarmor[i]);
            }
        }
        /// <summary>
        /// Return a random accessory 
        /// </summary>
        public int GetAccessory()
        {
            AddAcc(FlagNumAcc());
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
        Color color1, color2, color3, color4;
        private void ColorHandle()
        {
            color1 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 20);
            color2 = new Color(Main.DiscoG, Main.DiscoB, Main.DiscoR, 20);
            color3 = new Color(Main.DiscoB, Main.DiscoR, Main.DiscoG, 20);
            color4 = new Color(Main.DiscoG, Main.DiscoR, Main.DiscoB, 20);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            ColorHandle();
            Main.instance.LoadItem(Item.type);
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(texture, position + new Vector2(2, 2), null, color1, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(-2, 2), null, color2, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(2, -2), null, color3, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(-2, -2), null, color4, 0, origin, scale, SpriteEffects.None, 0);
            }
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            ColorHandle();
            //if (Item.whoAmI != whoAmI)
            //{
            //    return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            //}
            Main.instance.LoadItem(Item.type);
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Item.position - Main.screenPosition + origin + origin * .5f;
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(texture, drawPos + new Vector2(2, 2), null, color1, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(-2, 2), null, color2, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(2, -2), null, color3, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(-2, -2), null, color4, rotation, origin, scale, SpriteEffects.None, 0);
            }
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
    }
    public class ChestLootDropPlayer : ModPlayer
    {
        public int CurrentSectionAmountOfChestOpen = 0;
        public float finalMultiplier = 1f;
        public int amountModifier = 0;
        public int weaponAmount;
        public int potionTypeAmount;
        public int potionNumAmount;
        public float MeleeChanceMutilplier = 1f;
        public float RangeChanceMutilplier = 1f;
        public float MagicChanceMutilplier = 1f;
        public float SummonChanceMutilplier = 1f;
        private int ModifyGetAmount(int ValueToModify) => finalMultiplier > 0 ? (int)Math.Ceiling(finalMultiplier * (ValueToModify + amountModifier)) : 0;
        /// <summary>
        /// This must be called before using
        /// <br/><see cref="weaponAmount"/>
        /// <br/><see cref="potionTypeAmount"/>
        /// <br/><see cref="potionNumAmount"/>
        /// </summary>
        public void GetAmount()
        {
            weaponAmount = 5;
            potionTypeAmount = 3;
            potionNumAmount = 4;
            if (ModContent.GetInstance<BossRushModConfig>().VeteranMode)
            {
                weaponAmount -= 2;
                potionTypeAmount -= 2;
                potionNumAmount -= 2;
            }
            if (Main.getGoodWorld)
            {
                weaponAmount = 2;
            }
            if (Main.hardMode)
            {
                weaponAmount += 1;
                potionTypeAmount += 1;
                potionNumAmount += 1;
            }
            weaponAmount = ModifyGetAmount(weaponAmount);
            potionTypeAmount = ModifyGetAmount(potionTypeAmount);
            potionNumAmount = ModifyGetAmount(potionNumAmount);
        }
        public override void ResetEffects()
        {
            amountModifier = 0;
            finalMultiplier = 1f;
            base.ResetEffects();
        }
        public override void Initialize()
        {
            MeleeChanceMutilplier = 1f;
            RangeChanceMutilplier = 1f;
            MagicChanceMutilplier = 1f;
            SummonChanceMutilplier = 1f;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRush.MessageType.ChanceMultiplayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(MeleeChanceMutilplier);
            packet.Write(RangeChanceMutilplier);
            packet.Write(MagicChanceMutilplier);
            packet.Write(SummonChanceMutilplier);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["MeleeChanceMulti"] = MeleeChanceMutilplier;
            tag["RangeChanceMulti"] = RangeChanceMutilplier;
            tag["MagicChanceMulti"] = MagicChanceMutilplier;
            tag["SummonChanceMulti"] = SummonChanceMutilplier;
        }
        public override void LoadData(TagCompound tag)
        {
            MeleeChanceMutilplier = (float)tag["MeleeChanceMulti"];
            RangeChanceMutilplier = (float)tag["RangeChanceMulti"];
            MagicChanceMutilplier = (float)tag["MagicChanceMulti"];
            SummonChanceMutilplier = (float)tag["SummonChanceMulti"];
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            MeleeChanceMutilplier = reader.ReadSingle();
            RangeChanceMutilplier = reader.ReadSingle();
            MagicChanceMutilplier = reader.ReadSingle();
            SummonChanceMutilplier = reader.ReadSingle();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            ChestLootDropPlayer clone = (ChestLootDropPlayer)targetCopy;
            clone.MeleeChanceMutilplier = MeleeChanceMutilplier;
            clone.RangeChanceMutilplier = RangeChanceMutilplier;
            clone.MagicChanceMutilplier = MagicChanceMutilplier;
            clone.SummonChanceMutilplier = SummonChanceMutilplier;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            ChestLootDropPlayer clone = (ChestLootDropPlayer)clientPlayer;
            if (MeleeChanceMutilplier != clone.MeleeChanceMutilplier) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (RangeChanceMutilplier != clone.RangeChanceMutilplier) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (MagicChanceMutilplier != clone.MagicChanceMutilplier) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (SummonChanceMutilplier != clone.SummonChanceMutilplier) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
}