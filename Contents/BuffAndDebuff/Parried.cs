using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff {
	internal class Parried : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage(DamageClass.Melee) += 5f;
			if (player.buffTime[buffIndex] == 0) {
				player.AddBuff(ModContent.BuffType<CoolDownParried>(), 360);
			}
		}

	}
	internal class CoolDownParried : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
			Main.buffNoSave[Type] = true;
		}
	}
}
