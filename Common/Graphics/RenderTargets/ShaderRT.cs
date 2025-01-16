using BossRush.Common.Graphics.Primitives;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics.RenderTargets;
public class ShaderRT : ARenderTargetContentByRequest {

	public virtual int width => 128;
	public virtual int height => 128;

	protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch) {

		PrepareARenderTarget_AndListenToEvents(ref _target, device, width, height, RenderTargetUsage.PreserveContents);
		var oldTargets = device.GetRenderTargets();
		device.SetRenderTarget(_target);
		device.Clear(Color.Transparent);
		Draw(device, spriteBatch);
		device.SetRenderTarget(null);
		_wasPrepared = true;

		device.SetRenderTargets(oldTargets);

	}

	public virtual void Draw(GraphicsDevice device, SpriteBatch spriteBatch) 
	{
	
	}
}

//public class NovaRT : ShaderRT {

//	public Color color = Color.White;
//	public float projectileAi0 = 0;
//	public float projectileAi1 = 0;
//	public float projectileAi2 = 0;
//	public float shaderOffset = 0;

//	public override void Draw(GraphicsDevice device, SpriteBatch spriteBatch) {
//		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

//		ModdedShaderHandler shader = new ModdedShaderHandler(EffectsLoader.loadedShaders["FlameBallPrimitive"].Value);
//		shader.setupTextures();
//		shader.setProperties(color, TextureAssets.Extra[193].Value, shaderData: new Vector4(projectileAi0, projectileAi1, projectileAi2, shaderOffset));
//		shader.apply();

//		PrimitivesDrawer.NewQuad(Vector2.One,Color.White,new Vector2(128));

		
//		spriteBatch.End();
//	}

//}


