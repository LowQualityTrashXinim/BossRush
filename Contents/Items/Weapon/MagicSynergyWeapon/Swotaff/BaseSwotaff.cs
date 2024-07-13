using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff {
	public abstract class SwotaffGemItem : SynergyModItem {
		public virtual void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType) {
			damage = 20;
			ProjectileType = 0;
			ShootType = 0;
		}
		int ProjectileType = 0;
		int ShootType = 0;
		public override void SetDefaults() {
			PreSetDefaults(out int damage, out int projectileType, out int shootType);
			ProjectileType = projectileType;
			ShootType = shootType;
			Item.BossRushDefaultMagic(60, 58, damage, 3f, 20, 20, ItemUseStyleID.Swing, ProjectileType, 7, 10, false);
			Item.crit = 10;
			Item.value = Item.buyPrice(gold: 50);
			Item.useTurn = false;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item8;
			Item.noUseGraphic = true;
		}
		public override float UseSpeedMultiplier(Player player) {
			if (player.altFunctionUse == 2) {
				return .5f;
			}
			return base.UseSpeedMultiplier(player);
		}
		public override bool CanUseItem(Player player) {
			if (player.GetModPlayer<SwotaffPlayer>().Swotaff_Spear_Counter >= 2) {
				return player.ownedProjectileCounts[ProjectileType] < 2;
			}
			return player.ownedProjectileCounts[ProjectileType] < 1;
		}
		public override void OnConsumeMana(Player player, int manaConsumed) {
			if (player.altFunctionUse == 2) {
				player.statMana += manaConsumed;
			}
		}
		public override void OnMissingMana(Player player, int neededMana) {
			if (player.statMana <= player.GetManaCost(Item)) {
				CanShootProjectile = -1;
			}
			player.statMana += neededMana;
		}
		public override bool AltFunctionUse(Player player) {
			CanShootProjectile = -1;
			if (player.GetModPlayer<SwotaffPlayer>().Swotaff_Spear_Counter >= 2) {
				return player.ownedProjectileCounts[ProjectileType] < 2;
			}
			return player.ownedProjectileCounts[ProjectileType] < 1;
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.HasItem(ItemID.Spear)) {
				modplayer.Swotaff_Spear = true;
				modplayer.SynergyBonus++;
			}
		}
		int CanShootProjectile = 1;
		int countIndex = 1;
		int time = 1;
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (player.statMana >= player.GetManaCost(Item) && player.altFunctionUse != 2) {
				CanShootProjectile = 1;
			}
			int projSwing = Projectile.NewProjectile(source, position, Vector2.Zero, type, (int)(damage * 1.25f), knockback * 2f, player.whoAmI, countIndex, CanShootProjectile);
			if (countIndex == 0) {
				Main.projectile[projSwing].knockBack = .5f;
			}
			if (CanShootProjectile == 1) {
				int proj = Projectile.NewProjectile(source, position, velocity, ShootType, damage, knockback, player.whoAmI);
				Main.projectile[proj].usesIDStaticNPCImmunity = true;
			}
			if (player.altFunctionUse != 2) {
				SwingComboHandle();
			}
			CanShootItem = false;
		}
		private void SwingComboHandle() {
			countIndex = countIndex != 0 ? countIndex * -1 : 1;
			time++;
			if (time >= 3) {
				countIndex = 0;
				time = 0;
			}
		}
	}
	/// <summary>
	/// By default, ai2 will contain index of gem
	/// </summary>
	public abstract class SwotaffProjectile : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = 66;
			Projectile.height = 66;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost);
			this.AltAttackAmountProjectile = AltAttackAmountProjectile;
			this.AltAttackProjectileType = AltAttackProjectileType;
			this.NormalBoltProjectile = NormalBoltProjectile;
			this.DustType = DustType;
			this.ManaCost = ManaCost;
		}
		public virtual void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost) {
			AltAttackAmountProjectile = 4;
			AltAttackProjectileType = ProjectileID.WoodenArrowFriendly;
			NormalBoltProjectile = ProjectileID.WoodenArrowFriendly;
			DustType = DustID.ManaRegeneration;
			ManaCost = 0;
		}
		Vector2 PosToGo;
		float AltAttackAmountProjectile;
		int AltAttackProjectileType, 
			NormalBoltProjectile,
			DustType, 
			ManaCost, 
			countdownBeforeReturn = 100, 
			AbsoluteCountDown = 420, 
			timeToSpin = 0, 
			projectileBelongToItem;
		bool isAlreadyHeldDown = false, 
			isAlreadyReleased = false,
			isAlreadySpinState = false, 
			ProjectileAlreadyExist = false;
		public override void OnSpawn(IEntitySource source) {
			base.OnSpawn(source);
			for (int i = 0; i < Main.maxProjectiles; i++) {
				if (Main.projectile[i] is null) {
					continue;
				}
				if (Main.projectile[i].type == AltAttackProjectileType && Main.projectile[i].active) {
					ProjectileAlreadyExist = true;
				}
			}
		}
		public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) {
			runAI = true;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (Projectile.ai[0] == 2) {
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				Projectile.velocity -= Projectile.velocity * .1f;
				if (!Projectile.velocity.IsLimitReached(.1f)) {
					if (Projectile.timeLeft > 20) {
						int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 30f, NormalBoltProjectile, Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
						Main.projectile[proj].extraUpdates = 6;
						Projectile.timeLeft = 20;
						Projectile.velocity += -Projectile.velocity * 40;
					}
					Projectile.ai[0] = 3;
				}
				return;
			}
			if (Projectile.ai[0] == 3) {
				Projectile.alpha = (int)MathHelper.Lerp(0, 255, (20 - Projectile.timeLeft) / 20f);
				return;
			}
			if (player.ItemAnimationJustStarted) {
				if (Projectile.ai[0] == 1 || Projectile.ai[0] == -1 || player.altFunctionUse == 2) {
					PosToGo = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
				}
				else {
					projectileBelongToItem = player.HeldItem.type;
					PosToGo = Main.MouseWorld;
				}
			}
			if (player.altFunctionUse == 2 || isAlreadySpinState) {
				player.heldProj = Projectile.whoAmI;
				SpinAroundPlayer(player);
				isAlreadySpinState = true;
				return;
			}
			if (Projectile.ai[0] == 1 || Projectile.ai[0] == -1) {
				player.heldProj = Projectile.whoAmI;
				BossRushUtils.ProjectileSwordSwingAI(Projectile, player, PosToGo, (int)Projectile.ai[0], 150);
				return;
			}
			SpinAtCursorAI(player);
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			base.OnHitNPCSynergy(player, modplayer, npc, hit, damageDone);
			npc.immune[Projectile.owner] = 8;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			if (Projectile.ai[0] == 0) {
				return;
			}
			BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, Main.player[Projectile.owner], Projectile.width, Projectile.height);
		}
		private void SpinAtCursorAI(Player player) {
			Item item = player.HeldItem;
			Vector2 length = PosToGo - Projectile.Center;
			if (Main.mouseLeft && !isAlreadyHeldDown && !isAlreadyReleased) {
				isAlreadyHeldDown = true;
			}
			if (isAlreadyHeldDown) {
				countdownBeforeReturn = 10;
			}
			if (!Main.mouseLeft && Main.mouseLeftRelease && isAlreadyHeldDown) {
				isAlreadyHeldDown = false;
				isAlreadyReleased = true;
			}
			countdownBeforeReturn -= countdownBeforeReturn > 0 ? 1 : 0;
			AbsoluteCountDown -= AbsoluteCountDown > 0 ? 1 : 0;
			if (countdownBeforeReturn <= 0 || AbsoluteCountDown <= 0 || item.type != projectileBelongToItem) {
				length = player.Center - Projectile.Center;
				float distanceTo = length.Length();
				if (distanceTo < 60) {
					Projectile.Kill();
				}
			}
			Projectile.velocity = (length.SafeNormalize(Vector2.Zero) * length.Length() + player.velocity).LimitedVelocity(20);
			Projectile.rotation += MathHelper.ToRadians(15);
			Vector2 velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * Main.rand.NextFloat(6, 9);
			int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(velocity, 50), 0, 0, DustType);
			Main.dust[dust].scale = Main.rand.NextFloat(.8f, 1.2f);
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
			Main.dust[dust].noGravity = true;
			if (Projectile.ai[1] == 1) {
				if (timeToSpin >= 24) {
					if (player.CheckMana(player.GetManaCost(item), true)) {
						player.manaRegenDelay = player.maxRegenDelay;
					}
					else {
						Projectile.ai[1] = -1;
					}
					timeToSpin = 0;
				}
				timeToSpin++;
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.PositionOFFSET(velocity, 50), velocity, NormalBoltProjectile, (int)(Projectile.damage * .55f), Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].timeLeft = 30;
			}
			if ((Projectile.Center - player.Center).LengthSquared() > 1000 * 1000) {
				Projectile.Kill();
			}
		}
		int amount = 1;
		private void SpinAroundPlayer(Player player) {
			player.direction = PosToGo.X > 0 ? 1 : -1;
			float maxProgress = player.itemAnimationMax;
			if (Projectile.timeLeft > maxProgress) {
				Projectile.timeLeft = (int)maxProgress;
			}
			float percentDone = (maxProgress - Projectile.timeLeft) / maxProgress;
			//percentDone = BossRushUtils.InExpo(percentDone);
			if (player.statMana >= ManaCost && !ProjectileAlreadyExist) {
				if (!isAlreadySpinState) {
					player.statMana = Math.Clamp(player.statMana - ManaCost, 0, player.statManaMax2);
				}
				float percentageToPass = Math.Clamp(1 / (AltAttackAmountProjectile + 1) * amount, 0, 1);
				if (percentDone >= percentageToPass) {
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
						(Projectile.rotation - MathHelper.ToRadians(90)).ToRotationVector2() * 4f, AltAttackProjectileType,
						Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, amount);
					amount++;
				}
			}
			Projectile.spriteDirection = player.direction;
			float baseAngle = PosToGo.ToRotation();
			float start = baseAngle;
			float end = baseAngle - MathHelper.TwoPi * player.direction;
			float currentAngle = MathHelper.SmoothStep(end, start, percentDone);
			Projectile.rotation = currentAngle;
			Projectile.rotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
			Projectile.Center = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * 42;
			player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
			int dustType = DustType;
			int dust = Dust.NewDust(player.Center.PositionOFFSET(Projectile.rotation.ToRotationVector2(), 50), 0, 0, dustType);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 0.1f;
			Main.dust[dust].velocity = Projectile.rotation.ToRotationVector2() * 2f;
			Main.dust[dust].fadeIn = 1.5f;
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		}
	}
	public class SwotaffPlayer : ModPlayer {
		public int Swotaff_Spear_Counter = 0;
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Player.GetModPlayer<PlayerSynergyItemHandle>().Swotaff_Spear && Player.altFunctionUse != 2) {
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
	}
}
