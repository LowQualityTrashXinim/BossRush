﻿using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;
internal class DivineHammer : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.rare = ItemRarityID.Red;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item37;
	}
	public override bool? UseItem(Player player) {
		if (!UniversalSystem.CanEnchantmentBeAccess()) {
			BossRushUtils.CombatTextRevamp(player.Hitbox, Color.Red, "Can't access enchantment ui");
			return false;
		}
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateEnchantmentUI();
		}
		return false;
	}
}
