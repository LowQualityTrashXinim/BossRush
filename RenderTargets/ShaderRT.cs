using BossRush.Contents.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace BossRush.RenderTargets;
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

public class SRT128x128 : ShaderRT 
{

}

public class RTLoaderAndUnloader : ModSystem 
{

	
	public static ShaderRT rt;
	public override void Load() {
		Main.ContentThatNeedsRenderTargets.Add(rt = new ShaderRT());
	}

	public override void Unload() {
		Main.ContentThatNeedsRenderTargets.Remove(rt);
	}


}
