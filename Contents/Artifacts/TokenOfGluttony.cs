using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using BossRush.Texture;
using Terraria;
using System;

namespace BossRush.Contents.Artifacts;
internal class TokenOfGluttonyArtifact : Artifact {
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfGluttonyPlayer : ModPlayer {
	bool TokenOfGluttony = false;
	public override void ResetEffects() {
		TokenOfGluttony = Player.HasArtifact<TokenOfGluttonyArtifact>();
	}
	public override void UpdateEquips() {
		if (TokenOfGluttony) {
			float defensesValue = MathF.Round(Player.statDefense.Positive * (1 + Player.statDefense.AdditiveBonus.Value) - Player.statDefense.Negative,2);
			float DRvalue = defensesValue / (float)Player.statLifeMax2;
			Player.endurance += DRvalue;
			Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Multiplicative: 0);
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if(!TokenOfGluttony) {
			return;
		}
		if(Main.rand.NextBool(10)) {
			Player.Heal((int)modifiers.FinalDamage.ApplyTo(proj.damage * .1f));
			modifiers.SetMaxDamage(1);
		}
	}
}
