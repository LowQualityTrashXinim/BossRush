using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts;
internal class TokenOfWrathArtifact : Artifact {
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfWrathPlayer : ModPlayer {
	bool TokenOfWrath = false;
	public override void ResetEffects() {
		TokenOfWrath = Player.HasArtifact<TokenOfWrathArtifact>();
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (TokenOfWrath) {
			modifiers.NonCritDamage += .5f;
			modifiers.CritDamage -= .75f;
		}
	}
	public override void ModifyWeaponCrit(Item item, ref float crit) {
		if (TokenOfWrath) {
			crit += 50;
		}
	}
}
