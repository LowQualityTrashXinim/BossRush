using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Potion {
	internal class CommanderElixir : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultPotion(20, 26, ModContent.BuffType<LeaderShip>(), 12000);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 25);
		}
	}
	internal class LeaderShip : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.maxMinions += 5;
			player.whipRangeMultiplier += .25f;
			player.GetDamage(DamageClass.Summon).Base += 5;

			player.GetDamage(DamageClass.Ranged) -= 0.1f;
			player.GetDamage(DamageClass.Melee) -= 0.1f;
			player.GetDamage(DamageClass.Magic) -= 0.1f;
		}
	}
}
