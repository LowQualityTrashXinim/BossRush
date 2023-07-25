using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Accessories.BouncyRelic
{
    internal class BouncyRelic : ModItem, ISynergyItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 28;
            Item.rare = 2;
            Item.value = 1000000;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PlayerRelic>().Bouncy = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.KingSlimeTrophy)
                .AddIngredient(ItemID.SlimeGun)
                .Register();
        }
    }
    public class PlayerRelic : ModPlayer
    {
        public bool Bouncy = false;
        public override void ResetEffects()
        {
            Bouncy = false;
        }
    }
    public class BouncyProjectileGlobal : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        int counter = 0;
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<PlayerRelic>().Bouncy && !projectile.minion)
            {
                projectile.tileCollide = true;
                Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                if (projectile.timeLeft > 180)
                {
                    projectile.timeLeft = 180;
                }
                counter++;
                if(counter > 10)
                {
                    return false;
                }
                projectile.damage = (int)(projectile.damage * 1.2f);
                return false;
            }
            return true;
        }
        public override void PostAI(Projectile projectile)
        {
        }
    }
}
