using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.Consumable.Potion;
internal class KeenPotion : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<FireResistanceBuff>(), BossRushUtils.ToMinute(2));
		Item.Set_AdvancedBuffItem();
	}
}
public class KeenBuff : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, 1f);
	}
}
public class KeenPlayer : ModPlayer {
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if(Player.HasBuff<KeenBuff>()) {
			modifiers.SetCrit();
			int buffindex = Player.FindBuffIndex(ModContent.BuffType<KeenBuff>());
			Player.buffTime[buffindex] -= BossRushUtils.ToSecond(30);
		}
	}
}
