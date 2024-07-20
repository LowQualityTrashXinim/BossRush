using BossRush.Common.Systems;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff;
internal class TheUnderworldWrath : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 180;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.lifeRegen -= 60;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.DefenseEffectiveness, Additive: .23f);
	}
}
