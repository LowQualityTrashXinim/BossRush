using BossRush.Contents.BuffAndDebuff;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Nightmare {
	internal class MasochistNPC : GlobalNPC {
		public override void SetDefaults(NPC npc) {
			if (!ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return;
			}
			npc.trapImmune = true;
			npc.lavaImmune = true;
			BossChange(npc);
			if (npc.type == NPCID.ServantofCthulhu) {
				npc.scale += 1.5f;
				npc.Size += new Vector2(50, 50);
				npc.lifeMax += 300;
			}
			npc.knockBackResist *= .5f;
		}
		private void BossChange(NPC npc) {
			if (!npc.boss)
				return;
			if (npc.type == NPCID.CultistBoss) {
				npc.lifeMax += 15000;
				npc.defense += 30;
			}
			if (npc.type == NPCID.KingSlime) {
				npc.defense += 10;
				npc.lifeRegen += 1;
			}
			if (npc.type == NPCID.EyeofCthulhu) {
				npc.scale -= 0.25f;
				npc.Size -= new Vector2(25, 25);
			}
			if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsBody) {
				npc.scale += 2.5f;
				npc.Size += new Vector2(200, 200);
				npc.lifeMax += 1500;
			}
		}
		public override void OnSpawn(NPC npc, IEntitySource source) {
			if (!ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return;
			}
			npc.damage += Main.rand.Next(npc.damage + 1);
			npc.lifeMax += Main.rand.Next(npc.lifeMax + 1);
			npc.life = npc.lifeMax;
		}
		public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
			if (!ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return;
			}
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.BrokenArmor, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Cursed, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Bleeding, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Burning, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Weak, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.CursedInferno, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Ichor, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Venom, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Poisoned, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Slow, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.ManaSickness, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.PotionSickness, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Obstructed, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Blackout, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Confused, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Darkness, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Electrified, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Stoned, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.WitheredArmor, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.WitheredWeapon, Main.rand.Next(1, 901));
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Suffocation, Main.rand.Next(1, 901));
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
			if (ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				maxSpawns += 100;
				spawnRate -= 10;
			}
		}
		public override bool PreAI(NPC npc) {
			return base.PreAI(npc);
		}
		public override void AI(NPC npc) {
			if (ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				//if (npc.aiStyle == 15) {
				//	KingSlimeAI(npc);
				//}
				//return;
			}
			base.AI(npc);
		}
		/// <summary>
		/// We need to deeply interfere with KS ai and so I port entire KS code from vanilla<br/>
		/// We could have done it like fargo but I don't like that approach cause it is too limiting
		/// </summary>
		/// <param name="npc"></param>
		private void KingSlimeAI(NPC npc) {
			float num236 = 1f;
			float num237 = 1f;
			bool flag7 = false;
			bool flag8 = false;
			//bool flag6 = false;
			// I have comment out flag6 cause it seem to do nothing beside allowing KS to resize ( which always is true ???) ?
			// Maybe terraria dev plan to at some point to reuse King slime AI onto other slime ?
			// I have look at wiki and found at no point this ai is reuse so idk https://terraria.wiki.gg/wiki/AI
			//Not sure what num 238 even do tbh
			if (Main.getGoodWorld) {
				num237 *= 2 - 1f - npc.life / (float)npc.lifeMax;
			}

			npc.aiAction = 0;
			if (npc.ai[3] == 0f && npc.life > 0)
				npc.ai[3] = npc.lifeMax;

			//On first AI
			if (npc.localAI[3] == 0f) {
				OnFirstSpawnKingSlime(npc);
				npc.localAI[3] = 1f;
				//flag6 = true;
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					npc.ai[0] = -100f;
					npc.TargetClosest();
					npc.netUpdate = true;
				}
			}

			int distance = 3000;
			if (Main.player[npc.target].dead || BossRushUtils.CompareSquareFloatValue(npc.Center, Main.player[npc.target].Center, distance)) {
				npc.TargetClosest();
				if (Main.player[npc.target].dead || BossRushUtils.CompareSquareFloatValue(npc.Center, Main.player[npc.target].Center, distance)) {
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
					KingSlimeTeleport(npc);
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
				KingSlimeStandingStill(npc);
				npc.velocity.X *= 0.8f;
				if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
					npc.velocity.X = 0f;

				if (!flag7) {
					npc.ai[0] += 2f;
					if (npc.life < npc.lifeMax * 0.8f) {
						npc.ai[0] += 1f;
						KingSlimePhase1(npc);
					}

					if (npc.life < npc.lifeMax * 0.6f) {
						npc.ai[0] += 1f;
						KingSlimePhase2(npc);
					}

					if (npc.life < npc.lifeMax * 0.4f) {
						npc.ai[0] += 2f;
						KingSlimePhase3(npc);
					}

					if (npc.life < npc.lifeMax * 0.2f) {
						npc.ai[0] += 3f;
						KingSlimePhase4(npc);
					}

					if (npc.life < npc.lifeMax * 0.1f) {
						npc.ai[0] += 4f;
						KingSlimePhase5(npc);
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
			if (num255 != npc.scale /*|| flag6*/) {
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
			int RandomSpawnNPCamount = Main.rand.Next(1, 4);
			for (int i = 0; i < RandomSpawnNPCamount; i++) {
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
		private void OnFirstSpawnKingSlime(NPC npc) {

		}
		private void KingSlimeStandingStill(NPC npc) {

		}
		private void KingSlimePhase1(NPC npc) {

		}
		private void KingSlimePhase2(NPC npc) {

		}
		private void KingSlimePhase3(NPC npc) {

		}
		private void KingSlimePhase4(NPC npc) {

		}
		private void KingSlimePhase5(NPC npc) {

		}
		private void KingSlimeTeleport(NPC npc) {

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
			if (npc.life < npc.lifeMax * .5f) {
				JumpStrength -= 3;
				MoveSpeed += 2f;
				DelayAttack += 30;
			}
		}
		public override void PostAI(NPC npc) {
			base.PostAI(npc);
			if (!ModContent.GetInstance<BossRushModConfig>().Nightmare) {
				return;
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
					for (int i = 0; i < 3; i++) {
						int dust = Dust.NewDust(Main.player[npc.target].Center, 0, 0, DustID.SolarFlare);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(7f, 7f);
						Main.dust[dust].fadeIn = 2f;
					}
				}
			}
		}
	}
}
