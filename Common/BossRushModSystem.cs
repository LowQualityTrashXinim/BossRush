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
                if (RBred > 0)
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
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color MultiColor(List<Color> color)
        {
            return Color.White;
        }
    }
}
