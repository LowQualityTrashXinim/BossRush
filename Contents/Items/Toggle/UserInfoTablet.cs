using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using System.Collections.Generic;
using Terraria.ID;

namespace BossRush.Contents.Items.Toggle {
	class UserInfoTablet : ModItem {
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			if (string.IsNullOrEmpty(InfoUI.InfoShowToItem)) {
				return;
			}
			tooltips.ForEach((t) => { if (t.Name != "ItemName") t.Hide(); });
			tooltips.Add(new(Mod, "Stats", InfoUI.InfoShowToItem.Substring(0, InfoUI.InfoShowToItem.Length - 1)));
		}
		public override bool? UseItem(Player player) {
			if (player.ItemAnimationJustStarted) {
				ModContent.GetInstance<UniversalSystem>().ActivateInfoUI();
			}
			return base.UseItem(player);
		}
	}
}
