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
public class EffectsLoader : ModSystem {

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

				foreach(string path in Mod.GetFileNames()) 
				{

					if (!path.StartsWith(EffectsFolderPath) || !path.EndsWith(".xnb"))
						continue;

					StringBuilder sb = new StringBuilder();
					sb.Append(path);
					sb.Remove(0,EffectsFolderPath.Length);
					sb.Replace(".xnb","");
					string effectName = sb.ToString();
					shaderHandlers[effectName] = new ModdedShaderHandler(ModContent.Request<Effect>(Mod.Name + "/" + EffectsFolderPath + effectName));
				}

			}).Wait();

			#endregion

		}
	}
}


