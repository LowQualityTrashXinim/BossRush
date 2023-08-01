using BossRush.Common.Global;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class copperRageMode : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        
        public override void Update(Player player, ref int buffIndex)
        {
            Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.Next(-player.width, player.width), Main.rand.Next(-player.height, player.height)), DustID.Electric, Main.rand.NextVector2CircularEdge(player.width, player.height) * 0.1f);
            player.accRunSpeed *= 3;
            player.moveSpeed += 1;
            player.GetDamage(DamageClass.Generic) += 0.075f;
        }

       
    }
}