using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.ReworkedAccessories;
public class Toolbelt : ItemReworker {


	public override int VanillaItemType => ItemID.Toolbelt;

	public override void UpdateEquip(Player player) {
		player.GetModPlayer<ToolbeltPlayer>().Toolbelt = true;
	}


}

public class ToolbeltPlayer : ModPlayer 
{

	public bool Toolbelt=false;
	public int nearbyMinions = 0;
	public override void ResetEffects() {
		Toolbelt = false;
		nearbyMinions = 0;
	}

	public override void UpdateEquips() {
		if (Toolbelt)
			Player.statDefense += nearbyMinions * 4;
	}

}

public class ToolbeltGlobalProjectile : GlobalProjectile 
{
	public int buffRange = 300;

	public override bool InstancePerEntity => true; 
	public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) {
		return entity.minion;
	}


	public override void AI(Projectile projectile) {
		base.AI(projectile);
		Player player = Main.player[projectile.owner];
		if (player.GetModPlayer<ToolbeltPlayer>().Toolbelt && projectile.Distance(player.Center) <= buffRange) 
		{
			player.GetModPlayer<ToolbeltPlayer>().nearbyMinions++;
		}
	}


}
