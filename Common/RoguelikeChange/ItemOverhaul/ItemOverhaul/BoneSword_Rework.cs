using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ItemOverhaul;
internal class Roguelike_BoneSword : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return UniversalSystem.Check_RLOH();
	}
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.BoneSword) {
			entity.damage = 23;
			entity.crit = 4;
			entity.ArmorPenetration = 5;
			entity.shoot = ProjectileID.BookOfSkullsSkull;
			entity.shootSpeed = 15;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.BoneSword) {
			BossRushUtils.AddTooltip(ref tooltips, new(Mod, $"RoguelikeOverhaul_{item.Name}", BossRushUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type != ItemID.BoneSword) {
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		Roguelike_BoneSword_ModPlayer modplayer = player.GetModPlayer<Roguelike_BoneSword_ModPlayer>();
		if (modplayer.Counter >= 60) {
			for (int i = 0; i < 3; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(3, 60, i), type, damage, knockback, player.whoAmI);
			}
		}
		modplayer.Counter = 0;
		if (++modplayer.SwingCounter >= 5) {
			modplayer.SwingCounter = 0;
			return true;
		}

		return false;
	}
	public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
		if (item.type != ItemID.BoneSword) {
			return;
		}
		Roguelike_BoneSword_ModPlayer modplayer = player.GetModPlayer<Roguelike_BoneSword_ModPlayer>();
		if (modplayer.PerfectStrike || modplayer.Counter >= 180) {
			damage *= 1.5f;
		}
		else if (modplayer.Counter >= 60) {
			damage *= 1.2f;
		}
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type == ItemID.BoneSword) {
			Projectile projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), target.Center.Add(0, target.height + 5), Vector2.UnitY * (-5 + Main.rand.NextFloat(0,2)), ProjectileID.Bone, player.GetWeaponDamage(item), 1f, player.whoAmI);
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 3;
			projectile.maxPenetrate = 3;
		}
	}
}
public class Roguelike_BoneSword_ModPlayer : ModPlayer {
	public int SwingCounter = 0;
	public bool PerfectStrike = false;
	public int Counter = 0;
	public override void ResetEffects() {
		if(!UniversalSystem.Check_RLOH()) {
			return;
		}
		if (Player.HeldItem.type != ItemID.BoneSword) {
			SwingCounter = 0;
		}
		else {
			if (Counter == 60) {
				PerfectStrikeEffect();
			}
		}
		PerfectStrike = Counter >= 60 && Counter <= 74;
		if (++Counter >= 180) {
			Counter = 180;
		}
	}
	private void PerfectStrikeEffect() {
		SoundEngine.PlaySound(SoundID.Item71 with { Pitch = -.5f }, Player.Center);
		Vector2 abovePlayer = Player.Center.Add(0, -60);
		for (int i = 0; i < 35; i++) {
			Dust dust = Dust.NewDustDirect(abovePlayer, 0, 0, DustID.Bone, Scale: Main.rand.NextFloat(.6f, .9f));
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5);
			dust.Dust_GetDust().FollowEntity = true;
			dust.Dust_BelongTo(Player);


			Dust dust2 = Dust.NewDustDirect(abovePlayer, 0, 0, DustID.GemSapphire, Scale: Main.rand.NextFloat(.8f, .9f));
			dust2.noGravity = true;
			dust2.velocity = Main.rand.NextVector2CircularEdge(4.5f, 4.5f);
			dust2.Dust_GetDust().FollowEntity = true;
			dust2.Dust_BelongTo(Player);
		}
		for (int o = 0; o < 5; o++) {
			for (int i = 0; i < 4; i++) {
				var Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * (3 + Main.rand.NextFloat()) * 5;
				for (int l = 0; l < 8; l++) {
					float multiplier = Main.rand.NextFloat();
					float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
					int dust = Dust.NewDust(abovePlayer, 0, 0, DustID.GemDiamond, 0, 0, 0, Color.White, scale);
					Main.dust[dust].velocity = Toward * multiplier;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].Dust_GetDust().FollowEntity = true;
					Main.dust[dust].Dust_BelongTo(Player);
				}
			}
		}
	}
}
