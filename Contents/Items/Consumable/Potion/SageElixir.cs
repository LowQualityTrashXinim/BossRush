using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Consumable.Potion {
	internal class SageElixir : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultPotion(20, 26, ModContent.BuffType<SageBuff>(), 12000);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 25);
		}
	}
	internal class SageBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage(DamageClass.Ranged) -= 0.1f;
			player.GetDamage(DamageClass.Summon) -= 0.1f;
			player.GetDamage(DamageClass.Melee) -= 0.1f;

			player.GetDamage(DamageClass.Magic) *= 1.5f;
			player.manaCost += 0.5f;
			player.statManaMax2 += 100;
			player.GetModPlayer<PlayerStatsHandle>().Rapid_ManaRegen += 1;
		}
	}
}
