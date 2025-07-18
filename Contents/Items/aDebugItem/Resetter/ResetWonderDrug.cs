﻿using BossRush.Contents.Items.Consumable.Potion;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem.Resetter {
	internal class ResetWonderDrug : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.Set_DebugItem(true);
		}
		public override bool? UseItem(Player player) {
			player.GetModPlayer<WonderDrugPlayer>().DrugDealer = 0;
			player.statLife += player.statLifeMax2 - player.statLife;
			return true;
		}
	}
}
