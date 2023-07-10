using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;

namespace BossRush.Contents.Items.aDebugItem
{
    internal class ShowPlayerStats : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.width = Item.height = 10;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            PlayerCardHandle cardplayer = Main.LocalPlayer.GetModPlayer<PlayerCardHandle>();
            string Curses = "";
            foreach(var curses in cardplayer.listCursesID)
            {
                Curses += $"{{{cardplayer.CursedStringStats(curses)}}}\n";
            }
            TooltipLine line = new TooltipLine(Mod, "StatsShowcase",
                "The below are card stats"+
                $"\nMelee Damage : {cardplayer.MeleeDMG}" +
                $"\nRange Damage : {cardplayer.RangeDMG}" +
                $"\nMagic Damage : {cardplayer.MagicDMG}" +
                $"\nSummon Damage : {cardplayer.SummonDMG}" +
                $"\nPure/Generic Damage : {cardplayer.DamagePure}" +
                $"\nCrit chance : {cardplayer.MeleeDMG}" +
                $"\nCrit damage : {cardplayer.CritDamage}" +
                $"\nMax HP : {cardplayer.HPMax}" +
                $"\nMax Mana : {cardplayer.ManaMax}" +
                $"\nHP regen : {cardplayer.HPRegen}" +
                $"\nMana regen : {cardplayer.ManaRegen}" +
                $"\nDefense : {cardplayer.DefenseBase}" +
                $"\nDefense effectiveness : {cardplayer.DefenseEffectiveness}" +
                $"\nMovement speed : {cardplayer.Movement}" +
                $"\nJump speed : {cardplayer.JumpBoost}" +
                $"\nMax minion : {cardplayer.MinionSlot}" +
                $"\nMax sentry/turret : {cardplayer.SentrySlot}"+
                "\n-Below are curses that you have-\n"+
                Curses
                );
            tooltips.Add(line);
        }
    }
}