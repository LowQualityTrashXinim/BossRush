using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Common.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.Chest;
using System.Collections.Generic;

namespace BossRush.Contents.NPCs
{
    //To ensure the code is readable and consistent throughout the process of making
    //ai[0] => timer
    //ai[1] => ai switch
    //ai[2] => counter ( not to be confused with timer )
    //The code in this file must follow the above rule
    internal class ChestLord : ModNPC
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
        public override void SetDefaults()
        {
            NPC.lifeMax = 6000;
            NPC.damage = 20;
            NPC.defense = 20;
            NPC.width = 60;
            NPC.height = 60;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.npcSlots = 6f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
        }
        //Use NPC.ai[0] to delay attack
        //Use NPC.ai[1] to switch attack
        //Use NPC.ai[2] for calculation
        //Use NPC.ai[3] to do movement
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (player.dead || !player.active)
            {
                NPC.active = false;
            }
            //Move above the player
            switch (NPC.ai[1])
            {
                case 0:
                    Move(player);
                    break;
                case 1:
                    ShootShortSword();
                    break;
                case 2:
                    ShootShortSword2();
                    break;
                case 3:
                    ShootBroadSword();
                    break;
                case 4:
                    ShootBroadSword2();
                    break;
            }
            ActivateBroadSword();
        }
        /// <summary>
        /// This is a way to make NPC itself handle it own projectile and activate as will
        /// Make sure there are no junk data or overlapped attack
        /// </summary>
        private void ActivateBroadSword()
        {
            List<SwordBroadAttackOne> broadSwordProjectile = new List<SwordBroadAttackOne>();
            for (int i = 0; i < ProjectileWhoAmI.Count; i++)
            {
                Projectile projectile = Main.projectile[ProjectileWhoAmI[i]];
                if (projectile.ModProjectile is SwordBroadAttackOne swordProj && projectile.ai[1] >= 3)
                {
                    broadSwordProjectile.Add(swordProj);
                }
            }
            if (broadSwordProjectile.Count >= TerrariaArrayID.AllOreBroadSword.Length)
            {
                foreach (SwordBroadAttackOne proj in broadSwordProjectile)
                {
                    proj.CanProgressToAI3 = true;
                }
                //We clean junk data here
                //Since we have proven that all of them are here, and most likely this attack won't change in term of number
                //We should clear the list so we can reuse the list
                ProjectileWhoAmI.Clear();
            }
        }
        public override void OnKill()
        {
            foreach (var projIndex in ProjectileWhoAmI)
            {
                Projectile projectile = Main.projectile[projIndex];
                if (projectile == null)
                {
                    continue;
                }
                if (projectile.active)
                {
                    projectile.Kill();
                }
                ProjectileWhoAmI.Clear();
            }
        }
        List<int> ProjectileWhoAmI = new List<int>();
        private void Move(Player player)
        {
            if (BossDelayAttack(0, 0, 0))
            {
                return;
            }
            Vector2 positionAbovePlayer = new Vector2(player.Center.X, player.Center.Y - 300);
            if (NPC.NPCMoveToPosition(positionAbovePlayer, 30f))
            {
                NPC.ai[0] = 20;
                NPC.ai[1] = Main.rand.Next(1, 5);
            }
        }
        private void ResetEverything()
        {
            NPC.ai[0] = 90;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
            NPC.ai[3] = 0;
        }
        private void ShootShortSword()
        {
            if (BossDelayAttack(10, 0, TerrariaArrayID.AllOreShortSword.Length - 1))
            {
                return;
            }
            Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)NPC.ai[2]) * 15f;
            int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackOne>(), NPC.damage, 2);
            Main.projectile[proj].ai[2] = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
            Main.projectile[proj].owner = NPC.target;
            NPC.ai[2]++;
        }
        private void ShootShortSword2()
        {
            Vector2 positionAbovePlayer = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 350);
            NPC.NPCMoveToPosition(positionAbovePlayer, 5f);
            if (BossDelayAttack(20, 0, TerrariaArrayID.AllOreShortSword.Length - 1))
            {
                return;
            }
            Vector2 vec = Vector2.UnitX * 20 * Main.rand.NextBool(2).BoolOne();
            int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackTwo>(), NPC.damage, 2);
            Main.projectile[proj].ai[2] = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
            Main.projectile[proj].ai[1] = -20;
            Main.projectile[proj].ai[0] = 2;
            Main.projectile[proj].owner = NPC.target;
            Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
            NPC.ai[2]++;
        }
        private void ShootBroadSword()
        {
            if (BossDelayAttack(0, 0, 0))
            {
                return;
            }
            for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++)
            {
                Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 160, i) * 20f;
                int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackOne>(), NPC.damage, 2);
                Main.projectile[proj].ai[2] = TerrariaArrayID.AllOreBroadSword[i];
                Main.projectile[proj].owner = NPC.target;
                if (Main.projectile[proj].ModProjectile is SwordBroadAttackOne swordProj)
                {
                    swordProj.OnSpawnDirection = vec.X > 0 ? 1 : -1;
                    swordProj.rememberThisPos = Main.player[NPC.target].Center + new Vector2(250 * swordProj.OnSpawnDirection, -80 + 40 * i);
                    ProjectileWhoAmI.Add(proj);
                }
            }
            NPC.ai[2]++;
            BossDelayAttack(0, 0, 0, 30);
        }
        private void ShootBroadSword2()
        {
            NPC.ai[0] = 0;
            if (BossDelayAttack(0, 0, 0))
            {
                return;
            }
            for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++)
            {
                Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 180, i) * 50f;
                vec.Y = 5;
                int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), NPC.damage, 2);
                Main.projectile[proj].ai[1] = 30;
                Main.projectile[proj].ai[2] = TerrariaArrayID.AllOreBroadSword[i];
                Main.projectile[proj].owner = NPC.target;
            }
            for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++)
            {
                Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 90, i) * 20f;
                int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), NPC.damage, 2);
                Main.projectile[proj].ai[1] = 40;
                Main.projectile[proj].ai[2] = TerrariaArrayID.AllOreBroadSword[i];
                Main.projectile[proj].owner = NPC.target;
            }
            NPC.ai[2]++;
            BossDelayAttack(0, 0, 0);

        }
        private bool BossDelayAttack(float delaytime, float nextattack, float whenAttackwillend, int additionalDelay = 0)
        {
            if (NPC.ai[0] <= 0)
            {
                NPC.ai[0] += delaytime;
            }
            else
            {
                NPC.ai[0]--;
                return true;
            }
            if (NPC.ai[2] > whenAttackwillend)
            {
                ResetEverything();
                NPC.ai[0] += additionalDelay;
                NPC.ai[1] = nextattack;
                return true;
            }
            return false;
        }
    }
    //This code did not follow the above rule and it should be change to follow the above rule
    public abstract class BaseHostileShortSword : ModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        protected int OnSpawnDirection = 0;
        public override void OnSpawn(IEntitySource source)
        {
            OnSpawnDirection = Projectile.velocity.X > 0 ? 1 : -1;
            base.OnSpawn(source);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Item[(int)Projectile.ai[2]].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
    class ShortSwordAttackOne : BaseHostileShortSword
    {
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 1)
            {
                if (Projectile.timeLeft > 120)
                    Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
                if (Projectile.timeLeft > 250)
                    Projectile.timeLeft = 250;
                return;
            }
            if (Projectile.velocity.IsLimitReached(3))
            {
                Projectile.velocity -= Projectile.velocity * .05f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                Projectile.ai[0] = 1;
            }
        }
    }
    class ShortSwordAttackTwo : BaseHostileShortSword
    {
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 LeftOfPlayer = new Vector2(player.Center.X + 400 * OnSpawnDirection, player.Center.Y);
            if (Projectile.ai[1] < 0)
            {
                Projectile.ai[1]++;
                return;
            }
            if (Projectile.ai[1] == 0)
            {
                if (!Projectile.Center.IsCloseToPosition(LeftOfPlayer, 10f))
                {
                    Vector2 distance = LeftOfPlayer - Projectile.Center;
                    float length = distance.Length();
                    if (length > 5)
                    {
                        length = 5;
                    }
                    Projectile.velocity -= Projectile.velocity * .08f;
                    Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
                    Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
                }
                else
                {
                    Projectile.velocity = -Vector2.UnitX * OnSpawnDirection;
                    Projectile.timeLeft = 150;
                    Projectile.ai[1] = 1;
                    Projectile.Center = LeftOfPlayer;
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] >= 60)
                {
                    if (Projectile.ai[1] >= 75)
                    {
                        Projectile.velocity -= Vector2.UnitX * 2 * OnSpawnDirection;
                        return;
                    }
                    Projectile.velocity += Vector2.UnitX * OnSpawnDirection;
                    return;
                }
            }
        }
    }
    public abstract class BaseHostileSwordBroad : ModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>((int)Projectile.ai[2])).Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
    class SwordBroadAttackOne : BaseHostileSwordBroad
    {
        public int OnSpawnDirection = 0;
        public bool CanProgressToAI3 = false;
        public Vector2 rememberThisPos = Vector2.Zero;
        public override void AI()
        {
            if (Projectile.ai[1] == 2)
            {
                if (!Projectile.Center.IsCloseToPosition(rememberThisPos, 20f))
                {
                    Vector2 distance = rememberThisPos - Projectile.Center;
                    float length = distance.Length();
                    if (length > 2)
                    {
                        length = 2;
                    }
                    Projectile.velocity -= Projectile.velocity * .08f;
                    Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
                    Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
                }
                else
                {
                    Projectile.Center = rememberThisPos;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.ai[1]++;
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
                return;
            }
            if (Projectile.ai[1] >= 3)
            {
                Vector2 newVel = Vector2.UnitX * OnSpawnDirection;
                if (CanProgressToAI3)
                {
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] >= 30)
                    {
                        Projectile.velocity -= newVel * 2;
                    }
                    else if (Projectile.ai[1] >= 20)
                    {
                        Projectile.velocity += newVel;
                    }
                    if (Projectile.timeLeft > 40)
                        Projectile.timeLeft = 120;
                }
                Projectile.rotation = newVel.ToRotation() + MathHelper.PiOver4 + MathHelper.Pi;
                return;
            }

            if (Projectile.velocity.IsLimitReached(7))
            {
                Projectile.velocity -= Projectile.velocity * .05f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                Projectile.ai[1] = 2;
                Projectile.velocity = Vector2.Zero;
            }
        }
    }
    class SwordBroadAttackTwo : BaseHostileSwordBroad
    {
        public override void AI()
        {
            Projectile.rotation = MathHelper.PiOver4 + MathHelper.PiOver2;
            if (Projectile.ai[1] == 1)
            {
                if (Projectile.timeLeft > 30)
                    Projectile.timeLeft = 30;
                Projectile.velocity.Y = 50;
                Projectile.velocity.X = 0;
                return;
            }
            if (Projectile.ai[1] > 1)
            {
                Projectile.velocity.Y += -.5f;
                Projectile.ai[1]--;
                Projectile.velocity -= Projectile.velocity * .1f;
            }
            else
            {
                Projectile.ai[1] = 1;
                Projectile.velocity = Vector2.Zero;
            }
        }
    }
}