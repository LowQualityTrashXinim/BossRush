using BossRush.Common.Systems.ArtifactSystem;
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
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (!TokenOfGluttony) {
			return;
		}
		float defensesValue = MathF.Round(Player.statDefense.Positive * (1 + Player.statDefense.AdditiveBonus.Value) - Player.statDefense.Negative, 2);
		float DRvalue = defensesValue / 2f;
		modifiers.ScalingArmorPenetration += 1;
		modifiers.FinalDamage *= Math.Clamp(1 - DRvalue * .01f, .25f, 1f);
		if (Main.rand.NextBool(5)) {
			Player.Heal((int)modifiers.FinalDamage.ApplyTo(npc.damage * .5f));
			modifiers.SetMaxDamage(1);
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (!TokenOfGluttony) {
			return;
		}
		float defensesValue = MathF.Round(Player.statDefense.Positive * (1 + Player.statDefense.AdditiveBonus.Value) - Player.statDefense.Negative, 2);
		float DRvalue = defensesValue / 2f;
		modifiers.ScalingArmorPenetration += 1;
		modifiers.FinalDamage *= Math.Clamp(1 - DRvalue * .01f, .25f, 1f);
		if (Main.rand.NextBool(5)) {
			Player.Heal((int)modifiers.FinalDamage.ApplyTo(proj.damage * .5f));
			modifiers.SetMaxDamage(1);
		}
	}
}
