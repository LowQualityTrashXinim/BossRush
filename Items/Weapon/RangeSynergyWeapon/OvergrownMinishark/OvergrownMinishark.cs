using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.OvergrownMinishark
{
    internal class OvergrownMinishark : WeaponTemplate
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Seafood with a touch of nature\nShoot out poisonous bullet");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 24;

            Item.damage = 24;
            Item.knockBack = 1f;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.rare = 2;

            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 15;
            Item.useAmmo = AmmoID.Bullet;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(gold: 50);
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.noMelee = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vec2ToRotate = velocity;
            Vector2 offset = velocity.SafeNormalize(Vector2.UnitX) * 40;
            if (Collision.CanHit(position, 0, 0, position * offset, 0, 0))
            {
                position += offset;
            }
            velocity = RotateRandom(7);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.Vilethorn)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
    class OvergrownMinisharkPlayer : ModPlayer
    {
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            Item item = new Item(ModContent.ItemType<OvergrownMinishark>());
            float randomRotation = Main.rand.Next(90);
            if (Player.HeldItem.type == ModContent.ItemType<OvergrownMinishark>() && !proj.minion && Main.rand.NextBool(10))
            {
                for (int i = 0; i < 6; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_ItemUse_WithPotentialAmmo(item, item.useAmmo), proj.Center, proj.velocity.RotatedBy(MathHelper.ToRadians(i * 60 + randomRotation)), ProjectileID.VilethornTip, damage, knockback, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_ItemUse_WithPotentialAmmo(item, item.useAmmo), proj.Center, proj.velocity.RotatedBy(MathHelper.ToRadians(i * 60 + randomRotation)), ProjectileID.VilethornBase, damage, knockback, Player.whoAmI);
                }
            }
        }
    }
}
