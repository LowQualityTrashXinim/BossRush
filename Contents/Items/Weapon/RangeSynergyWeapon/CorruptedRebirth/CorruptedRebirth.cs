using BossRush.Common.Systems.ObjectSystem;
using BossRush.Contents.Projectiles;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.CorruptedRebirth;

public class CorruptedRebirth : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 30, 1f, 5, 25, ItemUseStyleID.Shoot, ProjectileID.WoodenArrowFriendly, 12, false, AmmoID.Arrow);
		Item.UseSound = SoundID.Item5;
		Item.reuseDelay = 30;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-6, 0);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		SoundEngine.PlaySound(SoundID.Item5, player.Center);
		if (player.ItemAnimationJustStarted) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AcidArrow>(), damage, knockback, player.whoAmI);
		}
		if (Main.rand.NextBool(3)) {
			Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(40, 40), Vector2.Zero, ModContent.ProjectileType<ToxicBubble>(), damage, knockback, player.whoAmI);
		}
		position += Main.rand.NextVector2CircularEdge(30, 30);
		velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * velocity.Length();
		if (type == ProjectileID.WoodenArrowFriendly) {
			type = ModContent.ProjectileType<AcidArrow>();
		}
		Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
		CanShootItem = false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.DD2PhoenixBow)
			.AddIngredient(ItemID.Toxikarp)
			.Register();
	}
}
public class ToxicBubble : SynergyModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.ToxicBubble);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 50;
		Projectile.friendly = true;
		Projectile.timeLeft = 180;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
		Projectile.alpha = 255;
		Projectile.scale = .66f;
	}
	NPC targetTo = null;
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (Projectile.alpha > 100) {
			Projectile.alpha -= 5;
		}
		//Color greeen = new(0, 255, 0, 0);
		//int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * .46f, 0, 0, DustID.WhiteTorch, 0, 0, 0, greeen, Main.rand.NextFloat(1.5f, 1.72f));
		//Main.dust[dust].velocity = Vector2.Zero;
		//Main.dust[dust].noGravity = true;
		Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 5);
		if (targetTo == null) {
			Projectile.ai[0] = 0;
			Projectile.ai[1] = 0;
			Projectile.Center.LookForHostileNPC(out NPC npc, 1000);
			targetTo = npc;
			return;
		}
		if (!targetTo.active) {
			targetTo = null;
			return;
		}
		if (targetTo.DistanceSQ(Projectile.Center) >= 1500 * 1500) {
			targetTo = null;
			return;
		}
		if (Projectile.timeLeft < 10) {
			Projectile.timeLeft += 360;
		}
		if (++Projectile.ai[0] >= 30) {
			if (Projectile.ai[1] < 10) {
				Projectile.ai[1] += .2f;
			}
			Projectile.velocity = (targetTo.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
		}
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		Color greeen = new(0, 255, 0, 0);
		for (int i = 0; i < 20; i++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.WhiteTorch, 0, 0, 0, greeen, Main.rand.NextFloat(1.5f, 1.72f));
			Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(5, 5);
			Main.dust[dust].noGravity = true;
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		npc.AddBuff<Acid>(BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
}
public class AcidArrow : SynergyModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.WoodenArrowFriendly);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 6;
		Projectile.penetrate = 3;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
		Projectile.timeLeft = 120;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		if (++Projectile.ai[0] >= 20) {
			if (Projectile.velocity.Y <= 20) {
				Projectile.velocity.Y += .5f;
			}
			Projectile.velocity.X *= .99f;
		}

		var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.CursedTorch);
		dust.noGravity = true;
		dust.velocity = Vector2.Zero;
	}
	public override void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) {
		if (npc.defense > 0) {
			npc.defense--;
		}
		npc.AddBuff<Acid>(BossRushUtils.ToSecond(Main.rand.Next(1, 4)));
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		Color greeen = new(0, 255, 0, 0);
		for (int i = 0; i < 10; i++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.WhiteTorch, 0, 0, 0, greeen, Main.rand.NextFloat(1.5f, 1.72f));
			Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(5, 5);
			Main.dust[dust].noGravity = true;
		}
	}
	public override Color? GetAlpha(Color lightColor) {
		return new Color(0, 255, 0, 0);
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out var texture, out var origin);
		var drawpos = Projectile.position - Main.screenPosition + origin * .25f;
		Main.EntitySpriteDraw(texture, drawpos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}

public class CorruptedRebirthModPlayer : ModPlayer {
	int counter = 0;
	int activeFlamethrower = 0;
	public override void UpdateEquips() {
		var item = Player.HeldItem;
		if (item.type == ModContent.ItemType<CorruptedRebirth>()) {
			if (counter >= 10) {
				Vector2 spawnPos = Player.Center + Main.rand.NextVector2CircularEdge(100, 100);
				Color greeen = new(0, 255, 0, 0);
				for (int i = 0; i < 400; i++) {
					int dust = Dust.NewDust(spawnPos, 0, 0, DustID.WhiteTorch, 0, 0, 0, greeen, Main.rand.NextFloat(1.5f, 1.72f));
					Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(17, 17);
					Main.dust[dust].noGravity = true;
				}
				for (int i = 0; i < 300; i++) {
					int dust = Dust.NewDust(spawnPos, 0, 0, DustID.GemEmerald, 0, 0, 0, greeen, Main.rand.NextFloat(1.5f, 1.72f));
					Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(15, 15);
					Main.dust[dust].noGravity = true;
				}
				for (int i = 0; i < 200; i++) {
					int dust = Dust.NewDust(spawnPos, 0, 0, DustID.GemEmerald, 0, 0, 0, greeen, Main.rand.NextFloat(1.5f, 1.72f));
					Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(10, 10);
					Main.dust[dust].noGravity = true;
				}
				counter = 0;
				activeFlamethrower = BossRushUtils.ToSecond(5);
				ModObject.NewModObject(spawnPos, Vector2.Zero, ModObject.GetModObjectType<CorruptedRebirthBowObject>());
			}
		}
		if (activeFlamethrower > 0) {
			activeFlamethrower--;
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Player.ItemAnimationJustStarted && item.type == ModContent.ItemType<CorruptedRebirth>() && activeFlamethrower <= 0) {
			counter++;
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
public class CorruptedRebirthBowObject : ModObject {
	public override void SetDefaults() {
		timeLeft = BossRushUtils.ToSecond(5);
	}
	public override void AI() {
		var player = Main.LocalPlayer;
		var item = player.HeldItem;
		var vel = (Main.MouseWorld - Center).SafeNormalize(Vector2.Zero);
		direction = BossRushUtils.DirectionFromEntityAToEntityB(position.X, Main.MouseWorld.X);
		rotation = vel.ToRotation();
		float custom = MathHelper.ToRadians(30);
		Color greeen = new(0, 255, 0, 0);
		for (int i = 0; i < 15; i++) {
			float velocityRotate = rotation + MathHelper.ToRadians(Main.rand.NextFloat(-10, 10));
			var dust = Dust.NewDustDirect(position + Main.rand.NextVector2Circular(30, 30), 0, 0, DustID.WhiteTorch);
			dust.noGravity = true;
			if (i % 2 == 0) {
				dust.velocity = -vel.RotatedBy(custom).SafeNormalize(Vector2.Zero) * 15 + Main.rand.NextVector2Circular(14, 1).RotatedBy(custom + velocityRotate) * Main.rand.NextFloat(1, 1.3f);
				dust.scale = Main.rand.NextFloat(.1f, .2f);
				dust.fadeIn = 1f;
				dust.color = greeen;
			}
			else {
				dust.velocity = -vel.RotatedBy(-custom).SafeNormalize(Vector2.Zero) * 15 + Main.rand.NextVector2Circular(14, 1).RotatedBy(-custom + velocityRotate) * Main.rand.NextFloat(1, 1.3f);
				dust.scale = Main.rand.NextFloat(.1f, .2f);
				dust.fadeIn = 1f;
				dust.color = greeen;
			}
		}
		if (timeLeft % 4 == 0) {
			var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), Center, vel * 10, ModContent.ProjectileType<FlameProjectile>(), player.GetWeaponDamage(item), item.knockBack, player.whoAmI, 0, 0);
			if (proj.ModProjectile is FlameProjectile flame) {
				flame.FlameColor = new(0, 255, 0, 0);
				flame.DebuffType = BuffID.CursedInferno;
			}
		}
	}
	public override void Draw(SpriteBatch spritebatch) {
		Main.instance.LoadItem(ModContent.ItemType<CorruptedRebirth>());
		var texture = TextureAssets.Item[ModContent.ItemType<CorruptedRebirth>()].Value;
		var origin = texture.Size() * .5f;
		var Drawpos = position - Main.screenPosition;
		var effect = direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		spritebatch.Draw(texture, Drawpos, null, Color.White, rotation, origin, 1f, effect, 0);
	}
}
public class Acid : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= npc.defense + 5;
	}
}
