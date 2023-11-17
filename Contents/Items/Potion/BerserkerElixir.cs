using BossRush.Contents.BuffAndDebuff;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Potion {
	internal class BerserkerElixir : ModItem {
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
			Item.buffType = ModContent.BuffType<BerserkBuff>();
			Item.buffTime = 12000;
		}
	}
	internal class BerserkBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage(DamageClass.Melee) *= 1.5f;
			player.GetAttackSpeed(DamageClass.Melee) *= 1.5f;

			player.GetDamage(DamageClass.Ranged) *= 0.1f;
			player.GetDamage(DamageClass.Summon) *= 0.1f;
			player.GetDamage(DamageClass.Magic) *= 0.1f;
		}
	}
}
