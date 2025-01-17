using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics;
public class EffectsLoader : ModSystem {

	public static Dictionary<string,Asset<Effect>> loadedShaders = new();
	public static readonly bool dontLoad = false;

	public override void Load() {

		if (dontLoad)
			return;

		if (Main.netMode != NetmodeID.Server) {

			#region Custom Shaders

			// FNA dosent like loading custom shaders when outside main thread for some reason lol
			Main.RunOnMainThread(() => {

				loadedShaders[ShadersID.TrailShader] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/TrailEffect");
				loadedShaders[ShadersID.FlameShader] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameEffect");
				loadedShaders[ShadersID.FlameBallShader] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBall");
				loadedShaders["FlameBallPrimitive"] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBallPrimitive");
				loadedShaders["ExplosionPrimitive"] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/ExplosionPrimitive");

			}).Wait();

			#endregion

		}

	}

	public override void Unload() {
		Main.RunOnMainThread(() => {

			for(int i = 0; i < loadedShaders.Count; i++) 
			{

				loadedShaders.ElementAt(i).Value.Dispose();
			}

		}).Wait();
		loadedShaders = null;

	}
}


