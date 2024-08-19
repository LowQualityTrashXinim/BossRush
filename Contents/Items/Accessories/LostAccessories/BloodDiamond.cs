using System;
using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class BloodDiamond: ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		player.GetModPlayer<BloodDiamondPlayer>().BloodDiamond = true;
	}
}
public class BloodDiamondPlayer : ModPlayer {
	public bool BloodDiamond = false;
	public int Stack = 0;
	public int Decay = 0;
	public override void ResetEffects() {
		BloodDiamond = false;
		Decay = BossRushUtils.CountDown(Decay);
		if(Stack > 0 && Decay <= 0) {
			Stack--;
			Decay = 150;
		}
	}
	public override void UpdateEquips() {
		if (Stack <= 0) {
			return;
		}
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, Additive: 1 + Stack * .2f);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 + Stack * .1f);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: 1 + Stack * .06f);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if(!BloodDiamond) {
			return;
		}
		if(hit.Crit) {
			Stack = Math.Clamp(Stack + 1, 0, 5);
			Decay = 150;
		}
	}
}
