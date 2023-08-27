using System;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.PlasmaBlaster
{
    internal class PlasmaBlaster : ModItem
    {
        public override void SetDefaults()
        {
            BossRushUtils.BossRushSetDefault(Item, 42, 24, 60, 4f, 30, 30, ItemUseStyleID.Shoot, false);
            BossRushUtils.BossRushDefaultMagic(Item, ModContent.ProjectileType<PlasmaBlasterLaserProjectile>(), 1f, 3);
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 50);
            Item.channel = true;
        }
    }

    class PlasmaBlasterLaserProjectile : ModProjectile
    {
        private const float MAX_CHARGE = 50f;
        private const float MOVE_DISTANCE = 60f;
        public float Distance
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public float Charge
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public bool IsAtMaxCharge => Charge == MAX_CHARGE;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hide = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
         
            if (IsAtMaxCharge)
            {
                DrawLaser(texture, Main.player[Projectile.owner].Center, Projectile.velocity);
            }
            return false;
        }
        public void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit)
        {

            Vector2 centerFloored = start + Projectile.velocity * 48;
            Vector2 drawScale = new Vector2(Projectile.scale);
            DelegateMethods.f_1 = 1f; 
            Vector2 startPosition = centerFloored - Main.screenPosition;
            Vector2 endPosition = (startPosition + unit * Distance);
            DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, Color.White * Projectile.Opacity);

        }

        private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
        {
            Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);
            DelegateMethods.c_1 = beamColor;
            Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!IsAtMaxCharge) return false;
            Player player = Main.player[Projectile.owner];
            Vector2 unit = Projectile.velocity;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center, player.Center + unit * Distance, 22, ref point);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 2;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = player.Center + Projectile.velocity * MOVE_DISTANCE;
            Projectile.timeLeft = 2;

            UpdatePlayer(player);
            ChargeLaser(player);

            if (Charge < MAX_CHARGE) return;

            SetLaserPosition(player);
            SpawnDusts(player);
            CastLights();
        }

        private void SpawnDusts(Player player)
        {
            Vector2 unit = Projectile.velocity * -1;
            Vector2 dustPos = player.Center + Projectile.velocity * Distance;
            for (int i = 0; i < 2; ++i)
            {
                float num1 = Projectile.velocity.ToRotation() + (Main.rand.NextBool(2) ? -1f : 1f) * 1.57f;
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, DustID.Electric, dustVel.X, dustVel.Y)];
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust = Dust.NewDustDirect(Main.player[Projectile.owner].Center, 0, 0, DustID.Smoke, -unit.X * Distance, -unit.Y * Distance);
                dust.fadeIn = 0f;
                dust.noGravity = true;
                dust.scale = 0.88f;
                dust.color = Color.Cyan;
            }
            if (Main.rand.NextBool(5))
            {
                Vector2 offset = Projectile.velocity.RotatedBy(1.57f) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
                Dust dust = Main.dust[Dust.NewDust(dustPos + offset - Vector2.One * 4f, 8, 8, DustID.Smoke, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity *= 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                unit = dustPos - Main.player[Projectile.owner].Center;
                unit.Normalize();
                dust = Main.dust[Dust.NewDust(Main.player[Projectile.owner].Center + 55 * unit, 8, 8, DustID.Smoke, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity = dust.velocity * 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
            }
        }
        private void SetLaserPosition(Player player)
        {
            for (Distance = MOVE_DISTANCE; Distance <= 2200f; Distance += 5f)
            {
                var start = Projectile.Center + Projectile.velocity * (Distance - 64);
                if (!Collision.CanHitLine(Projectile.Center + Projectile.velocity, 1, 1, start, 1, 1))
                {
                    Distance -= 5f;
                    break;
                }
            }
        }
        private void ChargeLaser(Player player)
        {
            if (!player.channel)
            {
                
                Projectile.Kill();
                return;
            }
            if(Main.rand.NextBool(4))
                if (!player.CheckMana(player.GetManaCost(player.HeldItem), true))
                    Projectile.Kill();

            Vector2 offset = Projectile.velocity * (MOVE_DISTANCE - 20);
            Vector2 pos = player.Center + offset - new Vector2(10, 10);
            if (Charge < MAX_CHARGE)
            {
                Charge++;
            }
            int chargeFact = (int)(Charge / 20f);
            Vector2 dustVelocity = (Vector2.UnitX * 18f).RotatedBy(Projectile.rotation - 1.57f);
            Vector2 spawnPos = Projectile.Center + dustVelocity;
            for (int k = 0; k < chargeFact + 1; k++)
            {
                Vector2 spawn = spawnPos + ((float)Main.rand.NextDouble() * 6.28f).ToRotationVector2() * (12f - chargeFact * 2);
                Dust dust = Main.dust[Dust.NewDust(pos, 20, 20, DustID.Electric, Projectile.velocity.X * .5f, Projectile.velocity.Y * .5f)];
                dust.velocity = Vector2.Normalize(spawnPos - spawn) * 1.5f * (10f - chargeFact * 2f) / 10f;
                dust.noGravity = true;
                dust.scale = Main.rand.Next(10, 20) * 0.05f;
            }

        }
        private void UpdatePlayer(Player player)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 diff = Main.MouseWorld - player.Center;
                diff.Normalize();
                Projectile.velocity = diff;
                Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                Projectile.netUpdate = true;
            }
            int dir = Projectile.direction;
            player.ChangeDir(dir);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
        }
        private void CastLights()
        {
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - MOVE_DISTANCE), 26, DelegateMethods.CastLight);
        }
        public override bool ShouldUpdatePosition() => false;
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
    }
}
