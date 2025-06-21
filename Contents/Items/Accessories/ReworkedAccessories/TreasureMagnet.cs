using BossRush.Common.Global;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Perks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.ReworkedAccessories;
public class TreasureMagnet : ItemReworker {

	public override int VanillaItemType => ItemID.TreasureMagnet;
	public override void UpdateEquip(Player player) {
		player.luck += 0.1f;
		player.GetModPlayer<PlayerStatsHandle>().DropModifier += player.luck;
	}


}


public class TreasureMagnetPlayer : ModPlayer 
{
	public bool TreasureMagnet = false;
	public float luckAmount = 0;
	public override void ResetEffects() {
		TreasureMagnet = false;
	}
	public override void UpdateEquips() {
		Player.luck += luckAmount;
	}
}
// anti cheese mechanic, basically i dont want peeps use this acc at the end of the boss fight and still recive the full benefits of this acc
public class TreasureMagnetGlobalNPC : GlobalNPC 
{
	public override bool InstancePerEntity => true;
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.boss;
	}
	public bool hasTreasureMagnetEffect = false;
	public override void OnSpawn(NPC npc, IEntitySource source) {
		hasTreasureMagnetEffect = false;

		foreach(Player player in Main.ActivePlayers) 
			if(player.GetModPlayer<TreasureMagnetPlayer>().TreasureMagnet)
				hasTreasureMagnetEffect = true;
		
	}

	public override void OnKill(NPC npc) {
		if (hasTreasureMagnetEffect) 
			foreach (Player player in Main.ActivePlayers)
				if (player.GetModPlayer<TreasureMagnetPlayer>().TreasureMagnet)
					player.GetModPlayer<TreasureMagnetPlayer>().luckAmount += 0.05f;
	}


	public override void PostAI(NPC npc) {
		base.PostAI(npc);

		foreach (Player player in Main.ActivePlayers)
			if (!player.GetModPlayer<TreasureMagnetPlayer>().TreasureMagnet && hasTreasureMagnetEffect)
				hasTreasureMagnetEffect = false;

		

	}
}
