using BossRush.Common.Systems;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class SilverArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.SilverHelmet;
		bodyID = ItemID.SilverChainmail;
		legID = ItemID.SilverGreaves;
	}
}
public class SilverHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.SilverHelmet;
		Add_Defense = 2;
		AddTooltip = true;
		TypeEquipment = Type_Head;
		ArmorName = "SilverArmor";
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.FullHPDamage, 1 + .16f);
	}
}
public class SilverChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.SilverChainmail;
		Add_Defense = 3;
		AddTooltip = true;
		TypeEquipment = Type_Body;
		ArmorName = "SilverArmor";
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.FullHPDamage, 1 + .24f);
	}
}

public class SilverGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.SilverGreaves;
		Add_Defense = 2;
		AddTooltip = true;
		TypeEquipment = Type_Leg;
		ArmorName = "SilverArmor";
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.FullHPDamage, 1 + .12f);
	}
}
public class SilverArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("SilverArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		PlayerStatsHandle.AddStatsToPlayer(Player, PlayerStats.FullHPDamage, 1 + 1.12f);
		bool IsAbover = Player.statLife < Player.statLifeMax2 * .75f;
		if (Main.IsItDay()) {
			Player.statDefense += IsAbover ? 10 : 20;
		}
		else {
			Player.GetDamage(DamageClass.Generic) += IsAbover ? .1f : .2f;
		}
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.CheckFirstStrike()) {
			target.AddBuff<SilverPurification>(BossRushUtils.ToSecond(Main.rand.Next(5, 10)));
		}
	}
}
public class SilverPurification : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().VelocityMultiplier -= .22f;
	}
}
