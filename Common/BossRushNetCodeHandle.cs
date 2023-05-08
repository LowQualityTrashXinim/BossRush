using System.IO;
using Terraria;
using BossRush.Common.ExtraChallenge;
using BossRush.Contents.Items.Artifact;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Potion;
using BossRush.Common.Global;
using BossRush.Contents.Items.Chest;

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
            ExtraChallenge,
            ArtifactRegister,
            ChanceMultiplayer
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
                    gamble.GambleDamage = reader.ReadSingle();
                    gamble.GambleDef = reader.ReadSingle();
                    gamble.GambleSpeed = reader.ReadSingle();
                    gamble.GambleHP = reader.ReadSingle();
                    gamble.GambleLifeRegen = reader.ReadSingle();
                    gamble.GambleMana = reader.ReadSingle();
                    gamble.GambleManaRegen = reader.ReadSingle();
                    gamble.GambleMinionSlot = reader.ReadInt32();
                    gamble.GambleCrit = reader.ReadInt32();
                    break;
                case MessageType.ExtraChallenge:
                    ExtraChallengePlayer extraChallenge = Main.player[playernumber].GetModPlayer<ExtraChallengePlayer>();
                    extraChallenge.ChallengeChooser = reader.ReadInt32();
                    break;
                case MessageType.ArtifactRegister:
                    ArtifactPlayerHandleLogic artifact = Main.player[playernumber].GetModPlayer<ArtifactPlayerHandleLogic>();
                    artifact.ArtifactDefinedID = reader.ReadInt32();
                    break;
                case MessageType.ChanceMultiplayer:
                    ChestLootDropPlayer chestplayer = Main.player[playernumber].GetModPlayer<ChestLootDropPlayer>();
                    chestplayer.MeleeChanceMutilplier = reader.ReadSingle();
                    chestplayer.RangeChanceMutilplier = reader.ReadSingle();
                    chestplayer.MagicChanceMutilplier = reader.ReadSingle();
                    chestplayer.SummonChanceMutilplier = reader.ReadSingle();
                    break;
            }
        }
    }
}
