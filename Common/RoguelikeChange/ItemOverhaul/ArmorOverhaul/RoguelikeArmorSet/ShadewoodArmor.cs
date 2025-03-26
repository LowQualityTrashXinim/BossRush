using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Common.Global;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class ShadewoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.ShadewoodHelmet;
		bodyID = ItemID.ShadewoodBreastplate;
		legID = ItemID.ShadewoodGreaves;
	}
}
public class ShadewoodHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.ShadewoodHelmet;
		Add_Defense = 3;
	}
}
public class ShadewoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.ShadewoodBreastplate;
		Add_Defense = 3;
	}
}
public class ShadewoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.ShadewoodGreaves;
		Add_Defense = 3;
	}
}
public class ShadewoodArmorPlayer : PlayerArmorHandle {
	int ShadewoodArmorCD = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("ShadewoodArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		ShadewoodArmorCD = BossRushUtils.CountDown(ShadewoodArmorCD);
		Player.statDefense += 7;
		Player.moveSpeed += .15f;
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_ShadewoodArmor();
	}
	private void OnHitNPC_ShadewoodArmor() {
		if (ShadewoodArmorCD <= 0) {
			for (int i = 0; i < 75; i++) {
				Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(300, 300), 0, 0, DustID.Crimson);
				Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(300, 300), 0, 0, DustID.GemRuby);
			}
			Player.Center.LookForHostileNPC(out List<NPC> npclist, 300);
			foreach (var npc in npclist) {
				Player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)Player.GetDamage(DamageClass.Generic).ApplyTo(30), 1));
				npc.AddBuff(BuffID.Ichor, 300);
				Player.Heal(1);
			}
			ShadewoodArmorCD = BossRushUtils.ToSecond(3);
		}
	}
}
