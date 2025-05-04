using BossRush.Common.Global;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Potion;

class RangerPotion : ModItem {
	public override void SetStaticDefaults() {
		BossRushModSystem.LootboxPotion.Add(Item);
	}
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<Ranger_Buff>(), BossRushUtils.ToMinute(10));
	}
}
public class Ranger_Buff : ModBuff {
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
