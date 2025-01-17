using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Perks;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Perks.WeaponUpgrade;

namespace BossRush.Contents.Perks.WeaponUpgrade.Content;
internal class WindSlash_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.WindSlash)) {
			return;
		}
		if (!entity.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckVanillaSwingWithModded) && entity.DamageType == DamageClass.Melee) {
			entity.damage += 5;
		}
	}
	public override void HoldItem(Item item, Player player) {
		if (!UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.WindSlash)) {
			return;
		}
		if (item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckVanillaSwingWithModded) && item.DamageType == DamageClass.Melee) {
			if (Main.mouseLeft && player.itemAnimation == player.itemAnimationMax) {
				Vector2 speed = Vector2.UnitX * player.direction;
				if (player.HeldItem.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded)) {
					speed = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				}
				int damage = (int)(player.HeldItem.damage * .75f);
				float length = player.HeldItem.Size.Length() * player.GetAdjustedItemScale(player.HeldItem);
				if (player.GetModPlayer<WindSlash_ModPlayer>().StrikeOpportunity) {
					speed *= 1.5f;
					damage *= 3;
				}
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center.PositionOFFSET(speed, length + 17), speed * 5, ModContent.ProjectileType<WindSlashProjectile>(), damage, 2f, player.whoAmI);
			}
		}
	}
}
public class WindSlash : Perk {
	public override void SetDefaults() {
		textureString = BossRushUtils.GetTheSameTextureAsEntity<WindSlash>();
		list_category.Add(PerkCategory.WeaponUpgrade);
		CanBeStack = false;
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.WindSlash);
		Mod.Reflesh_GlobalItem(player);
	}
}
public class WindSlash_ModPlayer : ModPlayer {
	public int OpportunityWindow = 0;
	public bool StrikeOpportunity = false;
	public override void PostUpdate() {
		if (!UpgradePlayer.Check_Upgrade(Player, WeaponUpgradeID.WindSlash)) {
			return;
		}
		if (Player.ItemAnimationActive) {
			OpportunityWindow = 0;
			StrikeOpportunity = false;
		}
		if (OpportunityWindow >= BossRushUtils.ToSecond(1.5f)) {
			if (!StrikeOpportunity) {
				for (int i = 0; i < 36; i++) {
					Vector2 vel = Vector2.One.Vector2DistributeEvenlyPlus(36, 360, i) * 5;
					Dust dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.SolarFlare);
					dust.noGravity = true;
					dust.velocity = vel;
				}
			}
			StrikeOpportunity = true;
			return;
		}
		OpportunityWindow++;
	}
}
