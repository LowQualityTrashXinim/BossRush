using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		device.SetRenderTarget(_target);
		device.Clear(Color.Transparent);
		device.SetRenderTarget(null);
		_wasPrepared = true;

	}
}

public class SRT128x128 : ShaderRT {

}

public class RTLoaderAndUnloader : ModSystem {


	public static SRT128x128 rt128X128;
	public override void Load() {
		Main.ContentThatNeedsRenderTargets.Add(rt128X128 = new SRT128x128());
	}

	public override void Unload() {
		Main.ContentThatNeedsRenderTargets.Remove(rt128X128);
	}
}
