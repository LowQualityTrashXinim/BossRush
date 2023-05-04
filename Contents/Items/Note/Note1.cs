using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Note
{
    internal class Note1 : ModItem
    {
        public override string Texture => BossRushTexture.NOTE;
        public override void SetDefaults()
        {
            Item.width = 41;
            Item.height = 29;
            Item.material = true;
            Item.rare = 0;
        }
    }
}
