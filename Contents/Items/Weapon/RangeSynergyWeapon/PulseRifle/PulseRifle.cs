using Terraria.ID;
using BossRush.Texture;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.PulseRifle;
internal class PulseRifle : SynergyModItem {
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.BossRushDefaultRange(94, 34, 34, 4f, 7, 7, ItemUseStyleID.Shoot, ProjectileID.PulseBolt, 16f, true, AmmoID.Bullet);
		Item.scale = .78f;
		Item.UseSound = SoundID.Item75 with { Pitch = 1 };
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-20, 0);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.PulseBolt;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.PulseBow)
			.AddIngredient(ItemID.Megashark)
			.Register();
	}
}
