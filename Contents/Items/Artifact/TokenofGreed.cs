using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Global;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofGreed : ModItem, IArtifactItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactCount++;
            player.GetModPlayer<GreedyPlayer>().TokenOfGreed = true;
            return true;
        }
    }
    public class GreedyPlayer : ModPlayer
    {
        public bool TokenOfGreed = false;
        public override void PostUpdate()
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
