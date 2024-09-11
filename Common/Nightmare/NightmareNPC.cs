using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.BuffAndDebuff;
using Terraria.Audio;
using BossRush.Common.General;

namespace BossRush.Common.Nightmare {
	internal class NightmareNPC : GlobalNPC {
		public override bool InstancePerEntity => true;
		int aiTimer = 0;
		public override void SetDefaults(NPC npc) {
			if (!ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return;
			}
			npc.trapImmune = true;
			npc.lavaImmune = true;
			BossChange(npc);
			if (npc.type == NPCID.VileSpitEaterOfWorlds) {
				npc.scale += .5f;
			}
			if (npc.type == NPCID.ServantofCthulhu) {
				npc.lifeMax += 100;
			}
			npc.knockBackResist *= .5f;
		}
		private void BossChange(NPC npc) {
			if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsBody) {
				npc.lifeMax += 250;
				npc.scale += .25f;
				if (npc.type == NPCID.EaterofWorldsHead) {
					npc.damage *= 3;
				}
			}
			if (!npc.boss)
				return;
			if (npc.type == NPCID.CultistBoss) {
				npc.lifeMax += 15000;
				npc.defense += 30;
			}
			if (npc.type == NPCID.KingSlime) {
				npc.defense += 10;
				npc.lifeMax += 1500;
			}
			if (npc.type == NPCID.EyeofCthulhu) {
				npc.scale -= 0.25f;
				npc.Size -= new Vector2(25, 25);
			}
		}
		public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
			if (!ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return;
			}
			switch (npc.type) {
				case NPCID.KingSlime:
					target.AddBuff(BuffID.BrokenArmor, 180);
					target.AddBuff(ModContent.BuffType<KingSlimeRage>(), 240);
					break;
				case NPCID.EyeofCthulhu:
					target.AddBuff(BuffID.Cursed, 90);
					target.AddBuff(BuffID.Bleeding, 150);
					target.AddBuff(BuffID.Obstructed, 180);
					target.AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 30);
					break;
				case NPCID.BrainofCthulhu:
					target.AddBuff(164, 60);
					target.AddBuff(BuffID.Ichor, 180);
					break;
				case NPCID.EaterofWorldsHead:
					target.AddBuff(BuffID.Weak, 180);
					target.AddBuff(BuffID.CursedInferno, 300);
					target.AddBuff(BuffID.BrokenArmor, 180);
					break;
				case NPCID.EaterofWorldsBody:
				case NPCID.EaterofWorldsTail:
					target.AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 30);
					break;
				case NPCID.QueenBee:
					target.AddBuff(BuffID.Venom, 180);
					target.AddBuff(BuffID.Bleeding, 180);
					break;
				default:
					break;
			}
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.BrokenArmor, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Cursed, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Bleeding, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Burning, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Weak, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.CursedInferno, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Ichor, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Venom, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Poisoned, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Slow, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.ManaSickness, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.PotionSickness, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Obstructed, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Blackout, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Confused, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Darkness, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Electrified, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Stoned, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.WitheredArmor, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.WitheredWeapon, Main.rand.Next(1, 901));
			//if (Main.rand.NextBool(10))
			//	target.AddBuff(BuffID.Suffocation, Main.rand.Next(1, 901));
		}
		private bool LifLowerOrEqualHalf(NPC npc) => npc.life <= npc.lifeMax * .5f;
		private bool IsNPCLifeAbovePercentage(NPC npc, float percentage) => npc.life >= npc.lifeMax * percentage;
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
			if (ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				maxSpawns += 100;
				spawnRate -= 10;
			}
		}
		public override bool PreAI(NPC npc) {
			if (!ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return base.PreAI(npc);
			}
			if (npc.type == NPCID.Spazmatism) {
				if (npc.ai[0] == 3) {
					npc.velocity /= 2;
				}
			}
			else if(npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail) {
				npc.velocity /= 2;
			}
			else if(npc.type == NPCID.BrainofCthulhu) {
				if (npc.ai[0] < 0) {
					npc.velocity /= 3f;
				}
			}
			else if(npc.type == NPCID.Creeper) {
				npc.velocity /= 1.5f;
			}
			else if(npc.type == NPCID.ServantofCthulhu) {
				npc.velocity /= 3;
			}
			else if (npc.type == NPCID.KingSlime) {
				npc.velocity /= 1.25f;
				KingSlimeAI(npc);
				return false;
			}
			else if(npc.type == NPCID.EyeofCthulhu) {
				npc.velocity /= 1.25f;
				EoCAi(npc);
				return false;
			}
			return base.PreAI(npc);
		}
		public override void PostAI(NPC npc) {
			base.PostAI(npc);
			if (!ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return;
			}
			if (npc.type == NPCID.Spazmatism) {
				if (npc.ai[0] == 3) {
					npc.velocity *= 2;
				}
			}
			else if(npc.type == NPCID.BrainofCthulhu) {
				if (npc.ai[0] < 0) {
					npc.velocity *= 3f;
				}
				npc.localAI[1] += 1;
			}
			else if(npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail) {
				npc.velocity *= 2;
			}
			else if(npc.type == NPCID.Creeper) {
				npc.velocity *= 1.5f;
			}
			else if(npc.type == NPCID.KingSlime) {
				npc.velocity *= 1.25f;
			}
			else if(npc.type == NPCID.EyeofCthulhu) {
				npc.velocity *= 1.25f;
			}
			else if(npc.type == NPCID.ServantofCthulhu) {
				npc.velocity *= 3f;
			}
			else if(npc.type == NPCID.CultistBoss) {
				if (npc.ai[0] == 5f) {
					if (npc.ai[1] >= 120f) {
						npc.chaseable = true;
						npc.ai[0] = 0f;
						npc.ai[1] = 0f;
						npc.ai[3] += 1f;
						npc.velocity = Vector2.Zero;
						npc.netUpdate = true;
					}
				}
				for (int i = 0; i < 300; i++) {
					int dust1 = Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(2000f, 2000f), 0, 0, DustID.SolarFlare);
					Main.dust[dust1].noGravity = true;
					Main.dust[dust1].velocity = (Main.dust[dust1].position - npc.Center).SafeNormalize(Vector2.Zero) * 3f;
				}
				if (!BossRushUtils.CompareSquareFloatValue(npc.Center, Main.player[npc.target].Center, 2000 * 2000)) {
					Main.player[npc.target].AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 120);
				}
			}
		}
		public override void OnKill(NPC npc) {
			aiTimer = 0;
		}
		/// <summary>
		/// We need to deeply interfere with KS ai and so I port entire KS code from vanilla<br/>
		/// We could have done it like fargo but I don't like that approach cause it is too limiting
		/// We could also steal code from calamity but hey, we aren't gonna use their unique AI
		/// </summary>
		/// <param name="npc"></param>
		private void KingSlimeAI(NPC npc) {
			aiTimer++;
			if (aiTimer == int.MaxValue) {
				aiTimer = 0;
			}
			float progress = 1f;
			float num237 = 1f;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;

			num237 *= 2 - (1 - npc.life / (float)npc.lifeMax);

			npc.aiAction = 0;
			if (npc.ai[3] == 0f && npc.life > 0) {
				npc.ai[3] = npc.lifeMax;
			}

			if (npc.localAI[3] == 0f) {
				npc.localAI[3] = 1f;
				flag6 = true;
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					npc.ai[0] = -100f;
					npc.TargetClosest();
					npc.netUpdate = true;
				}
			}

			//npc is for checking whenever player is dead
			int num239 = 5000;
			if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > num239) {
				npc.TargetClosest();
				num239 += 2000;
				if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > num239) {
					npc.EncourageDespawn(10);
					npc.direction = (Main.player[npc.target].Center.X < npc.Center.X).ToDirectionInt();
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
					Point playerTilePos = Main.player[npc.target].Center.ToTileCoordinates();
					Vector2 disToPlayer = Main.player[npc.target].Center - npc.Center;
					int num241 = 0;
					int num242 = 7;
					int num243 = 0;
					bool flag9 = false;
					if (npc.localAI[0] >= 360f || disToPlayer.Length() > 2000f) {
						if (npc.localAI[0] >= 360f)
							npc.localAI[0] = 360f;

						flag9 = true;
						num243 = 100;
					}

					while (!flag9 && num243 < 100) {
						num243++;
						int num244 = Main.rand.Next(playerTilePos.X - 10, playerTilePos.X + 10 + 1);
						int num245 = Main.rand.Next(playerTilePos.Y - 10, playerTilePos.Y + 1);
						if (num245 >= playerTilePos.Y - num242 && num245 <= playerTilePos.Y + num242 && num244 >= playerTilePos.X - num242 && num244 <= playerTilePos.X + num242
							|| num245 >= point3.Y - num241 && num245 <= point3.Y + num241 && num244 >= point3.X - num241 && num244 <= point3.X + num241
							|| Main.tile[num244, num245].HasUnactuatedTile)
							continue;

						int num247 = 0;
						if (Main.tile[num244, num245].HasUnactuatedTile
							&& Main.tileSolid[Main.tile[num244, num245].TileType]
							&& !Main.tileSolidTop[Main.tile[num244, num245].TileType]) {
							num247 = 1;
						}
						else {
							for (; num247 < 150 && num245 + num247 < Main.maxTilesY; num247++) {
								int num248 = num245 + num247;
								if (Main.tile[num244, num248].HasUnactuatedTile && Main.tileSolid[Main.tile[num244, num248].TileType] && !Main.tileSolidTop[Main.tile[num244, num248].TileType]) {
									num247--;
									break;
								}
							}
						}

						num245 += num247;

						if (Main.tile[num244, num245].LiquidType != LiquidID.Lava && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0)) {
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
				if (npc.localAI[0] < 0f) {
					npc.localAI[0] = 0f;
				}
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
				progress = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
				progress = 0.5f + progress * 0.5f;
				if (npc.ai[0] >= 60f)
					flag8 = true;

				if (npc.ai[0] == 60f) {
					Gore.NewGore(npc.GetSource_FromAI(), npc.Center + new Vector2(-40f, -npc.height / 2), npc.velocity, 734);
				}

				if (npc.ai[0] >= 60f && Main.netMode != NetmodeID.MultiplayerClient) {
					npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
					npc.ai[1] = 6f;
					npc.ai[0] = 0f;
					npc.netUpdate = true;
					KingSlimeTeleport(npc);
				}

				if (Main.netMode == NetmodeID.MultiplayerClient && npc.ai[0] >= 120f) {
					npc.ai[1] = 6f;
					npc.ai[0] = 0f;
				}

				if (!flag8) {
					for (int i = 0; i < 10; i++) {
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
				progress = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
				progress = 0.5f + progress * 0.5f;
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

				for (int i = 0; i < 10; i++) {
					int dustNum = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustID.TintableDust, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
					Main.dust[dustNum].noGravity = true;
					dust = Main.dust[dustNum];
					dust.velocity *= 2f;
				}
			}

			npc.dontTakeDamage = npc.hide = flag8;
			if (npc.velocity.Y == 0f) {
				KingSlimeStandingStill(npc);
				npc.velocity.X *= 0.8f;
				if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
					npc.velocity.X = 0f;

				if (!flag7) {
					npc.ai[0] += 2f;
					if (npc.life < npc.lifeMax * 0.8f) {
						npc.ai[0] += 1f;
					}

					if (npc.life < npc.lifeMax * 0.6f) {
						npc.ai[0] += 1f;
					}

					if (npc.life < npc.lifeMax * 0.4f) {
						npc.ai[0] += 2f;
					}

					if (npc.life < npc.lifeMax * 0.2f) {
						npc.ai[0] += 3f;
					}

					if (npc.life < npc.lifeMax * 0.1f) {
						npc.ai[0] += 4f;
					}

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
								jumpStrength = -13f;
								MoveSpeed = 3.5f;
								DelayAttack = -200f;
								KingSlimeStartOfBigJump(npc, ref jumpStrength, ref MoveSpeed, ref DelayAttack);
								npc.ai[1] = 0f;
								break;
							default:
								jumpStrength = -8f;
								MoveSpeed = 4f;
								DelayAttack = -120;
								npc.ai[1] += 1f;
								break;
						}
						KingSlimePostJumpModifier(npc, ref jumpStrength, ref MoveSpeed, ref DelayAttack);
						npc.velocity.Y = jumpStrength;
						npc.velocity.X += MoveSpeed * npc.direction;
						npc.ai[0] = DelayAttack;
					}
					else if (npc.ai[0] >= -30f) {
						npc.aiAction = 1;
					}
				}
			}
			else {
				KingSlimeOnAir(npc);
				if (npc.target < 255) {
					if (npc.direction == 1 && npc.velocity.X < 6 || npc.direction == -1 && npc.velocity.X > 0f - 6) {
						if (npc.direction == -1 && npc.velocity.X < 0.1 || npc.direction == 1 && npc.velocity.X > -0.1)
							npc.velocity.X += 0.2f * npc.direction;
						else
							npc.velocity.X *= 0.93f;
					}
				}
			}
			int num254 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.TintableDust, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
			Main.dust[num254].noGravity = true;
			dust = Main.dust[num254];
			dust.velocity *= 0.5f;
			if (npc.life <= 0)
				return;

			float lifeScaleprogress = npc.life / (float)npc.lifeMax;
			lifeScaleprogress = lifeScaleprogress * 0.5f + 0.75f;
			lifeScaleprogress *= progress;
			lifeScaleprogress *= num237;
			if (lifeScaleprogress != npc.scale || flag6) {
				float halfwidth = npc.width * .5f;
				npc.position.X += halfwidth;
				npc.position.Y += npc.height;
				npc.scale = lifeScaleprogress;
				npc.width = (int)(98f * npc.scale);
				npc.height = (int)(92f * npc.scale);
				npc.position.X -= halfwidth;
				npc.position.Y -= npc.height;
			}

			KingSlimeSpawnMinion(npc);
		}
		private void KingSlimeSpawnMinion(NPC npc) {
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;

			int lifeunder5Percentage = (int)(npc.lifeMax * .05f);
			if (npc.life + lifeunder5Percentage >= npc.ai[3])
				return;

			npc.ai[3] = npc.life;
			int RandomSpawnNPCamount = Main.rand.Next(1, 4);
			if (LifLowerOrEqualHalf(npc)) {
				RandomSpawnNPCamount += Main.rand.Next(2, 5);
			}

			for (int i = 0; i < RandomSpawnNPCamount; i++) {
				int x = (int)(npc.position.X + Main.rand.NextFloat(npc.width - 32));
				int y = (int)(npc.position.Y + Main.rand.NextFloat(npc.height - 32));
				int npcType = NPCID.BlueSlime;
				if (Main.rand.NextBool(4)) {
					npcType = NPCID.SlimeSpiked;
				}

				int slimeMinion = NPC.NewNPC(npc.GetSource_FromAI(), x, y, npcType);
				Main.npc[slimeMinion].SetDefaults(npcType);
				if (npcType == NPCID.SlimeSpiked && LifLowerOrEqualHalf(npc)) {
					Main.npc[slimeMinion].velocity = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(10, 15);
				}
				else {
					Main.npc[slimeMinion].velocity.X = Main.rand.NextFloat(-15, 16) * 0.1f;
					Main.npc[slimeMinion].velocity.Y = Main.rand.NextFloat(-30, 1) * 0.1f;
				}
				Main.npc[slimeMinion].ai[0] = -1000 * Main.rand.Next(3);
				Main.npc[slimeMinion].ai[1] = 0f;
				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, slimeMinion);
			}
		}
		private void KingSlimeStandingStill(NPC npc) {

		}
		private void KingSlimeTeleport(NPC npc) {
			Vector2 towardPlayer = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero) * 15f;
			for (int i = 0; i < 12; i++) {
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, towardPlayer.Vector2RotateByRandom(30).Vector2RandomSpread(2, Main.rand.NextFloat(.75f, 1.25f)), ProjectileID.SpikedSlimeSpike, npc.damage, 4f);
			}
		}
		private void KingSlimeOnAir(NPC npc) {
			if (aiTimer % 10 == 0) {
				Vector2 towardPlayer = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero) * 15f;
				if (Main.player[npc.target].Center.Y > npc.Center.Y) {
					towardPlayer = towardPlayer.RotatedBy(MathHelper.ToRadians(-BossRushUtils.DirectionFromPlayerToNPC(Main.player[npc.target].Center.X, npc.Center.X) * 15));
				}
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, towardPlayer, ProjectileID.SpikedSlimeSpike, npc.damage, 4f);
			}
		}
		private void KingSlimePostJumpModifier(NPC npc, ref float JumpStrength, ref float MoveSpeed, ref float DelayAttack) {
			if (npc.life <= npc.lifeMax * .8f) {
				if (npc.ai[1] == 3) {
					DelayAttack += 60;
				}
				if (npc.ai[1] == 0 || npc.ai[1] == 1) {
					JumpStrength += .5f;
					MoveSpeed += 1;
					DelayAttack += 30;
				}
			}
			if (npc.life <= npc.lifeMax * .4f) {
				if (npc.ai[1] == 2) {
					MoveSpeed += 5;
				}
				if (npc.ai[1] == 0 || npc.ai[1] == 1) {
					JumpStrength += .6f;
					MoveSpeed += 2;
					DelayAttack += 40;
				}
			}
			if (DelayAttack > 0) {
				DelayAttack = 0;
			}
		}
		/// <summary>
		/// npc is where King slime would do a massive jump, only when 
		/// </summary>
		/// <param name="npc">The king slime himself</param>
		/// <param name="JumpStrength">npc dictate how much velocity.Y will KS gain, must be negative</param>
		/// <param name="MoveSpeed">npc dictate how much velocity.X will KS gain</param>
		/// <param name="DelayAttack">npc is a timer that count upward, so setting negative mean it will delay his attack even longer</param>
		private void KingSlimeStartOfBigJump(NPC npc, ref float JumpStrength, ref float MoveSpeed, ref float DelayAttack) {
			for (int i = 0; i < 16; i++) {
				Vector2 spreadoutring = Vector2.One.Vector2DistributeEvenly(16, 360, i) * 10f;
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, spreadoutring, ProjectileID.SpikedSlimeSpike, npc.damage, 4f);
			}
			if (npc.life <= npc.lifeMax * .75f) {
				DelayAttack += 30;
			}
		}
		private bool CommonTargetCheck(NPC npc) => npc.target < 0 || npc.target > 255 || Main.player[npc.target].dead || !Main.player[npc.target].active;
		private void EoCAi(NPC npc) {
			bool flag2 = false;
			if (Main.expertMode && !IsNPCLifeAbovePercentage(npc, .12f))
				flag2 = true;

			bool flag3 = false;
			if (Main.expertMode && !IsNPCLifeAbovePercentage(npc, .04f))
				flag3 = true;

			float num4 = 20f;
			if (flag3)
				num4 = 10f;

			if (CommonTargetCheck(npc))
				npc.TargetClosest();

			Player player = Main.player[npc.target];
			bool dead = player.dead;
			Vector2 distanceTo = npc.position + npc.Size.Subtract(npc.width * .5f, 59f) - player.Center;
			float num5 = npc.position.X + (float)(npc.width / 2) - player.position.X - (float)(player.width / 2);
			float num6 = npc.position.Y + (float)npc.height - 59f - player.position.Y - (float)(player.height / 2);
			float num7 = (float)Math.Atan2(num6, num5) + 1.57f;
			if (num7 < 0f)
				num7 += 6.283f;
			else if ((double)num7 > 6.283)
				num7 -= 6.283f;

			float num8 = 0f;
			if (npc.ai[0] == 0f && npc.ai[1] == 0f)
				num8 = 0.02f;

			if (npc.ai[0] == 0f && npc.ai[1] == 2f && npc.ai[2] > 40f)
				num8 = 0.05f;

			if (npc.ai[0] == 3f && npc.ai[1] == 0f)
				num8 = 0.05f;

			if (npc.ai[0] == 3f && npc.ai[1] == 2f && npc.ai[2] > 40f)
				num8 = 0.08f;

			if (npc.ai[0] == 3f && npc.ai[1] == 4f && npc.ai[2] > num4)
				num8 = 0.15f;

			if (npc.ai[0] == 3f && npc.ai[1] == 5f)
				num8 = 0.05f;

			if (Main.expertMode)
				num8 *= 1.5f;

			if (flag3 && Main.expertMode)
				num8 = 0f;

			if (npc.rotation < num7) {
				if ((double)(num7 - npc.rotation) > 3.1415)
					npc.rotation -= num8;
				else
					npc.rotation += num8;
			}
			else if (npc.rotation > num7) {
				if ((double)(npc.rotation - num7) > 3.1415)
					npc.rotation += num8;
				else
					npc.rotation -= num8;
			}

			if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8)
				npc.rotation = num7;

			if (npc.rotation < 0f)
				npc.rotation += 6.283f;
			else if ((double)npc.rotation > 6.283)
				npc.rotation -= 6.283f;

			if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8)
				npc.rotation = num7;

			if (Main.rand.NextBool(5)) {
				int num9 = Dust.NewDust(npc.position.Add(0, npc.height * .25f), npc.width, (int)(npc.height * 0.5f), DustID.Blood, npc.velocity.X, 2f);
				Main.dust[num9].velocity.X *= 0.5f;
				Main.dust[num9].velocity.Y *= 0.1f;
			}

			npc.reflectsProjectiles = false;
			if (dead) {
				npc.velocity.Y -= 0.04f;
				npc.EncourageDespawn(10);
				return;
			}

			if (npc.ai[0] == 0f) {
				if (npc.ai[1] == 0f) {
					float num10 = 5f;
					float num11 = 0.04f;
					if (Main.expertMode) {
						num11 = 0.15f;
						num10 = 7f;
					}

					if (Main.getGoodWorld) {
						num11 += 0.05f;
						num10 += 1f;
					}

					Vector2 vector = npc.Center;
					float num12 = player.Center.X - vector.X;
					float num13 = player.Center.Y - 200f - vector.Y;
					float directionToPlayerlength = MathF.Sqrt(num12 * num12 + num13 * num13);
					float num15 = directionToPlayerlength;
					directionToPlayerlength = num10 / directionToPlayerlength;
					num12 *= directionToPlayerlength;
					num13 *= directionToPlayerlength;
					if (npc.velocity.X < num12) {
						npc.velocity.X += num11;
						if (npc.velocity.X < 0f && num12 > 0f)
							npc.velocity.X += num11;
					}
					else if (npc.velocity.X > num12) {
						npc.velocity.X -= num11;
						if (npc.velocity.X > 0f && num12 < 0f)
							npc.velocity.X -= num11;
					}

					if (npc.velocity.Y < num13) {
						npc.velocity.Y += num11;
						if (npc.velocity.Y < 0f && num13 > 0f)
							npc.velocity.Y += num11;
					}
					else if (npc.velocity.Y > num13) {
						npc.velocity.Y -= num11;
						if (npc.velocity.Y > 0f && num13 < 0f)
							npc.velocity.Y -= num11;
					}

					npc.ai[2] += 1f;
					float num16 = 600f;
					if (Main.expertMode)
						num16 *= 0.35f;

					if (npc.ai[2] >= num16) {
						npc.ai[1] = 1f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						npc.target = 255;
						npc.netUpdate = true;
					}
					else if ((npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && num15 < 500f) || (Main.expertMode && num15 < 500f)) {
						if (!Main.player[npc.target].dead)
							npc.ai[3] += 1f;

						float num17 = 110f;
						if (Main.expertMode)
							num17 *= 0.4f;

						if (Main.getGoodWorld)
							num17 *= 0.8f;

						if (npc.ai[3] >= num17) {
							npc.ai[3] = 0f;
							npc.rotation = num7;
							float num18 = 5f;
							if (Main.expertMode)
								num18 = 6f;

							float num19 = Main.player[npc.target].Center.X - vector.X;
							float num20 = Main.player[npc.target].Center.Y - vector.Y;
							float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
							num21 = num18 / num21;
							Vector2 vector2 = vector;
							Vector2 vector3 = default;
							vector3.X = num19 * num21;
							vector3.Y = num20 * num21;
							vector2.X += vector3.X * 10f;
							vector2.Y += vector3.Y * 10f;
							if (Main.netMode != 1) {
								int num22 = NPC.NewNPC(npc.GetSource_FromAI(), (int)vector2.X, (int)vector2.Y, 5);
								Main.npc[num22].velocity.X = vector3.X;
								Main.npc[num22].velocity.Y = vector3.Y;
								if (Main.netMode == 2 && num22 < 200)
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num22);
							}

							SoundEngine.PlaySound(SoundID.NPCHit1, vector2);
							for (int m = 0; m < 10; m++) {
								Dust.NewDust(vector2, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f);
							}
						}
					}
				}
				else if (npc.ai[1] == 1f) {
					npc.rotation = num7;
					float num23 = 6f;
					if (Main.expertMode)
						num23 = 7f;

					if (Main.getGoodWorld)
						num23 += 1f;
					//TODO : rewrite code here
					Vector2 vector4 = npc.Center;
					float num24 = Main.player[npc.target].Center.X - vector4.X;
					float num25 = Main.player[npc.target].Center.Y - vector4.Y;
					float num26 = MathF.Sqrt(num24 * num24 + num25 * num25);
					num26 = num23 / num26;
					npc.velocity.X = num24 * num26;
					npc.velocity.Y = num25 * num26;
					npc.ai[1] = 2f;
					npc.netUpdate = true;
					if (npc.netSpam > 10)
						npc.netSpam = 10;
				}
				else if (npc.ai[1] == 2f) {
					npc.ai[2] += 1f;
					if (npc.ai[2] >= 40f) {
						npc.velocity *= 0.98f;
						if (Main.expertMode)
							npc.velocity *= 0.985f;

						if (Main.getGoodWorld)
							npc.velocity *= 0.99f;

						if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
							npc.velocity.X = 0f;

						if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
							npc.velocity.Y = 0f;
					}
					else {
						npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
					}

					int num27 = 150;
					if (Main.expertMode)
						num27 = 100;

					if (Main.getGoodWorld)
						num27 -= 15;

					if (npc.ai[2] >= (float)num27) {
						npc.ai[3] += 1f;
						npc.ai[2] = 0f;
						npc.target = 255;
						npc.rotation = num7;
						if (npc.ai[3] >= 3f) {
							npc.ai[1] = 0f;
							npc.ai[3] = 0f;
						}
						else {
							npc.ai[1] = 1f;
						}
					}
				}

				float TransitionCondition = 0.5f;
				if (Main.expertMode)
					TransitionCondition = 0.65f;

				if (!IsNPCLifeAbovePercentage(npc, TransitionCondition)) {
					npc.ai[0] = 1f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					npc.netUpdate = true;
					if (npc.netSpam > 10)
						npc.netSpam = 10;
				}

				return;
			}

			if (npc.ai[0] == 1f || npc.ai[0] == 2f) {
				if (npc.ai[0] == 1f || npc.ai[3] == 1f) {
					npc.ai[2] += 0.005f;
					if ((double)npc.ai[2] > 0.5)
						npc.ai[2] = 0.5f;
				}
				else {
					npc.ai[2] -= 0.005f;
					if (npc.ai[2] < 0f)
						npc.ai[2] = 0f;
				}

				npc.rotation += npc.ai[2];
				npc.ai[1] += 1f;
				if (Main.getGoodWorld)
					npc.reflectsProjectiles = true;

				int num29 = 20;
				if (Main.getGoodWorld && npc.life < npc.lifeMax / 3)
					num29 = 10;

				if (Main.expertMode && npc.ai[1] % (float)num29 == 0f) {
					float num30 = 5f;
					float num31 = Main.rand.Next(-200, 200);
					float num32 = Main.rand.Next(-200, 200);
					if (Main.getGoodWorld) {
						num31 *= 3f;
						num32 *= 3f;
					}

					float num33 = (float)Math.Sqrt(num31 * num31 + num32 * num32);
					num33 = num30 / num33;
					Vector2 vector6 = npc.Center;
					Vector2 vector7 = default(Vector2);
					vector7.X = num31 * num33;
					vector7.Y = num32 * num33;
					vector6.X += vector7.X * 10f;
					vector6.Y += vector7.Y * 10f;
					if (Main.netMode != 1) {
						int num34 = NPC.NewNPC(npc.GetSource_FromAI(), (int)vector6.X, (int)vector6.Y, 5);
						Main.npc[num34].velocity.X = vector7.X;
						Main.npc[num34].velocity.Y = vector7.Y;
						if (Main.netMode == 2 && num34 < 200)
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num34);
					}

					for (int n = 0; n < 10; n++) {
						Dust.NewDust(vector6, 20, 20, 5, vector7.X * 0.4f, vector7.Y * 0.4f);
					}
				}

				if (npc.ai[1] >= 100f) {
					if (npc.ai[3] == 1f) {
						npc.ai[3] = 0f;
						npc.ai[1] = 0f;
					}
					else {
						npc.ai[0] += 1f;
						npc.ai[1] = 0f;
						if (npc.ai[0] == 3f) {
							npc.ai[2] = 0f;
						}
						else {
							SoundEngine.PlaySound(SoundID.NPCHit1, npc.position);
							for (int num35 = 0; num35 < 2; num35++) {
								for (int i = 6; i <= 8; i++) {
									Vector2 velocity = new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
									Gore.NewGore(npc.GetSource_FromAI(), npc.position, velocity, i);
								}
							}

							for (int num36 = 0; num36 < 20; num36++) {
								Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
							}

							SoundEngine.PlaySound(SoundID.Roar, npc.position);
						}
					}
				}

				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
				npc.velocity.X *= 0.98f;
				npc.velocity.Y *= 0.98f;
				if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
					npc.velocity.X = 0f;

				if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
					npc.velocity.Y = 0f;

				return;
			}

			npc.defense = 0;
			int num37 = 23;
			int num38 = 18;
			if (Main.expertMode) {
				if (flag2)
					npc.defense = -15;

				if (flag3) {
					num38 = 20;
					npc.defense = -30;
				}
			}

			npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(num37, num38);
			npc.damage = npc.GetAttackDamage_ScaledByStrength(npc.damage);
			if (npc.ai[1] == 0f && flag2)
				npc.ai[1] = 5f;

			if (npc.ai[1] == 0f) {
				float num39 = 6f;
				float num40 = 0.07f;
				Vector2 vector8 = npc.Center;
				//TODO : re write code here
				float num41 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector8.X;
				float num42 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 120f - vector8.Y;
				float num43 = MathF.Sqrt(num41 * num41 + num42 * num42);
				if (num43 > 400f && Main.expertMode) {
					num39 += 1f;
					num40 += 0.05f;
					if (num43 > 600f) {
						num39 += 1f;
						num40 += 0.05f;
						if (num43 > 800f) {
							num39 += 1f;
							num40 += 0.05f;
						}
					}
				}

				if (Main.getGoodWorld) {
					num39 += 1f;
					num40 += 0.1f;
				}

				num43 = num39 / num43;
				num41 *= num43;
				num42 *= num43;
				if (npc.velocity.X < num41) {
					npc.velocity.X += num40;
					if (npc.velocity.X < 0f && num41 > 0f)
						npc.velocity.X += num40;
				}
				else if (npc.velocity.X > num41) {
					npc.velocity.X -= num40;
					if (npc.velocity.X > 0f && num41 < 0f)
						npc.velocity.X -= num40;
				}

				if (npc.velocity.Y < num42) {
					npc.velocity.Y += num40;
					if (npc.velocity.Y < 0f && num42 > 0f)
						npc.velocity.Y += num40;
				}
				else if (npc.velocity.Y > num42) {
					npc.velocity.Y -= num40;
					if (npc.velocity.Y > 0f && num42 < 0f)
						npc.velocity.Y -= num40;
				}

				npc.ai[2] += 1f;
				if (npc.ai[2] >= 200f) {
					npc.ai[1] = 1f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					if (Main.expertMode && !IsNPCLifeAbovePercentage(npc, .35f))
						npc.ai[1] = 3f;

					npc.target = 255;
					npc.netUpdate = true;
				}

				if (Main.expertMode && flag3) {
					npc.TargetClosest();
					npc.netUpdate = true;
					npc.ai[1] = 3f;
					npc.ai[2] = 0f;
					npc.ai[3] -= 1000f;
				}
			}
			else if (npc.ai[1] == 1f) {
				SoundStyle roar = SoundID.ForceRoar;
				roar.Pitch = .25f;
				SoundEngine.PlaySound(roar, npc.position);
				npc.rotation = num7;
				float num44 = 6.8f;
				if (Main.expertMode && npc.ai[3] == 1f)
					num44 *= 1.15f;

				if (Main.expertMode && npc.ai[3] == 2f)
					num44 *= 1.3f;

				if (Main.getGoodWorld)
					num44 *= 1.2f;

				//TODO : re write code here
				Vector2 vector9 = npc.Center;
				float num45 = Main.player[npc.target].Center.X - vector9.X;
				float num46 = Main.player[npc.target].Center.Y - vector9.Y;
				float num47 = MathF.Sqrt(num45 * num45 + num46 * num46);
				num47 = num44 / num47;
				npc.velocity.X = num45 * num47;
				npc.velocity.Y = num46 * num47;
				npc.ai[1] = 2f;
				npc.netUpdate = true;
				if (npc.netSpam > 10)
					npc.netSpam = 10;
			}
			else if (npc.ai[1] == 2f) {
				float num48 = 40f;
				npc.ai[2] += 1f;
				if (Main.expertMode)
					num48 = 50f;

				if (npc.ai[2] >= num48) {
					npc.velocity *= 0.97f;
					if (Main.expertMode)
						npc.velocity *= 0.98f;

					if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
						npc.velocity.X = 0f;

					if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
						npc.velocity.Y = 0f;
				}
				else {
					npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - MathHelper.PiOver2;
				}

				int num49 = 130;
				if (Main.expertMode)
					num49 = 90;

				if (npc.ai[2] >= (float)num49) {
					npc.ai[3] += 1f;
					npc.ai[2] = 0f;
					npc.target = 255;
					npc.rotation = num7;
					if (npc.ai[3] >= 3f) {
						npc.ai[1] = 0f;
						npc.ai[3] = 0f;
						if (Main.expertMode && Main.netMode != 1 && LifLowerOrEqualHalf(npc)) {
							npc.ai[1] = 3f;
							npc.ai[3] += Main.rand.Next(1, 4);
						}

						npc.netUpdate = true;
						if (npc.netSpam > 10)
							npc.netSpam = 10;
					}
					else {
						npc.ai[1] = 1f;
					}
				}
			}
			else if (npc.ai[1] == 3f) {
				if (npc.ai[3] == 4f && flag2 && npc.Center.Y > Main.player[npc.target].Center.Y) {
					npc.TargetClosest();
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					npc.netUpdate = true;
					if (npc.netSpam > 10)
						npc.netSpam = 10;
				}
				else if (Main.netMode != 1) {
					npc.TargetClosest();
					float num50 = 20f;
					//TODO : re write code here
					Vector2 vector10 = npc.Center;
					float num51 = Main.player[npc.target].Center.X - vector10.X;
					float num52 = Main.player[npc.target].Center.Y - vector10.Y;
					float num53 = Math.Abs(Main.player[npc.target].velocity.X) + Math.Abs(Main.player[npc.target].velocity.Y) / 4f;
					num53 += 10f - num53;
					if (num53 < 5f)
						num53 = 5f;

					if (num53 > 15f)
						num53 = 15f;

					if (npc.ai[2] == -1f && !flag3) {
						num53 *= 4f;
						num50 *= 1.3f;
					}

					if (flag3)
						num53 *= 2f;

					num51 -= Main.player[npc.target].velocity.X * num53;
					num52 -= Main.player[npc.target].velocity.Y * num53 / 4f;
					num51 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					num52 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					if (flag3) {
						num51 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
						num52 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					}

					float num54 = (float)Math.Sqrt(num51 * num51 + num52 * num52);
					float num55 = num54;
					num54 = num50 / num54;
					npc.velocity.X = num51 * num54;
					npc.velocity.Y = num52 * num54;
					npc.velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
					npc.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
					if (flag3) {
						npc.velocity.X += (float)Main.rand.Next(-50, 51) * 0.1f;
						npc.velocity.Y += (float)Main.rand.Next(-50, 51) * 0.1f;
						float num56 = Math.Abs(npc.velocity.X);
						float num57 = Math.Abs(npc.velocity.Y);
						if (npc.Center.X > Main.player[npc.target].Center.X)
							num57 *= -1f;

						if (npc.Center.Y > Main.player[npc.target].Center.Y)
							num56 *= -1f;

						npc.velocity.X = num57 + npc.velocity.X;
						npc.velocity.Y = num56 + npc.velocity.Y;
						npc.velocity.Normalize();
						npc.velocity *= num50;
						npc.velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
						npc.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
					}
					else if (num55 < 100f) {
						if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y)) {
							float num58 = Math.Abs(npc.velocity.X);
							float num59 = Math.Abs(npc.velocity.Y);
							if (npc.Center.X > Main.player[npc.target].Center.X)
								num59 *= -1f;

							if (npc.Center.Y > Main.player[npc.target].Center.Y)
								num58 *= -1f;

							npc.velocity.X = num59;
							npc.velocity.Y = num58;
						}
					}
					else if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y)) {
						float num60 = (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) / 2f;
						float num61 = num60;
						if (npc.Center.X > Main.player[npc.target].Center.X)
							num61 *= -1f;

						if (npc.Center.Y > Main.player[npc.target].Center.Y)
							num60 *= -1f;

						npc.velocity.X = num61;
						npc.velocity.Y = num60;
					}

					npc.ai[1] = 4f;
					npc.netUpdate = true;
					if (npc.netSpam > 10)
						npc.netSpam = 10;
				}
			}
			else if (npc.ai[1] == 4f) {
				if (npc.ai[2] == 0f) {
					SoundEngine.PlaySound(SoundID.ForceRoar, npc.position);
				}

				float num62 = num4;
				npc.ai[2] += 1f;
				if (npc.ai[2] == num62 && Vector2.Distance(npc.position, Main.player[npc.target].position) < 200f)
					npc.ai[2] -= 1f;

				if (npc.ai[2] >= num62) {
					npc.velocity *= 0.9f;
					if (!npc.velocity.IsLimitReached(1)) {
						npc.velocity = Vector2.Zero;
					}
				}
				else {
					npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
				}

				float num63 = num62 + 13f;
				if (npc.ai[2] >= num63) {
					npc.netUpdate = true;
					if (npc.netSpam > 10)
						npc.netSpam = 10;

					npc.ai[3] += 1f;
					npc.ai[2] = 0f;
					if (npc.ai[3] >= 5f) {
						npc.ai[1] = 0f;
						npc.ai[3] = 0f;
						if (npc.target >= 0 && Main.getGoodWorld && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, npc.width, npc.height)) {
							SoundEngine.PlaySound(SoundID.Roar, npc.position);
							npc.ai[0] = 2f;
							npc.ai[1] = 0f;
							npc.ai[2] = 0f;
							npc.ai[3] = 1f;
							npc.netUpdate = true;
						}
					}
					else {
						npc.ai[1] = 3f;
					}
				}
			}
			else if (npc.ai[1] == 5f) {
				float num64 = 600f;
				float num65 = 9f;
				float num66 = 0.3f;
				//TODO : re write code here
				Vector2 vector11 = npc.Center;
				float num67 = Main.player[npc.target].Center.X - vector11.X;
				float num68 = Main.player[npc.target].Center.Y + num64 - vector11.Y;
				float num69 = (float)Math.Sqrt(num67 * num67 + num68 * num68);
				num69 = num65 / num69;
				num67 *= num69;
				num68 *= num69;
				if (npc.velocity.X < num67) {
					npc.velocity.X += num66;
					if (npc.velocity.X < 0f && num67 > 0f)
						npc.velocity.X += num66;
				}
				else if (npc.velocity.X > num67) {
					npc.velocity.X -= num66;
					if (npc.velocity.X > 0f && num67 < 0f)
						npc.velocity.X -= num66;
				}

				if (npc.velocity.Y < num68) {
					npc.velocity.Y += num66;
					if (npc.velocity.Y < 0f && num68 > 0f)
						npc.velocity.Y += num66;
				}
				else if (npc.velocity.Y > num68) {
					npc.velocity.Y -= num66;
					if (npc.velocity.Y > 0f && num68 < 0f)
						npc.velocity.Y -= num66;
				}

				npc.ai[2] += 1f;
				if (npc.ai[2] >= 70f) {
					npc.TargetClosest();
					npc.ai[1] = 3f;
					npc.ai[2] = -1f;
					npc.ai[3] = Main.rand.Next(-3, 1);
					npc.netUpdate = true;
				}
			}

			if (flag3 && npc.ai[1] == 5f)
				npc.ai[1] = 3f;

		}
	}
}
