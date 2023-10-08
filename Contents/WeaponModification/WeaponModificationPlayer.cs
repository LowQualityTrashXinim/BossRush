using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.WeaponModification {
	/// <summary>
	/// This should be where all the effect of Weapon Modification be handle and activate
	/// </summary>
	internal class WeaponModificationPlayer : ModPlayer {
		public int[] WeaponModification_inventory = new int[20];
		Item item = null;
		public override void OnEnterWorld() {
			WeaponModificationSystem uiSystemInstance = ModContent.GetInstance<WeaponModificationSystem>();
			if (uiSystemInstance.userInterface.CurrentState != null) {
				uiSystemInstance.userInterface.SetState(null);
			}
		}
		public float Delay = 0;
		public float Recharge = 0;
		public int currentIndex = 0;

		public float damage = 1;
		public float knockback = 1;
		public float shootspeed = 1;
		public int critChance = 0;
		public float critDamage = 1;

		public bool IsOnRecharge = false;
		public override void PostUpdate() {
			if (item == null || item != Player.HeldItem) {
				item = Player.HeldItem;
				Delay = 0;
				Recharge = 0;
				currentIndex = 0;
				damage = 1;
			}
			if (Delay > 0) {
				Delay = BossRushUtils.CoolDown(Delay);
				return;
			}
			if (item.TryGetGlobalItem(out WeaponModificationGlobalItem globalItem)) {
				if (globalItem.ModWeaponSlotType == null)
					return;
				if (Recharge <= 0) {
					Recharge = globalItem.Recharge;
					IsOnRecharge = false;
					currentIndex = 0;
				}
				if (Recharge > 0 && currentIndex >= globalItem.ModWeaponSlotType.Length) {
					if (!IsOnRecharge) {
						BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.Red, "Recharged !");
						IsOnRecharge = true;
					}
					Recharge = BossRushUtils.CoolDown(Recharge);
					return;
				}
				if (!Player.ItemAnimationActive) {
					return;
				}
				Delay = globalItem.Delay;
				for (int i = 1; i > 0; i--) {
					if (currentIndex >= globalItem.ModWeaponSlotType.Length) {
						currentIndex = 0;
						break;
					}
					if (globalItem.ModWeaponSlotType[currentIndex] == -1) {
						currentIndex++;
						continue;
					}
					ModWeaponParticle modweapon = ModifierWeaponLoader.GetWeaponMod(globalItem.ModWeaponSlotType[currentIndex]);
					shootspeed = modweapon.ShootSpeed;
					knockback = modweapon.KnockBack;
					modweapon.PreUpdate(Player);
					modweapon.ModifyModificationDelay(Player, ref Delay, ref Recharge, ref i);
					modweapon.ModifyAttack(Player, ref damage, ref knockback, ref shootspeed);
					modweapon.ModifyCritAttack(Player, ref critChance, ref critDamage);
					modweapon.PostUpdate(Player);
					if (modweapon.ProjectileType != ProjectileID.None) {
						for (int l = 0; l < modweapon.ShootAmount; l++) {
							if (modweapon.Shoot(Player, Player.Center, (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero) * shootspeed, (int)(modweapon.ProjectileDamage * damage), knockback, i) != null) {
								break;
							}
							Projectile proj = modweapon.Shoot(Player, l);
							proj.knockBack = knockback;
							if (Main.rand.Next(1, 101) < critChance)
								damage *= 2 + critDamage;
							proj.damage = (int)(modweapon.ProjectileDamage * damage);
							proj.velocity = (Main.MouseWorld - proj.position).SafeNormalize(Vector2.Zero) * shootspeed;
						}
					}
					currentIndex++;
				}
			}
		}
		public override void ProcessTriggers(TriggersSet triggersSet) {
			if (WeaponModificationSystem.WeaponModificationKeybind.JustPressed) {
				WeaponModificationSystem uiSystemInstance = ModContent.GetInstance<WeaponModificationSystem>();
				//if ((uiSystemInstance.userInterface.CurrentState == null)
				//{
				//    BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.Red, "You must hold a weapon !");
				//    return;
				//}
				if (uiSystemInstance.userInterface.CurrentState is null) {
					uiSystemInstance.WM_uiState.whoAmI = Player.whoAmI;
					uiSystemInstance.userInterface.SetState(uiSystemInstance.WM_uiState);
				}
				else {
					uiSystemInstance.userInterface.SetState(null);
				}
			}
		}
		public override bool CanUseItem(Item item) {
			WeaponModificationSystem uiSystemInstance = ModContent.GetInstance<WeaponModificationSystem>();
			if (uiSystemInstance.userInterface.CurrentState is not null) {
				return false;
			}
			return base.CanUseItem(item);
		}
		public override void Initialize() {
			base.Initialize();
			WeaponModification_inventory = new int[20];
			Array.Fill(WeaponModification_inventory, -1);
		}
		public override void SaveData(TagCompound tag) {
			base.SaveData(tag);
			tag.Add("WeaponModification_inventory", WeaponModification_inventory);
		}
		public override void LoadData(TagCompound tag) {
			base.LoadData(tag);
			WeaponModification_inventory = tag.Get<int[]>("WeaponModification_inventory");
		}
		//public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
		//{
		//    foreach (int CustomParticle in item.GetGlobalItem<weaponMod_GlobalItem>().weaponMod_slot)
		//    {
		//        if(ItemLoader.GetItem(CustomParticle) is DamageIncreaseModifier particle)
		//        {
		//            damage += particle.DamageIncrease;
		//        }
		//    }
		//}
	}
}
