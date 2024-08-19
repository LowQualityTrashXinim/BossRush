using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.Systems;
using BossRush.Contents.Perks;
using BossRush.Contents.Skill;
using Microsoft.Xna.Framework;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts;
internal class GamblerSoulArtifact : Artifact {
	public override Color DisplayNameColor => Color.LightGoldenrodYellow;
	public override string TexturePath => BossRushTexture.MissingTexture_Default;
}
public class GamblerSoulPlayer : ModPlayer {
	bool GamblerSoul = false;
	int AlreadyGotGivenItem = 0;
	public override void ResetEffects() {
		GamblerSoul = Player.HasArtifact<GamblerSoulArtifact>();
	}
	public override void OnEnterWorld() {
		if (GamblerSoul && AlreadyGotGivenItem == 0) {
			Player.GetModPlayer<SkillHandlePlayer>().RequestAddSkill_Inventory(ModSkill.GetSkillType<AllOrNothing>(), false);
			Player.QuickSpawnItem(null, ModContent.ItemType<LuckEssence>());
			AlreadyGotGivenItem++;
		}
	}
	public override bool CanUseItem(Item item) {
		if (GamblerSoul)
			return item.type != ModContent.ItemType<EnchantmentTablet>();
		return base.CanUseItem(item);
	}
	public override void PostUpdate() {
		if (GamblerSoul)
			Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.LootDropIncrease, Base: 1);
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (GamblerSoul)
			modifiers.DamageVariationScale *= 1.65f;
	}
	public override void SaveData(TagCompound tag) {
		tag.Add("AlreadyGotGivenItem", AlreadyGotGivenItem);
	}
	public override void LoadData(TagCompound tag) {
		AlreadyGotGivenItem = (int)tag["AlreadyGotGivenItem"];
	}
}
