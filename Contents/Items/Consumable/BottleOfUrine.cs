using Terraria;
using Terraria.ID;
using Terraria.Audio;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Consumable;

class BottleOfUrine : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(18, 20, 10, 1f, 15, 15, ItemUseStyleID.Swing, ModContent.ProjectileType<BottleOfUrineProjectile>(), 10, false);
		Item.consumable = true;
		Item.maxStack = 9999;
		Item.noUseGraphic = true;
		Item.UseSound = SoundID.Item1;
	}
}
public class Urine_Debuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Ichor);
		dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.1f, .2f);
	}
}
public class BottleOfUrineProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<BottleOfUrine>();
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 3600;
		Projectile.tileCollide = true;
	}
	public override void AI() {
		Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 10 * -Projectile.direction);
		if (++Projectile.ai[0] >= 20)
			Projectile.velocity.Y += .75f;

		Projectile.velocity.X *= .98f;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff<Urine_Debuff>(BossRushUtils.ToSecond(60));
	}
	public override void OnKill(int timeLeft) {
		SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
		for (int i = 0; i < 20; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Glass);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.5f, 1.25f);
		}
		for (int i = 0; i < 10; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Ichor);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.5f, 1.25f);
		}
	}
}
