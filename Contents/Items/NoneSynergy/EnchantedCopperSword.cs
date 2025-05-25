using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NoneSynergy;

public class EnchantedCopperSword : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(50, 50, 10, 4, 20, 20, ItemUseStyleID.Swing, ModContent.ProjectileType<Legacy_EnchantedCopperSwordP>(), 10f, true);
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
			global.SwingType = BossRushUseStyle.Swipe;
		}
	}
}
class Legacy_EnchantedCopperSwordP : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperShortsword);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.alpha = 0;
		Projectile.light = 0.45f;
	}

	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		Projectile.alpha += 5;
		if (Projectile.alpha >= 235) {
			Projectile.Kill();
		}
	}
}
