using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Deagle
{
    internal class Deagle : SynergyModItem, IRogueLikeRangeGun
    {
        float IRogueLikeRangeGun.OffSetPosition => 50;

        public float Spread { get; set; }

        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(56, 30, 70, 5f, 21, 21, ItemUseStyleID.Shoot, ProjectileID.Bullet, 40, false, AmmoID.Bullet);
            Item.rare = 3;
            Item.value = Item.sellPrice(silver: 1000);
            Item.scale -= 0.25f;
            Item.UseSound = SoundID.Item38;
            Spread = 0;
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.PhoenixBlaster))
            {
                modplayer.Deagle_PhoenixBlaster = true;
                modplayer.SynergyBonus++;
            }
            if(player.HasItem(ItemID.DaedalusStormbow))
            {
                modplayer.Deagle_DaedalusStormBow = true;
                modplayer.SynergyBonus++;
            }
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            if (modplayer.Deagle_PhoenixBlaster)
            {
                tooltips.Add(new TooltipLine(Mod, "Deagle_PhoenixBlaster", $"[i:{ItemID.PhoenixBlaster}] You shoot out additional bullet but at a random position, getting crit will make the next shot shoot out a fire phoenix dealing quadruple damage"));
            }
            if(modplayer.Deagle_DaedalusStormBow)
            {
                tooltips.Add(new TooltipLine(Mod, "Deagle_DaedalusStormBow",
                    $"[i:{ItemID.DaedalusStormbow}] Upon critical hit, storm of bullet fly down at target, have a 10 second cool down"));
            }
        }
        public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.velocity != Vector2.Zero)
            {
                Spread = 120;
                velocity = velocity.RotateRandom(120);
            }
            else
            {
                Spread = 0;
                velocity *= 1.5f;
                damage = (int)(damage * 1.5f);
                knockback *= 2f;
                if (type == ProjectileID.Bullet)
                {
                    type = ProjectileID.BulletHighVelocity;
                }
            }
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            if (modplayer.Deagle_PhoenixBlaster)
            {
                Vector2 position2 = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(position, 300, 300);
                if (modplayer.Deagle_PhoenixBlaster_Critical)
                {
                    Projectile.NewProjectile(source, position, (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * Item.shootSpeed, ProjectileID.DD2PhoenixBowShot, damage * 4, knockback, player.whoAmI);
                    int proj = Projectile.NewProjectile(source, position2, (Main.MouseWorld - position2).SafeNormalize(Vector2.Zero) * Item.shootSpeed, ProjectileID.DD2PhoenixBowShot, damage * 2, knockback, player.whoAmI);
                    Main.projectile[proj].scale = .5f;
                    Main.projectile[proj].width = (int)(Main.projectile[proj].width * .5f);
                    Main.projectile[proj].height = (int)(Main.projectile[proj].height * .5f);
                    modplayer.Deagle_PhoenixBlaster_Critical = false;
                    CanShootItem = false;
                    return;
                }
                Projectile.NewProjectile(source, position2, (Main.MouseWorld - position2).SafeNormalize(Vector2.Zero) * Item.shootSpeed, type, damage, knockback, player.whoAmI);
            }
            CanShootItem = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 2);
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            if (player.velocity == Vector2.Zero)
            {
                crit += 55f;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Handgun)
                .AddIngredient(ItemID.Musket)
                .Register();
        }
    }
}