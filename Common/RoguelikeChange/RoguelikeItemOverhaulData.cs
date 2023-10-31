using System;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace BossRush.Common.RoguelikeChange;
internal class RoguelikeItemOverhaulData : ModSystem {
	public HashSet<ArmorSet> ArmorHashSet = new HashSet<ArmorSet>();
	public override void SetStaticDefaults() {
		ArmorHashSet.Add(new WoodArmorSet());
		ArmorHashSet.Add(new BorealWoodArmorSet());
		ArmorHashSet.Add(new RichMahoganyWoodArmorSet());
		ArmorHashSet.Add(new ShadeWoodArmorSet());
		ArmorHashSet.Add(new EbonWoodArmorSet());
		ArmorHashSet.Add(new AshWoodArmorSet());
		ArmorHashSet.Add(new CactusArmorSet());
		ArmorHashSet.Add(new PalmWoodArmorSet());
		ArmorHashSet.Add(new PumpkinArmorSet());
		ArmorHashSet.Add(new TinArmorSet());
		ArmorHashSet.Add(new LeadArmorSet());
		ArmorHashSet.Add(new CopperArmorSet());
		ArmorHashSet.Add(new PearlwoodSet());
		ArmorHashSet.Add(new IronArmorSet());
		ArmorHashSet.Add(new SilverArmorSet());
		ArmorHashSet.Add(new TungstenArmorSet());
		ArmorHashSet.Add(new GoldArmorSet());
		ArmorHashSet.Add(new PlatinumArmorSet());
		base.SetStaticDefaults();
	}
	public override void Unload() {
		ArmorHashSet = null;
	}
}
public class WoodArmorSet : ArmorSet {
	public WoodArmorSet() : base(ItemID.WoodHelmet, ItemID.WoodBreastplate, ItemID.WoodGreaves) {
		ArmorSetBonusToolTip = "When in forest biome :" +
				   "\nIncrease defense by 11" +
				   "\nIncrease movement speed by 25%" +
				   "\nYour attack have 25% chance to drop down a acorn dealing 10 damage";
	}
}
public class BorealWoodArmorSet : ArmorSet {
	public BorealWoodArmorSet() : base(ItemID.BorealWoodHelmet, ItemID.BorealWoodBreastplate, ItemID.BorealWoodGreaves) {
		ArmorSetBonusToolTip = "When in snow biome :" +
				   "\nIncrease defense by 13" +
				   "\nIncrease movement speed by 20%" +
				   "\nYou are immune to Chilled" +
				   "\nYour attack have 10% chance to inflict frost burn for 10 second";
	}
}
public class RichMahoganyWoodArmorSet : ArmorSet {
	public RichMahoganyWoodArmorSet() : base(ItemID.RichMahoganyHelmet, ItemID.RichMahoganyBreastplate, ItemID.RichMahoganyGreaves) {
		ArmorSetBonusToolTip = "When in jungle biome :" +
				   "\nIncrease defense by 12" +
				   "\nIncrease movement speed by 30%" +
				   "\nGetting hit release sharp leaf around you that deal 12 damage";
	}
}
public class ShadeWoodArmorSet : ArmorSet {
	public ShadeWoodArmorSet() : base(ItemID.ShadewoodHelmet, ItemID.ShadewoodBreastplate, ItemID.ShadewoodGreaves) {
		ArmorSetBonusToolTip = "When in crimson biome :" +
				   "\nIncrease defense by 17" +
				   "\nIncrease movement speed by 15%" +
				   "\nIncrease critical strike chance by 5" +
				   "\nIncrease life regen by 1" +
				   "\nWhenever you strike a enemy :" +
				   "\nA ring of crimson burst out that deal fixed 10 damage and heal you for each enemy hit and debuff them with ichor";
	}
}
public class EbonWoodArmorSet : ArmorSet {
	public EbonWoodArmorSet() : base(ItemID.EbonwoodHelmet, ItemID.EbonwoodBreastplate, ItemID.EbonwoodGreaves) {
		ArmorSetBonusToolTip = "When in corruption biome :" +
					"\nIncrease defense by 6" +
					"\nIncrease movement speed by 35%" +
					"\nIncrease damage by 5%" +
					"\nYou leave a trail of corruption that deal 3 damage and inflict cursed inferno for 2s";
	}
}
public class AshWoodArmorSet : ArmorSet {
	public AshWoodArmorSet() : base(ItemID.AshWoodHelmet, ItemID.AshWoodBreastplate, ItemID.AshWoodGreaves) {
		ArmorSetBonusToolTip = "Increase defense by 16" +
				   "\nIncrease damage by 10%" +
				   "\nWhen in underworld or underground caven level :" +
				   "\nGetting hit fires a burst of flames at the attacker, dealing from 5 to 15 damage" +
				   "\nAll attacks inflicts On Fire! for 5 seconds" +
				   "\nIncreased life regen by 1";
	}
}
public class CactusArmorSet : ArmorSet {
	public CactusArmorSet() : base(ItemID.CactusHelmet, ItemID.CactusBreastplate, ItemID.CactusLeggings) {
		ArmorSetBonusToolTip = "Increase defenses by 10" +
				   "\nGetting hit will drop down a rolling cactus that is friendly with 5s cool down" +
				   "\nGetting hit will shoot out 8 cactus spike that is friendly deal 15 damage";
	}
}
public class PalmWoodArmorSet : ArmorSet {
	public PalmWoodArmorSet() : base(ItemID.PalmWoodHelmet, ItemID.PalmWoodBreastplate, ItemID.PalmWoodGreaves) {
		ArmorSetBonusToolTip = "Increase defense by 10" +
					   "\nIncrease movement speed by 17%" +
					   "\nJumping will leave a trail of sand that deal 12 damage";
	}
}
public class PumpkinArmorSet : ArmorSet {
	public PumpkinArmorSet() : base(ItemID.PumpkinHelmet, ItemID.PumpkinBreastplate, ItemID.PumpkinLeggings) {
		ArmorSetBonusToolTip = "When in overworld :" +
					   "\nGrant well fed buff for 5s on getting hit" +
					   "\nhitting enemies has 25% to inflict pumpkin overdose" +
					   "\ninflicting the same debuff to an enemy who already has it " +
					   "\ncauses an explosion, dealing 5 + 5% of damage dealt" +
					   "\nWhile below 20% HP, you gain 5x health regen";
	}
}
public class TinArmorSet : ArmorSet {
	public TinArmorSet() : base(ItemID.TinHelmet, ItemID.TinChainmail, ItemID.TinGreaves) {
		ArmorSetBonusToolTip = "Increase defense by 5" +
						"\nIncrease movement speed by 21%" +
						"\nVanilla tin weapon are stronger";
	}
}
public class LeadArmorSet : ArmorSet {
	public LeadArmorSet() : base(ItemID.LeadHelmet, ItemID.LeadChainmail, ItemID.LeadGreaves) {
		ArmorSetBonusToolTip = "Increase defense by 7" +
						"\nYour attack can inflict irradiation poison" +
						"\nLead irradiation increase enemy defense by 20 but deal 50 DoT";
	}
}
public class CopperArmorSet : ArmorSet {
	public CopperArmorSet() : base(ItemID.CopperHelmet, ItemID.CopperChainmail, ItemID.CopperGreaves) {
		ArmorSetBonusToolTip = "Increase movement speed by 15%" +
					   "\nEvery 50 hit to enemy grants you the over charged for 5s" +
					   "\nDuring the rain, your hit requirement reduce by half" +
					   "\nOver charged: Increases movement speed, weapon speed, damage by 10% and create a aura that damage enemy";
	}
}
public class PearlwoodSet : ArmorSet {
	public PearlwoodSet() : base(ItemID.PearlwoodHelmet, ItemID.PearlwoodBreastplate, ItemID.PearlwoodGreaves) {
		ArmorSetBonusToolTip = "Increase movement speed by 35%" +
						"\nAttacking an enemy summons 6 hallow Swords that deals 5 damage with 4 seconds cooldown" +
						"\nIncrease damage by 15% during day" +
						"\nIncrease defense By 12" +
						"\nWhen in Hallow biome:" +
						"\n Hallow Swords deal 15 damage";
	}
}
public class IronArmorSet : ArmorSet {
	public IronArmorSet() : base(ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves) {
		ArmorSetBonusToolTip = "Increase damage reduction 2.5%" +
					   "\nIncrease defense effectivness by 10%" +
					   "\nIncrease damage by 5%" +
					   "\nDecrease movement speed 5%" +
					   "\nWhile under 50% HP, gain 15 bonus defense, but -10% less attack speed";
	}
}
public class SilverArmorSet : ArmorSet {
	public SilverArmorSet() : base(ItemID.SilverHelmet, ItemID.SilverChainmail, ItemID.SilverGreaves) {
		ArmorSetBonusToolTip = "During the day :" +
					   "\nGain 10 defense" +
					   "\nDuring the night :" +
					   "\nIncrease damage by 10%" +
					   "\nAt full life, these effects are multiply by 2";
	}
}
public class TungstenArmorSet : ArmorSet {
	public TungstenArmorSet() : base(ItemID.TungstenHelmet, ItemID.TungstenChainmail, ItemID.TungstenGreaves) {
		ArmorSetBonusToolTip = "Increase defense by 15" +
					   "\nWhen at full hp :" +
					   "\nReduce your defense down to 0" +
					   "\nIncrease speed by 30%" +
					   "\nThe closer your enemy is, the more damage increases";
	}
}
public class GoldArmorSet : ArmorSet {
	public GoldArmorSet() : base(ItemID.GoldHelmet, ItemID.GoldChainmail, ItemID.GoldGreaves) {
		ArmorSetBonusToolTip = "Your attack have 15% to inflict Midas for 10 seconds" +
					   "\nAttacking enemies with midas debuff will : " +
					   "\nDeal additional damage based on their defense";
	}
}
public class PlatinumArmorSet : ArmorSet {
	public PlatinumArmorSet() : base(ItemID.PlatinumHelmet, ItemID.PlatinumChainmail, ItemID.PlatinumGreaves) {
		ArmorSetBonusToolTip = "Increase weapon uses speed by 35%" +
					   "\nAttacking too much will lit on fire";
	}
}
public class ArmorSet {
	public int headID, bodyID, legID;
	protected string ArmorSetBonusToolTip = "";
	public ArmorSet(int headID, int bodyID, int legID) {
		this.headID = headID;
		this.bodyID = bodyID;
		this.legID = legID;
	}
	public string AttemptToGetToolTip() =>
		ModContent.GetInstance<RoguelikeItemOverhaulData>().ArmorHashSet.Where(a => a.headID == headID && a.bodyID == bodyID && a.legID == legID).FirstOrDefault().ArmorSetBonusToolTip;

	public static string ConvertIntoArmorSetFormat(int headID, int bodyID, int legID) => $"{headID}:{bodyID}:{legID}";
	/// <summary>
	/// Expect there is only 3 item in a array
	/// </summary>
	/// <param name="armor"></param>
	/// <returns></returns>
	public static string ConvertIntoArmorSetFormat(int[] armor) => $"{armor[0]}:{armor[1]}:{armor[2]}";
	public override string ToString() => $"{headID}:{bodyID}:{legID}";

	public bool ContainAnyOfArmorPiece(int type) => type == headID || type == bodyID || type == legID;
}
