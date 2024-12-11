using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace BossRush.Common.Graphics.Primitives;
public abstract class PrimitivesBase {

	protected static GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice;
	public ModdedShaderHandler shaderHandler = null;
	public abstract void DrawPrimitive(Effect effect, Vector2 position, float Size);


}

