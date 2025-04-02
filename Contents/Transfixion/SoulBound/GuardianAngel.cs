using System;
using Terraria;
using Humanizer;
using Terraria.ID;
using BossRush.Common.Global;
using Terraria.DataStructures;

namespace BossRush.Contents.Transfixion.SoulBound;
internal class GuardianAngel : ModSoulBound {
	public override string ModifiedToolTip(Item item) {
		int level = GetLevel(item);
		return Description.FormatWith(new string[] {
			Math.Round((.35f + .05f * level) * 100).ToString(),
			Math.Round((.7f + .01f * level) * 100).ToString(),
		});
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, 1 - .7f - .01f * GetLevel(item));
		PlayerStatsHandle.Set_Chance_SecondLifeCondition(player, "SB_GA", .35f + .05f * GetLevel(item));
	}
	public override bool PreKill(Player player, Item acc, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
		if (PlayerStatsHandle.Get_Chance_SecondLife(player, "SB_GA")) {
			player.Heal(50 + 5 * GetLevel(acc));
			player.AddImmuneTime(-1, 60);
			return false;
		}
		return base.PreKill(player, acc, damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
	}
}
internal class GuardianAngelItem : BaseSoulBoundItem {
	public override short SoulBoundType => ModSoulBound.GetSoulBoundType<GuardianAngel>();
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SoulofLight, 15)
			.AddIngredient(ItemID.HallowedBar, 12)
			.AddIngredient(ItemID.SilverBar, 40)
			.AddIngredient(ItemID.LightShard, 3)
			.AddIngredient(ItemID.PaladinsShield)
			.AddTile(TileID.LunarCraftingStation)
			.Register();
	}
}
