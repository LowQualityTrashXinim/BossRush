﻿using Terraria;
using System.IO;
using Terraria.ID;
using BossRush.Common;
using BossRush.Contents.Artifacts;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Toggle;
using BossRush.Contents.Items.NohitReward;

namespace BossRush
{
    partial class BossRush
    {
        internal enum MessageType : byte
        {
            SkillIssuePlayer,
            DrugSyncPlayer,
            NoHitBossNum,
            GambleAddiction,
            ChanceMultiplayer,
            GodUltimateChallenge
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            byte playernumber = reader.ReadByte();
            switch (msgType)
            {
                case MessageType.NoHitBossNum:
                    NoHitPlayerHandle nohitplayer = Main.player[playernumber].GetModPlayer<NoHitPlayerHandle>();
                    nohitplayer.ReceivePlayerSync(reader);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        nohitplayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                case MessageType.SkillIssuePlayer:
                    SkillIssuedArtifactPlayer SkillISsue = Main.player[playernumber].GetModPlayer<SkillIssuedArtifactPlayer>();
                    SkillISsue.ReceivePlayerSync(reader);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        SkillISsue.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                case MessageType.DrugSyncPlayer:
                    WonderDrugPlayer drugplayer = Main.player[playernumber].GetModPlayer<WonderDrugPlayer>();
                    drugplayer.ReceivePlayerSync(reader);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        drugplayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                case MessageType.GambleAddiction:
                    GamblePlayer gamble = Main.player[playernumber].GetModPlayer<GamblePlayer>();
                    gamble.ReceivePlayerSync(reader);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        gamble.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                case MessageType.ChanceMultiplayer:
                    ChestLootDropPlayer chestplayer = Main.player[playernumber].GetModPlayer<ChestLootDropPlayer>();
                    chestplayer.ReceivePlayerSync(reader);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        chestplayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                case MessageType.GodUltimateChallenge:
                    ModdedPlayer moddedplayer = Main.player[playernumber].GetModPlayer<ModdedPlayer>();
                    moddedplayer.ReceivePlayerSync(reader);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        moddedplayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
            }
        }
    }
}
