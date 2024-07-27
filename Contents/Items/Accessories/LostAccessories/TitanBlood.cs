using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class TitanBlood : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<TitanBloodPlayer>().TitanBlood = true;
		float damagereduction = 0;
		if(player.ComparePlayerHealthInPercentage(.5f)) {
			damagereduction += .1f;
		}
		if(player.ComparePlayerHealthInPercentage(.35f)) {
			damagereduction += .05f;
		}
		if (player.ComparePlayerHealthInPercentage(.15f)) {
			damagereduction += .05f;
		}
		player.endurance += damagereduction;
	}
}
class TitanBloodPlayer : ModPlayer {
	public bool TitanBlood = false;
	public override void ResetEffects() {
		TitanBlood = false;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		TitanBlock(ref hurtInfo);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		TitanBlock(ref hurtInfo);
	}
	public void TitanBlock(ref Player.HurtInfo hurtInfo) {
		if(Main.rand.NextBool(12) && TitanBlood) {
			hurtInfo.SourceDamage = 1;
			hurtInfo.CooldownCounter += 60;
			hurtInfo.DustDisabled = true;
		}
	}
}
