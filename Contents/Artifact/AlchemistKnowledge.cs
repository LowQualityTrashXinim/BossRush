using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using BossRush.Contents.Items.Potion;

namespace BossRush.Contents.Artifact
{
    internal class AlchemistKnowledge : ArtifactModItem
    {
        public override string Texture => base.Texture;
        public override void ArtifactSetDefault()
        {
            width = height = 32;
            Item.rare = ItemRarityID.Cyan;
        }
    }
    class AlchemistKnowledgePlayer : ModPlayer
    {
        bool Alchemist = false;
        public override void ResetEffects()
        {
            Alchemist = Player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == ModContent.ItemType<AlchemistKnowledge>();
        }
        public override void PostUpdate()
        {
            if (Alchemist)
            {
                Player.GetModPlayer<MysteriousPotionPlayer>().PotionPointAddition += 5;
            }
            for (int i = 0; i < Player.inventory.Length; i++)
            {
                if (TerrariaArrayID.MovementPotion.Any(i => i == Player.inventory[i].type) || TerrariaArrayID.NonMovementPotion.Any(i => i == Player.inventory[i].type))
                {
                    Player.inventory[i].active = false;
                    Player.inventory[i].stack = 0;
                }
                if (i >= 50)
                    break;
            }
        }
    }
}