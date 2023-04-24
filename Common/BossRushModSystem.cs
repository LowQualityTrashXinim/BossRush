using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Common
{
    internal class BossRushModSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            ChallengeGodColorAnimation();
            YellowPulseYellowWhiteAnimation();
            SynergyColorAnimation();
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
        private void YellowPulseYellowWhiteAnimation()
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
        static int BWRed = 0, BWGreen = 150, BWBlue = 150;
        int Switch3 = 0;
        private void SynergyColorAnimation()
        {
            //Pulsing Yellow
            if (Switch3 != 1)
            {
                if (BWBlue < 255)
                {//default value 100, 100, 0
                    Math.Clamp(++BWRed, 0, 255);
                    Math.Clamp(++BWGreen, 150, 255);
                    Math.Clamp(++BWBlue, 150, 255);
                }
                else
                {
                    Switch3 = 1;
                }
            }
            else
            {
                if (BWBlue > 150)
                {
                    Math.Clamp(--BWRed, 0, 255);
                    Math.Clamp(--BWGreen, 150, 255);
                    Math.Clamp(--BWBlue, 150, 255);
                }
                else
                {
                    Switch3 = 0;
                }
            }
        }
    }
}
