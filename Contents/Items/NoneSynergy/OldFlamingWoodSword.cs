using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace BossRush.Contents.Items.NoneSynergy;
internal class OldFlamingWoodSword : ModItem {
	public override void SetDefaults() {
		Item.BossRushSetDefault(32, 36, 22, 5f, 4, 40, 1, false);
		Item.DamageType = DamageClass.Melee;

		Item.crit = 5;
		Item.useTurn = false;
		Item.UseSound = SoundID.Item1;
		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 50);
		Item.shoot = ProjectileID.WandOfSparkingSpark;
		Item.shootSpeed = 6;
	}
	public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire, 180);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		var vel = (player.itemRotation + MathHelper.PiOver4 * -player.direction).ToRotationVector2() * 3 * player.direction;
		Projectile.NewProjectile(source, position.PositionOFFSET(vel, 30), vel, type, (int)(damage * 0.45f), knockback, player.whoAmI);
		return false;
	}
	//public override void AddRecipes() {
	//	CreateRecipe()
	//		.AddRecipeGroup("Wood Sword")
	//		.AddIngredient(ItemID.WandofSparking)
	//		.Register();
	//}

}
