using System;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.Mechanic;
internal class KnockBackDamage : ModPlayer {
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			modifiers.SourceDamage += item.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			modifiers.SourceDamage += proj.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
		}
	}
}
