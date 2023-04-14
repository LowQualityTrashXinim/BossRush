using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Terraria.DataStructures;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofPride : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.TOKENOFPRIDE;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Token of Pride");
            Tooltip.SetDefault("\"Pride of having the skill to use, care little for reward\"" +
                "\nPositive Effect : Increase weapon damage by 45%" +
                "\nNegative Effect : Halves your drop from treasure chest");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 6));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 9;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactCount++;
            player.GetModPlayer<QualityPlayer>().TokenOfPride = true;
            return base.UseItem(player);
        }
    }
    public class QualityPlayer : ModPlayer
    {
        public bool TokenOfPride = false;
        public override void PreUpdate()
        {
            if (TokenOfPride)
            {
                Player.GetModPlayer<ChestLootDropPlayer>().multiplier = true;
                Player.GetModPlayer<ChestLootDropPlayer>().amountModifier = .5f;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (TokenOfPride)
            {
                damage += .45f;
            }
        }
    }
}
