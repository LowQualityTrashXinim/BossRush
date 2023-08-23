using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedOreSword : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMeleeShootCustomProjectile(50, 50, 19, 6f, 24, 24, BossRushUseStyle.GenericSwingDownImprove, ModContent.ProjectileType<EnchantedSilverSwordP>(), 15f, true);
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
        }
        int count = -1;
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            if (modplayer.EnchantedOreSword_StarFury)
            {
                tooltips.Add(new TooltipLine(Mod, "EnchantedOreSword_StarFury", $"[i:{ItemID.Starfury}] Shortsword will leave a trail of star"));
            }
            if (modplayer.EnchantedOreSword_EnchantedSword)
            {
                tooltips.Add(new TooltipLine(Mod, "EnchantedOreSword_EnchantedSword", $"[i:{ItemID.EnchantedSword}] you shoot out additional shortsword attack of random"));
            }
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.HoldSynergyItem(player, modplayer);
            if (player.HasItem(ItemID.Starfury))
            {
                modplayer.EnchantedOreSword_StarFury = true;
            }
            if (player.HasItem(ItemID.EnchantedSword))
            {
                modplayer.EnchantedOreSword_EnchantedSword = true;
            }
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            if (count < TerrariaArrayID.EnchantedOreSwordProjectile.Length - 1)
            {
                count++;
            }
            else
            {
                count = 0;
            }
            if (modplayer.EnchantedOreSword_EnchantedSword)
            {
                ShootSwordSword(player, source, position, velocity, damage, knockback, Main.rand.Next(TerrariaArrayID.EnchantedOreSwordProjectile.Length));
            }
            ShootSwordSword(player, source, position, velocity, damage, knockback, count);
            CanShootItem = false;
        }
        private void ShootSwordSword(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, int counter)
        {
            Vector2 Above;
            Vector2 AimTo;
            float Rotation;
            switch (counter)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vel = velocity.Vector2DistributeEvenly(3, 30, i);
                        Projectile.NewProjectile(source, position, vel, TerrariaArrayID.EnchantedOreSwordProjectile[count], damage, knockback, player.whoAmI);
                    }
                    break;
                case 1:
                    Projectile.NewProjectile(source, position, velocity, TerrariaArrayID.EnchantedOreSwordProjectile[count], damage, knockback, player.whoAmI);
                    break;
                case 2:
                    Rotation = MathHelper.ToRadians(180);
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 RotateSurround = velocity.RotatedBy(MathHelper.Lerp(-Rotation, Rotation, i / 8f));
                        Projectile.NewProjectile(source, position, RotateSurround, TerrariaArrayID.EnchantedOreSwordProjectile[count], damage, knockback, player.whoAmI);
                    }
                    break;
                case 3:
                    Above = Main.MouseWorld + velocity.SafeNormalize(Vector2.UnitX) * 250f;
                    AimTo = (player.Center - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                    Rotation = MathHelper.ToRadians(15);
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 RotateSurround = AimTo.RotatedBy(MathHelper.Lerp(-Rotation, Rotation, i / 5f));
                        Projectile.NewProjectile(source, Above, RotateSurround, TerrariaArrayID.EnchantedOreSwordProjectile[count], damage, knockback, player.whoAmI);
                    }
                    break;
                case 4:
                    Above = new Vector2(Main.MouseWorld.X + Main.rand.Next(-300, 300), player.Center.Y - 500);
                    AimTo = (Main.MouseWorld - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                    Projectile.NewProjectile(source, Above, AimTo, TerrariaArrayID.EnchantedOreSwordProjectile[count], damage, knockback, player.whoAmI);
                    break;
                case 5:
                    Above = new Vector2(Main.MouseWorld.X + Main.rand.Next(-300, 300), player.Center.Y + 500);
                    AimTo = (Main.MouseWorld - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                    Projectile.NewProjectile(source, Above, AimTo, TerrariaArrayID.EnchantedOreSwordProjectile[count], damage, knockback, player.whoAmI);
                    break;
                case 6:
                    Projectile.NewProjectile(source, position, velocity, TerrariaArrayID.EnchantedOreSwordProjectile[count], damage, knockback, player.whoAmI);
                    break;
                case 7:
                    Projectile.NewProjectile(source, position, velocity, TerrariaArrayID.EnchantedOreSwordProjectile[count], damage, knockback, player.whoAmI);
                    break;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("OreShortSword")
                .AddRecipeGroup("OreBroadSword")
                .Register();
        }
    }
}