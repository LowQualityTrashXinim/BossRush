using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Common.RoguelikeChange;
using BossRush.Texture;

namespace BossRush.Contents.BuffAndDebuff;
internal class Shatter : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeOverhaulNPC>().StatDefense *= 0;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Multiplicative: 0);
	}
}
