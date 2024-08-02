using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class StealthCloak : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<StealthCloakPlayer>().StealthCloak = true;
		if(player.invis) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, 1.35f);
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.15f);
		}
	}
}
class StealthCloakPlayer : ModPlayer {
	public bool StealthCloak = false;
	int InvisCooldown = 0;
	public override void ResetEffects() {
		StealthCloak = false;
	}
	public override void UpdateEquips() {
		if(StealthCloak && (--InvisCooldown <= 0 || Player.HasBuff(BuffID.Invisibility))) {
			Player.AddBuff(BuffID.Invisibility, 60);
			InvisCooldown = BossRushUtils.ToSecond(15);
		}
	}
	public override bool FreeDodge(Player.HurtInfo info) {
		if(info.Dodgeable) {
			return base.FreeDodge(info);
		}
		return StealthCloak && Player.HasBuff(BuffID.Invisibility) && Main.rand.NextBool(15);
	}
}
