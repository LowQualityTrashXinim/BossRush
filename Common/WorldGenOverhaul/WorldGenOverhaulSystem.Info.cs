using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BossRush.Common.WorldGenOverhaul
{
    internal partial class WorldGenOverhaulSystem : ModSystem
    {
        public static int DomeRadius { get; set; }
        
        public static Point JungleTopLeft { get; set; }
        public static int JungleWidth { get; set; }
        public static int JungleHeight { get; set; }
    }
}
