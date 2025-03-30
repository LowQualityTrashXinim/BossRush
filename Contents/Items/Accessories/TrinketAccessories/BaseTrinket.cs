using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Accessories.TrinketAccessories;
public abstract class BaseTrinket : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.DefaultToAccessory();
		TrinketDefault();
	}
	public virtual void TrinketDefault() { }
	public virtual void UpdateTrinket(Player player, TrinketPlayer modplayer) { }
	public sealed override void UpdateEquip(Player player) {
		UpdateTrinket(player, player.GetModPlayer<TrinketPlayer>());
	}
}
//This will store all the information about the trinket and how they will interact with player
public class TrinketPlayer : ModPlayer {
	public PlayerStatsHandle GetStatsHandle() => Player.GetModPlayer<PlayerStatsHandle>();

	public int counterToFullPi = 0;
	public override void PreUpdate() {
		if (++counterToFullPi >= 360)
			counterToFullPi = 0;
	}
	public override void PostUpdate() {
	}
}
public class Trinket_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int Trinket_of_Perpetuation_PointStack = 0;
}
public abstract class TrinketBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public sealed override void SetStaticDefaults() {
		TrinketSetStaticDefaults();
	}
	public virtual void TrinketSetStaticDefaults() { }
	public sealed override void Update(NPC npc, ref int buffIndex) {
		base.Update(npc, ref buffIndex);
		UpdateTrinketNPC(npc);
		if (npc.buffTime[buffIndex] <= 0) {
			OnEnded(npc);
		}
	}
	public virtual void UpdateTrinketNPC(NPC npc) { }
	public sealed override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
		UpdateTrinketPlayer(player, player.GetModPlayer<TrinketPlayer>(), ref buffIndex);
		if (player.buffTime[buffIndex] <= 0) {
			OnEnded(player);
		}
	}
	public virtual void OnEnded(Player player) { }
	public virtual void OnEnded(NPC npc) { }
	public virtual void UpdateTrinketPlayer(Player player, TrinketPlayer modplayer, ref int buffIndex) { }
}
