using BossRush.Common.Systems;
using BossRush.Common.Systems.Achievement;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Systems.ObjectSystem;
using BossRush.Contents.Items.Chest;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Transfixion.Artifacts {
	internal class EternalWealthArtifact : Artifact {
		public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
		public override Color DisplayNameColor => Color.LightGoldenrodYellow;
		public override bool CanBeSelected(Player player) => AchievementSystem.IsAchieved("TokenOfGreed");
	}
	class EternalWealthPlayer : ModPlayer {
		bool EternalWealth = false;

		int timer = 0;
		ModObject[] objs = new ModObject[5];
		int counterOldPos = 0;
		public int MidasInfection = 0;
		public bool Midas_IsInField = false;
		public override void ResetEffects() {
			EternalWealth = Player.HasArtifact<EternalWealthArtifact>();
			Midas_IsInField = false;
		}
		public override void PostUpdate() {
			if (EternalWealth) {
				Player.GetModPlayer<ChestLootDropPlayer>().DropModifier *= 3;
				timer = BossRushUtils.CountDown(timer);
				if (timer <= 0) {
					counterOldPos = BossRushUtils.Safe_SwitchValue(counterOldPos, objs.Length - 1);
					objs[counterOldPos] = ModObject.NewModObject(Player.Center, Vector2.Zero, ModObject.GetModObjectType<EternalWealth_ModObject>());
					timer = 600;
				}
				float distance = 500;
				foreach (EternalWealth_ModObject obj in objs) {
					if (obj == null) {
						continue;
					}
					if (Player.Center.IsCloseToPosition(obj.position, distance)) {
						Midas_IsInField = true;
						MidasInfection++;
						if (MidasInfection >= 180)
							Player.statLife = Math.Clamp(Player.statLife - 1, 1, Player.statLifeMax2);
					}
				}
				if (!Midas_IsInField)
					MidasInfection = BossRushUtils.CountDown(MidasInfection);
			}
		}
	}
	public class EternalWealth_ModObject : ModObject {
		public override void SetDefaults() {
			timeLeft = 600;
		}
		public override void AI() {
			float distance = 500;
			for (int i = 0; i < 25; i++) {
				int dust = Dust.NewDust(position + Main.rand.NextVector2Circular(distance, distance), 0, 0, DustID.GoldCoin);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
			}
			for (int i = 0; i < 25; i++) {
				int dust = Dust.NewDust(position + Main.rand.NextVector2CircularEdge(distance, distance), 0, 0, DustID.GoldCoin);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
			}
		}
	}
}
