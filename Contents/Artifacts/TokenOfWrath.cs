using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts;
internal class TokenOfWrathArtifact : Artifact {
	public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfWrathPlayer : ModPlayer {
	bool TokenOfWrath = false;
	float critrate = 0;
	public override void ResetEffects() {
		TokenOfWrath = Player.HasArtifact<TokenOfWrathArtifact>();
		critrate = 0;
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (TokenOfWrath) {
			damage += critrate * .01f;
			damage += (Player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage + Player.GetModPlayer<PlayerStatsHandle>().CritDamage);
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (TokenOfWrath) {
			modifiers.DisableCrit();
		}
	}
	public override void ModifyWeaponCrit(Item item, ref float crit) {
		if (TokenOfWrath) {
			critrate = crit;
			crit -= crit * 10;
		}
	}
}
