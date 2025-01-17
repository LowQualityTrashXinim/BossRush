using BossRush.Contents.Perks;
using BossRush.Contents.Perks.WeaponUpgrade;
using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Perks.WeaponUpgrade.Content;

public class NaturalUpgrade_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.NaturalUpgrade)) {
			switch (entity.type) {
				case ItemID.WoodenBow:
				case ItemID.AshWoodBow:
				case ItemID.BorealWoodBow:
				case ItemID.RichMahoganyBow:
				case ItemID.PalmWoodBow:
				case ItemID.EbonwoodBow:
				case ItemID.ShadewoodBow:
					entity.shootSpeed += 3;
					entity.crit += 6;
					entity.damage += 10;
					break;
			}
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.NaturalUpgrade)) {
			switch (item.type) {
				case ItemID.WoodenBow:
				case ItemID.AshWoodBow:
				case ItemID.BorealWoodBow:
				case ItemID.RichMahoganyBow:
				case ItemID.PalmWoodBow:
				case ItemID.EbonwoodBow:
				case ItemID.ShadewoodBow:
					float chance = Main.rand.NextFloat(.2f, .4f);
					if (Main.rand.NextFloat() <= chance) {
						Vector2 pos = Main.MouseWorld + Main.rand.NextVector2CircularEdge(2000, 700);
						Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 5;
						Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<WindShot>(), (int)(damage * 1.5f), 5f, player.whoAmI);
					}
					Vector2 newPos1 = position.IgnoreTilePositionOFFSET(velocity.RotatedBy(MathHelper.PiOver2), 5);
					Vector2 newVelocity1 = (Main.MouseWorld - newPos1).SafeNormalize(Vector2.Zero) * velocity.Length();
					Vector2 newPos2 = position.IgnoreTilePositionOFFSET(velocity.RotatedBy(-MathHelper.PiOver2), 5);
					Vector2 newVelocity2 = (Main.MouseWorld - newPos2).SafeNormalize(Vector2.Zero) * velocity.Length();
					Projectile arrow1 = Projectile.NewProjectileDirect(source, newPos1, newVelocity1, type, damage, knockback, player.whoAmI);
					Projectile arrow2 = Projectile.NewProjectileDirect(source, newPos2, newVelocity2, type, damage, knockback, player.whoAmI);
					if (ContentSamples.ProjectilesByType[type].arrow) {
						arrow1.extraUpdates += 1;
						arrow2.extraUpdates += 1;
					}
					return false;
			}
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}

public class NaturalUpgrade_Player : ModPlayer {
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ProjectileID.WoodenArrowFriendly || !proj.Check_ItemTypeSource(Player.HeldItem.type)) {
			return;
		}
		if (UpgradePlayer.Check_Upgrade(Player, WeaponUpgradeID.NaturalUpgrade)) {
			float chance = Main.rand.NextFloat(.2f, .4f);
			if (Main.rand.NextFloat() <= chance) {
				Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(2000, 700);
				Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 5;
				Projectile.NewProjectile(proj.GetSource_OnHit(target), pos, vel, ModContent.ProjectileType<WindShot>(), (int)(proj.damage * 1.5f), 5f, Player.whoAmI);
			}
			chance = Main.rand.NextFloat(.15f, .2f);
			if (Main.rand.NextFloat() <= chance) {
				for (int i = 0; i < 50; i++) {
					Dust dustRing = Dust.NewDustDirect(target.Center + Main.rand.NextVector2CircularEdge(120, 120), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(.5f, .7f));
					dustRing.noGravity = true;
					dustRing.fadeIn = 1.5f;
					Dust dust = Dust.NewDustDirect(target.Center, 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(.5f, .7f));
					dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(1, 3.5f);
					dust.noGravity = true;
					dust.fadeIn = 1.5f;
					dust.rotation = Main.rand.NextFloat();
				}
				target.Center.LookForHostileNPC(out List<NPC> npclist, 120f);
				if (npclist.Count < 1) {
					return;
				}
				hit.Crit = false;
				hit.Damage *= 2;
				foreach (NPC npc in npclist) {
					hit.HitDirection = BossRushUtils.DirectionFromPlayerToNPC(Player.Center.X, npc.Center.X);
					Player.StrikeNPCDirect(npc, hit);
				}
			}
		}
	}
}
public class NaturalUpgrade : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.NaturalUpgrade);
		Mod.Reflesh_GlobalItem(player);
	}
}
