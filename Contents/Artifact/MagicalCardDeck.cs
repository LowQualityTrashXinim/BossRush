using BossRush.Common;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifact {
	internal class MagicalCardDeck : ArtifactModItem {
		protected override bool CanBeCraft => ModContent.GetInstance<BossRushModConfig>().Nightmare;
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 9));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		public override void ArtifactSetDefault() {
			width = 32; height = 37;
			Item.rare = ItemRarityID.Cyan;
		}
	}
	class MagicalCardDeckPlayer : ModPlayer {
		public bool MagicalCardDeck = false;
		public override void ResetEffects() {
			MagicalCardDeck = Player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == ModContent.ItemType<MagicalCardDeck>();
		}
	}
}