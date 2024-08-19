using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.Weapon.MagicSynergyWeapon.SinisterBook;
using Terraria.ID;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class ForbiddenTome : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<ForbiddenTomePlayer>().ForbiddenTome = true;
	}
}
class ForbiddenTomePlayer : ModPlayer {
	public bool ForbiddenTome = false;
	public override void ResetEffects() {
		ForbiddenTome = false;
	}
	public override void PostUpdate() {
		if (Player.ownedProjectileCounts[ModContent.ProjectileType<ForbiddenTomeProjectile>()] > 0 || !ForbiddenTome) {
			return;
		}
		Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ForbiddenTomeProjectile>(), 0, 0, Player.whoAmI, 0, 1);
	}
}
class ForbiddenTomeProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<SinisterBook>();
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 999;
		Projectile.tileCollide = false;
	}
	public override bool? CanDamage() {
		return false;
	}
	public float Progression { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public float Flip { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }

	const int MaxProgress = 50;
	public override void AI() {
		Projectile.timeLeft = 999;
		Player player = Main.player[Projectile.owner];
		SpawnProjectile(player);
		float LeftMostPos = 250;
		float BottomMostPos = 100;
		if (Flip == 0) {
			Flip = 1;
		}
		if (Progression >= MaxProgress) {
			Flip = -1;
		}
		else if (Progression <= 0) {
			Flip = 1;
		}

		if (player.dead || !player.active || !player.GetModPlayer<ForbiddenTomePlayer>().ForbiddenTome) {
			Projectile.Kill();
		}
		int halfmaxProgress = (int)(MaxProgress * .5f);
		int quadmaxProgress = (int)(MaxProgress * .25f);
		float progress;
		if (Progression > halfmaxProgress) {
			progress = (MaxProgress - Progression) / (float)halfmaxProgress;
		}
		else {
			progress = Progression / (float)halfmaxProgress;
		}
		float X = MathHelper.SmoothStep(0, LeftMostPos, progress);
		ProgressYHandle(BottomMostPos, halfmaxProgress, quadmaxProgress, out float Y);
		Projectile.Center = player.Center.Add(X * Flip, Y * Flip);
		Progression += Flip;
	}
	private void ProgressYHandle(float MaxLengthY, float progressMaxHalf, float progressMaxQuad, out float Y) {
		if (Progression > progressMaxHalf + progressMaxQuad) {
			float progressY = 1 - (Progression - (progressMaxHalf + progressMaxQuad)) / progressMaxQuad;
			Y = MathHelper.SmoothStep(0, MaxLengthY, progressY);
			return;
		}
		if (Progression > progressMaxQuad) {
			float progressY = 1 - (Progression - progressMaxQuad) / progressMaxHalf;
			Y = MathHelper.SmoothStep(MaxLengthY, -MaxLengthY, progressY);
			return;
		}
		else {
			float progressY = 1 - Progression / progressMaxQuad;
			Y = MathHelper.SmoothStep(-MaxLengthY, 0, progressY);
			return;
		}
	}
	private void SpawnProjectile(Player player) {
		int damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(42);
		if (++Projectile.ai[2] <= 15) {
			return;
		}
		Vector2 vec = Main.rand.NextVector2CircularEdge(3, 3);
		if (Projectile.Center.LookForAnyHostileNPC(75)) {
			for (int i = 0; i < 5; i++) {
				Vector2 vec2 = vec.Vector2DistributeEvenly(5, 360, i);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec2, ProjectileID.ShadowFlame, damage, 3f, player.whoAmI);
			}
			Projectile.ai[2] = 0;
		}
	}
}
