using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul;

namespace BossRush.Contents.Items.Accessories.SynergyAccessories.MashedOfCopper {
	public class MashedOfCopper : SynergyModItem {
		public override void SetDefaults() {
			Item.DefaultToAccessory(27, 27);
			Item.value = Item.sellPrice(silver: 100);
			Item.rare = ItemRarityID.Blue;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statDefense += 6;
			player.GetModPlayer<RoguelikeArmorPlayer>().CopperArmor = true;
			player.GetModPlayer<MashedOfCopperPlayer>().MashedOfCopper = true;
		}

		public override void AddRecipes() {
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperGreaves, 1);
			recipe.AddIngredient(ItemID.CopperChainmail, 1);
			recipe.AddIngredient(ItemID.CopperHelmet, 1);
			recipe.AddIngredient(ItemID.CopperBow, 1);
			recipe.AddIngredient(ItemID.CopperShortsword, 1);
			recipe.AddIngredient(ItemID.CopperBroadsword, 1);
			recipe.Register();
		}
	}
	public class MashedOfCopperPlayer : ModPlayer {
		public bool MashedOfCopper;
		public override void ResetEffects() {
			MashedOfCopper = false;
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			OnHitByAny();
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			OnHitByAny();
		}
		private void OnHitByAny() {
			if (!MashedOfCopper) return;
			for (int i = 0; i < 5; i++) {
				var speed = Main.rand.NextVector2Unit(MathHelper.Pi + MathHelper.PiOver4, MathHelper.PiOver2) * 10f;
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, speed, ModContent.ProjectileType<MoCCopperOre>(), 30, 2, Player.whoAmI);
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (MashedOfCopper && Main.rand.NextBool(5) && proj.type != ModContent.ProjectileType<MoCShortSwordP>()) {
				var directionFromPlayerToNPC = (target.Center - Player.Center).SafeNormalize(Vector2.UnitX) * 15f;
				Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center, directionFromPlayerToNPC, ModContent.ProjectileType<MoCShortSwordP>(), 7, hit.Knockback, Player.whoAmI);
			}
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (MashedOfCopper && Main.rand.NextBool(5)) {
				var directionFromPlayerToNPC = (target.Center - Player.Center).SafeNormalize(Vector2.UnitX) * 15f;
				Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center, directionFromPlayerToNPC, ModContent.ProjectileType<MoCShortSwordP>(), 7, hit.Knockback, Player.whoAmI);
			}
		}
	}
	public class MoCCopperOre : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 2;
			Projectile.tileCollide = true;
		}
		public override void AI() {
			Projectile.rotation += 0.2f * Projectile.direction;
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 15; i++) {
				int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(16, 16), 0, 0, DustID.Copper);
				Main.dust[dust].noGravity = true;
			}
		}
	}
	public class MoCShortSwordP : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 2;
			Projectile.tileCollide = true;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
	}
}
