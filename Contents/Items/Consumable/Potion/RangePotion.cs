using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Consumable.Potion;

class RangePotion : ModItem {
	public override void SetStaticDefaults() {
		BossRushModSystem.LootboxPotion.Add(Item);
	}
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<Range_Buff>(), BossRushUtils.ToMinute(10));
	}
}
public class Range_Buff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AddStatsToPlayer(PlayerStats.RangeDMG, Multiplicative: 1.1f);
		handle.AddStatsToPlayer(PlayerStats.RangeCritChance, Base: 10);
		handle.AddStatsToPlayer(PlayerStats.RangeAtkSpeed, 1.1f);
	}
}
