using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class StarCharm : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.SummonDMG, 1.2f);

		player.GetModPlayer<StarCharmPlayer>().StarCharm = true;
	}
}
public class StarCharmPlayer : ModPlayer {
	public int ChanceToActivate = 0;
	public bool StarCharm = false;
	public override void ResetEffects() {
		StarCharm = false;
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (StarCharm && (proj.DamageType == DamageClass.Summon || proj.minion)) {
			float Chance = ChanceToActivate * 0.01f;
			if (!Main.dayTime) {
				Chance *= 1.5f;
			}
			if (Main.rand.NextFloat() < Chance) {
				ChanceToActivate = 0;
				Vector2 newPos = target.Center.Add(Main.rand.Next(-100, 100), -1000);
				Vector2 velocityTo = (target.Center - newPos).SafeNormalize(Vector2.UnitX) * 30;
				int damage = (int)(Player.GetDamage(DamageClass.Summon).ApplyTo(40) * (Player.maxMinions + Player.maxTurrets) / 2);
				int projectile = Projectile.NewProjectile(Player.GetSource_OnHit(target), newPos, velocityTo, ProjectileID.Starfury, damage, 1f, Player.whoAmI);
				Main.projectile[projectile].DamageType = DamageClass.Summon;
			}
			else {
				ChanceToActivate++;
			}
		}
	}
}
