using BossRush.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.UI;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Common.Graphics.AnimationSystem;
public class ItemProjectile : ModProjectile {

	public Vector2 startingPosition;
	public Vector2 startingPositionOffset;
	public Vector2 PositionOffset;
	public delegate bool Animate(Player player, Vector2 startingPosition, Vector2 startingPositionOffset);
	public Animate animation;
	public Texture2D sprite;
	public Rectangle spriteSourceRect;
	public Color spriteColor = Color.White;
	public Vector2 spriteOrigin;
	public Vector2 spriteScale;
	public SpriteEffects spriteEffect;
	public bool justReachedItsHalfTimeleft = false;
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = 0;
		Projectile.height = 0;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		
	}

	public static ItemProjectile SpawnItemProjectile(Player player, Vector2 position, float rotation, int timeLeft,Texture2D sprite, Rectangle? spriteSourceRect, ) 
	{
		var proj = Projectile.NewProjectileDirect(null, position, Vector2.Zero, ModContent.ProjectileType<ItemProjectile>(), 0, 0, player.whoAmI);
		proj.timeLeft = timeLeft;
		proj.rotation = rotation;
		return (ItemProjectile) proj.ModProjectile;

	}

	public override bool PreDraw(ref Color lightColor) {

		Main.EntitySpriteDraw(sprite,Projectile.Center - Main.screenPosition,null, spriteColor, Projectile.rotation,spriteOrigin,spriteScale,spriteEffect);

		return false;
	}

	public override void OnSpawn(IEntitySource source) {
		startingPosition = Projectile.Center;
		startingPositionOffset = Main.player[Projectile.owner].Center - startingPosition;
	}

	public override void AI() {
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		Projectile.Center = player.Center + PositionOffset;



	}


}
