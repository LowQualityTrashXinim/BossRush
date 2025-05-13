using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.MagicHandCannon {
	internal class MagicHandCannon : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.Flamelash, $"[i:{ItemID.Flamelash}] When magic shadow flame is inside the ring, shoot out a home in shadow magic flame and damage dealing outside of the ring increases by 45%");
		}
		public override void SetDefaults() {
			Item.BossRushDefaultMagic(54, 32, 30, 5f, 30, 30, ItemUseStyleID.Shoot, ModContent.ProjectileType<MagicHandCannonProjectile>(), 12, 13, false);
			Item.scale = .75f;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item92 with { Pitch = 1f };
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.Flamelash);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 50);
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			CanShootItem = true;
			for (int i = 0; i < 100; i++) {
				if (i < 30) {
					int dust = Dust.NewDust(position.PositionOFFSET(velocity, 10), 0, 0, DustID.Shadowflame);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position += Main.rand.NextVector2CircularEdge(5, 3.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
					Main.dust[dust].velocity = velocity * .5f;
					Main.dust[dust].fadeIn = 1f;
				}
				if (i < 60) {
					int dust1 = Dust.NewDust(position, 0, 0, DustID.Shadowflame);
					Main.dust[dust1].noGravity = true;
					Main.dust[dust1].position += Main.rand.NextVector2CircularEdge(12.5f, 4.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
					Main.dust[dust1].velocity = velocity * .35f;
					Main.dust[dust1].fadeIn = 1f;
					int dust2 = Dust.NewDust(position.PositionOFFSET(velocity, -5), 0, 0, DustID.Shadowflame);
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].position += Main.rand.NextVector2CircularEdge(20, 5.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
					Main.dust[dust2].velocity = velocity * .2f;
					Main.dust[dust2].fadeIn = 1f;
				}
				Vector2 rotate = Main.rand.NextVector2CircularEdge(10, 3.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2);
				int dust3 = Dust.NewDust(position.PositionOFFSET(velocity, -10), 0, 0, DustID.Shadowflame);
				Main.dust[dust3].noGravity = true;
				Main.dust[dust3].velocity = rotate;
				Main.dust[dust3].fadeIn = 1f;
			}
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Handgun)
				.AddIngredient(ItemID.WaterBolt)
				.AddIngredient(ItemID.ShadowFlameHexDoll)
				.Register();
		}
	}
	class MagicHandCannonPlayer : ModPlayer {
		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
			if (Player.HeldItem.type == ModContent.ItemType<MagicHandCannon>()) {
				BossRushUtils.BresenhamCircle(Player.Center, 350, Color.MediumPurple);
			}
		}
	}
	class MagicHandCannonProjectile : SynergyModProjectile {
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 11;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.wet = false;
			Projectile.timeLeft = 150;
			Projectile.light = 1f;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			int dust = Dust.NewDust(Projectile.position + Main.rand.NextVector2Circular(5, 5), 0, 0, DustID.Shadowflame);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = -Projectile.velocity * .1f;
			Main.dust[dust].scale = Main.rand.NextFloat(.5f, 1.25f);
			if (Projectile.direction == 1) {
				DrawOffsetX = -5;
			}
			else {
				DrawOffsetX = 0;
			}
			bool outsideBorder = !Projectile.Center.IsCloseToPosition(Main.player[Projectile.owner].Center, 350);
			SelectFrame();
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<MagicHandCannon>(), ItemID.Flamelash)) {
				bool ShootProjectile = false;
				if (!outsideBorder) {
					Projectile.ai[0]++;
					ShootProjectile = true;
				}
				if (ShootProjectile && Projectile.ai[0] >= 30) {
					Projectile.ai[0] = 0;
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.rotation).ToRotationVector2(), ModContent.ProjectileType<ShadowMagicMissle>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
			float rotateTo = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero).ToRotation();
			if (!outsideBorder) {
				float currentRotation = Projectile.velocity.ToRotation();
				Projectile.velocity = Projectile.velocity.RotatedBy(rotateTo - currentRotation);
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) {
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<MagicHandCannon>(), ItemID.Flamelash)) {
				if (!Projectile.Center.IsCloseToPosition(Main.player[Projectile.owner].Center, 350)) {
					modifiers.SourceDamage += .45f;
				}
			}
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			npc.AddBuff(BuffID.ShadowFlame, 180);
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			for (int i = 0; i < 20; i++) {
				int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2CircularEdge(5f, 5f), 0, 0, DustID.Shadowflame);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(5f, 5f);
				Main.dust[dust].scale = Main.rand.NextFloat(1f, 1.5f);
			}
		}
		public void SelectFrame() {
			if (++Projectile.frameCounter >= 4) {
				Projectile.frameCounter = 0;
				Projectile.frame += 1;
				if (Projectile.frame >= Main.projFrames[Type]) {
					Projectile.frame = 0;
				}
			}
		}
	}
	public class ShadowMagicMissle : ModProjectile {
		public override string Texture => BossRushTexture.SMALLWHITEBALL;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 50;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 11;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 10;
		}
		public override void AI() {
			if (++Projectile.ai[0] >= 5) {
				int dust = Dust.NewDust(Projectile.position + Main.rand.NextVector2Circular(5, 5), 0, 0, DustID.Shadowflame);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = Main.rand.NextFloat(.5f, 2f);
				Projectile.ai[0] = 0;
			}
			Projectile.alpha = (int)MathHelper.Lerp(0, 255, (600 - Projectile.timeLeft) / 600f);
			if (Projectile.Center.LookForHostileNPC(out NPC npc, 1000)) {
				float rotateTo = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero).ToRotation();
				float currentRotation = Projectile.velocity.ToRotation();
				Projectile.velocity = Projectile.velocity.RotatedBy(rotateTo - currentRotation);
				Projectile.timeLeft = 600;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(BuffID.ShadowFlame, 180);
		}
		public override Color? GetAlpha(Color lightColor) {
			return new Color(155, 0, 255);
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrail(lightColor, .02f);
			return base.PreDraw(ref lightColor);
		}
	}
}
