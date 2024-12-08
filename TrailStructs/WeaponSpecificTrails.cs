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

namespace BossRush.TrailStructs;
public static class WeaponSpecificTrails {

}
public struct WyvernTrailMain {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset) {

		MiscShaderData miscShaderData = GameShaders.Misc["TrailEffect"];
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.UseColor(Color.LightSeaGreen);

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
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(2f, 5f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}

public struct WyvernTrailMini {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset) {

		MiscShaderData miscShaderData = GameShaders.Misc["FlameEffect"];
		Asset<Texture2D> NOISE = ModContent.Request<Texture2D>(BossRushTexture.PERLINNOISE);
		miscShaderData.UseImage1(NOISE);
		miscShaderData.UseColor(Color.LightSeaGreen);
		miscShaderData.UseShaderSpecificData(new Microsoft.Xna.Framework.Vector4(60, 1, 0, 0));
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
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(2f, 5f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}

public struct BeamTrail {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Projectile projectile, Color color) {

		MiscShaderData miscShaderData = GameShaders.Misc["TrailEffect"];
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.UseColor(color);

		miscShaderData.Apply();

		_vertexStrip.PrepareStrip(projectile.oldPos, projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + projectile.Size * 0.5f);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(4, 1, progressOnStrip);
}
public struct FlameThrowerCombined {
	private static VertexStrip _vertexStrip = new VertexStrip();
	private float maxProgress = 0;
	private float progress = 0;
	public FlameThrowerCombined() {
	}

	public void Draw(Projectile projectile, float counter) {

		MiscShaderData miscShaderData = GameShaders.Misc["FlameEffect"];
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.UseColor(Color.Pink);
		miscShaderData.UseShaderSpecificData(new Vector4(60, 1, 0, 0));
		this.progress = Utils.GetLerpValue(0, 1, counter / 20, true);
		miscShaderData.Apply();

		_vertexStrip.PrepareStrip(projectile.oldPos, projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + projectile.Size * 0.5f);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0f, 255f, progressOnStrip * Utils.GetLerpValue(0, 1, progress / 10, true)));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(8f, 35f * progress, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}
public struct FlameThrowerFrost {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset, float progress, float maxProgress = 30) {

		MiscShaderData miscShaderData = GameShaders.Misc["FlameEffect"];
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.UseColor(Color.CornflowerBlue);
		miscShaderData.UseShaderSpecificData(new Vector4(progress, maxProgress, 0, 0));

		miscShaderData.Apply();

		_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth, -Main.screenPosition + offset);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0f, 255f, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5f, 12f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}
public struct FlameThrowerFire {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset, float progress, float maxProgress = 30) {

		MiscShaderData miscShaderData = GameShaders.Misc["FlameEffect"];
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.UseColor(Color.Orange);
		miscShaderData.UseShaderSpecificData(new Vector4(progress, maxProgress, 0, 0));
		miscShaderData.Apply();

		_vertexStrip.PrepareStrip(oldPos, oldRot, StripColors, StripWidth, -Main.screenPosition + offset);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0f, 255f, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5f, 12f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
}

public struct StarTrail {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset) {

		MiscShaderData miscShaderData = GameShaders.Misc["TrailEffect"];
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.UseColor(Color.CornflowerBlue);

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
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(35, 5, progressOnStrip);
}
public struct StarTrailEmpowered {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Vector2[] oldPos, float[] oldRot, Vector2 offset) {

		MiscShaderData miscShaderData = GameShaders.Misc["TrailEffect"];
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.UseColor(Color.Gold);

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
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(12, 0, progressOnStrip);
}
