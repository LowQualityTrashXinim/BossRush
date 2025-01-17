using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff {
	class BloodButchererEnchantmentDebuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;

		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.GrantImmunityWith[Type].Add(BuffID.BloodButcherer);
		}

		public override void Update(NPC npc, ref int buffIndex) {
			Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, Main.rand.Next(-5, 6), -4);
			npc.lifeRegen -= 20;
		}
	}
}
