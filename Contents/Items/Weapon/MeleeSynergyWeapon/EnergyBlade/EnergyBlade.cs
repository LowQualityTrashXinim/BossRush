using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnergyBlade {
	internal class EnergyBlade : SynergyModItem {
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		public override void SetDefaults() {
			Item.BossRushDefaultMeleeCustomProjectile(64, 62, 21, 0, 30, 30, ItemUseStyleID.Swing, ModContent.ProjectileType<EnergyBladeProjectile>(), true);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.useTurn = false;
			Item.UseSound = SoundID.Item1;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (modplayer.EnergyBlade_Code1) {
				tooltips.Add(new TooltipLine(Mod, "EnergyBlade_Code1", $"[i:{ItemID.Code1}] Unlock 1st Energy Blade ability"));
			}
			if (modplayer.EnergyBlade_Code2) {
				tooltips.Add(new TooltipLine(Mod, "EnergyBlade_Code2", $"[i:{ItemID.Code2}] Unlock 2nd Energy Blade ability"));
			}
			base.ModifySynergyToolTips(ref tooltips, modplayer);
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.HasItem(ItemID.Code1)) {
				modplayer.EnergyBlade_Code1 = true;
				modplayer.SynergyBonus++;
			}
			if (player.HasItem(ItemID.Code2)) {
				modplayer.EnergyBlade_Code2 = true;
				modplayer.SynergyBonus++;
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (modplayer.EnergyBlade_Code1) {
				if (modplayer.EnergyBlade_Code1_Energy > 0) {
					for (int i = 0; i < modplayer.EnergyBlade_Code1_Energy; i++) {
						Vector2 lerp = velocity.Vector2DistributeEvenly(modplayer.EnergyBlade_Code1_Energy, 90, i) * 5f;
						int proj = Projectile.NewProjectile(source, position, lerp, ModContent.ProjectileType<EnergyBladeEnergyBallProjectile>(), damage, knockback, player.whoAmI, 1);
						Main.projectile[proj].timeLeft = 300;
					}
					modplayer.EnergyBlade_Code1_Energy = 0;
				}
				if (player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1) {
					int proj = Projectile.NewProjectile(source, position, velocity, type, (int)(damage * 1.55f), knockback, player.whoAmI, 1);
					Main.projectile[proj].height *= 2;
					Main.projectile[proj].width *= 2;
					Main.projectile[proj].scale += .5f;
					Main.projectile[proj].Size *= .5f;
				}
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
			CanShootItem = player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.EnchantedSword)
				.AddIngredient(ItemID.Terragrim)
				.Register();
		}
	}
	public class EnergyBladeProjectile : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<EnergyBlade>();
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 8;
		}
		public override void SetDefaults() {
			Projectile.width = 64;
			Projectile.height = 62;
			Projectile.penetrate = -1;
			Projectile.wet = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			npc.immune[Projectile.owner] = 0;
			if (modplayer.EnergyBlade_Code1) {
				if (Projectile.ai[0] == 1) {
					Vector2 Toplayer = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.Center, Toplayer * 6f, ModContent.ProjectileType<EnergyBladeEnergyBallProjectile>(), 0, 0, Projectile.owner);
				}
			}
		}
		Vector2 data;
		Player player;
		public override void OnSpawn(IEntitySource source) {
			player = Main.player[Projectile.owner];
			data = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
			if (Projectile.ai[0] == 1) {
				Projectile.timeLeft = 30;
			}
		}
		float outrotation = 0;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			frameCounter();
			if (Projectile.ai[0] == 1) {
				EnergySword_Code1AI();
				return;
			}
			if (Projectile.ai[0] == 2) {
				return;
			}
			BossRushUtils.ProjectileSwordSwingAI(Projectile, player, data);
			if (modplayer.EnergyBlade_Code1) {
				float rotation = Projectile.rotation - (Projectile.spriteDirection > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 + MathHelper.PiOver2);
				int energycode1 = Projectile.NewProjectile(Projectile.GetSource_FromAI(),
					Projectile.Center,
					rotation.ToRotationVector2() * 10f,
					ModContent.ProjectileType<EnergyBladeEnergyBallProjectile>(),
					(int)(Projectile.damage * .25f),
					1f,
					Projectile.owner, 2);
				Main.projectile[energycode1].timeLeft = 60;
				Main.projectile[energycode1].penetrate = 1;
			}
		}
		private void EnergySword_Code1AI() {
			if (Projectile.timeLeft > player.itemAnimationMax) {
				Projectile.timeLeft = player.itemAnimationMax;
			}
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			percentDone = Math.Clamp(percentDone, 0, 1);
			Projectile.spriteDirection = player.direction;
			float baseAngle = data.ToRotation();
			float angle = MathHelper.ToRadians(150) * player.direction;
			float start = baseAngle + angle;
			float end = baseAngle - angle;
			float rotation = MathHelper.Lerp(start, end, percentDone);
			outrotation = rotation;
			Projectile.rotation = rotation;
			Projectile.rotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
			Projectile.velocity.X = player.direction;
			Projectile.Center = player.Center + Vector2.UnitX.RotatedBy(rotation) * 180f;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			if (Projectile.ai[0] == 1) {
				BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, player, outrotation, (int)(Projectile.width * 1.5f), (int)(Projectile.height * 1.5f), 140f);
				return;
			}
			BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, player, Projectile.width, Projectile.height);
		}
		public void frameCounter() {
			if (++Projectile.frameCounter >= 3) {
				Projectile.frameCounter = 0;
				Projectile.frame += 1;
				if (Projectile.frame >= Main.projFrames[Projectile.type]) {
					Projectile.frame = 0;
				}
			}
		}
	}
	class EnergyBladeEnergyBallProjectile : SynergyModProjectile {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Projectile.hide = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.wet = false;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 6;
			Projectile.timeLeft = 150;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (Projectile.ai[0] == 2) {
				Projectile.velocity -= Projectile.velocity * .1f;
				if (Main.rand.NextBool(4)) {
					int type = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemSapphire, DustID.GemRuby });
					int dust = Dust.NewDust(Projectile.Center, 0, 0, type);
					Main.dust[dust].noGravity = true;
				}
				return;
			}
			for (int i = 0; i < 2; i++) {
				int type = Main.rand.Next(new int[] { DustID.GemDiamond, DustID.GemSapphire, DustID.GemRuby });
				int dust = Dust.NewDust(Projectile.Center, 0, 0, type);
				Main.dust[dust].noGravity = true;
			}
			if (Projectile.ai[0] == 1) {
				if (Projectile.ai[1] <= 0) {
					Projectile.ai[1] = 5;
					Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(60));
				}
				else {
					Projectile.ai[1]--;
				}
				return;
			}
			if (BossRushUtils.CompareSquareFloatValue(Projectile.Center, player.Center, 20 * 20)) {
				modplayer.EnergyBlade_Code1_Energy++;
				Projectile.Kill();
			}
			base.SynergyAI(player, modplayer);
		}
	}
}