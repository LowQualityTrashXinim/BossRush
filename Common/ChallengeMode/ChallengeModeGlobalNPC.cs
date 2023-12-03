using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Common.ChallengeMode {
	internal class ChallengeModeGlobalNPC : GlobalNPC {
		public override void SetDefaults(NPC entity) {
			int amount = BossRushUtils.AmountOfModCurrentlyEnable();
			entity.lifeMax *= amount;
			entity.life = entity.lifeMax;
			entity.damage *= amount;
			entity.defense *= amount;
		}
		public override bool PreAI(NPC npc) {
			return true;
		}
		public override void AI(NPC npc) {
			//if (npc.type == NPCID.KingSlime)
			//{
			//    KingSlimeAI(npc);
			//}
			base.AI(npc);
		}
		private void KingSlimeAI(NPC npc) {
			float num236 = 1f;
			float num237 = 1f;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			float num238 = 2f;
			if (Main.getGoodWorld) {
				num238 -= 1f - npc.life / (float)npc.lifeMax;
				num237 *= num238;
			}

			npc.aiAction = 0;
			if (npc.ai[3] == 0f && npc.life > 0)
				npc.ai[3] = npc.lifeMax;

			if (npc.localAI[3] == 0f) {
				npc.localAI[3] = 1f;
				flag6 = true;
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					npc.ai[0] = -100f;
					npc.TargetClosest();
					npc.netUpdate = true;
				}
			}

			int num239 = 3000;
			if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > num239) {
				npc.TargetClosest();
				if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > num239) {
					npc.EncourageDespawn(10);
					if (Main.player[npc.target].Center.X < npc.Center.X)
						npc.direction = 1;
					else
						npc.direction = -1;

					if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[1] != 5f) {
						npc.netUpdate = true;
						npc.ai[2] = 0f;
						npc.ai[0] = 0f;
						npc.ai[1] = 5f;
						npc.localAI[1] = Main.maxTilesX * 16;
						npc.localAI[2] = Main.maxTilesY * 16;
					}
				}
			}

			if (!Main.player[npc.target].dead && npc.timeLeft > 10 && npc.ai[2] >= 300f && npc.ai[1] < 5f && npc.velocity.Y == 0f) {
				npc.ai[2] = 0f;
				npc.ai[0] = 0f;
				npc.ai[1] = 5f;
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					npc.TargetClosest(false);
					Point point3 = npc.Center.ToTileCoordinates();
					Point point4 = Main.player[npc.target].Center.ToTileCoordinates();
					Vector2 vector30 = Main.player[npc.target].Center - npc.Center;
					int num240 = 10;
					int num241 = 0;
					int num242 = 7;
					int num243 = 0;
					bool flag9 = false;
					if (npc.localAI[0] >= 360f || vector30.Length() > 2000f) {
						if (npc.localAI[0] >= 360f)
							npc.localAI[0] = 360f;

						flag9 = true;
						num243 = 100;
					}

					while (!flag9 && num243 < 100) {
						num243++;
						int num244 = Main.rand.Next(point4.X - num240, point4.X + num240 + 1);
						int num245 = Main.rand.Next(point4.Y - num240, point4.Y + 1);
						if ((num245 >= point4.Y - num242 && num245 <= point4.Y + num242 && num244 >= point4.X - num242 && num244 <= point4.X + num242) || (num245 >= point3.Y - num241 && num245 <= point3.Y + num241 && num244 >= point3.X - num241 && num244 <= point3.X + num241) || Main.tile[num244, num245].HasUnactuatedTile)
							continue;

						int num246 = num245;
						int num247 = 0;
						if (Main.tile[num244, num246].HasUnactuatedTile && Main.tileSolid[Main.tile[num244, num246].TileType] && !Main.tileSolidTop[Main.tile[num244, num246].TileType]) {
							num247 = 1;
						}
						else {
							for (; num247 < 150 && num246 + num247 < Main.maxTilesY; num247++) {
								int num248 = num246 + num247;
								if (Main.tile[num244, num248].HasUnactuatedTile && Main.tileSolid[Main.tile[num244, num248].TileType] && !Main.tileSolidTop[Main.tile[num244, num248].TileType]) {
									num247--;
									break;
								}
							}
						}

						num245 += num247;
						bool flag10 = true;
						if (flag10 && (Main.tile[num244, num245].LiquidType == LiquidID.Lava))
							flag10 = false;

						if (flag10 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
							flag10 = false;

						if (flag10) {
							npc.localAI[1] = num244 * 16 + 8;
							npc.localAI[2] = num245 * 16 + 16;
							break;
						}
					}

					if (num243 >= 100) {
						Vector2 bottom = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
						npc.localAI[1] = bottom.X;
						npc.localAI[2] = bottom.Y;
					}
				}
			}

			if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) || Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 160f) {
				npc.ai[2]++;
				if (Main.netMode != NetmodeID.MultiplayerClient)
					npc.localAI[0]++;
			}
			else if (Main.netMode != NetmodeID.MultiplayerClient) {
				npc.localAI[0]--;
				if (npc.localAI[0] < 0f)
					npc.localAI[0] = 0f;
			}

			if (npc.timeLeft < 10 && (npc.ai[0] != 0f || npc.ai[1] != 0f)) {
				npc.ai[0] = 0f;
				npc.ai[1] = 0f;
				npc.netUpdate = true;
				flag7 = false;
			}

			Dust dust;
			if (npc.ai[1] == 5f) {
				flag7 = true;
				npc.aiAction = 1;
				npc.ai[0]++;
				num236 = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
				num236 = 0.5f + num236 * 0.5f;
				if (npc.ai[0] >= 60f)
					flag8 = true;

				if (npc.ai[0] == 60f)
					Gore.NewGore(npc.GetSource_FromAI(), npc.Center + new Vector2(-40f, -npc.height / 2), npc.velocity, 734);

				if (npc.ai[0] >= 60f && Main.netMode != NetmodeID.MultiplayerClient) {
					npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
					npc.ai[1] = 6f;
					npc.ai[0] = 0f;
					npc.netUpdate = true;
				}

				if (Main.netMode == NetmodeID.MultiplayerClient && npc.ai[0] >= 120f) {
					npc.ai[1] = 6f;
					npc.ai[0] = 0f;
				}

				if (!flag8) {
					for (int num249 = 0; num249 < 10; num249++) {
						int num250 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustID.TintableDust, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
						Main.dust[num250].noGravity = true;
						dust = Main.dust[num250];
						dust.velocity *= 0.5f;
					}
				}
			}
			else if (npc.ai[1] == 6f) {
				flag7 = true;
				npc.aiAction = 0;
				npc.ai[0]++;
				num236 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
				num236 = 0.5f + num236 * 0.5f;
				if (npc.ai[0] >= 30f && Main.netMode != NetmodeID.MultiplayerClient) {
					npc.ai[1] = 0f;
					npc.ai[0] = 0f;
					npc.netUpdate = true;
					npc.TargetClosest();
				}

				if (Main.netMode == NetmodeID.MultiplayerClient && npc.ai[0] >= 60f) {
					npc.ai[1] = 0f;
					npc.ai[0] = 0f;
					npc.TargetClosest();
				}

				for (int num251 = 0; num251 < 10; num251++) {
					int num252 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustID.TintableDust, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
					Main.dust[num252].noGravity = true;
					dust = Main.dust[num252];
					dust.velocity *= 2f;
				}
			}

			npc.dontTakeDamage = npc.hide = flag8;
			if (npc.velocity.Y == 0f) {
				npc.velocity.X *= 0.8f;
				if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
					npc.velocity.X = 0f;

				if (!flag7) {
					npc.ai[0] += 2f;
					if (npc.life < npc.lifeMax * 0.8f)
						npc.ai[0] += 1f;

					if (npc.life < npc.lifeMax * 0.6f)
						npc.ai[0] += 1f;

					if (npc.life < npc.lifeMax * 0.4f)
						npc.ai[0] += 2f;

					if (npc.life < npc.lifeMax * 0.2f)
						npc.ai[0] += 3f;

					if (npc.life < npc.lifeMax * 0.1f)
						npc.ai[0] += 4f;

					if (npc.ai[0] >= 0f) {
						npc.netUpdate = true;
						npc.TargetClosest();
						float jumpStrength = 0;
						float MoveSpeed = 0;
						float DelayAttack = 0;
						switch (npc.ai[1]) {
							case 2:
								jumpStrength = -6;
								MoveSpeed = 4.5f;
								DelayAttack = -120;
								npc.ai[1] += 1f;
								break;
							case 3:
								KingSlimeStartOfBigJump(npc, ref jumpStrength, ref MoveSpeed, ref DelayAttack);
								jumpStrength = -13f;
								MoveSpeed = 3.5f;
								DelayAttack = -200f;
								npc.ai[1] = 0f;
								break;
							default:
								jumpStrength = -8f;
								MoveSpeed = 4f;
								DelayAttack = -120;
								npc.ai[1] += 1f;
								break;
						}
						npc.velocity.Y = jumpStrength;
						npc.velocity.X += MoveSpeed * npc.direction;
						npc.ai[0] = DelayAttack;
					}
					else if (npc.ai[0] >= -30f) {
						npc.aiAction = 1;
					}
				}
			}
			else if (npc.target < 255) {
				float num253 = 3f;
				if (Main.getGoodWorld)
					num253 = 6f;

				if ((npc.direction == 1 && npc.velocity.X < num253) || (npc.direction == -1 && npc.velocity.X > 0f - num253)) {
					if ((npc.direction == -1 && npc.velocity.X < 0.1) || (npc.direction == 1 && npc.velocity.X > -0.1))
						npc.velocity.X += 0.2f * npc.direction;
					else
						npc.velocity.X *= 0.93f;
				}
			}
			int num254 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.TintableDust, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
			Main.dust[num254].noGravity = true;
			dust = Main.dust[num254];
			dust.velocity *= 0.5f;
			if (npc.life <= 0)
				return;

			float num255 = npc.life / (float)npc.lifeMax;
			num255 = num255 * 0.5f + 0.75f;
			num255 *= num236;
			num255 *= num237;
			if (num255 != npc.scale || flag6) {
				npc.position.X += npc.width / 2;
				npc.position.Y += npc.height;
				npc.scale = num255;
				npc.width = (int)(98f * npc.scale);
				npc.height = (int)(92f * npc.scale);
				npc.position.X -= npc.width / 2;
				npc.position.Y -= npc.height;
			}

			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;

			int num256 = (int)(npc.lifeMax * .05f);
			if (!((npc.life + num256) < npc.ai[3]))
				return;

			npc.ai[3] = npc.life;
			int num257 = Main.rand.Next(1, 4);
			for (int num258 = 0; num258 < num257; num258++) {
				int x = (int)(npc.position.X + Main.rand.NextFloat(npc.width - 32));
				int y = (int)(npc.position.Y + Main.rand.NextFloat(npc.height - 32));
				int num259 = 1;
				if (Main.expertMode && Main.rand.NextBool(4))
					num259 = 535;

				int slimeMinion = NPC.NewNPC(npc.GetSource_FromAI(), x, y, num259);
				Main.npc[slimeMinion].SetDefaults(num259);
				Main.npc[slimeMinion].velocity.X = Main.rand.NextFloat(-15, 16) * 0.1f;
				Main.npc[slimeMinion].velocity.Y = Main.rand.NextFloat(-30, 1) * 0.1f;
				Main.npc[slimeMinion].ai[0] = -1000 * Main.rand.Next(3);
				Main.npc[slimeMinion].ai[1] = 0f;
				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, slimeMinion);
			}
		}
		private void KingSlimeAttackSwitcher(NPC npc) {

		}
		private void KingSlimeStartOfBigJump(NPC npc, ref float JumpStrength, ref float MoveSpeed, ref float DelayAttack) {
			for (int i = 0; i < 16; i++) {
				Vector2 spreadoutring = Vector2.One.Vector2DistributeEvenly(16, 360, i) * 10f;
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, spreadoutring, ProjectileID.SpikedSlimeSpike, npc.damage, 4f);
			}
		}
		private void SkeletronAIHand(NPC npc) {
			npc.spriteDirection = -(int)npc.ai[0];
			if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 11) {
				npc.ai[2] += 10f;
				if (npc.ai[2] > 50f || Main.netMode != NetmodeID.Server) {
					npc.life = -1;
					npc.HitEffect();
					npc.active = false;
				}
			}

			if (npc.ai[2] == 0f || npc.ai[2] == 3f) {
				if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
					npc.EncourageDespawn(10);

				if (Main.npc[(int)npc.ai[1]].ai[1] != 0f) {
					if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 100f) {
						if (npc.velocity.Y > 0f)
							npc.velocity.Y *= 0.96f;

						npc.velocity.Y -= 0.07f;
						if (npc.velocity.Y > 6f)
							npc.velocity.Y = 6f;
					}
					else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 100f) {
						if (npc.velocity.Y < 0f)
							npc.velocity.Y *= 0.96f;

						npc.velocity.Y += 0.07f;
						if (npc.velocity.Y < -6f)
							npc.velocity.Y = -6f;
					}

					if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 120f * npc.ai[0]) {
						if (npc.velocity.X > 0f)
							npc.velocity.X *= 0.96f;

						npc.velocity.X -= 0.1f;
						if (npc.velocity.X > 8f)
							npc.velocity.X = 8f;
					}

					if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 120f * npc.ai[0]) {
						if (npc.velocity.X < 0f)
							npc.velocity.X *= 0.96f;

						npc.velocity.X += 0.1f;
						if (npc.velocity.X < -8f)
							npc.velocity.X = -8f;
					}
				}
				else {
					npc.ai[3] += 1f;
					if (Main.expertMode)
						npc.ai[3] += 0.5f;

					if (npc.ai[3] >= 300f) {
						npc.ai[2] += 1f;
						npc.ai[3] = 0f;
						npc.netUpdate = true;
					}

					if (Main.expertMode) {
						if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 230f) {
							if (npc.velocity.Y > 0f)
								npc.velocity.Y *= 0.96f;

							npc.velocity.Y -= 0.04f;
							if (npc.velocity.Y > 3f)
								npc.velocity.Y = 3f;
						}
						else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f) {
							if (npc.velocity.Y < 0f)
								npc.velocity.Y *= 0.96f;

							npc.velocity.Y += 0.04f;
							if (npc.velocity.Y < -3f)
								npc.velocity.Y = -3f;
						}

						if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0]) {
							if (npc.velocity.X > 0f)
								npc.velocity.X *= 0.96f;

							npc.velocity.X -= 0.07f;
							if (npc.velocity.X > 8f)
								npc.velocity.X = 8f;
						}

						if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0]) {
							if (npc.velocity.X < 0f)
								npc.velocity.X *= 0.96f;

							npc.velocity.X += 0.07f;
							if (npc.velocity.X < -8f)
								npc.velocity.X = -8f;
						}
					}

					if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 230f) {
						if (npc.velocity.Y > 0f)
							npc.velocity.Y *= 0.96f;

						npc.velocity.Y -= 0.04f;
						if (npc.velocity.Y > 3f)
							npc.velocity.Y = 3f;
					}
					else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f) {
						if (npc.velocity.Y < 0f)
							npc.velocity.Y *= 0.96f;

						npc.velocity.Y += 0.04f;
						if (npc.velocity.Y < -3f)
							npc.velocity.Y = -3f;
					}

					if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0]) {
						if (npc.velocity.X > 0f)
							npc.velocity.X *= 0.96f;

						npc.velocity.X -= 0.07f;
						if (npc.velocity.X > 8f)
							npc.velocity.X = 8f;
					}

					if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0]) {
						if (npc.velocity.X < 0f)
							npc.velocity.X *= 0.96f;

						npc.velocity.X += 0.07f;
						if (npc.velocity.X < -8f)
							npc.velocity.X = -8f;
					}
				}

				Vector2 vector22 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num181 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector22.X;
				float num182 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector22.Y;
				float num183 = (float)Math.Sqrt(num181 * num181 + num182 * num182);
				npc.rotation = (float)Math.Atan2(num182, num181) + 1.57f;
			}
			else if (npc.ai[2] == 1f) {
				Vector2 vector23 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num184 = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector23.X;
				float num185 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector23.Y;
				float num186;
				npc.rotation = (float)Math.Atan2(num185, num184) + 1.57f;
				npc.velocity.X *= 0.95f;
				npc.velocity.Y -= 0.1f;
				if (Main.expertMode) {
					npc.velocity.Y -= 0.06f;
					if (npc.velocity.Y < -13f)
						npc.velocity.Y = -13f;
				}
				else if (npc.velocity.Y < -8f) {
					npc.velocity.Y = -8f;
				}

				if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 200f) {
					npc.TargetClosest();
					npc.ai[2] = 2f;
					vector23 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					num184 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector23.X;
					num185 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector23.Y;
					num186 = (float)Math.Sqrt(num184 * num184 + num185 * num185);
					num186 = ((!Main.expertMode) ? (18f / num186) : (21f / num186));
					npc.velocity.X = num184 * num186;
					npc.velocity.Y = num185 * num186;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[2] == 2f) {
				if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f)
					npc.ai[2] = 3f;
			}
			else if (npc.ai[2] == 4f) {
				Vector2 vector24 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
				float num187 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width * .5f - 200f * npc.ai[0] - vector24.X;
				float num188 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector24.Y;
				float num189;
				npc.rotation = (float)Math.Atan2(num188, num187) + 1.57f;
				npc.velocity.Y *= 0.95f;
				npc.velocity.X += 0.1f * (0f - npc.ai[0]);
				if (Main.expertMode) {
					npc.velocity.X += 0.07f * (0f - npc.ai[0]);
					if (npc.velocity.X < -12f)
						npc.velocity.X = -12f;
					else if (npc.velocity.X > 12f)
						npc.velocity.X = 12f;
				}
				else if (npc.velocity.X < -8f) {
					npc.velocity.X = -8f;
				}
				else if (npc.velocity.X > 8f) {
					npc.velocity.X = 8f;
				}

				if (npc.position.X + npc.width * .5f < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width * .5f - 500f
					|| npc.position.X + npc.width * .5f > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width * .5f + 500f) {
					npc.TargetClosest();
					npc.ai[2] = 5f;
					vector24 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
					num187 = Main.player[npc.target].position.X + Main.player[npc.target].width * .5f - vector24.X;
					num188 = Main.player[npc.target].position.Y + Main.player[npc.target].height * .5f - vector24.Y;
					num189 = (float)Math.Sqrt(num187 * num187 + num188 * num188);
					num189 = ((!Main.expertMode) ? (17f / num189) : (22f / num189));
					npc.velocity.X = num187 * num189;
					npc.velocity.Y = num188 * num189;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[2] == 5f &&
				((npc.velocity.X > 0f && npc.position.X + npc.width * .5f > Main.player[npc.target].position.X + Main.player[npc.target].width * .5f) ||
				(npc.velocity.X < 0f && npc.position.X + npc.width * .5f < Main.player[npc.target].position.X + Main.player[npc.target].width * .5f))) {
				npc.ai[2] = 0f;
			}

			return;
		}
		private void SkeletronAI(NPC npc) {
			float halfWidth = npc.width * .5f, halfHeight = npc.height * .5f;
			npc.reflectsProjectiles = false;
			npc.defense = npc.defDefense;
			if (npc.ai[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient) {
				npc.TargetClosest();
				npc.ai[0] = 1f;
				if (npc.type != NPCID.DungeonGuardian) {
					int num148 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + halfWidth), (int)(npc.position.Y + halfHeight), 36, npc.whoAmI);
					Main.npc[num148].ai[0] = -1f;
					Main.npc[num148].ai[1] = npc.whoAmI;
					Main.npc[num148].target = npc.target;
					Main.npc[num148].netUpdate = true;
					num148 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + halfWidth), (int)(npc.position.Y + halfHeight), 36, npc.whoAmI);
					Main.npc[num148].ai[0] = 1f;
					Main.npc[num148].ai[1] = npc.whoAmI;
					Main.npc[num148].ai[3] = 150f;
					Main.npc[num148].target = npc.target;
					Main.npc[num148].netUpdate = true;
				}
			}

			if ((npc.type == NPCID.DungeonGuardian || Main.netMode == NetmodeID.MultiplayerClient) && npc.localAI[0] == 0f) {
				npc.localAI[0] = 1f;
				SoundEngine.PlaySound(SoundID.Roar, npc.position);
			}

			if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f) {
				npc.TargetClosest();
				if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
					npc.ai[1] = 3f;
			}

			if ((npc.type == NPCID.DungeonGuardian || Main.IsItDay()) && npc.ai[1] != 3f && npc.ai[1] != 2f) {
				npc.ai[1] = 2f;
				SoundEngine.PlaySound(SoundID.Roar, npc.position);
			}

			int num149 = 0;
			if (Main.expertMode) {
				for (int num150 = 0; num150 < 200; num150++) {
					if (Main.npc[num150].active && Main.npc[num150].type == npc.type + 1)
						num149++;
				}

				npc.defense += num149 * 25;
				if ((num149 < 2 || (double)npc.life < (double)npc.lifeMax * 0.75) && npc.ai[1] == 0f) {
					float num151 = 80f;
					if (num149 == 0)
						num151 /= 2f;

					if (Main.getGoodWorld)
						num151 *= 0.8f;

					if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] % num151 == 0f) {
						Vector2 center3 = npc.Center;
						float num152 = Main.player[npc.target].position.X + Main.player[npc.target].width * .5f - center3.X;
						float num153 = Main.player[npc.target].position.Y + Main.player[npc.target].height * .5f - center3.Y;
						float num154 = (float)Math.Sqrt(num152 * num152 + num153 * num153);
						if (Collision.CanHit(center3, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height)) {
							float num155 = 3f;
							if (num149 == 0)
								num155 += 2f;

							float num156 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - center3.X + Main.rand.Next(-20, 21);
							float num157 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - center3.Y + Main.rand.Next(-20, 21);
							float num158 = (float)Math.Sqrt(num156 * num156 + num157 * num157);
							num158 = num155 / num158;
							num156 *= num158;
							num157 *= num158;
							Vector2 vector19 = new Vector2(num156 * 1f + (float)Main.rand.Next(-50, 51) * 0.01f, num157 * 1f + (float)Main.rand.Next(-50, 51) * 0.01f);
							vector19.Normalize();
							vector19 *= num155;
							vector19 += npc.velocity;
							num156 = vector19.X;
							num157 = vector19.Y;
							int attackDamage_ForProjectiles = GetAttackDamage_ForProjectiles(npc, 17f, 17f);
							int num159 = 270;
							center3 += vector19 * 5f;
							int num160 = Projectile.NewProjectile(npc.GetSource_FromAI(), center3.X, center3.Y, num156, num157, num159, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
							Main.projectile[num160].timeLeft = 300;
						}
					}
				}
			}

			if (npc.ai[1] == 0f) {
				npc.damage = npc.defDamage;
				npc.ai[2] += 1f;
				if (npc.ai[2] >= 800f) {
					npc.ai[2] = 0f;
					npc.ai[1] = 1f;
					npc.TargetClosest();
					npc.netUpdate = true;
				}

				npc.rotation = npc.velocity.X / 15f;
				float num161 = 0.02f;
				float num162 = 2f;
				float num163 = 0.05f;
				float num164 = 8f;
				if (Main.expertMode) {
					num161 = 0.03f;
					num162 = 4f;
					num163 = 0.07f;
					num164 = 9.5f;
				}

				if (Main.getGoodWorld) {
					num161 += 0.01f;
					num162 += 1f;
					num163 += 0.05f;
					num164 += 2f;
				}

				if (npc.position.Y > Main.player[npc.target].position.Y - 250f) {
					if (npc.velocity.Y > 0f)
						npc.velocity.Y *= 0.98f;

					npc.velocity.Y -= num161;
					if (npc.velocity.Y > num162)
						npc.velocity.Y = num162;
				}
				else if (npc.position.Y < Main.player[npc.target].position.Y - 250f) {
					if (npc.velocity.Y < 0f)
						npc.velocity.Y *= 0.98f;

					npc.velocity.Y += num161;
					if (npc.velocity.Y < 0f - num162)
						npc.velocity.Y = 0f - num162;
				}

				if (npc.position.X + halfWidth > Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2)) {
					if (npc.velocity.X > 0f)
						npc.velocity.X *= 0.98f;

					npc.velocity.X -= num163;
					if (npc.velocity.X > num164)
						npc.velocity.X = num164;
				}

				if (npc.position.X + halfWidth < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2)) {
					if (npc.velocity.X < 0f)
						npc.velocity.X *= 0.98f;

					npc.velocity.X += num163;
					if (npc.velocity.X < 0f - num164)
						npc.velocity.X = 0f - num164;
				}
			}
			else if (npc.ai[1] == 1f) {
				if (Main.getGoodWorld) {
					if (num149 > 0) {
						npc.reflectsProjectiles = true;
					}
					else if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] % 200f == 0f && NPC.CountNPCS(32) < 6) {
						int num165 = 1;
						for (int num166 = 0; num166 < num165; num166++) {
							int num167 = 1000;
							for (int num168 = 0; num168 < num167; num168++) {
								int num169 = (int)(npc.Center.X / 16f) + Main.rand.Next(-50, 51);
								int num170;
								for (num170 = (int)(npc.Center.Y / 16f) + Main.rand.Next(-50, 51); num170 < Main.maxTilesY - 10 && !WorldGen.SolidTile(num169, num170); num170++) {
								}

								num170--;
								if (!WorldGen.SolidTile(num169, num170)) {
									int num171 = NPC.NewNPC(npc.GetSource_FromAI(), num169 * 16 + 8, num170 * 16, 32);
									if (Main.netMode == NetmodeID.Server && num171 < Main.maxNPCs)
										NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num171);

									break;
								}
							}
						}
					}
				}

				npc.defense -= 10;
				npc.ai[2] += 1f;
				if (npc.ai[2] == 2f)
					SoundEngine.PlaySound(SoundID.Roar, npc.position);

				if (npc.ai[2] >= 400f) {
					npc.ai[2] = 0f;
					npc.ai[1] = 0f;
				}

				npc.rotation += npc.direction * 0.3f;
				Vector2 vector20 = new Vector2(npc.position.X + halfWidth, npc.position.Y + halfHeight);
				float num172 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector20.X;
				float num173 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector20.Y;
				float num174 = (float)Math.Sqrt(num172 * num172 + num173 * num173);
				float num175 = 1.5f;
				npc.damage = GetAttackDamage_LerpBetweenFinalValues(npc, npc.defDamage, npc.defDamage * 1.3f);
				if (Main.expertMode) {
					num175 = 3.5f;
					if (num174 > 150f)
						num175 *= 1.05f;

					if (num174 > 200f)
						num175 *= 1.1f;

					if (num174 > 250f)
						num175 *= 1.1f;

					if (num174 > 300f)
						num175 *= 1.1f;

					if (num174 > 350f)
						num175 *= 1.1f;

					if (num174 > 400f)
						num175 *= 1.1f;

					if (num174 > 450f)
						num175 *= 1.1f;

					if (num174 > 500f)
						num175 *= 1.1f;

					if (num174 > 550f)
						num175 *= 1.1f;

					if (num174 > 600f)
						num175 *= 1.1f;

					switch (num149) {
						case 0:
							num175 *= 1.1f;
							break;
						case 1:
							num175 *= 1.05f;
							break;
					}
				}

				if (Main.getGoodWorld)
					num175 *= 1.3f;

				num174 = num175 / num174;
				npc.velocity.X = num172 * num174;
				npc.velocity.Y = num173 * num174;
			}
			else if (npc.ai[1] == 2f) {
				npc.damage = 1000;
				npc.defense = 9999;
				npc.rotation += npc.direction * 0.3f;
				Vector2 vector21 = new Vector2(npc.position.X + halfWidth, npc.position.Y + halfHeight);
				float num176 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector21.X;
				float num177 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector21.Y;
				float num178 = (float)Math.Sqrt(num176 * num176 + num177 * num177);
				num178 = 8f / num178;
				npc.velocity.X = num176 * num178;
				npc.velocity.Y = num177 * num178;
			}
			else if (npc.ai[1] == 3f) {
				npc.velocity.Y += 0.1f;
				if (npc.velocity.Y < 0f)
					npc.velocity.Y *= 0.95f;

				npc.velocity.X *= 0.95f;
				npc.EncourageDespawn(50);
			}

			if (npc.ai[1] != 2f && npc.ai[1] != 3f && npc.type != NPCID.DungeonGuardian && (num149 != 0 || !Main.expertMode)) {
				int num179 = Dust.NewDust(new Vector2(npc.position.X + halfWidth - 15f - npc.velocity.X * 5f, npc.position.Y + halfHeight), 30, 10, DustID.Blood, (0f - npc.velocity.X) * 0.2f, 3f, 0, default(Color), 2f);
				Main.dust[num179].noGravity = true;
				Main.dust[num179].velocity.X *= 1.3f;
				Main.dust[num179].velocity.X += npc.velocity.X * 0.4f;
				Main.dust[num179].velocity.Y += 2f + npc.velocity.Y;
				for (int num180 = 0; num180 < 2; num180++) {
					num179 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 120f), npc.width, 60, DustID.Blood, npc.velocity.X, npc.velocity.Y, 0, default(Color), 2f);
					Main.dust[num179].noGravity = true;
					Dust dust = Main.dust[num179];
					dust.velocity -= npc.velocity;
					Main.dust[num179].velocity.Y += 5f;
				}
			}
		}
		public int GetAttackDamage_ForProjectiles(NPC npc, float normalDamage, float expertDamage) {
			float amount = (Main.expertMode ? 1 : 0);
			if (Main.GameModeInfo.IsJourneyMode)
				amount = MathHelper.Clamp(npc.strengthMultiplier - 1f, 0f, 1f);

			return (int)MathHelper.Lerp(normalDamage, expertDamage, amount);
		}
		public int GetAttackDamage_LerpBetweenFinalValues(NPC npc, float normalDamage, float expertDamage) {
			float amount = (Main.expertMode ? 1 : 0);
			if (Main.GameModeInfo.IsJourneyMode)
				amount = MathHelper.Clamp(npc.strengthMultiplier - 1f, 0f, 1f);

			return (int)MathHelper.Lerp(normalDamage, expertDamage, amount);
		}
		public override void PostAI(NPC npc) {
			if (BossRushUtils.IsAnyVanillaBossAlive()) {
				if (npc.type == NPCID.Nurse) {
					npc.StrikeInstantKill();
				}
			}
		}
		public override void ModifyShop(NPCShop shop) {
			if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode) {
				//Re add removing shop soon
			}
		}
	}
}
