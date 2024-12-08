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
public class RT128x128 : ARenderTargetContentByRequest {

	protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch) {
		PrepareARenderTarget_AndListenToEvents(ref _target, device, 128, 128, RenderTargetUsage.PreserveContents);
		device.SetRenderTarget(_target);
		device.Clear(Color.Transparent);
		device.SetRenderTarget(null);
		_wasPrepared = true;
		
	}
}

public class RT128x128LoaderAndUnloader : ModSystem 
{

	
	public static RT128x128 rt;
	public override void Load() {
		Main.ContentThatNeedsRenderTargets.Add(rt = new RT128x128());
	}

	public override void Unload() {
		Main.ContentThatNeedsRenderTargets.Remove(rt);
	}


}
