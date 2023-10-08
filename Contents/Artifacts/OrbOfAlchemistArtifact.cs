using BossRush.Common.Utils;
using BossRush.Contents.Items.Potion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Texture;

namespace BossRush.Contents.Artifacts
{
    internal class OrbOfAlchemistArtifact : Artifact
    {
        public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
    }

    class AlchemistKnowledgePlayer : ModPlayer
    {
        bool Alchemist = false;
        public override void ResetEffects()
        {
            Alchemist = Player.HasArtifact<OrbOfAlchemistArtifact>();
        }
        public override void PostUpdate()
        {
            if (Alchemist)
            {
                Player.GetModPlayer<MysteriousPotionPlayer>().PotionPointAddition += 5;
                for (int i = 0; i < Player.inventory.Length; i++)
                {
                    if (TerrariaArrayID.MovementPotion.Any(l => l == Player.inventory[i].type) || TerrariaArrayID.NonMovementPotion.Any(l => l == Player.inventory[i].type))
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
}
