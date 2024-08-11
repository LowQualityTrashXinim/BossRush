using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BossRush.Common {
	public class BossRushModConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;
		[Header($"BaseGameHeader")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool BossRushMode { get; set; }
		[DefaultValue(true)]
		public bool SynergyMode { get; set; }
		[DefaultValue(true)]
		public bool RoguelikeOverhaul { get; set; }

		[Header($"GameModeHeader")]
		[DefaultValue(false)]
		public bool SynergyFeverMode { get; set; }
		[ReloadRequired]
		[DefaultValue(false)]
		public bool Nightmare { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool Ascension { get; set; }

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
		[DefaultValue(false)]
		public bool NoMoreChestFromBuilderLootbox { get; set; }
		[Header($"LegacyHeader")]
		[DefaultValue(false)]
		public bool LegacyLootBoxDrop { get; set; }
		public bool LegacyBossRushWorldGen { get; set; }

		[Header($"DebugHeader")]
		[DefaultValue(false)]
		public bool WorldGenTest { get; set; }
	}
}
