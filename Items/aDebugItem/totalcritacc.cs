using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Items.aDebugItem
{
    internal class totalcritacc : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("100% crit");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 100;
        }
    }
}
