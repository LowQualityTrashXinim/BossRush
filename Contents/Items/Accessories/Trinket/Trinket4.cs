using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket4 : BaseTrinket {
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		modplayer.ManaStats.Base += 60;
		player.GetModPlayer<Trinket4_ModPlayer>().Trinket4 = true;
	}
}
public class Trinket4_ModPlayer : ModPlayer {
	bool IsManaCheckSuccess = false;
	bool SuccessfulBlockAHit = false;
	public bool Trinket4 = false;
	public override void ResetEffects() {
		Trinket4 = false;
	}
	public override void PostUpdate() {
		if (!Trinket4)
			return;
		if (!SuccessfulBlockAHit)
			return;
		for (int i = 0; i < 100; i++) {
			Vector2 evenVec = Vector2.One.Vector2DistributeEvenly(100, 360, i) * 30;
			int dust = Dust.NewDust(Player.Center + evenVec, 0, 0, DustID.ManaRegeneration);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Vector2.Zero;
			Main.dust[dust].scale = 1;
		}
		SuccessfulBlockAHit = false;
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (!Trinket4)
			return;
		if (Player.statMana >= Player.statLifeMax2 * .5f) {
			if (Player.CheckMana(10, true)) {
				IsManaCheckSuccess = true;
				velocity *= 1.2f;
				damage = (int)(damage * 1.2f);
			}
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (IsManaCheckSuccess) {
			for (int i = 0; i < 5; i++) {
				Projectile.NewProjectile(source, position,
					velocity.Vector2RotateByRandom(25).Vector2RandomSpread(1, Main.rand.NextFloat(.95f, 1.1f)),
					ProjectileID.CrystalStorm, (int)(damage * .45f), knockback, Player.whoAmI);
			}
			IsManaCheckSuccess = false;
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		ManaShield(npc.damage, ref modifiers);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		ManaShield(proj.damage, ref modifiers);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		base.OnHitByNPC(npc, hurtInfo);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		base.OnHitByProjectile(proj, hurtInfo);
	}
	private void ManaShield(int damageValue, ref Player.HurtModifiers modifiers) {
		if (!Trinket4)
			return;
		if (Player.CheckMana(damageValue, true)) {
			modifiers.SetMaxDamage(1);
			SuccessfulBlockAHit = true;
		}
	}
}
