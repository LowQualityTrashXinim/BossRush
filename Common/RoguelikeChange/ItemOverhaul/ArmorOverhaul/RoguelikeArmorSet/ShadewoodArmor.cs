using BossRush.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class ShadewoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.ShadewoodHelmet;
		bodyID = ItemID.ShadewoodBreastplate;
		legID = ItemID.ShadewoodGreaves;
	}
}
public class ShadewoodHelmet : ModArmorPiece {
	public override int _pieceID => ItemID.ShadewoodHelmet;
	public override int Add_Defense => 3;
}
public class ShadewoodBreastplate : ModArmorPiece {
	public override int _pieceID => ItemID.ShadewoodBreastplate;
	public override int Add_Defense => 4;
}
public class ShadewoodGreaves : ModArmorPiece {
	public override int _pieceID => ItemID.ShadewoodGreaves;
	public override int Add_Defense => 3;
}
public class ShadewoodArmorPlayer : PlayerArmorHandle {
	int ShadewoodArmorCD = 0;
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneCrimson) {
			Player.statDefense += 7;
			Player.moveSpeed += .15f;
		}
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_ShadewoodArmor();
	}
	private void OnHitNPC_ShadewoodArmor() {
		if (ShadewoodArmorCD <= 0) {
			float radius = Player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(300);
			for (int i = 0; i < 75; i++) {
				Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(radius, radius), 0, 0, DustID.Crimson);
				Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(radius, radius), 0, 0, DustID.GemRuby);
			}
			Player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
			foreach (var npc in npclist) {
				Player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)Player.GetDamage(DamageClass.Generic).ApplyTo(30), 1));
				npc.AddBuff(BuffID.Ichor, 300);
				Player.Heal(1);
			}
			ShadewoodArmorCD = BossRushUtils.ToSecond(3);
		}
	}
}
