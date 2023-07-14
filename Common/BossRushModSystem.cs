using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using Humanizer;

namespace BossRush.Common
{
    public static class BossRushColor
    {
        /// <summary>
        /// This one will keep track somewhat, will reset if the list you put in is different
        /// This is a semi util that does color tranfering like disco color from <see cref="Main.DiscoColor"/><br/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color MultiColor(List<Color> color, int speed)
        {
            if (progress >= 255)
            {
                progress = 0;
            }
            else
            {
                progress = Math.Clamp(progress + 1 * speed, 0, 255);
            }
            if (color.Count < 1)
            {
                return Color.White;
            }
            if (color.Count < 2)
            {
                return color[0];
            }
            int count = 0;
            foreach(Color c in listcolor)
            {
                if(color.Contains(c))
                {
                    count++;
                }
            }
            if (count != color.Count)
            {
                listcolor = color;
                color1 = new Color();
                color2 = new Color();
            }
            if (color1.Equals(color2))
            {
                color1 = color[currentIndex];
                color3 = color[currentIndex];
                currentIndex = Math.Clamp((currentIndex + 1 >= color.Count) ? 0 : currentIndex + 1, 0, color.Count - 1);
                color2 = color[currentIndex];
                progress = 0;
            }
            if (!color1.Equals(color2))
            {
                color1 = Color.Lerp(color3, color2, Math.Clamp(progress / 255f, 0, 1f));
            }
            return color1;
        }
        private static int currentIndex = 0, progress = 0;
        static Color color1 = new Color(), color2 = new Color(), color3 = new Color();
        static List<Color> listcolor = new List<Color>();
    }
}
