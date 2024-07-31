using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Projectiles;
internal class GlassProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Glass);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 300;
	}
	public override void AI() {
		Projectile.velocity *= .98f;
		if (Projectile.velocity.Y < 20) {
			Projectile.velocity.Y += .75f;
		}
		Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length()) * (Projectile.velocity.X > 0 ? 1 : -1);
	}
	public override void OnKill(int timeLeft) {
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 75f);
		Player player = Main.player[Projectile.owner];
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(Projectile.damage * .34f), 0));
		}
	}
}
