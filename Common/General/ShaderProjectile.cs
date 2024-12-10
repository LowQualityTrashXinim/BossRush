﻿using BossRush.Common.Graphics;
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
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.General;
public struct ModShaderData {
	public MiscShaderData _shader = null;
	public ShaderSettings shaderSettings = new ShaderSettings();
	//public ShaderRT rt;
	public Vector2 position;
	public bool enabled = false;

	public ModShaderData() {
	}

	public void ActivateShader(ref Color lightColor) {

		_shader = GameShaders.Misc[shaderSettings.shaderType];
		_shader.UseImage1(shaderSettings.image1);
		_shader.UseImage2(shaderSettings.image2);
		_shader.UseShaderSpecificData(shaderSettings.shaderData);
		_shader.UseColor(shaderSettings.Color);
		_shader.Apply();

		//rt.Request();
		//if (rt.IsReady)
		//	Main.spriteBatch.Draw(rt.GetTarget(), position - Main.screenPosition, null, Color.White, 0, rt.GetTarget().Size() / 2f, 1f, SpriteEffects.None, 0f);

	}
}

public interface IDrawsShader {

	public abstract void updateShader();
	public abstract void PreShaderDraw(ref Color lightcolor);

}

public class ShaderGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;
	public ModShaderData shaderData = new ModShaderData();
	public override bool PreDraw(Projectile projectile, ref Color lightColor) {



		if (shaderData.enabled) {
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			if (projectile.ModProjectile is IDrawsShader) {

				var proj = projectile.ModProjectile as IDrawsShader;
				proj.PreShaderDraw(ref lightColor);
			}

			shaderData.ActivateShader(ref lightColor);


		}


		return true;
	}

	public override void PostDraw(Projectile projectile, Color lightColor) {

		if (shaderData.enabled) {
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
		}

		base.PostDraw(projectile, lightColor);

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
