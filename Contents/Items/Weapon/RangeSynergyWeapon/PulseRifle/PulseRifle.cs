using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.PulseRifle;
internal class PulseRifle : SynergyModItem {
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.BossRushDefaultRange(94, 34, 34, 4f, 7, 7, ItemUseStyleID.Shoot, ProjectileID.PulseBolt, 16f, true, AmmoID.Bullet);
		Item.scale = .78f;
		Item.UseSound = SoundID.Item75 with { Pitch = 1 };
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-20, 0);
	}
	public int Counter = 0;
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.PulseBolt;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		Counter++;
		if (Counter >= 30) {
			for (int i = 0; i < 4; i++) {
				int proj = Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 50), velocity.Vector2DistributeEvenlyPlus(4, 40, i), type, damage, knockback, player.whoAmI);
				Main.projectile[proj].penetrate = 1;
			}
			Counter = 0;
		}
		if (Main.rand.NextBool(5)) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 50), velocity.Vector2RotateByRandom(30) * .1f, ModContent.ProjectileType<PulseHomingProjectile>(), (int)(damage * 1.25f), knockback, player.whoAmI);
		}
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.PulseBow)
			.AddIngredient(ItemID.Megashark)
			.Register();
	}
}
public class PulseHomingProjectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1800;
		Projectile.extraUpdates = 20;
		Projectile.penetrate = 1;
	}
	NPC npc = null;
	public override Color? GetAlpha(Color lightColor) {
		return new Color(255, 255, 255, 255);
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (++Projectile.ai[0] <= 90) {
			return;
		}
		if (npc == null) {
			if (Main.MouseWorld.LookForHostileNPC(out NPC target, 500f)) {
				npc = target;
			}
		}
		else {
			if (!npc.active || npc.life <= 0) {
				npc = null;
				return;
			}
			Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (npc.Center - Projectile.Center).Length() / 6400f;
			Projectile.velocity = Projectile.velocity.LimitedVelocity((npc.Center - Projectile.Center).Length() / 128f);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		for (int i = 0; i < Projectile.oldPos.Length; i++) {
			Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = new Color(100, 100, 255, 255) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale - i * .01f, SpriteEffects.None, 0);
		}
		for (int i = 0; i < Projectile.oldPos.Length; i++) {
			Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color2 = new Color(255, 255, 255, 255) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color2, Projectile.rotation, origin, (Projectile.scale - i * .01f) * .35f, SpriteEffects.None, 0);
		}
		return base.PreDraw(ref lightColor);
	}
}
