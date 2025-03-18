using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BossRush.Common.General
{
    public class RogueLikeConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header($"BaseGameHeader")]
        [DefaultValue(true)]
        public bool RoguelikeOverhaul { get; set; }

        [Header($"GameModeHeader")]
        [DefaultValue(true)]
        public bool BossRushMode { get; set; }
        [DefaultValue(false)]
        public bool SynergyFeverMode { get; set; }
        //TODO : Add a world data IsNightmareWorld 
        [ReloadRequired]
        [DefaultValue(false)]
        public bool Nightmare { get; set; }
        [DefaultValue(false)]
        public bool TotalRNG { get; set; }
        [ReloadRequired]
        [DefaultValue(false)]
        public bool Ascension { get; set; }
        //Replace Cursed skull
        [ReloadRequired]
        [DefaultValue(false)]
        public bool HellishEndeavour { get; set; }
        //Cursed challenge
        [ReloadRequired]
        [DefaultValue(false)]
        public bool LifeOfThorn { get; set; }
        //Chaotic god challenge
        [ReloadRequired]
        [DefaultValue(false)]
        public bool DreamlikeWorld { get; set; }
        [ReloadRequired]
        [DefaultValue(false)]
        public bool UnfairMode { get; set; }
        [Header($"LuckDepartmentHeader")]
        [DefaultValue(true)]
        public bool RareSpoils { get; set; }
        [DefaultValue(true)]
        public bool RareLootbox { get; set; }
        [DefaultValue(true)]
        public bool LostAccessory { get; set; }
        [DefaultValue(true)]
        public bool WeaponEnchantment { get; set; }
        [DefaultValue(true)]
        public bool AccessoryPrefix { get; set; }
        [Header($"QoLHeader")]
        [DefaultValue(false)]
        public bool AutoHardCore { get; set; }
        [ReloadRequired]
        [DefaultValue(false)]
        public bool HardEnableFeature { get; set; }
        [ReloadRequired]
        [DefaultValue(false)]
        public bool ForceBossDropRegadless { get; set; }
        [ReloadRequired]
        [DefaultValue(false)]
        public bool AutoRandomizeCharacter { get; set; }
        [Header($"LegacyHeader")]
        [DefaultValue(false)]
        public bool LegacyLootBoxDrop { get; set; }
        public bool LegacyBossRushWorldGen { get; set; }
        public bool LegacySpoils { get; set; }

        [Header($"DebugHeader")]
        [DefaultValue(false)]
        public bool WorldGenTest { get; set; }
        public bool TemplateTest { get; set; }
        public bool WorldGenRLSettingTest { get; set; }
    }
}
