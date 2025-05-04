using BossRush.Common.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.ReworkedAccessories;
public class MechanicalLens : ItemReworker {

	public override int VanillaItemType => ItemID.MechanicalLens;

	public override void UpdateEquip(Player player) {
		player.GetCritChance(DamageClass.Generic) += 4;
		player.GetCritChance(DamageClass.Generic) *= 1.33f;
		player.GetModPlayer<MechanicalLensPlayer>().MechanicalLens = true;
	}

	
}

public class MechanicalLensPlayer : ModPlayer 
{

	public bool MechanicalLens = false;
	public override void ResetEffects() {
		MechanicalLens = false;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {

		if (!MechanicalLens)
			return;

		modifiers.CritDamage -= 0.15f;
		modifiers.DamageVariationScale *= 0.25f;
		
	}

}
