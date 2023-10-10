using BossRush.Contents.Artifacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Common.Systems.ArtifactSystem
{
    internal class ArtifactPlayer : ModPlayer
    {
        public int ActiveArtifact { get; set; } = 0;

        public override void OnEnterWorld()
        {
            while (ActiveArtifact == Artifact.ArtifactType<RandomArtifact>())
            {
                ActiveArtifact = Main.rand.Next(Artifact.AllArtifacts.Count);
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["ActiveArtifact"] = ActiveArtifact;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("ActiveArtifact", out int value))
            {
                ActiveArtifact = value;
            }
        }
    }
}
