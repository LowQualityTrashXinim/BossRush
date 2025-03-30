using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Consumable.Scroll {
	class ScrollofStrike : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<StrikeSpell>(), BossRushUtils.ToMinute(1));
			Item.Set_ItemIsRPG();
		}
	}
	public class StrikeSpell : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, 1.1f);
		}
	}
	public class StrikePlayer : ModPlayer {
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if (Player.HasBuff<StrikeSpell>()) {
				Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<StrikeSpell>()));
				for (int i = 0; i < 10; i++) {
					Player.StrikeNPCDirect(target, hit);
				}
			}
		}
	}
}
