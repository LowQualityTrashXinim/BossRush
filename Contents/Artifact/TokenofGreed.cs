using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifact
{
    internal class TokenofGreed : ArtifactModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<EternalWealth>();
        }
        public override void ArtifactSetDefault()
        {
            width = height = 32;
            Item.rare = ItemRarityID.Cyan;
        }
    }
    public class GreedPlayer : ArtifactPlayerHandleLogic
    {
        bool Greed = false;
        public override void ResetEffects()
        {
            Greed = ArtifactDefinedID == ModContent.ItemType<TokenofGreed>();
        }
        public override void PostUpdate()
        {
            if (Greed)
                chestmodplayer.amountModifier += 4;
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Greed)
                damage *= .65f;
        }
    }
}