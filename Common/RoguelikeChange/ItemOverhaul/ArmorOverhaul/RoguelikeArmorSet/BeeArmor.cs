using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class BeeArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.BeeHeadgear;
		bodyID = ItemID.BeeBreastplate;
		legID = ItemID.BeeGreaves;
		OverrideOriginalToolTip = true;
	}
}
public class BeeHeadgear : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BeeHeadgear;
		Add_Defense = 6;
		TypeEquipment = Type_Head;
		ArmorName = "BeeArmor";
		AddTooltip = true;
		OverrideTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetDamage(DamageClass.Melee) += .04f;
		player.GetDamage(DamageClass.Ranged) += .04f;
		player.GetDamage(DamageClass.Magic) += .04f;
		player.GetCritChance(DamageClass.Generic) += 3;
	}
}
public class BeeBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BeeBreastplate;
		Add_Defense = 7;
		TypeEquipment = Type_Body;
		ArmorName = "BeeArmor";
		AddTooltip = true;
		OverrideTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetDamage(DamageClass.Melee) += .05f;
		player.GetDamage(DamageClass.Ranged) += .05f;
		player.GetDamage(DamageClass.Magic) += .05f;
		player.GetAttackSpeed(DamageClass.Melee) += .06f;
	}
}

public class BeeGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BeeGreaves;
		Add_Defense = 5;
		TypeEquipment = Type_Leg;
		ArmorName = "BeeArmor";
		AddTooltip = true;
		OverrideTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetDamage(DamageClass.Melee) += .05f;
		player.GetDamage(DamageClass.Ranged) += .05f;
		player.GetDamage(DamageClass.Magic) += .05f;
		player.manaCost -= .16f;
	}
}
public class BeeArmorPlayer : PlayerArmorHandle {
	public const int DashRight = 2;
	public const int DashLeft = 3;

	public const int DashCooldown = 50;
	public const int DashDuration = 35;

	public const float DashVelocity = 12.5f;

	public int DashDir = -1;

	public int DashDelay = 0;
	public int DashTimer = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("BeeArmor", this);
	}
	public override void Armor_ResetEffects() {
		// ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
		// When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
		// If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
		if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15) {
			DashDir = DashRight;
		}
		else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15) {
			DashDir = DashLeft;
		}
		else {
			DashDir = -1;
		}
	}
	public override void Armor_UpdateEquipsSet() {
		Player.GetDamage(DamageClass.Melee) += .1f;
		Player.GetDamage(DamageClass.Ranged) += .1f;
		Player.GetDamage(DamageClass.Magic) += .1f;
		Player.maxMinions++;
	}
	public override void Armor_PreUpdateMovement() {
		if (CanUseDash() && DashDir != -1 && DashDelay == 0 && Player.HeldItem.DamageType == DamageClass.Melee) {
			Vector2 newVelocity = Player.velocity;

			switch (DashDir) {
				case DashLeft when Player.velocity.X > -DashVelocity:
				case DashRight when Player.velocity.X < DashVelocity: {
						float dashDirection = DashDir == DashRight ? 1 : -1;
						newVelocity.X = dashDirection * DashVelocity;
						break;
					}
				default:
					return;
			}
			DashDelay = DashCooldown;
			DashTimer = DashDuration;
			Player.velocity = newVelocity;
		}

		if (DashDelay > 0)
			DashDelay--;

		if (DashTimer > 0) {
			Player.eocDash = DashTimer;
			Player.armorEffectDrawShadowEOCShield = true;
			DashTimer--;
		}
	}
	private bool CanUseDash() {
		return Player.dashType == DashID.None
			&& !Player.setSolar
			&& !Player.mount.Active;
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.DamageType == DamageClass.Ranged) {
			int proj = Projectile.NewProjectile(source, position, velocity, ProjectileID.Stinger, damage, knockback, Player.whoAmI);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
			Main.projectile[proj].penetrate = 1;
		}
		if (item.DamageType == DamageClass.Magic) {
			int proj = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), ProjectileID.QueenBeeStinger, damage, knockback, Player.whoAmI);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
