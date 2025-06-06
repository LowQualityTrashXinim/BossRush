﻿using BossRush.Common.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Terraria;

namespace BossRush.Common.Graphics.Structs.QuadStructs {
	public struct SparkQuad {

		private static PrimitiveDrawer QuadDrawer = new PrimitiveDrawer(PrimitiveShape.Quad);

		public void Draw(Vector2 position, float rotation, Vector2 size, ShaderSettings shaderSettings) {

			ModdedShaderHandler shader = EffectsLoader.shaderHandlers["LightSpark"];
			shader.setProperties(shaderSettings);
			
			shader.apply();


			QuadDrawer.Draw([position], [Color.White], [size], rotation, position);
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();

		}

	}
}
