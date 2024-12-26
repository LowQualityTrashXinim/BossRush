using BossRush.Texture;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria;
using BossRush.Common.Graphics;
using Terraria.GameContent;
using BossRush.Common.Graphics.Primitives;

namespace BossRush.Common.Graphics.TrailStructs;
public struct WyvernTrailMain {
	private static VertexStrip _vertexStrip = new VertexStrip();
	private static ModdedShaderHandler shader = new ModdedShaderHandler(EffectsLoader.loadedShaders[ShadersID.FlameShader].Value);
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset) {


		shader.setProperties(Color.LightSeaGreen, ModContent.Request<Texture2D>(BossRushTexture.PERLINNOISE).Value);
		shader.setupTextures();

		shader.apply();

		_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth, -Main.screenPosition + offset);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(2f, 5f, Terraria.Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Terraria.Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}
public struct WyvernTrailMini {
	private static VertexStrip _vertexStrip = new VertexStrip();
	private static ModdedShaderHandler shader = new ModdedShaderHandler(EffectsLoader.loadedShaders[ShadersID.FlameShader].Value);
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset) {

		//MiscShaderData miscShaderData = GameShaders.Misc["FlameEffect"];
		//Asset<Texture2D> NOISE = ModContent.Request<Texture2D>(BossRushTexture.PERLINNOISE);
		//miscShaderData.UseImage1(NOISE);
		//miscShaderData.UseColor(Color.LightSeaGreen);
		//miscShaderData.UseShaderSpecificData(new Microsoft.Xna.Framework.Vector4(60, 1, 0, 0));
		//miscShaderData.Apply();

		shader.setProperties(Color.LightSeaGreen, ModContent.Request<Texture2D>(BossRushTexture.PERLINNOISE).Value);
		shader.setupTextures();
		shader.apply();


		_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth, -Main.screenPosition + offset);
		_vertexStrip.DrawTrail();



		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(2f, 5f, Terraria.Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Terraria.Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}
public struct BeamTrail {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Projectile projectile, Color color, Vector2 offset) {
		ModdedShaderHandler shader = new ModdedShaderHandler(EffectsLoader.loadedShaders[ShadersID.TrailShader].Value);
		shader.setProperties(color, TextureAssets.Extra[193].Value);
		shader.setupTextures();
		shader.apply();

		_vertexStrip.PrepareStrip(projectile.oldPos,projectile.oldRot,StripColors,StripWidth,offset);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(7, 1, progressOnStrip);
}
public struct FlameThrowerFrost {
	private static VertexStrip _vertexStrip = new VertexStrip();
	private static ModdedShaderHandler shader = new ModdedShaderHandler(EffectsLoader.loadedShaders[ShadersID.FlameShader].Value);
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset, float progress, float maxProgress = 30) {

		shader.setProperties(Color.CornflowerBlue, TextureAssets.Extra[193].Value);
		shader.setupTextures();
		shader.apply();

		_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth, -Main.screenPosition + offset);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0f, 255f, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5f, 12f, Terraria.Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Terraria.Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}
public struct FlameThrowerFire {
	private static VertexStrip _vertexStrip = new VertexStrip();
	private static ModdedShaderHandler shader = new ModdedShaderHandler(EffectsLoader.loadedShaders[ShadersID.FlameShader].Value);

	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset, float progress, float maxProgress = 30) {

		//MiscShaderData miscShaderData = GameShaders.Misc["FlameEffect"];
		//miscShaderData.UseImage1("Images/Extra_" + (short)193);
		//miscShaderData.UseColor(Color.Orange);
		//miscShaderData.UseShaderSpecificData(new Vector4(progress, maxProgress, 0, 0));
		//miscShaderData.Apply();
		shader.setProperties(Color.Orange, TextureAssets.Extra[193].Value);
		shader.setupTextures();
		shader.apply();
		_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth, -Main.screenPosition + offset);
		_vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();


	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0f, 255f, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5f, 12f, Terraria.Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Terraria.Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}
public struct StarTrail {
	private static VertexStrip _vertexStrip = new VertexStrip();
	private static ModdedShaderHandler shader = new ModdedShaderHandler(EffectsLoader.loadedShaders[ShadersID.TrailShader].Value);
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset) {

		//MiscShaderData miscShaderData = GameShaders.Misc["TrailEffect"];
		//miscShaderData.UseImage1("Images/Extra_" + (short)193);
		//miscShaderData.UseColor(Color.CornflowerBlue);

		//miscShaderData.Apply();


		shader.setProperties(Color.SkyBlue, TextureAssets.Extra[193].Value);
		shader.setupTextures();
		shader.apply();

		_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth,  offset, null, true);
		_vertexStrip.DrawTrail();


		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5, 5, progressOnStrip);
}
public struct StarTrailEmpowered {
	private static VertexStrip _vertexStrip = new VertexStrip();
	private static ModdedShaderHandler shader = new ModdedShaderHandler(EffectsLoader.loadedShaders[ShadersID.TrailShader].Value);
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset) {

		shader.setProperties(Color.Gold, TextureAssets.Extra[193].Value);
		shader.setupTextures();
		shader.apply();

		_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth,offset, null, true);
		_vertexStrip.DrawTrail();


		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(12, 0, progressOnStrip);
}
