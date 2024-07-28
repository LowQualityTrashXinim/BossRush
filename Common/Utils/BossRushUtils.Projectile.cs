using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush {
	public static partial class BossRushUtils {
		public static void SpawnBoulderOnTopPlayer(Player player, float range, bool Randomize = true) {
			float RandomizeX = Randomize ? Main.rand.NextFloat(-range, range) : range;
			Vector2 spawn = new Vector2(RandomizeX + player.Center.X, player.Center.Y - 1000);
			int projectile = Projectile.NewProjectile(null, spawn, Vector2.Zero, ProjectileID.Boulder, 400, 10f);
			Main.projectile[projectile].hostile = true;
			Main.projectile[projectile].friendly = false;
		}
		public static void SpawnHostileProjectile(Vector2 position, Vector2 velocity, int ProjectileType, int damage, float knockback) {
			int projectile = Projectile.NewProjectile(null, position, velocity, ProjectileType, damage, knockback);
			Main.projectile[projectile].hostile = true;
			Main.projectile[projectile].friendly = false;
		}
		public static int SpawnHostileProjectileDirectlyOnPlayer(Player player, float rangeX, float rangeY, bool randomizePosition, Vector2 velocity, int ProjectileType, int damage, float knockback) {
			float RandomizeX = randomizePosition ? Main.rand.NextFloat(-rangeX, rangeX) : rangeX;
			float RandomizeY = randomizePosition ? Main.rand.NextFloat(-rangeY, rangeY) : rangeY;
			Vector2 spawn = new Vector2(player.Center.X + RandomizeX, player.Center.Y - 1000 + RandomizeY);
			int projectile = Projectile.NewProjectile(null, spawn, velocity, ProjectileType, damage, knockback);
			Main.projectile[projectile].hostile = true;
			Main.projectile[projectile].friendly = false;
			return projectile;
		}
		public static void ProjectileDefaultDrawInfo(this Projectile projectile, out Texture2D texture, out Vector2 origin) {
			Main.instance.LoadProjectile(projectile.type);
			texture = TextureAssets.Projectile[projectile.type].Value;
			origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
		}
		public static void DrawTrail(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0) {
			projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
			if (ProjectileID.Sets.TrailingMode[projectile.type] != 2) {
				for (int k = 0; k < projectile.oldPos.Length; k++) {
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
				}
			}
			else {
				for (int k = 0; k < projectile.oldPos.Length; k++) {
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.oldRot[k], origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
				}
			}
		}
		public static void DrawTrailWithoutColorAdjustment(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0) {
			projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
			if (ProjectileID.Sets.TrailingMode[projectile.type] != 2) {
				for (int k = 0; k < projectile.oldPos.Length; k++) {
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
					Main.EntitySpriteDraw(texture, drawPos, null, lightColor, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
				}
			}
			else {
				for (int k = 0; k < projectile.oldPos.Length; k++) {
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
					Main.EntitySpriteDraw(texture, drawPos, null, lightColor, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
				}
			}
		}
		public static void ProjectileAlphaDecay(this Projectile projectile, float timeCountdown) {
			projectile.alpha = (int)MathHelper.Lerp(0, 255, (timeCountdown - projectile.timeLeft) / timeCountdown);
		}
		public static void BresenhamCircle(Vector2 pos, int radius, Color color) {
			Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.WHITEDOT).Value;
			int x = 0, y = radius;
			int decesionParameter = 3 - 2 * radius;
			DrawBresenhamCircle(texture, pos, x, y, color);
			while (y >= x) {
				x++;
				if (decesionParameter > 0) {
					y--;
					decesionParameter = decesionParameter + 4 * (x - y) + 10;
				}
				else {
					decesionParameter = decesionParameter + 4 * x + 6;//This is magic math value, don't ask me
				}
				DrawBresenhamCircle(texture, pos, x, y, color);
			}
		}
		private static void DrawBresenhamCircle(Texture2D texture, Vector2 c, int x, int y, Color color) {
			Vector2 drawPos = c - Main.screenPosition;
			Main.spriteBatch.Draw(texture, drawPos.Subtract(x, y), null, color, 0, Vector2.One, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, drawPos.Subtract(-x, y), null, color, 0, Vector2.One, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, drawPos.Subtract(x, -y), null, color, 0, Vector2.One, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, drawPos.Subtract(-x, -y), null, color, 0, Vector2.One, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, drawPos.Subtract(y, x), null, color, 0, Vector2.One, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, drawPos.Subtract(-y, x), null, color, 0, Vector2.One, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, drawPos.Subtract(y, -x), null, color, 0, Vector2.One, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, drawPos.Subtract(-y, -x), null, color, 0, Vector2.One, 1, SpriteEffects.None, 0);
		}
	}
}
