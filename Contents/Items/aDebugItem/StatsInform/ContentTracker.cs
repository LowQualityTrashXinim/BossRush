using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Systems.CursesSystem;
using BossRush.Common.Systems.SpoilSystem;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Perks;
using BossRush.Contents.Skill;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Systems.ArgumentsSystem;

namespace BossRush.Contents.Items.aDebugItem.StatsInform;
internal class ContentTracker : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 10;
		Item.Set_DebugItem(true);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		base.ModifyTooltips(tooltips);
		var line = new TooltipLine(Mod, "StatsShowcase",
			$"LootBox amount : {BossRushModSystem.ListLootboxType.Count}" +
			$"\nSynergy item amount : {BossRushModSystem.SynergyItem.Count}" +
			$"\nLost accessories amount : {BossRushModSystem.LostAccessories.Count}" +
			$"\nArtifact amount : {Artifact.ArtifactCount}" +
			$"\nWeapon enchantment amount : {EnchantmentLoader.TotalCount}" +
			$"\nSkill amount: {SkillModSystem.TotalCount}" +
			$"\nPerk amount : {ModPerkLoader.TotalCount}" +
			$"\nSpoils amount : {ModSpoilSystem.TotalCount}" +
			$"\nRelic template amount : {RelicTemplateLoader.TotalCount}" +
			$"\nAugments amount : {AugmentsLoader.TotalCount}" +
			$"\nCurses taboo amount : {CursesLoader.CurseTabooCount}"
			);
		tooltips.Add(line);
	}
}
