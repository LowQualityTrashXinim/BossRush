using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.SuperShortSword
{
    internal class SuperShortSwordOrbitShortSword : ModProjectile
    {
        public override string Texture => getTexture();
        private string getTexture()
        {
            string body = "BossRush/Items/Weapon/MeleeSynergyWeapon/SuperShortSword/";
            switch (Projectile.ai[0])
            {
                case 0:
                    return body + "SpeCopper";
                case 1:
                    return body + "SpeTin";
                case 2:
                    return body + "SpeIron";
                case 3:
                    return body + "SpeLead";
                case 4:
                    return body + "SpeSilver";
                case 5:
                    return body + "SpeTungsten";
                case 6:
                    return body + "SpeGold";
                case 7:
                    return body + "SpePlatinum";
            }
            return "BossRush/MissingTexture";
        }
        public override void SetDefaults()
        {
            Projectile.height = 32;
            Projectile.width = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.light = 0.7f;
        }
        Player player => Main.player[Projectile.owner];
        Vector2 RotatePosition;
        Vector2 FixedMousePosition;
        int Counter = 0;
        int timer = 999;
        public override bool PreAI()
        {
            if(player.ItemAnimationJustStarted)
            {
                FixedMousePosition = Main.MouseWorld;
            }
            if (player.ItemAnimationActive)
            {
                Vector2 ToMouse = (FixedMousePosition - Projectile.Center).SafeNormalize(Vector2.UnitX);
                float duration = player.itemAnimationMax;
                float halfProgress = duration * .5f;
                float progress;
                if (timer > duration)
                {
                    timer = (int)duration;
                }
                if (timer < halfProgress)
                {
                    progress = timer / halfProgress;
                }
                else
                {
                    progress = (duration - timer) / halfProgress;
                }
                Projectile.Center = RotatePosition + Vector2.SmoothStep(ToMouse * 1f, ToMouse * 200f, progress);
                timer--;
            }
            if(timer == 0)
            {
                timer = 999;
            }
            RotatePosition = getPosToReturn(player, 45 * Projectile.ai[0], Counter);
            return !player.ItemAnimationActive;
        }
        public override void AI()
        {
            Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * 0.25f * player.GetTotalDamage(DamageClass.Melee).Additive);
            Projectile.CritChance = (int)(player.GetCritChance(DamageClass.Melee) + player.GetCritChance(DamageClass.Generic));
            if (player.dead || !player.active || !player.HasBuff(ModContent.BuffType<SuperShortSwordPower>()))
            {
                Projectile.Kill();
            }
            if (Counter == MathHelper.TwoPi * 100 || Counter == -MathHelper.TwoPi * 100) { Counter = 0; }
            if (player.direction == 1)
            {
                Counter++;
            }
            else
            {
                Counter--;
            }
            RotatePosition = getPosToReturn(player, 45 * Projectile.ai[0], Counter);
            Projectile.Center = RotatePosition;
        }
        public Vector2 getPosToReturn(Player player, float offSet, int Counter, float Distance = 50)
        {
            Vector2 SafeDegree = (Main.MouseWorld - Projectile.position).SafeNormalize(Vector2.UnitX);
            Projectile.rotation = SafeDegree.ToRotation() + MathHelper.PiOver4;
            Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
            return player.Center + Rotate.RotatedBy(Counter * 0.05f) * Distance;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
        }
    }
}