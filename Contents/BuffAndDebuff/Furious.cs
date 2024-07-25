using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.BuffAndDebuff {
	public class Furious : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.Defense, -.5f);
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, -.25f);
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.5f);
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 50);
			if (player.buffTime[buffIndex] == 0) {
				player.AddBuff(ModContent.BuffType<FuriousCoolDown>(), 420);
			}
		}
	}
}
