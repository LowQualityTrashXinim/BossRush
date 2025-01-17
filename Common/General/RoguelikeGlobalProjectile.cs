using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.BuilderItem;
using BossRush.Contents.Items.Accessories.LostAccessories;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HeavenSmg;
using BossRush.Contents.Items.RelicItem;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon.ArcaneRange.MagicBow;
using BossRush.Common.Systems;

namespace BossRush.Common.General;
internal class RoguelikeGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;

	public int Source_ItemType = -1;
	public string Source_CustomContextInfo = string.Empty;
	public bool Source_FromDeathScatterShot = false;
	public int OnKill_ScatterShot = -1;
	private float TravelDistanceBeforeKill = -1f;
	public float VelocityMultiplier = 1f;
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source is null) {
			return;
		}
		Item itemUse = null;
		if (source is EntitySource_ItemUse parent) {
			itemUse = parent.Item;
			Source_ItemType = parent.Item.type;
		}
		if (source is EntitySource_Misc parent2 && parent2.Context == "OnKill_ScatterShot") {
			Source_FromDeathScatterShot = true;
		}
		Source_CustomContextInfo = source.Context;
		if (itemUse != null) {
			if (itemUse.ModItem is Relic) {
				if (int.TryParse(source.Context, out int type)) {
					if (type == RelicTemplate.GetRelicType<SlimeSpikeTemplate>()) {
						TravelDistanceBeforeKill = 375;
					}
					else if (type == RelicTemplate.GetRelicType<FireBallTemplate>()) {
						TravelDistanceBeforeKill = 350;
					}
					else if (type == RelicTemplate.GetRelicType<SkyFractureTemplate>()) {
						TravelDistanceBeforeKill = 450;
					}
					else if (type == RelicTemplate.GetRelicType<MagicMissileTemplate>()) {
						TravelDistanceBeforeKill = 650;
					}
					else if (type == RelicTemplate.GetRelicType<DemonScytheTemplate>()) {
						TravelDistanceBeforeKill = 600;
					}
				}
			}
		}
	}
	public override bool PreAI(Projectile projectile) {
		if (VelocityMultiplier != 0) {
			projectile.velocity /= VelocityMultiplier;
		}
		else {
			projectile.velocity /= .001f;
		}
		return base.PreAI(projectile);
	}
	public override void PostAI(Projectile projectile) {
		if (VelocityMultiplier != 0) {
			projectile.velocity *= VelocityMultiplier;
		}
		else {
			projectile.velocity *= .001f;
		}
		Player player = Main.player[projectile.owner];
		if (projectile.hostile) {
			VelocityMultiplier = 1f + player.GetModPlayer<PlayerStatsHandle>().Hostile_ProjectileVelocityAddition;
			player.GetModPlayer<PlayerStatsHandle>().Hostile_ProjectileVelocityAddition = 0;
		}
		else {
			VelocityMultiplier = 1f;
		}
		if (TravelDistanceBeforeKill > 0 && Vector2.DistanceSquared(player.Center, projectile.Center) >= TravelDistanceBeforeKill * TravelDistanceBeforeKill) {
			projectile.Kill();
		}
	}
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Source_CustomContextInfo == "Skill_IceAge") {
			Player player = Main.player[projectile.owner];
			target.AddBuff(BuffID.Frozen, BossRushUtils.ToSecond(Main.rand.Next(4, 7)));
			target.Center.LookForHostileNPC(out List<NPC> npclist, 75);
			NPC.HitInfo hitweaker = hit;
			hitweaker.Damage = (int)(hit.Damage * .54f);
			foreach (NPC npc in npclist) {
				if (npc.whoAmI == target.whoAmI) {
					continue;
				}
				player.StrikeNPCDirect(npc, hitweaker);
			}
		}
	}
	public override void OnKill(Projectile projectile, int timeLeft) {
		Player player = Main.player[projectile.owner];
		if (Source_FromDeathScatterShot
			|| OnKill_ScatterShot <= 0
			|| player.heldProj == projectile.owner
			|| projectile.hostile
			|| !projectile.friendly
			|| projectile.minion
			|| projectile.aiStyle == 4
			|| projectile.aiStyle == 19
			|| projectile.aiStyle == 39
			|| projectile.aiStyle == 46
			|| projectile.aiStyle == 75
			|| projectile.aiStyle == 99
			|| projectile.aiStyle == 101
			|| projectile.minion
			|| projectile.sentry
			|| projectile.type == ProjectileID.PhantasmArrow
			|| projectile.type == ProjectileID.IchorDart
			|| projectile.type == ProjectileID.ExplosiveBunny
			|| projectile.type == ProjectileID.FinalFractal
			|| projectile.type == ProjectileID.PortalGun
			|| projectile.type == ProjectileID.PortalGunBolt
			|| projectile.type == ProjectileID.PortalGunGate
			|| projectile.type == ProjectileID.LightsBane
			|| projectile.type == ModContent.ProjectileType<LeafProjectile>()
			|| projectile.type == ModContent.ProjectileType<AngelicSmgHeld>()
			|| projectile.type == ModContent.ProjectileType<MagicBullet>()//This is to prevent lag
			|| projectile.type == ModContent.ProjectileType<DiamondGemP>()
			|| projectile.type == ModContent.ProjectileType<ArenaMakerProj>()
			|| projectile.type == ModContent.ProjectileType<NeoDynamiteExplosion>()
			|| projectile.type == ModContent.ProjectileType<TowerDestructionProjectile>()) {
			return;
		}
		if (!projectile.velocity.IsLimitReached(1)) {
			projectile.velocity *= Main.rand.NextFloat(5, 7);
		}
		for (int i = 0; i < OnKill_ScatterShot; i++) {
			int proj = Projectile.NewProjectile(projectile.GetSource_Misc("OnKill_ScatterShot"), projectile.Center, projectile.velocity.Vector2RotateByRandom(360), projectile.type, (int)(projectile.damage * .65f), projectile.knockBack * .55f, projectile.owner);
			Main.projectile[proj].timeLeft = 120;
		}
	}
}
