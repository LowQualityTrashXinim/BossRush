using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class RichMahoganyArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.RichMahoganyHelmet;
		bodyID = ItemID.RichMahoganyBreastplate;
		legID = ItemID.RichMahoganyGreaves;
	}
}
class RichMahoganyHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.RichMahoganyHelmet;
		Add_Defense = 3;
		AddTooltip = true;
		ArmorName = "RichMahoganyArmor";
		TypeEquipment = Type_Head;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.SummonDMG, 1 + .12f);
		player.GetModPlayer<RichMahoganyPlayer>().Standing = true;
	}
}
class RichMahoganyBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.RichMahoganyBreastplate;
		Add_Defense = 4;
		AddTooltip = true;
		ArmorName = "RichMahoganyArmor";
		TypeEquipment = Type_Body;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxMinion, Base: 1);
		player.GetModPlayer<RichMahoganyPlayer>().Hit = true;
	}
}
class RichMahoganyGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.RichMahoganyGreaves;
		Add_Defense = 3;
		AddTooltip = true;
		ArmorName = "RichMahoganyArmor";
		TypeEquipment = Type_Leg;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxSentry, Base: 1);
		player.whipRangeMultiplier += .15f;
	}
}
public class RichMahoganyPlayer : ModPlayer {
	public bool Standing = false;
	public bool Hit = false;
	int counter = 0;
	int requirement = 300;
	public override void ResetEffects() {
		Standing = false;
		Hit = false;
	}
	public override void PostUpdateEquips() {
		if (Standing) {
			if (Player.velocity == Vector2.Zero) {
				if (counter >= requirement || CheckArmorActive() && counter >= requirement / 3) {
					Player.AddBuff(BuffID.DryadsWard, BossRushUtils.ToSecond(8));
				}
				else {
					counter++;
				}
			}
			else {
				counter = 0;
			}
		}
	}
	bool CheckArmorActive() => Player.GetModPlayer<RoguelikeArmorPlayer>().ArmorSetCheck(Player.GetModPlayer<RichMahoganyArmorPlayer>());
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		OnHitNPC_Armor(npc);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitNPC_Armor(proj);
	}
	private void OnHitNPC_Armor(Entity entity) {
		if (!Hit) {
			return;
		}
		int damage = 12;
		if (CheckArmorActive()) {
			damage += Player.HeldItem.damage / 4;
			damage += 10;
		}
		for (int i = 0; i < 10; i++) {
			Vector2 spread = Vector2.One.Vector2DistributeEvenly(10f, 360, i);
			int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, spread * 2f, ProjectileID.BladeOfGrass, damage, 1f, Player.whoAmI);
			Main.projectile[proj].penetrate = -1;
		}
	}
}
class RichMahoganyArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("RichMahoganyArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 6;
		Player.moveSpeed += .30f;
		Player.ModPlayerStats().UpdateSentry.Base += 1;
		Player.ModPlayerStats().UpdateMinion.Base += 1;
	}
}
