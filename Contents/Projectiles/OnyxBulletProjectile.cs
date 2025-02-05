using BossRush.Common.Graphics;
using BossRush.Common.Graphics.Structs.QuadStructs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
public class OnyxBulletProjectile : ModProjectile {

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.BulletHighVelocity); 
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.aiStyle = -1;
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.timeLeft = 10;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1;
		Projectile.tileCollide = false;
		Projectile.ai[1] = 1;
	}

	public override void OnSpawn(IEntitySource source) {
		SoundStyle pewpewSound = new SoundStyle($"BossRush/Contents/Projectiles/OnyxBlasterWESound");
		SoundEngine.PlaySound(pewpewSound with { Pitch = Main.rand.NextFloat(-1f,1f)},Projectile.Center);
		Projectile.velocity.SafeNormalize(Vector2.UnitY);
		startingPos = Projectile.Center;
		Projectile.ai[2] = Main.rand.NextFloat(MathHelper.TwoPi);
	}

	Vector2 startingPos;

	public override void PostAI() {
		Projectile.Center = startingPos;
		LaserLength = 0;
		for (int i = 0; i < 50; i++) {

			if (Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + Projectile.velocity * LaserLength, 1, 1))
				LaserLength = i;
			else 
	
			break;

		}
	}

	public float LaserLength 
	{

		get { return Projectile.ai[0]; }
		set { Projectile.ai[0] = value ; }

	}

	public override bool OnTileCollide(Vector2 oldVelocity) {



		return true;
	}

	public override bool PreDraw(ref Color lightColor) {

		ShaderSettings shaderSettings = new ShaderSettings();
		shaderSettings.image1 = TextureAssets.Extra[193];
		shaderSettings.image2 = null;
		shaderSettings.image3 = null;
		shaderSettings.Color = Color.Magenta;
		shaderSettings.shaderData = new Vector4(Color.Black.ToVector3(), Projectile.velocity.Length() * LaserLength);


		default(LaserQuad).Draw(Projectile.Center,Projectile.velocity.ToRotation(), new Vector2(Projectile.velocity.Length() * LaserLength,  100 * ((float)Projectile.timeLeft / 15f)), shaderSettings);

		shaderSettings.image1 = TextureAssets.Extra[193];
		shaderSettings.image2 = null;
		shaderSettings.image3 = null;
		shaderSettings.Color = Color.DarkMagenta;
		shaderSettings.shaderData = new Vector4((float)Projectile.timeLeft / 10, BossRushUtils.InOutSine((float)Projectile.timeLeft / 10),0, Projectile.ai[2]);

		default(SparkQuad).Draw(Projectile.Center + Projectile.velocity * LaserLength, 0, Vector2.One * 128, shaderSettings);

		return false;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		float point = 0f;
		return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * LaserLength, 22, ref point);
	}
}

