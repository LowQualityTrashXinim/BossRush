using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BossRush.Common {
	public class BossRushModConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(false)]
		public bool AutoHardCore { get; set; }
		//Do note delete reload required as it is to fix "Growing Spider Cave"
		[ReloadRequired]
		[DefaultValue(true)]
		public bool BossRushMode { get; set; }

		[DefaultValue(true)]
		public bool SynergyMode { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool Nightmare { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool Ascension { get; set; }

		[DefaultValue(true)]
		public bool RoguelikeOverhaul { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool HardEnableFeature { get; set; }

		[ReloadRequired]
		[DefaultValue(false)]
		public bool ForceBossDropRegadless { get; set; }

		[DefaultValue(false)]
		public bool WorldGenTest { get; set; }

		[DefaultValue(false)]
		public bool NoMoreChestFromBuilderLootbox { get; set; }
	}
}
