using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.OvergrownMinishark
{
    internal class OvergrownMinishark : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Seafood with a touch of nature\nShoot out poisonous bullet");
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(54, 24, 24, 2f, 11, 11, ItemUseStyleID.Shoot, ProjectileID.Bullet, 15, true, AmmoID.Bullet);

            Item.rare = 2;
            Item.value = Item.sellPrice(gold: 50);
            Item.UseSound = SoundID.Item11;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 offset = velocity.SafeNormalize(Vector2.UnitX) * 40;
            if (Collision.CanHit(position, 0, 0, position * offset, 0, 0))
            {
                position += offset;
            }
            velocity = velocity.RotateRandom(7);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.Vilethorn)
                .Register();
        }
    }
    class OvergrownMinisharkPlayer : ModPlayer
    {
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            Item item = new Item(ModContent.ItemType<OvergrownMinishark>());
            float randomRotation = Main.rand.Next(90);
            if (Player.HeldItem.type == ModContent.ItemType<OvergrownMinishark>() && !proj.minion && Main.rand.NextBool(10) && proj.type != ProjectileID.VilethornTip && proj.type != ProjectileID.VilethornBase)
            {
                for (int i = 0; i < 6; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_ItemUse_WithPotentialAmmo(item, item.useAmmo), proj.Center, proj.velocity.RotatedBy(MathHelper.ToRadians(i * 60 + randomRotation)), ProjectileID.VilethornTip, hit.Damage, hit.Knockback, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_ItemUse_WithPotentialAmmo(item, item.useAmmo), proj.Center, proj.velocity.RotatedBy(MathHelper.ToRadians(i * 60 + randomRotation)), ProjectileID.VilethornBase, hit.Damage, hit.Knockback, Player.whoAmI);
                }
            }
        }
    }
}
