using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon;
internal class SlipGun : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.BossRushDefaultRange(30, 30, 50, 5f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.Bullet, 30, true, AmmoID.Bullet);

		Item.rare = ItemRarityID.Pink;
		Item.value = Item.sellPrice(gold: 50);
	}
}
