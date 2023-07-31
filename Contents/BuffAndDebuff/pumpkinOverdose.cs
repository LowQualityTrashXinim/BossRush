using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class pumpkinOverdose : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {

            npc.color = Color.DarkOrange;
            



        }

        


    }
}
