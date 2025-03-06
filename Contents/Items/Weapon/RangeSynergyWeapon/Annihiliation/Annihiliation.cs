using BossRush.Common.Systems;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Annihiliation;
internal class Annihiliation : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(138, 36, 33, 3f, 2, 6, ItemUseStyleID.Shoot, ModContent.ProjectileType<AnnihiliationBullet>(), 20f, true, AmmoID.Bullet);
		Item.scale = .87f;
		Item.UseSound = SoundID.Item38 with {
			Pitch = 1f
		};
		Item.Set_InfoItem();
	}
	public override Vector2? HoldoutOffset() {
		return new(-30, 0);
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		if (!player.ItemAnimationActive) {
			modplayer.Annihiliation_Counter++;
			if (modplayer.Annihiliation_Counter >= 360) {
				player.AddBuff<Epilogue_Ishboshet>(BossRushUtils.ToSecond(5));
			}
		}
		else {
			modplayer.Annihiliation_Counter = 0;
		}
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity * .1f;
		type = Item.shoot;
		position = position.PositionOFFSET(velocity, 90);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(3), type, damage, knockback, player.whoAmI);
		CanShootItem = false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.ChainGun)
			.AddIngredient(ItemID.TrueNightsEdge)
			.Register();
	}
}
public class Epilogue_Ishboshet : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		player.buffTime[buffIndex] = time;
		return true;
	}
	public override void Update(Player player, ref int buffIndex) {
		for (int i = 0; i < 2; i++) {
			Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, Main.rand.Next(new int[] { DustID.Shadowflame, DustID.Wraith, DustID.DemonTorch }));
			dust.noGravity = true;
			dust.velocity = Vector2.UnitY * -Main.rand.NextFloat(10);
			dust.scale = Main.rand.NextFloat(0.75f, 1.25f);
		}
		if (player.HeldItem.type == ModContent.ItemType<Annihiliation>()) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 4);
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 2);
		}
	}
}
public class AnnihiliationBullet : SynergyModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = BossRushUtils.ToSecond(30);
		Projectile.extraUpdates = 20;
		Projectile.penetrate = 12;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
	}
	public override bool? CanDamage() {
		return Projectile.penetrate != 1;
	}
	public override Color? GetAlpha(Color lightColor) {
		Color color = Color.White;
		color.A = 0;
		//money symbol
		return color * Projectile.Opacity;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (Projectile.penetrate <= 10) {
			int timeleft = 300;
			if (Projectile.timeLeft > timeleft) {
				Projectile.timeLeft = timeleft;
			}
			float progress = Projectile.timeLeft / (float)timeleft;
			//Projectile.alpha = (int)(255 * progress);
			Projectile.Opacity = BossRushUtils.InOutExpo(progress, 15);
			if (Projectile.ai[0] < 0) {
				return;
			}
			Projectile.velocity *= .995f;
		}
	}
	public override void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) {
		if (player.HasBuff<Epilogue_Ishboshet>()) {
			modifiers.ScalingArmorPenetration += 1;
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		float randomrotation = Main.rand.NextFloat(90);
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		for (int i = 0; i < 4; i++) {
			Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * Main.rand.NextFloat(2, 3);
			for (int l = 0; l < 4; l++) {
				float multiplier = Main.rand.NextFloat();
				float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
				int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemAmethyst, 0, 0, 0, Main.rand.Next(new Color[] { Color.White, Color.Purple }), scale);
				Main.dust[dust].velocity = Toward * multiplier;
				Main.dust[dust].noGravity = true;
			}
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -1;
		Projectile.timeLeft = 120;
		return false;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Color.Magenta * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color.A = 0;
			float scaling = Math.Clamp(k * .01f, 0, 10f);
			Main.EntitySpriteDraw(texture, drawPos, null, color * Projectile.Opacity, Projectile.oldRot[k], origin, Projectile.scale - scaling, SpriteEffects.None, 0);

			Color color2 = Color.White * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color2.A = 0;
			Main.EntitySpriteDraw(texture, drawPos, null, color2 * Projectile.Opacity, Projectile.oldRot[k], origin, (Projectile.scale - scaling) * .5f, SpriteEffects.None, 0);
		}
		return false;
	}
}
