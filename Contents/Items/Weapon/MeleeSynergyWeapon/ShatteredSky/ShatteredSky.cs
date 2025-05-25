using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.ShatteredSky;
public class ShatteredSky : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(94, 92, 100, 10f, 30, 30, ItemUseStyleID.Swing, 1, 1, true);
		MeleeWeaponOverhaul meleeItem = Item.GetGlobalItem<MeleeWeaponOverhaul>();
		Item.UseSound = SoundID.Item1;
		meleeItem.SwingType = BossRushUseStyle.SwipeDown;
		meleeItem.SwingStrength = 11;
		meleeItem.CircleSwingAmount = 2.6f;
		meleeItem.DistanceThrust = 150;
		meleeItem.OffsetThrust = 20;
	}
	int ComboCounter = 0;
	int RealCounter = 0;
	int Timer = 0;
	public override bool CanUseItem(Player player) {
		if (!player.ItemAnimationActive) {
			ComboCounter++;
			MeleeWeaponOverhaul overhaul = Item.GetGlobalItem<MeleeWeaponOverhaul>();
			switch (ComboCounter) {
				case 1:
					overhaul.HideSwingVisual = false;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					overhaul.SwingStrength = 11;
					break;
				case 2:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					overhaul.SwingStrength = 11;
					break;
				case 3:
					overhaul.SwingType = BossRushUseStyle.Spin;
					overhaul.IframeDivision = 2;
					break;
				case 4:
					overhaul.SwingType = BossRushUseStyle.Thrust;
					overhaul.SwingStrength = 15;
					overhaul.HideSwingVisual = true;
					overhaul.IframeDivision = 1;
					break;
			}
			if (ComboCounter >= 4) {
				ComboCounter = 0;
			}
		}
		return base.CanUseItem(player);
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		Timer++;
		if (!player.ItemAnimationActive) {
			if (Timer >= 60 + player.itemAnimationMax) {
				ComboCounter = 0;
				RealCounter = 0;
			}
			return;
		}
		Timer = 0;
		int type = ProjectileID.CultistBossLightningOrbArc;
		Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
		EntitySource_ItemUse source = new EntitySource_ItemUse(player, Item);
		Vector2 position = player.Center;
		int damage = player.GetWeaponDamage(Item);
		float knockback = player.GetWeaponKnockback(Item);
		if (ComboCounter == 3 && player.itemAnimation == player.itemAnimationMax - 1) {
			float offsetLength = Item.Size.Length();
			for (int i = 0; i < 16; i++) {
				Vector2 vel = velocity.Vector2DistributeEvenly(16, 360, i) * 10;
				Projectile projectile = Projectile.NewProjectileDirect(source, position.PositionOFFSET(vel, offsetLength), vel, type, damage / 3, knockback, player.whoAmI, vel.ToRotation(), Main.rand.Next(100));
				projectile.friendly = true;
				projectile.hostile = false;
				projectile.extraUpdates = 3;
				projectile.penetrate = -1;
				projectile.maxPenetrate = -1;
				projectile.usesIDStaticNPCImmunity = true;
				projectile.idStaticNPCHitCooldown = 10;
			}
			SoundEngine.PlaySound(SoundID.Thunder);
		}
		else if (ComboCounter == 0 && player.itemAnimation == player.itemAnimationMax * .4f) {
			if (RealCounter >= 8) {
				RealCounter = 0;
				Projectile projectile = Projectile.NewProjectileDirect(source, position.PositionOFFSET(velocity, Item.Size.Length() * .9f) - Vector2.UnitY * 700, Vector2.Zero, ModContent.ProjectileType<ShatteredSkyProjectileHidden>(), damage, knockback, player.whoAmI, player.direction);
			}
			else {
				Projectile projectile = Projectile.NewProjectileDirect(source, position.PositionOFFSET(velocity, Item.Size.Length() * .9f), velocity * 10, type, damage, knockback, player.whoAmI, velocity.ToRotation(), Main.rand.Next(100));
				projectile.friendly = true;
				projectile.hostile = false;
				projectile.extraUpdates = 5;
				projectile.penetrate = -1;
				projectile.maxPenetrate = -1;
				projectile.usesIDStaticNPCImmunity = true;
				projectile.idStaticNPCHitCooldown = 10;
				projectile.scale = 2;
				SoundEngine.PlaySound(SoundID.Thunder);
			}
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		if (ComboCounter == 1 || ComboCounter == 2) {
			for (int i = 0; i < 3; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(source, position, -velocity.Vector2RotateByRandom(90) * 5, ProjectileID.CultistBossLightningOrbArc, damage / 4, knockback, player.whoAmI, velocity.ToRotation(), Main.rand.Next(100));
				projectile.friendly = true;
				projectile.hostile = false;
				projectile.extraUpdates = 2;
				projectile.penetrate = 5;
				projectile.maxPenetrate = 5;
				projectile.usesIDStaticNPCImmunity = true;
				projectile.idStaticNPCHitCooldown = 10;
				projectile.scale = .5f;
			}
			SoundEngine.PlaySound(SoundID.Thunder);
		}
		RealCounter++;
	}
	public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Electrified)) {
			modifiers.SourceDamage += 1;
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.HasBuff(BuffID.Electrified)) {
			Vector2 sky = new(target.Center.X, player.Center.Y - 1000);
			Vector2 vel = target.Center - sky;
			Projectile projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), sky, vel.SafeNormalize(Vector2.Zero) * 10, ProjectileID.CultistBossLightningOrbArc, hit.Damage, hit.Knockback, player.whoAmI, vel.ToRotation(), Main.rand.Next(100));
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.extraUpdates = 15;
			projectile.penetrate = -1;
			projectile.maxPenetrate = -1;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
		}
		target.AddBuff(BuffID.Electrified, BossRushUtils.ToSecond(1));
	}
	public override float UseTimeMultiplier(Player player) {
		return base.UseTimeMultiplier(player);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.BreakerBlade)
			.AddIngredient(ItemID.NimbusRod)
			.Register();
	}
}
public class ShatteredSkyProjectileHidden : SynergyModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.hide = true;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 40;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		Projectile.Center += player.velocity;
		Projectile.velocity = Vector2.Zero;
		if (Projectile.timeLeft % 2 == 0) {
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY * 5, ProjectileID.CultistBossLightningOrbArc, Projectile.damage, Projectile.knockBack, player.whoAmI, Vector2.UnitY.ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(-10, 10)), Main.rand.Next(100));
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.extraUpdates = 10;
			projectile.penetrate = -1;
			projectile.maxPenetrate = -1;
			projectile.tileCollide = false;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
			projectile.scale += Main.rand.NextFloat();
			SoundEngine.PlaySound(SoundID.Thunder);
			Projectile.Center += Vector2.UnitX * Projectile.ai[0] * (44 + Main.rand.Next(-16, 16));
		}
	}
}
