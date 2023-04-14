using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Global;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofGreed : ModItem, IArtifactItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Token of Greed");
            Tooltip.SetDefault("\"Greed is satified by just having, care not for weapon quality\"" +
                "\nPositive Effect : Increase amount of drop by 4" +
                "\nNegative Effect : Decrease weapon damage globally by 35%");
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
            player.GetModPlayer<GreedyPlayer>().TokenOfGreed = true;
            return base.UseItem(player);
        }
    }
    public class GreedyPlayer : ModPlayer
    {
        public bool TokenOfGreed = false;
        public override void PreUpdate()
        {
            if (TokenOfGreed)
            {
                Player.GetModPlayer<ChestLootDropPlayer>().amountModifier += 4;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (TokenOfGreed)
            {
                damage *= .65f;
            }
        }
    }
}
