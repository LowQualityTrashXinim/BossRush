using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BossRush.Contents.Items;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Artifact
{
    internal class TokenofPride : ArtifactModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 10));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void ArtifactSetDefault()
        {
            width = height = 32;
            Item.rare = ItemRarityID.Cyan;
        }
    }
    public class PridePlayer : ModPlayer
    {
        bool Pride = false; protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
        public override void ResetEffects()
        {
            Pride = Player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == ModContent.ItemType<TokenofPride>();
        }
        public override void PostUpdate()
        {
            if (Pride)
            {
                chestmodplayer.finalMultiplier -= .5f;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Pride)
            {
                float reward = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count * .1f;
                damage += .45f + reward;
            }
        }
    }
}