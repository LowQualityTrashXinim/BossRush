using BossRush.Contents.Items.Artifact;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Potion;
using BossRush.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

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
            ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
            PlayerCardHandle cardplayer = Main.LocalPlayer.GetModPlayer<PlayerCardHandle>();
            WonderDrugPlayer drugplayer = Main.LocalPlayer.GetModPlayer<WonderDrugPlayer>();
            NoHitPlayerHandle nohitPlayer = Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>();
            ArtifactPlayerHandleLogic artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>();
            Player player = Main.LocalPlayer;
            TooltipLine line = new TooltipLine(Mod, "Stats", 
                $"Melee Damage : {player.GetTotalDamage(DamageClass.Melee).Flat}" +
                $"\nRange Damage : {player.GetTotalDamage(DamageClass.Ranged).Flat}" +
                $"\nMagic Damage : {player.GetTotalDamage(DamageClass.Magic).Flat}" +
                $"\nSummon Damage : {player.GetTotalDamage(DamageClass.Summon).Flat}" +
                $"\nPure/Generic Damage : {player.GetTotalDamage(DamageClass.Generic).Flat}" +
                $"\nCrit chance : {player.GetCritChance(DamageClass.Generic)}" +
                $"\nCrit damage : {cardplayer.CritDamage}" +
                $"\nMax HP : {player.statLifeMax2}" +
                $"\nMax Mana : {player.statManaMax2}" +
                $"\nHP regen : {player.lifeRegen}" +
                $"\nMana regen : {player.manaRegen}" +
                $"\nDefense : {player.statDefense}" +
                $"\nDefense effectiveness : {player.DefenseEffectiveness.Value}" +
                $"\nMovement speed : {player.accRunSpeed}" +
                $"\nJump speed : {player.jumpSpeedBoost}" +
                $"\nMax minion : {player.maxMinions}" +
                $"\nMax sentry/turret : {player.maxTurrets}" +
                $"\nAmount increase : {chestplayer.amountModifier}" +
                $"\nMelee drop chance : {chestplayer.MeleeChanceMutilplier}" +
                $"\nRange drop chance : {chestplayer.RangeChanceMutilplier}" +
                $"\nMagic drop chance : {chestplayer.MagicChanceMutilplier}" +
                $"\nSummon drop chance : {chestplayer.SummonChanceMutilplier}" +
                $"\nWonder drug consumed rate : {drugplayer.DrugDealer}" +
                $"\nAmount boss no-hit : {nohitPlayer.BossNoHitNumber.Count}" +
                $"\nCurrent active artifact : {artifactplayer.ToStringArtifact()}" 
                );
            tooltips.Add(line);
        }
    }
}