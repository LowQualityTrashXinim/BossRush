﻿using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.Audio;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Contents.NPCs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Deagle;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.IceStorm;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HorusEye;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HeavenSmg;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.BurningPassion;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.SuperShortSword;
using BossRush.Contents.Items.Weapon.MagicSynergyWeapon.StarLightDistributer;
using Steamworks;
using BossRush.Common.RoguelikeChange.Mechanic;

namespace BossRush.Contents.Items.Weapon {
	/// <summary>
	///This mod player should hold all the logic require for the item, if the item is shooting out the projectile, it should be doing that itself !<br/>
	///Same with projectile unless it is a vanilla projectile then we can refer to global projectile<br/>
	///This should only hold custom bool or data that we think should be hold/use/transfer<br/>
	///We will name using the following format "Synergy item"_"vanilla item" to assign synergy power so that it is clear to read and easy to maintain<br/>
	///If a ability that require modplayer class, you can create your own custom player class with the item name, it should also follow a format which goes  "Synergy item"_ModPlayer
	///This is class is only purpose is to serve as a central class where it contain bool and data and potential synergy manipulation
	/// </summary>
	public class PlayerSynergyItemHandle : ModPlayer {
		public bool SynergyBonusBlock = false;
		public int SynergyBonus = 0;

		public bool BurningPassion_WandofFrosting = false;
		public int BurningPassion_Cooldown = 0;

		public bool DarkCactus_BatScepter = false;
		public bool DarkCactus_BladeOfGrass = false;

		public bool EnchantedOreSword_StarFury = false;
		public bool EnchantedOreSword_EnchantedSword = false;

		public bool EnchantedStarfury_SkyFacture = false;
		public bool EnchantedStarfury_BreakerBlade = false;

		public bool IceStorm_SnowBallCannon = false;
		public bool IceStorm_FlowerofFrost = false;
		public bool IceStorm_BlizzardStaff = false;
		public float IceStorm_SpeedMultiplier = 1;

		public bool EnergyBlade_Code1 = false;
		public bool EnergyBlade_Code2 = false;
		public int EnergyBlade_Code1_Energy = 0;

		public bool Swotaff_Spear = false;
		public int Swotaff_Spear_Counter = 0;

		public bool AmberBoneSpear_MandibleBlade = false;

		public bool Deagle_PhoenixBlaster = false;
		public bool Deagle_DaedalusStormBow = false;
		public int Deagle_DaedalusStormBow_coolDown = 0;
		public bool Deagle_PhoenixBlaster_Critical = false;

		public bool OvergrownMinishark_CrimsonRod = false;
		public bool OvergrownMinishark_DD2ExplosiveTrapT1Popper = false;

		public bool StreetLamp_Firecracker = false;
		public bool StreetLamp_VampireFrogStaff = false;
		public int StreetLamp_VampireFrogStaff_HitCounter = 0;

		public bool OrbOfEnergy_BookOfSkulls = false;
		public bool OrbOfEnergy_DD2LightningAuraT1Popper = false;

		public bool SinisterBook_DemonScythe = false;
		public int SinisterBook_DemonScythe_Counter = 0;

		public bool StarLightDistributer_MeteorArmor = false;
		public bool StarLightDistributer_MagicMissile = false;
		public bool StarlightDistributer_StarCannon = false;

		public bool BloodyShoot_AquaScepter = false;

		public bool RectangleShotgun_QuadBarrelShotgun = false;

		public bool SharpBoomerang_EnchantedBoomerang = false;

		public bool SuperFlareGun_Phaseblade = false;

		public bool QuadDemonBlaster = false;
		public float QuadDemonBlaster_SpeedMultiplier = 1;

		public bool MagicHandCannon_Flamelash = false;

		public bool ZapSnapper_WeatherPain = false;
		public bool ZapSnapper_ThunderStaff = false;

		public bool MagicGrenade_MagicMissle = false;

		public bool DeathBySpark_AleThrowingGlove = false;

		public int SuperShortSword_Counter = 0;
		public int SuperShortSword_AttackType = 0;
		public int SuperShortSword_Delay = 0;
		public int SuperShortSword_ProjectileInReadyPosition = 0;
		public bool SuperShortSword_IsHoldingDownRightMouse = false;

		public int HeavenSmg_Stacks = 0;
		public override void ResetEffects() {
			SynergyBonusBlock = false;
			SynergyBonus = 0;

			BurningPassion_WandofFrosting = false;

			DarkCactus_BatScepter = false;
			DarkCactus_BladeOfGrass = false;

			EnchantedOreSword_StarFury = false;
			EnchantedOreSword_EnchantedSword = false;

			EnchantedStarfury_SkyFacture = false;
			EnchantedStarfury_BreakerBlade = false;

			IceStorm_SnowBallCannon = false;
			IceStorm_FlowerofFrost = false;
			IceStorm_BlizzardStaff = false;

			EnergyBlade_Code1 = false;
			EnergyBlade_Code2 = false;

			Swotaff_Spear = false;

			AmberBoneSpear_MandibleBlade = false;

			Deagle_PhoenixBlaster = false;
			Deagle_DaedalusStormBow = false;

			OvergrownMinishark_CrimsonRod = false;
			OvergrownMinishark_DD2ExplosiveTrapT1Popper = false;

			StarLightDistributer_MeteorArmor = false;

			StreetLamp_Firecracker = false;
			StreetLamp_VampireFrogStaff = false;

			OrbOfEnergy_BookOfSkulls = false;
			OrbOfEnergy_DD2LightningAuraT1Popper = false;

			SinisterBook_DemonScythe = false;

			StarLightDistributer_MagicMissile = false;
			StarlightDistributer_StarCannon = false;

			BloodyShoot_AquaScepter = false;

			RectangleShotgun_QuadBarrelShotgun = false;

			SharpBoomerang_EnchantedBoomerang = false;

			SuperFlareGun_Phaseblade = false;

			MagicHandCannon_Flamelash = false;

			ZapSnapper_WeatherPain = false;
			ZapSnapper_ThunderStaff = false;

			MagicGrenade_MagicMissle = false;

			DeathBySpark_AleThrowingGlove = false;
		}
		int check = 1;
		public override void PreUpdate() {
			Item item = Player.HeldItem;
			SuperShortSwordUpdate(item);
		}
		private void SuperShortSwordUpdate(Item item) {
			if (item.type == ModContent.ItemType<SuperShortSword>()) {
				SuperShortSword_Delay = BossRushUtils.CountDown(SuperShortSword_Delay);
				if (Main.mouseLeft && SuperShortSword_AttackType == 0 && SuperShortSword_Delay <= 0) {
					SuperShortSword_AttackType = 1;
				}
				if (SuperShortSword_ProjectileInReadyPosition >= 8 && SuperShortSword_AttackType == 1) {
					SuperShortSword_ProjectileInReadyPosition = 0;
					SuperShortSword_AttackType = 0;
					SuperShortSword_Delay = 10;
				}

				if (Main.mouseRight && SuperShortSword_AttackType == 0 && SuperShortSword_Delay <= 0) {
					SuperShortSword_AttackType = 2;
					SuperShortSword_IsHoldingDownRightMouse = true;
				}
				if (SuperShortSword_AttackType == 2) {
					if (SuperShortSword_IsHoldingDownRightMouse) {
						if (Main.mouseRightRelease && !Main.mouseRight)
							SuperShortSword_IsHoldingDownRightMouse = false;
					}
					if (!SuperShortSword_IsHoldingDownRightMouse && SuperShortSword_ProjectileInReadyPosition >= 8 && SuperShortSword_AttackType == 2) {
						SuperShortSword_ProjectileInReadyPosition = 0;
						SuperShortSword_AttackType = 0;
						SuperShortSword_Delay = 10;
					}
				}
				if (SuperShortSword_AttackType != 0) {
					return;
				}
				if (SuperShortSword_Counter == MathHelper.TwoPi * 100 || SuperShortSword_Counter == -MathHelper.TwoPi * 100) {
					SuperShortSword_Counter = 0;
				}
				SuperShortSword_Counter += Player.direction;
			}
			else {
				SuperShortSword_AttackType = 0;
				SuperShortSword_Delay = 10;
				SuperShortSword_Counter = 0;
				SuperShortSword_ProjectileInReadyPosition = 0;
			}
		}
		public override void PostUpdate() {
			Item item = Player.HeldItem;
			if (item.type == ModContent.ItemType<BurningPassion>()) {
				if (!Player.ItemAnimationActive && check == 0) {
					Player.velocity *= .25f;
					check++;
				}
				else if (Player.ItemAnimationActive && Main.mouseRight) {
					Player.gravity = 0;
					Player.velocity.Y -= 0.3f;
					Player.ignoreWater = true;
					check = 0;
				}
			}
			if (item.type == ModContent.ItemType<Deagle>()) {
				if (Deagle_DaedalusStormBow) {
					Deagle_DaedalusStormBow_coolDown = BossRushUtils.CountDown(Deagle_DaedalusStormBow_coolDown);
				}
			}
			if (item.type != ModContent.ItemType<IceStorm>()) {
				IceStorm_SpeedMultiplier = 1;
			}
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Swotaff_Spear && Player.altFunctionUse != 2) {
				if (Swotaff_Spear_Counter < 2) {
					Swotaff_Spear_Counter++;
				}
				else {
					Vector2 cirRanPos = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(position, 30, 90, velocity.ToRotation());
					Vector2 vel = (Main.MouseWorld - cirRanPos).SafeNormalize(Vector2.Zero) * 10;
					Projectile.NewProjectile(source, cirRanPos, vel, type, damage, knockback, Player.whoAmI, 2);
					Swotaff_Spear_Counter = 0;
				}
			}
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			if (Player.HeldItem.type == ModContent.ItemType<HeavenSmg>()) {
				ModPlayer_resetStacks();
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (hit.Crit) {
				if (Deagle_PhoenixBlaster) {
					Deagle_PhoenixBlaster_Critical = true;
				}
				if (Deagle_DaedalusStormBow && Deagle_DaedalusStormBow_coolDown <= 0) {
					for (int i = 0; i < 15; i++) {
						Vector2 positionAboveSky = target.Center + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-1100, -1000));
						int projectile = Projectile.NewProjectile(
							Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.ammo),
							positionAboveSky,
							(target.Center - positionAboveSky).SafeNormalize(Vector2.Zero) * 20f,
							ProjectileID.BulletHighVelocity,
							hit.Damage,
							0,
							Player.whoAmI);
						Main.projectile[projectile].usesLocalNPCImmunity = true;
					}
					Deagle_DaedalusStormBow_coolDown = 600;
				}
			}
			if (Player.HeldItem.type == ModContent.ItemType<HeavenSmg>()) {
				ModPlayer_resetStacks();
			}
		}
		public void IncreaseStack() {
			for (int i = 0; i < 5; i++) {
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.One.Vector2DistributeEvenly(5f, 360, i), ModContent.ProjectileType<HeavenBolt>(), 30, 0, Player.whoAmI, 1);
			}
			if (HeavenSmg_Stacks >= 40) {
				SoundEngine.PlaySound(SoundID.Item9 with { Pitch = -2f }, Player.Center);
				return;
			}
			HeavenSmg_Stacks++;
			SoundEngine.PlaySound(SoundID.NPCHit5 with { Pitch = HeavenSmg_Stacks * 0.075f }, Player.Center);
		}
		public void ModPlayer_resetStacks() {
			if (Player.HeldItem.type == ModContent.ItemType<HeavenSmg>()) {
				SoundEngine.PlaySound(SoundID.NPCDeath7, Player.Center);
			}
			HeavenSmg_Stacks = 0;
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			damage += SynergyBonus * .5f;
			if (item.type == ModContent.ItemType<HeavenSmg>()) {
				damage += HeavenSmg_Stacks * 0.05f;
			}
		}
		public override float UseSpeedMultiplier(Item item) {
			float multiplier = base.UseAnimationMultiplier(item);
			if (item.type == ModContent.ItemType<HeavenSmg>()) {
				return multiplier + HeavenSmg_Stacks * 0.01f;
			}
			return base.UseSpeedMultiplier(item);
		}
		public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable) {
			if (Player.ItemAnimationActive && Player.HeldItem.ModItem is BurningPassion && Player.ownedProjectileCounts[ModContent.ProjectileType<BurningPassionP>()] > 0) {
				return true;
			}
			return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
		}

		public override void UpdateEquips() {
			if (Player.head == ArmorIDs.Head.MeteorHelmet && Player.body == ArmorIDs.Body.MeteorSuit && Player.legs == ArmorIDs.Legs.MeteorLeggings) {
				StarLightDistributer_MeteorArmor = true;
			}
		}
		public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
			if (StarLightDistributer_MeteorArmor && item.ModItem is StarLightDistributer) {
				mult = 0f;
			}
		}
		public override void PostHurt(Player.HurtInfo info) {
			float Modify = IceStorm_SpeedMultiplier <= 3 ? 1f : IceStorm_SpeedMultiplier - 2f;
			IceStorm_SpeedMultiplier = Modify;
			base.PostHurt(info);
		}
	}
	public abstract class SynergyModItem : ModItem {
		public override void SetStaticDefaults() {
			ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<SynergyEnergy>();
			CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(50, 100, 100) });
		}
		ColorInfo CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(100, 150, 150) });
		public override sealed void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			ModifySynergyToolTips(ref tooltips, Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>());
			if (CustomColor != null) {
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = CustomColor.MultiColor(5);
			}
		}
		public virtual void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) { }
		public override sealed void HoldItem(Player player) {
			base.HoldItem(player);
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (modplayer.SynergyBonusBlock) {
				return;
			}
			HoldSynergyItem(player, modplayer);
		}
		public override sealed void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
			ModifySynergyShootStats(player, player.GetModPlayer<PlayerSynergyItemHandle>(), ref position, ref velocity, ref type, ref damage, ref knockback);
		}
		public override sealed void UpdateInventory(Player player) {
			base.UpdateInventory(player);
			SynergyUpdateInventory(player, player.GetModPlayer<PlayerSynergyItemHandle>());
		}
		public virtual void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {

		}
		public virtual void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

		}
		/// <summary>
		/// You should use this to set condition, the condition must be pre set in <see cref="PlayerSynergyItemHandle"/> and then check condition in here
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			SynergyShoot(player, player.GetModPlayer<PlayerSynergyItemHandle>(), source, position, velocity, type, damage, knockback, out bool CanShootItem);
			return CanShootItem;
		}
		public virtual void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) { CanShootItem = true; }
		public override sealed void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			base.OnHitNPC(player, target, hit, damageDone);
			OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
		}
		public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) { }

		private int countX = 0;
		private float positionRotateX = 0;
		private void PositionHandle() {
			if (positionRotateX < 3.5f && countX == 1) {
				positionRotateX += .2f;
			}
			else {
				countX = -1;
			}
			if (positionRotateX > 0 && countX == -1) {
				positionRotateX -= .2f;
			}
			else {
				countX = 1;
			}
		}
		Color auraColor;
		private void ColorHandle() {
			switch (Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus) {
				case 1:
					auraColor = new Color(255, 50, 0, 30);
					break;
				case 2:
					auraColor = new Color(255, 255, 0, 30);
					break;
				case 3:
					auraColor = new Color(0, 255, 255, 30);
					break;
				default:
					auraColor = new Color(255, 255, 255, 30);
					break;
			}
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			ColorHandle();
			if (ItemID.Sets.AnimatesAsSoul[Type] || Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus < 1) {
				return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
			}
			Main.instance.LoadItem(Item.type);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(1.5f, 1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(1.5f, -1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, 1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, -1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
	public abstract class SynergyModProjectile : ModProjectile {
		public virtual void SpawnDustPostPreAI(Player player) { }
		public virtual void SpawnDustPostAI(Player player) { }
		public virtual void SpawnDustPostPostAI(Player player) { }
		public override sealed bool PreAI() {
			Player player = Main.player[Projectile.owner];
			SynergyPreAI(player, player.GetModPlayer<PlayerSynergyItemHandle>(), out bool stopAI);
			SpawnDustPostPreAI(player);
			return stopAI;
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		/// <param name="runAI"></param>
		public virtual void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) { runAI = true; }
		public override sealed void AI() {
			Player player = Main.player[Projectile.owner];
			SynergyAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
			SpawnDustPostAI(player);
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed void PostAI() {
			Player player = Main.player[Projectile.owner];
			SynergyPostAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
			SpawnDustPostPostAI(player);
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void SynergyPostAI(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			Player player = Main.player[Projectile.owner];
			ModifyHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, ref modifiers);
		}
		public virtual void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) { }
		public override sealed void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Player player = Main.player[Projectile.owner];
			OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
		}
		public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) { }
		public override sealed void OnKill(int timeLeft) {
			base.OnKill(timeLeft);
			Player player = Main.player[Projectile.owner];
			SynergyKill(player, player.GetModPlayer<PlayerSynergyItemHandle>(), timeLeft);
		}
		public virtual void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		}
	}
	public abstract class SynergyBuff : ModBuff {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override sealed void SetStaticDefaults() {
			base.SetStaticDefaults();
			SynergySetStaticDefaults();
		}
		public virtual void SynergySetStaticDefaults() {

		}
		public override sealed void Update(Player player, ref int buffIndex) {
			base.Update(player, ref buffIndex);
			UpdatePlayer(player, ref buffIndex);
		}
		public virtual void UpdatePlayer(Player player, ref int buffIndex) {

		}
		public override sealed void Update(NPC npc, ref int buffIndex) {
			base.Update(npc, ref buffIndex);
			UpdateNPC(npc, ref buffIndex);
		}
		public virtual void UpdateNPC(NPC npc, ref int buffIndex) {

		}
	}
	public class SynergyModSystem : ModSystem {
		public bool GodAreEnraged = false;
		public int CooldownCheck = 999;
		private void SynergyEnergyCheckPlayer(Player player) {
			int synergyCounter = 0;
			synergyCounter += player.CountItem(ModContent.ItemType<SynergyEnergy>(), 2);
			synergyCounter += player.inventory.Where(itemInv => itemInv.ModItem is SynergyModItem).Count();
			int maxCount = NPC.GetActivePlayerCount() + 1;
			if (synergyCounter >= maxCount) {
				GodAreEnraged = true;
			}
		}
		private void GodDecision(Player player) {
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			if (NPC.AnyNPCs(ModContent.NPCType<Guardian>()) || player.GetModPlayer<ChestLootDropPlayer>().CanDropSynergyEnergy)
				return;
			if (player.IsDebugPlayer())
				return;
			CooldownCheck = BossRushUtils.CountDown(CooldownCheck);
			//Main.NewText(CooldownCheck);
			if (CooldownCheck <= 0) {
				SynergyEnergyCheckPlayer(player);
			}
			if (GodAreEnraged) {
				Vector2 randomSpamLocation = Main.rand.NextVector2CircularEdge(1500, 1500) + player.Center;
				NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)randomSpamLocation.X, (int)randomSpamLocation.Y, ModContent.NPCType<Guardian>());
				BossRushUtils.CombatTextRevamp(player.Hitbox, Color.Red, "You have anger the God!");
				CooldownCheck = 999;
				GodAreEnraged = false;
			}
		}
		public override void PostUpdateWorld() {
			GodDecision(Main.LocalPlayer);
		}
	}
}
