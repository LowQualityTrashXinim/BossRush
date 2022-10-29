using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword;

namespace BossRush.Note
{
    internal class Note2 : ModItem
    {
        public override string Texture => "BossRush/Note/Note1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Note 02");
            Tooltip.SetDefault(
                "Now that you got the basic gis of the mod, we will explain more about"+$"[i:{ModContent.ItemType<SynergyEnergy>()}]" +
                "\nThis item is needed if you want to get better stuff in the mod, in development i found out player often die due to" +
                "\nlack of dps and the gameplay loop is cool and all but it gotten boring quickly due to how \"balance\" terraria weapon is" +
                "\nThese synergy item is a solution to the problem we having as it introduce more stuff and fun stuff you can use" +
                "\nWith some of it can be use to increase power of certain class or player power overall, all of these stuff are unbalance" +
                "\nThe decision for making weapon to be unbalance as it is intended to be come across randomly by pure chance with some worse than other" +
                "\nThe item can be craft from this can be found in the chest and is intended that way, some example like" + $"[i:{ModContent.ItemType<SynergyEnergy>()}] + [i:{ItemID.CopperShortsword}] + [i:{ItemID.PlatinumBroadsword}] = [i:{ModContent.ItemType<EnchantedSilverSword>()}]" +
                "\nMost synergy item are design with a unique or fun little idea that promote a more vary playstyle, some synergy weapon have lower dps than other" +
                "\nbut it is up to you and decide if it worth to get a better dps weapon than what you have or play the chance game and get a better synergy item" +
                "\nThe way you get synergy weapon is by fighting optional boss that don't matter to normal game progress like" + $"[i:{ItemID.Abeemination}]" +
                "\nYou could also revert the synergy item that you craft back into pure synergy energy form but will lose the item that you craft with it" +
                "\nYou can disable the config in the mod to completely block synergy energy getting involved, we hope that you find that easy to understand");
        }
        public override void SetDefaults()
        {
            Item.width = 41;
            Item.height = 29;
            Item.material = true;
            Item.rare = 0;
        }
    }
}
