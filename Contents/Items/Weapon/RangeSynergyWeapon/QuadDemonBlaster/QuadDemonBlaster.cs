using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.QuadDemonBlaster
{
    class QuadDemonBlaster : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(40, 30, 39, 3f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.Bullet, 15, true, AmmoID.Bullet);

            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Orange;
            Item.reuseDelay = 15;
            Item.UseSound = SoundID.Item41;
        }
        public override void HoldItem(Player player)
        {
            player.GetModPlayer<QuadDemonBlasterPlayer>().SpeedMultiplier -= player.GetModPlayer<QuadDemonBlasterPlayer>().SpeedMultiplier == 1 ? 0 : .25f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            float ProjNum = 10;
            float Rotation = MathHelper.ToRadians(player.GetModPlayer<QuadDemonBlasterPlayer>().SpeedMultiplier);
            for (int i = 0; i < ProjNum; i++)
            {
                Vector2 Rotate = velocity.RotatedBy(MathHelper.Lerp(Rotation, -Rotation, i / (ProjNum - 1)));
                float RandomSpeadx = Main.rand.NextFloat(0.5f, 1f);
                float RandomSpeady = Main.rand.NextFloat(0.5f, 1f);
                Projectile.NewProjectile(source, position.X, position.Y,
                    Rotate.X * (player.GetModPlayer<QuadDemonBlasterPlayer>().SpeedMultiplier == 1 ? 1 : RandomSpeadx),
                    Rotate.Y * (player.GetModPlayer<QuadDemonBlasterPlayer>().SpeedMultiplier == 1 ? 1 : RandomSpeady),
                    type, damage, knockback, player.whoAmI);
            }
            player.GetModPlayer<QuadDemonBlasterPlayer>().SpeedMultiplier += player.GetModPlayer<QuadDemonBlasterPlayer>().SpeedMultiplier < 45 ? 10 : 1;
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.QuadBarrelShotgun)
                .AddIngredient(ItemID.PhoenixBlaster)
                .Register();
        }
    }
    public class QuadDemonBlasterPlayer : ModPlayer
    {
        public float SpeedMultiplier = 1;
    }
}
