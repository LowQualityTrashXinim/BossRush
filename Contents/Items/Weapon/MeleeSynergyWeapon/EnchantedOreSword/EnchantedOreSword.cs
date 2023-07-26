using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedOreSword : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMeleeShootCustomProjectile(50, 50, 15, 6f, 28, 28, BossRushUseStyle.GenericSwingDownImprove, ModContent.ProjectileType<EnchantedSilverSwordP>(), 15f, true);
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
            if (modplayer.EnchantedOreSword_Musket)
            {
                tooltips.Add(new TooltipLine(Mod, "EnchantedOreSword_StarFury", $"[i:{ItemID.Musket}] Shortsword on hit will launch out a ghost musket that shoot enemy ( up to 5 muskets )"));
            }
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.HoldSynergyItem(player, modplayer);
            if (player.HasItem(ItemID.Starfury))
            {
                modplayer.EnchantedOreSword_StarFury = true;
            }
            if (player.HasItem(ItemID.Musket))
            {
                modplayer.EnchantedOreSword_Musket = true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int[] RandomShortSword = new int[] {
                ModContent.ProjectileType<EnchantedCopperSwordP>(),
                ModContent.ProjectileType<EnchantedTinSwordP>(),
                ModContent.ProjectileType<EnchantedLeadSwordP>(),
                ModContent.ProjectileType<EnchantedIronSwordP>(),
                ModContent.ProjectileType<EnchantedSilverSwordP>(),
                ModContent.ProjectileType<EnchantedTungstenSwordP>(),
                ModContent.ProjectileType<EnchantedGoldSwordP>(),
                ModContent.ProjectileType<EnchantedPlatinumSwordP>() };
            if (count < RandomShortSword.Length - 1)
            {
                count++;
            }
            else
            {
                count = 0;
            }
            Vector2 Above;
            Vector2 AimTo;
            float Rotation;
            switch (count)
            {
                case 0:
                    Projectile.NewProjectile(source, position, velocity, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
                case 1:
                    Projectile.NewProjectile(source, position, velocity, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
                case 2:
                    Rotation = MathHelper.ToRadians(180);
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 RotateSurround = velocity.RotatedBy(MathHelper.Lerp(-Rotation, Rotation, i / 8f));
                        Projectile.NewProjectile(source, position, RotateSurround, RandomShortSword[count], damage, knockback, player.whoAmI);
                    }
                    break;
                case 3:
                    Above = Main.MouseWorld + velocity.SafeNormalize(Vector2.UnitX) * 250f;
                    AimTo = (player.Center - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                    Rotation = MathHelper.ToRadians(15);
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 RotateSurround = AimTo.RotatedBy(MathHelper.Lerp(-Rotation, Rotation, i / 5f));
                        Projectile.NewProjectile(source, Above, RotateSurround, RandomShortSword[count], damage, knockback, player.whoAmI);
                    }
                    break;
                case 4:
                    Above = new Vector2(Main.MouseWorld.X + Main.rand.Next(-300, 300), player.Center.Y - 500);
                    AimTo = (Main.MouseWorld - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                    Projectile.NewProjectile(source, Above, AimTo, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
                case 5:
                    Above = new Vector2(Main.MouseWorld.X + Main.rand.Next(-300, 300), player.Center.Y + 500);
                    AimTo = (Main.MouseWorld - Above).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
                    Projectile.NewProjectile(source, Above, AimTo, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
                case 6:
                    Projectile.NewProjectile(source, position, velocity, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
                case 7:
                    Projectile.NewProjectile(source, position, velocity, RandomShortSword[count], damage, knockback, player.whoAmI);
                    break;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("OreShortSword")
                .AddRecipeGroup("OreBroadSword")
                .AddRecipeGroup("Wood Sword")
                .Register();
        }
    }
}