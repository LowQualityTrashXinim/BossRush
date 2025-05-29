using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria;
using ReLogic.Peripherals.RGB;

namespace BossRush {
	public static partial class BossRushUtils {
		public static Rectangle GetSource(this Texture2D texture, int verticalFrames, int index) {
			int frameHeight = texture.Height / verticalFrames;
			return new Rectangle(0, (index % verticalFrames) * frameHeight, texture.Width, frameHeight);
		}
		public static string GetTheSameTextureAsEntity<T>() where T : class {
			var type = typeof(T);
			string NameSpace = type.Namespace;
			if (NameSpace == null) {
				return BossRushTexture.MissingTexture_Default;
			}
			return NameSpace.Replace(".", "/") + "/" + type.Name;
		}
		public static string GetTheSameTextureAs<T>(string altName = "") where T : class {
			var type = typeof(T);
			if (string.IsNullOrEmpty(altName)) {
				altName = type.Name;
			}
			string NameSpace = type.Namespace;
			if (NameSpace == null) {
				return BossRushTexture.MissingTexture_Default;
			}
			return NameSpace.Replace(".", "/") + "/" + altName;
		}
		public static float Scale_OuterTextureWithInnerTexture(Vector2 size1, Vector2 size2, float adjustment) => size1.Length() / size2.Length() * adjustment;
		public static string GetVanillaTexture<T>(int EntityType) where T : class => $"Terraria/Images/{typeof(T).Name}_{EntityType}";
	}
}
