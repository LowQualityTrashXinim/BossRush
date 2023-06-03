using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnhancedKatana
{
    internal class PlatinumKatana : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(50, 52, 43, 4, 20, 20, ItemUseStyleID.Swing, true);
            Item.BossRushSetDefaultSpear(ModContent.ProjectileType<KatanaSlash>(), 3);
            Item.rare = 1;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item1;
        }
        int count = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, count);
            count++;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Katana)
                .AddRecipeGroup("OreBroadSword")
                .Register();
        }
    }
    public class KatanaSlash : ModProjectile
    {
        protected virtual float HoldoutRangeMax => 50f;
        protected virtual float HoldoutRangeMin => 10f;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 40; Projectile.height = 54;
            Projectile.penetrate = -1;
            Projectile.light = 0.1f;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.wet = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 Direction = (Projectile.rotation + MathHelper.ToRadians(90)).ToRotationVector2() * 60;
            Vector2 Head = Projectile.Center + Direction;
            Vector2 End = Projectile.Center - Direction;
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Head, End, Projectile.width, ref point);
        }
        public override void AI()
        {
            SelectFrame();
            Projectile.ai[1]++;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            int duration = player.itemAnimationMax;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[0] % 2 == 0)
            {
                if (Projectile.timeLeft == duration)
                {
                    Projectile.spriteDirection = -Projectile.spriteDirection;
                }
                Projectile.rotation += MathHelper.ToRadians(180);
            }
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            float halfDuration = duration * .5f;
            float progress = (duration - Projectile.timeLeft) / halfDuration;
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            if (Projectile.ai[1] >= 5)
            {
                Projectile.scale -= 0.03f;
                Projectile.alpha += 20;
            }
        }
        public void SelectFrame()
        {
            if (++Projectile.frameCounter >= 1)
            {
                Projectile.frameCounter = 0;
                Projectile.frame += 1;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = Main.projFrames[Projectile.type] - 1;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
        //public override bool PreDraw(ref Color lightColor)
        //{
        //    Main.instance.LoadProjectile(Projectile.type);
        //    Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
        //    Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
        //    Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
        //    SpriteEffects spriteEffects = Projectile.ai[0] % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
        //    Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
        //    return false;
        //}
    }
}