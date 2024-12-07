using BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.StarWhip;
using BossRush.Texture;
using BossRush.TrailStructs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.WyvernWrath;
public class WyvernWrath : SynergyModItem {


	public override void Synergy_SetStaticDefaults() {
		Item.staff[Type] = true;
	}
	public override void SetDefaults() {
		Item.damage = 32;
		Item.ArmorPenetration = 10;
		Item.DamageType = DamageClass.Magic;
		Item.knockBack = 15;
		Item.crit = 30;
		Item.useAnimation = 30;
		Item.useTime = 10;
		Item.reuseDelay = 10;
		Item.shoot = ModContent.ProjectileType<WyvernWrathMainProjectile>();
		Item.width = Item.height = 37;
		Item.mana = 24;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.shootSpeed = 20;
		Item.noMelee = true;
		Item.UseSound = SoundID.Item125;
	}


	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

		int playerDir = player.direction;
		position = playerDir == 1 ? new Vector2(position.X - 35 - Main.rand.Next(0, 35), position.Y - Main.rand.Next(0,35)) : new Vector2(position.X + 35 + Main.rand.Next(0,35), position.Y - Main.rand.Next(0, 35));

		velocity = position.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.UnitY) * velocity.Length();

		for (int i = 0; i < 25; i++) 
		{
			var dust = Dust.NewDustPerfect(position, DustID.WhiteTorch, Main.rand.NextVector2CircularEdge(5, 5), 0, Color.Turquoise);
			dust.noGravity = true;
		}
	}

	public override void AddRecipes() {
		CreateRecipe().AddIngredient(ItemID.SkyFracture).AddIngredient(ItemID.CrystalSerpent).Register();
	}

}

public class WyvernWrathMainProjectile : SynergyModProjectile 
{
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 35;
	}

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SkyFracture);

	public override void SetDefaults() {

		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.light = 0.8f;
		Projectile.tileCollide = false;

	
	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.FillProjectileOldPosAndRot();
		Projectile.frame = Main.rand.Next(14);
		

	}
	public override bool PreDraw(ref Color lightColor) {

		Asset<Texture2D> texture = TextureAssets.Projectile[Type];
		Main.instance.LoadProjectile(ProjectileID.SkyFracture);

		Main.EntitySpriteDraw(texture.Value,Projectile.Center - Main.screenPosition,new Rectangle(Projectile.frame * 38, 0, 38,38),Color.Turquoise,Projectile.velocity.ToRotation() + MathHelper.PiOver4,new Vector2(38) / 2f,1f,SpriteEffects.None);
		default(WyvernTrailMain).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f);

		return false;
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		Vector2 pos = (Projectile.Center + new Vector2(750, 0)).RotatedBy(Main.rand.NextFloat(MathHelper.ToRadians(360)),Projectile.Center);
		Projectile.NewProjectile(Projectile.GetSource_OnHit(npc), pos, pos.DirectionTo(Projectile.Center).SafeNormalize(Vector2.UnitY) * 25, ModContent.ProjectileType<WyvernWrathMiniProjectile>(), (int)(Projectile.damage * 0.8f), 0f, Projectile.owner);
	}
}

public class WyvernWrathMiniProjectile : SynergyModProjectile 
{
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 35;
	}

	public override void SetDefaults() 
	{


		Projectile.width = Projectile.height = 8;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 3;
		Projectile.penetrate = -1;
		Projectile.localNPCHitCooldown = 15;
		Projectile.usesLocalNPCImmunity = true;


	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.FillProjectileOldPosAndRot();

		for (int i = 0; i < 25; i++) {
			var dust = Dust.NewDustPerfect(Projectile.Center, DustID.WhiteTorch, Main.rand.NextVector2CircularEdge(5, 5), 0, Color.Turquoise);
			dust.noGravity = true;
		}
	}

	public override string Texture => BossRushTexture.MissingTexture_Default;


	public override bool PreDraw(ref Color lightColor) {

		default(WyvernTrailMini).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f);

		return false;
	}

}
