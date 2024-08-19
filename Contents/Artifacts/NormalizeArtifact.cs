using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossRush.Contents.Artifacts
{
    internal class NormalizeArtifact : Artifact
    {
        public override string TexturePath => BossRushTexture.MissingTexture_Default;
    }
}
