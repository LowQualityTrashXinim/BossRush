using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using BossRush.Contents.Items;
using Terraria.GameContent;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.NPCs {
	internal class LootBoxLord : ModNPC {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
		public override void SetStaticDefaults() {
			NPCID.Sets.DontDoHardmodeScaling[Type] = true;
			NPCID.Sets.NeedsExpertScaling[Type] = false;
		}
		public override void SetDefaults() {
			NPC.lifeMax = 54000;
			NPC.damage = 150;
			NPC.defense = 50;
			NPC.width = 38;
			NPC.height = 30;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.knockBackResist = 0f;
			NPC.boss = true;
			NPC.dontTakeDamage = true;
			NPC.strengthMultiplier = 1;
			NPC.ScaleStats_UseStrengthMultiplier(1);
			NPC.GetGlobalNPC<RoguelikeGlobalNPC>().NPC_SpecialException = true;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			//for (int i = 0; i < TerrariaArrayID.MeleePreBoss.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleePreBoss[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.MeleePreEoC.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleePreEoC[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.MeleeEvilBoss.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleeEvilBoss[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.RangePreBoss.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangePreBoss[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.RangePreEoC.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangePreEoC[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.RangeSkele.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangeSkele[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.MagicPreBoss.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicPreBoss[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.MagicPreEoC.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicPreEoC[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.MagicSkele.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicSkele[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.SummonPreBoss.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonPreBoss[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.SummonerPreEoC.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonerPreEoC[i]));
			//}
			//for (int i = 0; i < TerrariaArrayID.SummonSkele.Length; i++) {
			//	npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonSkele[i]));
			//}
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PowerEnergy>()));
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
			float adjustment = 1;
			if (Main.expertMode)
				adjustment = 2;
			else if (Main.masterMode)
				adjustment = 3;
			else
				adjustment = 1;

			NPC.lifeMax = (int)(54000 / adjustment);
			NPC.life = NPC.lifeMax;
			NPC.damage = (int)(150 / adjustment);
			NPC.defense = (int)(50 / adjustment);
		}
		public int BossDamagePercentage(float percentage) => (int)Math.Ceiling(NPC.damage * percentage);
		//Use NPC.ai[0] to delay attack
		//Use NPC.ai[1] to switch attack
		//Use NPC.ai[2] for calculation
		//Use NPC.ai[3] to do movement
		bool AlreadySaidThat = false;
		bool BeforeAttack = true;
		bool CanTrackPlayer = false;
		public override void AI() {
			if (BeforeAttack) {
				if (NPC.ai[0] == 0) {
					BossRushUtils.CombatTextRevamp(NPC.Hitbox, Color.Yellow, "Hmm..");
					NPC.ai[0] = 1;
				}
				else if (NPC.ai[1] == 0) {
					if (NPC.ai[0] > 120) {
						BossRushUtils.CombatTextRevamp(NPC.Hitbox, Color.Yellow, "Thy see... thou seek the treasure");
						NPC.ai[1] = 1;
					}
					else {
						NPC.ai[0]++;
					}
				}
				else if (NPC.ai[2] == 0) {
					if (NPC.ai[1] > 120) {
						BossRushUtils.CombatTextRevamp(NPC.Hitbox, Color.Yellow, "Very well, prove thy worthiness");
						NPC.ai[2] = 1;
						BeforeAttack = false;
						NPC.dontTakeDamage = false;
						ResetEverything(120);
					}
					else {
						NPC.ai[1]++;
					}
				}
				return;
			}
			Player player = Main.player[NPC.target];
			if (player.dead || !player.active) {
				NPC.FindClosestPlayer();
				NPC.TargetClosest();
				player = Main.player[NPC.target];
				if (player.dead || !player.active) {
					NPC.active = false;
				}
			}
			if (NPC.CountNPCS(Type) > 1) {
				if (!AlreadySaidThat) {
					BossRushUtils.CombatTextRevamp(NPC.Hitbox, Color.Red, "Do not");
					AlreadySaidThat = true;
					if (!NPC.AnyNPCs(ModContent.NPCType<ElderGuardian>()))
						NPC.NewNPC(NPC.GetSource_FromAI(), NPC.Hitbox.X, NPC.Hitbox.Y, ModContent.NPCType<ElderGuardian>());
				}
			}
			//TODO : change phase when boss hp is below 50%
			//Move above the player
			switch (NPC.ai[1]) {
				case 0:
					Move(player);
					break;
				case 1:
					ShootShortSword();
					CanTrackPlayer = false;
					break;
				case 2:
					CanTrackPlayer = true;
					ShootShortSword2();
					break;
				case 3:
					ShootBroadSword();
					CanTrackPlayer = false;
					break;
				case 4:
					ShootBroadSword2();
					CanTrackPlayer = false;
					break;
				case 5:
					ShootWoodBow();
					CanTrackPlayer = false;
					break;
				case 6:
					CanTrackPlayer = true;
					ShootWoodBow2();
					break;
				case 7:
					CanTrackPlayer = true;
					ShootStaff(player);
					break;
				case 8:
					CanTrackPlayer = true;
					ShootStaff2();
					break;
				case 9:
					CanTrackPlayer = true;
					ShootOreBow1();
					break;
				case 10:
					CanTrackPlayer = true;
					ShootOreBow2();
					break;
				case 11:
					CanTrackPlayer = true;
					ShootGun(player);
					break;
			}
		}
		public override void PostAI() {
			Player player = Main.player[NPC.target];
			if (CanTrackPlayer) {
				NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * ((player.Center - NPC.Center).Length() / 32f);
			}
			if (!BossRushUtils.CompareSquareFloatValue(NPC.Center, player.Center, 1000 * 1000)) {
				NPC.life = Math.Clamp(NPC.life + 1, 0, NPC.lifeMax);
				if (Main.rand.NextBool(5)) {
					int dust = Dust.NewDust(NPC.Center + Main.rand.NextVector2Circular(30, 30), 0, 0, DustID.HealingPlus, Scale: Main.rand.NextFloat(1, 1.5f));
					Main.dust[dust].velocity = Vector2.UnitY * Main.rand.NextFloat(1, 3);
				}
			}
		}
		public override void OnKill() {
		}
		Vector2 offsetPos = Vector2.Zero;
		private void Move(Player player) {
			if (BossDelayAttack(0, 0, 0)) {
				return;
			}
			CanTrackPlayer = false;
			Vector2 positionAbovePlayer = new Vector2(player.Center.X, player.Center.Y - 200) + offsetPos;
			if (NPC.NPCMoveToPosition(positionAbovePlayer, 30f)) {
				NPC.ai[0] = 20;
				NPC.ai[1] = MoveSetHandle();
			}
		}
		private int MoveSetHandle() {
			int Move = (int)Math.Clamp(++NPC.ai[3], 1, 12);
			if (Move >= 12) {
				Move = 1;
				NPC.ai[3] = 1;
			}
			return Move;
		}
		private void ResetEverything(int delayAttack = 90) {
			lastPlayerPosition = Main.player[NPC.target].Center;
			HasReachPos = false;
			NPC.ai[0] = delayAttack;
			NPC.ai[1] = 0;
			NPC.ai[2] = 0;
			offsetPos = Vector2.Zero;
			NPC.velocity = Vector2.Zero;
		}
		private void ShootShortSword() {
			if (Main.expertMode || Main.masterMode) {
				Expert_ShortSwordAttack();
			}
			else {
				Normal_ShortSwordAttack();
			}
		}
		private void Normal_ShortSwordAttack() {
			if (BossDelayAttack(10, 0, TerrariaArrayID.AllOreShortSword.Length - 1, 30)) {
				return;
			}
			Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)NPC.ai[2]) * 15f;
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackOne>(),
				BossDamagePercentage(.75f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
			NPC.ai[2]++;
		}
		private void Expert_ShortSwordAttack() {
			if (BossDelayAttack(10, 0, TerrariaArrayID.AllOreShortSword.Length - 1, 30)) {
				return;
			}
			Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)NPC.ai[2]) * 15f;
			for (int i = 0; i < 2; i++) {
				bool vecChange = i == 0;
				int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec * vecChange.ToDirectionInt(), ModContent.ProjectileType<ShortSwordAttackOne>(),
					BossDamagePercentage(.75f), 2, NPC.target);
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
			}
			NPC.ai[2]++;
		}
		private void ShootShortSword2() {
			Vector2 positionAbovePlayer = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 350);
			NPC.NPCMoveToPosition(positionAbovePlayer, 5f);
			if (NPC.ai[2] >= TerrariaArrayID.AllOreShortSword.Length - 1) {
				NPC.velocity = Vector2.Zero;
				ResetEverything(30);
				return;
			}
			if (BossDelayAttack(20, 0, TerrariaArrayID.AllOreShortSword.Length - 1)) {
				return;
			}
			Vector2 vec = Vector2.UnitX * 20 * Main.rand.NextBool(2).ToDirectionInt();
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackTwo>(), BossDamagePercentage(.75f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
			Main.projectile[proj].ai[1] = -20;
			Main.projectile[proj].ai[0] = 2;
			Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
			NPC.ai[2]++;
		}
		private void Normal_BroadSwordAttack1() {
			if (BossDelayAttack(30, 0, TerrariaArrayID.AllOreShortSword.Length - 1)) {
				return;
			}

			Vector2 vec = Main.rand.NextVector2Circular(35, 35);
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackOne>(), BossDamagePercentage(.85f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is SwordBroadAttackOne swordProj) {
				swordProj.SetNPCOwner(NPC.whoAmI);
				swordProj.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[(int)NPC.ai[2]];
			}
			NPC.ai[2]++;
		}
		private void Expert_BroadSwordAttack1() {
			if (BossDelayAttack(30, 0, TerrariaArrayID.AllOreShortSword.Length - 1)) {
				return;
			}
			for (int i = 0; i < 3; i++) {
				Vector2 vec = Main.rand.NextVector2Circular(35, 35);
				int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackOne>(), BossDamagePercentage(.85f), 2, NPC.target);
				if (Main.projectile[proj].ModProjectile is SwordBroadAttackOne swordProj) {
					swordProj.SetNPCOwner(NPC.whoAmI);
					swordProj.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[(int)NPC.ai[2]];
				}
			}
			NPC.ai[2]++;
		}
		private void ShootBroadSword() {
			if (Main.expertMode || Main.masterMode) {
				Expert_BroadSwordAttack1();
			}
			else {
				Normal_BroadSwordAttack1();
			}
		}
		private void ShootBroadSword2() {
			NPC.ai[0] = 0;
			if (BossDelayAttack(0, 0, 0)) {
				return;
			}
			if (Main.rand.NextBool()) {
				for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
					Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 180, i) * 50f;
					vec.Y = 5;
					int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), BossDamagePercentage(.85f), 2, NPC.target);
					Main.projectile[proj].ai[1] = 35;
					if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
						projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
				}
			}
			else {
				for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
					Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 90, i) * 20f;
					int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), BossDamagePercentage(.85f), 2, NPC.target);
					Main.projectile[proj].ai[1] = 50;
					if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
						projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
				}
			}
			NPC.ai[2]++;
			BossDelayAttack(0, 0, 0, 10);

		}
		private void ShootWoodBow() {
			if (BossDelayAttack(0, 0, 0)) {
				return;
			}
			CanTrackPlayer = false;
			int length = TerrariaArrayID.AllWoodBowPHM.Length;
			for (int i = 0; i < length; i++) {
				if (TerrariaArrayID.AllWoodBowPHM[i] == ItemID.AshWoodBow) {
					int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, -Vector2.UnitY * 5, ModContent.ProjectileType<WoodBowAttackOne>(), BossDamagePercentage(.65f), 2, NPC.target);
					if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
						projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
					Main.projectile[proj].ai[2] = 15;
					continue;
				}
				if (i < length / 2) {
					Vector2 velocity = (Vector2.UnitX * 2).Vector2DistributeEvenlyPlus(3, 45, i) * 5;
					int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
						ModContent.ProjectileType<WoodBowAttackOne>(), BossDamagePercentage(.35f), 2, NPC.target);
					if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
						projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
					if (i % 2 == 1)
						Main.projectile[proj].ai[1] = -MathHelper.PiOver4;
					else
						Main.projectile[proj].ai[1] = MathHelper.PiOver4;
				}
				else {
					Vector2 velocity = (-Vector2.UnitX * 2).Vector2DistributeEvenlyPlus(3, 45, i - 3) * 5;
					int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
						ModContent.ProjectileType<WoodBowAttackOne>(), BossDamagePercentage(.35f), 2, NPC.target);
					if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
						projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
					if (i % 2 == 1)
						Main.projectile[proj].ai[1] = -MathHelper.PiOver4;
					else
						Main.projectile[proj].ai[1] = MathHelper.PiOver4;
				}
			}
			NPC.ai[2]++;
			BossDelayAttack(120, 0, 260);

		}
		private void ShootWoodBow2() {
			if (BossDelayAttack(5, 0, 0, 150)) {
				return;
			}
			for (int i = 0; i < TerrariaArrayID.AllWoodBowPHM.Length; i++) {
				Vector2 vec = Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllWoodBowPHM.Length, 120, i) * -30f;
				int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<WoodBowAttackTwo>(), BossDamagePercentage(.45f), 2, NPC.target);
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
			}
			NPC.ai[2]++;
		}
		bool HasReachPos = false;
		private void ShootStaff(Player player) {
			if (!HasReachPos) {
				lastPlayerPosition = player.Center;
			}
			if (!NPC.Center.IsCloseToPosition(lastPlayerPosition - new Vector2(0, 350), 30) && !HasReachPos) {
				NPC.NPCMoveToPosition(lastPlayerPosition - new Vector2(0, 350), 10, 30);
				CanTrackPlayer = false;
				return;
			}
			HasReachPos = true;
			BossCircleMovement(5, TerrariaArrayID.AllGemStaffPHM.Length, out float percent);
			if (BossDelayAttack(5, 0, TerrariaArrayID.AllGemStaffPHM.Length - 1, 120)) {
				CanTrackPlayer = true;
				NPC.velocity = Vector2.Zero;
				return;
			}
			Vector2 vec = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0, 360, percent)));
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<GemStaffAttackOne>(), BossDamagePercentage(.75f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileGemStaff gemstaffProj) {
				gemstaffProj.ItemIDtextureValue = TerrariaArrayID.AllGemStaffPHM[(int)NPC.ai[2]];
				gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[(int)NPC.ai[2]];
			}
			NPC.ai[2]++;
		}
		private void ShootStaff2() {
			if (BossDelayAttack(120, 0, 0, 200)) {
				return;
			}
			CanTrackPlayer = false;
			for (int i = 0; i < TerrariaArrayID.AllGemStaffPHM.Length; i++) {
				int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(12, 12), ModContent.ProjectileType<GemStaffAttackTwo>(), BossDamagePercentage(.75f), 2, NPC.target);
				Main.projectile[proj].ai[2] = i;
				Main.projectile[proj].rotation = MathHelper.PiOver4 + MathHelper.PiOver2;
				if (Main.projectile[proj].ModProjectile is BaseHostileGemStaff gemstaffProj) {
					gemstaffProj.SetNPCOwner(NPC.whoAmI);
					gemstaffProj.ItemIDtextureValue = TerrariaArrayID.AllGemStaffPHM[i];
					gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[i];
				}
			}
			NPC.ai[2]++;
		}
		private void ShootOreBow1() {
			if (BossDelayAttack(20, 0, TerrariaArrayID.AllOreBowPHM.Length - 1, 120)) {
				return;
			}
			CanTrackPlayer = false;
			Vector2 vec = Vector2.UnitY.Vector2DistributeEvenly(8, 360, (int)NPC.ai[2]) * 10f;
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<OreBowAttackOne>(), BossDamagePercentage(.55f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)NPC.ai[2]];
			Main.projectile[proj].ai[1] = 60;
			NPC.ai[2]++;
		}
		private void ShootOreBow2() {
			if (NPC.ai[2] >= TerrariaArrayID.AllOreBowPHM.Length - 1) {
				ResetEverything();
				return;
			}
			CanTrackPlayer = false;
			Vector2 positionAbovePlayer = Main.player[NPC.target].Center + new Vector2(0, -350);
			NPC.NPCMoveToPosition(positionAbovePlayer, 5f);
			if (BossDelayAttack(10, 0, TerrariaArrayID.AllOreBowPHM.Length - 1, 120)) {
				return;
			}
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<OreBowAttackTwo>(), BossDamagePercentage(.55f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)NPC.ai[2]];
			NPC.ai[2]++;
		}
		private void ShootGun(Player player) {
			Vector2 positionAbovePlayer = Main.player[NPC.target].Center + new Vector2(0, -350);
			NPC.NPCMoveToPosition(positionAbovePlayer, 5f);
			if (BossDelayAttack(BossRushUtils.ToSecond(5), 0, 0)) {
				return;
			}
			int direction;
			if (player.Center.X > NPC.Center.X) {
				direction = 1;
			}
			else {
				direction = -1;
			}
			int minishark = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileMinishark>(), BossDamagePercentage(.25f), 2, NPC.target);
			if (Main.projectile[minishark].ModProjectile is BaseHostileGun minisharkproj) {
				minisharkproj.ItemIDtextureValue = ItemID.Minishark;
				minisharkproj.Projectile.ai[2] = -direction;
			}
			int Musket = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileMusket>(), NPC.damage, 2, NPC.target);
			if (Main.projectile[Musket].ModProjectile is BaseHostileGun musketproj) {
				musketproj.ItemIDtextureValue = ItemID.Musket;
				Main.projectile[Musket].ai[2] = direction;
			}
			CanTrackPlayer = false;
			NPC.ai[2]++;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Main.instance.LoadNPC(NPC.type);
			if (NPC.velocity == Vector2.Zero) {
				return base.PreDraw(spriteBatch, screenPos, drawColor);
			}
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			Vector2 origin = NPC.Size * .5f;
			Vector2 drawPos = NPC.Center - screenPos - origin;
			for (int i = 0; i < 4; i++) {
				spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(25, 25), new Color(255, 0, 0, 50));
				spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(25, 25), new Color(0, 0, 255, 50));
				if (i == 0 || i == 2) {
					spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(45, 45), new Color(255, 0, 0, 50));
				}
				else {
					spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(45, 45), new Color(0, 0, 255, 50));
				}
			}
			return base.PreDraw(spriteBatch, screenPos, drawColor);
		}
		Vector2 lastPlayerPosition = Vector2.Zero;
		private void BossCircleMovement(int delayValue, int AttackEndValue, out float percent) {
			float total = delayValue * AttackEndValue;
			percent = Math.Clamp(((delayValue - NPC.ai[0] >= 0 ? delayValue - NPC.ai[0] : 0) + delayValue * NPC.ai[2]) / total, 0, 1f);
			float rotation = MathHelper.Lerp(0, 360, percent);
			Vector2 rotateAroundPlayerCenter = lastPlayerPosition - Vector2.UnitY.RotatedBy(MathHelper.ToRadians(rotation)) * 350;
			NPC.Center = rotateAroundPlayerCenter;
		}

		/// <summary>
		/// This is to ensure boss have a certain delay
		/// </summary>
		/// <param name="delaytime">the delay between each attack, use if you want to shoot out projectile individually or have a space out</param>
		/// <param name="nextattack">Will set the next attack</param>
		/// <param name="whenAttackwillend">determined whenever the attack will end</param>
		/// <param name="additionalDelay">the post delay after the attack is done</param>
		/// <returns></returns>
		private bool BossDelayAttack(float delaytime, float nextattack, float whenAttackwillend, int additionalDelay = 0) {
			//This only run whenever a delay is given but only when the timer reach 0 or below
			if (NPC.ai[0] <= 0) {
				NPC.ai[0] += delaytime;
			}
			else {
				//The timer decrease
				NPC.ai[0]--;
				return true;
			}
			//this will check if the counter (NPC.ai[2]) reach the requirement to reset everything
			if (NPC.ai[2] > whenAttackwillend) {
				ResetEverything();
				NPC.ai[0] += additionalDelay;
				NPC.ai[1] = nextattack;
				return true;
			}
			return false;
		}
	}
	//This code did not follow the above rule and it should be change to follow the above rule
	public abstract class BaseHostileProjectile : ModProjectile {
		public override void SetDefaults() {
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			SetHostileDefaults();
		}
		public virtual void SetHostileDefaults() { }
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public int ItemIDtextureValue = 1;
		int NPC_WhoAmI = -1;
		public bool CanDealContactDamage = true;
		public override bool CanHitPlayer(Player target) {
			return CanDealContactDamage;
		}
		public bool IsNPCActive(out NPC npc) {
			npc = null;
			if (NPC_WhoAmI < 0 && NPC_WhoAmI > 255) {
				return false;
			}
			npc = Main.npc[NPC_WhoAmI];
			if (npc.active && npc.life > 0) {
				return true;
			}
			else {
				return false;
			}
		}
		public void SetNPCOwner(int whoAmI) {
			NPC_WhoAmI = whoAmI;
		}
		public virtual void PreDrawDraw(Texture2D texture, Vector2 drawPos, Vector2 origin, ref Color lightColor, out bool DrawOrigin) { DrawOrigin = true; }
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemIDtextureValue)).Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			SpriteEffects effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			for (int i = 0; i < 3; i++) {
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(2, 2), null, new Color(255, 0, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(-2, 2), null, new Color(255, 0, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(2, -2), null, new Color(255, 0, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
				Main.EntitySpriteDraw(texture, drawPos + new Vector2(-2, -2), null, new Color(255, 0, 0, 30), Projectile.rotation, origin, Projectile.scale, effect, 0);
			}
			PreDrawDraw(texture, drawPos, origin, ref lightColor, out bool DrawOrigin);
			if (DrawOrigin) {
				Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, effect, 0);
			}
			return false;
		}
	}
	public abstract class BaseHostileShortSword : BaseHostileProjectile {
		public override void SetHostileDefaults() {
			Projectile.width = Projectile.height = 32;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		protected int OnSpawnDirection = 0;
		public override void OnSpawn(IEntitySource source) {
			OnSpawnDirection = Projectile.velocity.X > 0 ? 1 : -1;
			base.OnSpawn(source);
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(2, 3.5f));
			}
		}
	}
	class ShortSwordAttackOne : BaseHostileShortSword {
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (Projectile.ai[0] == 1) {
				if (Projectile.timeLeft > 30)
					Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				if (Projectile.timeLeft > 160)
					Projectile.timeLeft = 160;
				return;
			}
			if (Projectile.velocity.IsLimitReached(3)) {
				Projectile.velocity -= Projectile.velocity * .05f;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			else {
				Projectile.ai[0] = 1;
			}
		}
	}
	class ShortSwordAttackTwo : BaseHostileShortSword {
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			Vector2 LeftOfPlayer = new Vector2(player.Center.X + 400 * OnSpawnDirection, player.Center.Y);
			if (Projectile.ai[1] < 0) {
				Projectile.ai[1]++;
				return;
			}
			if (Projectile.ai[1] == 0) {
				if (!Projectile.Center.IsCloseToPosition(LeftOfPlayer, 10f)) {
					Vector2 distance = LeftOfPlayer - Projectile.Center;
					float length = distance.Length();
					if (length > 5) {
						length = 5;
					}
					Projectile.velocity -= Projectile.velocity * .08f;
					Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
					Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
				}
				else {
					Projectile.velocity = -Vector2.UnitX * OnSpawnDirection;
					Projectile.timeLeft = 150;
					Projectile.ai[1] = 1;
					Projectile.Center = LeftOfPlayer;
				}
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			else {
				Projectile.ai[1]++;
				if (Projectile.ai[1] >= 30) {
					if (Projectile.ai[1] >= 45) {
						Projectile.velocity -= Vector2.UnitX * 2 * OnSpawnDirection;
						return;
					}
					Projectile.velocity += Vector2.UnitX * OnSpawnDirection;
					return;
				}
			}
		}
	}
	public abstract class BaseHostileSwordBroad : BaseHostileProjectile {
		public override void SetHostileDefaults() {
			Projectile.width = Projectile.height = 36;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
	}
	class SwordBroadAttackOne : BaseHostileSwordBroad {
		bool AiChange = false;
		public override void AI() {
			if (++Projectile.ai[0] <= 40) {
				Projectile.velocity *= .96f;
				Projectile.rotation = MathHelper.ToRadians(Projectile.ai[0] * 10);
				return;
			}
			if (IsNPCActive(out NPC npc)) {
				npc.TargetClosest();
				Player player = Main.player[npc.target];
				if (!player.active || player.dead) {
					Projectile.velocity = Vector2.Zero;
					return;
				}
				if (!AiChange) {
					Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 15;
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
					Projectile.timeLeft = 150 + (int)(player.Center - Projectile.Center).Length();
					AiChange = !AiChange;
				}
			}
		}
	}
	class SwordBroadAttackTwo : BaseHostileSwordBroad {
		public override void AI() {
			Projectile.rotation = MathHelper.PiOver4 + MathHelper.PiOver2;
			if (Projectile.ai[1] == 1) {
				if (Projectile.timeLeft > 30)
					Projectile.timeLeft = 30;
				Projectile.velocity.Y = 50;
				Projectile.velocity.X = 0;
				return;
			}
			if (Projectile.ai[1] > 1) {
				Projectile.velocity.Y += -.5f;
				Projectile.ai[1]--;
				Projectile.velocity -= Projectile.velocity * .1f;
			}
			else {
				Projectile.ai[1] = 1;
				Projectile.velocity = Vector2.Zero;
			}
		}
	}
	public abstract class BaseHostileBow : BaseHostileProjectile {
		public override void SetHostileDefaults() {
			Projectile.width = 16;
			Projectile.height = 32;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
	}
	class WoodBowAttackOne : BaseHostileBow {
		public override void AI() {
			CanDealContactDamage = false;
			if (Projectile.ai[2] == 0)
				Projectile.ai[2] = 25;
			Projectile.velocity *= .97f;
			if (Projectile.timeLeft > 150) {
				Projectile.timeLeft = 150;
				Projectile.rotation = -MathHelper.PiOver2 + MathHelper.Pi + Projectile.ai[1];
			}
			if (Projectile.ai[1] == MathHelper.PiOver4)
				Projectile.rotation += MathHelper.ToRadians(5);
			else if (Projectile.ai[1] == -MathHelper.PiOver4)
				Projectile.rotation -= MathHelper.ToRadians(5);
			else {
				Projectile.rotation = (Main.player[Projectile.owner].Center - Projectile.Center).ToRotation();
			}
			if (++Projectile.ai[0] >= Projectile.ai[2]) {
				BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
				Projectile.ai[0] = 0;
			}
		}
	}
	class WoodBowAttackTwo : BaseHostileBow {
		public override void AI() {
			CanDealContactDamage = false;
			int Requirement = 35;
			if (Projectile.ai[1] <= 0)
				Projectile.rotation = (-Projectile.velocity).ToRotation();
			else {
				Requirement += 15;
			}
			if (Projectile.timeLeft > 90)
				Projectile.timeLeft = 90;
			if (++Projectile.ai[0] >= Requirement) {
				if (Projectile.ai[1] <= 0) {
					BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1);
				}
				else {
					BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
				}
				Projectile.ai[0] = 0;
				Projectile.ai[1]++;
			}
			Projectile.velocity -= Projectile.velocity * .05f;
		}
	}
	class OreBowAttackOne : BaseHostileBow {
		public override void AI() {
			CanDealContactDamage = false;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.ai[1] > 0) {
				Projectile.ai[1]--;
				Projectile.velocity *= .98f;
				return;
			}
			Player player = Main.player[Projectile.owner];
			Vector2 vel = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			if (Projectile.timeLeft > 150)
				Projectile.timeLeft = 150;
			if (++Projectile.ai[0] >= 50) {
				BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel * 15, ProjectileID.WoodenArrowHostile, Projectile.damage, 1); ;
				Projectile.ai[0] = 0;
			}
			Projectile.rotation = vel.ToRotation();
		}
	}
	class OreBowAttackTwo : BaseHostileBow {
		public override void AI() {
			CanDealContactDamage = false;
			Projectile.rotation = Vector2.UnitY.ToRotation();
			Player player = Main.player[Projectile.owner];
			if (Projectile.timeLeft > 150)
				Projectile.timeLeft = 150;
			Vector2 vel = (new Vector2(player.Center.X + Main.rand.Next(-100, 100), 0) - new Vector2(Projectile.Center.X, 0)).SafeNormalize(Vector2.Zero);
			Projectile.velocity += vel;
			if (++Projectile.ai[0] >= 30) {
				BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY * 15, ProjectileID.WoodenArrowHostile, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
				Projectile.ai[0] = 0;
			}
		}
	}
	public abstract class BaseHostileGemStaff : BaseHostileProjectile {
		public int ProjectileType = ProjectileID.AmethystBolt;
		public override void SetHostileDefaults() {
			Projectile.width = 40;
			Projectile.height = 42;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			CanDealContactDamage = false;
		}
	}
	class GemStaffAttackOne : BaseHostileGemStaff {
		public override void AI() {
			if (Projectile.timeLeft > 180)
				Projectile.timeLeft = 180;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.ToRadians(Projectile.ai[1] - 70);
			if (++Projectile.ai[0] >= 35) {
				BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 10f, ProjectileType, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
				Projectile.ai[0] = 0;
			}
			Projectile.ai[1] += 2;
		}
	}
	class GemStaffAttackTwo : BaseHostileGemStaff {
		//Projectile.ai[2] act as projectile index
		public override void AI() {
			if (++Projectile.ai[1] <= 60) {
				Projectile.ai[0] = Projectile.ai[2] * 90 / 6;
				Projectile.velocity *= .985f;
				Projectile.rotation = MathHelper.ToRadians(Projectile.ai[1] * 10);
				return;
			}
			if (IsNPCActive(out NPC npc)) {
				npc.TargetClosest();
				Player player = Main.player[npc.target];
				if (!player.active || player.dead) {
					Projectile.velocity = Vector2.Zero;
					return;
				}
				Vector2 pos = npc.Center + Vector2.One.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllGemStaffPHM.Length + 1, 360, Projectile.ai[2]).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * .5f)) * 460;
				Projectile.velocity = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * (pos - Projectile.Center).Length() / 32f;
				Vector2 specializePlayerVelocity = player.velocity;
				specializePlayerVelocity.X *= 32;
				specializePlayerVelocity.Y *= 8;
				float rotateToPlayer = (player.Center + specializePlayerVelocity - Projectile.Center).ToRotation();
				Projectile.rotation = rotateToPlayer + MathHelper.PiOver4;
				if (Projectile.timeLeft > 300)
					Projectile.timeLeft = 300;
				if (++Projectile.ai[0] >= 90) {
					BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 6, ProjectileType, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
					Projectile.ai[0] = 0;
				}
			}
		}
	}
	public abstract class BaseHostileSpecialBow : ModProjectile {

	}
	public abstract class BaseHostileGun : BaseHostileProjectile {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void PreDrawDraw(Texture2D texture, Vector2 drawPos, Vector2 origin, ref Color lightColor, out bool DrawOrigin) {
			SpriteEffects effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, effect, 0);
			DrawOrigin = false;
		}
	}
	public class HostileMinishark : BaseHostileGun {
		public override void SetHostileDefaults() {
			Projectile.width = 54;
			Projectile.height = 20;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			CanDealContactDamage = false;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (Projectile.ai[2] == 0) {
				Projectile.ai[2] = -1;
			}
			Vector2 AbovePlayer = player.Center + new Vector2(-200 * Projectile.ai[2] + Main.rand.NextFloat(-50, 50), -450 + Main.rand.Next(-25, 25));
			Vector2 TowardPlayer = Vector2.UnitY;
			Projectile.velocity = (AbovePlayer - Projectile.Center).SafeNormalize(Vector2.Zero) * (AbovePlayer - Projectile.Center).Length() / 32f;
			Projectile.rotation = TowardPlayer.ToRotation();
			if (++Projectile.ai[1] <= 50) {
				return;
			}
			if (++Projectile.ai[0] >= 8) {
				Projectile.ai[0] = 0;
				TowardPlayer = TowardPlayer.Vector2RotateByRandom(15);
				BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer * Main.rand.NextFloat(7, 11), ProjectileID.Bullet, Projectile.damage / 3, 1, AdjustHostileProjectileDamage: false);
			}
		}
	}
	public class HostileMusket : BaseHostileGun {
		public override void SetHostileDefaults() {
			Projectile.width = 56;
			Projectile.height = 18;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			CanDealContactDamage = false;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (Projectile.ai[2] == 0) {
				Projectile.ai[2] = 1;
			}
			Vector2 BesidePlayer = player.Center + new Vector2(Main.rand.Next(-50, 50) - 600 * Projectile.ai[2], Main.rand.Next(-10, 10));
			Vector2 TowardPlayer = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			Projectile.velocity = (BesidePlayer - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
			Projectile.velocity = Projectile.velocity.LimitedVelocity((BesidePlayer - Projectile.Center).Length() * .05f);
			if (++Projectile.ai[0] >= 40) {
				SoundEngine.PlaySound(SoundID.Item38 with {
					Pitch = 1f
				}, Projectile.Center);
				Projectile.ai[0] = 0;
				TowardPlayer = TowardPlayer.Vector2RotateByRandom(2);
				BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer * 15f, ProjectileID.Bullet, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
				for (int i = 0; i < 30; i++) {
					int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(TowardPlayer, 10), 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(7f, 19f);
					Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
				}
			}
			Projectile.rotation = TowardPlayer.ToRotation();
			Projectile.spriteDirection = (int)Projectile.ai[2];
		}
	}
}
