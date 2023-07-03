using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Contents.UI
{
    internal class BossRushAchievementButton : UIState
    {
        private UIText text;
        private UIElement area;
        private UIImage barFrame;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 600, 1f);
            area.Top.Set(30, 0f);
            area.Width.Set(182, 0f);
            area.Height.Set(60, 0f);

            barFrame = new UIImage(ModContent.Request<Texture2D>("BossRush/icon"));
            barFrame.Left.Set(0, 0f);
            barFrame.Top.Set(0, 0f);
            barFrame.Width.Set(80, 0f);
            barFrame.Height.Set(80, 0f);

            text = new UIText("Mod Achievement", 0.8f);
            text.Width.Set(138, 0f);
            text.Height.Set(34, 0f);
            text.Top.Set(barFrame.Height.Pixels, 0f);
            text.Left.Set(barFrame.Width.Pixels * .5f - text.Width.Pixels * .5f, 0f);

            area.Append(text);
            area.Append(barFrame);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (true)
                return;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
    class ResponsivePanal : UIState
    {

    }
    class AchievementButton : UIState
    {

    }
}