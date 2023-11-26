using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Items.Chest;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifacts
{
    internal class EternalWealthArtifact : Artifact
    {
        public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
		public override Color DisplayNameColor => Color.LightGoldenrodYellow;
		public override bool CanBeSelected(Player player) => false;
	}
	class EternalWealthPlayer : ModPlayer {
		bool EternalWealth = false;

		int timer = 0;
		Vector2[] oldPos = new Vector2[5];
		int counterOldPos = 0;
		int MidasInfection = 0;
		public override void ResetEffects() {
			EternalWealth = Player.HasArtifact<EternalWealthArtifact>();
		}
		protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
		public override void PostUpdate() {
			if (EternalWealth) {
				chestmodplayer.finalMultiplier += 2;
				timer = BossRushUtils.CoolDown(timer);
				if (timer <= 0) {
					if (counterOldPos >= oldPos.Length - 1) {
						counterOldPos = 0;
					}
					else {
						counterOldPos++;
					}
					oldPos[counterOldPos] = Player.Center;
					timer = 600;
				}
				float distance = 500;
				bool IsInField = false;
				foreach (Vector2 vec in oldPos) {
					if (Player.Center.IsCloseToPosition(vec, distance)) {
						IsInField = true;
						MidasInfection++;
						if (MidasInfection >= 180)
							Player.statLife = Math.Clamp(Player.statLife - 1, 1, Player.statLifeMax2);
					}
					for (int i = 0; i < 25; i++) {
						int dust = Dust.NewDust(vec + Main.rand.NextVector2Circular(distance, distance), 0, 0, DustID.GoldCoin);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
					}
					for (int i = 0; i < 25; i++) {
						int dust = Dust.NewDust(vec + Main.rand.NextVector2CircularEdge(distance, distance), 0, 0, DustID.GoldCoin);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
					}
				}
				if (!IsInField)
					MidasInfection = BossRushUtils.CoolDown(MidasInfection);
			}
		}
	}
}
