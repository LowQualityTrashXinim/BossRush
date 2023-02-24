using Mono.Cecil;
using System.Reflection;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ID;

namespace BossRush.Items.Weapon
{
    internal class GhostHitBox : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 1;
        }
    }
    internal class GhostHitBox2 : ModProjectile
    {
        public override string Texture => "BossRush/Items/Weapon/GhostHitBox";
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Melee;
        }
        //public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        //{
        //    Player player = Main.player[Projectile.owner];
        //    Item item = player.HeldItem;
        //    try
        //    {
        //        typeof(Player).GetMethod("ItemCheck_MeleeHitNPCs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(player, new object[] { item, Projectile.Hitbox, item.OriginalDamage, item.knockBack });
        //    }
        //    catch (MissingMethodException)
        //    {
        //        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("something went wrong"), Colors.RarityNormal);
        //        return;
        //    }
        //}
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
    }
}