﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Build.Execution;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;
using BossRush.TrailStructs;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.MythrilBeamSword;
public class MythrilBeamSword : SynergyModItem {


	public override void SetDefaults() 
	{

		Item.CloneDefaults(ItemID.BeamSword);
		Item.width = Item.width = 48;
		Item.damage = 88;
		Item.shoot = ModContent.ProjectileType<MythrilBeam>();
		Item.shootSpeed = 15;
	}

	public override void AddRecipes() {
		CreateRecipe().AddIngredient(ItemID.MythrilSword).AddIngredient(ItemID.BeamSword).Register();
	}

}

public class MythrilBeam : SynergyModProjectile 
{

	bool retargeting = false;

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 35;
	}

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SwordBeam);


	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.SwordBeam);
		Projectile.aiStyle = -1;
		
	}

	public override void OnSpawn(IEntitySource source) {
		retargeting = false;
		Projectile.FillProjectileOldPosAndRot();
	}

	public override bool PreDraw(ref Color lightColor) {


		Asset<Texture2D> texture = TextureAssets.Projectile[ProjectileID.SwordBeam];
		Main.instance.LoadProjectile(ProjectileID.SwordBeam);

		if (!retargeting) {

			Main.EntitySpriteDraw(texture.Value, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 15 - Main.screenPosition, null, Color.Yellow, Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2f, 1f, SpriteEffects.None);
			default(BeamTrail).Draw(Projectile,Color.Yellow);


		}
		else 
		{

			Main.EntitySpriteDraw(texture.Value, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 15 - Main.screenPosition, null, Color.MediumVioletRed, Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2f, 1f, SpriteEffects.None);
			default(BeamTrail).Draw(Projectile, Color.MediumVioletRed);

		}
		
		return false;
	}

	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) 
	{ 

		Projectile.ai[0]++;
		Projectile.rotation = Projectile.velocity.ToRotation();

		int range = 1200;
		Vector2 targetPos = Projectile.Center.LookForHostileNPCPositionClosest(range);

		if (Projectile.ai[0] == 25) 
		{
			retargeting = true;
			Projectile.damage = (int)(Projectile.damage * 1.25f);
			for (int i = 0; i < 35; i++) {
				var dust = Dust.NewDustPerfect(Projectile.position, DustID.OrangeTorch, Main.rand.NextVector2CircularEdge(10, 10));
				dust.noGravity = true;

			}
			Projectile.velocity =
			targetPos != Vector2.Zero
			? Projectile.Center.DirectionTo(targetPos) * Projectile.velocity.Length()
			: Projectile.velocity;


		}
	



	}

}
