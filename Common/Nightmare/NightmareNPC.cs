using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.BuffAndDebuff;

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
			if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail) {
				npc.velocity /= 2;
			}
			if (npc.type == NPCID.BrainofCthulhu) {
				if (npc.ai[0] < 0) {
					npc.velocity /= 3f;
				}
			}
			if (npc.type == NPCID.Creeper) {
				npc.velocity /= 1.5f;
			}
			if (npc.type == NPCID.ServantofCthulhu) {
				npc.velocity /= 3;
			}
			if (npc.type == NPCID.KingSlime) {
				npc.velocity /= 1.25f;
				KingSlimeAI(npc);
				return false;
			}
			return base.PreAI(npc);
		}
		public override void AI(NPC npc) {
			base.AI(npc);
		}
		private bool LifLowerOrEqualHalf(NPC npc) => npc.life <= npc.lifeMax * .5f;
		/// <summary>
		/// We need to deeply interfere with KS ai and so I port entire KS code from vanilla<br/>
		/// We could have done it like fargo but I don't like that approach cause it is too limiting
		/// We could also steal code from calamity but hey, we aren't gonna use their unique AI
		/// </summary>
		/// <param name="npc"></param>
		private void KingSlimeAI(NPC npc) {
			aiTimer++;
			if(aiTimer == int.MaxValue) {
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

			//This is for checking whenever player is dead
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
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, towardPlayer.Vector2RotateByRandom(30).Vector2RandomSpread(2, Main.rand.NextFloat(.75f,1.25f)), ProjectileID.SpikedSlimeSpike, npc.damage, 4f);
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
		/// This is where King slime would do a massive jump, only when 
		/// </summary>
		/// <param name="npc">The king slime himself</param>
		/// <param name="JumpStrength">This dictate how much velocity.Y will KS gain, must be negative</param>
		/// <param name="MoveSpeed">This dictate how much velocity.X will KS gain</param>
		/// <param name="DelayAttack">This is a timer that count upward, so setting negative mean it will delay his attack even longer</param>
		private void KingSlimeStartOfBigJump(NPC npc, ref float JumpStrength, ref float MoveSpeed, ref float DelayAttack) {
			for (int i = 0; i < 16; i++) {
				Vector2 spreadoutring = Vector2.One.Vector2DistributeEvenly(16, 360, i) * 10f;
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, spreadoutring, ProjectileID.SpikedSlimeSpike, npc.damage, 4f);
			}
			if (npc.life <= npc.lifeMax * .75f) {
				DelayAttack += 30;
			}
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
			if (npc.type == NPCID.BrainofCthulhu) {
				if (npc.ai[0] < 0) {
					npc.velocity *= 3f;
				}
				npc.localAI[1] += 1;
			}
			if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail) {
				npc.velocity *= 2;
			}
			if (npc.type == NPCID.Creeper) {
				npc.velocity *= 1.5f;
			}
			if (npc.type == NPCID.KingSlime) {
				npc.velocity *= 1.25f;
			}
			if (npc.type == NPCID.ServantofCthulhu) {
				npc.velocity *= 3f;
			}
			if (npc.type == NPCID.CultistBoss) {
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
	}
}
