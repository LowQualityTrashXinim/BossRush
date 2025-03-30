using Terraria;
using Terraria.ModLoader;
using BossRush.Texture;
using BossRush.Common.Global;

namespace BossRush.Contents.BuffAndDebuff;
internal class Penetrating : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 10;
	}
}
