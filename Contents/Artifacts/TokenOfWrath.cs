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
	public override void UpdateEquips() {
		if(!TokenOfWrath) { 
			return;
		}
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.1f);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, .25f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 50);
		modplayer.NonCriticalDamage += .5f;
	}
}
