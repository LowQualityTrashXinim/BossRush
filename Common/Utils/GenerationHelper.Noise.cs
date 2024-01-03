using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BossRush.Common.Utils
{
    internal partial class GenerationHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		//Xinim : what the point of this ???
        private static int IntFloor(float x) => x < 0 ? (int)x - 1 : (int)x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Lerp(float a, float b, float t) => a + t * (b - a);

        private static (float, float) RandomGradient(int seed, float x, float y)
        {
            float random = x * y * ((seed * 1103515245 + 12345) & 0x7fffffff);
            random *= random * 224f;
            return (MathF.Cos(random), MathF.Sin(random));
        }

        public class PerlinNoise2D
        {
            public int Seed { get; set; }
            public InterpolationType InterpolationType { get; set; }

            public PerlinNoise2D(int seed, InterpolationType interpolationType = InterpolationType.Linear)
            {
                Seed = seed;
                InterpolationType = interpolationType;
            }

            public float GetValue(float x, float y)
            {
                int floorX = IntFloor(x),
                    floorY = IntFloor(y);

                (float lerpX, float lerpY) = InterpolationType switch
                {
                    InterpolationType.Linear => (x - floorX, y - floorY),
                    _ => throw new NotImplementedException()
                };

                return Lerp(
                    Lerp(DotGradient(floorX, floorY, x, y), DotGradient(floorX + 1, floorY, x, y), lerpX),
                    Lerp(DotGradient(floorX, floorY + 1, x, y), DotGradient(floorX + 1, floorY + 1, x, y), lerpX),
                    lerpY
                );
            }

            private float DotGradient(float cornerX, float cornerY, float x, float y)
            {
                (float gradientX, float gradientY) = RandomGradient(Seed, cornerX, cornerY);
                (float offsetX, float offsetY) = (x - cornerX, y - cornerY);

                return offsetX * gradientX + offsetY * gradientY;
            }
        }

        public enum InterpolationType
        {
            Linear
        }
    }
}
