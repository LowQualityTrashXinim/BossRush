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

class MeleePotion : ModItem {
	public override void SetStaticDefaults() {
		BossRushModSystem.LootboxPotion.Add(Item);
	}
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<Melee_Buff>(), BossRushUtils.ToMinute(10));
	}
}
public class Melee_Buff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AddStatsToPlayer(PlayerStats.MeleeDMG, Multiplicative: 1.1f);
		handle.AddStatsToPlayer(PlayerStats.MeleeCritChance, Base: 10);
		handle.AddStatsToPlayer(PlayerStats.MeleeAtkSpeed, 1.1f);
	}
}
