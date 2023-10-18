using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Terraria;
using BossRush.Common.Systems.ArtifactSystem;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Artifacts {
	internal class SkillIssuedArtifact : Artifact {
		public override bool CanBeSelected(Player player) {
			return player.name.ToLower().Contains("skillissue");
		}

		public override Color DisplayNameColor => Color.Orange;
	}

	public class SkillIssuedArtifactPlayer : ModPlayer {
		public int SkillIssue = 0;
		public bool SkillIssuePlayer = false;
		public override void ResetEffects() {
			SkillIssuePlayer = Player.HasArtifact<SkillIssuedArtifact>();
			Player.GetDamage(DamageClass.Generic) *= SkillIssue * 0.01f + 1;
			Player.statLifeMax2 += (int)(SkillIssue * 0.5f);
			Player.thorns *= SkillIssue * 0.01f + 1;
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.SkillIssuePlayer);
			packet.Write((byte)Player.whoAmI);
			packet.Write(SkillIssue);
			packet.Send(toWho, fromWho);
		}
		public override void Initialize() {
			SkillIssue = 0;
		}
		public override void SaveData(TagCompound tag) {
			tag["SkillIssue"] = SkillIssue;
		}

		public override void LoadData(TagCompound tag) {
			SkillIssue = (int)tag["SkillIssue"];
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			SkillIssue = reader.ReadByte();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			SkillIssuedArtifactPlayer clone = (SkillIssuedArtifactPlayer)targetCopy;
			clone.SkillIssue = SkillIssue;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			SkillIssuedArtifactPlayer clone = (SkillIssuedArtifactPlayer)clientPlayer;
			if (SkillIssue != clone.SkillIssue) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}

	public class EnemyForSkillIssuePlayer : GlobalNPC {
		public override void OnKill(NPC npc) {
			int playerKill = npc.lastInteraction;
			if (!Main.player[playerKill].active || Main.player[playerKill].dead) {
				return;
			}
			Player player = Main.player[playerKill];

			if (player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssuePlayer && player.name.ToLower().Contains("skillissue") && player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssue <= 10000000) {
				player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssue++;
			}
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
			if (player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssuePlayer) {
				spawnRate = 2;
				maxSpawns = 400;
			}
		}
	}
}
