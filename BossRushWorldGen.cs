using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.IO;

namespace BossRush
{
    internal class BossRushWorldGen : ModSystem
    {
        public override bool CanWorldBePlayed(PlayerFileData playerData, WorldFileData worldFileData)
        {
            if (worldFileData.GameMode != 3 && ModContent.GetInstance<BossRushModConfig>().YouLikeToHurtYourself && !worldFileData.ForTheWorthy)
            {
                return false;
            }
            else if(worldFileData.GameMode == 3 && ModContent.GetInstance<BossRushModConfig>().YouLikeToHurtYourself && worldFileData.ForTheWorthy)
            {
                return true;
            }
            return true;
        }

        public override string WorldCanBePlayedRejectionMessage(PlayerFileData playerData, WorldFileData worldData)
        {
            if(ModContent.GetInstance<BossRushModConfig>().YouLikeToHurtYourself && worldData.GameMode != 3 && !worldData.ForTheWorthy)
            {
                return "A force of pain block you from cheesing\nokay, with the stupid cringe edgy line out of the way, you must play on Master difficulty world";
            }
            return base.WorldCanBePlayedRejectionMessage(playerData, worldData);
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            if(ModContent.GetInstance<BossRushModConfig>().YouLikeToHurtYourself)
            {
                Main.getGoodWorld = true;
            }
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                int BigAssTree = tasks.FindIndex(GenPass => GenPass.Name.Equals("Living Trees"));
                tasks.RemoveAt(BigAssTree);
                int BigAssTreeWall = tasks.FindIndex(GenPass => GenPass.Name.Equals("Wood Tree Walls"));
                tasks.RemoveAt(BigAssTreeWall);
                int FloatIsland = tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Islands"));
                tasks.RemoveAt(FloatIsland);
                int FloatIslandHouse = tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Island Houses"));
                tasks.RemoveAt(FloatIslandHouse);
                int Dungeon = tasks.FindIndex(GenPass => GenPass.Name.Equals("Dungeon"));
                tasks.RemoveAt(Dungeon);
                int LifeCrystal = tasks.FindIndex(GenPass => GenPass.Name.Equals("Life Crystals"));
                tasks.RemoveAt(LifeCrystal);
                int Shines = tasks.FindIndex(GenPass => GenPass.Name.Equals("Shinies"));
                tasks.RemoveAt(Shines);
                int Pyramids = tasks.FindIndex(GenPass => GenPass.Name.Equals("Pyramids"));
                tasks.RemoveAt(Pyramids);
                int Altars = tasks.FindIndex(GenPass => GenPass.Name.Equals("Altars"));
                tasks.RemoveAt(Altars);
                int Hives = tasks.FindIndex(GenPass => GenPass.Name.Equals("Hives"));
                tasks.RemoveAt(Hives);
                int JungleChests = tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests"));
                tasks.RemoveAt(JungleChests);
                int BuriedChests = tasks.FindIndex(GenPass => GenPass.Name.Equals("Buried Chests"));
                tasks.RemoveAt(BuriedChests);
                int SurfaceChests = tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Chests"));
                tasks.RemoveAt(SurfaceChests);
                int JungleChestsPlacement = tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests Placement"));
                tasks.RemoveAt(JungleChestsPlacement);
                int WaterChests = tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Chests"));
                tasks.RemoveAt(WaterChests);
                int JungleTrees = tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Trees"));
                tasks.RemoveAt(JungleTrees);
                int JungleTemple = tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Temple"));
                tasks.RemoveAt(JungleTemple);
                int MicroBiomes = tasks.FindIndex(GenPass => GenPass.Name.Equals("Micro Biomes"));
                tasks.RemoveAt(MicroBiomes);
                int Moss = tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss"));
                tasks.RemoveAt(Moss);
                int SurfaceOreandStone = tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Ore and Stone"));
                tasks.RemoveAt(SurfaceOreandStone);
                int PlantingTrees = tasks.FindIndex(GenPass => GenPass.Name.Equals("Planting Trees"));
                tasks.RemoveAt(PlantingTrees);
                int Larva = tasks.FindIndex(GenPass => GenPass.Name.Equals("Larva"));
                tasks.RemoveAt(Larva);
            }
        }
    }
}
