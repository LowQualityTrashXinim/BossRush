using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.StickySlime {
	public class StickyFriend : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("slimy ghost friend");
			// Description.SetDefault("Will tries his best");

			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
		}

		public override void Update(Player player, ref int buffIndex) {
			// If the minions exist reset the buff time, otherwise remove the buff from the player
			if (player.ownedProjectileCounts[ModContent.ProjectileType<GhostSlime>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
