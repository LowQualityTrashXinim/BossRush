using Terraria;
using Terraria.ModLoader;

namespace BossRush.Items.Artifact
{
    internal class TokenofPride : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Token of Pride");
            Tooltip.SetDefault("Increase weapon damage exchange for half of the reward\n" +
                "\"Pride of having the skill to use, care little for reward\"");
        }
        public override string Texture => "BossRush/MissingTexture";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 9;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                .Register();
        }
    }
    public class QualityPlayer : ModPlayer
    {
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Player.GetModPlayer<ModdedPlayer>().ArtifactCount <= 1)
            {
                if (Player.HasItem(ModContent.ItemType<TokenofPride>()))
                {
                    damage += .45f;
                }
            }
        }
    }
}
