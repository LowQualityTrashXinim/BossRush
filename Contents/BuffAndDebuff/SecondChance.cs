﻿using Terraria;
using Terraria.ModLoader;
using BossRush.Texture;
using BossRush.Contents.Items.Artifact;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class SecondChance : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("SecondChance");
            // Description.SetDefault("You had survived a fatal attack, so don't get hit a 2nd time");
            Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = false;
        }
    }
}
