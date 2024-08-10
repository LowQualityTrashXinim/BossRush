using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using BossRush.Texture;
using Terraria;
using System;

namespace BossRush.Contents.Artifacts;
internal class TokenOfGluttonyArtifact : Artifact {
	public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfGluttonyPlayer : ModPlayer {
	bool TokenOfGluttony = false;
	public override void ResetEffects() {
		TokenOfGluttony = Player.HasArtifact<TokenOfGluttonyArtifact>();
	}
	public override void PostUpdate() {
		if (TokenOfGluttony) {
			float defensesValue = MathF.Round(Player.statDefense.Positive * (1 + Player.statDefense.AdditiveBonus.Value) - Player.statDefense.Negative,2);
			float DRvalue = defensesValue / (float)Player.statLifeMax2;
			Player.endurance += DRvalue;
			Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Multiplicative: 0);
		}
	}
}
