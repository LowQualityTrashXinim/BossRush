using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class PumpkinArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.PumpkinHelmet;
		bodyID = ItemID.PumpkinBreastplate;
		legID = ItemID.PumpkinLeggings;
	}
}
public class PumpkinHelmet : ModArmorPiece {
	public override int Add_Defense => 2;
	public override int _pieceID => ItemID.PumpkinHelmet;
}
public class PumpkinBreastplate : ModArmorPiece {
	public override int Add_Defense => 3;
	public override int _pieceID => ItemID.PumpkinBreastplate;
}
public class PumpkinLeggings : ModArmorPiece {
	public override int Add_Defense => 2;
	public override int _pieceID => ItemID.PumpkinLeggings;
}

public class PumpkinArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("PumpkinArmor", this);
	}
	bool inZone = false;
	public override void Armor_ResetEffects() {
		inZone = false;
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneOverworldHeight) {
			inZone = true;
		}
	}
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		OnHitEffect_PumpkinArmor();
	}
	public override void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitEffect_PumpkinArmor();
	}
	public override void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_PumpkinArmor(target, Player.GetWeaponDamage(item));
	}
	public override void Armor_OnHitWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_PumpkinArmor(target, proj.damage);
	}
	private void OnHitEffect_PumpkinArmor() {
		if (inZone) {
			Player.AddBuff(BuffID.WellFed3, 300);
		}
	}
	private void OnHitNPC_PumpkinArmor(NPC npc, float damage) {
		if (!inZone || !Main.rand.NextBool(3)) {
			return;
		}
		if (npc.HasBuff(ModContent.BuffType<pumpkinOverdose>())) {
			int explosionRaduis = 75 + (int)MathHelper.Clamp(damage, 0, 125);
			for (int i = 0; i < 35; i++) {
				Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(explosionRaduis, explosionRaduis), 0, 0, DustID.Pumpkin);
				Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(explosionRaduis, explosionRaduis), 0, 0, DustID.OrangeTorch);
			}
			npc.Center.LookForHostileNPC(out List<NPC> npclist, explosionRaduis);
			foreach (var i in npclist) {
				Player.StrikeNPCDirect(npc, i.CalculateHitInfo(5 + (int)(damage * 0.05f), 1, Main.rand.NextBool(40)));
			}
			SoundEngine.PlaySound(SoundID.NPCDeath46);
			npc.AddBuff(ModContent.BuffType<pumpkinOverdose>(), 240);
		}
		else {
			npc.AddBuff(ModContent.BuffType<pumpkinOverdose>(), 240);
		}
	}
}
