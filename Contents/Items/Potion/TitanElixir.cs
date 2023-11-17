using BossRush.Contents.BuffAndDebuff;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Potion {
	internal class TitanElixir : ModItem {
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.rare = ItemRarityID.Orange;
			Item.buffType = ModContent.BuffType<Protection>();
			Item.buffTime = 12000;
		}
	}
	internal class Protection : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("The Titan's Protection");
			// Description.SetDefault("This newfound aegis is almost... suffocating...");
			Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.endurance += 0.45f;
			player.statLifeMax2 += 400;
			player.statDefense += 25;

			player.GetDamage(DamageClass.Generic) *= 0.65f;

			player.moveSpeed *= 0.5f;
			player.maxRunSpeed = 0.5f;
			player.runAcceleration *= 0.5f;
			player.jumpSpeedBoost *= 0.5f;
			player.accRunSpeed *= 0.5f;
		}
	}
}
