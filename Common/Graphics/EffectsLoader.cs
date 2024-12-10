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
internal class EffectsLoader : ModSystem {

	public static Asset<Effect> flameBall;
	public static Asset<Effect> trailEffect;
	public static Asset<Effect> flameEffect;
	public static Asset<Effect> primitiveFlameBall;
	public static HashSet<Asset<Effect>> toUnload;

	public override void Load() {

		if (Main.netMode != NetmodeID.Server) {

			#region Modded MiscShaders

			trailEffect = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/TrailEffect");
			GameShaders.Misc[MiscShadersID.TrailShader] = new MiscShaderData(trailEffect, "FadeTrail");

			flameEffect = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameEffect");
			GameShaders.Misc[MiscShadersID.FlameShader] = new MiscShaderData(flameEffect, "FlamethrowerFlame");

			flameBall = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBall");
			GameShaders.Misc[MiscShadersID.FlameBallShader] = new MiscShaderData(flameBall, "ballOfire");

			#endregion


			#region Primitive Drawing Shaders

			// FNA dosent like loading custom 
			Main.RunOnMainThread(() => {

				primitiveFlameBall = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBallPrimitiveTest");

			}).Wait();
			#endregion

		}
	}

	public override void Unload() {

	
	}
}


