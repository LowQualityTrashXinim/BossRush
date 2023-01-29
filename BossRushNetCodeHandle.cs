using System.IO;
using Terraria;
using Terraria.ModLoader.Utilities;
using BossRush.Items.CustomPotion;
using BossRush.Items.Artifact;
using BossRush.Items.NohitReward;
using BossRush.ExtraChallengeConfig;
using BossRush.Items.Toggle;

namespace BossRush
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

			if (msgType == MessageType.DrugSyncPlayer)
			{
				byte playernumber = reader.ReadByte();
				WonderDrugPlayer DrugDealing = Main.player[playernumber].GetModPlayer<WonderDrugPlayer>();
				DrugDealing.DrugDealer = reader.ReadInt32();
			}
			if (msgType == MessageType.SkillIssuePlayer)
			{
				byte playernumber = reader.ReadByte();
				SkillIssuedArtifactPlayer SkillISsue = Main.player[playernumber].GetModPlayer<SkillIssuedArtifactPlayer>();
				SkillISsue.SkillIssue= reader.ReadInt32();
			}
			if (msgType == MessageType.EoCNoHit)
			{
				byte playernumber = reader.ReadByte();
				EoCNoHit EOC = Main.player[playernumber].GetModPlayer<EoCNoHit>();
				EOC.EoC0hit = reader.ReadInt32();
			}
			if (msgType == MessageType.KingSlimeNoHit)
			{
				byte playernumber = reader.ReadByte();
				KingSlimeNoHit KSNOHIT = Main.player[playernumber].GetModPlayer<KingSlimeNoHit>();
				KSNOHIT.KS0hit = reader.ReadInt32();
			}
            if (msgType == MessageType.EoWNoHit)
            {
                byte playernumber = reader.ReadByte();
                EoWNoHit EoW = Main.player[playernumber].GetModPlayer<EoWNoHit>();
                EoW.EoW0Hit = reader.ReadInt32();
            }
            if (msgType == MessageType.BoCNoHit)
            {
                byte playernumber = reader.ReadByte();
                BoCNoHit BoC = Main.player[playernumber].GetModPlayer<BoCNoHit>();
                BoC.BoC0Hit = reader.ReadInt32();
            }
            if (msgType == MessageType.GambleAddiction)
			{
				byte playernumber = reader.ReadByte();
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
			}
			if(msgType == MessageType.ExtraChallenge)
            {
				byte playernumber = reader.ReadByte();
				ExtraChallengePlayer extraChallenge = Main.player[playernumber].GetModPlayer<ExtraChallengePlayer>();
				extraChallenge.ChallengeChooser = reader.ReadInt32();
			}
        }
	}
}
