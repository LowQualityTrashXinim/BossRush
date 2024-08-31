using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class DivinityStone : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
		Item.Set_InfoItem(true);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		player.GetModPlayer<DivinityStonePlayer>().DivinityStone = true;
	}
}
public class DivinityStonePlayer : ModPlayer {
	public bool DivinityStone = false;
	public int Booster = 0;
	public int Booster_Decay = 0;
	public override void ResetEffects() {
		DivinityStone = false;
		if (Booster > 0) {
			if (--Booster_Decay <= 0) {
				Booster = 0;
				Booster_Decay = BossRushUtils.ToSecond(8);
			}
		}
	}
	public override void UpdateEquips() {
		if(DivinityStone) {
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Additive: 1 + .1f * Booster, Flat: 2 * Booster);
			modplayer.AddStatsToPlayer(PlayerStats.RegenMana, Additive: 1 + .25f * Booster, Flat: 2 * Booster);
			modplayer.AddStatsToPlayer(PlayerStats.StaticDefense, Additive: 1 + .15f * Booster, Flat: 3 * Booster);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (DivinityStone) {
			if(hit.Crit) {
				Booster = Math.Clamp(Booster + 1, 0, 7);
				Booster_Decay = BossRushUtils.ToSecond(8);
			}
		}
	}
}
