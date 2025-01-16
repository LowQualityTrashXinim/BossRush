using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria;
using System.Diagnostics;
using System;


namespace BossRush.Common.Graphics;
public struct TrailShaderSettings {

	public string shaderType;
	public Color Color;
	public Vector2[] oldPos;
	public float[] oldRot;
	public Asset<Texture2D> image1;
	public Asset<Texture2D> image2;
	public Vector4 shaderData;
	public Vector2 offset;
}

public struct ShaderSettings {

	public Color Color;
	public Asset<Texture2D> image1;
	public Asset<Texture2D> image2;
	public Asset<Texture2D> image3;
	public Vector4 shaderData;
}
/// <summary>
/// Keep in mind that:
/// Spritebatch automatically Sets Main.Instance.GraphicsDevice.Textures[0] to the texture its currently drawing in the batch (when calling Draw() for immediate mode and End() for other modes),
/// and if you want to modify Main.Instance.GraphicsDevice.Textures, for things like vertex buffers, you would do it while spritebatch is not active (before Begin() or after End()),
/// </summary>
public class ModdedShaderHandler : IDisposable {
	static GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice;
	Effect _effect;
	Color _color = new Color(0, 0, 0);
	Texture2D _texutre1 = null;
	Texture2D _texutre2 = null;
	Texture2D _texutre3 = null;
	Vector4 _shaderData = new Vector4(0, 0, 0, 0);
	public bool enabled = false;
	public ModdedShaderHandler(Effect effect) {

		this._effect = effect;

	}
	public void setProperties(Color color, Texture2D texutre1 = null, Texture2D texutre2 = null, Texture2D texutre3 = null, Vector4 shaderData = default) {
		this._color = color;
		this._texutre1 = texutre1;
		this._texutre2 = texutre2;
		this._texutre3 = texutre3;
		this._shaderData = shaderData;
	}
	public void setProperties(ShaderSettings shaderSettings) {
		this._color = shaderSettings.Color;
		this._texutre1 = shaderSettings.image1.Value;
		this._texutre2 = shaderSettings.image2.Value;
		this._texutre3 = shaderSettings.image3.Value;
		this._shaderData = shaderSettings.shaderData;
	}
	/// <summary>
	/// call this before Begin() or after End() 
	/// </summary>
	public void setupTextures() 
	{

		if (_texutre1 != null) {
			GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
			GraphicsDevice.Textures[1] = _texutre1;
		}
		if (_texutre2 != null) {
			GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;
			GraphicsDevice.Textures[2] = _texutre2;
		}
		if (_texutre3 != null) {
			GraphicsDevice.SamplerStates[3] = SamplerState.LinearWrap;
			GraphicsDevice.Textures[3] = _texutre3;
		}
	}
	public void apply() {
		var viewport = GraphicsDevice.Viewport;
		setupTextures();
		_effect.Parameters["viewWorldProjection"].SetValue(Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix * Matrix.CreateOrthographicOffCenter(left: 0, right: viewport.Width, bottom: viewport.Height, top: 0, zNearPlane: -1, zFarPlane: 10));
		_effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly);
		_effect.Parameters["color"].SetValue(_color.ToVector3());
		_effect.Parameters["shaderData"].SetValue(_shaderData);
		_effect.CurrentTechnique.Passes[0].Apply();
		
	}

	public void Dispose() {
		_effect?.Dispose();
	}
}
