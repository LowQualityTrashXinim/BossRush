using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using BossRush.Common.Enraged;
using BossRush.Contents.Artifact;

namespace BossRush.Common
{
    public class IsPlayerAlreadyHaveASpawner : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return info.player.GetModPlayer<ModdedPlayer>().HowManyBossIsAlive <= 1;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Can drop only when all boss is dead";
    }
    public class ChallengeModeException : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode || ModContent.GetInstance<BossRushModConfig>().ForceBossDropRegadless;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Exclusive to challenge mode";
    }
    public class EvilBossChallengeModeException : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if(!info.IsInSimulation)
                return (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode || ModContent.GetInstance<BossRushModConfig>().ForceBossDropRegadless) && NPC.downedBoss2;
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Exclusive to challenge mode";
    }
    public class MagicalCardDeckException : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return info.player.GetModPlayer<MagicalCardDeckPlayer>().MagicalCardDeck;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Magical card deck call in more card";
    }
    public class SynergyDrop : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return ModContent.GetInstance<BossRushModConfig>().SynergyMode || ModContent.GetInstance<BossRushModConfig>().HardEnableFeature;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Exclusive to Synergy mode";
    }
    public class BossIsEnragedBySpecialSpawner : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return (info.player.GetModPlayer<EnragedPlayer>().Enraged || ModContent.GetInstance<BossRushModConfig>().Enraged) && info.player.GetModPlayer<ModdedPlayer>().HowManyBossIsAlive <= 1;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Drops only if all the enraged bosses that is present is dead";
    }
    public class QueenBeeEnranged : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return !info.player.ZoneJungle;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Drops if player enrage queen bee";
    }
    public class DukeIsEnrage : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return !info.player.ZoneBeach;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Drops if player aren't at beach";
    }
    public class DeerclopHateYou : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return Main.raining && !Main.dayTime && info.player.ZoneSnow;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Drop if player is fighting in snow biome, in night and is snowing";
    }
    public class IsNotABossAndBossIsAlive : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return BossRushUtils.IsAnyVanillaBossAlive() && !info.npc.boss;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Drop only when npc is not a boss and boss is alive";
    }
    public class GitGudMode : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return info.player.GetModPlayer<ModdedPlayer>().amountoftimegothit == 0
                    || info.player.GetModPlayer<ModdedPlayer>().gitGud > 0;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Drop if player beat boss in no hit aka git gud mode";
    }
    public class NightmareMode : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return ModContent.GetInstance<BossRushModConfig>().Nightmare
                    || ModContent.GetInstance<BossRushModConfig>().HardEnableFeature;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Nightmare mode exclusive";
    }
    public class LifeCrystalMax : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return info.player.statLifeMax < 400;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "";
    }
    public class ManaCrystalMax : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return info.player.statManaMax < 200;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "";
    }
}