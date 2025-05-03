using BossRush.Contents.Skill;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.ReworkedAccessories;
public class DepthMeter : ItemReworker {

	public override int VanillaItemType => ItemID.DepthMeter;
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<DepthMeterPlayer>().depthMeter = true;
	}

	public override void UpdateInfoAccessory(Player player) {
		player.accDepthMeter++;
	}
}

public class DepthMeterPlayer : ModPlayer 
{
	public bool depthMeter = false;
	public override void ResetEffects() {
		depthMeter = false;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (depthMeter)
			modifiers.SourceDamage += Utils.GetLerpValue(0f,0.25f,Player.Distance(target.Center) / 500f,true);
	}

}
