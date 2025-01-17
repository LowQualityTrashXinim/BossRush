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
using System.Diagnostics;

namespace BossRush.Common.Graphics;
internal class EffectsLoader : ModSystem {

	//public static Dictionary<string,Asset<Effect>> loadedShaders = new();
	public static Dictionary<string, ModdedShaderHandler> shaderHandlers = new();
	public static readonly bool dontLoad = false;
	private const string EffectsFolderPath = "Common/Graphics/Shaders/";

	public override void Load() {

		if (dontLoad)
			return;

		if (Main.netMode != NetmodeID.Server) {

			#region Custom Shaders

			// FNA dosent like loading custom shaders when outside main thread for some reason lol
			Main.RunOnMainThread(() => {

				//loadedShaders[ShadersID.TrailShader] = ModContent.Request<Effect>("TrailEffect");
				//loadedShaders[ShadersID.FlameShader] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameEffect");
				//loadedShaders[ShadersID.FlameBallShader] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBall");
				//loadedShaders["FlameBallPrimitive"] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBallPrimitive");
				//loadedShaders["ExplosionPrimitive"] = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/ExplosionPrimitive");

				foreach(string path in Mod.GetFileNames()) 
				{

					if (!path.StartsWith(EffectsFolderPath) || !path.EndsWith(".xnb"))
						continue;

					StringBuilder sb = new StringBuilder();
					sb.Append(path);
					sb.Remove(0,EffectsFolderPath.Length);
					sb.Replace(".xnb","");
					string effectName = sb.ToString();
					//loadedShaders[effectName] = ModContent.Request<Effect>(EffectsFolderPath + effectName);
					shaderHandlers[effectName] = new ModdedShaderHandler(ModContent.Request<Effect>(Mod.Name + "/" + EffectsFolderPath + effectName));
				}
				Debug.WriteLine(shaderHandlers.ToString());

			}).Wait();

			#endregion

		}
	}

	//public override void Unload() {
	//	for (int i = 0; i < shaderHandlers.Count; i++) 
	//	{
	//		shaderHandlers.ElementAt(i).Value.Unload();
	//	}
	//}
}


