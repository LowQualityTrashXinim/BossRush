using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BossRush.Common.Systems.ArtifactSystem
{
    internal class ArtifactSelectionUIButton : UIImageButton
    {
        private int ArtifactType { get; }
        private Player Player { get; }
        private Asset<Texture2D> SelectedBorderTexture { get; }
        private Asset<Texture2D> HoveredBorderTexture { get; }
        public ArtifactSelectionUIButton(int artifactType, Player player) : base(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel"))
        {
            ArtifactType = artifactType;
            Player = player;

            SelectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight");
            HoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();

            base.DrawSelf(spriteBatch);
            if (Player.ActiveArtifact() == ArtifactType)
            {
                spriteBatch.Draw(   
                    SelectedBorderTexture.Value,
                    dimensions.Center(),
                    null,
                    Color.White,
                    0f,
                    SelectedBorderTexture.Size() / 2f,
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            if (IsMouseHovering)
            {
                spriteBatch.Draw(
                    HoveredBorderTexture.Value,
                    dimensions.Position(),
                    Color.White
                );
            }

            Artifact artifact;
            if ((artifact = Artifact.AllArtifacts[ArtifactType]) is not null)
            {
                artifact.DrawInUI(spriteBatch, dimensions);
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            Player.GetModPlayer<ArtifactPlayer>().ActiveArtifact = ArtifactType;
        }
    }
}
