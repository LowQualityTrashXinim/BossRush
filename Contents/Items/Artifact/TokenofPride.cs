using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using BossRush.Common;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofPride : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.TOKENOFPRIDE;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Token of Pride");
            Tooltip.SetDefault("Increase weapon damage exchange for half of the reward\n" +
                "\"Pride of having the skill to use, care little for reward\"");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 9;
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
