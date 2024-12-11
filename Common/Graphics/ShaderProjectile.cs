using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.RGB;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics;

/// <summary>
/// Due to how vanilla code works, the sprite that the vanilla drawing code draws will have the shader applied to it,
/// Return false in the preDraw() method if you dont want this effect
/// </summary>
public interface IDrawsShader {
	
	public abstract void updateShader();
	public abstract void preDrawWithoutShader(ref Color lightcolor);
	public abstract void postDrawWithoutShader(Color lightcolor);

}

public class ShaderGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;
	public ModdedShaderHandler shader = null;
	public override bool PreDraw(Projectile projectile, ref Color lightColor) {
		if (shader != null && shader.enabled) {
			if (projectile.ModProjectile is IDrawsShader) {

				var proj = projectile.ModProjectile as IDrawsShader;
				proj.preDrawWithoutShader(ref lightColor);
			}
			Main.spriteBatch.End();
			shader.setupTextures();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			shader.apply();

		}
		return true;
	}

	public override void PostDraw(Projectile projectile, Color lightColor) {




		if (shader != null && shader.enabled) {
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
		}

		if (projectile.ModProjectile is IDrawsShader) {

			var proj = projectile.ModProjectile as IDrawsShader;
			proj.postDrawWithoutShader(lightColor);
		}


	}

	public override void AI(Projectile projectile) {
		updateProjectileShader(projectile);
		base.AI(projectile);
	}

	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		updateProjectileShader(projectile);
		base.OnSpawn(projectile, source);
	}

	public void updateProjectileShader(Projectile projectile) {
		if (projectile.ModProjectile is IDrawsShader) {

			var proj = projectile.ModProjectile as IDrawsShader;
			proj.updateShader();
		}
	}
}
