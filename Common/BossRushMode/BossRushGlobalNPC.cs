using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;

namespace BossRush.Common.ChallengeMode {
	internal class BossRushGlobalNPC : GlobalNPC {
		public override void SetDefaults(NPC entity) {
			if(entity.boss && UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
				float multiplier = ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count * .5f;
				if(Main.hardMode) {
					multiplier *= 2;
				}
				entity.lifeMax = (int)(3000 + (3000 * multiplier));
				entity.damage = (int)(30 + (30 * multiplier));
			}
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
			if (!UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
				return;
			}
			if (UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_WORLDGEN)) {
				spawnRate *= 100;
			}
		}
		public override bool PreAI(NPC npc) {
			if (!UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
				return base.PreAI(npc);
			}
			if (npc.type == NPCID.SkeletronHand) {
				SkeletronAIHand(npc);
				return false;
			}
			if (npc.type == NPCID.SkeletronHead) {
				SkeletronAI(npc);
				return false;
			}
			if (npc.type == NPCID.OldMan) {
				return false;
			}
			if (npc.type == NPCID.TravellingMerchant) {
				return false;
			}
			return true;
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
		}
		public override void OnKill(NPC npc) {
			if(npc.type == NPCID.WallofFlesh && !Main.hardMode) {
				ModContent.GetInstance<UniversalSystem>().defaultUI.TurnOnEndOfDemoMessage();
			}
		}
	}
}
