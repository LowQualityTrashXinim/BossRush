using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        //for real, who the fuk came up with these name
        public readonly static int ScreenWidth = Main.PendingResolutionWidth;
        public readonly static int ScreenHeight = Main.PendingResolutionHeight;

        public static void UISetWidthHeight(this UIElement ui, float width, float height)
        {
            ui.Width.Pixels = width;
            ui.Height.Pixels = height;
        }
        public static void UISetPosition(this UIElement ui, Vector2 position, Vector2 origin)
        {
            Vector2 drawpos = position - Main.screenPosition - origin;
            ui.Left.Pixels = drawpos.X + (drawpos.X * (1 - Main.UIScale));
            ui.Top.Pixels = drawpos.Y + (drawpos.X * (1 - Main.UIScale));
        }
    }
}