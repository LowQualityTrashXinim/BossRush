using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.WinterFlame;
public class WinterFlame : SynergyModItem {

	public override void SetDefaults() {
		
		Item.CloneDefaults(ItemID.Flamethrower);
		Item.useAnimation = 60;
		Item.useTime = 10;
		Item.width = 120;
		Item.height = 30;
		Item.shoot = ModContent.ProjectileType<WinterFlameProjectile>();
		Item.shootSpeed = 15;
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-40,0);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position += velocity.SafeNormalize(Vector2.UnitY) * 45;
		Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(Item,Item.ammo), position, velocity, type, damage, knockback, player.whoAmI, 1);
	}



}

public class WinterFlameProjectile : SynergyModProjectile 
{

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Flames);
	bool isFrost;
	Vector2 startingVelocity;
	private readonly static float maxCounter = 35f;
	public int coutner {
		get => (int)Projectile.ai[1];
		set => Projectile.ai[1] = value;
	}
	Point flameTexutreSize = new Point(98,686);


	public struct FlameThrowerFrost {
		private static VertexStrip _vertexStrip = new VertexStrip();
		public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset, float progress) {

			MiscShaderData miscShaderData = GameShaders.Misc["FlameEffect"];
			miscShaderData.UseImage1("Images/Extra_" + (short)193);
			miscShaderData.UseColor(Color.CornflowerBlue);
			miscShaderData.UseShaderSpecificData(new Vector4(progress, 0, 0, 0));

			miscShaderData.Apply();

			_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth, -Main.screenPosition + offset);
			_vertexStrip.DrawTrail();

			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}
		private Color StripColors(float progressOnStrip) {
			Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
			//result.A /= 2;
			return result;
		}
		private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5f, 12f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
	}
	public struct FlameThrowerFire {
		private static VertexStrip _vertexStrip = new VertexStrip();
		public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset, float progress) {

			MiscShaderData miscShaderData = GameShaders.Misc["FlameEffect"];
			miscShaderData.UseImage1("Images/Extra_" + (short)193);
			miscShaderData.UseColor(Color.Orange);
			miscShaderData.UseShaderSpecificData(new Vector4(progress,0,0,0));
			miscShaderData.Apply();

			_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth, -Main.screenPosition + offset);
			_vertexStrip.DrawTrail();

			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}
		private Color StripColors(float progressOnStrip) {
			Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
			//result.A /= 2;
			return result;
		}
		private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5f, 12f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
	}
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 30;
		ProjectileID.Sets.TrailingMode[Type] = 3;
	}
	public override void SetDefaults() {
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 300;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 120;
		Projectile.penetrate = -1;
		Projectile.localNPCHitCooldown = 15;
		Projectile.usesLocalNPCImmunity = true;

	}

	public override void OnSpawn(IEntitySource source) {

		startingVelocity = Projectile.velocity;

		if (Projectile.ai[0] == 0)
			isFrost = false;
		else
			isFrost = true;

	}


	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {

		if (!isFrost)
			npc.AddBuff(BuffID.OnFire3,60);
		else
			npc.AddBuff(BuffID.Frostburn2, 60);

	}

	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {

		Dust dust;
		
		for (int i = 0; i < Projectile.oldPos.Length; i++)
			for(int j = 0; j < 5; j++)
				if (!isFrost) {
					dust = Dust.NewDustDirect(Projectile.oldPos[i], 16, 16, DustID.InfernoFork, startingVelocity.X, startingVelocity.Y);
					dust.noGravity = true;

				}
				else 
				{

					dust = Dust.NewDustDirect(Projectile.oldPos[i], 16, 16, DustID.FrostHydra, startingVelocity.X, startingVelocity.Y);
					dust.noGravity = true;

				}

	}

	public override bool PreDraw(ref Color lightColor) {

		//Asset<Texture2D> flameTexture = TextureAssets.Projectile[Type];

		//Main.instance.LoadProjectile(Projectile.type);

		//float scale = MathHelper.Clamp(MathHelper.Lerp(0f, 1f, coutner / maxCounter), 0f, 1f);
		//Color flameColor = Color.Lerp(isFrost ? Color.OrangeRed : Color.DeepSkyBlue, Color.Lerp(isFrost ? Color.Orange : Color.CornflowerBlue, Color.Gray, (coutner - 60f) / maxCounter), coutner / maxCounter);

	
		//Main.EntitySpriteDraw(flameTexture.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 1 * 98, 98, 98), flameColor, Projectile.rotation, new Vector2(98) / 2f, scale, SpriteEffects.None);

		

		if (!isFrost)
			default(FlameThrowerFrost).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f, coutner);
		else
			default(FlameThrowerFire).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f, coutner);

		return false;
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		startingVelocity = Vector2.Zero;
		Projectile.velocity = Vector2.Zero;
		return base.OnTileCollide(oldVelocity);
	}

	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {

		coutner++;

		float sinOffset = -2f;
		if(player.direction == 1)
			Projectile.velocity = startingVelocity.RotatedBy(isFrost ? MathF.Sin((coutner + sinOffset) * 0.25f) * 1f : -MathF.Sin((coutner + sinOffset) * .25f) * 1f);
		else
			Projectile.velocity = startingVelocity.RotatedBy(isFrost ? -MathF.Sin((coutner + sinOffset) * 0.25f) * 1f : MathF.Sin((coutner + sinOffset) * .25f) * 1f);

		Projectile.rotation = Projectile.velocity.ToRotation();

	}

}

