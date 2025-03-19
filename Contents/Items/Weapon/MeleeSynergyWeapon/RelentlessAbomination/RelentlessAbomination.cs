using Terraria.ID;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using BossRush.Common.RoguelikeChange;
using BossRush.Texture;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.RelentlessAbomination {
	class RelentlessAbomination : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultMeleeShootCustomProjectile(58, 68, 45, 9f, 28, 28, ItemUseStyleID.Swing, ProjectileID.Bee, 10f, true);
			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul melee)) {
				melee.SwingType = BossRushUseStyle.Swipe;
			}
			Item.scale = 1.2f;
		}
		public int Get_ProjectileType() {
			return Main.rand.Next(new int[] {
				ModContent.ProjectileType<RA_AntlionClaw>(),
				ModContent.ProjectileType<RA_BatBat>(),
				ModContent.ProjectileType<RA_BeeKeeper>(),
				ModContent.ProjectileType<RA_BoneSword>(),
				ModContent.ProjectileType<RA_PurpleClubberfish>(),
				ModContent.ProjectileType<RA_ZombieArm>(),
			});
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			CanShootItem = false;
			type = Get_ProjectileType();
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
			int type = Get_ProjectileType();
			Vector2 pos = player.Center + Main.rand.NextVector2CircularEdge(250, 250);
			Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * Item.shootSpeed, type, player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.ZombieArm)
				.AddIngredient(ItemID.AntlionClaw)
				.AddIngredient(ItemID.BoneSword)
				.AddIngredient(ItemID.BatBat)
				.AddIngredient(ItemID.PurpleClubberfish)
				.AddIngredient(ItemID.BeeKeeper)
				.Register();
		}
	}
	class RA_ZombieArm : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.ZombieArm);
		public override void SetDefaults() {
			Projectile.width = 38;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 3;
			Projectile.scale = .75f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(5, 5), 0, 0, DustID.Blood);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			dust.scale = Main.rand.NextFloat(.5f, .9f);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
		public override void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) {
			if (npc.HasBuff<RA_Rotting>()) {
				modifiers.SourceDamage += .1f;
			}
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			if (!npc.HasBuff<RA_Rotting>()) {
				npc.AddBuff<RA_Rotting>(BossRushUtils.ToSecond(Main.rand.Next(3, 8)));
			}
		}
	}
	class RA_Rotting : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.lifeRegen -= 20;
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 5;
		}
	}
	class RA_AntlionClaw : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.AntlionClaw);
		NPC npc = null;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
			Projectile.scale = .75f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
		}
		Vector2 offset = Vector2.Zero;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (Projectile.timeLeft == 300) {
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			}
			if (Projectile.velocity != Vector2.Zero && npc == null) {
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				Projectile.velocity *= 1.1f;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(25);
				Projectile.damage = Projectile.originalDamage + (int)Math.Ceiling(Projectile.velocity.Length());
			}
			if (npc != null) {
				if (npc.life <= 0 || !npc.active) {
					Projectile.Kill();
					return;
				}
				else {
					Projectile.Center = npc.Center;
				}
			}
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			Projectile.velocity = Vector2.Zero;
			if (this.npc == null) {
				this.npc = npc;
				Projectile.ai[1] = (npc.Center - Projectile.Center).Length();
				Projectile.knockBack = 0;
				offset = npc.Center - Projectile.Center;
			}
			Projectile.damage = (int)(Projectile.damage * .99f);
		}
	}
	class RA_BoneSword : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BoneSword);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 50;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 1;
			Projectile.scale = .75f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(5, 5), 0, 0, DustID.Bone);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			dust.scale = Main.rand.NextFloat(.5f, .9f);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
		public override void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) {
			modifiers.ArmorPenetration += 10;
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			for (int i = 0; i < 5; i++) {
				int type = ProjectileID.Bone;
				int damage = Projectile.damage / 5;
				if (Main.rand.NextBool(5)) {
					type = ProjectileID.BoneGloveProj;
					damage += damage * 2;
				}
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2CircularEdge(5, 5), type, damage, Projectile.knockBack, Projectile.owner);
				proj.friendly = true;
				proj.hostile = false;
				proj.penetrate += 3;
				proj.maxPenetrate = proj.penetrate;
				proj.usesIDStaticNPCImmunity = true;
				proj.idStaticNPCHitCooldown = 30;
			}
		}
	}
	class RA_BatBat : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BatBat);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 52;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 4;
			Projectile.scale = .75f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.rotation = MathHelper.ToRadians(++Projectile.ai[0] * 30);
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			player.Heal(1);
		}
	}
	class RA_PurpleClubberfish : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PurpleClubberfish);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 50;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
			Projectile.scale = .75f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			base.SynergyAI(player, modplayer);
			if (Projectile.ai[1] == 0) {
				Projectile.ai[1] = 1;
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-15));
			}
			if (++Projectile.ai[0] >= 10) {
				Projectile.ai[1] *= -1;
				Projectile.ai[0] = 0;
			}
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 3));
			Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.rotation += Projectile.spriteDirection == 1 ? MathHelper.PiOver4 : MathHelper.PiOver2 + MathHelper.PiOver4;
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			base.OnHitNPCSynergy(player, modplayer, npc, hit, damageDone);
		}
	}
	class RA_BeeKeeper : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BeeKeeper);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 50;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 4;
			Projectile.scale = .75f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			base.SynergyAI(player, modplayer);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			int type = ProjectileID.Bee;
			int damage = Projectile.damage / 4;
			if (player.strongBees) {
				damage += damage;
				type = ProjectileID.GiantBee;
			}
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.Vector2RotateByRandom(180), type, damage, 2, Projectile.owner);
		}
	}
}
