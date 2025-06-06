﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Common.General;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul {
	//public readonly static int[] GunType = {
	//    ItemID.RedRyder,
	//    ItemID.Minishark,
	//    ItemID.Gatligator,
	//    ItemID.Handgun,
	//    ItemID.PhoenixBlaster,
	//    ItemID.Musket,
	//    ItemID.TheUndertaker,
	//    ItemID.FlintlockPistol,
	//    ItemID.Revolver,
	//    ItemID.ClockworkAssaultRifle,
	//    ItemID.Megashark,
	//    ItemID.Uzi,
	//    ItemID.VenusMagnum,
	//    ItemID.SniperRifle,
	//    ItemID.ChainGun,
	//    ItemID.SDMG,
	//    ItemID.Boomstick,
	//    ItemID.QuadBarrelShotgun,
	//    ItemID.Shotgun,
	//    ItemID.OnyxBlaster,
	//    ItemID.TacticalShotgun
	//};
	public class RangeWeaponOverhaul : GlobalItem {
		public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
			if (entity.damage > 0 && !entity.accessory) {
				return true;
			}
			else {
				return false;
			}
		}
		public override bool InstancePerEntity => true;
		public float OffSetPost = 0;
		public float SpreadAmount = 0;
		public float AdditionalSpread = 0;
		public float AdditionalMulti = 1;
		public int NumOfProjectile = 1;
		public int VariableBulletAmount = 0;
		public bool itemIsAShotgun = false;
		public override void SetDefaults(Item entity) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			switch (entity.type) {
				case ItemID.Minishark:
					NumOfProjectile = 1;
					OffSetPost = 15;
					SpreadAmount = 6.5f;
					AdditionalSpread = 2;
					break;
				case ItemID.Gatligator:
					NumOfProjectile = 1;
					OffSetPost = 20;
					SpreadAmount = 15;
					AdditionalSpread = 3;
					break;
				case ItemID.RedRyder:
					NumOfProjectile = 1;
					OffSetPost = 20;
					SpreadAmount = 5;
					AdditionalSpread = .5f;
					break;
				case ItemID.Musket:
					NumOfProjectile = 1;
					OffSetPost = 35;
					SpreadAmount = 3.5f;
					break;
				case ItemID.FlintlockPistol:
					NumOfProjectile = 1;
					OffSetPost = 10;
					SpreadAmount = 7;
					AdditionalSpread = 2;
					break;
				case ItemID.TheUndertaker:
					NumOfProjectile = 1;
					OffSetPost = 20;
					SpreadAmount = 3;
					AdditionalSpread = 2;
					break;
				case ItemID.Revolver:
					NumOfProjectile = 1;
					OffSetPost = 10;
					SpreadAmount = 1;
					AdditionalSpread = 1.2f;
					break;
				case ItemID.Handgun:
					NumOfProjectile = 1;
					OffSetPost = 10;
					SpreadAmount = 5;
					AdditionalSpread = 1;
					break;
				case ItemID.PhoenixBlaster:
					NumOfProjectile = 1;
					OffSetPost = 10;
					SpreadAmount = 4;
					AdditionalSpread = .5f;
					break;
				case ItemID.ClockworkAssaultRifle:
					NumOfProjectile = 1;
					OffSetPost = 15;
					SpreadAmount = 9;
					AdditionalSpread = 1;
					break;
				case ItemID.Megashark:
					NumOfProjectile = 1;
					OffSetPost = 30;
					SpreadAmount = 7;
					AdditionalSpread = 2;
					break;
				case ItemID.Uzi:
					NumOfProjectile = 1;
					SpreadAmount = 12;
					AdditionalSpread = 1;
					break;
				case ItemID.VenusMagnum:
					NumOfProjectile = 1;
					OffSetPost = 25;
					SpreadAmount = 10;
					AdditionalSpread = 2;
					break;
				case ItemID.SniperRifle:
					NumOfProjectile = 1;
					OffSetPost = 35;
					SpreadAmount = 2;
					break;
				case ItemID.ChainGun:
					NumOfProjectile = 1;
					OffSetPost = 35;
					SpreadAmount = 27;
					AdditionalSpread = 3;
					break;
				case ItemID.VortexBeater:
					OffSetPost = 35;
					SpreadAmount = 20;
					AdditionalSpread = 2;
					break;
				case ItemID.SDMG:
					NumOfProjectile = 1;
					OffSetPost = 35;
					SpreadAmount = 4;
					AdditionalSpread = 2;
					break;
				case ItemID.Boomstick:
					OffSetPost = 25;
					SpreadAmount = 18;
					AdditionalSpread = 4;
					AdditionalMulti = .4f;
					NumOfProjectile = 4;
					VariableBulletAmount = 2;
					itemIsAShotgun = true;
					break;
				case ItemID.QuadBarrelShotgun:
					OffSetPost = 25;
					SpreadAmount = 45;
					AdditionalSpread = 6;
					NumOfProjectile += 6;
					itemIsAShotgun = true;
					break;
				case ItemID.Shotgun:
					OffSetPost = 35;
					SpreadAmount = 24;
					AdditionalSpread = 6;
					AdditionalMulti = .5f;
					NumOfProjectile = 4;
					VariableBulletAmount = 2;
					itemIsAShotgun = true;
					break;
				case ItemID.OnyxBlaster:
					OffSetPost = 35;
					SpreadAmount = 15;
					AdditionalSpread = 6;
					NumOfProjectile = 4;
					VariableBulletAmount = 2;
					itemIsAShotgun = true;
					break;
				case ItemID.TacticalShotgun:
					OffSetPost = 35;
					SpreadAmount = 18;
					AdditionalSpread = 3;
					AdditionalMulti = .76f;
					NumOfProjectile = 6;
					itemIsAShotgun = true;
					break;
			}
			//Prototype_PrefixedWorldVault(entity);
		}
		private void Prototype_PrefixedWorldVault(Item item) {
			if (item.type == ItemID.Minishark) {
				item.damage += 2;
				item.knockBack += 2;
				item.useTime += 2;
				item.useAnimation += 2;
				item.Set_ItemCriticalDamage(.5f);
				item.crit += 10;

			}
		}
		/// <summary>
		/// Use this if your weapon have spread or not
		/// </summary>
		/// <param name="modplayer"></param>
		/// <param name="velocity"></param>
		/// <returns></returns>
		public Vector2 RoguelikeGunVelocity(RangerOverhaulPlayer modplayer, Vector2 velocity) {
			return velocity
				.Vector2RotateByRandom(modplayer.SpreadModify.ApplyTo(SpreadAmount))
				.Vector2RandomSpread(modplayer.SpreadModify.ApplyTo(AdditionalSpread), modplayer.SpreadModify.ApplyTo(AdditionalMulti));
		}
		public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (!UniversalSystem.Check_RLOH() && item.ModItem == null) {
				return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
			}
			if (itemIsAShotgun && item.ModItem == null) {
				return true;
			}
			RangerOverhaulPlayer modplayer = player.GetModPlayer<RangerOverhaulPlayer>();
			int amount = NumOfProjectile + modplayer.ProjectileAmountModify;
			if (VariableBulletAmount > 0) {
				amount += Main.rand.Next(VariableBulletAmount + 1);
			}
			if (amount >= 2) {
				amount--;
				for (int i = 0; i < amount; i++) {
					Vector2 velocity2 = RoguelikeGunVelocity(modplayer, velocity);
					Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, player.whoAmI);
				}
			}
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (!UniversalSystem.Check_RLOH() && item.ModItem == null) {
				return;
			}
			RangerOverhaulPlayer modplayer = player.GetModPlayer<RangerOverhaulPlayer>();
			position = position.PositionOFFSET(velocity, OffSetPost);
			if (!itemIsAShotgun) {
				velocity = RoguelikeGunVelocity(modplayer, velocity);
			}
		}
	}
	public class RangerOverhaulPlayer : ModPlayer {
		/// <summary>
		/// Use this to change globaly and when you are using a accessories or something of similar, do not use "=" as that set and is not the correct way to use
		/// </summary>
		public StatModifier SpreadModify = new();
		/// <summary>
		/// Use this to change when you are using a accessories or something of similar, do not use "=" as that set and is not the correct way to use<br/>
		/// Default to 0
		/// </summary>
		public int ProjectileAmountModify = 0;
		public override void ResetEffects() {
			SpreadModify = new();
			ProjectileAmountModify = 0;
		}
	}
}
