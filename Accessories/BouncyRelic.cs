using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Accessories
{
    internal class BouncyRelic : ModItem
    {
        public override string Texture => "BossRush/MissingTexture";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"A small ancient power accidentally left from a old god, said to be cursed because of jealousy from the new god\"" +
                "\nMost projectile can bounce of tile" +
                "\nBouncing off a tile increase damage by 20 %" +
                "\nBouncing off a tile will make the projectile disappear after 3s");
        }
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
    }
    public class PlayerRelic : ModPlayer
    {
        public bool Bouncy;
        public override void ResetEffects()
        {
            Bouncy = false;
        }
    }
    public class BouncyProjectileGlobal : GlobalProjectile
    {
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Player player = Main.player[Main.myPlayer];
            if (player.GetModPlayer<PlayerRelic>().Bouncy)
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
                projectile.damage = (int)(projectile.damage*1.2);
                return false;
            }
            return true;
        }
    }
}
