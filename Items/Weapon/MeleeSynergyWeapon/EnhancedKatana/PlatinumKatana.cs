using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.EnhancedKatana
{
    internal class PlatinumKatana : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enhanced Katana");
            Tooltip.SetDefault("The best katana there is, yet");
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 52;

            Item.damage = 43;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.shoot = ModContent.ProjectileType<KatanaSlash>();
            Item.DamageType = DamageClass.Melee;
            Item.shootSpeed = 3;
            Item.rare = 1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
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
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.Katana)
                .AddRecipeGroup("OreBroadSword")
                .Register();
        }
    }
    public class KatanaSlash : ModProjectile
    {
        protected virtual float HoldoutRangeMax => 50f;
        protected virtual float HoldoutRangeMin => 10f;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 88;
            Projectile.penetrate = -1;
            Projectile.light = 0.1f;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.wet = false;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }
        public override void AI()
        {
            Projectile.ai[1]++;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            int duration = player.itemAnimationMax;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 8;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
            SpriteEffects spriteEffects = Projectile.ai[0] % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}