using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.Weapon.NoneSynergyWeapon.Resolve;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class SpectreQuiver : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, Base: 10);
		player.GetModPlayer<SpectreQuiverPlayer>().SpectreQuiver = true;

	}
}
class SpectreQuiverPlayer : ModPlayer {
	public bool SpectreQuiver = false;
	public int timer = 0;
	public override void ResetEffects() {
		SpectreQuiver = false;
		if(timer <= 60) {
			timer++;
		}
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if ((item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Stake) && SpectreQuiver) {
			velocity = (velocity * 1.1f).LimitedVelocity(20f);
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (timer >= 60 && (item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Stake) && SpectreQuiver) {
			for (int i = 0; i < 4; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(4, 40, i), ModContent.ProjectileType<ResolveGhostArrow>(), (int)(damage * .34f), knockback, Player.whoAmI);
			}
			timer = 0;
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
