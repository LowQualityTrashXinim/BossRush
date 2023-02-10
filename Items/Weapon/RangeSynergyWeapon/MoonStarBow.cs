using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BossRush.Items.Weapon.RangeSynergyWeapon
{
    internal class MoonStarBow : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Make your wish !");
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
            position.X += Main.rand.Next(-100, 100);
            velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MoonCharm)
                .AddIngredient(ItemID.DaedalusStormbow)
                .AddIngredient(ItemID.PulseBow)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
    class MoonStarProjectile : ModProjectile
    {
        int originDamage;
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 100;
        }
        int ExtraUpdaterReCounter = 0;
        float speedMultiplier = 1;
        int AlphaAdditionalCounter = 255;
        int counter = 0;
        public override bool PreAI()
        {
            float multiscale = Projectile.ai[1] == 1 ? .5f : 1;
            Projectile.scale = multiscale;
            return base.PreAI();
        }
        public override void AI()
        {
            ExtraUpdateRecounter();
            if (Projectile.ai[1] == 1)
            {
                AttackHomeIn();
                return;
            }
            Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * speedMultiplier;
            Projectile.rotation += MathHelper.ToRadians(.5f);
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] == 1)
            {
                if (counter >= 15)
                {
                    return true;
                }
                return false;
            }
            return base.CanHitNPC(target);
        }
        private void ExtraUpdateRecounter()
        {
            ExtraUpdaterReCounter -= ExtraUpdaterReCounter > 0 ? 1 : 0;
            if (ExtraUpdaterReCounter == 0)
            {
                counter++;
                if (Projectile.ai[1] == 1)
                {
                    Projectile.penetrate = 1;
                }
                if (Projectile.ai[1] == 0)
                {
                    Projectile.damage += (int)(Math.Abs(Projectile.velocity.X + Projectile.velocity.Y) * .35f);
                    Projectile.alpha++;
                    AlphaAdditionalCounter -= AlphaAdditionalCounter > 0 ? -2 : 0;
                    if (Projectile.alpha >= 255) Projectile.Kill();
                }
                ExtraUpdaterReCounter = 6;
                speedMultiplier += .01f;
            }
        }
        bool alreadygotBelow = false;
        private void AttackHomeIn()
        {
            if (counter < 15)
            {
                if (Math.Abs(Projectile.velocity.X) > .1f && Math.Abs(Projectile.velocity.Y) > .1f)
                {
                    Projectile.velocity -= Projectile.velocity * .005f;
                    speedMultiplier = 0;
                }
                return;
            }
            if (Math.Abs(Projectile.velocity.X) > .1f && Math.Abs(Projectile.velocity.Y) > .1f && !alreadygotBelow)
            {
                Projectile.velocity -= Projectile.velocity * .005f;
                speedMultiplier = 0;
            }
            else
            {
                NPC npc = Main.npc[(int)Projectile.ai[0]];
                if (!npc.active)
                {
                    return;
                }
                Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * speedMultiplier;
                alreadygotBelow = true;
            }
        }
        bool hitEnemyAlready = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[1] == 0 && !hitEnemyAlready)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Main.rand.NextVector2CircularEdge(3f, 3f),
                    ModContent.ProjectileType<MoonStarProjectile>(),
                    20, 1f,
                    Projectile.owner, target.whoAmI, 1);
                hitEnemyAlready = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<MoonStarProjectileTrail>()].Value;
            if (Projectile.ai[1] == 0)
            {
                Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
                Vector2 FullOrigin = origin * 2f;
                Vector2 threehalfOrigin = origin * .5f;
                Vector2 halfTexture = new Vector2(texture.Width, texture.Height) * .5f * .5f;
                for (int k = 1; k < Projectile.oldPos.Length + 1; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + (FullOrigin - threehalfOrigin + halfTexture) + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(new Color(0, 0, 255, Math.Abs(AlphaAdditionalCounter) / k));
                    Main.EntitySpriteDraw(texture, drawPos, null, color, 0, origin, Projectile.scale - (k - 1) * .01f, SpriteEffects.None, 0);
                }
                for (int k = 1; k < (int)(Projectile.oldPos.Length * .5f) + 1; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + (FullOrigin - threehalfOrigin - halfTexture) + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(new Color(255, 255, 255, Math.Abs(AlphaAdditionalCounter) / k));
                    Main.EntitySpriteDraw(texture, drawPos, null, color, 0, origin, (Projectile.scale - (k - 1) * .02f) * .5f, SpriteEffects.None, 0);
                }
                Texture2D thisProjectiletexture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 thisProjectileorigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
                Vector2 thisProjectiledrawPos = Projectile.position - Main.screenPosition + thisProjectileorigin + new Vector2(0f, Projectile.gfxOffY);
                Color thisProjectilecolor = Projectile.GetAlpha(lightColor);
                Main.EntitySpriteDraw(thisProjectiletexture, thisProjectiledrawPos, null, thisProjectilecolor, -Projectile.rotation, thisProjectileorigin, Projectile.scale, SpriteEffects.None, 0);

                Color largerProjectilecolor = Projectile.GetAlpha(new Color(255, 255, 255, 20));
                Main.EntitySpriteDraw(thisProjectiletexture, thisProjectiledrawPos, null, largerProjectilecolor, -Projectile.rotation, thisProjectileorigin, Projectile.scale * 2, SpriteEffects.None, 0);
            }
            else
            {
                Vector2 origin = new Vector2(Projectile.width * .5f, Projectile.height * .5f);
                Vector2 offsetOriginbyQuad = origin * .33f;
                for (int k = 1; k < Projectile.oldPos.Length + 1; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin + offsetOriginbyQuad + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(new Color(0, 0, 255, Math.Abs(AlphaAdditionalCounter) / k));
                    Main.EntitySpriteDraw(texture, drawPos, null, color, 0, origin, Projectile.scale - (k - 1) * .5f * .01f, SpriteEffects.None, 0);
                }
            }
            return base.PreDraw(ref lightColor);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 80; i++)
            {
                Vector2 vec = Main.rand.NextVector2Unit(MathHelper.PiOver4, MathHelper.PiOver2) * Main.rand.NextFloat(3, 5) * -1f;
                int dust = Dust.NewDust(Projectile.Center, 0, 0, 226, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.1f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = vec.RotatedBy(MathHelper.ToRadians(90 * (i % 4)));
            }
            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, 229, 0, 0, 0, default, Main.rand.NextFloat(1.35f, 1.5f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(7f, 7f);
            }
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
