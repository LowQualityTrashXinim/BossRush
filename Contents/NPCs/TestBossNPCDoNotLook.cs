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

namespace BossRush.Contents.NPCs
{
    //To ensure the code is readable and consistent throughout the process of making
    //ai[0] => timer
    //ai[1] => ai switch
    //ai[2] => counter ( not to be confused with timer )
    //The code in this file must follow the above rule
    internal class ChestLord : ModNPC
    {
        public override string Texture => "BossRush/Contents/Items/Chest/WoodenTreasureChest";
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
            if (NPC.ai[1] < 4 && NPC.ai[0] <= 0)
            {
                NPC.ai[1]++;
            }
            else
            {
                NPC.ai[1] = 0;
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
            }
        }
        private void ResetEverything()
        {
            NPC.ai[0] = 90;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
            NPC.ai[3] = 0;
        }
        private void Move(Player player)
        {
            Vector2 positionAbovePlayer = new Vector2(player.Center.X, player.Center.Y - 350);
            if (NPC.NPCMoveToPosition(positionAbovePlayer, 30f))
            {
                NPC.ai[0] = 90;
            }
        }
        private void ShootShortSword()
        {
            if (BossDelayAttack(10, 0, TerrariaArrayID.AllOreShortSword.Length - 1))
            {
                return;
            }
            Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)NPC.ai[2]) * 10f;
            int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordProjectile>(), NPC.damage, 2);
            Main.projectile[proj].ai[2] = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
            Main.projectile[proj].owner = NPC.target;
            NPC.ai[2]++;
        }
        private void ShootShortSword2()
        {
            if (BossDelayAttack(20, 0, TerrariaArrayID.AllOreShortSword.Length - 1))
            {
                return;
            }
            Vector2 vec = Vector2.UnitX * 20 * Main.rand.NextBool(2).BoolOne();
            int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordProjectile>(), NPC.damage, 2);
            Main.projectile[proj].ai[2] = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
            Main.projectile[proj].ai[1] = -20;
            Main.projectile[proj].ai[0] = 2;
            Main.projectile[proj].owner = NPC.target;
            Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
            NPC.ai[2]++;
        }
        private void ShootBroadSword()
        {
            if (BossDelayAttack(0, 0, 0, 360))
            {
                return;
            }
            for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++)
            {
                Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenly(TerrariaArrayID.AllOreBroadSword.Length, 160, i) * 5f;
                int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadProjectile>(), NPC.damage, 2);
                Main.projectile[proj].ai[2] = TerrariaArrayID.AllOreBroadSword[i];
                Main.projectile[proj].ai[0] = -20;
                Main.projectile[proj].owner = NPC.target;
                Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
            }
            NPC.ai[2]++;
            BossDelayAttack(0, 0, 0, 360);
        }
        private bool BossDelayAttack(float delaytime, float nextattack, float whenAttackwillend, int additionalDelay = 0)
        {
            if (NPC.ai[0] <= 0)
            {
                NPC.ai[0] = delaytime;
            }
            else
            {
                NPC.ai[0]--;
                return true;
            }
            if (NPC.ai[2] > whenAttackwillend)
            {
                ResetEverything();
                NPC.ai[0] = additionalDelay;
                NPC.ai[1] = nextattack;
                return true;
            }
            return false;
        }
    }
    //This code did not follow the above rule and it should be change to follow the above rule
    class ShortSwordProjectile : ModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        int OnSpawnDirection = 0;
        public override void OnSpawn(IEntitySource source)
        {
            OnSpawnDirection = Projectile.velocity.X > 0 ? 1 : -1;
            base.OnSpawn(source);
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 1)
            {
                if (Projectile.ai[1] >= 30)
                {
                    Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 25f + player.velocity;
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
                    Projectile.ai[1] = -1;
                    Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
                }
                else
                {
                    if (Projectile.ai[1] != -1)
                    {
                        Projectile.timeLeft = 150;
                        Projectile.ai[1]++;
                    }
                }
                return;
            }
            if (Projectile.ai[0] == 2)
            {
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
                return;
            }
            if (Projectile.velocity.IsLimitReached(1))
            {
                Projectile.velocity -= Projectile.velocity * .05f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[0] = 1;
            }
            base.AI();
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
    class SwordBroadProjectile : ModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        int OnSpawnDirection = 0;
        public override void OnSpawn(IEntitySource source)
        {
            OnSpawnDirection = Projectile.velocity.X > 0 ? 1 : -1;
            base.OnSpawn(source);
        }
        Vector2 rememberThisPos = Vector2.Zero;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[1] == 1)
            {
                if (Projectile.rotation != -MathHelper.PiOver4)
                {
                    float rotation = MathHelper.ToRadians(1);
                    float total;
                    if (Projectile.rotation > -MathHelper.PiOver4)
                    {
                        total = Projectile.rotation - rotation;
                        if (Math.Abs(total) <= MathHelper.PiOver4)
                        {
                            Projectile.rotation = -MathHelper.PiOver4;
                            return;
                        }
                        Projectile.rotation = total;
                    }
                    else
                    {
                        total = Projectile.rotation + rotation;
                        if (Math.Abs(total) >= MathHelper.PiOver4)
                        {
                            Projectile.rotation = -MathHelper.PiOver4;
                            return;
                        }
                        Projectile.rotation = total;
                    }
                    return;
                }
                Projectile.ai[1] = 2;
            }
            if (Projectile.ai[1] == 2)
            {
                Vector2 BesideThePlayer = player.Center + new Vector2(100 * OnSpawnDirection, 0).RotatedBy(MathHelper.ToRadians(-120) * OnSpawnDirection);
                if(Projectile.ProjectileMoveToPosition(BesideThePlayer, 10f))
                {
                    rememberThisPos = BesideThePlayer;
                    Projectile.ai[1] = 3;
                }
            }
            if (Projectile.ai[1] == 3)
            {
                if(Projectile.timeLeft > 30)
                {
                    Projectile.timeLeft = 30;
                }
                //Projectile.velocity = Vector2.One.RotatedBy(MathHelper.Lerp(rememberThisPos.ToRotation(), rememberThisPos.ToRotation() + MathHelper.ToRadians(120) * OnSpawnDirection, (30 - Projectile.timeLeft) / 30f));
                return;
            }
            if (Projectile.velocity.IsLimitReached(1))
            {
                Projectile.velocity -= Projectile.velocity * .05f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            else
            {
                Projectile.ai[1] = 1;
                Projectile.velocity = Vector2.Zero;
            }
            base.AI();
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
}