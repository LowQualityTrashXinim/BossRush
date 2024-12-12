using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Skill;
using BossRush.Common.Utils;
using System.Linq;

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
		var entitySource = player.GetSource_OpenItem(Type);
		if(Main.rand.NextBool(100)) {
			player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.Trinket));
			return;
		}
		LootBoxBase.GetWeapon(out int Weapon, out int amount);
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
		skillplayer.RequestAddSkill_Inventory(Main.rand.Next(SkillModSystem.TotalCount));
	}
}
internal class SpecialSkillLootBox : ModItem {
	public override string Texture => BossRushTexture.PLACEHOLDERCHEST;
	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 30;
		Item.rare = ItemRarityID.LightPurple;
	}
	public override bool CanRightClick() => true;
	public override void RightClick(Player player) {
		SkillHandlePlayer skillplayer = player.GetModPlayer<SkillHandlePlayer>();
		skillplayer.RequestAddSkill_Inventory(Main.rand.Next(SkillModSystem.dict_skill[SkillTypeID.Skill_Projectile].Select(i => i.Type).ToList()));
	}
}
