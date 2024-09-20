using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.WeaponEnchantment;
using Terraria.ID;

namespace BossRush.Contents.Artifacts {
	internal class TokenOfPrideArtifact : Artifact {
		public override int Frames => 10;
		public override Color DisplayNameColor => Color.PaleGreen;
	}
	public class PridePlayer : ModPlayer {
		bool Pride = false;
		protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
		public override void ResetEffects() {
			Pride = Player.HasArtifact<TokenOfPrideArtifact>();
		}
		public override void PostUpdate() {
			if (Pride) {
				chestmodplayer.finalMultiplier = 0;
			}
		}
		public override void PreUpdate() {
			if (!Pride) {
				return;
			}
			Item item = Player.HeldItem;
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				if (globalitem.EnchantmenStlot == null || globalitem.EnchantmenStlot.Length < 1 && EnchantmentGlobalItem.CanBeEnchanted(item) 
					|| globalitem.EnchantmenStlot[3] != ItemID.None || globalitem.EnchantmenStlot[3] != -1) {
					return;
				}
				EnchantmentSystem.EnchantItem(item, 3);
			}
		}
	}
}
