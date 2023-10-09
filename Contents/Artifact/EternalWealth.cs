using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Artifact
{
    internal class EternalWealth : ArtifactModItem
    {
        protected override bool CanBeCraft => false;
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void ArtifactSetDefault()
        {
            width = 32; height = 32;
            Item.rare = ItemRarityID.Cyan;
        }
    }
    class EternalWealthPlayer : ModPlayer
    {
        bool EternalWealth = false;

        int timer = 0;
        Vector2[] oldPos = new Vector2[5];
        int counterOldPos = 0;
        int MidasInfection = 0;
        public override void ResetEffects()
        {
            EternalWealth = Player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == ModContent.ItemType<EternalWealth>();
        }
        protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
        public override void PostUpdate()
        {
            if (EternalWealth)
            {
                chestmodplayer.finalMultiplier += 2;
                timer = BossRushUtils.CoolDown(timer);
                if (timer <= 0)
                {
                    if (counterOldPos >= oldPos.Length - 1)
                    {
                        counterOldPos = 0;
                    }
                    else
                    {
                        counterOldPos++;
                    }
                    oldPos[counterOldPos] = Player.Center;
                    timer = 600;
                }
                float distance = 500;
                bool IsInField = false;
                foreach (Vector2 vec in oldPos)
                {
                    if (Player.Center.IsCloseToPosition(vec, distance))
                    {
                        IsInField = true;
                        MidasInfection++;
                        if (MidasInfection >= 180)
                            Player.statLife = Math.Clamp(Player.statLife - 1, 1, Player.statLifeMax2);
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        int dust = Dust.NewDust(vec + Main.rand.NextVector2Circular(distance, distance), 0, 0, DustID.GoldCoin);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        int dust = Dust.NewDust(vec + Main.rand.NextVector2CircularEdge(distance, distance), 0, 0, DustID.GoldCoin);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
                    }
                }
                if (!IsInField)
                    MidasInfection = BossRushUtils.CoolDown(MidasInfection);
            }
        }
    }
}