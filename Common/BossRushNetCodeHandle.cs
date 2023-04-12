using System.IO;
using Terraria;
using BossRush.Common.ExtraChallenge;
using BossRush.Contents.Items.Artifact;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Potion;

namespace BossRush.Common
{
    partial class BossRushNetCodeHandle
    {
        internal enum MessageType : byte
        {
            SkillIssuePlayer,
            DrugSyncPlayer,
            KingSlimeNoHit,
            EoCNoHit,
            EoWNoHit,
            BoCNoHit,
            GambleAddiction,
            ExtraChallenge
        }
        public void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            byte playernumber = reader.ReadByte();
            switch (msgType)
            {
                case MessageType.DrugSyncPlayer:
                    WonderDrugPlayer DrugDealing = Main.player[playernumber].GetModPlayer<WonderDrugPlayer>();
                    DrugDealing.DrugDealer = reader.ReadInt32();
                    break;
                case MessageType.SkillIssuePlayer:
                    SkillIssuedArtifactPlayer SkillISsue = Main.player[playernumber].GetModPlayer<SkillIssuedArtifactPlayer>();
                    SkillISsue.SkillIssue = reader.ReadInt32();
                    break;
                case MessageType.KingSlimeNoHit:
                    KingSlimeNoHit KSNOHIT = Main.player[playernumber].GetModPlayer<KingSlimeNoHit>();
                    KSNOHIT.KS0hit = reader.ReadInt32();
                    break;
                case MessageType.EoCNoHit:
                    EoCNoHit EOC = Main.player[playernumber].GetModPlayer<EoCNoHit>();
                    EOC.EoC0hit = reader.ReadInt32();
                    break;
                case MessageType.EoWNoHit:
                    EoWNoHit EoW = Main.player[playernumber].GetModPlayer<EoWNoHit>();
                    EoW.EoW0Hit = reader.ReadInt32();
                    break;
                case MessageType.BoCNoHit:
                    BoCNoHit BoC = Main.player[playernumber].GetModPlayer<BoCNoHit>();
                    BoC.BoC0Hit = reader.ReadInt32();
                    break;
                case MessageType.GambleAddiction:
                    GamblePlayer gamble = Main.player[playernumber].GetModPlayer<GamblePlayer>();
                    gamble.GambleDamage = reader.ReadInt32();
                    gamble.GambleDef = reader.ReadInt32();
                    gamble.GambleSpeed = reader.ReadInt32();
                    gamble.GambleHP = reader.ReadInt32();
                    gamble.GambleLifeRegen = reader.ReadInt32();
                    gamble.GambleMana = reader.ReadInt32();
                    gamble.GambleManaRegen = reader.ReadInt32();
                    gamble.GambleMinionSlot = reader.ReadInt32();
                    gamble.GambleCrit = reader.ReadInt32();
                    break;
                case MessageType.ExtraChallenge:
                    ExtraChallengePlayer extraChallenge = Main.player[playernumber].GetModPlayer<ExtraChallengePlayer>();
                    extraChallenge.ChallengeChooser = reader.ReadInt32();
                    break;
            }
        }
    }
}
