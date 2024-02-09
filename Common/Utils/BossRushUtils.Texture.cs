using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Common.Utils {
	public static partial class BossRushUtils {
		public static Rectangle GetSource(this Texture2D texture, int verticalFrames, int index) {
			int frameHeight = texture.Height / verticalFrames;
			return new Rectangle(0, (index % verticalFrames) * frameHeight, texture.Width, frameHeight);
		}
	}
}
