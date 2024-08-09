using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.SynergyAccessories;
internal class ArcaneMaster : SynergyModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory(30, 30);
		Item.value = 1000000;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MagicDMG, Multiplicative: 1.5f);
		player.manaCost -= .5f;
		player.manaMagnet = true;
		player.manaFlower = true;
		player.starCloakItem_manaCloakOverrideItem = Item;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.ArcaneFlower)
			.AddIngredient(ItemID.MagnetFlower)
			.AddIngredient(ItemID.ManaCloak)
			.Register();
	}
}
class ArcaneMasterPlayer : ModPlayer {
	public bool ArcaneMaster = false;
	public int ManaCostIncreases = 0;
	public int Decay_ManaCostIncreases = 0;
	public override void ResetEffects() {
		ArcaneMaster = true;
		if (ManaCostIncreases <= 0 || Player.ItemAnimationActive) {
			return;
		}
		if (++Decay_ManaCostIncreases >= 60) {
			ManaCostIncreases--;
			Decay_ManaCostIncreases = 0;
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (ArcaneMaster && item.DamageType == DamageClass.Magic) {
			damage += ManaCostIncreases * .05f;
		}
	}
	public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
		if (ArcaneMaster) {
			mult += ManaCostIncreases * .1f;
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (ArcaneMaster) {
			ManaCostIncreases = Math.Clamp(ManaCostIncreases + 1, 0, 60);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
