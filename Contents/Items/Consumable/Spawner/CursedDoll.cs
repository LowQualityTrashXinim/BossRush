using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Consumable.Spawner {
	public class CursedDoll : BaseSpawnerItem {
		public override int[] NPCtypeToSpawn => new int[] { NPCID.SkeletronHead };
		public override void PostSetStaticDefaults() {
			NPCID.Sets.MPAllowedEnemies[NPCID.SkeletronHand] = true;
		}
		public override void SetSpawnerDefault(out int width, out int height) {
			height = 32;
			width = 32;
		}
		public override bool UseSpecialSpawningMethod => true;
		public override void SpecialSpawningLogic(Player player) {
			int spawnY = 750;
			NPC.SpawnBoss((int)player.Center.X, (int)(player.Center.Y - spawnY), NPCtypeToSpawn[0], player.whoAmI);
			for (int i = 0; i <= spawnY; i++) {
				Dust.NewDustPerfect(player.Center - new Vector2((float)(Math.Sin(i * 8) * (i * 0.25)), i * 1.2f), DustID.PurpleTorch);
				Dust.NewDustPerfect(new Vector2(player.Center.X, player.Center.Y - spawnY), DustID.BoneTorch, Main.rand.NextVector2Unit() * 65);
				Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen);
			}
		}
		public override bool CanUseItem(Player player) {
			if (Main.IsItDay()) {
				Main.time = Main.dayLength;
			}
			return !Main.IsItDay();
		}
	}
}
