using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff {
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