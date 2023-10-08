using BossRush.Common.RoguelikeChange;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff {
	internal class GodVision : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetCritChance(DamageClass.Generic) += 70;
			player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify -= 100;
		}
	}
}
