using BossRush.Texture;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BossRush.Common.Utils {
	public static partial class BossRushUtils {
		public static Rectangle GetSource(this Texture2D texture, int verticalFrames, int index) {
			int frameHeight = texture.Height / verticalFrames;
			return new Rectangle(0, (index % verticalFrames) * frameHeight, texture.Width, frameHeight);
		}
	}
}
