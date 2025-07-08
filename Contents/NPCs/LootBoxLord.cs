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
using BossRush.Common.Global;

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
			for (int i = 0; i < TerrariaArrayID.MeleePreBoss.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleePreBoss[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.MeleePreEoC.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleePreEoC[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.MeleeEvilBoss.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MeleeEvilBoss[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.RangePreBoss.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangePreBoss[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.RangePreEoC.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangePreEoC[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.RangeSkele.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.RangeSkele[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.MagicPreBoss.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicPreBoss[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.MagicPreEoC.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicPreEoC[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.MagicSkele.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.MagicSkele[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.SummonPreBoss.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonPreBoss[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.SummonerPreEoC.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonerPreEoC[i], 10));
			}
			for (int i = 0; i < TerrariaArrayID.SummonSkele.Length; i++) {
				npcLoot.Add(ItemDropRule.Common(TerrariaArrayID.SummonSkele[i], 10));
			}
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PowerEnergy>()));
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
			NPC.lifeMax = (int)(54000);
			NPC.life = NPC.lifeMax;
			NPC.damage = (int)(150);
			NPC.defense = (int)(50);
		}
		public int BossDamagePercentage(float percentage) => (int)Math.Ceiling(NPC.damage * percentage);
		//Use NPC.ai[0] to delay attack
		//Use NPC.ai[1] to switch attack
		//Use NPC.ai[2] for calculation
		//Use NPC.ai[3] to do movement
		bool AlreadySaidThat = false;
		int dialogNumber = 0;
		int dialogCD = 0;
		bool BeforeAttack = true;
		/// <summary>
		/// This is so that the NPC can fly toward player
		/// </summary>
		bool CanTrackPlayer = false;
		public void Dialog() {
			if (--dialogCD > 0) {
				return;
			}
			string dialog = "";
			Color color = Color.White;
			switch (dialogNumber) {
				case 0:
					dialog = "You summon me ?";
					break;
				case 1:
					dialog = "... I see";
					break;
				case 2:
					dialog = "I'm just a mere toy in this play";
					break;
				case 3:
					dialog = "I will make you regret";
					color = Color.Red;
					BeforeAttack = false;
					NPC.dontTakeDamage = false;
					ResetEverything(120);
					break;
			}
			dialogCD = 120;
			dialogNumber++;
			BossRushUtils.CombatTextRevamp(NPC.Hitbox, color, dialog);
		}

		public override void AI() {
			if (BeforeAttack) {
				Dialog();
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
					BossRushUtils.CombatTextRevamp(NPC.Hitbox, Color.Red, "You think so lowly of me ...");
					AlreadySaidThat = true;
					if (!NPC.AnyNPCs(ModContent.NPCType<ElderGuardian>()))
						NPC.NewNPC(NPC.GetSource_FromAI(), NPC.Hitbox.X, NPC.Hitbox.Y, ModContent.NPCType<ElderGuardian>());
				}
			}
			//TODO : change phase when boss hp is below 50%
			//Move above the player
			switch (CurrentAttack) {
				case 0:
					Move(player);
					break;
				case 1:
					ShootShortSword();
					CanTrackPlayer = false;
					break;
				case 2:
					CanTrackPlayer = true;
					ShootShortSword2(player);
					break;
				case 3:
					ShootBroadSword(player);
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
		Vector2 offsetPos = Vector2.Zero;
		private void Move(Player player) {
			if (BossDelayAttack(0, 1, 0)) {
				return;
			}
			CanTrackPlayer = false;
			Vector2 positionAbovePlayer = new Vector2(player.Center.X, player.Center.Y - 200) + offsetPos;
			if (NPC.NPCMoveToPosition(positionAbovePlayer, 30f)) {
				AttackCounter++;
			}
		}
		public float AttackTimer { get => NPC.ai[0]; set => NPC.ai[0] = value; }
		public float CurrentAttack { get => NPC.ai[1]; set => NPC.ai[1] = value; }
		public float AttackCounter { get => NPC.ai[2]; set => NPC.ai[2] = value; }
		private void ResetEverything(int delayAttack = 90) {
			lastPlayerPosition = Main.player[NPC.target].Center;
			HasReachPos = false;
			AttackTimer = delayAttack;
			CurrentAttack = 0;
			AttackCounter = 0;
			offsetPos = Vector2.Zero;
			NPC.velocity = Vector2.Zero;
		}
		private void ShootShortSword() {
			if (BossDelayAttack(10, 2, TerrariaArrayID.AllOreShortSword.Length - 1, 30)) {
				return;
			}
			Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)NPC.ai[2]) * 15f;
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackOne>(),
				BossDamagePercentage(.75f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
			AttackCounter++;
		}
		private void ShootShortSword2(Player player) {
			//Custom boss movement oooohhh
			if (AttackCounter >= TerrariaArrayID.AllOreShortSword.Length - 1) {
				CanTrackPlayer = false;
				Vector2 distance = (player.Center - Vector2.UnitY * 200) - NPC.Center;
				if (distance.LengthSquared() < 50 * 50 || AttackCounter > TerrariaArrayID.AllOreShortSword.Length) {
					NPC.velocity *= .8f;
					AttackCounter++;
					if (!NPC.velocity.IsLimitReached(.1f)) {
						CurrentAttack++;
						AttackCounter = 0;
					}
				}
				else {
					NPC.velocity = distance.SafeNormalize(Vector2.Zero) * distance.Length() / 8f;
				}
				return;
			}
			if (++AttackTimer < 20) {
				return;
			}
			AttackTimer = 0;
			Vector2 vec = Vector2.UnitX * 20 * Main.rand.NextBool(2).ToDirectionInt();
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackTwo>(), BossDamagePercentage(.75f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)AttackCounter];
			Main.projectile[proj].ai[1] = -20;
			Main.projectile[proj].ai[0] = 2;
			Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
			AttackCounter++;
		}
		private void ShootBroadSword(Player player) {
			//ahh movment, after the attack counter reach
			if (AttackCounter >= TerrariaArrayID.AllOreBroadSword.Length - 1) {
				Vector2 distance = player.Center.Add(0, 200) - NPC.Center;
				if (distance.LengthSquared() < 150 * 150 || AttackCounter > TerrariaArrayID.AllOreBroadSword.Length) {
					NPC.velocity *= .8f;
					AttackCounter++;
					if (!NPC.velocity.IsLimitReached(.1f)) {
						AttackCounter = 0;
					}
				}
				else {
					NPC.velocity = distance.SafeNormalize(Vector2.Zero) * distance.Length() / 8f;
				}
				return;
			}//Before attack counter reach
			else {
				if (AttackTimer == 0 && AttackCounter == 0) {
					lastPlayerPosition = player.Center;
					Vector2 distance2 = player.Center.Add(0, 300) - NPC.Center;
					if (distance2.LengthSquared() > 100 * 100) {
						NPC.velocity = distance2.SafeNormalize(Vector2.Zero) * distance2.Length() / 4f;
						return;
					}
				}
				Vector2 distance = lastPlayerPosition - NPC.Center;
				NPC.velocity = distance.SafeNormalize(Vector2.Zero) * 15;
				NPC.velocity = NPC.velocity.RotatedBy(MathHelper.ToRadians(90));
			}
			if (++AttackTimer < 10) {
				return;
			}
			AttackTimer = 0;
			Vector2 vec = (NPC.Center - player.Center).SafeNormalize(Vector2.Zero) * 10;
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackOne>(), BossDamagePercentage(.85f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is SwordBroadAttackOne swordProj) {
				swordProj.SetNPCOwner(NPC.whoAmI);
				swordProj.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[(int)AttackCounter];
			}
			AttackCounter++;
		}
		private void ShootBroadSword2() {
			NPC.ai[0] = 0;
			if (BossDelayAttack(0, 5, 0)) {
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
			NPC.velocity = (Vector2.One * 15).RotatedBy(MathHelper.ToRadians(30));
			NPC.rotation = NPC.velocity.ToRotation();
			if (BossDelayAttack(30, 5, TerrariaArrayID.AllWoodBowPHM.Length - 1)) {
				return;
			}
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity.RotatedBy(-MathHelper.PiOver2), ModContent.ProjectileType<WoodBowAttackOne>(), BossDamagePercentage(.65f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[(int)NPC.ai[2]];
			Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation();
			NPC.ai[2]++;

		}
		private void ShootWoodBow2() {
			if (BossDelayAttack(5, 6, 0, 150)) {
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
			if (BossDelayAttack(5, 7, TerrariaArrayID.AllGemStaffPHM.Length - 1, 120)) {
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
			if (BossDelayAttack(120, 8, 0, 200)) {
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
			if (BossDelayAttack(20, 9, TerrariaArrayID.AllOreBowPHM.Length - 1, 120)) {
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
			if (BossDelayAttack(10, 10, TerrariaArrayID.AllOreBowPHM.Length - 1, 120)) {
				return;
			}
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<OreBowAttackTwo>(), BossDamagePercentage(.55f), 2, NPC.target);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBowPHM[(int)NPC.ai[2]];
			NPC.ai[2]++;
		}
		int gunCounter = 0;
		private void ShootGun(Player player) {
			if (++gunCounter >= 30) {
				gunCounter = 0;
				int boomStick = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, (NPC.Center - player.Center).SafeNormalize(Vector2.Zero) * 20, ModContent.ProjectileType<HostileBoomStick>(), NPC.damage, 2, NPC.target);
				if (Main.projectile[boomStick].ModProjectile is BaseHostileGun hostileGun) {
					hostileGun.ItemIDtextureValue = ItemID.Boomstick;
					hostileGun.SetNPCOwner(NPC.whoAmI);
				}
			}
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
				minisharkproj.SetNPCOwner(NPC.whoAmI);
			}
			int Musket = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HostileMusket>(), NPC.damage, 2, NPC.target);
			if (Main.projectile[Musket].ModProjectile is BaseHostileGun musketproj) {
				musketproj.ItemIDtextureValue = ItemID.Musket;
				Main.projectile[Musket].ai[2] = direction;
				musketproj.SetNPCOwner(NPC.whoAmI);
			}
			CanTrackPlayer = false;
			NPC.ai[2]++;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Main.instance.LoadNPC(NPC.type);
			if (NPC.velocity == Vector2.Zero) {
				return base.PreDraw(spriteBatch, screenPos, drawColor);
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
		/// This is to ensure boss have a certain delay<br/>
		/// <paramref name="nextattack"/> will only run when delay reached 0 or below 0 
		/// </summary>
		/// <param name="delaytime">the delay between each attack, use if you want to shoot out projectile individually or have a space out</param>
		/// <param name="nextattack">Will set the next attack</param>
		/// <param name="whenAttackwillend">determined whenever the attack will end</param>
		/// <param name="additionalDelay">the post delay after the attack is done</param>
		/// <returns></returns>
		private bool BossDelayAttack(float delaytime, float nextattack, float whenAttackwillend, int additionalDelay = 0) {
			//This only run whenever a delay is given but only when the timer reach 0 or below
			if (AttackTimer <= 0) {
				AttackTimer += delaytime;
			}
			else {
				//The timer decrease
				AttackTimer--;
				return true;
			}
			//this will check if the counter (NPC.ai[2]) reach the requirement to reset everything
			if (AttackCounter > whenAttackwillend) {
				ResetEverything();
				AttackTimer += additionalDelay;
				CurrentAttack = nextattack;
				return true;
			}
			return false;
		}
	}
	//This code did not follow the above rule and it should be change to follow the above rule
	public abstract class BaseHostileProjectile : ModProjectile {
		public sealed override void SetDefaults() {
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
			if (NPC_WhoAmI < 0 || NPC_WhoAmI > 255) {
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
		public sealed override void OnKill(int timeLeft) {
			for (int i = 0; i < 10; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke);
				dust.noGravity = true;
				dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.75f, 1.25f);
				dust.scale = Main.rand.NextFloat(2, 3.5f);
			}
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
			else {
				Projectile.Kill();
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
			Vector2 vel = (new Vector2(player.Center.X + Main.rand.Next(-100, 100), 0) - new Vector2(Projectile.Center.X, player.Center.Y - 500)).SafeNormalize(Vector2.Zero);
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
			if (IsNPCActive(out NPC npc)) {
				npc.TargetClosest();
				if (Projectile.ai[2] == 0) {
					Projectile.ai[2] = Main.rand.NextBool().ToDirectionInt();
				}
				Player player = Main.player[npc.target];
				float rotation = MathHelper.ToRadians(Projectile.timeLeft * 3 * Projectile.ai[2]);
				Vector2 TowardPlayer = rotation.ToRotationVector2();
				Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (player.Center - Projectile.Center).Length() / 128f;
				Projectile.rotation = rotation;
				if (++Projectile.ai[1] <= 40) {
					return;
				}
				if (++Projectile.ai[0] >= 8) {
					Projectile.ai[0] = 0;
					BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer.Vector2RotateByRandom(7) * Main.rand.NextFloat(7, 11), ProjectileID.Bullet, Projectile.damage / 3, 1, AdjustHostileProjectileDamage: false);
				}
			}
			else {
				Projectile.Kill();
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
			if (IsNPCActive(out NPC npc)) {
				npc.TargetClosest();
				Player player = Main.player[npc.target];
				if (Projectile.ai[2] == 0) {
					Projectile.ai[2] = 1;
				}
				Vector2 BesidePlayer = player.Center + new Vector2(Main.rand.Next(-50, 50) - 600 * Projectile.ai[2], Main.rand.Next(-10, 10));
				Vector2 TowardPlayer = (player.Center - Projectile.Center + player.velocity).SafeNormalize(Vector2.Zero);
				Projectile.velocity = (BesidePlayer - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
				Projectile.velocity = Projectile.velocity.LimitedVelocity((BesidePlayer - Projectile.Center).Length() * .05f);
				if (++Projectile.ai[0] >= 40) {
					SoundEngine.PlaySound(SoundID.Item38 with {
						Pitch = 1f
					}, Projectile.Center);
					Projectile.ai[0] = 0;
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
			else {
				Projectile.Kill();
			}
		}
	}
	public class HostileBoomStick : BaseHostileGun {
		public override void SetHostileDefaults() {
			Projectile.width = 56;
			Projectile.height = 18;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
			CanDealContactDamage = false;
		}
		public override void AI() {
			if (IsNPCActive(out NPC npc)) {
				npc.TargetClosest();
				Player player = Main.player[npc.target];
				Projectile.velocity *= .9f;
				Vector2 TowardPlayer = (player.Center - Projectile.Center + player.velocity).SafeNormalize(Vector2.Zero);
				Projectile.rotation = TowardPlayer.ToRotation();
				if (Projectile.timeLeft < 30 && Projectile.ai[0] == 0) {
					Projectile.ai[0]++;
					Projectile.velocity = -TowardPlayer * 10f;
					Projectile.alpha += 255 / 30;
					SoundEngine.PlaySound(SoundID.Item38 with {
						Pitch = 1f
					}, Projectile.Center);
					for (int i = 0; i < 4; i++) {
						BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, TowardPlayer.Vector2RotateByRandom(30) * 12f * Main.rand.NextFloat(.5f, 1f), ProjectileID.Bullet, Projectile.damage, 1, AdjustHostileProjectileDamage: false);
					}
					for (int i = 0; i < 15; i++) {
						int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(TowardPlayer, 10), 0, 0, DustID.Torch);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(4f, 11f);
						Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
					}
				}
			}
			else {
				Projectile.Kill();
			}
		}
		public class BaseHostileSpear : BaseHostileProjectile {
			public override void SetHostileDefaults() {
				Projectile.width = Projectile.height = 16;
			}
		}
	}
}
