using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Texture;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight
{
    internal class KingSlimeDelight : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            Item.rare = 7;
            Item.value = 10000000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpSpeedBoost += 2.5f;
            player.accRunSpeed += .1f;
            player.statDefense += 5;
            player.GetModPlayer<KingSlimePowerPlayer>().KingSlimePower = true;
        }
    }

    internal class KingSlimePowerPlayer : ModPlayer
    {
        //King Slime
        int KSPcounter = 0;
        public bool KingSlimePower;
        public override void ResetEffects()
        {
            KingSlimePower = false;
        }

        public override void UpdateEquips()
        {
            if (KingSlimePower)
            {
                if (Player.Center.LookForHostileNPC(100f))
                {
                    KSPcounter++;
                    if (KSPcounter == 100)
                    {
                        float rotation = MathHelper.ToRadians(180);
                        float ProjectileNum = 18;
                        Vector2 DirectionFromProjToAim = Vector2.One.SafeNormalize(Vector2.UnitX) * 8f;
                        for (int i = 0; i < ProjectileNum; i++)
                        {
                            Vector2 RotateSpeed = DirectionFromProjToAim.RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (ProjectileNum - 1)));
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, RotateSpeed, ModContent.ProjectileType<FriendlySlimeProjectile>(), 25, 1f, Player.whoAmI);
                            KSPcounter = 0;
                        }
                    }
                }
            }
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (KingSlimePower)
            {
                Vector2 DistanceFromProjToAim = Main.MouseWorld - Player.Center;
                Vector2 DirectionFromProjToAim = DistanceFromProjToAim.SafeNormalize(Vector2.UnitX);
                Vector2 speed = DirectionFromProjToAim * 20f;
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, speed, ModContent.ProjectileType<FriendlySlimeProjectile>(), (int)(damage * 0.35f), 1f, Player.whoAmI);
            }
        }
    }
}
