using BossRush.Contents.BuffAndDebuff;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Potion {
	internal class CommanderElixir : ModItem {
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
			Item.buffType = ModContent.BuffType<LeaderShip>();
			Item.buffTime = 12000;
		}
	}
	internal class LeaderShip : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("The Commander's Patience");
			// Description.SetDefault("Fighting alongside a horde has never been easier!");
			Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.maxMinions += 5;
			player.whipRangeMultiplier *= 1.25f;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1.5f;

			player.GetDamage(DamageClass.Ranged) *= 0.1f;
			player.GetDamage(DamageClass.Melee) *= 0.1f;
			player.GetDamage(DamageClass.Magic) *= 0.1f;
		}
	}
}
