﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Artifact
{
    internal class TokenofGreed : ModItem, IArtifactItem
    {
        public int ArtifactID => ArtifactItemID.TokenOfGreed;

        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 32);
            Item.rare = 9;
        }
    }
}