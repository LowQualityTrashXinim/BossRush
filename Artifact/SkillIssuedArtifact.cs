using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Artifact
{
    internal class SkillIssuedArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Infinite Growth");
			Tooltip.SetDefault("\"with this, nothing is impossible, even with skill issue person\"" +
			"\nFor each monster kill, they will get 0.05 % increase toward Damage and Max HP" +
            "\nTo further increase progress, by default this accessory will make monster spawn increase by CRAZY amount" +
            "\nCapped at 10 million kill so your game don't break, you can thx me for that");
        }
        public override void SetDefaults()
        {
			Item.width = 80;
			Item.height = 80;
			Item.accessory = true;
			Item.rare = ItemRarityID.Expert;
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
			Player.GetDamage(DamageClass.Generic) += SkillIssue * 0.05f;
			Player.statLifeMax2 += (int)(SkillIssue * 0.05f);
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
			SkillIssue = (int)tag["Drug"];
		}
	}

	public class EnemyForSkillIssuePlayer : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
			Player player = Main.LocalPlayer;
            if(player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssuePlayer && player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssue <= 10000000)
            {
				player.GetModPlayer<SkillIssuedArtifactPlayer>().SkillIssue++;
            }
        }
    }
}
