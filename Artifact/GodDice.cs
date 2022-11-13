using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System;

namespace BossRush.Artifact
{
    internal class GodDice : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Give you a roll for each boss kill\n\"Unmei Goddess shall decide your journey\"");
        }
        public override string Texture => "BossRush/MissingTexture";
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.width = 32;
            Item.height = 32;
            Item.rare = 9;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            tooltips.Add(new TooltipLine(Mod, "", "Damage Multiply : "+player.GetModPlayer<GamblePlayer>().GambleDamage + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Defense Multiply : "+player.GetModPlayer<GamblePlayer>().GambleDef+ ""));
            tooltips.Add(new TooltipLine(Mod, "", "Speed Multiply : "+ player.GetModPlayer<GamblePlayer>().GambleSpeed + ""));
            tooltips.Add(new TooltipLine(Mod, "", "HP Multiply : " + player.GetModPlayer<GamblePlayer>().GambleHP + ""));
            tooltips.Add(new TooltipLine(Mod, "", "HP Regen Multiply : " + player.GetModPlayer<GamblePlayer>().GambleLifeRegen + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Mana Multiply : " + player.GetModPlayer<GamblePlayer>().GambleMana + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Mana Regen Multiply : " + player.GetModPlayer<GamblePlayer>().GambleManaRegen + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Extra minion : " + player.GetModPlayer<GamblePlayer>().GambleMinionSlot + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Crit chance : " + player.GetModPlayer<GamblePlayer>().GambleCrit + ""));
            tooltips.Add(new TooltipLine(Mod, "", "Rerolls Available : " + player.GetModPlayer<GamblePlayer>().Roll + ""));
        }
        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<GamblePlayer>().Roll > 0 || player.name.Contains("Test") || player.name.Contains("Debug");
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if((player.name.Contains("Test") || player.name.Contains("Debug")) && player.altFunctionUse == 2)
            {
                player.GetModPlayer<GamblePlayer>().Roll++;
                player.GetModPlayer<GamblePlayer>().GambleDamage = 1;
                player.GetModPlayer<GamblePlayer>().GambleDef = 1;
                player.GetModPlayer<GamblePlayer>().GambleSpeed = 1;
                player.GetModPlayer<GamblePlayer>().GambleHP = 1;
                player.GetModPlayer<GamblePlayer>().GambleLifeRegen = 1;
                player.GetModPlayer<GamblePlayer>().GambleMana = 1;
                player.GetModPlayer<GamblePlayer>().GambleManaRegen = 1;
                player.GetModPlayer<GamblePlayer>().GambleMinionSlot = 0;
                player.GetModPlayer<GamblePlayer>().GambleCrit = 0;
                return true;
            }
            if(player.GetModPlayer<GamblePlayer>().Roll>0)player.GetModPlayer<GamblePlayer>().Roll--;

            player.GetModPlayer<GamblePlayer>().GambleDamage = (float)Math.Round(Main.rand.NextFloat(.15f, 2f),2);
            player.GetModPlayer<GamblePlayer>().GambleDef = Main.rand.Next(-100, 100);
            player.GetModPlayer<GamblePlayer>().GambleSpeed = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            player.GetModPlayer<GamblePlayer>().GambleHP = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            player.GetModPlayer<GamblePlayer>().GambleLifeRegen = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            player.GetModPlayer<GamblePlayer>().GambleMana = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            player.GetModPlayer<GamblePlayer>().GambleManaRegen = (float)Math.Round(Main.rand.NextFloat(.15f, 2f), 2);
            player.GetModPlayer<GamblePlayer>().GambleMinionSlot = Main.rand.Next(0,10);
            player.GetModPlayer<GamblePlayer>().GambleCrit = Main.rand.Next(0,100);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                .Register();
        }
    }
    class GamblePlayer : ModPlayer
    {
        public float GambleDamage = 1;
        public float GambleDef = 1;
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
            Player.lifeRegen = (int)(GambleLifeRegen*Player.lifeRegen);
            Player.lifeRegenTime = (int)(GambleLifeRegen * Player.lifeRegenTime);
            Player.lifeRegenCount = (int)(GambleLifeRegen * Player.lifeRegenCount);
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
