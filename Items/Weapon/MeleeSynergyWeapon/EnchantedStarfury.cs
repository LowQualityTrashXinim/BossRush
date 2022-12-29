using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class EnchantedStarfury : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A truely unthinkable");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;

            Item.damage = 32;
            Item.knockBack = 4f;
            Item.useTime = 80;
            Item.useAnimation = 20;
            Item.rare = 4;
            Item.shoot = ProjectileID.EnchantedBeam;
            Item.shootSpeed = 20f;

            Item.width = 66;
            Item.height = 66;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }
        int switchProj = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            BossRushWeaponSpreadUtils.NumOfProjectile = 5;
            for (int i = 0; i < BossRushWeaponSpreadUtils.NumOfProjectile; i++)
            {
                Vector2 rotate = velocity.RotateCode(player.direction == 1 ? -20 : 20, i);

                Projectile.NewProjectile(source, position, rotate,
                    switchProj % 2 == 0 ? ModContent.ProjectileType<EnchantedSwordProjectile>() : ModContent.ProjectileType<StarProjectile>(),
                    damage, knockback, player.whoAmI, i);
            }
            switchProj++;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.Starfury)
                .Register();
        }
    }
    class EnchantedSwordProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.penetrate = 1;
            Projectile.scale = .65f;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        int timer = 0;
        Vector2 localOriginalvelocity;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (timer == 0)
            {
                localOriginalvelocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            }
            if (timer <= 20 + Projectile.ai[0] * 2)
            {
                Projectile.timeLeft = 200;
                Projectile.velocity -= Projectile.velocity * .1f;
                timer++;
            }
            else
            {
                if (!Projectile.velocity.reachedLimited(20)) Projectile.velocity += localOriginalvelocity;
            }
        }
    }

    class StarProjectile : ModProjectile
    {
        public override string Texture => "BossRush/VanillaSprite/WhiteStar";
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        int timer = 0;
        Vector2 localOriginalvelocity;
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(10);
            if (timer == 0)
            {
                localOriginalvelocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            }
            if (timer <= 20 + Projectile.ai[0] * 2)
            {
                Projectile.velocity -= Projectile.velocity * .1f;
                timer++;
                Projectile.timeLeft = 120;
            }
            else
            {
                if (!Projectile.velocity.reachedLimited(20)) Projectile.velocity += localOriginalvelocity;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 0, 255, 200);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
    }
}
