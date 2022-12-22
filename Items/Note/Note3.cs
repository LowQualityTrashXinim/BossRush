using Terraria;
using Terraria.ModLoader;
namespace BossRush.Items.Note
{
    internal class Note3 : ModItem
    {
        public override string Texture => "BossRush/Items/Note/Note";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("" +
                "Congrats");
        }
    }
}
