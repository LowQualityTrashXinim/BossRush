using BossRush.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange;
internal class RoguelikeBuffOverhaul : GlobalBuff {
	public override void SetStaticDefaults() {
		if(!UniversalSystem.Check_RLOH()) {
			return;
		}
		//I am unsure why this is set to true
		Main.debuff[BuffID.Campfire] = false;
		Main.debuff[BuffID.Honey] = false;
		Main.debuff[BuffID.StarInBottle] = false;
		Main.debuff[BuffID.HeartLamp] = false;
		Main.debuff[BuffID.CatBast] = false;
		Main.debuff[BuffID.Sunflower] = false;
	}
	public override void Update(int type, NPC npc, ref int buffIndex) {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		RoguelikeOverhaulNPC globalnpc = npc.GetGlobalNPC<RoguelikeOverhaulNPC>();
		if (type == BuffID.Electrified) {
			int lifelose = 22;
			if (npc.velocity != Microsoft.Xna.Framework.Vector2.Zero) {
				lifelose += 11;
			}
			npc.lifeRegen -= lifelose;
			globalnpc.StatDefense.Base -= 10;
		}
	}
}
