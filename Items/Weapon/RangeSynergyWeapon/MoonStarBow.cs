using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IL.Terraria.GameContent.ObjectInteractions;
using System;

namespace BossRush.Items.Weapon.RangeSynergyWeapon
{
    internal class MoonStarBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;

            Item.damage = 40;
            Item.knockBack = 1f;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.noMelee = true;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<MoonStarProjectile>();
            Item.shootSpeed = 5f;
            Item.value = Item.buyPrice(gold: 50);

            Item.scale = .5f;
            Item.UseSound = SoundID.Item75;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position.Y += Main.rand.Next(-900, -800);
            velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
    class MoonStarProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.wet = false;
            Projectile.penetrate = -1;
            Projectile.light = 1f;
            Projectile.extraUpdates = 6;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3000;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 100;
        }
        int ExtraUpdaterReCounter = 0;
        float speedMultiplier = 1;
        int AlphaAdditionalCounter = 255;
        public override void AI()
        {
            ExtraUpdateRecounter();
            Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * speedMultiplier;
            Projectile.rotation += MathHelper.ToRadians(10);
        }

        public void ExtraUpdateRecounter()
        {
            ExtraUpdaterReCounter -= ExtraUpdaterReCounter > 0 ? 1 : 0;
            if (ExtraUpdaterReCounter == 0)
            {
                ExtraUpdaterReCounter = 6;
                speedMultiplier += .01f;
                Projectile.alpha++;
                AlphaAdditionalCounter -= AlphaAdditionalCounter > 0 ? -2 : 0;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<MoonStarProjectileTrail>()].Value;
            Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            Vector2 FullOrigin = origin * 2f;
            Vector2 threehalfOrigin = origin * .5f;
            Vector2 halfTexture = new Vector2(texture.Width, texture.Height) * .5f * .5f;
            for (int k = 1; k < Projectile.oldPos.Length + 1; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + (FullOrigin - threehalfOrigin + halfTexture) + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(new Color(0, 0, 255, Math.Abs(AlphaAdditionalCounter) / k));
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale - (k - 1) * 0.01f, SpriteEffects.None, 0);
            }
            return base.PreDraw(ref lightColor);
        }
    }
    class MoonStarProjectileTrail : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
        }
    }
}
