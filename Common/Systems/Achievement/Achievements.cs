using BossRush.Common.Global;
using BossRush.Common.Mode.HellishEndeavour;
using BossRush.Common.Mode.Nightmare;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Consumable.SpecialReward;
using BossRush.Contents.NPCs;
using BossRush.Contents.Transfixion.Artifacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Common.Systems.Achievement;

public class TheBeginningOfEndless : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Tutorial;
	}
	public override bool Condition() {
		return RoguelikeData.Lootbox_AmountOpen > 0;
	}
	public override void SpecialEffectOnAchieved() {
		Projectile.NewProjectile(null, Main.LocalPlayer.Center, -Vector2.UnitY, ProjectileID.FireworkFountainRainbow, 0, 0);
	}
}

public class TheFirstOfMany : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Tutorial;
	}
	public override bool Condition() {
		return UniversalSystem.DidPlayerBeatTheMod();
	}
}
public class WeaponChallenge1 : ModAchievement {
	public override void SetStaticDefault() {
		DifficultyTag = AchievementTag.Challenge;
	}
	public override bool Condition() {
		Player player = Main.LocalPlayer;
		if (player.TryGetModPlayer(out ModdedPlayer modplayer)) {
			return UniversalSystem.DidPlayerBeatTheMod() && modplayer.UseOnly1ItemSinceTheStartOfTheGame(ItemID.CopperShortsword);
		}
		else {
			return false;
		}
	}
}
