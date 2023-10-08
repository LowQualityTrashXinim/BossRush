using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace BossRush.Contents.Artifacts
{
    internal class MagicalCardDeckArtifact : Artifact
    {
        public override int Frames => 9;
        /*int timer;
        public override void DrawInUI(SpriteBatch spriteBatch, CalculatedStyle dimensions)
        {
            Rectangle source = Texture.Value.GetSource(9, timer++ / 6);
            spriteBatch.Draw(
                Texture.Value,
                dimensions.Center(),
                source,
                Color.White,
                0f,
                source.Size() / 2f,
                (dimensions.Height - 12f) / source.Height,
                SpriteEffects.None,
                0f
            );
        }*/
    }
}
