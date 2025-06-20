using BossRush.Contents.NPCs.TrialNPC.NPCHeldItem;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.NPCs.TrialNPC;
internal class TrialKnight : BaseTrialNPC {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void TrialNPCDefaults() {
		NPC.lifeMax = 300;
		NPC.damage = 50;
		NPC.defense = 20;
		NPC.friendly = false;
		NPC.width = NPC.height = 30;
		NPC.lavaImmune = true;
		NPC.trapImmune = true;
		NPC.knockBackResist = .3f;
		NPC.color = Color.AliceBlue;
		NPC.GravityMultiplier *= 1.4f;
		ItemTypeHold = ModContent.ProjectileType<Trial_IronBroadsword>();
	}
	public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
		return false;
	}
	Player player;
	public override void TrialAI() {
		if (player == null || player.dead) {
			NPC.TargetClosest();
			player = Main.player[NPC.target];
		}
		if (!NPC.Center.IsCloseToPosition(player.Center, 60f) && NPC.ai[2] == 0) {
			NPC.ai[1] += .1f;
			if (NPC.ai[1] > 3) {
				NPC.ai[1] = 3;
			}
			NPC.velocity.X = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero).X * NPC.ai[1];
			NPC.ai[0] = 0;
			return;
		}
		if (NPC.Center.IsCloseToPosition(player.Center, 100f)) {
			NPC.ai[2] = 1;
			NPC.velocity *= .9f;
			NPC.ai[1] = 0;
			if (++NPC.ai[0] >= 20) {
				Attack(60, 60);
				NPC.ai[0] = 0;
			}
		}
		else {
			NPC.ai[2] = 0;
		}
	}
}
