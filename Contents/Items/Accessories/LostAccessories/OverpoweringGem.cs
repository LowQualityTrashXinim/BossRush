using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class OverpoweringGem : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<OverpoweringGemPlayer>().OverpoweringGem = true;
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MaxHP, Base: 10);
		modplayer.AddStatsToPlayer(PlayerStats.MaxMana, Base: 5);
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 2);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.1f);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.05f);
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, 1.15f);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.2f);
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Base:3);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.15f);
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.15f);
		modplayer.AddStatsToPlayer(PlayerStats.LifeStealEffectiveness, 1.05f);
		modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.FullHPDamage, 1.35f);
	}
}
class OverpoweringGemPlayer : ModPlayer {
	public bool OverpoweringGem = false;
	public override void ResetEffects() {
		OverpoweringGem = false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(30) && OverpoweringGem) {
			Player.Heal(Main.rand.Next(3, 7));
		}
	}
	public override bool FreeDodge(Player.HurtInfo info) {
		if (info.Dodgeable) {
			return base.FreeDodge(info);
		}
		return OverpoweringGem && Main.rand.NextFloat() <= .025f;
	}
}
