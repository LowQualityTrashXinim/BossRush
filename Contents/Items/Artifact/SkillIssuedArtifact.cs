using Terraria;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Artifact
{
    internal class SkillIssuedArtifact : ModItem, IArtifactItem
    {
        public bool CanBeCraft => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinite Growth");
            Tooltip.SetDefault("\"with this, nothing is impossible, even with skill issue in blood stream\"" +
            "\nFor each monster kill, your MaxHP will increase by 0.5 and 0.01% boost toward damage and thorn ( Multiplicative )" +
            "\nTo further increase progress, by default this accessory will make monster spawn increase by CRAZY amount" +
            "\nCapped at 10 million kill so your game don't break, you can thx me for that" +
            "\nOnly work with people have skill issue live up to the name");
        }
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            tooltips.Add(new TooltipLine(Mod, "SkillIssue", "Total Mobs kill : " + player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssue));
            tooltips.Add(new TooltipLine(Mod, "SkillIssueEffect", "Effect : NONE"));
            foreach (var item in tooltips)
            {
                if(item.Name == "SkillIssue")
                {
                    item.OverrideColor = Main.DiscoColor;
                }
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssuePlayer = true;
        }
    }
    public class SkillIssuedArtifactPlayer : ModPlayer
    {
        public int SkillIssue = 0;
        public bool SkillIssuePlayer;
        public override void ResetEffects()
        {
            SkillIssuePlayer = false;
            Player.GetDamage(DamageClass.Generic) *= SkillIssue * 0.01f + 1;
            Player.statLifeMax2 += (int)(SkillIssue * 0.5f);
            Player.thorns *= SkillIssue * 0.01f + 1;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.SkillIssuePlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(SkillIssue);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["SkillIssue"] = SkillIssue;
        }

        public override void LoadData(TagCompound tag)
        {
            SkillIssue = (int)tag["SkillIssue"];
        }
    }

    public class EnemyForSkillIssuePlayer : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            int playerKill = npc.lastInteraction;
            if (!Main.player[playerKill].active || Main.player[playerKill].dead)
            {
                return;
            }
            Player player = Main.player[playerKill];

            if (player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssuePlayer && player.name.ToLower().Contains("skillissue") && player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssue <= 10000000)
            {
                player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssue++;
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssuePlayer)
            {
                spawnRate = 2;
                maxSpawns = 400;
            }
        }
    }
}
