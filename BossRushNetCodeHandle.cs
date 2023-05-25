using Terraria;
using System.IO;
using Terraria.ID;
using BossRush.Common.Global;
using BossRush.Common.ExtraChallenge;
using BossRush.Contents.Items.Artifact;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Card;

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
            ExtraChallenge,
            ArtifactRegister,
            ChanceMultiplayer,
            CardEffect
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            base.HandlePacket(reader, whoAmI);
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
            }
        }
    }
    partial class BossRushNetCodeHandle
    {
        internal enum MessageType : byte
        {
            SkillIssuePlayer,
            DrugSyncPlayer,
            GambleAddiction,
            ExtraChallenge,
            ArtifactRegister,
            ChanceMultiplayer,
            CardEffect
        }
        public void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            byte playernumber = reader.ReadByte();
            switch (msgType)
            {
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
                case MessageType.CardEffect:
                    PlayerCardHandle modplayer = Main.player[playernumber].GetModPlayer<PlayerCardHandle>();
                    modplayer.MeleeDMG = reader.ReadSingle();
                    modplayer.RangeDMG = reader.ReadSingle();
                    modplayer.MagicDMG = reader.ReadSingle();
                    modplayer.SummonDMG = reader.ReadSingle();
                    modplayer.Movement = reader.ReadSingle();
                    modplayer.JumpBoost = reader.ReadSingle();
                    modplayer.HPMax = reader.ReadInt32();
                    modplayer.HPRegen = reader.ReadSingle();
                    modplayer.ManaMax = reader.ReadInt32();
                    modplayer.ManaRegen = reader.ReadSingle();
                    modplayer.DefenseBase = reader.ReadInt32();
                    modplayer.DamagePure = reader.ReadSingle();
                    modplayer.CritStrikeChance = reader.ReadInt32();
                    modplayer.CritDamage = reader.ReadSingle();
                    modplayer.DefenseEffectiveness = reader.ReadSingle();
                    modplayer.DropAmountIncrease = reader.ReadInt32();
                    modplayer.MinionSlot = reader.ReadInt32();
                    modplayer.SentrySlot = reader.ReadInt32();
                    break;
            }
        }
    }
}
