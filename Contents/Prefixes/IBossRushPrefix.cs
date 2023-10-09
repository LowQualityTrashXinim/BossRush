using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace BossRush.Contents.Prefixes
{
    interface IBossRushPrefix
    {
        public PrefixCategory WeaponType { get; }
        public float WeaponRollchance { get; }
    }
}
