using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.ILEditing;
public class SkipLunarEvent : ModSystem {

	public override void Load() {

		IL_NPC.DoDeathEvents += HookDoDeathEvents;

	}

	private void HookDoDeathEvents(ILContext il) {
		try {

			ILCursor c = new ILCursor(il);
			c.GotoNext(i => i.MatchCall(typeof(WorldGen).GetMethod("TriggerLunarApocalypse")));
			c.Remove();

			// make sure towers are considered downed for the sake of compatibilty 
			c.EmitDelegate(() => 
			{

				NPC.downedTowerNebula = true;
				NPC.downedTowerSolar = true;
				NPC.downedTowerStardust = true;
				NPC.downedTowerVortex = true;

			});
		}
		catch (Exception e) {

			MonoModHooks.DumpIL(ModContent.GetInstance<BossRush>(), il);
		}
	}

	public override void Unload() {
		IL_NPC.DoDeathEvents -= HookDoDeathEvents;
	}

}
