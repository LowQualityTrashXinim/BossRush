using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;
internal class DivineHammer : ModItem {
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.rare = ItemRarityID.Red;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item37;
		Item.Set_InfoItem();
	}
	public override bool? UseItem(Player player) {
		if (!UniversalSystem.CanEnchantmentBeAccess()) {
			BossRushUtils.CombatTextRevamp(player.Hitbox, Color.Red, "Can't access enchantment ui");
			return false;
		}
		if (player.ItemAnimationJustStarted) {
			ModContent.GetInstance<UniversalSystem>().ActivateEnchantmentUI();
		}
		return false;
	}
}
