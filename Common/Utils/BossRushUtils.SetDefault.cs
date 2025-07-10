using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Common.General;
using BossRush.Common.Systems;
using System.Diagnostics;
using System.Threading.Channels;
using BossRush.Contents.Transfixion.Arguments;
using BossRush.Common.RoguelikeChange.Mechanic;

namespace BossRush {
	public static partial class BossRushUtils {
		public static void BossRushSetDefaultBuff(this ModBuff buff) {
			Main.debuff[buff.Type] = false;
			Main.buffNoSave[buff.Type] = true;
		}
		public static void BossRushSetDefaultDeBuff(this ModBuff buff, bool Save = false, bool Cure = false) {
			Main.debuff[buff.Type] = true;
			Main.buffNoSave[buff.Type] = Save;
			BuffID.Sets.NurseCannotRemoveDebuff[buff.Type] = Cure;
		}
		/// <summary>
		/// Set your own DamageClass type
		/// </summary>
		public static void BossRushSetDefault(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, bool autoReuse) {
			item.width = width;
			item.height = height;
			item.damage = damage;
			item.knockBack = knockback;
			item.useTime = useTime;
			item.useAnimation = useAnimation;
			item.useStyle = useStyle;
			item.autoReuse = autoReuse;
		}
		public static void BossRushDefaultToConsume(this Item item, int width, int height, int useStyle = ItemUseStyleID.HoldUp) {
			item.width = width;
			item.height = height;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = useStyle;
			item.autoReuse = false;
			item.consumable = true;
		}
		/// <summary>
		/// Use this along with <see cref="BossRushSetDefault(Item, int, int, int, float, int, int, int, bool)"/>
		/// </summary>
		/// <param name="item"></param>
		/// <param name="spearType"></param>
		/// <param name="shootSpeed"></param>
		public static void BossRushSetDefaultSpear(this Item item, int spearType, float shootSpeed) {
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shootSpeed = shootSpeed;
			item.shoot = spearType;
			item.DamageType = DamageClass.Melee;
		}
		public static void BossRushDefaultMeleeShootCustomProjectile(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootspeed, bool autoReuse) {
			BossRushSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootspeed;
			item.DamageType = DamageClass.Melee;
		}
		public static void BossRushDefaultMeleeCustomProjectile(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, bool autoReuse) {
			BossRushSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = 1;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.DamageType = DamageClass.Melee;
		}
		public static void BossRushDefaultRange(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootSpeed, bool autoReuse, int useAmmo = 0
		) {
			BossRushSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.useAmmo = useAmmo;
			item.noMelee = true;
			item.DamageType = DamageClass.Ranged;
		}
		public static void BossRushDefaultPotion(this Item item, int width, int height, int BuffType, int BuffTime) {
			item.width = width;
			item.height = height;
			item.useStyle = ItemUseStyleID.DrinkLiquid;
			item.useAnimation = item.useTime = 15;
			item.useTurn = true;
			item.maxStack = 30;
			item.consumable = true;
			item.buffType = BuffType;
			item.buffTime = BuffTime;
		}
		public static void BossRushDefaultMagic(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootSpeed, int manaCost, bool autoReuse
			) {
			BossRushSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.mana = manaCost;
			item.noMelee = true;
			item.DamageType = DamageClass.Magic;
		}

		public static void BossRushDefaultMagic(Item item, int shoot, float shootSpeed, int manaCost) {
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.mana = manaCost;
			item.noMelee = true;
		}
		public static void Set_InfoItem(this Item item, bool ExtraInfo = true) {
			if (item.TryGetGlobalItem(out GlobalItemHandle globalitem)) {
				globalitem.ExtraInfo = ExtraInfo;
			}
		}
		public static void Set_DebugItem(this Item item, bool Debug = true) {
			if (item.TryGetGlobalItem(out GlobalItemHandle globalitem)) {
				globalitem.DebugItem = Debug;
			}
		}
		public static void Set_AdvancedBuffItem(this Item item, bool Advanced = true) {
			if (item.TryGetGlobalItem(out GlobalItemHandle globalitem)) {
				globalitem.AdvancedBuffItem = Advanced;
			}
		}
		public static void Set_LostAccessory(this Item item, int width, int height, bool Lost = true) {
			item.DefaultToAccessory(width, height);
			if (item.TryGetGlobalItem(out GlobalItemHandle globalitem)) {
				globalitem.LostAccessories = Lost;
			}
		}
		public static void Set_ItemIsRPG(this Item item, bool RPG = true) {
			if (item.TryGetGlobalItem(out GlobalItemHandle globalitem)) {
				globalitem.RPGItem = RPG;
			}
		}
		public static void Set_ItemCriticalDamage(this Item item, float critDmg) {
			if (item.TryGetGlobalItem(out GlobalItemHandle globalitem)) {
				globalitem.CriticalDamage = critDmg;
			}
		}
		/// <summary>
		/// This will work for most vanilla accessory, however item effect such as follow will not work :<br/>
		/// - kbglove<br/>
		/// - accFishingBobber<br/>
		/// - skyStoneEffects<br/>
		/// - dd2Accessory<br/>
		/// - accFlipper<br/>
		/// - chiselSpeed<br/>
		/// - equippedAnyWallSpeedAcc<br/>
		/// - equippedAnyTileRangeAcc<br/>
		/// - accWatch<br/>
		/// - hasLuck_LuckyHorseshoe<br/>
		/// - hasLuck_LuckyCoin<br/>
		/// - dpsStarted<br/>
		/// For detailed information on why those won't work, see <see cref="Player.UpdateEquips"/><br/>
		/// Should you want to remove any of above effect, do it manually after UpdateEquips via a hook is recommended
		/// </summary>
		/// <param name="item"></param>
		/// <param name="itemoverride"></param>
		public static void Item_Set_OverrideVanillaEffect(this Item item, bool itemoverride = true) {
			if (item.TryGetGlobalItem(out GlobalItemHandle handle)) {
				handle.OverrideVanillaEffect = itemoverride;
			}
		}
		public static bool Item_Can_OverrideVanillaEffect(this Item item) {
			if (item.TryGetGlobalItem(out GlobalItemHandle handle)) {
				return handle.OverrideVanillaEffect;
			}
			return false;
		}
		public static void Set_ShieldStats(this Item item, int health, float res) {
			if (!BossRushModSystem.Shield.Contains(item.type)) {
				BossRushModSystem.Shield.Add(item.type);
			}
			if (item.TryGetGlobalItem(out Shield_GlobalItem globalitem)) {
				globalitem.ShieldPoint = health;
				globalitem.ShieldRes = res;
			}
		}
		public enum MeleeStyle {
			/// <summary>
			/// This will check vanilla swing and modded swing whenever or not either of 2 is active
			/// </summary>
			CheckVanillaSwingWithModded,
			/// <summary>
			/// This will check for modded swing only whenever or not the modded swing is active
			/// </summary>
			CheckOnlyModded,
			/// <summary>
			/// This will check only modded swing but exclude the default swing
			/// </summary>
			CheckOnlyModdedWithoutDefault
		}
		public static bool CheckUseStyleMelee(this Item item, MeleeStyle WhatToCheck) {
			if (!UniversalSystem.Check_RLOH()) {
				return false;
			}
			if (item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
				switch (WhatToCheck) {
					case MeleeStyle.CheckVanillaSwingWithModded:
						return item.useStyle == ItemUseStyleID.Swing || meleeItem.SwingType != 0;
					case MeleeStyle.CheckOnlyModded:
						return meleeItem.SwingType != 0;
					case MeleeStyle.CheckOnlyModdedWithoutDefault:
						return meleeItem.SwingType == BossRushUseStyle.Swipe;
					default:
						Console.WriteLine("Fail to know what to check !");
						return false;
				}
			}
			return false;
		}
		public static void DrawAuraEffect(SpriteBatch spriteBatch, Texture2D texture, Vector2 drawPos, float offsetX, float offsetY, Color color, float rotation, float scale) {
			Vector2 origin = texture.Size() * .5f;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
			}
		}
		public static void DrawAuraEffect(this Item item, SpriteBatch spriteBatch, Vector2 drawPos, float offsetX, float offsetY, Color color, float rotation, float scale) {
			Main.instance.LoadItem(item.type);
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Vector2 origin = texture.Size() * .5f;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
			}
		}
		public static void DrawAuraEffect(this Item item, SpriteBatch spriteBatch, float offsetX, float offsetY, Color color, float rotation, float scale) {
			Main.instance.LoadItem(item.type);
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Vector2 origin = texture.Size() * .5f;
			Vector2 drawPos = item.position - Main.screenPosition + origin;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
			}
		}
	}
}
