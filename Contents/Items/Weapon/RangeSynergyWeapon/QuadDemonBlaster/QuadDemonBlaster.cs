using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.QuadDemonBlaster;
class QuadDemonBlaster : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		base.Synergy_SetStaticDefaults();
	}
	public override void SetDefaults() {
		Item.BossRushDefaultRange(40, 30, 29, 3f, 15, 15, ItemUseStyleID.Shoot, ProjectileID.Bullet, 15, true, AmmoID.Bullet);
		Item.value = Item.buyPrice(gold: 50);
		Item.rare = ItemRarityID.Orange;
		Item.reuseDelay = 15;
		Item.UseSound = SoundID.Item41;
		if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
			weapon.OffSetPost = 30;
			weapon.NumOfProjectile = 1;
		}
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		base.ModifySynergyToolTips(ref tooltips, modplayer);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		QuadDemonBlaster_ModPlayer quad = player.GetModPlayer<QuadDemonBlaster_ModPlayer>();
		if (quad.HitBucketLimitReached) {
			quad.HitBucket -= 500;
			type = ProjectileID.ExplosiveBullet;
			if (Main.rand.NextBool()) {
				type = ProjectileID.Flamelash;
			}
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		QuadDemonBlaster_ModPlayer quad = player.GetModPlayer<QuadDemonBlaster_ModPlayer>();
		float rotation = quad.QuadDemonBlaster_SpeedMultiplier;
		for (int i = 0; i < 10; i++) {
			if (Main.rand.NextFloat() <= .1f) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30), ProjectileID.Flamelash, (int)(damage * 1.25f), knockback, player.whoAmI);
			}
			Vector2 Rotate = velocity.Vector2DistributeEvenly(10, rotation, i);
			float RandomSpeadx = Main.rand.NextFloat(0.5f, 1f);
			float RandomSpeady = Main.rand.NextFloat(0.5f, 1f);
			Projectile.NewProjectile(source, position.X, position.Y,
				Rotate.X * (quad.QuadDemonBlaster_SpeedMultiplier == 1 ? 1 : RandomSpeadx),
				Rotate.Y * (quad.QuadDemonBlaster_SpeedMultiplier == 1 ? 1 : RandomSpeady),
				type, damage, knockback, player.whoAmI);
		}
		if (Main.rand.NextFloat() <= .1f) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.DD2PhoenixBowShot, damage * 3, knockback * 3, player.whoAmI);
		}
		quad.QuadDemonBlaster_SpeedMultiplier += quad.QuadDemonBlaster_SpeedMultiplier < 45 ? 20 : 1;
		CanShootItem = false;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-4, 2);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.QuadBarrelShotgun)
			.AddIngredient(ItemID.PhoenixBlaster)
			.Register();
	}
}
public class QuadDemonBlaster_ModPlayer : ModPlayer {
	public float QuadDemonBlaster_SpeedMultiplier = 1;
	public int HitBucket = 0;
	public bool HitBucketLimitReached = false;
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource<QuadDemonBlaster>()) {
			if (HitBucket >= 5000) {
				HitBucketLimitReached = true;
			}
			else if (HitBucket <= 0) {
				HitBucketLimitReached = false;
			}
			if (!HitBucketLimitReached)
				HitBucket += hit.Damage;
		}
	}
	public override void ResetEffects() {
		QuadDemonBlaster_SpeedMultiplier -= QuadDemonBlaster_SpeedMultiplier == 1 ? 0 : .25f;
		if (!Player.ItemAnimationActive) {
			HitBucket = BossRushUtils.CountDown(HitBucket);
		}
	}
}
