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
		Item.BossRushDefaultRange(26, 72, 20, 2f, 23, 23, ItemUseStyleID.Shoot, ProjectileID.WoodenArrowFriendly, 10, true, AmmoID.Arrow);
		Item.Set_InfoItem();
		Item.UseSound = SoundID.Item5;
	}
	int delayBetweenShot = 0;
	float chance = .001f;
	public override Vector2? HoldoutOffset() {
		return new Vector2(-8, 0);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
		TundraBow_ModPlayer tundra = player.GetModPlayer<TundraBow_ModPlayer>();
		if (Main.rand.NextFloat() <= chance) {
			tundra.ActivateSnowStorm(Item);
			chance = .001f;
		}
		else {
			if (tundra.SnowStormCounter <= 0 && false) {
				chance += .001f;
			}
		}
		int snowdustamount = Main.rand.Next(12, 19);
		for (int i = 0; i < snowdustamount; i++) {
			Dust dust = Dust.NewDustDirect(position + Main.rand.NextVector2Circular(26, 36).RotatedBy(velocity.ToRotation()), 0, 0, DustID.Snow);
			dust.velocity = velocity.Vector2RotateByRandom(20) * Main.rand.NextFloat(.2f, .5f);
			dust.scale = Main.rand.NextFloat(.95f, 1.22f);
			dust.noGravity = true;
		}
		int hitcounter = tundra.HitCounter;
		int snowballDamage = (int)(damage * (.35f + hitcounter * .1f));
		if (++delayBetweenShot >= 5 || tundra.SnowStormCounter > 0) {
			int amount = Main.rand.Next(3, 6);
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(amount, 15, i), ProjectileID.SnowBallFriendly, snowballDamage, knockback, player.whoAmI);
			}
			delayBetweenShot = 0;
		}
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
	public int SnowStormCounter = 0;
	public int SnowStormDamage = 0;
	public Item SnowStormItemSource = null;
	public int SnowStormInitialDirection = 1;
	public void ActivateSnowStorm(Item item) {
		SnowStormCounter = BossRushUtils.ToSecond(5);
		SnowStormDamage = Player.GetWeaponDamage(item);
		SnowStormItemSource = item.Clone();
		SnowStormInitialDirection = Player.direction;
	}
	public override void ResetEffects() {
		if (HitCounter > 0) {
			if (++HitCounter_Counter >= 30) {
				HitCounter--;
				HitCounter_Counter = 0;
			}
		}
		SnowStormCounter = BossRushUtils.CountDown(SnowStormCounter);
	}
	public override void UpdateEquips() {
		if (SnowStormCounter > 0 && SnowStormItemSource != null) {
			Vector2 vel = Vector2.One;
			vel.X *= SnowStormInitialDirection * 5;
			for (int i = 0; i < 10; i++) {
				Vector2 pos = Player.Center.Add(Main.rand.Next(-1500, 1500), 800); ;
				Dust dust = Dust.NewDustDirect(pos, 0, 0, DustID.Snow);
				dust.velocity = Vector2.UnitY.Vector2RotateByRandom(30) * 5 * Main.rand.NextFloat(1, 5);
				dust.scale += Main.rand.NextFloat(2);
			}
			for (int i = 0; i < 4; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(SnowStormItemSource), Player.Center.Add(Main.rand.Next(-1500, 1500), 800), vel, ProjectileID.SnowBallFriendly, SnowStormDamage, 1f, Player.whoAmI);
				projectile.tileCollide = false;
				projectile.timeLeft = BossRushUtils.ToSecond(2.5f);
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		//this if for the buff itself, don't change its condition
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
