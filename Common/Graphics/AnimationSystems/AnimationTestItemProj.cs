using BossRush.Common.Graphics.AnimationSystem;
using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics.AnimationSystems;
public class AnimationTestItemProj : ModItem {

	ItemProjectile itemProjectile;
	Tween<Vector2> recoilTween = new Tween<Vector2>(Vector2.Lerp);
	Tween<float> recoilRotationTween = new Tween<float>(MathHelper.Lerp);
	Tween<Vector2> recoilScaleTween = new Tween<Vector2>(Vector2.Lerp);

	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Minishark);
	public override void SetDefaults() {

		Item.CloneDefaults(ItemID.Minishark);
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.noUseGraphic = true;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {

		velocity = velocity.RotatedByRandom(MathHelper.PiOver4 / 5);
		
		if(itemProjectile != null)
			itemProjectile.Projectile.Kill();

		itemProjectile = ItemProjectile.SpawnItemProjectile(player,position,velocity.ToRotation(), Item.useAnimation, TextureAssets.Item[ItemID.Minishark].Value, Rectangle.Empty, TextureAssets.Item[ItemID.Minishark].Value.Size() / 2f, new Vector2(1f, 1f), player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, Animate);
		itemProjectile.PositionOffset = velocity.SafeNormalize(Vector2.UnitY) * 15;
		recoilScaleTween.Start(new Vector2(0.5f,1.5f),new Vector2(1f,1f), TweenEaseType.None, Item.useAnimation);
		recoilTween.Start(player.Center.DirectionTo(player.Center + velocity) * 5 - new Vector2(0,5), player.Center.DirectionTo(player.Center + velocity) * 15 - new Vector2(0,5), TweenEaseType.None, Item.useAnimation);

		return true;
	}

	public bool Animate(Player player, Vector2 startingPosition, Vector2 startingPositionOffset) 
	{
		recoilRotationTween.Update();
		recoilTween.Update();
		recoilScaleTween.Update();

		if (itemProjectile == null)
			return true;

		itemProjectile.PositionOffset = recoilTween.currentProgress;
		itemProjectile.spriteScale = recoilScaleTween.currentProgress;

		return true;
	}

}
