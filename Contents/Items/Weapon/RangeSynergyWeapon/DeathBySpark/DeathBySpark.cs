using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.DeathBySpark
{
    internal class DeathBySpark : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(34, 24, 15, 1f, 84, 84, ItemUseStyleID.Shoot, ModContent.ProjectileType<SparkFlare>(), 12, false, AmmoID.Flare);
            Item.rare = 3;
            Item.UseSound = SoundID.Item11;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            base.ModifySynergyToolTips(ref tooltips, modplayer);
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.HoldSynergyItem(player, modplayer);
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
        }
        public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SparkFlare>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FlareGun)
                .AddIngredient(ItemID.WandofSparking)
                .Register();
        }
    }
    internal class SparkFlare : SynergyModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.light = 2f;
        }
        bool hittile = false;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Projectile.ai[0]++;
            if (!hittile)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (Projectile.ai[0] >= 40)
                {
                    if (Projectile.velocity.Y < 16) Projectile.velocity.Y += 0.1f;
                }
            }
            Vector2 OppositeVelocity = Projectile.rotation.ToRotationVector2() * -4.5f;
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, OppositeVelocity + Main.rand.NextVector2Circular(1f, 1f), ProjectileID.WandOfSparkingSpark, (int)(Projectile.damage * 0.65f), Projectile.owner, player.whoAmI);
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 20;
            Main.projectile[proj].timeLeft = 12;

        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            npc.AddBuff(BuffID.OnFire, 300);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hittile)
            {
                Projectile.position += Projectile.velocity;
            }
            hittile = true;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}