using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.OvergrownMinishark {
	internal class OvergrownMinishark : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.CrimsonRod);
		}
		public override void SetDefaults() {
			Item.BossRushDefaultRange(54, 24, 14, 2f, 11, 11, ItemUseStyleID.Shoot, ProjectileID.Bullet, 15, true, AmmoID.Bullet);

			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(gold: 50);
			Item.UseSound = SoundID.Item11;
			if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
				weapon.SpreadAmount = 7;
				weapon.OffSetPost = 40;
			}
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-4, 0);
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.CrimsonRod))
				tooltips.Add(new TooltipLine(Mod, "OvergrownMinishark_CrimsonRod", $"[i:{ItemID.CrimsonRod}] When shooting, you summon blood rain at cursor"));
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.CrimsonRod)) {
				Vector2 pos = Main.MouseWorld + new Vector2(Main.rand.NextFloat(-30, 30), -1000);
				int proj = Projectile.NewProjectile(source, pos, Vector2.UnitY * Main.rand.NextFloat(9, 11), ProjectileID.BloodRain, damage, knockback, player.whoAmI);
				Main.projectile[proj].penetrate = 1;
			}
			CanShootItem = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Minishark)
				.AddIngredient(ItemID.Vilethorn)
				.Register();
		}
	}
	class OvergrownMinisharkProjectileModify : GlobalProjectile {
		public override void OnSpawn(Projectile projectile, IEntitySource source) {
			if (source is EntitySource_ItemUse_WithAmmo entity && entity.Item.ModItem is OvergrownMinishark) {
				IsTruelyFromSource = true;
			}
		}
		public override bool InstancePerEntity => true;
		bool IsTruelyFromSource = false;
		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
			if (!IsTruelyFromSource) {
				return;
			}
			target.AddBuff(BuffID.Poisoned, 420);
			if (Main.rand.NextBool(10)) {
				float randomRotation = Main.rand.Next(90);
				Vector2 velocity;
				for (int i = 0; i < 6; i++) {
					velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(i * 60 + randomRotation)) * .5f;
					Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, velocity, ProjectileID.VilethornTip, (int)(hit.Damage * .35f), hit.Knockback, projectile.owner);
					Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, velocity, ProjectileID.VilethornBase, (int)(hit.Damage * .35f), hit.Knockback, projectile.owner);
				}
			}
		}
	}
}
