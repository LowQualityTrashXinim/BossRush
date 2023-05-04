using Terraria;
using Terraria.ModLoader;
using BossRush.Texture;

namespace BossRush.Contents.Items.Note
{
    internal class Note2 : ModItem
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
