using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using BossRush.Common.Systems;

namespace BossRush.Common.RoguelikeChange.Prefixes;
internal class RoguelikePrefix : GlobalItem {
	public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand) {
		if (pre == -2 || pre == -1) {

		}
		return base.PrefixChance(item, pre, rand);
	}
	public override bool AllowPrefix(Item item, int pre) {
		if (pre == PrefixID.Legendary || pre == PrefixID.Legendary2 || pre == PrefixID.Mythical || pre == PrefixID.Unreal) {
			return Main.rand.NextBool(10);
		}
		return base.AllowPrefix(item, pre);
	}
	public override int ChoosePrefix(Item item, UnifiedRandom rand) {
		return base.ChoosePrefix(item, rand);
	}
}
public class RoguelikePrefixSystem : ModSystem {
	public override void Load() {
		On_Item.TryGetPrefixStatMultipliersForItem += On_Item_TryGetPrefixStatMultipliersForItem;
	}

	private bool On_Item_TryGetPrefixStatMultipliersForItem(On_Item.orig_TryGetPrefixStatMultipliersForItem orig, Item self, int rolledPrefix, out float dmg, out float kb, out float spd, out float size, out float shtspd, out float mcst, out int crt) {
		dmg = 1f;
		kb = 1f;
		spd = 1f;
		size = 1f;
		shtspd = 1f;
		mcst = 1f;
		crt = 0;
		if (!UniversalSystem.Check_RLOH()) {
			return orig(self, rolledPrefix, out dmg, out kb, out spd, out size, out shtspd, out mcst, out crt);
		}
		switch (rolledPrefix) {
			//Universal
			case PrefixID.Keen:
				crt = 12;
				break;
			case PrefixID.Superior:
				dmg = 15;
				kb = 15;
				crt = 3;
				break;
			case PrefixID.Forceful:
				kb = 30;
				size = 15;
				break;
			case PrefixID.Broken:
				dmg = -30;
				kb = -20;
				//extra attribute
				break;
			case PrefixID.Damaged:
				dmg = -50;
				crt = -5;
				//extra attribute
				break;
			case PrefixID.Shoddy:
				dmg = -20;
				//extra attribute
				break;
			case PrefixID.Hurtful:
				dmg = 20;
				break;
			case PrefixID.Strong:
				dmg = 10;
				kb = 30;
				break;
			case PrefixID.Unpleasant:
				dmg = 10;
				kb = 35;
				crt = 5;
				break;
			case PrefixID.Weak:
				dmg = -10;
				kb = -30;
				//extra attribute
				break;
			case PrefixID.Ruthless:
				dmg = 50;
				kb = -40;
				break;
			case PrefixID.Godly:
				dmg = 25;
				kb = 20;
				crt = 10;
				break;
			case PrefixID.Demonic:
				dmg = 30;
				kb = 10;
				crt = 3;
				break;
			case PrefixID.Zealous:
				crt = 18;
				break;
			//Common
			case PrefixID.Quick:
				spd = 15;
				break;
			case PrefixID.Deadly:
				dmg = 15;
				spd = 10;
				crt = 5;
				break;
			case PrefixID.Agile:
				spd = 30;
				crt = 10;
				break;
			case PrefixID.Nimble:
				spd = 10;
				crt = 5;
				break;
			case PrefixID.Murderous:
				dmg = 23;
				spd = 17;
				kb = 15;
				break;
			case PrefixID.Slow:
				spd = -15;
				break;
			case PrefixID.Sluggish:
				spd = -25;
				break;
			case PrefixID.Lazy:
				spd = -10;
				break;
			case PrefixID.Annoying:
				dmg = -20;
				spd = -30;
				crt = -10;
				break;
			case PrefixID.Nasty:
				dmg = 23;
				spd = 27;
				crt = 8;
				kb = -9;
				break;
			//Sword
			case PrefixID.Large:
				size = 44;
				kb = 20;
				spd = -8;
				break;
			case PrefixID.Massive:
				dmg = 15;
				size = 66;
				kb = 33;
				spd = -18;
				break;
			case PrefixID.Dangerous:
				dmg = 22;
				size = 18;
				kb = 29;
				crt = 7;
				break;
			case PrefixID.Savage:
				dmg = 18;
				size = 15;
				kb = 18;
				break;
			case PrefixID.Sharp:
				dmg = 20;
				crt = 10;
				kb = -30;
				break;
			case PrefixID.Pointy:
				dmg = 10;
				crt = 15;
				kb = -40;
				break;
			case PrefixID.Tiny:
				size = -50;
				spd = 40;
				break;
			case PrefixID.Small:
				size = -20;
				spd = 20;
				break;
			case PrefixID.Terrible:
				dmg = -15;
				kb = -30;
				spd = -25;
				crt = -10;
				break;
			case PrefixID.Dull:
				dmg = -20;
				crt = -20;
				kb = 40;
				break;
			case PrefixID.Unhappy:
				spd = -15;
				kb = -27;
				size = -19;
				break;
			case PrefixID.Bulky:
				size = 40;
				kb = 20;
				dmg = 10;
				spd = -10;
				break;
			case PrefixID.Shameful:
				dmg = -12;
				size = -16;
				kb = 44;
				break;
			case PrefixID.Heavy:
				kb = 104;
				spd = -34;
				break;
			case PrefixID.Light:
				spd = 55;
				dmg = -12;
				kb = -18;
				break;
			//case PrefixID.Legendary:
			//Range
			case PrefixID.Sighted:
				dmg = 22;
				crt = 10;
				break;
			case PrefixID.Rapid:
				spd = 44;
				shtspd = 15;
				break;
			case PrefixID.Hasty:
				spd = 22;
				shtspd = 38;
				break;
			case PrefixID.Intimidating:
				shtspd = 12;
				kb = 22;
				break;
			case PrefixID.Deadly2:
				dmg = 14;
				crt = 10;
				break;
			case PrefixID.Staunch:
				dmg = 22;
				kb = 44f;
				spd = -16;
				break;
			case PrefixID.Powerful:
				dmg = 33;
				kb = 66f;
				spd = -34;
				break;
			case PrefixID.Frenzying:
				dmg = -22;
				spd = 22;
				break;
			//case PrefixID.Unreal:
			//Magic
			case PrefixID.Mystic:
				dmg = 18;
				mcst = 24;
				break;
			case PrefixID.Adept:
				mcst = 33;
				break;
			case PrefixID.Masterful:
				dmg = 20;
				mcst = 15;
				kb = 10;
				break;
			case PrefixID.Intense:
				dmg = 30;
				mcst = -33;
				break;
			case PrefixID.Taboo:
				dmg = 22;
				kb = 11;
				mcst = -27;
				break;
			case PrefixID.Celestial:
				dmg = 22;
				spd = -15;
				kb = 18;
				mcst = 22;
				break;
			case PrefixID.Furious:
				dmg = 33;
				kb = 22;
				mcst = -66;
				break;
			case PrefixID.Manic:
				dmg = -22;
				spd = 23;
				mcst = -33;
				break;
			default:
				return orig(self, rolledPrefix, out dmg, out kb, out spd, out size, out shtspd, out mcst, out crt);
		}
		if (dmg != 1f)
			dmg = dmg * .01f + 1 + 0.02f;
		if (kb != 1f)
			kb = kb * .01f + 1;
		if (spd != 1f)
			spd = 1 - spd * .01f;
		if (size != 1f)
			size = size * .01f + 1;
		if (shtspd != 1f)
			shtspd = shtspd * .01f + 1;
		if (mcst != 1f)
			mcst = 1 - mcst * .01f;

		//if (dmg != 1f && Math.Round(self.damage * dmg) == self.damage)
		//	return false;

		//if (spd != 1f && Math.Round(self.useAnimation * spd) == self.useAnimation)
		//	return false;

		//if (mcst != 1f && Math.Round(self.mana * mcst) == self.mana)
		//	return false;

		//if (kb != 1f && self.knockBack == 0f)
		//	return false;
		return true;
	}
}
