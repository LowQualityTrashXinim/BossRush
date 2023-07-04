using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        //for real, who the fuk came up with these name
        public readonly static int ScreenWidth = Main.PendingResolutionWidth;
        public readonly static int ScreenHeight = Main.PendingResolutionHeight;
        public static void SetPercentageLeft(this UIElement area, float positionToSet, float percentage = 0)
        {
            float position = (positionToSet * percentage);
            if(positionToSet > 0)
            {
                positionToSet *= -1;
                position *= -1;
            }
            area.Left.Set(positionToSet - position, 1f);
        }
        public static void SetPercentageTop(this UIElement area, float positionToSet, float percentage = 0)
        {
            float position = (positionToSet * percentage);
            if (positionToSet > 0)
            {
                positionToSet *= -1;
                position *= -1;
            }
            area.Top.Set(positionToSet - position, 1f);
        }
    }
}
