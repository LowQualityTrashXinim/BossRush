using BossRush.Contents.Items.Accessories.SynergyAccessories.FuryEmblem;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff {
	public class Furious : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.statDefense -= 100000;
			player.lifeRegen = 0;
			player.lifeRegenCount = 0;
			player.lifeRegenTime = 0;
			player.GetDamage(DamageClass.Generic) += 1.5f;
			player.GetCritChance(DamageClass.Generic) += 50;
			if (player.buffTime[buffIndex] == 0) {
				player.AddBuff(ModContent.BuffType<FuriousCoolDown>(), 420);
				player.GetModPlayer<FuryPlayer>().CooldownFurious = true;
			}
		}
	}
}
