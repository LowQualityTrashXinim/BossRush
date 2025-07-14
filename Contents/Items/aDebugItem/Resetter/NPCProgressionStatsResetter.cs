using BossRush.Common.Systems.SpoilSystem;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Chest;
using System;
using Terraria;
using Terraria.ModLoader;
using BossRush.Texture;

namespace BossRush.Contents.Items.aDebugItem.Resetter {
	class NPCProgressionStatsResetter : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(32, 32);
			Item.Set_DebugItem(true);
		}
		public override bool? UseItem(Player player) {
			if (player.ItemAnimationJustStarted) {
				ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Clear();
			}
			return base.UseItem(player);
		}
	}
}
