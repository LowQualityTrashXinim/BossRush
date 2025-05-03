using BossRush.Common.Global;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Perks;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Transfixion.Artifacts;

public class SmallLootBoxArtifact : Artifact {
	public override Color DisplayNameColor => Color.Brown;
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
}
public class SmallLootboxArtifactPlayer : ModPlayer {
	bool SmallLootbox = false;
	public override void ResetEffects() {
		SmallLootbox = Player.HasArtifact<SmallLootBoxArtifact>();
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (SmallLootbox) {
			damage -= .2f;
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (SmallLootbox) {
			modifiers.FinalDamage += .35f;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (SmallLootbox) {
			modifiers.FinalDamage += .35f;
		}
	}
	public override void UpdateEquips() {
		if (SmallLootbox)
			Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.LootDropIncrease, Base: 1);
	}
}
