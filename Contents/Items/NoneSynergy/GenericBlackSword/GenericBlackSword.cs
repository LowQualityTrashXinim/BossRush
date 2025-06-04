using BossRush.Common.Graphics;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NoneSynergy.GenericBlackSword {
	internal class GenericBlackSword : ModItem {
		public override void SetStaticDefaults() {
			SwordSlashTrail.averageColorByID[Type] = Color.White;
		}
		public override void SetDefaults() {
			Item.BossRushDefaultMeleeShootCustomProjectile(40, 40, 27, 2f, 24, 24, ItemUseStyleID.Swing, ModContent.ProjectileType<GenericBlackSwordSlash>(), 5, true);
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(gold: 1);
			Item.scale = 1.5f;
			Item.UseSound = SoundID.Item1;
			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul overhaul)) {
				overhaul.SwingType = BossRushUseStyle.Swipe;
			}
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 50);
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			player.GetModPlayer<GenericBlackSwordPlayer>().VoidCount++;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			var hitboxCenter = new Vector2(hitbox.X, hitbox.Y);

			int dust = Dust.NewDust(hitboxCenter, hitbox.Width, hitbox.Height, DustID.t_Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(1.25f, 1.75f));
			Main.dust[dust].noGravity = true;
		}
		List<SpriteTracker> tracker = new();
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			if (tracker == null) {
				tracker = new();
			}
			if (tracker.Count <= 60) {
				SpriteTracker track = new SpriteTracker();
				track = new(Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(.5f, 1f), MathHelper.ToRadians(Main.rand.Next(-20, 20)), Main.rand.Next(60, 90));
				tracker.Add(track);
			}
			Texture2D texture = TextureAssets.Item[Type].Value;
			for (int i = tracker.Count - 1; i >= 0; i--) {
				SpriteTracker tr = tracker[i];
				if (tr.position == Vector2.Zero) {
					tr.position = position;
				}
				tr.position += tr.velocity;
				tr.rotation += tr.rotationSp;
				Color baseOnScale = drawColor;
				baseOnScale.A = (byte)(baseOnScale.A * tr.scale * .25f);
				spriteBatch.Draw(texture, tr.position, null, baseOnScale, tr.rotation, origin, tr.scale, SpriteEffects.None, 0);
				tr.scale -= .01f;
				if (--tr.TimeLeft <= 0 || tr.scale <= 0) {
					tracker.RemoveAt(i);
				}
				else {
					tracker[i] = tr;
				}
			}

			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
	public struct SpriteTracker {
		public Vector2 position = Vector2.Zero;
		public Vector2 velocity = Vector2.Zero;
		public float rotation = 0;
		public float scale = 1;
		public float rotationSp = 0;
		public int TimeLeft = 0;

		public SpriteTracker(Vector2 vel, float rotationSpeed, int time) {
			velocity = vel;
			rotationSp = rotationSpeed;
			TimeLeft = time;
		}
	}

	internal class GenericBlackSwordProjectileBlade : ModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<GenericBlackSword>();
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 40;
			Projectile.penetrate = -1;
			Projectile.light = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 6;
		}
		public void Behavior(Player player, float offSet, int Counter, float Distance = 150) {
			var Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
			var NewCenter = player.Center + Rotate.RotatedBy(Counter * 0.01f) * Distance;
			Projectile.Center = NewCenter;
		}
		public override void OnSpawn(IEntitySource source) {
			for (int i = 0; i < 90; i++) {
				var randomSpeed = Main.rand.NextVector2CircularEdge(5, 5);
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Granite, randomSpeed.X, randomSpeed.Y, 0, Color.Black, 1f);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void AI() {
			if (Projectile.timeLeft <= 100 && Projectile.ai[1] == 0) {
				Projectile.timeLeft += 314 * 2;
			}
			var player = Main.player[Projectile.owner];
			if (player.GetModPlayer<GenericBlackSwordPlayer>().YouGotHitLMAO && Projectile.ai[1] == 0) {
				Projectile.ai[1] = 1;
				Projectile.velocity = Vector2.Zero;
			}
			if (Projectile.ai[1] == 1) {
				Projectile.Center.LookForHostileNPC(out NPC closestNPC, 1500);
				if (++Projectile.ai[0] >= 150) {
					if (closestNPC != null) {
						if (Projectile.ai[0] == 150) {
							Projectile.damage *= 5;
							Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10f;
							Projectile.timeLeft = 400;
						}
					}
					else {
						Projectile.ai[0] = 150;
					}
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				}
				else {
					if (closestNPC != null) {
						Projectile.rotation = MathHelper.PiOver4 + (closestNPC.Center - Projectile.Center).ToRotation();
					}
					else {
						Projectile.rotation = MathHelper.PiOver4 + (Main.MouseWorld - Projectile.Center).ToRotation();
					}
				}
			}
			else {
				if (player.dead || !player.active) {
					Projectile.Kill();
				}

				if (Main.rand.NextBool(3)) {
					int dust = Dust.NewDust(Projectile.Center, 10, 10, DustID.t_Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(.8f, 1f));
					Main.dust[dust].noGravity = true;
				}

				Projectile.rotation = MathHelper.PiOver4 + MathHelper.ToRadians(72 * Projectile.ai[2]) - MathHelper.ToRadians(Projectile.timeLeft);
				Behavior(player, 72 * Projectile.ai[2], Projectile.timeLeft);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if (Projectile.ai[1] == 1) {
				target.immune[Projectile.owner] = 3;
			}
			else {
				target.immune[Projectile.owner] = 8;
			}
			for (int i = 0; i < 35; i++) {
				var randomSpeed = Main.rand.NextVector2CircularEdge(10, 10);
				int dust = Dust.NewDust(Projectile.position, 0, 0, DustID.t_Granite, randomSpeed.X, randomSpeed.Y, 0, Color.Black, 1.2f);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 40; i++) {
				var randomSpeed = Main.rand.NextVector2CircularEdge(3, 3);
				Dust.NewDust(Projectile.position, 0, 0, DustID.t_Granite, randomSpeed.X, randomSpeed.Y, 0, Color.Black, Main.rand.NextFloat(1f, 1.25f));
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			var texture = TextureAssets.Projectile[Type].Value;

			var origin = texture.Size() * .5f;
			for (int k = 1; k < Projectile.oldPos.Length + 1; k++) {
				var drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
				var color = new Color(0, 0, 0, 255 / k);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale - (k - 1) * 0.02f, SpriteEffects.None, 0);
			}

			return true;
		}
	}

	public class GenericBlackSwordSlash : ModProjectile {
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults() {
			Projectile.width = 68;
			Projectile.height = 112;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 250;
			Projectile.light = 0.5f;
			Projectile.extraUpdates = 6;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if (Projectile.timeLeft <= 75) {
				Projectile.velocity *= .96f;
				Projectile.alpha = Math.Clamp(Projectile.alpha - 3, 0, 255);
			}
			Projectile.rotation = Projectile.velocity.ToRotation();

			var BetterTop = new Vector2(Projectile.Center.X, Projectile.Center.Y - Projectile.height * 0.5f);
			Dust.NewDust(BetterTop, Projectile.width, Projectile.height, DustID.t_Granite, Projectile.velocity.X, 0, 0, Color.Black, Main.rand.NextFloat(0.55f, 1f));

		}
		public override void OnKill(int timeLeft) {
			var BetterTop = new Vector2(Projectile.Center.X, Projectile.Center.Y - Projectile.height * 0.5f);
			for (int i = 0; i < 20; i++) {
				Dust.NewDust(BetterTop, Projectile.width, Projectile.height, DustID.t_Granite, Projectile.velocity.X, 0, 0, Color.Black, Main.rand.NextFloat(0.5f, 1f));
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if (Projectile.damage > 10) {
				Projectile.damage = (int)(Projectile.damage * .95f);
			}
			else {
				Projectile.damage = 10;
			}
			Projectile.velocity *= .98f;
			Main.player[Projectile.owner].GetModPlayer<GenericBlackSwordPlayer>().VoidCount++;
			target.immune[Projectile.owner] = 7;
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Type);
			var texture = TextureAssets.Projectile[Projectile.type].Value;
			float percentageAlpha = Math.Clamp(Projectile.alpha / 255f, 0, 1f);
			var origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 1; k < Projectile.oldPos.Length + 1; k++) {
				var drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
				var color = new Color(0, 0, 0, 255 / k);
				Main.EntitySpriteDraw(texture, drawPos, null, color * percentageAlpha, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			}

			return false;
		}
	}
	internal class GenericBlackSwordPlayer : ModPlayer {
		public int VoidCount = 0;
		public bool YouGotHitLMAO = false;

		public override void PostUpdate() {
			if (VoidCount >= 10) {
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<GenericBlackSwordProjectileBlade>()] < 1) {
					int PostUpdateDamage = Player.HeldItem.damage;
					YouGotHitLMAO = false;
					for (int i = 0; i < 5; i++) {
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<GenericBlackSwordProjectileBlade>(), PostUpdateDamage, 0, Player.whoAmI, ai2: i);
					}
				}
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		}

		public override void OnHurt(Player.HurtInfo info) {
			VoidCount = 0;
			YouGotHitLMAO = true;
		}
	}
}
