using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace BossRush.Contents.Prefixes
{
    internal class BasePrefix : ModPrefix, IBossRushPrefix
    {
        //All of this is set default
        public PrefixCategory WeaponType => PrefixCategory.AnyWeapon;
        public float WeaponRollchance => 5f;
        public virtual float Power => 1f;
        public override PrefixCategory Category => WeaponType;
        public override float RollChance(Item item)
        {
            return WeaponRollchance;
        }

        // Determines if it can roll at all.
        // Use this to control if a prefix can be rolled or not.
        public override bool CanRoll(Item item)
        {
            return true;
        }

        // Modify the cost of items with this modifier with this function.
        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 10f;
        }

        // This is used to modify most other stats of items which have this modifier.
        public override void Apply(Item item)
        {
            //
        }
        public static LocalizedText PowerTooltip { get; private set; }
        // This prefix doesn't affect any non-standard stats, so these additional tooltiplines aren't actually necessary, but this pattern can be followed for a prefix that does affect other stats.
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            // Due to inheritance, this code runs for ExamplePrefix and ExampleDerivedPrefix. We add 2 tooltip lines, the first is the typical prefix tooltip line showing the stats boost, while the other is just some additional flavor text.

            // The localization key for Mods.ExampleMod.Prefixes.PowerTooltip uses a special format that will automatically prefix + or - to the value.
            // This shared localization is formatted with the Power value, resulting in different text for ExamplePrefix and ExampleDerivedPrefix.
            // This results in "+1 Power" for ExamplePrefix and "+2 Power" for ExampleDerivedPrefix.
            // Power isn't an actual stat, the effects of Power are already shown in the "+X% damage" tooltip, so this example is purely educational.
            yield return new TooltipLine(Mod, "BossRushPrefix", PowerTooltip.Format(Power))
            {
                IsModifier = true, // Sets the color to the positive modifier color.
            };
            // This localization is not shared with the inherited classes. ExamplePrefix and ExampleDerivedPrefix have their own translations for this line.
            // If possible and suitable, try to reuse the name identifier and translation value of Terraria prefixes. For example, this code uses the vanilla translation for the word defense, resulting in "-5 defense". Note that IsModifierBad is used for this bad modifier.
            /*yield return new TooltipLine(Mod, "PrefixAccDefense", "-5" + Lang.tip[25].Value) {
				IsModifier = true,
				IsModifierBad = true,
			};*/
        }
    }
}
