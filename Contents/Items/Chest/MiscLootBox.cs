using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Skill;

namespace BossRush.Contents.Items.Chest;
internal class WeaponLootBox : ModItem {
	public override string Texture => BossRushTexture.PLACEHOLDERCHEST;
	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 30;
		Item.rare = ItemRarityID.LightPurple;
	}
	public override bool CanRightClick() => true;
	public override void RightClick(Player player) {
		LootBoxBase.GetWeapon(out int Weapon, out int amount);
		var entitySource = player.GetSource_OpenItem(Type);
		player.QuickSpawnItem(entitySource, Weapon, amount);
	}
}
internal class SkillLootBox : ModItem {
	public override string Texture => BossRushTexture.PLACEHOLDERCHEST;
	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 30;
		Item.rare = ItemRarityID.LightPurple;
	}
	public override bool CanRightClick() => true;
	public override void RightClick(Player player) {
		SkillHandlePlayer skillplayer = player.GetModPlayer<SkillHandlePlayer>();
		skillplayer.RequestAddSkill_Inventory(Main.rand.Next(SkillLoader.TotalCount));
	}
}
