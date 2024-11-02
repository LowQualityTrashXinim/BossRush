using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class PearlwoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.PearlwoodHelmet;
		bodyID = ItemID.PearlwoodBreastplate;
		legID = ItemID.PearlwoodGreaves;
	}
}
public class PearlwoodArmorPlayer : PlayerArmorHandle {
	int pearlWoodArmorCD = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("PearlwoodArmor", this);
	}
	public override void Armor_ResetEffects() {
		pearlWoodArmorCD = BossRushUtils.CountDown(pearlWoodArmorCD);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.moveSpeed += 0.35f;
		Player.statDefense += 12;
		if (Main.dayTime)
			Player.GetDamage(DamageClass.Generic) += 0.15f;
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_PearlWoodArmor(target);
	}
	private void OnHitNPC_PearlWoodArmor(NPC npc) {
		if (pearlWoodArmorCD <= 0) {
			int dmg = 12;
			if (Player.ZoneHallow) {
				dmg += 35;
			}
			for (int i = 0; i < 6; i++) {
				Vector2 pos = npc.Center + new Vector2(0, -20).Vector2DistributeEvenly(6, 360, i) * 10;
				Vector2 vel = npc.Center - pos;
				Projectile.NewProjectile(Player.GetSource_OnHit(npc), pos, vel.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<pearlSwordProj>(), dmg, 1, Player.whoAmI);
			}
			pearlWoodArmorCD = BossRushUtils.ToSecond(4);
		}
	}
}
