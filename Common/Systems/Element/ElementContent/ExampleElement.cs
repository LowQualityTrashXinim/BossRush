using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace BossRush.Common.Systems.Element.ElementContent;
internal class ExampleElement : Element {
	//This is a example element where it will teach you the most basic stuff
	//First we will set data
	public override void SetDefault() {
		//We will mainly uses this to set data
		ElementSystem.AssignedNPC(NPCID.Demon, Type, 1.2f, true);
		//Let break down this method
		//This method allow dev to easily assigned data during load time
		//ElementSystem.AssingedNPC take 4 parameters, NPCID | Element ID | Element resistance/weakness value | Whenever or not the NPC have this element
		//Currently we are assigning this element to Demon NPC and give this Demon have 1.2x damage multiplier whenever it taken damage from this element
		//It is totally possible to set NPCHasElem parameter to false but that would cause this element to do nothing with that NPC
		//Pleases referred to this site if you don't have VS https://terraria.wiki.gg/wiki/NPC_IDs so that you can assigned NPCID

		//You don't really need to assign any value to NPCHasElem parameter because it is automatically assigned that for you
		ElementSystem.AssignedNPC(NPCID.FireImp, Type, 1.4f);
		ElementSystem.AssignedNPC(NPCID.LavaSlime, Type, 1.5f);

		//I recommend to seperate ElementSystem.AssignedNPC and ElementSystem.AssignedItem into 2 different paragraph, do note that they still have to be in this method
		ElementSystem.AssignedItem(ItemID.FieryGreatsword, Type);
		ElementSystem.AssignedItem(ItemID.Muramasa, Type);
		//Also referred to this for Item ID https://terraria.wiki.gg/wiki/Item_IDs
	}
	//Moving on to writing custom reaction to element, here I will write you a basic line that will make your life easier
	//You don't need to understand how this line work, you only need to understand how to uses this line ( tho remember to copy them to other element )
	private static bool HasElement<T>(HashSet<ushort> hash) where T : Element {
		return hash.Contains(GetElementType<T>());
	}
	private bool OneInXChance(int X) => Main.rand.NextBool(X);
	private bool PercentageChance(int Percentage) => Main.rand.NextFloat() >= Percentage * .01f;
	//Now in this method, this is where the magic happen, for now as a example we will do some simple elemental reaction
	public override void OnHitNPC(NPC target, NPC.HitInfo info, HashSet<ushort> hash_elementType) {
		//1st example : Example Element + Fire Element
		if (HasElement<Fire>(hash_elementType)) {
			//Let do some funny stuff
			if (OneInXChance(10)) {
				target.Heal(100);
				target.defense -= 1;
			}
		}
		//This will make it so that whenever this element react with fire element,
		//there are 1 in 10 chance to heal that NPC for 100 life but decreases their defenses permanently by 1

		//Let move to 2nd example

		//2nd example : Example Element + Water Element
		if (HasElement<Water>(hash_elementType)) {
			if (PercentageChance(50)) {
				target.defense += 1;
				target.AddBuff(BuffID.Wet, 60 * 10);
			}
		}
		//This will make it so that there are 50% chance when this element react with water element
		//the target will get 1 defeneses permanently but are given Wet debuff for 10s
		//the add buff method wortk in tick so you will need to do 60 * 10 or do BossRushUtils.ToSecond(10) so that it work like you expected
	}
}
