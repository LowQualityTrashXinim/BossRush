using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.BuffAndDebuff.PlayerDebuff {
	class LifeLoss : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxHP, .5f);
		}
	}
}
