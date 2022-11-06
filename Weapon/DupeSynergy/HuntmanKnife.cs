using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.DupeSynergy
{
    internal class HuntmanKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Perfect for hunting silently");
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.knockBack = 2f;

            Item.width = 34;
            Item.height = 76;
            
            Item.useAnimation = 15;
            Item.useTime = 15;

            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.shoot = ModContent.ProjectileType<HuntmanKnifeP>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(Main.rand.NextBool(4))
            {
                Projectile.NewProjectile(source, position, velocity*0.75f, ModContent.ProjectileType<AdditionalKnife>(), damage, knockback, player.whoAmI);
            }
            return true;
        }
    }

    internal class AdditionalKnife : ModProjectile
    {
        public override string Texture => "BossRush/Weapon/MeleeSynergyWeapon/EnchantedOreSword/EnchantedCopperSwordP";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 150;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;   
        }
    }
}