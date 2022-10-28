using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Weapon.BasicWeapon
{
    internal class BlueMinishark : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("what ? you think spraying it blue will make it better ? i mean in this case it is but still\nHave 38% chance to not consume ammo");
        }
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 20;

            Item.damage = 15;
            Item.knockBack = 2f;
            Item.shootSpeed = 10;
            Item.useTime = 8;
            Item.useAnimation = 8;

            Item.noMelee = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = 3;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.38f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 offset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
            if (Collision.CanHit(position, 0, 0, offset * offset, 0, 0))
            {
                position += offset;
            }
            int Chance = Main.rand.Next(10);
            int amount = 2;
            if (Chance <= 3)
            {
                amount++;
            }
            for (int i = 0; i < amount; i++)
            {
                Vector2 velRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(3));
                Projectile.NewProjectile(source, position, velRan, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, -2f);
        }
    }
}
