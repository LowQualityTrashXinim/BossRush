using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.SakuraKatana;
internal class SakuraKatana : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushSetDefault(48, 92, 48, 6f, 20, 20, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
			meleeItem.SwingType = BossRushUseStyle.Poke;
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		base.OnHitNPCSynergy(player, modplayer, target, hit, damageDone);
	}
	public override void MeleeEffects(Player player, Rectangle hitbox) {
	}
}
public class SakuraLeaf_Projectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.timeLeft = 900;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 1;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		SelectFrame();
	}
	public void SelectFrame() {
		if (++Projectile.frameCounter >= 6) {
			Projectile.frameCounter = 0;
			Projectile.frame += 1;
			if (Projectile.frame >= 8) {
				Projectile.frame = 0;
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Main.instance.LoadGore(GoreID.TreeLeaf_VanityTreeSakura);
		Texture2D texture = TextureAssets.Gore[GoreID.TreeLeaf_VanityTreeSakura].Value;
		Vector2 origin = texture.Size();
		origin.X /= 32;
		origin.Y /= 8;
		Vector2 pos = Projectile.position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, pos, texture.Frame(32, 8, 0, Projectile.frame), Color.White, Projectile.rotation, origin * .5f, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
