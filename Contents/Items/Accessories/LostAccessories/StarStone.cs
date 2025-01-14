using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class StarStone : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<StarStonePlayer>().StarStone = true;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.SynergyDamage, Additive: 1.15f);
	}
}
class StarStonePlayer : ModPlayer {
	public bool StarStone = false;
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (StarStone && Player.GetModPlayer<SynergyModPlayer>().CompareOldvsNewItemType) {
			Vector2 rotate = (Vector2.UnitX * 125).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(90)));
			for (int i = 0; i < 3; i++) {
				Vector2 rotation = rotate.Vector2DistributeEvenly(3, 360, i);
				Vector2 newPos = Main.MouseWorld + rotation;
				Vector2 newVel = -rotation.SafeNormalize(Vector2.Zero) * 10f;
				int proj = Projectile.NewProjectile(source, newPos, newVel, ProjectileID.StarCannonStar, (int)Player.GetDamage(DamageClass.Generic).ApplyTo(65), 3f, Player.whoAmI);
				Main.projectile[proj].timeLeft = 30;
				Main.projectile[proj].tileCollide = false;
			}
		}
		return base.Shoot(item,source, position, velocity, type, damage, knockback);
	}
}
