using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Potion {
	internal class BerserkerElixir : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultPotion(20, 26, ModContent.BuffType<BerserkBuff>(), 12000);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 25);
		}
	}
	public class BerserkerElixir_ModPlayer : ModPlayer {
		public override void ModifyItemScale(Item item, ref float scale) {
			if (Player.HasBuff(ModContent.BuffType<BerserkBuff>()) && item.DamageType == DamageClass.Melee) {
				scale += .3f;
			}
		}
	}
	internal class BerserkBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage(DamageClass.Melee) += .2f;
			player.GetAttackSpeed(DamageClass.Melee) += .15f;

			player.GetDamage(DamageClass.Ranged) -= 0.1f;
			player.GetDamage(DamageClass.Summon) -= 0.1f;
			player.GetDamage(DamageClass.Magic) -= 0.1f;
		}
	}
}
