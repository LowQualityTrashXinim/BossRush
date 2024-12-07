using BossRush.Contents.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
public class ShaderData {
	public MiscShaderData _shader = null;
	public ShaderSettings shaderSettings;
	public ARenderTargetContentByRequest rt;
	public Vector2 position;
	public bool enabled = false;
	public virtual void DrawShader(ref Color lightColor) 
	{

		MiscShaderData miscShaderData = GameShaders.Misc[shaderSettings.shaderType];
		miscShaderData.UseImage1(shaderSettings.image1);
		miscShaderData.UseImage2(shaderSettings.image2);
		miscShaderData.UseShaderSpecificData(shaderSettings.shaderData);
		miscShaderData.UseColor(shaderSettings.Color);
		miscShaderData.Apply();

		rt.Request();

		if(rt.IsReady)
			Main.spriteBatch.Draw(rt.GetTarget(), position, Color.White);

	}

	public virtual void setDefaults() 
	{
		
	}


}

public class ShaderGlobalProjectile : GlobalProjectile 
{
	public override bool InstancePerEntity => true;
	public ShaderData shaderData;
	public override bool PreDraw(Projectile projectile, ref Color lightColor) {
		
		if(shaderData.enabled) 
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			shaderData.DrawShader(ref lightColor);


		}


		return base.PreDraw(projectile,ref lightColor);
	}

	public override void PostDraw(Projectile projectile, Color lightColor) {

		if (shaderData.enabled) {
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
		}

	}


}
