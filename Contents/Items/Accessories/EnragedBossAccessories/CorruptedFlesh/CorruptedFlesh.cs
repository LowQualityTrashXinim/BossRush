using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Texture;
using BossRush.Contents.Items.Artifact;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.CorruptedFlesh
{
    internal class CorruptedFlesh : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Isn't this is just rotten flesh but even more rotten ?" +
                "\nIncrease damage by 10%" +
                "\nIncrease movement speed by 15%" +
                "\nIncrease melee speed by 10%" +
                "\nYou shoot out tiny eater for each shot" +
                "\nUpon getting hit, you will shoot out mini eater");
        }
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
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
            player.accRunSpeed += 0.15f;
            player.GetModPlayer<CorruptedFleshPlayer>().CorruptedPower = true;
        }
    }
    public class CorruptedFleshPlayer : ModPlayer
    {
        public bool CorruptedPower;
        public override void ResetEffects()
        {
            CorruptedPower = false;
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CorruptedPower && item.type != ModContent.ItemType<GodDice>()) Projectile.NewProjectile(source, Player.Center, Main.rand.NextVector2CircularEdge(10, 10), ProjectileID.TinyEater, damage, knockback, Player.whoAmI);
            return true;
        }
        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            CreateProjectile();
        }
        public void CreateProjectile()
        {
            if (CorruptedPower)
            {
                float rotation = MathHelper.ToRadians(180f);
                float amount = 20f;
                for (int i = 0; i < amount; i++)
                {
                    Vector2 Rotate = Vector2.One.RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (amount - 1)));
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Rotate, ProjectileID.TinyEater, 30, 2f, Player.whoAmI);
                }
            }
        }
    }
}
