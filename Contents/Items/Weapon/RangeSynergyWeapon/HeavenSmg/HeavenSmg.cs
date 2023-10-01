using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Texture;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.Audio;
using log4net.Util;
using BossRush.Common;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HeavenSmg
{
    public class HeavenSmg : SynergyModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            BossRushUtils.BossRushDefaultRange(Item, 32, 32, 6, 1, 5, 30, ItemUseStyleID.Shoot, ProjectileID.Bullet, 45, true, AmmoID.Bullet);
            Item.reuseDelay = 30;
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            var HeavenSMGmodplayer = player.GetModPlayer<heavenSmgPlayer>();
            player.GetDamage(DamageClass.Generic) += HeavenSMGmodplayer.heavenSmgStacks * 0.05f;
            player.GetAttackSpeed(DamageClass.Generic) += HeavenSMGmodplayer.heavenSmgStacks * 0.01f;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                player.itemTime = player.itemAnimation;
                type = ModContent.ProjectileType<HeavenSmgThrow>();
                damage *= 3;
            }
            else
                SoundEngine.PlaySound(SoundID.Item36 with { Pitch = 1.5f }, player.Center);
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            if (player.velocity.Y > 0f && player.HasBuff<heavenSmgBuff>())
            {
                Projectile.NewProjectileDirect(source, position, velocity * 0.5f, ModContent.ProjectileType<heavenBolt>(), (int)(damage * 1.25f), 0, Main.myPlayer, 1);
            }
            CanShootItem = true;
        }
    }
    public struct HeavenTrail
    {
        private static VertexStrip _vertexStrip = new VertexStrip();
        public void Draw(Projectile proj)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
            miscShaderData.UseSaturation(-2f);
            miscShaderData.UseOpacity(2f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }
        private Color StripColors(float progressOnStrip)
        {
            Color result = new Color(255, 255, 255, 0);
            //result.A /= 2;
            return result;
        }
        private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5f, 32f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
    }
    public class HeavenSmgThrow : SynergyModProjectile
    {
        static bool returningToOwner = false;
        static bool targetHit = false;
        int oldposFrameAmount = 15;
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = oldposFrameAmount;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.idStaticNPCHitCooldown = 30;
            returningToOwner = false;
            Projectile.usesIDStaticNPCImmunity = true;
            currentSpeed = 0;
            targetHit = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item71 with { Pitch = 2f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item71 with { Pitch = 0.25f }, Projectile.Center);
        }
        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
            Color bigColor = shineColor * opacity * 0.5f;
            bigColor.A = 0;
            Vector2 origin = sparkleTexture.Size() / 2f;
            Color smallColor = drawColor * 0.5f;
            float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
            Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
            bigColor *= lerpValue;
            smallColor *= lerpValue;
            // Bright, large part
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
            // Dim, small part
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2f, 1f, SpriteEffects.None);
            if (targetHit)
            {
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, Projectile.Center - Main.screenPosition, new Color(255, 255, 255, 0), new Color(200, 255, 200, 0), currentSpeed / maxThrowSpeed, 0f, 0.1f, 0f, 1f, 0f, texture.Size() / 2f, Vector2.One);
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, Projectile.Center - Main.screenPosition, new Color(255, 255, 255, 125), new Color(200, 255, 200, 125), currentSpeed / maxThrowSpeed, 0f, 0.25f, 0f, 0.75f, MathHelper.PiOver4, texture.Size() / (maxThrowSpeed / currentSpeed), Vector2.One);
                if (Projectile.timeLeft < timeleftReset - oldposFrameAmount / 3)
                    default(HeavenTrail).Draw(Projectile);
            }
            return false;
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            if (!returningToOwner)
            {
                targetHit = true;
                player.AddBuff(ModContent.BuffType<heavenSmgBuff>(), 60 * 4);
            }
            returnToPlayer();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            resetStacks();
            returnToPlayer();
            return false;
        }
        private void resetStacks()
        {
            Player player = Main.player[Projectile.owner];
            var modplayer = player.GetModPlayer<heavenSmgPlayer>();
            modplayer.ModPlayer_resetStacks();
        }
        private void returnToPlayer()
        {
            if (!returningToOwner)
            {
                Projectile.velocity.Y -= 15f;
                returningToOwner = true;
                Projectile.timeLeft = timeleftReset;
            }
        }
        float currentSpeed = 0;
        float maxThrowSpeed = 50f;
        float accel = 0.8f;
        public int timeleftReset = 120;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (!returningToOwner && Projectile.timeLeft <= 2)
            {
                resetStacks();
                returnToPlayer();
            }
            player.itemAnimation = player.itemTime = 2;
            player.heldProj = Projectile.whoAmI;
            if (returningToOwner)
            {
                Vector2 vel = player.Center - Projectile.Center;
                vel.Normalize();
                currentSpeed += accel;
                if (currentSpeed > maxThrowSpeed)
                {
                    currentSpeed = maxThrowSpeed;
                }
                vel *= currentSpeed;
                vel.Y -= 1.25f * (maxThrowSpeed / currentSpeed);
                Projectile.velocity = vel;
                if (Projectile.Center.Distance(player.Center) <= 35)
                    Projectile.Kill();
            }
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            player.reuseDelay = 30;
            if (targetHit)
                for (int i = 1; i < oldposFrameAmount; i++)
                {
                    Vector2 oldVel = Projectile.oldPos[i].DirectionTo(Projectile.oldPos[i - 1]);
                    oldVel *= 50;
                    for (int j = 0; j < 10; j++)
                    {
                        int dustyDust = Dust.NewDust(Projectile.oldPos[i], (int)Projectile.Size.X / 2, (int)Projectile.Size.Y / 2, DustID.WhiteTorch, oldVel.X, oldVel.Y, (i * 20), default, 1);
                        Main.dust[dustyDust].noGravity = true;
                    }
                }
        }
    }
    internal class heavenSmgPlayer : ModPlayer
    {
        public bool heavenBuff;
        public override void ResetEffects()
        {
            heavenBuff = false;
        }
        public int heavenSmgStacks = 0;
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) => ModPlayer_resetStacks();
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) => ModPlayer_resetStacks();
        public void ModPlayer_IncreaseStack()
        {
            int MaxStacks = 40;
            heavenSmgStacks++;
            if (heavenSmgStacks == MaxStacks)
            {
                SoundEngine.PlaySound(SoundID.Item9 with { Pitch = -2f }, Player.Center);
            }
            else SoundEngine.PlaySound(SoundID.NPCHit5 with { Pitch = heavenSmgStacks * 0.075f }, Player.Center);
            heavenSmgStacks = (int)MathHelper.Clamp(heavenSmgStacks, 0, MaxStacks);
            for (int i = 0; i < 5; i++)
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Main.rand.NextVector2CircularEdge(1f, 1f) * 15, ModContent.ProjectileType<heavenBolt>(), 30, 0, Main.myPlayer, 1);
        }
        public void ModPlayer_resetStacks()
        {
            if (Player.HeldItem.type == ModContent.ItemType<HeavenSmg>())
                SoundEngine.PlaySound(SoundID.NPCDeath7, Player.Center);
            heavenSmgStacks = 0;
        }
    }
    internal class heavenSmgBuff : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void Update(Player player, ref int buffIndex)
        {
            player.slowFall = true;
            player.jumpSpeedBoost = 5;


            player.GetModPlayer<heavenSmgPlayer>().heavenBuff = true;
            if (player.buffTime[buffIndex] == 2)
            {
                player.GetModPlayer<heavenSmgPlayer>().ModPlayer_IncreaseStack();
            }
        }
    }
    internal class heavenBolt : SynergyModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        static int oldposFrameAmount = 25;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = oldposFrameAmount;
        }
        bool isMiniProjectile = false;
        bool isVanishingAnimation = false;
        int miniProjectileAmount = 3;
        int TheDamage;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 570;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            isMiniProjectile = false;
            projSpeed = 0f;
            canDealDamage = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] == 1)
                isMiniProjectile = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D MainTextureForProj = TextureAssets.Extra[98].Value;
            Color MainColor = new Color(200 + (MathF.Sin(Projectile.timeLeft) * 55), 255, 225, 0);
            Vector2 projectilePos = Projectile.Center - Main.screenPosition;
            Vector2 projectileOrigin = MainTextureForProj.Size() / 2f;
            Main.EntitySpriteDraw(MainTextureForProj, projectilePos, null, MainColor, Projectile.rotation, projectileOrigin, isMiniProjectile == true ? 1f : 1.5f, SpriteEffects.None);
            if (Projectile.ai[0] == 0)
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, projectilePos, MainColor, MainColor, 0.5f, 0f, 0.5f, 0.5f, 1f, 0f, Vector2.One * 2, Vector2.One);
            default(HeavenTrail).Draw(Projectile);
            return false;
        }
        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
            Color bigColor = shineColor * opacity * 0.5f;
            bigColor.A = 0;
            Vector2 origin = sparkleTexture.Size() / 2f;
            Color smallColor = drawColor * 0.5f;
            float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
            Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
            bigColor *= lerpValue;
            smallColor *= lerpValue;
            // Bright, large part
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
            // Dim, small part
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
        }
        public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item125 with { Pitch = 2f }, Projectile.Center);
            for (int i = 0; i < 35; i++)
            {
                var dust = Dust.NewDustPerfect(Projectile.Center, DustID.WhiteTorch, Main.rand.NextVector2CircularEdge(1f, 1f) * (isMiniProjectile == true ? 5f : 15f), default, new Color(255, 255, 255, 0), 2f);
                dust.noGravity = true;
            }
            if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0)
            {
                for (int i = 0; i < miniProjectileAmount; i++)
                {
                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.oldPosition, Main.rand.NextVector2CircularEdge(1f, 1f), ModContent.ProjectileType<heavenBolt>(), Projectile.damage / 3, 0, Main.myPlayer, 1);
                    proj.timeLeft = 600;
                }
            }
        }
        float maxProjSpeed = 15f;
        float projSpeed = 0f;
        bool canDealDamage = false;
        public override bool? CanHitNPC(NPC target) => canDealDamage;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            float maxDetectRadius = 2000f;
            Vector2 vel = Projectile.velocity;
            float accel = 2f;
            Projectile.Center.LookForHostileNPC(out NPC closestNPC, maxDetectRadius);
            if (closestNPC == null || (Projectile.timeLeft > 570 && Projectile.ai[0] == 1))
            {
                accel = 1f;
                canDealDamage = false;
            }
            else
            {
                vel = closestNPC.Center - Projectile.Center;
                canDealDamage = true;
            }
            vel.Normalize();
            projSpeed += accel;
            if (projSpeed > maxProjSpeed)
            {
                projSpeed = maxProjSpeed;
            }
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Projectile.Center + vel) * projSpeed, 0.025f);
        }
    }
}