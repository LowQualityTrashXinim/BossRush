using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Artifact
{
    internal class GodDice : ModItem, IArtifactItem
    {
        public bool CanBeCraft => false;

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 7));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.width = 60;
            Item.height = 73;
            Item.rare = 9;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            GamblePlayer gamblePlayer = player.GetModPlayer<GamblePlayer>();
            tooltips.Add(new TooltipLine(Mod, "", "Damage Multiply : " + gamblePlayer.GambleDamage + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Defense Multiply : " + gamblePlayer.GambleDef + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Speed Multiply : " + gamblePlayer.GambleSpeed + ""));
            tooltips.Add(new TooltipLine(Mod, "", "HP Multiply : " + gamblePlayer.GambleHP + ""));
            tooltips.Add(new TooltipLine(Mod, "", "HP Regen Multiply : " + gamblePlayer.GambleLifeRegen + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Mana Multiply : " + gamblePlayer.GambleMana + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Mana Regen Multiply : " + gamblePlayer.GambleManaRegen + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Extra minion : " + gamblePlayer.GambleMinionSlot + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Crit chance : " + gamblePlayer.GambleCrit + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Rerolls Available : " + gamblePlayer.Roll + ""));
        }
        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<GamblePlayer>().Roll > 0 || player.name.Contains("Test") || player.name.Contains("Debug") || player.name == "LowQualityTrashXinim";
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = -1;
            GamblePlayer gamblePlayer = player.GetModPlayer<GamblePlayer>();
            if ((player.name.Contains("Test") || player.name.Contains("Debug") || player.name == "LowQualityTrashXinim") && player.altFunctionUse == 2)
            {
                gamblePlayer.Roll++;
                gamblePlayer.GambleDamage = 1;
                gamblePlayer.GambleDef = 0;
                gamblePlayer.GambleSpeed = 1;
                gamblePlayer.GambleHP = 1;
                gamblePlayer.GambleLifeRegen = 1;
                gamblePlayer.GambleMana = 1;
                gamblePlayer.GambleManaRegen = 1;
                gamblePlayer.GambleMinionSlot = 0;
                gamblePlayer.GambleCrit = 0;
                return true;
            }
            if (gamblePlayer.Roll > 0) gamblePlayer.Roll--;

            gamblePlayer.GambleDamage = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            gamblePlayer.GambleDef = Main.rand.Next(-100, 100);
            gamblePlayer.GambleSpeed = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            gamblePlayer.GambleHP = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            gamblePlayer.GambleLifeRegen = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            gamblePlayer.GambleMana = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            gamblePlayer.GambleManaRegen = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            gamblePlayer.GambleMinionSlot = Main.rand.Next(0, 10);
            gamblePlayer.GambleCrit = Main.rand.Next(0, 100);
            return true;
        }
    }
    class GamblePlayer : ModPlayer
    {
        public float GambleDamage = 1;
        public float GambleDef = 0;
        public float GambleSpeed = 1;
        public float GambleHP = 1;
        public float GambleLifeRegen = 1;
        public float GambleMana = 1;
        public float GambleManaRegen = 1;
        public int GambleMinionSlot = 0;
        public int GambleCrit = 0;
        public int Roll = 0;
        public override void ResetEffects()
        {
            Player.GetDamage(DamageClass.Generic) *= GambleDamage;
            Player.statDefense += (int)GambleDef;
            Player.accRunSpeed *= GambleSpeed;
            Player.statLifeMax2 = (int)(GambleHP * Player.statLifeMax);
            Player.lifeRegen = (int)(GambleLifeRegen * Player.lifeRegen);
            Player.statManaMax2 = (int)(GambleMana * Player.statManaMax);
            Player.manaRegenDelay = (int)(Player.manaRegenDelay * GambleManaRegen);
            Player.manaRegenDelayBonus = (int)(Player.manaRegenDelayBonus * GambleManaRegen);
            Player.manaRegenCount = (int)(Player.manaRegenCount * GambleManaRegen);
            Player.manaRegenBonus = (int)(Player.manaRegenBonus * GambleManaRegen);
            Player.manaRegen = (int)(Player.manaRegen * GambleManaRegen);
            Player.maxMinions += GambleMinionSlot;
            Player.maxTurrets += GambleMinionSlot;
            Player.GetCritChance(DamageClass.Generic) += GambleCrit;
        }
        public override void NaturalLifeRegen(ref float regen)
        {
            regen *= GambleLifeRegen;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.GambleAddiction);
            packet.Write((byte)Player.whoAmI);
            packet.Write(GambleDamage);
            packet.Write(GambleDef);
            packet.Write(GambleSpeed);
            packet.Write(GambleHP);
            packet.Write(GambleLifeRegen);
            packet.Write(GambleMana);
            packet.Write(GambleManaRegen);
            packet.Write(GambleMinionSlot);
            packet.Write(GambleCrit);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["GamblePlayer"] = GambleDamage;
            tag["GambleDef"] = GambleDef;
            tag["GambleSpeed"] = GambleSpeed;
            tag["GambleHP"] = GambleHP;
            tag["GambleLifeRegen"] = GambleLifeRegen;
            tag["GambleMana"] = GambleMana;
            tag["GambleManaRegen"] = GambleManaRegen;
            tag["GambleMinionSlot"] = GambleMinionSlot;
            tag["GambleCrit"] = GambleCrit;
        }

        public override void LoadData(TagCompound tag)
        {
            GambleDamage = (float)tag["GamblePlayer"];
            GambleDef = (float)tag["GambleDef"];
            GambleSpeed = (float)tag["GambleSpeed"];
            GambleHP = (float)tag["GambleHP"];
            GambleLifeRegen = (float)tag["GambleLifeRegen"];
            GambleMana = (float)tag["GambleMana"];
            GambleManaRegen = (float)tag["GambleManaRegen"];
            GambleMinionSlot = (int)tag["GambleMinionSlot"];
            GambleCrit = (int)tag["GambleCrit"];
        }
    }
}