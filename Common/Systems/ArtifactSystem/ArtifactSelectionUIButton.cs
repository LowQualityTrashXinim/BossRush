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
		private Asset<Texture2D> LockTexture { get; }
		public ArtifactSelectionUIButton(int artifactType, Player player) : base(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel"))
        {
            ArtifactType = artifactType;
            Player = player;

            SelectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight");
            HoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder");
			LockTexture = ModContent.Request<Texture2D>("BossRush/Texture/UI/lock");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
			Artifact artifact = Artifact.GetArtifact(ArtifactType);

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

			artifact.DrawInUI(spriteBatch, dimensions);

			if (!artifact.CanBeSelected(Player) && IsMouseHovering) {
				spriteBatch.Draw(
					LockTexture.Value,
					dimensions.Center(),
					null,
					Color.White,
					0f,
					LockTexture.Size() / 2f,
					1f,
					SpriteEffects.None,
					0f
				);
			}
        }

        public override void LeftClick(UIMouseEvent evt)
        {
			Artifact artifact = Artifact.GetArtifact(ArtifactType);
			if (artifact.CanBeSelected(Player)) {
				SoundEngine.PlaySound(SoundID.MenuTick);
				Player.GetModPlayer<ArtifactPlayer>().ActiveArtifact = ArtifactType;
			} else {
				SoundEngine.PlaySound(SoundID.MenuClose);
			}
            
        }
    }
}
