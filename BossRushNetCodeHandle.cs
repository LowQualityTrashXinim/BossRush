using Terraria;
using System.IO;
using Terraria.ID;
using BossRush.Contents.Perks;
using BossRush.Contents.Skill;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Systems.SpoilSystem;
using BossRush.Contents.Items.Consumable.Potion;
using BossRush.Contents.Items.Consumable.SpecialReward;
using BossRush.Contents.Transfixion.Artifacts;
using BossRush.Common.Global;

namespace BossRush {
	partial class BossRush {
		internal enum MessageType : byte {
			SkillIssuePlayer,
			DrugSyncPlayer,
			NoHitBossNum,
			GambleAddiction,
			GodUltimateChallenge,
			Perk,
			Skill,
			Artifact,
			SpoilsThatYetToChoose,
			PlayerStatsHandle
		}
		public override void HandlePacket(BinaryReader reader, int whoAmI) {
			MessageType msgType = (MessageType)reader.ReadByte();
			byte playernumber = reader.ReadByte();
			switch (msgType) {
				case MessageType.NoHitBossNum:
					NoHitPlayerHandle nohitplayer = Main.player[playernumber].GetModPlayer<NoHitPlayerHandle>();
					nohitplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						nohitplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.SkillIssuePlayer:
					SkillIssuedArtifactPlayer SkillISsue = Main.player[playernumber].GetModPlayer<SkillIssuedArtifactPlayer>();
					SkillISsue.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						SkillISsue.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.DrugSyncPlayer:
					WonderDrugPlayer drugplayer = Main.player[playernumber].GetModPlayer<WonderDrugPlayer>();
					drugplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						drugplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.GambleAddiction:
					GamblePlayer gamble = Main.player[playernumber].GetModPlayer<GamblePlayer>();
					gamble.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						gamble.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.GodUltimateChallenge:
					ModdedPlayer moddedplayer = Main.player[playernumber].GetModPlayer<ModdedPlayer>();
					moddedplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						moddedplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.Perk:
					PerkPlayer perkplayer = Main.player[playernumber].GetModPlayer<PerkPlayer>();
					perkplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						perkplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.Skill:
					SkillHandlePlayer skillplayer = Main.player[playernumber].GetModPlayer<SkillHandlePlayer>();
					skillplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						skillplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.Artifact:
					ArtifactPlayer artifactPlayer = Main.player[playernumber].GetModPlayer<ArtifactPlayer>();
					artifactPlayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						artifactPlayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.SpoilsThatYetToChoose:
					SpoilsPlayer spoilplayer = Main.player[playernumber].GetModPlayer<SpoilsPlayer>();
					spoilplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						spoilplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				case MessageType.PlayerStatsHandle:
					PlayerStatsHandle statplayer = Main.player[playernumber].GetModPlayer<PlayerStatsHandle>();
					statplayer.ReceivePlayerSync(reader);
					if (Main.netMode == NetmodeID.Server) {
						statplayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
			}
		}
	}
}
