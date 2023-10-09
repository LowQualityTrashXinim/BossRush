using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BossRush.Common.Systems.ArtifactSystem;
using Microsoft.Xna.Framework;
using Terraria;

namespace BossRush.Contents.Artifacts
{
    internal class BootsOfSpeedManipulationArtifact : Artifact
    {
        int timer;
        public override Color DisplayNameColor => Color.Lerp(Color.BlueViolet, Color.Aqua, (MathF.Sin(timer++ * 0.1f) + 1f) / 2f);
    }
}
