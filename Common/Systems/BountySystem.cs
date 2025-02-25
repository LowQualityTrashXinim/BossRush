using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Systems {
	public class BountyNPCGlobal : GlobalNPC {
		public override bool InstancePerEntity => true;
		public override void SetDefaults(NPC entity) {
			if(entity.type == ModContent.GetInstance<BountySystem>().CurrentTargetBountyNPC) {
				OnBounty = true;
			}
		}
		public bool OnBounty = false;
		public override void OnKill(NPC npc) {
			if(OnBounty) {
				ModContent.GetInstance<BountySystem>().BountyNPCisKilled = true;
			}
		}
	}
	public class BountySystem : ModSystem{
		public int CurrentTargetBountyNPC = -1;
		public bool BountyNPCisKilled = false;
		public override void PostUpdateNPCs() {

		}
	}
}
