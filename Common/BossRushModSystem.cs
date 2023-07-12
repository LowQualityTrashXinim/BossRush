using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using Humanizer;

namespace BossRush.Common
{
    public static class BossRushColor
    {
        /// <summary>
        /// This one will keep track somewhat, will reset if the list you put in is different
        /// This is a semi util that does color tranfering like disco color from <see cref="Main.DiscoColor"/><br/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color MultiColor(List<Color> color, int speed)
        {
            if (progress >= 255)
            {
                progress = 0;
            }
            else
            {
                progress = Math.Clamp(progress + 1 * speed, 0, 255);
            }
            if (color.Count < 1)
            {
                return Color.White;
            }
            if (color.Count < 2)
            {
                return color[0];
            }
            int count = 0;
            foreach(Color c in listcolor)
            {
                if(color.Contains(c))
                {
                    count++;
                }
            }
            if (count != color.Count)
            {
                listcolor = color;
                color1 = new Color();
                color2 = new Color();
            }
            if (color1.Equals(color2))
            {
                color1 = color[currentIndex];
                color3 = color[currentIndex];
                currentIndex = Math.Clamp((currentIndex + 1 >= color.Count) ? 0 : currentIndex + 1, 0, color.Count - 1);
                color2 = color[currentIndex];
                progress = 0;
            }
            if (!color1.Equals(color2))
            {
                color1 = Color.Lerp(color3, color2, Math.Clamp(progress / 255f, 0, 1f));
            }
            return color1;
        }
        private static int currentIndex = 0, progress = 0;
        static Color color1 = new Color(), color2 = new Color(), color3 = new Color();
        static List<Color> listcolor = new List<Color>();
    }


    public class onBossKillCheck : GlobalNPC
    {

        public override void OnKill(NPC npc)
        {


            if (!BossRushUtils.IsAnyVanillaBossAlive() && npc.active && npc.life <= 0)
            {
                if (npc.type == BossProgression.NextcurrentProgressionBoss && npc.type != NPCID.MoonLordCore)
                    BossProgression.NextcurrentProgressionBoss++;

            }
        }
    }

    public class BossProgression : ModSystem
    {

        //public List[] mechBosses = new int[] { NPCID.TheDestroyer, NPCID.Retinazer, NPCID.SkeletronPrime };

        // all possible bosses that can summoned by this item, modded bosses is possible to add if anyone really cares.        
        private static int[] summonableBosses = new int[] {


        };


        public static int NextcurrentProgressionBoss
        {

            get { return summonableBosses[nextBossIDFromListIndex]; }
            set { nextBossIDFromListIndex = value; }

        }

        //public int getEvilBoss() { return 0; }
        //public int? getMechBoss() { return null; }

        // get current world evil boss
        //public int WorldEvilBoss = WorldGen.crimson ? NPCID.BrainofCthulhu : NPCID.EaterofWorldsHead;

        //public static int chooseUndefeatedMech() {

        //    int num = Main.rand.Next(0, 3);

        //    if (!NPC.downedMechBoss1 && num == 0)
        //        return num;
        //    else
        //        if (!NPC.downedMechBoss2 && num == 1)
        //        return num;
        //    else
        //        if (!NPC.downedMechBoss3 && num == 2)
        //        return num;
        //    else
        //        return 0;

        //}


        // if a boss was killed with the same id as the indexed boss id, choose the next boss id from summonableBosses List.
        public static int nextBossIDFromListIndex = 0;

        public override void SaveWorldData(TagCompound tag)
        {
            tag["nextBossIDFromListIndex"] = nextBossIDFromListIndex;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            nextBossIDFromListIndex = tag.GetInt("nextBossIDFromListIndex");

        }

    }


    // WABD stands for "Where Are The Bosses Data"...
    // each object of Wabd contains the following: The boss's NPCID, spawnItem, tier, and their color code
    // WHY??: this acts as a reference to Bosses and their corrisponding spawn item and intended progression 
    // modded bosses should be also addable here.
    public class WABD
    {
        public int bossID { get; set; }
        public int spawnItemID { get; set; }
        public int tier { get; set; }
        public Color colorCode { get; set; }

        public static int bossesCount = BossDirectory.Count;

       
        //le constructer
        public WABD(int bossID, int spawnItemID, int tier, Color colorCode)
        {
            this.bossID = bossID;
            this.spawnItemID = spawnItemID;
            this.tier = tier;
            this.colorCode = colorCode;
        }

        public static WABD KingSlime = new WABD(NPCID.KingSlime, ItemID.SlimeCrown, 1, Color.Aqua);
        public static WABD EyeofCthulhu = new WABD(NPCID.EyeofCthulhu, ItemID.SuspiciousLookingEye, 2, Color.Red);
        public static WABD BrainofCthulhu = new WABD(NPCID.BrainofCthulhu, ItemID.BloodySpine, 3, Color.LightPink);
        public static WABD EaterOfWorlds = new WABD(NPCID.EaterofWorldsHead, ItemID.WormFood, 3, Color.Purple);
        public static WABD QueenBee = new WABD(NPCID.QueenBee, ItemID.Abeemination, 4, Color.Orange);
        public static WABD Skeletron = new WABD(NPCID.Skeleton, ModContent.ItemType<Contents.Items.Spawner.CursedDoll>(), 4, Color.Gray);
        public static WABD Deerclops = new WABD(NPCID.Deerclops, ItemID.DeerThing, 5, Color.White);
        public static WABD QueenSlime = new WABD(NPCID.QueenSlimeBoss, ItemID.QueenSlimeCrystal, 6, Color.Pink);
        public static WABD TheDestroyer = new WABD(NPCID.TheDestroyer, ItemID.MechanicalWorm, 6, Color.Purple);
        public static WABD Retinazer = new WABD(NPCID.Retinazer, ItemID.MechanicalLens, 6, Color.Red);
        public static WABD Spazmatism = new WABD(NPCID.Spazmatism, ItemID.MechanicalLens, 6, Color.Red);
        public static WABD SkeletronPrime = new WABD(NPCID.SkeletronPrime, ItemID.MechanicalSkull, 6, Color.Gray);
        public static WABD Plantera = new WABD(NPCID.Plantera, ModContent.ItemType<Contents.Items.Spawner.PlanteraSpawn>(), 7, Color.LimeGreen);
        public static WABD Golem = new WABD(NPCID.Golem, ItemID.LihzahrdPowerCell, 8, Color.Brown);
        public static WABD EmpressOfLight = new WABD(NPCID.HallowBoss, ItemID.EmpressButterfly, 8, Color.HotPink);
        public static WABD DukeFishron = new WABD(NPCID.DukeFishron, ItemID.TruffleWorm, 8, Color.DarkBlue);
        public static WABD LunaticCultist = new WABD(NPCID.CultistBoss, ItemID.AncientPinkDungeonBrick, 9, Color.BlueViolet);
        public static WABD MoonLord = new WABD(NPCID.MoonLordCore, ItemID.CelestialSigil, 10, Color.Aquamarine);

        public static Dictionary<int, WABD> BossDirectory = new Dictionary<int, WABD>()
        {
            {NPCID.KingSlime, KingSlime},
            {NPCID.EyeofCthulhu, EyeofCthulhu},
            {NPCID.BrainofCthulhu, BrainofCthulhu},
            {NPCID.EaterofWorldsHead, EaterOfWorlds},
            {NPCID.Deerclops, Deerclops},
            {NPCID.Skeleton, Skeletron},
            {NPCID.QueenSlimeBoss, QueenSlime},
            {NPCID.TheDestroyer, TheDestroyer},
            {NPCID.SkeletronPrime, SkeletronPrime},
            {NPCID.Golem, Golem},
            {NPCID.HallowBoss, EmpressOfLight},
            {NPCID.DukeFishron, DukeFishron},
            {NPCID.CultistBoss, LunaticCultist},
            {NPCID.MoonLordCore, MoonLord}

        };
        
        

    }



}
