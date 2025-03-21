using BossRush.Common.Global;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class KillingThrill : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.FullHPDamage, Additive: 3);
		player.GetModPlayer<KillingThrillPlayer>().KillingThrill = true;
	}
}
class KillingThrillPlayer : ModPlayer {
	public bool KillingThrill = false;
	public int KillCount_Decay = 0;
	public int Decay_CoolDown = 0;
	public override void ResetEffects() {
		KillingThrill = false;
	}
	public override void UpdateEquips() {
		if (KillCount_Decay <= 0) {
			return;
		}
		Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, Base: KillCount_Decay * .05f);
		if (++Decay_CoolDown >= 240) {
			KillCount_Decay = Math.Clamp(KillCount_Decay - 1, 0, 5);
			Decay_CoolDown = 0;
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (KillingThrill) {
			if (KillCount_Decay > 0) {
				damage += KillCount_Decay * .1f;
				return;
			}
			damage -= .25f;
		}
	}
}
