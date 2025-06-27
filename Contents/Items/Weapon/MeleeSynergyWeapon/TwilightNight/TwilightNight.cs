using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Common.RoguelikeChange.ItemOverhaul;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.TwilightNight;

public class TwilightNight : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(68, 68, 35, 4f, 14, 14, ItemUseStyleID.Swing, ModContent.ProjectileType<TwilightNightProjectile>(), 20f, true);
		Item.GetGlobalItem<MeleeWeaponOverhaul>().SwingType = BossRushUseStyle.SwipeDown;
	}
	int ComboCounter = 0;
	int Timer = 0;
	public override bool CanUseItem(Player player) {
		if (!player.ItemAnimationActive) {
			if (++ComboCounter > 7) {
				ComboCounter = 1;
			}
			MeleeWeaponOverhaul overhaul = Item.GetGlobalItem<MeleeWeaponOverhaul>();
			switch (ComboCounter) {
				case 1:
					overhaul.OffSetAnimationPercentage = 1.25f;
					overhaul.SwingDegree = 140;
					overhaul.SwingStrength = 9;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					break;
				case 2:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					break;
				case 3:
					overhaul.SwingStrength = 15;
					overhaul.OffSetAnimationPercentage = 1;
					overhaul.SwingDegree = 70;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					break;
				case 4:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					break;
				case 5:
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					break;
				case 6:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					break;
				case 7:
					overhaul.OffSetAnimationPercentage = 3;
					overhaul.SwingStrength = 11;
					overhaul.SwingDegree = 160;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					break;
			}
		}
		return base.CanUseItem(player);
	}
	public override void ModifyItemScale(Player player, ref float scale) {
		switch (ComboCounter) {
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				return;
			case 7:
				scale += .15f;
				return;
		}
	}
	public override float UseSpeedMultiplier(Player player) {
		float speedMulti = base.UseSpeedMultiplier(player);
		switch (ComboCounter) {
			case 1:
			case 2:
				return speedMulti;
			case 3:
			case 4:
			case 5:
			case 6:
				return speedMulti * 2f;
			case 7:
				return speedMulti * .25f;
		}
		return base.UseSpeedMultiplier(player);
	}
	public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {
		Timer++;
		if (!player.ItemAnimationActive) {
			if (Timer >= 30 + player.itemAnimationMax) {
				ComboCounter = 0;
			}
			return;
		}
		Timer = 0;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		if (Main.rand.NextBool()) {
			var pos = position + Main.rand.NextVector2CircularEdge(60, 60) * Main.rand.NextFloat(.5f, 1);
			var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
			Projectile.NewProjectile(source, pos, vel, ProjectileID.SkyFracture, damage, knockback, player.whoAmI);
		}
		else {
			var pos2 = position + Main.rand.NextVector2CircularEdge(60, 60) * Main.rand.NextFloat(.5f, 1);
			var vel2 = (Main.MouseWorld - pos2).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
			Projectile.NewProjectile(source, pos2, vel2, ProjectileID.MagicMissile, damage, knockback, player.whoAmI);
		}
		if (Main.rand.NextBool(5)) {
			Vector2 customPos = new Vector2(position.X + Main.rand.Next(-100, 100), position.Y - 900 + Main.rand.Next(-200, 200));
			Vector2 vel3 = (Main.MouseWorld - customPos).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
			Projectile proj = Projectile.NewProjectileDirect(source, customPos, vel3, ProjectileID.SkyFracture, damage * 2, knockback, player.whoAmI);
			proj.extraUpdates += 1;
			proj.penetrate += 3;
			proj.maxPenetrate = proj.penetrate;
		}
		CanShootItem = true;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.MagicMissile)
			.AddIngredient(ItemID.SkyFracture)
			.Register();
	}
}
public class TwilightNightProjectile : SynergyModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TitaniumSword);
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 5;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 54;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 600;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(3)) {
			Vector2 pos = npc.Center + Main.rand.NextVector2CircularEdge(400, 400) + Main.rand.NextVector2CircularEdge(npc.width, npc.height) * 2;
			float randomrotation = Main.rand.NextFloat(90);
			Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
			for (int i = 0; i < 4; i++) {
				Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * (3 + Main.rand.NextFloat());
				for (int l = 0; l < 16; l++) {
					float multiplier = Main.rand.NextFloat();
					float scale = MathHelper.Lerp(2.1f, .9f, multiplier);
					int dust = Dust.NewDust(pos + randomPosOffset, 0, 0, DustID.GemSapphire, 0, 0, 0, Main.rand.Next(new Color[] { Color.White, Color.Cyan }), scale);
					Main.dust[dust].velocity = Toward * multiplier;
					Main.dust[dust].noGravity = true;
				}
			}
			Vector2 vel = (npc.Center - pos).SafeNormalize(Vector2.Zero) * 10;
			Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), pos, vel, ProjectileID.MagicMissile, Projectile.damage, Projectile.knockBack, Projectile.owner);
			proj.tileCollide = false;
		}
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		for (int i = 0; i < 30; i++) {
			float multiplier = Main.rand.NextFloat();
			float scale = MathHelper.Lerp(2.1f, .9f, multiplier);
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemSapphire, 0, 0, 0, Main.rand.Next(new Color[] { Color.White, Color.Cyan }), scale);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Projectile.velocity.Vector2RotateByRandom(30) * (2.1f - scale);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		Vector2 DrawPos = Projectile.position - Main.screenPosition + origin;
		Main.EntitySpriteDraw(texture, DrawPos, null, new(100, 250, 255, 0), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		Projectile.DrawTrail(new(100, 250, 255, 0));
		return false;
	}
}
