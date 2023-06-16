using BossRush.Contents.Items.Chest;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush
{
    public partial class BossRush : Mod
    {
        public override void Load()
        {
            base.Load();
        }
        public override void Unload()
        {
            base.Unload();
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
        string Name, Description, ConditionText;

        public AchievementHolder dataholder => Main.LocalPlayer.GetModPlayer<AchievementHolder>();

        public BossRushAchivement()
        {

        }
        public BossRushAchivement(string name, string description, string conditionText)
        {
            Name = name;
            Description = description;
            ConditionText = conditionText;
        }
        protected int HowManyChestHasPlayerOpenInCurrentSection()
        {
            if(Main.netMode == NetmodeID.SinglePlayer)
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
        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            return base.HoverSlot(inventory, context, slot);
        }
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