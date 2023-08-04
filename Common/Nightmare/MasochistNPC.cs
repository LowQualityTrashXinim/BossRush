using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Common.YouLikeToHurtYourself
{
    internal class MasochistNPC : GlobalNPC
    {
        private bool PlayerNameContain(string contain)
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].name.Contains(contain))
                {
                    return true;
                }
            }
            return false;
        }
        public override void SetDefaults(NPC npc)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().Nightmare)
            {
                return;
            }
            npc.trapImmune = true;
            npc.lavaImmune = true;
            BossChange(npc);
            if (npc.type == NPCID.ServantofCthulhu)
            {
                npc.scale += 1.5f;
                npc.Size += new Vector2(50, 50);
                npc.lifeMax += 300;
            }
            npc.knockBackResist *= .5f;
        }
        private void BossChange(NPC npc)
        {
            if (!npc.boss)
                return;
            if (npc.type == NPCID.CultistBoss)
            {
                npc.lifeMax += 15000;
                npc.defense += 30;
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                npc.scale -= 0.25f;
                npc.Size -= new Vector2(25, 25);
            }
            if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsBody)
            {
                npc.scale += 2.5f;
                npc.Size += new Vector2(200, 200);
                npc.lifeMax += 1500;
            }
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().Nightmare)
            {
                return;
            }
            npc.damage += Main.rand.Next(npc.damage + 1);
            npc.lifeMax += Main.rand.Next(npc.lifeMax + 1);
            npc.life = npc.lifeMax;
        }
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().Nightmare)
            {
                return;
            }
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.BrokenArmor, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Cursed, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Bleeding, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Burning, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Weak, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.CursedInferno, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Ichor, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Venom, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Poisoned, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Slow, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.ManaSickness, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.PotionSickness, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Obstructed, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Blackout, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Confused, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Darkness, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Electrified, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Stoned, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.WitheredArmor, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.WitheredWeapon, Main.rand.Next(1, 901));
            if (Main.rand.NextBool(10))
                target.AddBuff(BuffID.Suffocation, Main.rand.Next(1, 901));
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (ModContent.GetInstance<BossRushModConfig>().Nightmare)
            {
                maxSpawns += 100;
                spawnRate -= 10;
            }
        }
        public override void PostAI(NPC npc)
        {
            base.PostAI(npc);
            if (!ModContent.GetInstance<BossRushModConfig>().Nightmare)
            {
                return;
            }
            if (npc.type == NPCID.CultistBoss)
            {
                if (npc.ai[0] == 5f)
                {
                    if (npc.ai[1] >= 120f)
                    {
                        npc.chaseable = true;
                        npc.ai[0] = 0f;
                        npc.ai[1] = 0f;
                        npc.ai[3] += 1f;
                        npc.velocity = Vector2.Zero;
                        npc.netUpdate = true;
                    }
                }
                for (int i = 0; i < 300; i++)
                {
                    int dust1 = Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(2000f, 2000f), 0, 0, DustID.SolarFlare);
                    Main.dust[dust1].noGravity = true;
                    Main.dust[dust1].velocity = (Main.dust[dust1].position - npc.Center).SafeNormalize(Vector2.Zero) * 3f;
                }
                if (!BossRushUtils.CompareSquareFloatValue(npc.Center, Main.player[npc.target].Center, 2000))
                {
                    Main.player[npc.target].AddBuff(ModContent.BuffType<AbsoluteStunMovement>(), 120);
                    for (int i = 0; i < 3; i++)
                    {
                        int dust = Dust.NewDust(Main.player[npc.target].Center, 0, 0, DustID.SolarFlare);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(7f, 7f);
                        Main.dust[dust].fadeIn = 2f;
                    }
                }
            }
        }
    }
}