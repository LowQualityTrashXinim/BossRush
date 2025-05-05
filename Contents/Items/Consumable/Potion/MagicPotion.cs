using BossRush.Common.Global;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace BossRush.Contents.Items.Consumable.Potion;

public class MagicPotion : ModItem {
	public override void SetStaticDefaults() {
		BossRushModSystem.LootboxPotion.Add(Item);
	}
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<Magic_Buff>(), BossRushUtils.ToMinute(10));
	}
}
public class Magic_Buff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AddStatsToPlayer(PlayerStats.MagicDMG, Multiplicative: 1.1f);
		handle.AddStatsToPlayer(PlayerStats.MagicCritChance, Base: 10);
		handle.AddStatsToPlayer(PlayerStats.MagicAtkSpeed, 1.1f);
	}
}
