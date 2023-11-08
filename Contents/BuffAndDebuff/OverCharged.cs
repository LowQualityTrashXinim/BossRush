using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff {
	internal class OverCharged : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public int Delay = 0;

		public override void Update(Player player, ref int buffIndex) {
			int dust = Dust.NewDust(player.Center + new Vector2(Main.rand.Next(-player.width, player.width)), 0, 0, DustID.Electric);
			Main.dust[dust].velocity = -player.velocity * .1f + Main.rand.NextVector2Circular(3, 3);
			player.moveSpeed += .1f;
			player.GetDamage(DamageClass.Generic) += 0.1f;
			player.GetAttackSpeed(DamageClass.Generic) += .1f;
			if (++Delay >= 25) {
				if (player.Center.LookForHostileNPCNotImmune(out NPC npc, 550, player.whoAmI)) {
					for (int i = 0; i < 100; i++) {
						int electic = Dust.NewDust(BossRushUtils.NextPointOn2Vector2(player.Center, npc.Center), 0, 0, DustID.Electric);
						Main.dust[electic].noGravity = true;
						Main.dust[electic].velocity = Vector2.Zero;
					}
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo(30, 0, damageVariation: true));
					Delay = 0;
				}
			}
		}
	}
}
