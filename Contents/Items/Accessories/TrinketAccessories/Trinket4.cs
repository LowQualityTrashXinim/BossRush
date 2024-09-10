using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Accessories.TrinketAccessories;
internal class Trinket4 : BaseTrinket {
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		modplayer.GetStatsHandle().AddStatsToPlayer(PlayerStats.MaxMana, Base: 60);
		player.GetModPlayer<Trinket4_ModPlayer>().Trinket4 = true;
	}
}
public class Trinket4_ModPlayer : ModPlayer {
	public bool Trinket4 = false;
	public override void ResetEffects() {
		Trinket4 = false;
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (!Trinket4)
			return;
		if (Player.statMana >= Player.statManaMax2 * .5f) {
			if (Player.CheckMana(10, true)) {
				Player.manaRegenDelay = (int)Player.maxRegenDelay;
				damage = (int)(damage * 1.2f);
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (!Trinket4)
			return;
		if (Player.statMana < Player.statManaMax2 * .5f) {
			modifiers.FinalDamage -= .3f;
			Player.statMana++;
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		ManaShield(npc.damage, ref modifiers);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		ManaShield(proj.damage, ref modifiers);
	}
	private void ManaShield(int damageValue, ref Player.HurtModifiers modifiers) {
		if (!Trinket4)
			return;
		if (Player.CheckMana(damageValue, true)) {
			Player.manaRegenDelay = Player.maxRegenDelay;
			modifiers.SetMaxDamage(1);
			for (int i = 0; i < 100; i++) {
				Vector2 evenVec = Vector2.One.Vector2DistributeEvenly(100, 360, i) * 30;
				int dust = Dust.NewDust(Player.Center + evenVec, 0, 0, DustID.ManaRegeneration);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = 1;
			}
		}
	}
}
