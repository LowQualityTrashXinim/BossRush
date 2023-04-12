using BossRush.Common;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofGreed : ModItem,IArtifactItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Token of Greed");
            Tooltip.SetDefault("Greed lower your weapon damage globally inexchange for more item" +
                "\n\"Greed is satified by just having, care not for weapon quality\"");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 9;
        }
    }
    public class GreedyPlayer : ModPlayer
    {
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Player.GetModPlayer<ModdedPlayer>().ArtifactCount <= 1)
            {
                if (Player.HasItem(ModContent.ItemType<TokenofGreed>()))
                {
                    damage -= .35f;
                }
            }
        }
    }
}
