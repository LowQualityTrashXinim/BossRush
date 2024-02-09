using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts
{
    internal class BootsOfSpeedManipulationArtifact : Artifact
    {
        int timer;
        public override Color DisplayNameColor => Color.Lerp(Color.BlueViolet, Color.Aqua, (MathF.Sin(timer++ * 0.1f) + 1f) / 2f);
    }

	class BootSpeedPlayer : ModPlayer {
		bool BootofSpeed = false;
		public override void ResetEffects() {
			BootofSpeed = Player.HasArtifact<BootsOfSpeedManipulationArtifact>();
			if (BootofSpeed) {
				Player.moveSpeed += 1f;
				Player.maxFallSpeed += 2f;
				Player.runAcceleration += .5f;
				Player.jumpSpeed += 3f;
				Player.noFallDmg = true;
				Player.accRunSpeed += 5f;
			}
		}
		public override void PostUpdate() {
			if (BootofSpeed) {
				Player.wingTime *= 0;
				Player.wingAccRunSpeed *= 0;
				Player.wingRunAccelerationMult *= 0;
				Player.wingTimeMax = 0;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (BootofSpeed){
				if (Player.velocity.IsLimitReached(5)){
					damage *= Main.rand.NextFloat(.3f, 1f);
				}
			}
		}
	}
}
