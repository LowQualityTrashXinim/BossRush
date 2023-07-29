using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.ChallengeMode
{
    internal class ChallengeModeGlobalNPC : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            return true;
        }
        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.KingSlime)
            {
                KingSlimeAI(npc);
            }
        }
        private void KingSlimeAI(NPC npc)
        {
            float num236 = 1f;
            float num237 = 1f;
            bool flag6 = false;
            bool flag7 = false;
            bool flag8 = false;
            float num238 = 2f;
            if (Main.getGoodWorld)
            {
                num238 -= 1f - npc.life / (float)npc.lifeMax;
                num237 *= num238;
            }

            npc.aiAction = 0;
            if (npc.ai[3] == 0f && npc.life > 0)
                npc.ai[3] = npc.lifeMax;

            if (npc.localAI[3] == 0f)
            {
                npc.localAI[3] = 1f;
                flag6 = true;
                if (Main.netMode != 1)
                {
                    npc.ai[0] = -100f;
                    npc.TargetClosest();
                    npc.netUpdate = true;
                }
            }

            int num239 = 3000;
            if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > num239)
            {
                npc.TargetClosest();
                if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > num239)
                {
                    npc.EncourageDespawn(10);
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                        npc.direction = 1;
                    else
                        npc.direction = -1;

                    if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[1] != 5f)
                    {
                        npc.netUpdate = true;
                        npc.ai[2] = 0f;
                        npc.ai[0] = 0f;
                        npc.ai[1] = 5f;
                        npc.localAI[1] = Main.maxTilesX * 16;
                        npc.localAI[2] = Main.maxTilesY * 16;
                    }
                }
            }

            if (!Main.player[npc.target].dead && npc.timeLeft > 10 && npc.ai[2] >= 300f && npc.ai[1] < 5f && npc.velocity.Y == 0f)
            {
                npc.ai[2] = 0f;
                npc.ai[0] = 0f;
                npc.ai[1] = 5f;
                if (Main.netMode != 1)
                {
                    npc.TargetClosest(false);
                    Point point3 = npc.Center.ToTileCoordinates();
                    Point point4 = Main.player[npc.target].Center.ToTileCoordinates();
                    Vector2 vector30 = Main.player[npc.target].Center - npc.Center;
                    int num240 = 10;
                    int num241 = 0;
                    int num242 = 7;
                    int num243 = 0;
                    bool flag9 = false;
                    if (npc.localAI[0] >= 360f || vector30.Length() > 2000f)
                    {
                        if (npc.localAI[0] >= 360f)
                            npc.localAI[0] = 360f;

                        flag9 = true;
                        num243 = 100;
                    }

                    while (!flag9 && num243 < 100)
                    {
                        num243++;
                        int num244 = Main.rand.Next(point4.X - num240, point4.X + num240 + 1);
                        int num245 = Main.rand.Next(point4.Y - num240, point4.Y + 1);
                        if ((num245 >= point4.Y - num242 && num245 <= point4.Y + num242 && num244 >= point4.X - num242 && num244 <= point4.X + num242) || (num245 >= point3.Y - num241 && num245 <= point3.Y + num241 && num244 >= point3.X - num241 && num244 <= point3.X + num241) || Main.tile[num244, num245].HasUnactuatedTile)
                            continue;

                        int num246 = num245;
                        int num247 = 0;
                        if (Main.tile[num244, num246].HasUnactuatedTile && Main.tileSolid[Main.tile[num244, num246].TileType] && !Main.tileSolidTop[Main.tile[num244, num246].TileType])
                        {
                            num247 = 1;
                        }
                        else
                        {
                            for (; num247 < 150 && num246 + num247 < Main.maxTilesY; num247++)
                            {
                                int num248 = num246 + num247;
                                if (Main.tile[num244, num248].HasUnactuatedTile && Main.tileSolid[Main.tile[num244, num248].TileType] && !Main.tileSolidTop[Main.tile[num244, num248].TileType])
                                {
                                    num247--;
                                    break;
                                }
                            }
                        }

                        num245 += num247;
                        bool flag10 = true;
                        if (flag10 && (Main.tile[num244, num245].LiquidType == LiquidID.Lava))
                            flag10 = false;

                        if (flag10 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                            flag10 = false;

                        if (flag10)
                        {
                            npc.localAI[1] = num244 * 16 + 8;
                            npc.localAI[2] = num245 * 16 + 16;
                            break;
                        }
                    }

                    if (num243 >= 100)
                    {
                        Vector2 bottom = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
                        npc.localAI[1] = bottom.X;
                        npc.localAI[2] = bottom.Y;
                    }
                }
            }

            if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) || Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 160f)
            {
                npc.ai[2]++;
                if (Main.netMode != 1)
                    npc.localAI[0]++;
            }
            else if (Main.netMode != 1)
            {
                npc.localAI[0]--;
                if (npc.localAI[0] < 0f)
                    npc.localAI[0] = 0f;
            }

            if (npc.timeLeft < 10 && (npc.ai[0] != 0f || npc.ai[1] != 0f))
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
                flag7 = false;
            }

            Dust dust;
            if (npc.ai[1] == 5f)
            {
                flag7 = true;
                npc.aiAction = 1;
                npc.ai[0]++;
                num236 = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
                num236 = 0.5f + num236 * 0.5f;
                if (npc.ai[0] >= 60f)
                    flag8 = true;

                if (npc.ai[0] == 60f)
                    Gore.NewGore(npc.GetSource_FromAI(), npc.Center + new Vector2(-40f, -npc.height / 2), npc.velocity, 734);

                if (npc.ai[0] >= 60f && Main.netMode != 1)
                {
                    npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                }

                if (Main.netMode == 1 && npc.ai[0] >= 120f)
                {
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                }

                if (!flag8)
                {
                    for (int num249 = 0; num249 < 10; num249++)
                    {
                        int num250 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                        Main.dust[num250].noGravity = true;
                        dust = Main.dust[num250];
                        dust.velocity *= 0.5f;
                    }
                }
            }
            else if (npc.ai[1] == 6f)
            {
                flag7 = true;
                npc.aiAction = 0;
                npc.ai[0]++;
                num236 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
                num236 = 0.5f + num236 * 0.5f;
                if (npc.ai[0] >= 30f && Main.netMode != 1)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                    npc.TargetClosest();
                }

                if (Main.netMode == 1 && npc.ai[0] >= 60f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.TargetClosest();
                }

                for (int num251 = 0; num251 < 10; num251++)
                {
                    int num252 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                    Main.dust[num252].noGravity = true;
                    dust = Main.dust[num252];
                    dust.velocity *= 2f;
                }
            }

            npc.dontTakeDamage = (npc.hide = flag8);
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X *= 0.8f;
                if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                    npc.velocity.X = 0f;

                if (!flag7)
                {
                    npc.ai[0] += 2f;
                    if (npc.life < npc.lifeMax * 0.8f)
                        npc.ai[0] += 1f;

                    if (npc.life < npc.lifeMax * 0.6f)
                        npc.ai[0] += 1f;

                    if (npc.life < npc.lifeMax * 0.4f)
                        npc.ai[0] += 2f;

                    if (npc.life < npc.lifeMax * 0.2f)
                        npc.ai[0] += 3f;

                    if (npc.life < npc.lifeMax * 0.1f)
                        npc.ai[0] += 4f;

                    if (npc.ai[0] >= 0f)
                    {
                        npc.netUpdate = true;
                        npc.TargetClosest();
                        switch (npc.ai[1])
                        {
                            case 2:
                            npc.velocity.Y = -6f;
                            npc.velocity.X += 4.5f * npc.direction;
                            npc.ai[0] = -120f;
                            npc.ai[1] += 1f;
                                break;
                            case 3:
                            npc.velocity.Y = -13f;
                            npc.velocity.X += 3.5f * npc.direction;
                            npc.ai[0] = -200f;
                            npc.ai[1] = 0f;
                                break;
                            default:
                            npc.velocity.Y = -8f;
                            npc.velocity.X += 4f * npc.direction;
                            npc.ai[0] = -120f;
                            npc.ai[1] += 1f;
                                break;
                        }
                    }
                    else if (npc.ai[0] >= -30f)
                    {
                        npc.aiAction = 1;
                    }
                }
            }
            else if (npc.target < 255)
            {
                float num253 = 3f;
                if (Main.getGoodWorld)
                    num253 = 6f;

                if ((npc.direction == 1 && npc.velocity.X < num253) || (npc.direction == -1 && npc.velocity.X > 0f - num253))
                {
                    if ((npc.direction == -1 && npc.velocity.X < 0.1) || (npc.direction == 1 && npc.velocity.X > -0.1))
                        npc.velocity.X += 0.2f * npc.direction;
                    else
                        npc.velocity.X *= 0.93f;
                }
            }

            int num254 = Dust.NewDust(npc.position, npc.width, npc.height, 4, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
            Main.dust[num254].noGravity = true;
            dust = Main.dust[num254];
            dust.velocity *= 0.5f;
            if (npc.life <= 0)
                return;

            float num255 = npc.life / (float)npc.lifeMax;
            num255 = num255 * 0.5f + 0.75f;
            num255 *= num236;
            num255 *= num237;
            if (num255 != npc.scale || flag6)
            {
                npc.position.X += npc.width / 2;
                npc.position.Y += npc.height;
                npc.scale = num255;
                npc.width = (int)(98f * npc.scale);
                npc.height = (int)(92f * npc.scale);
                npc.position.X -= npc.width / 2;
                npc.position.Y -= npc.height;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            int num256 = (int)(npc.lifeMax * .05f);
            if (!((npc.life + num256) < npc.ai[3]))
                return;

            npc.ai[3] = npc.life;
            int num257 = Main.rand.Next(1, 4);
            for (int num258 = 0; num258 < num257; num258++)
            {
                int x = (int)(npc.position.X + Main.rand.NextFloat(npc.width - 32));
                int y = (int)(npc.position.Y + Main.rand.NextFloat(npc.height - 32));
                int num259 = 1;
                if (Main.expertMode && Main.rand.NextBool(4))
                    num259 = 535;

                int slimeMinion = NPC.NewNPC(npc.GetSource_FromAI(), x, y, num259);
                Main.npc[slimeMinion].SetDefaults(num259);
                Main.npc[slimeMinion].velocity.X = Main.rand.NextFloat(-15, 16) * 0.1f;
                Main.npc[slimeMinion].velocity.Y = Main.rand.NextFloat(-30, 1) * 0.1f;
                Main.npc[slimeMinion].ai[0] = -1000 * Main.rand.Next(3);
                Main.npc[slimeMinion].ai[1] = 0f;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(23, -1, -1, null, slimeMinion);
            }
        }
        public override void PostAI(NPC npc)
        {
            if (BossRushUtils.IsAnyVanillaBossAlive())
            {
                if (npc.type == NPCID.Nurse)
                {
                    npc.StrikeInstantKill();
                }
            }
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                //Re add removing shop soon
            }
        }
    }
}
