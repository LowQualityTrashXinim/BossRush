using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon;

class RisingStar : ModItem {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PhoenixBlaster);
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 30, 3, 18, 18, ItemUseStyleID.Shoot, ProjectileID.Bullet, 1, true, AmmoID.Bullet);
	}
	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ModContent.ProjectileType<RisingStarProjectile>();
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, position, velocity * .5f, type, damage, knockback, player.whoAmI);
		return false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Handgun)
			.AddIngredient(ModContent.ItemType<WeaponBluePrint>())
			.Register();
	}
}
class RisingStarProjectile : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.timeLeft = 1800;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 10;
		Projectile.penetrate = 1;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff<RisingStarBurning_Debuff>(BossRushUtils.ToSecond(5));
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.FinalDamage.Flat = Math.Clamp(target.lifeMax / 100, 1, int.MaxValue);
		modifiers.FinalDamage *= 0;
		modifiers.SetMaxDamage(int.MaxValue);
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutAlpha(new Color(255, 50, 50), .01f);
		return false;
	}
}
class RisingStarBurning_Debuff : ModBuff {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= Math.Max((npc.lifeMax / 100), 1);
	}
	public override void Update(Player player, ref int buffIndex) {
		player.lifeRegen -= Math.Max((player.statLifeMax2 / 100), 1);
	}
}
