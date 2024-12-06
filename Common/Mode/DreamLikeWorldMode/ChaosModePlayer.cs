using BossRush.Common.Mode.DreamLikeWorld;
using BossRush.Common.Systems.ArgumentsSystem;
using BossRush.Contents.WeaponEnchantment;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Mode.DreamLikeWorldMode;
internal class ChaosModePlayer : ModPlayer {
	public override void OnEnterWorld() {
		int type = Main.rand.Next(ModContent.GetInstance<ChaosModeSystem>().Dict_Chaos_Weapon.Keys.ToList());
		Player.QuickSpawnItem(Player.GetSource_None(), type);
	}
	public override void UpdateEquips() {
		Player.GetModPlayer<AugmentsPlayer>().IncreasesChance += 1f;
		Player.GetModPlayer<EnchantmentModplayer>().RandomizeChanceEnchantment += .2f;
	}
}
