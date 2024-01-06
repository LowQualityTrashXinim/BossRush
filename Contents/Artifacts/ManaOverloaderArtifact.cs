using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;
using Terraria.ID;
using BossRush.Contents.Items.Accessories.Trinket;

namespace BossRush.Contents.Artifacts {
	internal class ManaOverloaderArtifact : Artifact {
		public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
		public override Color DisplayNameColor => Color.LimeGreen;
	}

	public class ManaOverloaderPlayer : ModPlayer {
		bool ManaOverLoader = false;
		public int StackPoint = 0;
		public override void ResetEffects() {
			ManaOverLoader = Player.HasArtifact<ManaOverloaderArtifact>();
		}
		public override void PostUpdate() {
			if (ManaOverLoader) {
				if (!Player.HasBuff(ModContent.BuffType<ManaReleaseBuff>())) {
					StackPoint = 0;
				}
				if (Player.statMana < Player.statManaMax2) {
					Player.statMana++;
				}
				for (int i = 0; i < StackPoint; i++) {
					Vector2 pos = Player.Center +
						Vector2.One.Vector2DistributeEvenly(StackPoint, 360, i)
						.RotatedBy(MathHelper.ToRadians(Player.GetModPlayer<TrinketPlayer>().counterToFullPi)) * 30;
					int dust = Dust.NewDust(pos, 0, 0, DustID.ManaRegeneration);
					Main.dust[dust].velocity = Vector2.Zero;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].fadeIn = 0;
				}
			}
		}
		public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
			if (ManaOverLoader) {
				mult += StackPoint * .1f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (!ManaOverLoader) {
				return;
			}
			int manacost = Player.GetManaCost(item);
			if (item.mana != 0) {
				float multiplier = (manacost - item.mana) / (float)item.mana;
				damage *= multiplier + Player.manaCost;
			}
			else {
				damage *= Player.manaCost;
			}
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			SpawnManaEffectiveOrb(target.Center + Main.rand.NextVector2Circular(100, 100));
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			SpawnManaEffectiveOrb(target.Center + Main.rand.NextVector2Circular(100, 100));
		}
		private void SpawnManaEffectiveOrb(Vector2 pos) {
			if (!ManaOverLoader) {
				return;
			}
			if (Main.rand.NextBool(10)) {
				Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), pos, Vector2.Zero, ModContent.ProjectileType<ManaReleaseOrb>(), 0, 0, Player.whoAmI);
			}
		}
		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
			ModifyHit(ref modifiers);
		}
		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
			ModifyHit(ref modifiers);
		}
		private void ModifyHit(ref Player.HurtModifiers modifiers) {
			if (!ManaOverLoader) {
				return;
			}
			if (Player.statMana >= Player.statManaMax2) {
				modifiers.SourceDamage += Player.statMana * 0.01f;
			}
		}
	}
	class ManaReleaseOrb : ModProjectile {
		public override string Texture => BossRushTexture.SMALLWHITEBALL;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.timeLeft = 301;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
		}
		public override bool? CanDamage() => false;
		public override Color? GetAlpha(Color lightColor) {
			return new Color(0, 0, 255);
		}
		Player player;
		public override void AI() {
			if (Projectile.timeLeft > 300) {
				Projectile.timeLeft = 300;
				player = Main.player[Projectile.owner];
				for (int i = 0; i < 50; i++) {
					int startdust = Dust.NewDust(Projectile.Center, 0, 0, DustID.ManaRegeneration);
					Main.dust[startdust].velocity = Main.rand.NextVector2CircularEdge(6, 6);
					Main.dust[startdust].noGravity = true;
				}
			}
			int dust = Dust.NewDust(Projectile.Center - new Vector2(5, 5) + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.ManaRegeneration);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Vector2.Zero;
			if (Projectile.Center.IsCloseToPosition(player.Center, 150)) {
				Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 3;
			}
			else {
				Projectile.velocity *= .98f;
			}
			if (player is not null & Projectile.Center.IsCloseToPosition(player.Center, 25)) {
				player.AddBuff(ModContent.BuffType<ManaReleaseBuff>(), BossRushUtils.ToSecond(7));
				Projectile.Kill();
			}
		}
	}
	class ManaReleaseBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
		}
		public override bool ReApply(Player player, int time, int buffIndex) {
			ManaOverloaderPlayer modplayer = player.GetModPlayer<ManaOverloaderPlayer>();
			modplayer.StackPoint++;
			return base.ReApply(player, time, buffIndex);
		}
	}
}
