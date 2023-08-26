using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items
{
    internal class CelestialWrath : ModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
        }
        public override bool? UseItem(Player player)
        {
            return true;
        }
    }
}