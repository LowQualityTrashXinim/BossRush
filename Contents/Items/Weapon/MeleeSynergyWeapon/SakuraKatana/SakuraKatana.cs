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
		Item.UseSound = SoundID.Item1;
	}
	int delayBetweenHit = 0;
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		if (delayBetweenHit > 0) {
			return;
		}
		float Rotation = MathHelper.ToRadians(Main.rand.NextFloat(90));
		Vector2 pos = target.Center + Main.rand.NextVector2Circular(target.width, target.height);
		for (int i = 0; i < 5; i++) {
			Vector2 vel = Vector2.One.Vector2DistributeEvenly(5, 360, i).RotatedBy(Rotation);
			Projectile proj = Projectile.NewProjectileDirect(Item.GetSource_OnHit(target), pos, vel * .1f, ModContent.ProjectileType<SakuraLeaf_Projectile_2>(), Item.damage, Item.knockBack, player.whoAmI);
			proj.frame = 1;
			proj.ai[1] = i * -10f;
		}
		delayBetweenHit = 60;
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		delayBetweenHit = BossRushUtils.CountDown(delayBetweenHit);
		if (player.ItemAnimationJustStarted) {
			int flip = player.GetModPlayer<MeleeOverhaulPlayer>().ComboNumber == 0 ? 1 : -1;
			int amount = Main.rand.Next(3, 6);
			for (int i = 0; i < amount; i++) {
				Vector2 vel = Vector2.UnitX * Main.rand.NextFloat(5, 10) * player.direction;
				Vector2 pos = Main.rand.NextVector2FromRectangle(player.Hitbox);
				Projectile proj = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), pos.Add(0, Item.height * flip), vel, ModContent.ProjectileType<SakuraLeaf_Projectile_1>(), (int)(Item.damage * .35f), Item.knockBack, player.whoAmI);
				proj.penetrate = 1;
				proj.ai[0] = flip * -player.direction;
			}
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Katana)
			.AddIngredient(ItemID.AbigailsFlower)
			.Register();
	}
}
public class SakuraLeaf_Projectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 14;
		Projectile.timeLeft = 180;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 3;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 30;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		SakuraAI();
	}
	public virtual void SakuraAI() { }
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Main.instance.LoadGore(GoreID.TreeLeaf_VanityTreeSakura);
		Texture2D texture = TextureAssets.Gore[GoreID.TreeLeaf_VanityTreeSakura].Value;
		Vector2 origin = texture.Size();
		origin.X /= 32;
		origin.Y /= 8;
		Vector2 pos = Projectile.position - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, pos, texture.Frame(32, 8, 0, Projectile.frame), Color.White * (1 - Projectile.alpha / 255f), Projectile.rotation, origin * .5f, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
public class SakuraLeaf_Projectile_1 : SakuraLeaf_Projectile {
	public override void SakuraAI() {
		if (++Projectile.frameCounter >= 6) {
			Projectile.frameCounter = 0;
			Projectile.frame += 1;
			if (Projectile.frame >= 8) {
				Projectile.frame = 0;
			}
		}
		Projectile.ProjectileAlphaDecay(30);
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		Projectile.velocity += Projectile.rotation.ToRotationVector2() * -Projectile.ai[0] * .5f;
		Projectile.velocity.Y += .01f;
	}
}
public class SakuraLeaf_Projectile_2 : SakuraLeaf_Projectile {
	public override void SakuraAI() {
		Projectile.ProjectileAlphaDecay(30);
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		if (++Projectile.ai[1] <= 10) {
			return;
		}
		Projectile.velocity += Projectile.velocity * .025f;
		Projectile.velocity = Projectile.velocity.LimitedVelocity(2);
	}
}
