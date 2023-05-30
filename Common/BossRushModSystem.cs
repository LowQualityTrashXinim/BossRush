using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Common
{
    internal class BossRushModSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            ChallengeGodColorAnimation();
            YellowPulseYellowWhiteColorAnimation(); 
            SynergyColorAnimation();
            RedToBlackColorAnimation();
        }

        public static Color ChallangeGodColor => new Color(ChallengeR, 0, ChallengeB);
        static int ChallengeR = 100, ChallengeB = 100;
        int Switch1 = 0;
        private void ChallengeGodColorAnimation()
        {
            //Pulsing Purple
            if (Switch1 != 1)
            {
                if (ChallengeR < 255)
                {
                    Math.Clamp(++ChallengeR, 100, 255);
                    Math.Clamp(++ChallengeB, 100, 255);
                }
                else
                {
                    Switch1 = 1;
                }
            }
            else
            {
                if (ChallengeR > 100)
                {
                    Math.Clamp(--ChallengeR, 100, 255);
                    Math.Clamp(--ChallengeB, 100, 255);
                }
                else
                {
                    Switch1 = 0;
                }
            }
        }

        public static Color YellowPulseYellowWhite => new Color(YWRed, YWGreen, YWBlue);
        static int YWRed = 150, YWGreen = 150, YWBlue = 0;
        int Switch2 = 0;
        private void YellowPulseYellowWhiteColorAnimation()
        {
            //Pulsing Yellow
            if (Switch2 != 1)
            {
                if (YWRed < 255)
                {//default value 100, 100, 0
                    Math.Clamp(++YWRed, 150, 255);
                    Math.Clamp(++YWGreen, 150, 255);
                    Math.Clamp(++YWBlue, 0, 255);
                }
                else
                {
                    Switch2 = 1;
                }
            }
            else
            {
                if (YWRed > 150)
                {
                    Math.Clamp(--YWRed, 150, 255);
                    Math.Clamp(--YWGreen, 150, 255);
                    Math.Clamp(--YWBlue, 0, 255);
                }
                else
                {
                    Switch2 = 0;
                }
            }
        }
        public static Color SynergyColor => new Color(BWRed, BWGreen, BWBlue);
        static int BWRed = 50, BWGreen = 175, BWBlue = 175;
        int Switch3 = 0;
        private void SynergyColorAnimation()
        {

            if (Switch3 != 1)
            {
                BWRed = Math.Clamp(BWRed + 4, 50, 225);
                BWGreen = Math.Clamp(BWGreen + 1, 175, 225);
                BWBlue = Math.Clamp(BWBlue + 1, 175, 225);
                if (BWBlue >= 225)
                {
                    Switch3 = 1;
                }
            }
            else
            {
                BWRed = Math.Clamp(BWRed - 4, 50, 225);
                BWGreen = Math.Clamp(BWGreen - 1, 175, 225);
                BWBlue = Math.Clamp(BWBlue - 1, 175, 225);
                if (BWBlue <= 175)
                {
                    Switch3 = 0;
                }
            }
        }
        public static Color RedToBlack => new Color(RBred, 0, 0);
        static int RBred = 255;
        int Switch4 = 0;
        private void RedToBlackColorAnimation()
        {
            //Pulsing Purple
            if (Switch4 != 1)
            {
                if (RBred < 255)
                {
                    ++RBred;
                }
                else
                {
                    Switch4 = 1;
                }
            }
            else
            {
                if (RBred > 50)
                {
                    --RBred;
                }
                else
                {
                    Switch4 = 0;
                }
            }
        }
        /// <summary>
        /// This one will keep track somewhat, will reset if the list you put in is different
        /// This is a semi util that does color tranfering like disco color from <see cref="Main.DiscoColor"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color MultiColor(List<Color> color, int speed)
        {
            if(color.Count < 1)
            {
                return Color.White;
            }
            if(color.Count < 2)
            {
                return color[0];
            }
            if(!listofMultiColor.Equals(color))
            {
                listofMultiColor.Clear();
                listofMultiColor = color;
            }
            if(listofMultiColor.Count < 1)
            {
                listofMultiColor = color;
            }
            if (color1 == color2)
            {
                color1 = color[currentIndex];
                currentIndex += currentIndex + 1 >= color.Count ? color.Count : 1;
                color2 = color[currentIndex];
            }
            if (color1 != color2)
            {
                Vector4 vec4 = new Vector4(Math.Clamp(color1.R + (color1.R == color2.R ? 0 : color1.R < color2.R ? speed : -speed), color1.R, color2.R),
                Math.Clamp(color1.G + (color1.G == color2.G ? 0 : color1.G < color2.G ? speed : -speed), color1.G, color2.G),
                Math.Clamp(color1.B + (color1.B == color2.B ? 0 : color1.B < color2.B ? speed : -speed), color1.B, color2.B),
                Math.Clamp(color1.R + (color1.A == color2.A ? 0 : color1.A < color2.A ? speed : -speed), color1.A, color2.A));
                color1 = new Color((int)vec4.X, (int)vec4.Y, (int)vec4.Z, (int)vec4.W);
            }
            return Color.White;
        }
        static List<Color> listofMultiColor = new List<Color>();
        static Color color1, color2;
        static int currentIndex = 0;
    }
}
