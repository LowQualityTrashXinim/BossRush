using BossRush.Common.Global;
using BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul;
using BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
using BossRush.Common.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange;
internal class RoguelikeBuffOverhaul : GlobalBuff {
	public override void SetStaticDefaults() {
		if (!UniversalSystem.Check_RLOH()) {
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
		RoguelikeGlobalNPC globalnpc = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		if (type == BuffID.Electrified) {
			int lifelose = 22;
			if (npc.velocity != Microsoft.Xna.Framework.Vector2.Zero) {
				lifelose += 11;
			}
			if (npc.HasBuff(BuffID.Wet)) {
				lifelose += 22;
			}
			npc.lifeRegen -= lifelose;
			globalnpc.StatDefense.Base -= 10;
			int extraElectric = (int)(npc.Size.Length() / 20) + 2;
			for (int i = 0; i < extraElectric; i++) {
				Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric, Scale: Main.rand.NextFloat(.25f, .5f));
				dust.velocity = Main.rand.NextVector2Circular(3, 3);
				dust.noGravity = true;
			}
		}
		if (BossRushModSystem.FireDeBuff.Contains(type)) {
			if (npc.HasBuff(BuffID.Slimed)) {
				npc.lifeRegen -= Math.Max(npc.lifeMax / 1000, 1);
			}
		}
	}
	public override void Update(int type, Player player, ref int buffIndex) {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		if (type == BuffID.Frostburn) {
			if (player.GetModPlayer<RoguelikeArmorPlayer>().ActiveArmor.Equals(ArmorLoader.GetModArmor("BorealwoodArmor"))) {
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: 35);
			}
		}
	}
}
