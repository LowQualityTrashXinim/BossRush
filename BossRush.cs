using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using Newtonsoft.Json;
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

        public Dictionary<int,BossRushAchivement> achievementData;
        public override void Load()
        {
            base.Load();
            achievementData.Add(1,new BossRushAchivement()
                {
                    Name = "The beginning of endless",
                    Description = "This mark the beginning of where it all start",
                    ConditionText = "Open your first chest",
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
            string path = "C:\\Users\\DELL\\Documents\\My Games\\Terraria\\tModLoader\\BossRushAchievement\\AchievementData.json";
            CheckIfFileExist(path);
            using (StreamReader r = new StreamReader(path))
            {

            }
        }
        public override void Unload()
        {
            base.Unload();
        }
        private void CheckIfFileExist(string path)
        {
            if (File.Exists(path))
            {
                if(!string.IsNullOrEmpty(File.ReadAllText(path)))
                {
                    return;
                }
                using (StreamWriter sw = File.AppendText(path))
                {

                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(path))
                {

                }
            }
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

        public AchievementHolder dataholder => Main.LocalPlayer.GetModPlayer<AchievementHolder>();

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
                return dataholder.chestplayer.CurrentSectionAmountOfChestOpen;
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
        public AchievementHolder dataholder { get; }
    }
}