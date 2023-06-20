using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnergyBlade
{
    internal class EnergyBlade : SynergyModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultMeleeCustomProjectile(64, 62, 21, 0, 30, 30, ItemUseStyleID.Swing, ModContent.ProjectileType<EnergyBladeProjectile>(), true);
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;
            Item.UseSound = SoundID.Item1;
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if(player.HasItem(ItemID.Code1))
            {
                modplayer.EnergyBlade_Code1 = true;
            }
            if(player.HasItem(ItemID.Code2))
            {
                modplayer.EnergyBlade_Code2 = true;
            }
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1;
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            if(modplayer.EnergyBlade_Code1)
            {
                int proj = Projectile.NewProjectile(source, position, -velocity, type, damage, knockback, player.whoAmI, 1);
                Main.projectile[proj].Size *= .5f;
            }
            CanShootItem = player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1;
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
        Player player;
        public override void OnSpawn(IEntitySource source)
        {
            player = Main.player[Projectile.owner];
            data = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.Center = player.Center;

                return;
            }
            frameCounter();
            BossRushUtils.ProjectileSwordSwingAI(Projectile, player, data);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, Main.player[Projectile.owner], Projectile.width, Projectile.height);
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