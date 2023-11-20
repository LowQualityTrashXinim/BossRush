using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket_of_Perpetuation : BaseTrinket {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		modplayer.Trinket_of_Perpetuation = true;
		player.GetAttackSpeed(DamageClass.Generic) += .15f;
	}
}
public class Samsara_of_Retribution : TrinketBuff {
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		npc.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack = Math.Clamp(++npc.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack, 0, 10);
		return base.ReApply(npc, time, buffIndex);
	}
	public override void UpdateTrinketNPC(NPC npc) {
		npc.lifeRegen -= 10 + npc.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack * 10;
	}
	public override void OnEnded(NPC npc) {
		npc.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack = 0;
	}
}
