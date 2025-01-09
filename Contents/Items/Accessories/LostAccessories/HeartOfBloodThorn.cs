using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.General;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class HeartOfBloodThorn : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<BloodBurstPlayer>().BloodBurst = true;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.HealEffectiveness, Additive: 1.15f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MaxHP, Base: 15);
	}
}
class BloodBurstPlayer : ModPlayer {
	public bool BloodBurst = false;
	public override void ResetEffects() {
		BloodBurst = false;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (BloodBurst) {
			BloodBurstAttack();
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (BloodBurst) {
			BloodBurstAttack();
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if(proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ModContent.ItemType<HeartOfBloodThorn>()) {
			Player.Heal(Main.rand.Next(3,7));
		}
	}
	public void BloodBurstAttack() {
		int damage = (int)Player.GetDamage(DamageClass.Magic).ApplyTo(45);
		Vector2 vecR = Vector2.One.Vector2RotateByRandom(90);
		for (int i = 0; i < 6; i++) {
			Vector2 vec = BossRushUtils.Vector2DistributeEvenly(vecR, 6, 360, i);
			Projectile.NewProjectile(Player.GetSource_ItemUse(ContentSamples.ItemsByType[ModContent.ItemType<HeartOfBloodThorn>()].Clone()), Player.Center, vec, ProjectileID.SharpTears, damage, 3f, Player.whoAmI,0,Main.rand.NextFloat(.9f,1.1f));
		}
	}
}
