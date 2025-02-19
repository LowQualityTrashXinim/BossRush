using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;
using BossRush.Common.Systems;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class TinArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.TinHelmet;
		bodyID = ItemID.TinChainmail;
		legID = ItemID.TinGreaves;
	}
}
public class TinHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TinHelmet;
		Add_Defense = 4;
		AddTooltip = true;
		ArmorName = "TinArmor";
		TypeEquipment = Type_Head;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.AttackSpeed, 1.09f);
	}
}
public class TinChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TinChainmail;
		Add_Defense = 5;
		AddTooltip = true;
		ArmorName = "TinArmor";
		TypeEquipment = Type_Body;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.endurance += .05f;
	}
}
public class TinGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TinGreaves;
		Add_Defense = 4;
		AddTooltip = true;
		ArmorName = "TinArmor";
		TypeEquipment = Type_Leg;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.12f);
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.1f);
	}
}
public class TinPlayer : ModPlayer {
	public override void ModifyWeaponCrit(Item item, ref float crit) {
		if (Player.GetModPlayer<RoguelikeArmorPlayer>().ActiveArmor.HeadID == ItemID.TinHelmet) {
			switch (item.type) {
				case ItemID.TinBroadsword:
				case ItemID.TinBow:
				case ItemID.TinAxe:
				case ItemID.TopazStaff:
				case ItemID.TinPickaxe:
				case ItemID.TinHammer:
				case ItemID.TinShortsword:
					crit += 4;
					break;
			}
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (Player.GetModPlayer<RoguelikeArmorPlayer>().ActiveArmor.BodyID == ItemID.TinChainmail) {
			switch (item.type) {
				case ItemID.TinBroadsword:
				case ItemID.TinBow:
				case ItemID.TinAxe:
				case ItemID.TopazStaff:
				case ItemID.TinPickaxe:
				case ItemID.TinHammer:
				case ItemID.TinShortsword:
					damage += .16f;
					break;
			}
		}
	}
}
public class TinArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("TinArmor", this);
	}
	public int TinArmorCountEffect = 0;
	public override void Armor_ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.useAmmo == AmmoID.Arrow) {
			velocity *= 2;
		}
	}
	public override bool Armor_Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.useAmmo == AmmoID.Arrow) {
			Vector2 pos = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(position, 50, 50);
			Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * velocity.Length();
			Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<TinOreProjectile>(), damage, knockback, Player.whoAmI);
			TinArmorCountEffect++;
			if (TinArmorCountEffect >= 5) {
				Projectile.NewProjectile(source, position, velocity * 1.15f, ModContent.ProjectileType<TinBarProjectile>(), (int)(damage * 1.5f), knockback, Player.whoAmI);
				TinArmorCountEffect = 0;
			}
		}
		if (item.mana > 0 && Item.staff[item.type]) {
			for (int i = 0; i < 3; i++) {
				Vector2 vec = velocity.Vector2DistributeEvenly(3, 10, i);
				int proj = Projectile.NewProjectile(source, position, vec, type, damage, knockback, Player.whoAmI);
				Main.projectile[proj].extraUpdates = 10;
			}
			return false;
		}
		if (item.useStyle == ItemUseStyleID.Rapier) {
			Vector2 pos = position + Main.rand.NextVector2Circular(50, 50);
			Projectile.NewProjectile(source, pos, Main.MouseWorld - pos, ModContent.ProjectileType<TinShortSwordProjectile>(), damage, knockback, Player.whoAmI);
		}
		return true;
	}
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 5;
		Player.moveSpeed += .21f;
	}
	public override void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.DamageType == DamageClass.Melee) {
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero), ModContent.ProjectileType<TinBroadSwordProjectile>(), 12, 1f, Player.whoAmI);
		}
	}
}
