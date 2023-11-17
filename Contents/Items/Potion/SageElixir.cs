using BossRush.Contents.BuffAndDebuff;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Potion {
	internal class SageElixir : ModItem {
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
			Item.buffType = ModContent.BuffType<SageBuff>();
			Item.buffTime = 12000;
		}
	}
	internal class SageBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage(DamageClass.Ranged) *= 0.1f;
			player.GetDamage(DamageClass.Summon) *= 0.1f;
			player.GetDamage(DamageClass.Melee) *= 0.1f;

			player.manaCost *= 0.1f;
			player.manaRegen += 50;
			player.statManaMax2 += 100;
			player.manaRegenBonus += 150;
			player.manaRegenDelay = (int)(player.manaRegenDelay * 0.35f);
			player.manaRegenDelayBonus = (int)(player.manaRegenDelayBonus * 0.35f);
			player.GetDamage(DamageClass.Magic) += .5f;
		}
	}
}
