using Terraria;
using Terraria.ID;

namespace BossRush.Common.Systems.ArgumentsSystem;

public class Flaming : ModArgument {
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
}

public class FireReactive : ModArgument {
	public override void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { 
		if(target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage += .2f;
		}
	}
}
