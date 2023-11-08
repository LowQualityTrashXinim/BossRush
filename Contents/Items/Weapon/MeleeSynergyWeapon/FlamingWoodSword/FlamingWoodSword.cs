using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.FlamingWoodSword {
	internal class FlamingWoodSword : SynergyModItem {
		public override void SetDefaults() {
			BossRushUtils.BossRushSetDefault(Item, 32, 36, 22, 5f, 4, 40, 1, false);
			Item.DamageType = DamageClass.Melee;

			Item.crit = 5;
			Item.useTurn = false;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(gold: 50);
			Item.shoot = ProjectileID.WandOfSparkingSpark;
			Item.shootSpeed = 6;
			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
				meleeItem.SwingType = BossRushUseStyle.GenericSwingDownImprove;
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(BuffID.OnFire, 90);
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			Projectile.NewProjectile(source, position, player.itemRotation.ToRotationVector2() * Item.shootSpeed * player.direction, type, (int)(damage * 0.45f), knockback, player.whoAmI);
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddRecipeGroup("Wood Sword")
				.AddIngredient(ItemID.WandofSparking)
				.Register();
		}
	}
}
