using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    class BloodButchererEnchantmentDebuff : ModBuff
    {
		public override string Texture => BossRushTexture.EMPTYBUFF;

		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.GrantImmunityWith[Type].Add(BuffID.BloodButcherer);
		}

		public override void Update(NPC npc, ref int buffIndex) {
			Dust.NewDust(npc.Center,2,2,DustID.Blood,Main.rand.Next(-5,6),-10);
			npc.GetGlobalNPC<BloodButchererEnchantmentDebuffGlobalNPC>().bloodButchererDebuff = true;
		}
	}

	class BloodButchererEnchantmentDebuffGlobalNPC : GlobalNPC 
	{
		public bool bloodButchererDebuff = false;

		public override bool InstancePerEntity => true;
		public override void ResetEffects(NPC npc) {
			bloodButchererDebuff = false;
		}
		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (bloodButchererDebuff) 
			{

				npc.lifeRegen -= 20;
			
			}
		}

	}
}
