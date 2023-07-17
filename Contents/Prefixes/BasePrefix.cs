using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace BossRush.Contents.Prefixes
{
    public abstract class BasePrefix : ModPrefix, IBossRushPrefix
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
    }
}
