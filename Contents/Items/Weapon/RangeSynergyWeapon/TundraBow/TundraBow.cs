using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.TundraBow;
public class TundraBow : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(26, 72, 20, 2f, 18, 18, ItemUseStyleID.Shoot, ProjectileID.WoodenArrowFriendly, 10, true, AmmoID.Arrow);
		Item.Set_InfoItem();
	}
	int delayBetweenShot = 0;
	public override Vector2? HoldoutOffset() {
		return new Vector2(-8, 0);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
		int snowdustamount = Main.rand.Next(5, 12);
		for (int i = 0; i < snowdustamount; i++) {
			Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.SnowBlock);
			dust.velocity = velocity.Vector2RotateByRandom(20) * Main.rand.NextFloat(.2f, .5f);
			dust.scale = Main.rand.NextFloat(.85f, 1.12f);
		}
		int hitcounter = player.GetModPlayer<TundraBow_ModPlayer>().HitCounter;
		int snowballDamage = (int)(damage * (.35f + hitcounter * .1f));
		if (++delayBetweenShot < 5) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(15), ProjectileID.SnowBallFriendly, snowballDamage, knockback, player.whoAmI);
			return;
		}
		int amount = Main.rand.Next(3, 6);
		for (int i = 0; i < amount; i++) {
			Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(amount, 15, i), ProjectileID.SnowBallFriendly, snowballDamage, knockback, player.whoAmI);
		}
		delayBetweenShot = 0;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddRecipeGroup("Wood Bow")
			.AddIngredient(ItemID.SnowballCannon)
			.Register();
	}
}
public class TundraBow_ModPlayer : ModPlayer {
	public int HitCounter = 0;
	public int HitCounter_Counter = 0;
	public override void ResetEffects() {
		if (HitCounter > 0) {
			if (++HitCounter_Counter >= 30) {
				HitCounter--;
				HitCounter_Counter = 0;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.type == ProjectileID.SnowBallFriendly && target.HasBuff<TundraHypothermia>()) {
			modifiers.SourceDamage += .2f;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(ModContent.ItemType<TundraBow>())) {
			HitCounter = Math.Clamp(HitCounter + 1, 0, 5);
			HitCounter_Counter = 0;
			if (proj.type == ProjectileID.SnowBallFriendly) {
				target.AddBuff<TundraHypothermia>(BossRushUtils.ToSecond(2));
			}
		}
	}
}
public class TundraHypothermia : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		var global = npc.GetGlobalNPC<RoguelikeGlobalNPC>();
		global.VelocityMultiplier -= .2f;
		global.StatDefense -= .1f;
		npc.lifeRegen -= 3;
	}
}
