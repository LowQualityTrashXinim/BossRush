using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;

namespace BossRush.Contents.Projectiles;
public class SunflowerProjectile : ModProjectile {
	public override void SetDefaults() {
		Projectile.width = 28;
		Projectile.height = 66;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = BossRushUtils.ToSecond(12);
		Projectile.light = 1f;
	}
	public override bool? CanDamage() {
		return false;
	}
	public override void AI() {
		Projectile.velocity = Microsoft.Xna.Framework.Vector2.Zero;
		Player player = Main.player[Projectile.owner];
		if (Projectile.Center.IsCloseToPosition(player.Center, 400)) {
			player.AddBuff(BuffID.Sunflower, 60);
		}
		Dust grass = Dust.NewDustDirect(Projectile.Center.Subtract(Main.rand.NextFloat(-7, 3), 30), 0, 0, DustID.Grass);
		grass.velocity = Microsoft.Xna.Framework.Vector2.UnitY * Main.rand.NextFloat();
		grass.scale = Main.rand.NextFloat(.75f, 1f);
		grass.noGravity = true;
		if (++Projectile.ai[0] >= 150) {
			for (int i = 0; i < 16; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center.Add(0, 33), 0, 0, DustID.Sunflower);
				dust.velocity = Microsoft.Xna.Framework.Vector2.UnitX.Vector2DistributeEvenlyPlus(16, 360, i) * 4;
				dust.scale = 1.5f;
				dust.noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 1 });
			Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center.Add(0, 33), ModContent.ItemType<SunItem>());
			Projectile.ai[0] = 0;
		}
	}
}
public class SunItem : ModItem {
	public override void SetStaticDefaults() {
		ItemID.Sets.ItemNoGravity[Type] = true;
		ItemID.Sets.ItemsThatShouldNotBeInInventory[Type] = true;
	}
	public override void SetDefaults() {
		Item.width = 48;
		Item.height = 48;
	}
	public override bool OnPickup(Player player) {
		player.Heal(10);
		player.ManaHeal(10);
		return false;
	}
}
