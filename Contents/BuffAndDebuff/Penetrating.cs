using Terraria;
using Terraria.ModLoader;
using BossRush.Common.RoguelikeChange;
using BossRush.Texture;

namespace BossRush.Contents.BuffAndDebuff;
internal class Penetrating : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeOverhaulNPC>().StatDefense.Base -= 10;
	}
}
