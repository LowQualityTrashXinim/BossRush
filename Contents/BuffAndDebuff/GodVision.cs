﻿using BossRush.Common.Global;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class GodVision : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Sniper's Vision");
            Description.SetDefault("Let's just hope you don't snap out of it...");
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 70;
            RangeWeaponOverhaul.SpreadModify = 0;
        }
    }
}