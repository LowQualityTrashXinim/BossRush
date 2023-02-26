using Terraria;
using Terraria.ModLoader;

namespace BossRush.Items.Artifact
{
    internal class totalcritacc : ModItem
    {
        public override string Texture => "BossRush/MissingTexture";
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
