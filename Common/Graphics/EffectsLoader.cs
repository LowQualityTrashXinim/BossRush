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

	public Asset<Effect> flameBall;
	public Asset<Effect> trailEffect;
	public Asset<Effect> flameEffect;
	public Asset<Effect> primitiveFlameBall;
	public Asset<Effect> primitiveExplosion;

	public HashSet<Asset<Effect>> toUnload;
	public static readonly bool dontLoad = false;

	public override void Load() {

		if (dontLoad)
			return;

		if (Main.netMode != NetmodeID.Server) {

			#region Custom Shaders

			// FNA dosent like loading custom shaders when outside main thread for some reason lol
			Main.RunOnMainThread(() => {

				trailEffect = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/TrailEffect");
				flameEffect = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameEffect");
				flameBall = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBall");
				primitiveFlameBall = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/FlameBallPrimitive");
				primitiveExplosion = ModContent.Request<Effect>("BossRush/Common/Graphics/Shaders/ExplosionPrimitive");

			}).Wait();

			#endregion

		}
	}
}


