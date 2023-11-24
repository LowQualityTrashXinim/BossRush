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
	public bool Trinket4 = false;
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
					velocity.Vector2RotateByRandom(10).Vector2RandomSpread(1, Main.rand.NextFloat(.95f, 1.1f)), ProjectileID.CrystalStorm, damage, knockback, Player.whoAmI);
			}
			IsManaCheckSuccess = false;
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		ManaShield(ref modifiers);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		ManaShield(ref modifiers);
	}
	private void ManaShield(ref Player.HurtModifiers modifiers) {
		if (!Trinket4)
			return;
		if (Player.CheckMana((int)(modifiers.FinalDamage.ToIntValue() * .5f), true)) {
			modifiers.SetMaxDamage(1);
		}
	}
}
