using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SnowballCannonMK2;
//SnowballCannonMK2
//Uses snowball as ammo, shoot out snowball faster
//After every 10th shot, shoot out a giant snowball that deal 300% of weapon damage
//Ice bolt have 10% to be accompany with every shot, Ice bolt deal 125% weapon damage
//Alt attack to do melee attack, melee attack will shoot out Ice bolt
internal class SnowballCannonMK2 : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.Minishark);
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.WandofFrosting);
	}
	public override void SetDefaults() {
		Item.BossRushDefaultRange(72, 26, 18, 3f, 14, 14, ItemUseStyleID.Shoot, ProjectileID.SnowBallFriendly, 12, true, AmmoID.Snowball);
		Item.UseSound = SoundID.Item11;
	}
	int counter = 0;
	public override Vector2? HoldoutOffset() {
		return new Vector2(-5, 0);
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.WandofFrosting)) {
			tooltips.Add(new(Mod, Set_TooltipName(ItemID.Minishark), $"[i:{ItemID.Minishark}] Decreases shot requirement for giant snowball to 3, increases attack speed by 25%"));
		}
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.WandofFrosting)) {
			tooltips.Add(new(Mod, Set_TooltipName(ItemID.WandofFrosting), $"[i:{ItemID.WandofFrosting}] Increases the chance for ice bolt to shoot by 40%, giant snowball explode out frost spark on death"));
		}
	}
	public override float UseSpeedMultiplier(Player player) {
		float speed = base.UseSpeedMultiplier(player);
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Minishark)) {
			speed += .25f;
		}
		return speed;
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 70);
		if (++counter >= 10 || SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Minishark) && counter >= 3) {
			type = ModContent.ProjectileType<GiantSnowBall>();
			damage *= 3;
			counter = 0;
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = true;
		int amount = 15;
		float scalerMax = 1;
		if (type == ModContent.ProjectileType<GiantSnowBall>()) {
			amount += 35;
			scalerMax = 3;
		}
		for (int i = 0; i < amount; i++) {
			float scale = Main.rand.NextFloat(scalerMax);
			var dust = Dust.NewDustDirect(position, 0, 0, DustID.Snow);
			dust.noGravity = true;
			dust.position += Main.rand.NextVector2Circular(10, 10);
			dust.velocity = velocity.Vector2RotateByRandom(25) * scale * .5f;
			dust.scale = scalerMax + .5f - scale;
		}
		if (Main.rand.NextBool(10) || SynergyBonus_System.Check_SynergyBonus(Type, ItemID.WandofFrosting) && Main.rand.NextBool()) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBolt, (int)(damage * 1.25f), knockback, player.whoAmI);
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SnowballCannon)
			.AddIngredient(ItemID.FlareGun)
			.AddIngredient(ItemID.IceBlade)
			.Register();
	}
}
public class GiantSnowBall : SynergyModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SnowBallFriendly);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 14;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 999;
		Projectile.penetrate = 1;
		Projectile.scale = 3;
		Projectile.extraUpdates = 1;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Snow);
		dust.noGravity = true;
		dust.position += Main.rand.NextVector2Circular(32, 32);
		if (++Projectile.ai[0] >= 25) {
			Projectile.velocity.Y += .5f;
		}
		Projectile.velocity.X *= .99f;
		Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0]);
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<SnowballCannonMK2>(), ItemID.WandofFrosting)) {
			for (int i = 0; i < 16; i++) {
				Vector2 vel = Vector2.One.InverseVector2DistributeEvenly(16, 360, i) * 3;
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, vel, ProjectileID.WandOfFrostingFrost, Projectile.damage / 3, 0, Projectile.owner);
				proj.timeLeft = 30;
			}
		}
		SoundEngine.PlaySound(SoundID.Item51 with { Pitch = -.25f });
		for (int i = 0; i < 35; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Snow);
			dust.noGravity = true;
			dust.position += Main.rand.NextVector2Circular(32, 32);
			dust.velocity = Projectile.velocity.Vector2RotateByRandom(15).Vector2RandomSpread(2, Main.rand.NextFloat(1, 1.44f)) * .55f;
			dust.scale = Main.rand.NextFloat(.85f, 1.34f);
		}
		for (int i = 0; i < 15; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Snow);
			dust.noGravity = true;
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(15).SafeNormalize(Vector2.Zero) * 5 * Main.rand.NextFloat();
			dust.scale = Main.rand.NextFloat(.85f, 1.34f);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out var tex, out var origin);
		var drawpos = Projectile.position - Main.screenPosition + origin * Projectile.scale;
		Main.EntitySpriteDraw(tex, drawpos, null, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
