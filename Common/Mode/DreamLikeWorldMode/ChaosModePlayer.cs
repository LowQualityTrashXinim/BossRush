using BossRush.Common.Systems.ArgumentsSystem;
using BossRush.Contents.WeaponEnchantment;
using Terraria.ModLoader;

namespace BossRush.Common.Mode.DreamLikeWorldMode;
internal class ChaosModePlayer : ModPlayer{
	public override void UpdateEquips() {
		Player.GetModPlayer<AugmentsPlayer>().IncreasesChance += 1f;
		Player.GetModPlayer<EnchantmentModplayer>().RandomizeChanceEnchantment += .2f;
	}
}
