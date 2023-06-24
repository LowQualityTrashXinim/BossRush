using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush
{
    public partial class BossRush : Mod
    {

        public Dictionary<int, BossRushAchivement> achievementData = new Dictionary<int, BossRushAchivement>();
        public override void Load()
        {
            base.Load();
            AddAchievement();
            string path = GeneratePathToAchievement();
            CheckIfFileExist(path);
        }
        public override void Unload()
        {
            base.Unload();
        }
        public string GeneratePathToAchievement()
        {
            string autoPathfinding = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            int index = autoPathfinding.Length - "AppData\\Roaming".Length;
            autoPathfinding = autoPathfinding.Substring(0, index);
            autoPathfinding += "Documents\\My Games\\Terraria\\tModLoader\\BossRushAchievement\\AchievementData.json";
            return autoPathfinding;
        }
        private void CheckIfFileExist(string path)
        {
            if (File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    string json = JsonConvert.SerializeObject(achievementData);
                    sw.WriteLine(json);
                }
            }
            else
            {
                Directory.CreateDirectory("C:\\Users\\DELL\\Documents\\My Games\\Terraria\\tModLoader\\BossRushAchievement").Create();
                using (StreamWriter sw = File.CreateText(path))
                {
                    string json = JsonConvert.SerializeObject(achievementData);
                    sw.WriteLine(json);
                }
            }
        }
        private void AddAchievement()
        {
            achievementData.Add(1, new BossRushAchivement()
            {
                Name = "The beginning of endless",
                Description = "This mark the beginning of where it all start",
                ConditionText = "Open your first lootboxs",
            });
            achievementData.Add(2, new BossRushAchivement()
            {
                Name = "The first artifact holder",
                Description = "Thing about to get spicy",
                ConditionText = "Use the first artifact (beside Broken artifact)",
            });
            achievementData.Add(3, new BossRushAchivement()
            {
                Name = "The first of many",
                Description = "First time is always the best",
                ConditionText = "Kill king slime boss",
            });
            achievementData.Add(4, new BossRushAchivement()
            {
                Name = "The start of addiction",
                Description = "",
                ConditionText = "Open 100 lootbox",
            });
            achievementData.Add(5, new BossRushAchivement()
            {
                Name = "There are many more",
                Description = "",
                ConditionText = "Open 1000 lootbox",
            });
            achievementData.Add(6, new BossRushAchivement()
            {
                Name = "Skill check",
                Description = "",
                ConditionText = "Beat a boss without getting hit",
            });
            achievementData.Add(7, new BossRushAchivement()
            {
                Name = "First success",
                Description = "",
                ConditionText = "Beat the mod from start to finish ( Pre boss to post moonlord )",
            });
        }
    }
    public class BossRushModSystem : ModSystem
    {
        public override void OnWorldLoad()
        {
            base.OnWorldLoad();
        }
        public override void OnWorldUnload()
        {
            base.OnWorldUnload();
        }
    }
    /// <summary>
    /// This should and will be run on client side only, this should never work in multiplayer no matter what
    /// </summary>
    public class BossRushAchivement : BossRushCondition, AchievementDataHolder
    {
        public string Name, Description, ConditionText;
        public bool ConditionMet = false;

        public AchievementHolder dataholder() => Main.LocalPlayer.GetModPlayer<AchievementHolder>();

        public BossRushAchivement()
        {

        }
        public BossRushAchivement(string name, string description, string conditionText, bool ConditionMet)
        {
            Name = name;
            Description = description;
            ConditionText = conditionText;
            this.ConditionMet = ConditionMet;
        }
        protected int HowManyChestHasPlayerOpenInCurrentSection()
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                return dataholder().chestplayer.CurrentSectionAmountOfChestOpen;
            }
            return -1;
        }
        public bool Condition() => false;
    }
    public class AchievementHolder : ModPlayer
    {
        public ChestLootDropPlayer chestplayer => Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
    }
    public interface BossRushCondition
    {
        public bool Condition();
    }
    public interface AchievementDataHolder
    {
        public AchievementHolder dataholder() => null;
    }
}