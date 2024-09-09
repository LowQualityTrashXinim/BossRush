using BossRush.Common.Systems;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using BossRush.Contents.Perks;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.aDebugItem.StatsInform;
internal class PerkDebugShower : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 10;
		Item.Set_DebugItem(true);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		base.ModifyTooltips(tooltips);
		var player = Main.LocalPlayer;
		var perkplayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();
		string perk = "";
		foreach (var perkItem in perkplayer.perks.Keys) {
			if (ModPerkLoader.GetPerk(perkItem) != null) {
				perk += "\n" + ModPerkLoader.GetPerk(perkItem).DisplayName + $" | Value : [{perkplayer.perks[perkItem]}]";
			}
		}
		var line = new TooltipLine(Mod, "StatsShowcase",
			"[For Debug purpose]" +
			perk
			);
		tooltips.Add(line);
	}
}
