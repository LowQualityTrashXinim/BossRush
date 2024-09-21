using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.BuffAndDebuff.PlayerDebuff;
internal class Drawback : ModBuff{
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(true);
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Flat: -10);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, -.25f);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, -.2f);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, -.15f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: -10);
	}
}
