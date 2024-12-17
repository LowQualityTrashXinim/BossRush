using BossRush.Common.Graphics.AnimationSystem;
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

	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Minishark);
	public override void SetDefaults() {

		Item.CloneDefaults(ItemID.Minishark);
		Item.useAnimation *= 2;
		Item.noUseGraphic = true;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if(itemProjectile != null)
			itemProjectile?.Projectile.Kill();
		itemProjectile = ItemProjectile.SpawnItemProjectile(player,position,velocity.ToRotation(), 25);
		itemProjectile.sprite = TextureAssets.Item[ItemID.Minishark].Value;
		itemProjectile.PositionOffset = velocity.SafeNormalize(Vector2.UnitY) * 15 + new Vector2(0,-5);
		itemProjectile.spriteOrigin = itemProjectile.sprite.Size() / 2f;
		itemProjectile.spriteScale = new Vector2(1, 1);
		itemProjectile.spriteSourceRect = Rectangle.Empty;
		recoilRotationTween.Start(player.itemRotation,player.itemRotation - MathHelper.PiOver4 * player.direction, TweenEaseType.None, 15);
		return true;
	}

	public override void UseStyle(Player player, Rectangle heldItemFrame) 
	{
		if (itemProjectile == null)
			return;

		itemProjectile.Projectile.rotation = recoilRotationTween.currentProgress;

		if(itemProjectile?.Projectile.timeLeft == 7) 
		{
			recoilRotationTween.Start(player.itemRotation, player.itemRotation + MathHelper.PiOver4 * player.direction, TweenEaseType.None, 15);

		}

	}

	public bool Animate(Player player, Vector2 startingPosition, Vector2 startingPositionOffset) 
	{
	


	
		return true;
	}

}
