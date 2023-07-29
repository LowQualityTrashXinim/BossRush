using System;
using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Artifact
{
    internal class GodDice : ModItem, IArtifactItem
    {
        public int ArtifactID => 998;

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 14));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.width = 46;
            Item.height = 54;
            Item.rare = ItemRarityID.Cyan;
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
            return player.GetModPlayer<GamblePlayer>().Roll > 0 || BossRushUtils.IsDebugPlayer(player);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            GamblePlayer gamblePlayer = player.GetModPlayer<GamblePlayer>();
            if (BossRushUtils.IsDebugPlayer(player) && player.altFunctionUse == 2)
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
        public float GambleSpeed = 1;
        public float GambleHP = 1;
        public float GambleLifeRegen = 1;
        public float GambleMana = 1;
        public float GambleManaRegen = 1;
        public int GambleDef = 0;
        public int GambleMinionSlot = 0;
        public int GambleCrit = 0;
        public int Roll = 0;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            damage *= GambleDamage;
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            health *= GambleHP;
            mana *= GambleMana;
        }
        public override void ResetEffects()
        {
            Player.statDefense += GambleDef;
            Player.accRunSpeed *= GambleSpeed;
            Player.lifeRegen = (int)(GambleLifeRegen * Player.lifeRegen);
            Player.manaRegen = (int)(Player.manaRegen * GambleManaRegen);
            Player.maxMinions += GambleMinionSlot;
            Player.maxTurrets += GambleMinionSlot;
            Player.GetCritChance(DamageClass.Generic) += GambleCrit;
        }
        public override void NaturalLifeRegen(ref float regen)
        {
            regen *= GambleLifeRegen;
        }
        public override void Initialize()
        {
            GambleDamage = 1;
            GambleDef = 0;
            GambleSpeed = 1;
            GambleHP = 1;
            GambleLifeRegen = 1;
            GambleMana = 1;
            GambleManaRegen = 1;
            GambleMinionSlot = 0;
            GambleCrit = 0;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRush.MessageType.GambleAddiction);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)GambleDamage);
            packet.Write((byte)GambleDef);
            packet.Write((byte)GambleSpeed);
            packet.Write((byte)GambleHP);
            packet.Write((byte)GambleLifeRegen);
            packet.Write((byte)GambleMana);
            packet.Write((byte)GambleManaRegen);
            packet.Write((byte)GambleMinionSlot);
            packet.Write((byte)GambleCrit);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["GambleDmg"] = GambleDamage;
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
            GambleDamage = tag.GetFloat("GambleDmg");
            if (tag.ContainsKey("GambleDef"))
            {
                object GambleDefData = tag["GambleDef"];
                if (GambleDefData is float GambleDefFloat)
                    GambleDef = (int)GambleDefFloat;
                if (GambleDefData is int GambleDefInt)
                    GambleDef = GambleDefInt;
            }
            GambleSpeed = tag.GetFloat("GambleSpeed");
            GambleHP = tag.GetFloat("GambleHP");
            GambleLifeRegen = tag.GetFloat("GambleLifeRegen");
            GambleMana = tag.GetFloat("GambleMana");
            GambleManaRegen = tag.GetFloat("GambleManaRegen");
            GambleMinionSlot = tag.GetInt("GambleMinionSlot");
            GambleCrit = tag.GetInt("GambleCrit");
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            GambleDamage = reader.ReadSingle();
            GambleDef = reader.ReadInt32();
            GambleSpeed = reader.ReadSingle();
            GambleHP = reader.ReadSingle();
            GambleLifeRegen = reader.ReadSingle();
            GambleMana = reader.ReadSingle();
            GambleManaRegen = reader.ReadSingle();
            GambleMinionSlot = reader.ReadInt32();
            GambleCrit = reader.ReadInt32();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            GamblePlayer clone = (GamblePlayer)targetCopy;
            clone.GambleDamage = GambleDamage;
            clone.GambleDef = GambleDef;
            clone.GambleSpeed = GambleSpeed;
            clone.GambleHP = GambleHP;
            clone.GambleLifeRegen = GambleLifeRegen;
            clone.GambleMana = GambleMana;
            clone.GambleManaRegen = GambleManaRegen;
            clone.GambleMinionSlot = GambleMinionSlot;
            clone.GambleCrit = GambleCrit;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            GamblePlayer clone = (GamblePlayer)clientPlayer;
            if (GambleDamage != clone.GambleDamage) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (GambleDef != clone.GambleDef) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (GambleSpeed != clone.GambleSpeed) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (GambleHP != clone.GambleHP) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (GambleLifeRegen != clone.GambleLifeRegen) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (GambleMana != clone.GambleMana) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (GambleManaRegen != clone.GambleManaRegen) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (GambleMinionSlot != clone.GambleMinionSlot) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (GambleCrit != clone.GambleCrit) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
}