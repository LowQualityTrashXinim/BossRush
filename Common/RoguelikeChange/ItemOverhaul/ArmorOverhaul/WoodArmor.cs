using BossRush.Common.Systems;
using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul;
internal class WoodArmor : ModArmor {
	public override void SetDefault() {
		headID = ItemID.WoodHelmet;
		bodyID = ItemID.WoodBreastplate;
		legID = ItemID.WoodGreaves;
	}
}
class WoodArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ModArmor armor = ArmorLoader.GetModArmor("WoodArmor");
		armor.modplayer = this;
		ArmorLoader.HeadDef.Add(armor.HeadID, 3);
		ArmorLoader.BodyDef.Add(armor.BodyID, 4);
		ArmorLoader.LegDef.Add(armor.LegID, 3);
	}
	public override void Armor_ResetEffects() {
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneForest) {
			Player.statDefense += 5;
			Player.moveSpeed += .25f;
		}
	}
	public override void Armor_OnHitWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(4) && (proj is null || proj is not null && proj.ModProjectile is not AcornProjectile)) {
			Projectile.NewProjectile(Player.GetSource_FromThis(),
				target.Center - new Vector2(0, 400),
				Vector2.UnitY * 10,
				ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Player.whoAmI);
		}
	}
	public override void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(4)) {
			Projectile.NewProjectile(Player.GetSource_FromThis(),
				target.Center - new Vector2(0, 400),
				Vector2.UnitY * 10,
				ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Player.whoAmI);
		}
	}
}
