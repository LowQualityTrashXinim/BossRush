using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using BossRush.Common;
using System.Reflection.Emit;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.Core;

namespace BossRush.Common.ChallengeMode
{
    internal class ChallengeModeWorldGen : ModSystem
    {
        public override void OnWorldLoad()
        {
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.type == NPCID.OldMan)
                    {
                        npc.active = false;
                    }
                }
            }
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Spider Caves")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Living Trees")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wood Tree Walls")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Islands")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Island Houses")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Life Crystals")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shinies")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Pyramids")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Altars")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Hives")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Buried Chests")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Chests")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests Placement")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Chests")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Trees")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Temple")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Micro Biomes")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Marble")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Granite")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mushrooms")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Ore and Stone")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Planting Trees")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Larva")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Cactus, Palm Trees, & Coral")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Gems In Ice Biome")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Random Gems")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Vines")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Piles")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Traps")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Statues")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shell Piles")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Oasis")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Plants")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Flowers")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Plants")));
                //tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dungeon")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wavy Caves")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Rock Layer Caves")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Weeds")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Webs And Honey")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Clay")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Herbs")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dye Plants")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dirt Layer Caves")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss Grass")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Hellforge")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Pots")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Place Fallen Log")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mushroom Patches")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Glowing Mushrooms and Jungle Plants")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Small Holes")));
                tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Remove Broken Traps")));
            }
        }
    }
}