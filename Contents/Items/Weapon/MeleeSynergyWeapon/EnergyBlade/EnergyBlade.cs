using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnergyBlade
{
    internal class EnergyBlade : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 62;

            Item.damage = 27;
            Item.knockBack = 0f;
            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.shoot = ModContent.ProjectileType<EnergyBladeProjectile>();
            Item.shootSpeed = 0;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Orange;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;

            Item.UseSound = SoundID.Item1;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.Terragrim)
                .Register();
        }
    }
    public class EnergyBladeProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<EnergyBlade>();
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 62;
            Projectile.penetrate = -1;
            Projectile.wet = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
        }
        Vector2 data;
        int FirstFrameOfAi = 0;
        public override void AI()
        {
            frameCounter();
            BossRushUtils.ProjectileSwordSwingAI(Projectile,ref data, ref FirstFrameOfAi, 1);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, Projectile);
        }
        public void frameCounter()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                Projectile.frame += 1;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
