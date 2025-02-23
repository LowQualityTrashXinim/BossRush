using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.BuffAndDebuff;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.AxeOverhaul;
internal class BloodLustClusterRework : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.WarAxeoftheNight) {
			entity.scale += .25f;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.BloodLustCluster) {
			BossRushUtils.AddTooltip(ref tooltips, new(Mod, item.type + "_Rework", "Inflict extreme bleeding\nMay spawn additional sentient blood"));
		}
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type == ItemID.BloodLustCluster) {
			target.AddBuff<BloodButchererEnchantmentDebuff>(BossRushUtils.ToSecond(Main.rand.Next(3, 7)));
		}
	}
	public override void MeleeEffects(Item item, Player player, Rectangle hitbox) {
		if (item.type == ItemID.BloodLustCluster) {
			if (player.itemAnimation == player.itemAnimationMax / 2 || player.itemAnimation == player.itemAnimationMax / 3 * 2 || player.itemAnimation == player.itemAnimationMax / 3) {
				if (!Main.rand.NextBool(5)) {
					return;
				}
				Projectile.NewProjectile(player.GetSource_ItemUse(item), Main.rand.NextVector2FromRectangle(hitbox), Vector2.Zero, ModContent.ProjectileType<SentientBlood>(), (int)(item.damage * .77f), .2f, player.whoAmI);
			}
		}
	}
}
public class SentientBlood : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.BloodShot);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 5;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
		Projectile.hide = true;
		Projectile.timeLeft = 300;
	}
	public override void AI() {
		Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood);
		dust.noGravity = true;
		dust.velocity = Main.rand.NextVector2Circular(3, 3);
		dust.scale = Main.rand.NextFloat(.9f, 1.1f);
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 250f)) {
			Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
		}
		Projectile.velocity *= .99f;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ScalingArmorPenetration += 1f;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 8; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2Circular(3, 3);
			dust.scale = Main.rand.NextFloat(.9f, 1.1f);
		}
	}
}
