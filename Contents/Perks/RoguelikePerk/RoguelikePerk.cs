using BossRush.Common.ChallengeMode;
using BossRush.Common.Global;
using BossRush.Common.Systems.Mutation;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Perks.RoguelikePerk;
public class SuppliesDrop : Perk {
	public override void SetDefaults() {
		textureString = BossRushTexture.SUPPILESDROP;
		CanBeStack = true;
		StackLimit = -1;
		CanBeChoosen = false;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void OnChoose(Player player) {
		LootBoxBase.GetWeapon(out int weapon, out int amount);
		player.QuickSpawnItem(player.GetSource_FromThis(), weapon, amount);
	}
}
public class GiftOfRelic : Perk {
	public override void SetDefaults() {
		textureString = BossRushTexture.Get_MissingTexture("Perk");
		CanBeStack = true;
		StackLimit = -1;
		CanBeChoosen = false;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void OnChoose(Player player) {
		player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Relic>());
	}
}
public class WeaponDismantle : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PerkPlayer>().perk_DismantleWeapon = true;
	}
}
public class EssenceExtraction : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 999;
	}
	public override bool SelectChoosing() {
		return !ModContent.GetInstance<BossRushWorldGen>().BossRushWorld;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PerkPlayer>().perk_DismantleWeapon = true;
	}
}
public class PeaceWithGod : Perk {
	public override void SetDefaults() {
		textureString = BossRushUtils.GetTheSameTextureAsEntity<PeaceWithGod>();
		CanBeStack = false;
	}
	public override bool SelectChoosing() {
		return false;
	}
	public override void ResetEffect(Player player) {
		player.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonusBlock = true;
		player.GetModPlayer<PlayerStatsHandle>().CanDropSynergyEnergy = true;
	}
}
public class LostInWonderLand : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 10;
	}
	public override bool SelectChoosing() {
		return false;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		ModContent.GetInstance<MutationSystem>().MutationChance += .1f * StackAmount(player);
		modplayer.AugmentationChance += .05f * StackAmount(player);
		modplayer.RandomizeChanceEnchantment += .05f * StackAmount(player);
	}
}
