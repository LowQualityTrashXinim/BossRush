using System;
using Terraria;
using System.Linq;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Utilities;

namespace BossRush {
	public static partial class BossRushUtils {
		public static string GetTheSameTextureAsEntity<T>() where T : class {
			var type = typeof(T);
			string NameSpace = type.Namespace;
			if (NameSpace == null) {
				return BossRushTexture.MISSINGTEXTURE;
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
				return BossRushTexture.MISSINGTEXTURE;
			}
			return NameSpace.Replace(".", "/") + "/" + altName;
		}
		public static string GetVanillaTexture<T>(int EntityType) where T : class => $"Terraria/Images/{typeof(T).Name}_{EntityType}";
		public static bool IsAnyVanillaBossAlive() {
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (npc.boss && npc.active) {
					return true;
				}
				else if ((npc.type == NPCID.EaterofWorldsBody
					|| npc.type == NPCID.EaterofWorldsHead
					|| npc.type == NPCID.EaterofWorldsTail)
					&& npc.active) {
					return true;
				}
			}
			return false;
		}
		public static int AmountOfModCurrentlyEnable() {
			int HowManyModIsEnable = ModLoader.Mods.Length;
			if (Main.LocalPlayer.IsDebugPlayer()) {
				return 1;
			}
			//Below are special mod that we partner or will later on partner, currently all these mod are the mod that I'm working with.
			//By all mean, do not speak of this to outsider, only announce or say that we don't want to do mod addon.
			//Also hardcode this shit so that people have a hard time IL this stuff
			if (ModLoader.HasMod("Malignant"))
				HowManyModIsEnable--;
			if (ModLoader.HasMod("TheSkeletronMod"))
				HowManyModIsEnable--;
			return HowManyModIsEnable;
		}
		public static int NextFromHashSet(this UnifiedRandom r, HashSet<int> hashset) {
			return hashset.ElementAt(r.Next(hashset.Count));
		}
		/// <summary>
		/// Spawn combat text above player without the random Y position
		/// </summary>
		/// <param name="location">player hitbox</param>
		/// <param name="color"></param>
		/// <param name="combatMessage"></param>
		/// <param name="offsetposY"></param>
		/// <param name="dramatic"></param>
		/// <param name="dot"></param>
		public static void CombatTextRevamp(Rectangle location, Color color, string combatMessage, int offsetposY = 0, int timeleft = 30, bool dramatic = false, bool dot = false) {
			int drama = 0;
			if (dramatic) {
				drama = 1;
			}
			int text = CombatText.NewText(new Rectangle(), color, combatMessage, dramatic, dot);
			CombatText cbtext = Main.combatText[text];
			Vector2 vector = FontAssets.CombatText[drama].Value.MeasureString(cbtext.text);
			cbtext.position.X = location.X + location.Width * 0.5f - vector.X * 0.5f;
			cbtext.position.Y = location.Y + offsetposY + location.Height * 0.25f - vector.Y * 0.5f;
			cbtext.lifeTime += timeleft;
		}
		/// <summary>
		/// Use to order 2 values from smallest to biggest
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static (int, int) Order(float v1, float v2) => v1 < v2 ? ((int)v1, (int)v2) : ((int)v2, (int)v1);
		/// <summary>
		/// Check if there any NPC that is within radius 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public static bool LookForAnyHostileNPC(this Vector2 position, float distance) {
			for (int i = 0; i < Main.maxNPCs; i++) {
				if (Main.npc[i].active && Main.npc[i].friendly) {
					if (CompareSquareFloatValue(position, Main.npc[i].Center, distance * distance)) return true;
				}
			}
			return false;
		}
		public static Vector2 LookForHostileNPCPositionClosest(this Vector2 position, float distance) {
			Vector2 hostilePos = Vector2.Zero;
			float maxDistanceSquare = distance * distance;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (Main.npc[i].active
					&& CompareSquareFloatValue(npc.Center, position, maxDistanceSquare, out float dis)
					&& npc.CanBeChasedBy()
					&& !npc.friendly
					&& (Collision.CanHitLine(position, 10, 10, npc.position, npc.width, npc.height))
					) {
					maxDistanceSquare = dis;
					hostilePos = npc.Center;
				}
			}
			return hostilePos;
		}
		public static bool LookForHostileNPC(this Vector2 position, out NPC npc, float distance, bool CanLookThroughTile = false) {
			float maxDistanceSquare = distance * distance;
			npc = null;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC mainnpc = Main.npc[i];
				if (mainnpc.active
					&& CompareSquareFloatValue(mainnpc.Center, position, maxDistanceSquare, out float dis)
					&& mainnpc.CanBeChasedBy()
					&& !mainnpc.friendly
					&& (Collision.CanHitLine(position, 10, 10, mainnpc.position, mainnpc.width, mainnpc.height) || !CanLookThroughTile)
					) {
					maxDistanceSquare = dis;
					npc = mainnpc;
				}
			}
			return npc != null;
		}
		public static bool LookForHostileNPCNotImmune(this Vector2 position, out NPC npc, float distance, int whoAmI, bool CanLookThroughTile = false) {
			float maxDistanceSquare = distance * distance;
			npc = null;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC mainnpc = Main.npc[i];
				if (mainnpc.active
					&& CompareSquareFloatValue(mainnpc.Center, position, maxDistanceSquare, out float dis)
					&& mainnpc.CanBeChasedBy()
					&& !mainnpc.friendly
					&& (Collision.CanHitLine(position, 10, 10, mainnpc.position, mainnpc.width, mainnpc.height) || !CanLookThroughTile)
					&& mainnpc.immune[whoAmI] <= 0
					) {
					maxDistanceSquare = dis;
					npc = mainnpc;
				}
			}
			return npc != null;
		}
		public static void LookForHostileNPC(this Vector2 position, out List<NPC> npc, float distance) {
			npc = new List<NPC>();
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC Npc = Main.npc[i];
				if (Npc.active && Npc.CanBeChasedBy() && Npc.type != NPCID.TargetDummy && !Npc.friendly && CompareSquareFloatValueWithHitbox(position, Npc.position, Npc.Hitbox, distance))
					npc.Add(Npc);
			}
		}
		public static int ToMinute(float minute) => (int)(ToSecond(60) * minute);
		public static int ToSecond(float second) => (int)(second * 60);
		public static int ToIntValue(this StatModifier modifier) => (int)(modifier.ApplyTo(1));
		public static float ToFloatValue(this StatModifier modifier, float additionalMulti = 1, int round = -1) 
			=> round == -1 ? modifier.ApplyTo(1) * additionalMulti : MathF.Round(modifier.ApplyTo(1) * additionalMulti, round);
		public static float InExpo(float t) => (float)Math.Pow(2, 5 * (t - 1));
		public static float OutExpo(float t) => 1 - InExpo(1 - t);
		public static float InOutExpo(float t) {
			if (t < 0.5) return InExpo(t * 2) * .5f;
			return 1 - InExpo((1 - t) * 2) * .5f;
		}

		public static float InExpo(float t, float strength) => (float)Math.Pow(2, strength * (t - 1));
		public static float OutExpo(float t, float strength) => 1 - InExpo(1 - t, strength);
		public static float InOutExpo(float t, float strength) {
			if (t < 0.5) return InExpo(t * 2, strength) * .5f;
			return 1 - InExpo((1 - t) * 2, strength) * .5f;
		}
		public static float InSine(float t) => (float)-Math.Cos(t * MathHelper.PiOver2);
		public static float OutSine(float t) => (float)Math.Sin(t * MathHelper.PiOver2);
		public static float InOutSine(float t) => (float)(Math.Cos(t * Math.PI) - 1) * -.5f;
		public static float InBack(float t) {
			float s = 1.70158f;
			return t * t * ((s + 1) * t - s);
		}
		public static float OutBack(float t) => 1 - InBack(1 - t);
		public static float InOutBack(float t) {
			if (t < 0.5) return InBack(t * 2) * .5f;
			return 1 - InBack((1 - t) * 2) * .5f;
		}
		/// <summary>
		/// Calculate square length of Vector2 and check if it is smaller than square max distance
		/// This won't power max distance by 2 so do it yourself
		/// </summary>
		/// <param name="pos1"></param>
		/// <param name="pos2"></param>
		/// <param name="maxDistance"></param>
		/// <returns>
		/// Return true if length of Vector2 smaller than max distance<br/>
		/// Return false if length of Vector2 greater than max distance
		/// </returns>
		public static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float maxDistance) {
			double value1X = pos1.X,
				value1Y = pos1.Y,
				value2X = pos2.X,
				value2Y = pos2.Y,
				DistanceX = value1X - value2X,
				DistanceY = value1Y - value2Y;
			return (DistanceX * DistanceX + DistanceY * DistanceY) < maxDistance;
		}
		/// <summary>
		/// Calculate square length of Vector2 and check if it is smaller than square max distance
		/// This won't power max distance by 2 so do it yourself
		/// </summary>
		/// <param name="pos1"></param>
		/// <param name="pos2"></param>
		/// <param name="maxDistance"></param>
		/// <returns>
		/// Return true if length of Vector2 smaller than max distance<br/>
		/// Return false if length of Vector2 greater than max distance
		/// </returns>
		public static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float maxDistance, out float distance) {
			float
				DistanceX = pos1.X - pos2.X,
				DistanceY = pos1.Y - pos2.Y;
			distance = (DistanceX * DistanceX + DistanceY * DistanceY);
			return distance < maxDistance;
		}
		public static bool CompareSquareFloatValueWithHitbox(Vector2 position, Vector2 positionEntity, Rectangle hitboxEntity, float maxDis) {
			float maxDistanceDouble = maxDis * maxDis;

			float disX = position.X - positionEntity.X;
			float disXWidth = disX - hitboxEntity.Width;
			float disXhalfWidth = disX - hitboxEntity.Width * .5f;

			float disY = position.Y - positionEntity.Y;
			float disYHeight = disY - hitboxEntity.Height;
			float disYhalfHeight = disY - hitboxEntity.Height * .5f;
			//|-|
			//0-|
			//|-|
			if (disX * disX + disYhalfHeight * disYhalfHeight < maxDistanceDouble)
				return true;
			//|O|
			//|-|
			//|-|
			if (disXhalfWidth * disXhalfWidth + disY * disY < maxDistanceDouble)
				return true;
			//|-|
			//|-O
			//|-|
			if (disXWidth * disXWidth + disYhalfHeight * disYhalfHeight < maxDistanceDouble)
				return true;
			//|-|
			//|-|
			//|O|
			if (disXhalfWidth * disXhalfWidth + disYHeight * disYHeight < maxDistanceDouble)
				return true;
			//0-|
			//|-|
			//|-|
			if (disX * disX + disY * disY < maxDistanceDouble)
				return true;
			//|-0
			//|-|
			//|-|
			if (disXWidth * disXWidth + disY * disY < maxDistanceDouble)
				return true;
			//|-|
			//|-|
			//0-|
			if (disX * disX + disYHeight * disYHeight < maxDistanceDouble)
				return true;
			//|-|
			//|-|
			//|-0
			if (disXWidth * disXWidth + disYHeight * disYHeight < maxDistanceDouble)
				return true;
			return false;
		}
	}
	/// <summary>
	/// Use this to set up your own logic for multi color changing effect, could done this with shader but well
	/// </summary>
	public class ColorInfo {
		public ColorInfo(List<Color> colorlist) {
			listcolor = colorlist;
		}
		int currentIndex = 0, progress = 0;
		Color color1 = new Color(), color2 = new Color(), color3 = new Color();
		List<Color> listcolor = new List<Color>();

		public Color MultiColor(int speed) {
			if (progress >= 255)
				progress = 0;
			else
				progress = Math.Clamp(progress + 1 * speed, 0, 255);

			if (listcolor.Count < 1)
				return Color.White;

			if (listcolor.Count < 2)
				return listcolor[0];

			if (color1.Equals(color2)) {
				color1 = listcolor[currentIndex];
				color3 = listcolor[currentIndex];
				currentIndex = Math.Clamp((currentIndex + 1 >= listcolor.Count) ? 0 : currentIndex + 1, 0, listcolor.Count - 1);
				color2 = listcolor[currentIndex];
				progress = 0;
			}

			if (!color1.Equals(color2))
				color1 = Color.Lerp(color3, color2, Math.Clamp(progress / 255f, 0, 1f));

			return color1;
		}
	}
}
