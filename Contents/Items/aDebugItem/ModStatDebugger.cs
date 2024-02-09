using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Potion;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Items.aDebugItem
{
    internal class ModStatsDebugger : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.width = Item.height = 10;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
            WonderDrugPlayer drugplayer = Main.LocalPlayer.GetModPlayer<WonderDrugPlayer>();
            NoHitPlayerHandle nohitPlayer = Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>();
			ArtifactPlayer artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
            chestplayer.GetAmount();
            TooltipLine line = new TooltipLine(Mod, "StatsShowcase",
                $"Amount drop chest addition : {chestplayer.amountModifier}" +
                $"\nAmount drop chest multiplication : {chestplayer.finalMultiplier}" +
                $"\nAmount drop chest final weapon : {chestplayer.weaponAmount}" +
                $"\nAmount drop chest final potion type : {chestplayer.potionTypeAmount}" +
                $"\nAmount drop chest final potion amount : {chestplayer.potionNumAmount}" +
                $"\nMelee drop chance : {chestplayer.MeleeChanceMutilplier + chestplayer.UpdateMeleeChanceMutilplier}" +
                $"\nRange drop chance : {chestplayer.RangeChanceMutilplier + chestplayer.UpdateRangeChanceMutilplier}" +
                $"\nMagic drop chance : {chestplayer.MagicChanceMutilplier + chestplayer.UpdateMagicChanceMutilplier}" +
                $"\nSummon drop chance : {chestplayer.SummonChanceMutilplier + chestplayer.UpdateSummonChanceMutilplier}" +
                $"\nWonder drug consumed rate : {drugplayer.DrugDealer}" +
                $"\nAmount boss no-hit : {nohitPlayer.BossNoHitNumber.Count}" +
                $"\nCurrent active artifact : {Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName}"
                );
            tooltips.Add(line);
        }
    }
}
