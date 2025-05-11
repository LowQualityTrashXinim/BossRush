using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.CorruptedFlesh;
internal class CorruptedFlesh : ModItem {
	public override void SetDefaults() {
		Item.accessory = true;
		Item.height = 40;
		Item.width = 40;
		Item.rare = ItemRarityID.Lime;
		Item.value = 10000000;
	}
	public override void UpdateAccessory(Player player, bool hideVisual) {
		player.GetDamage(DamageClass.Generic) += 0.1f;
		player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
		player.accRunSpeed += 0.15f;
		player.GetModPlayer<CorruptedFleshPlayer>().CorruptedPower = true;
	}
}
public class CorruptedFleshPlayer : ModPlayer {
	public bool CorruptedPower;
	public override void ResetEffects() {
		CorruptedPower = false;
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (CorruptedPower) {
			Projectile.NewProjectile(source, Player.Center, Main.rand.NextVector2CircularEdge(10, 10), ProjectileID.TinyEater, damage, knockback, Player.whoAmI);
		}
		return true;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		CreateProjectile();
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		CreateProjectile();
	}
	public void CreateProjectile() {
		if (CorruptedPower) {
			for (int i = 0; i < 20; i++) {
				Vector2 Rotate = Vector2.One.Vector2DistributeEvenly(20, 360, i);
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Rotate, ProjectileID.TinyEater, 30, 2f, Player.whoAmI);
			}
		}
	}
}

