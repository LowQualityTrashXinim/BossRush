using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff;
internal class CrimsonAbsorbtion : ModBuff {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
	public override void Update(NPC npc, ref int buffIndex) {
		if (npc.life >= npc.lifeMax * .5f) {
			npc.lifeRegen -= 30;
		}
	}
	public override void Update(Player player, ref int buffIndex) {
		if (player.IsHealthAbovePercentage(.5f)) {
			player.lifeRegen -= 30;
		}
	}
}
