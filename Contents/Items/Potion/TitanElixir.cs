using BossRush.Contents.BuffAndDebuff;
using BossRush.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Potion {
	internal class TitanElixir : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultPotion(20, 26, ModContent.BuffType<Protection>(), 12000);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 25);
		}
	}
	public class TitanElixir_ModPlayer : ModPlayer {
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (Player.HasBuff(ModContent.BuffType<Protection>())) {
				Player.ClearBuff(ModContent.BuffType<Protection>());
				Player.Heal(Player.statLifeMax2);
				return false;
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}
	}
	internal class Protection : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.endurance += 0.45f;
			player.statLifeMax2 += 400;
			player.statDefense += 45;

			player.GetDamage(DamageClass.Generic) -= 0.25f;

			player.moveSpeed *= .75f;
			player.maxRunSpeed = .75f;
			player.runAcceleration *= .75f;
			player.jumpSpeedBoost *= .75f;
			player.accRunSpeed *= .75f;
		}
	}
}
