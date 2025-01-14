using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Common.General;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class MagicalScroll : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<MagicalScrollPlayer>().MagicalScroll = true;
		player.manaCost += .05f;
	}
}
public class MagicalScrollPlayer : ModPlayer {
	public bool MagicalScroll = false;
	public override void ResetEffects() {
		MagicalScroll = false;
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (MagicalScroll
			&& proj.type != ModContent.ProjectileType<MagicalBolt>()
			&& proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == Player.HeldItem.type
			&& Main.rand.NextBool(5)) {
			int damage = (int)Player.GetDamage(DamageClass.Magic).ApplyTo(42);
			Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center, Main.rand.NextVector2CircularEdge(3, 3), ModContent.ProjectileType<MagicalBolt>(), damage, 4f, Player.whoAmI);
		}
	}
}
class MagicalBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.timeLeft = 600;
		Projectile.extraUpdates = 3;
		Projectile.penetrate = 2;
	}
	public override Color? GetAlpha(Color lightColor) {
		return new Color(255, 0, 255);
	}
	public override bool? CanDamage() {
		return Projectile.penetrate > 1;
	}
	public override void AI() {
		if (Projectile.timeLeft <= 60) {
			Projectile.ProjectileAlphaDecay(60);
		}
		if (Projectile.penetrate <= 1) {
			if (Projectile.ai[1] != 1) {
				Projectile.timeLeft = 60;
				Projectile.ai[1] = 1;
			}
			Projectile.velocity = Projectile.velocity * .95f;
			return;
		}
		Projectile.rotation += MathHelper.ToRadians(3);
		if (Projectile.velocity.IsLimitReached(1f)) {
			Projectile.velocity *= .99f;
		}
		if (++Projectile.ai[0] <= 90) {
			return;
		}
		Projectile.Center.LookForHostileNPC(out NPC npc, 1200);
		if (npc == null) {
			return;
		}
		Projectile.timeLeft = 600;
		Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .1f;
		Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
	}
	public void DrawTrail1(Texture2D texture, Vector2 origin) {
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = new Color(25, 0, 25, 1) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color * Projectile.Opacity, Projectile.oldRot[k], origin, Projectile.scale - k / 100f, SpriteEffects.None, 0);
		}
	}
	public void DrawTrail2(Texture2D texture, Vector2 origin) {
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = new Color(25, 25, 25, 1) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color * Projectile.Opacity, Projectile.oldRot[k], origin, (Projectile.scale - k / 100f) * .5f, SpriteEffects.None, 0);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.SMALLWHITEBALL).Value;
		Vector2 origin = Projectile.Size * .5f;
		DrawTrail1(texture, origin);
		DrawTrail2(texture, origin);
		return false;
	}
}
