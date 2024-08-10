using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Items.Accessories.Trinket;

namespace BossRush.Contents.Artifacts;
internal class TokenOfSlothArtifact : Artifact {
	public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfSlothPlayer : ModPlayer {
	bool TokenOfSloth = false;
	public int SlothMeter = 0;
	public int Counter_Sloth = 0;
	public int Decay_SlothMeter = 0;
	public override void ResetEffects() {
		TokenOfSloth = Player.HasArtifact<TokenOfSlothArtifact>();
		if (SlothMeter <= 0 || Player.velocity == Vector2.Zero || !TokenOfSloth) {
			return;
		}
		if (++Decay_SlothMeter >= 180) {
			Decay_SlothMeter = 0;
			Counter_Sloth = 0;
			SlothMeter = Math.Clamp(SlothMeter - 1, 0, 10);
		}
	}
	public override void UpdateEquips() {
		if (TokenOfSloth) {
			for (int i = 0; i < SlothMeter; i++) {
				Vector2 pos = Player.Center +
						Vector2.One.Vector2DistributeEvenly(SlothMeter, 360, i)
						.RotatedBy(MathHelper.ToRadians(Player.GetModPlayer<TrinketPlayer>().counterToFullPi)) * 30;
				int dust = Dust.NewDust(pos, 0, 0, DustID.GemTopaz);
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0;
			}
			if (Player.velocity == Vector2.Zero) {
				if (++Counter_Sloth >= 90) {
					SlothMeter = Math.Clamp(SlothMeter + 1, 0, 10);
					Counter_Sloth = 0;
					Decay_SlothMeter = 0;
				}
			}
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, .35f);
			modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, .65f);
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.35f * (1 + SlothMeter * .1f));
			modplayer.AddStatsToPlayer(PlayerStats.FullHPDamage, 1.5f * (1 + SlothMeter * .1f));
		}
	}
}
