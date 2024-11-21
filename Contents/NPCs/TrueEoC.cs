using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.NPCs {
	internal class TrueEoC : ModNPC {
		public override string Texture => BossRushUtils.GetVanillaTexture<NPC>(NPCID.MoonLordFreeEye);
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("True Eye Of Cthulhu");
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.MoonLordFreeEye];
		}
		public override void SetDefaults() {
			NPC.defense = 0;
			NPC.damage = 60;
			NPC.lifeMax = 100;
			NPC.width = 60;
			NPC.height = 60;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.npcSlots = 0f;
			NPC.noGravity = true;
			NPC.dontTakeDamage = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			AnimationType = NPCID.MoonLordFreeEye;
		}
		public override void AI() {
			Vector2 val29 = default;
			if (Main.rand.NextBool(420)) {
				//SoundEngine.PlaySound(29, (int)NPC.Center.X, (int)NPC.Center.Y, Main.rand.Next(100, 101));
			}
			Vector2 value20 = new(30);
			float num286 = 0f;
			float OldAI = NPC.ai[0];
			NPC.ai[1]++;
			int num288 = 0;
			int num289 = 0;
			for (; num288 < 10; num288++) {
				num286 = NPC.MoonLordAttacksArray2[1, num288];
				if (!(num286 + num289 <= NPC.ai[1])) {
					break;
				}
				num289 += (int)num286;
			}
			if (num288 == 10) {
				num288 = 0;
				NPC.ai[1] = 0f;
				num286 = NPC.MoonLordAttacksArray2[1, num288];
				num289 = 0;
			}
			NPC.ai[0] = NPC.MoonLordAttacksArray2[0, num288];
			float num285 = (int)NPC.ai[1] - num289;
			if (NPC.ai[0] != OldAI) {
				NPC.netUpdate = true;
			}
			if (NPC.ai[0] == -1f) {
				AI0();
			}
			if (NPC.ai[0] == 0f) {
				AI1();
			}
			else if (NPC.ai[0] == 1f) {
				AI2(num285, num286, value20);
			}
			else if (NPC.ai[0] == 2f) {
				AI3(num285, val29);
			}
			else if (NPC.ai[0] == 3f) {
				AI4(num285, value20);
			}
			else {
				AI5(num285, num286, val29, value20);
			}
		}
		private void AI0() {
			NPC.ai[1]++;
			if (NPC.ai[1] > 180f) {
				NPC.ai[1] = 0f;
			}
			float num290;
			if (NPC.ai[1] < 60f) {
				num290 = 0.75f;
				NPC.localAI[0] = 0f;
				NPC.localAI[1] = (float)Math.Sin(NPC.ai[1] * ((float)Math.PI * 2f) / 15f) * 0.35f;
				if (NPC.localAI[1] < 0f) {
					NPC.localAI[0] = (float)Math.PI;
				}
			}
			else if (NPC.ai[1] < 120f) {
				num290 = 1f;
				if (NPC.localAI[1] < 0.5f) {
					NPC.localAI[1] += 0.025f;
				}
				NPC.localAI[0] += (float)Math.PI / 15f;
			}
			else {
				num290 = 1.15f;
				NPC.localAI[1] -= 0.05f;
				if (NPC.localAI[1] < 0f) {
					NPC.localAI[1] = 0f;
				}
			}
			NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], num290, 0.3f);
		}
		private void AI1() {
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(false);
			Vector2 v9 = player.Center + player.velocity * 20f - NPC.Center;
			NPC.localAI[0] = NPC.localAI[0].AngleLerp(v9.ToRotation(), 0.5f);
			NPC.localAI[1] += 0.05f;
			if (NPC.localAI[1] > 0.7f) {
				NPC.localAI[1] = 0.7f;
			}
			NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], 1f, 0.2f);
			Vector2 DistanceBetweenNPCtoPlayer = player.Center - NPC.Center;
			Vector2 vector104 = DistanceBetweenNPCtoPlayer - Vector2.UnitY * 200f;
			vector104 = Vector2.Normalize(vector104) * 24f;//scale factor
			NPC.velocity.X = (NPC.velocity.X * 29 + vector104.X) / 30;
			NPC.velocity.Y = (NPC.velocity.Y * 29 + vector104.Y) / 30;
			float num293 = 0.25f;
			for (int i = 0; i < Main.maxNPCs; i++) {
				if (i != NPC.whoAmI
					&& Main.npc[i].active
					&& Main.npc[i].type == NPC.type
					&& Vector2.Distance(NPC.Center, Main.npc[i].Center) < 150f) {
					if (NPC.position.X < Main.npc[i].position.X) {
						NPC.velocity.X -= num293;
					}
					else {
						NPC.velocity.X += num293;
					}
					if (NPC.position.Y < Main.npc[i].position.Y) {
						NPC.velocity.Y -= num293;
					}
					else {
						NPC.velocity.Y += num293;
					}
				}
			}
		}
		private void AI2(float num285, float num286, Vector2 value20) {
			if (num285 == 0f) {
				NPC.TargetClosest(faceTarget: false);
				NPC.netUpdate = true;
			}
			NPC.velocity *= 0.95f;
			if (NPC.velocity.Length() < 1f) {
				NPC.velocity = Vector2.Zero;
			}
			Vector2 v10 = Main.player[NPC.target].Center + Main.player[NPC.target].velocity * 20f - NPC.Center;
			NPC.localAI[0] = NPC.localAI[0].AngleLerp(v10.ToRotation(), 0.5f);
			NPC.localAI[1] += 0.05f;
			if (NPC.localAI[1] > 1f) {
				NPC.localAI[1] = 1f;
			}
			if (num285 < 20f) {
				NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], 1.1f, 0.2f);
			}
			else {
				NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], 0.4f, 0.2f);
			}
			if (num285 == num286 - 35f) {
				//SoundEngine.PlaySound(4, (int)NPC.position.X, (int)NPC.position.Y, 6);
			}
			if ((num285 == num286 - 14f || num285 == num286 - 7f || num285 == num286)
				&& Main.netMode != NetmodeID.MultiplayerClient) {
				Vector2 vector105 = Utils.Vector2FromElipse(NPC.localAI[0].ToRotationVector2(), value20 * NPC.localAI[1]);
				Vector2 vector106 = Vector2.Normalize(v10) * 8f;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + vector105.X, NPC.Center.Y + vector105.Y, vector106.X, vector106.Y, ProjectileID.PhantasmalBolt, 35, 0f, Main.myPlayer);
			}
		}
		private void AI3(float num285, Vector2 val29) {
			if (num285 < 15f) {
				NPC.localAI[1] -= 0.07f;
				if (NPC.localAI[1] < 0f) {
					NPC.localAI[1] = 0f;
				}
				NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], 0.4f, 0.2f);
				NPC.velocity *= 0.8f;
				if (NPC.velocity.Length() < 1f) {
					NPC.velocity = Vector2.Zero;
				}
			}
			else if (num285 < 75f) {
				float num295 = (num285 - 15f) / 10f;
				int num296 = 0;
				int num297 = 0;
				switch ((int)num295) {
					case 0:
						num296 = 0;
						num297 = 2;
						break;
					case 1:
						num296 = 2;
						num297 = 5;
						break;
					case 2:
						num296 = 5;
						num297 = 3;
						break;
					case 3:
						num296 = 3;
						num297 = 1;
						break;
					case 4:
						num296 = 1;
						num297 = 4;
						break;
					case 5:
						num296 = 4;
						num297 = 0;
						break;
				}
				Vector2 spinningpoint13 = Vector2.UnitY * -30f;
				double radians25 = num296 * ((float)Math.PI * 2f) / 6f;
				Vector2 value23 = spinningpoint13.RotatedBy(radians25, val29);
				double radians26 = num297 * ((float)Math.PI * 2f) / 6f;
				Vector2 value24 = spinningpoint13.RotatedBy(radians26, val29);
				Vector2 vector107 = Vector2.Lerp(value23, value24, num295 - (float)(int)num295);
				float value25 = vector107.Length() / 30f;
				NPC.localAI[0] = vector107.ToRotation();
				NPC.localAI[1] = MathHelper.Lerp(NPC.localAI[1], value25, 0.5f);
				for (int num298 = 0; num298 < 2; num298++) {
					Vector2 val83 = NPC.Center + vector107 - Vector2.One * 4f;
					Color newColor = default;
					int num299 = Dust.NewDust(val83, 0, 0, DustID.Vortex, 0f, 0f, 0, newColor);
					Dust dust22 = Main.dust[num299];
					Dust dust90 = dust22;
					dust90.velocity += vector107 / 15f;
					Main.dust[num299].noGravity = true;
				}
				if ((num285 - 15f) % 10f == 0f
					&& Main.netMode != NetmodeID.MultiplayerClient) {
					Vector2 vec4 = Vector2.Normalize(vector107);
					if (vec4.HasNaNs()) {
						vec4 = Vector2.UnitY * -1f;
					}
					vec4 *= 4f;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + vector107.X, NPC.Center.Y + vector107.Y, vec4.X, vec4.Y, ProjectileID.PhantasmalSphere, 55, 0f, Main.myPlayer, 30f, NPC.whoAmI);
				}
			}
			else if (num285 < 105f) {
				NPC.localAI[0] = NPC.localAI[0].AngleLerp(NPC.ai[2] - (float)Math.PI / 2f, 0.2f);
				NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], 0.75f, 0.2f);
				if (num285 == 75f) {
					NPC.TargetClosest(faceTarget: false);
					NPC.netUpdate = true;
					NPC.velocity = Vector2.UnitY * -7f;
					for (int num301 = 0; num301 < 1000; num301++) {
						Projectile projectile8 = Main.projectile[num301];
						if (projectile8.active && projectile8.type == 454 && projectile8.ai[1] == NPC.whoAmI && projectile8.ai[0] != -1f) {
							Projectile projectile9 = projectile8;
							Projectile projectile11 = projectile9;
							projectile11.velocity += NPC.velocity;
							projectile8.netUpdate = true;
						}
					}
				}
				NPC.velocity.Y *= 0.96f;
				NPC.ai[2] = (Main.player[NPC.target].Center - NPC.Center).ToRotation() + (float)Math.PI / 2f;
				NPC.rotation = NPC.rotation.AngleTowards(NPC.ai[2], (float)Math.PI / 30f);
			}
			else if (num285 < 120f) {
				//SoundEngine.PlaySound(29, (int)NPC.Center.X, (int)NPC.Center.Y, 102);
				if (num285 == 105f) {
					NPC.netUpdate = true;
				}
				Vector2 velocity7 = (NPC.ai[2] - (float)Math.PI / 2f).ToRotationVector2() * 12f;
				NPC.velocity = velocity7 * 2f;
				for (int num303 = 0; num303 < 1000; num303++) {
					Projectile projectile10 = Main.projectile[num303];
					if (projectile10.active && projectile10.type == 454 && projectile10.ai[1] == NPC.whoAmI && projectile10.ai[0] != -1f) {
						projectile10.ai[0] = -1f;
						projectile10.velocity = velocity7;
						projectile10.netUpdate = true;
					}
				}
			}
			else {
				NPC.velocity *= 0.92f;
				NPC.rotation = NPC.rotation.AngleLerp(0f, 0.2f);
			}
		}
		private void AI4(float num285, Vector2 value20) {
			if (num285 < 15f) {
				NPC.localAI[1] -= 0.07f;
				if (NPC.localAI[1] < 0f) {
					NPC.localAI[1] = 0f;
				}
				NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], 0.4f, 0.2f);
				NPC.velocity *= 0.9f;
				if (NPC.velocity.Length() < 1f) {
					NPC.velocity = Vector2.Zero;
				}
			}
			else if (num285 < 45f) {
				NPC.localAI[0] = 0f;
				NPC.localAI[1] = (float)Math.Sin((num285 - 15f) * ((float)Math.PI * 2f) / 15f) * 0.5f;
				if (NPC.localAI[1] < 0f) {
					NPC.localAI[0] = (float)Math.PI;
				}
			}
			else if (num285 < 185f) {
				if (num285 == 45f) {
					NPC.ai[2] = Main.rand.NextBool(2).ToDirectionInt() * ((float)Math.PI * 2f) / 40f;
					NPC.netUpdate = true;
				}
				if ((num285 - 15f - 30f) % 40f == 0f) {
					NPC.ai[2] *= 0.95f;
				}
				NPC.localAI[0] += NPC.ai[2];
				NPC.localAI[1] += 0.05f;
				if (NPC.localAI[1] > 1f) {
					NPC.localAI[1] = 1f;
				}
				Vector2 vector108 = NPC.localAI[0].ToRotationVector2() * value20 * NPC.localAI[1];
				float scaleFactor8 = MathHelper.Lerp(8f, 20f, (num285 - 15f - 30f) / 140f);
				NPC.velocity = Vector2.Normalize(vector108) * scaleFactor8;
				NPC.rotation = NPC.rotation.AngleLerp(NPC.velocity.ToRotation() + (float)Math.PI / 2f, 0.2f);
				if ((num285 - 15f - 30f) % 10f == 0f
					&& Main.netMode != NetmodeID.MultiplayerClient) {
					Vector2 vector109 = NPC.Center + Vector2.Normalize(vector108) * value20.Length() * 0.4f;
					Vector2 vector110 = Vector2.Normalize(vector108) * 8f;
					float ai3 = ((float)Math.PI * 2f * (float)Main.rand.NextDouble() - (float)Math.PI) / 30f + (float)Math.PI / 180f * NPC.ai[2];
					Projectile.NewProjectile(NPC.GetSource_FromAI(), vector109.X, vector109.Y, vector110.X, vector110.Y, ProjectileID.PhantasmalEye, 35, 0f, Main.myPlayer, 0f, ai3);
				}
			}
			else {
				NPC.velocity *= 0.88f;
				NPC.rotation = NPC.rotation.AngleLerp(0f, 0.2f);
				NPC.localAI[1] -= 0.07f;
				if (NPC.localAI[1] < 0f) {
					NPC.localAI[1] = 0f;
				}
				NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], 1f, 0.2f);
			}
		}
		private void AI5(float num285, float num286, Vector2 val29, Vector2 value20) {
			if (NPC.ai[0] != 4f) {
				return;
			}
			if (num285 == 0f) {
				NPC.TargetClosest(faceTarget: false);
				NPC.netUpdate = true;
			}
			if (num285 < 180f) {
				NPC.localAI[2] = MathHelper.Lerp(NPC.localAI[2], 1f, 0.2f);
				NPC.localAI[1] -= 0.05f;
				if (NPC.localAI[1] < 0f) {
					NPC.localAI[1] = 0f;
				}
				NPC.velocity *= 0.95f;
				if (NPC.velocity.Length() < 1f) {
					NPC.velocity = Vector2.Zero;
				}
				if (!(num285 >= 60f)) {
					return;
				}
				Vector2 center20 = NPC.Center;
				int num304 = 0;
				if (num285 >= 120f) {
					num304 = 1;
				}
				for (int num305 = 0; num305 < 1 + num304; num305++) {
					int num306 = 229;
					float num307 = 0.8f;
					if (num305 % 2 == 1) {
						num306 = 229;
						num307 = 1.65f;
					}
					Vector2 vector111 = center20 + ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * value20 * .5f;
					Vector2 val84 = vector111 - Vector2.One * 8f;
					int num1715 = num306;
					float speedX27 = NPC.velocity.X * .5f;
					float speedY26 = NPC.velocity.Y * .5f;
					Color newColor = default;
					int num308 = Dust.NewDust(val84, 16, 16, num1715, speedX27, speedY26, 0, newColor);
					Main.dust[num308].velocity = Vector2.Normalize(center20 - vector111) * 3.5f * (10f - num304 * 2f) * .1f;
					Main.dust[num308].noGravity = true;
					Main.dust[num308].scale = num307;
					Main.dust[num308].customData = NPC;
				}
			}
			else if (num285 < num286 - 15f) {
				if (num285 == 180f
					&& Main.netMode != NetmodeID.MultiplayerClient) {
					NPC.TargetClosest(faceTarget: false);
					Vector2 spinningpoint2 = Main.player[NPC.target].Center - NPC.Center;
					spinningpoint2.Normalize();
					float num309 = -1f;
					if (spinningpoint2.X < 0f) {
						num309 = 1f;
					}
					double radians27 = (0f - num309) * ((float)Math.PI * 2f) / 6f;
					spinningpoint2 = spinningpoint2.RotatedBy(radians27, val29);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, spinningpoint2, ProjectileID.PhantasmalDeathray, 50, 0f, Main.myPlayer, num309 * ((float)Math.PI * 2f) / 540f, NPC.whoAmI);
					NPC.ai[2] = (spinningpoint2.ToRotation() + (float)Math.PI * 3f) * num309;
					NPC.netUpdate = true;
				}
				NPC.localAI[1] += 0.05f;
				if (NPC.localAI[1] > 1f) {
					NPC.localAI[1] = 1f;
				}
				float num310 = (NPC.ai[2] >= 0f).ToDirectionInt();
				float num311 = NPC.ai[2];
				if (num311 < 0f) {
					num311 *= -1f;
				}
				num311 += (float)Math.PI * -3f;
				num311 += num310 * ((float)Math.PI * 2f) / 540f;
				NPC.localAI[0] = num311;
				NPC.ai[2] = (num311 + (float)Math.PI * 3f) * num310;
			}
			else {
				NPC.localAI[1] -= 0.07f;
				if (NPC.localAI[1] < 0f) {
					NPC.localAI[1] = 0f;
				}
			}
		}
		public override Color? GetAlpha(Color drawColor) {
			int num2 = (int)Math.Clamp(drawColor.R * 1.5, 0, 255);
			int num3 = (int)Math.Clamp(drawColor.G * 1.5, 0, 255);
			int num4 = (int)Math.Clamp(drawColor.B * 1.5, 0, 255);
			int num5 = drawColor.A - NPC.alpha;
			return new Color(num2, num3, num4, num5);
		}
	}
}
