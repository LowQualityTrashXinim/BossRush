using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Common.Systems.WorldSystem;
internal class PostBossFightSystem : ModSystem {
	public static bool DespawnEncouragement_AIStyle2_FloatingEye_IsDiscouraged(NPC npc) {
		if (!Main.player[npc.target].ZoneGraveyard && npc.position.Y <= Main.worldSurface * 16.0) {
			if (npc.type != NPCID.DemonEye
				&& npc.type != NPCID.WanderingEye
				&& npc.type != NPCID.CataractEye
				&& npc.type != NPCID.SleepyEye
				&& npc.type != NPCID.DialatedEye
				&& npc.type != NPCID.GreenEye
				&& npc.type != NPCID.PurpleEye
				&& npc.type != NPCID.DemonEyeOwl)
				return npc.type == NPCID.DemonEyeSpaceship;

			return true;
		}

		return false;
	}
}
class PostBossFightGlobalNPC : GlobalNPC {
	public override bool PreAI(NPC npc) {
		if (npc.aiStyle == 2) {
			AIStyle_02(npc);
			return false;
		}
		return base.PreAI(npc);
	}
	private void AIStyle_02(NPC npc) {
		if ((npc.type == NPCID.PigronCorruption || npc.type == NPCID.PigronHallow || npc.type == NPCID.PigronCrimson) && Main.rand.NextBool(1000))
			SoundEngine.PlaySound(SoundID.Zombie9);

		npc.noGravity = true;
		if (!npc.noTileCollide) {
			if (npc.collideX) {
				npc.velocity.X = npc.oldVelocity.X * -0.5f;
				if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
					npc.velocity.X = 2f;

				if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
					npc.velocity.X = -2f;
			}

			if (npc.collideY) {
				npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
				if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
					npc.velocity.Y = 1f;

				if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
					npc.velocity.Y = -1f;
			}
		}

		if (Main.player[npc.target].dead || !Main.player[npc.target].active) {
			npc.TargetClosest();
			if (Main.player[npc.target].dead || !Main.player[npc.target].active) {
				npc.EncourageDespawn(10);
				npc.directionY = -1;
				if (npc.velocity.Y > 0f)
					npc.direction = 1;

				npc.direction = -1;
				if (npc.velocity.X > 0f)
					npc.direction = 1;
			}
		}
		else {
			npc.TargetClosest();
		}

		if (npc.type == NPCID.PigronCorruption || npc.type == NPCID.PigronHallow || npc.type == NPCID.PigronCrimson) {
			if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height)) {
				if (npc.ai[1] > 0f && !Collision.SolidCollision(npc.position, npc.width, npc.height)) {
					npc.ai[1] = 0f;
					npc.ai[0] = 0f;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[1] == 0f) {
				npc.ai[0] += 1f;
			}

			if (npc.ai[0] >= 300f) {
				npc.ai[1] = 1f;
				npc.ai[0] = 0f;
				npc.netUpdate = true;
			}

			if (npc.ai[1] == 0f) {
				npc.alpha = 0;
				npc.noTileCollide = false;
			}
			else {
				npc.wet = false;
				npc.alpha = 200;
				npc.noTileCollide = true;
			}

			npc.rotation = npc.velocity.Y * 0.1f * (float)npc.direction;
			npc.TargetClosest();
			if (npc.direction == -1 && npc.velocity.X > -4f && npc.position.X > Main.player[npc.target].position.X + (float)Main.player[npc.target].width) {
				npc.velocity.X -= 0.08f;
				if (npc.velocity.X > 4f)
					npc.velocity.X -= 0.04f;
				else if (npc.velocity.X > 0f)
					npc.velocity.X -= 0.2f;

				if (npc.velocity.X < -4f)
					npc.velocity.X = -4f;
			}
			else if (npc.direction == 1 && npc.velocity.X < 4f && npc.position.X + (float)npc.width < Main.player[npc.target].position.X) {
				npc.velocity.X += 0.08f;
				if (npc.velocity.X < -4f)
					npc.velocity.X += 0.04f;
				else if (npc.velocity.X < 0f)
					npc.velocity.X += 0.2f;
				if (npc.velocity.X > 4f)
					npc.velocity.X = 4f;
			}

			if (npc.directionY == -1 && npc.velocity.Y > -2.5 && npc.position.Y > Main.player[npc.target].position.Y + (float)Main.player[npc.target].height) {
				npc.velocity.Y -= 0.1f;
				if (npc.velocity.Y > 2.5)
					npc.velocity.Y -= 0.05f;
				else if (npc.velocity.Y > 0f)
					npc.velocity.Y -= 0.15f;

				if (npc.velocity.Y < -2.5)
					npc.velocity.Y = -2.5f;
			}
			else if (npc.directionY == 1 && npc.velocity.Y < 2.5 && npc.position.Y + npc.height < Main.player[npc.target].position.Y) {
				npc.velocity.Y += 0.1f;
				if (npc.velocity.Y < -2.5)
					npc.velocity.Y += 0.05f;
				else if (npc.velocity.Y < 0f)
					npc.velocity.Y += 0.15f;
				if (npc.velocity.Y > 2.5)
					npc.velocity.Y = 2.5f;
			}
		}
		else if (npc.type == NPCID.TheHungryII) {
			npc.TargetClosest();
			Lighting.AddLight((int)(npc.position.X + npc.width * .5f) / 16, (int)(npc.position.Y + npc.height * .5f) / 16, 0.3f, 0.2f, 0.1f);
			if (npc.direction == -1 && npc.velocity.X > -6f) {
				npc.velocity.X -= 0.1f;
				if (npc.velocity.X > 6f)
					npc.velocity.X -= 0.1f;
				else if (npc.velocity.X > 0f)
					npc.velocity.X -= 0.2f;

				if (npc.velocity.X < -6f)
					npc.velocity.X = -6f;
			}
			else if (npc.direction == 1 && npc.velocity.X < 6f) {
				npc.velocity.X += 0.1f;
				if (npc.velocity.X < -6f)
					npc.velocity.X += 0.1f;
				else if (npc.velocity.X < 0f)
					npc.velocity.X += 0.2f;

				if (npc.velocity.X > 6f)
					npc.velocity.X = 6f;
			}

			if (npc.directionY == -1 && npc.velocity.Y > -2.5) {
				npc.velocity.Y -= 0.04f;
				if (npc.velocity.Y > 2.5)
					npc.velocity.Y -= 0.05f;
				else if (npc.velocity.Y > 0f)
					npc.velocity.Y -= 0.15f;

				if (npc.velocity.Y < -2.5)
					npc.velocity.Y = -2.5f;
			}
			else if (npc.directionY == 1 && npc.velocity.Y < 1.5) {
				npc.velocity.Y += 0.04f;
				if (npc.velocity.Y < -2.5)
					npc.velocity.Y += 0.05f;
				else if (npc.velocity.Y < 0f)
					npc.velocity.Y += 0.15f;

				if (npc.velocity.Y > 2.5)
					npc.velocity.Y = 2.5f;
			}

			if (Main.rand.NextBool(40)) {
				npc.position += npc.netOffset;
				int num = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), 5, npc.velocity.X, 2f);
				Main.dust[num].velocity.X *= 0.5f;
				Main.dust[num].velocity.Y *= 0.1f;
				npc.position -= npc.netOffset;
			}
		}
		else if (npc.type == NPCID.WanderingEye) {
			if (npc.life < npc.lifeMax * 0.5) {
				if (npc.direction == -1 && npc.velocity.X > -6f) {
					npc.velocity.X -= 0.1f;
					if (npc.velocity.X > 6f)
						npc.velocity.X -= 0.1f;
					else if (npc.velocity.X > 0f)
						npc.velocity.X += 0.05f;

					if (npc.velocity.X < -6f)
						npc.velocity.X = -6f;
				}
				else if (npc.direction == 1 && npc.velocity.X < 6f) {
					npc.velocity.X += 0.1f;
					if (npc.velocity.X < -6f)
						npc.velocity.X += 0.1f;
					else if (npc.velocity.X < 0f)
						npc.velocity.X -= 0.05f;

					if (npc.velocity.X > 6f)
						npc.velocity.X = 6f;
				}

				if (npc.directionY == -1 && npc.velocity.Y > -4f) {
					npc.velocity.Y -= 0.1f;
					if (npc.velocity.Y > 4f)
						npc.velocity.Y -= 0.1f;
					else if (npc.velocity.Y > 0f)
						npc.velocity.Y += 0.05f;

					if (npc.velocity.Y < -4f)
						npc.velocity.Y = -4f;
				}
				else if (npc.directionY == 1 && npc.velocity.Y < 4f) {
					npc.velocity.Y += 0.1f;
					if (npc.velocity.Y < -4f)
						npc.velocity.Y += 0.1f;
					else if (npc.velocity.Y < 0f)
						npc.velocity.Y -= 0.05f;

					if (npc.velocity.Y > 4f)
						npc.velocity.Y = 4f;
				}
			}
			else {
				if (npc.direction == -1 && npc.velocity.X > -4f) {
					npc.velocity.X -= 0.1f;
					if (npc.velocity.X > 4f)
						npc.velocity.X -= 0.1f;
					else if (npc.velocity.X > 0f)
						npc.velocity.X += 0.05f;

					if (npc.velocity.X < -4f)
						npc.velocity.X = -4f;
				}
				else if (npc.direction == 1 && npc.velocity.X < 4f) {
					npc.velocity.X += 0.1f;
					if (npc.velocity.X < -4f)
						npc.velocity.X += 0.1f;
					else if (npc.velocity.X < 0f)
						npc.velocity.X -= 0.05f;

					if (npc.velocity.X > 4f)
						npc.velocity.X = 4f;
				}

				if (npc.directionY == -1 && npc.velocity.Y > -1.5) {
					npc.velocity.Y -= 0.04f;
					if (npc.velocity.Y > 1.5)
						npc.velocity.Y -= 0.05f;
					else if (npc.velocity.Y > 0f)
						npc.velocity.Y += 0.03f;

					if (npc.velocity.Y < -1.5)
						npc.velocity.Y = -1.5f;
				}
				else if (npc.directionY == 1 && npc.velocity.Y < 1.5) {
					npc.velocity.Y += 0.04f;
					if (npc.velocity.Y < -1.5)
						npc.velocity.Y += 0.05f;
					else if (npc.velocity.Y < 0f)
						npc.velocity.Y -= 0.03f;

					if (npc.velocity.Y > 1.5)
						npc.velocity.Y = 1.5f;
				}
			}
		}
		else {
			float num2 = 4f;
			float num3 = 1.5f;
			num2 *= 1f + (1f - npc.scale);
			num3 *= 1f + (1f - npc.scale);
			if (npc.direction == -1 && npc.velocity.X > 0f - num2) {
				npc.velocity.X -= 0.1f;
				if (npc.velocity.X > num2)
					npc.velocity.X -= 0.1f;
				else if (npc.velocity.X > 0f)
					npc.velocity.X += 0.05f;

				if (npc.velocity.X < 0f - num2)
					npc.velocity.X = 0f - num2;
			}
			else if (npc.direction == 1 && npc.velocity.X < num2) {
				npc.velocity.X += 0.1f;
				if (npc.velocity.X < 0f - num2)
					npc.velocity.X += 0.1f;
				else if (npc.velocity.X < 0f)
					npc.velocity.X -= 0.05f;

				if (npc.velocity.X > num2)
					npc.velocity.X = num2;
			}

			if (npc.directionY == -1 && npc.velocity.Y > 0f - num3) {
				npc.velocity.Y -= 0.04f;
				if (npc.velocity.Y > num3)
					npc.velocity.Y -= 0.05f;
				else if (npc.velocity.Y > 0f)
					npc.velocity.Y += 0.03f;

				if (npc.velocity.Y < 0f - num3)
					npc.velocity.Y = 0f - num3;
			}
			else if (npc.directionY == 1 && npc.velocity.Y < num3) {
				npc.velocity.Y += 0.04f;
				if (npc.velocity.Y < 0f - num3)
					npc.velocity.Y += 0.05f;
				else if (npc.velocity.Y < 0f)
					npc.velocity.Y -= 0.03f;

				if (npc.velocity.Y > num3)
					npc.velocity.Y = num3;
			}
		}

		if ((npc.type == NPCID.DemonEye
			|| npc.type == NPCID.WanderingEye
			|| npc.type == NPCID.CataractEye
			|| npc.type == NPCID.SleepyEye
			|| npc.type == NPCID.DialatedEye
			|| npc.type == NPCID.GreenEye
			|| npc.type == NPCID.PurpleEye) && Main.rand.NextBool(40)) {
			npc.position += npc.netOffset;
			int num4 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), DustID.Blood, npc.velocity.X, 2f);
			Main.dust[num4].velocity.X *= 0.5f;
			Main.dust[num4].velocity.Y *= 0.1f;
			npc.position -= npc.netOffset;
		}

		if (npc.wet && npc.type != NPCID.PigronCorruption && npc.type != NPCID.PigronHallow) {
			if (npc.velocity.Y > 0f)
				npc.velocity.Y *= 0.95f;

			npc.velocity.Y -= 0.5f;
			if (npc.velocity.Y < -4f)
				npc.velocity.Y = -4f;

			npc.TargetClosest();
		}
	}

	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
		if (NPC.downedSlimeKing) {
			//Slime
			pool.Add(NPCID.GreenSlime, 1);
			pool.Add(NPCID.BlueSlime, 1);
			pool.Add(NPCID.PurpleSlime, 1);
			pool.Add(NPCID.RedSlime, 1);
			pool.Add(NPCID.YellowSlime, 1);
			pool.Add(NPCID.BlackSlime, 1);
			pool.Add(NPCID.MotherSlime, 1);
			pool.Add(NPCID.SpikedJungleSlime, 1);
			pool.Add(NPCID.SpikedIceSlime, 1);
			pool.Add(NPCID.UmbrellaSlime, 1);
			pool.Add(NPCID.SlimeSpiked, 1);
			if (Main.getGoodWorld) {
				pool.Add(NPCID.LavaSlime, 1);
			}
		}
		if (NPC.downedBoss1) {
			//eye
			pool.Add(NPCID.DemonEye, 0.75f);
			pool.Add(NPCID.DemonEye2, 0.75f);
			pool.Add(NPCID.DemonEyeOwl, 0.75f);
			pool.Add(NPCID.DemonEyeSpaceship, 0.75f);
			pool.Add(NPCID.CataractEye, 0.75f);
			pool.Add(NPCID.CataractEye2, 0.75f);
			pool.Add(NPCID.DialatedEye, 0.75f);
			pool.Add(NPCID.DialatedEye2, 0.75f);
			pool.Add(NPCID.GreenEye, 0.75f);
			pool.Add(NPCID.GreenEye2, 0.75f);
			pool.Add(NPCID.PurpleEye, 0.75f);
			pool.Add(NPCID.PurpleEye2, 0.75f);
			pool.Add(NPCID.WanderingEye, 0.65f);
			pool.Add(NPCID.EyeballFlyingFish, 0.45f);
		}
		//if (NPC.downedBoss2) {
		//	pool.Add(NPCID.Corruptor, 0.25f);
		//	pool.Add(NPCID.Slimer, 0.25f);
		//}
		//if (NPC.downedBoss2) {
		//	pool.Add(NPCID.CrimsonBunny, 0.25f);
		//	pool.Add(NPCID.CrimsonGoldfish, 0.25f);
		//}
		if (NPC.downedQueenBee) {
			//bee
			pool.Add(NPCID.Bee, 0.8f);
			pool.Add(NPCID.BeeSmall, 0.8f);
			//Hornet
			pool.Add(NPCID.Hornet, 0.7f);
			pool.Add(NPCID.HornetFatty, 0.7f);
			pool.Add(NPCID.HornetHoney, 0.7f);
			pool.Add(NPCID.HornetLeafy, 0.7f);
			pool.Add(NPCID.HornetSpikey, 0.7f);
			pool.Add(NPCID.HornetStingy, 0.7f);
			//MossHornet
			pool.Add(NPCID.MossHornet, 0.5f);
			pool.Add(NPCID.BigMossHornet, 0.5f);
			pool.Add(NPCID.GiantMossHornet, 0.5f);
			pool.Add(NPCID.LittleMossHornet, 0.5f);
			pool.Add(NPCID.TinyMossHornet, 0.5f);
		}
	}
}
