using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.Scabbard
{
    internal class SwordScabbard : SynergyModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<ParryPlayer>().Parry)
            {
                tooltips.Add(new TooltipLine(Mod, "SwordBrother", $"[i:{ModContent.ItemType<ParryScabbard>()}] Increase parry duration and increase wind slash speed"));
            }
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 42;
            Item.width = 66;
            Item.rare = 3;
            Item.value = 10000000;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<SwordPlayer>().SwordSlash = true;
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood Sword")
                .AddRecipeGroup("OreBroadSword")
                .Register();
        }
    }

    public class SwordPlayer : ModPlayer
    {
        public bool SwordSlash;
        public override void ResetEffects()
        {
            SwordSlash = false;
        }
        public override void PostUpdate()
        {
            if (Player.HeldItem.DamageType == DamageClass.Melee
                && Player.HeldItem.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckVanillaSwingWithModded)
                && SwordSlash
                && Main.mouseLeft
                && Player.ItemAnimationJustStarted
                && Player.ItemAnimationActive)
            {
                Vector2 speed = Player.direction == 1 ? new Vector2(Player.GetModPlayer<ParryPlayer>().Parry ? 3 : 1, 0) : new Vector2(Player.GetModPlayer<ParryPlayer>().Parry ? -3 : -1, 0);
                Item item = Player.HeldItem;
                if (Player.HeldItem.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded))
                {
                    speed = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero);
                }
                float length = new Vector2(item.width, item.height).Length() * Player.GetAdjustedItemScale(item);
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.PositionOFFSET(speed, length + 17), speed * 5, ModContent.ProjectileType<SwordSlash>(), (int)(Player.HeldItem.damage * .75f), 2f, Player.whoAmI);
            }
        }
    }

    public class SwordSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 360;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 6;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void AI()
        {
            Projectile.alpha += 255 / 50;
            Projectile.Size += new Vector2(0.05f, 0.05f);
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.damage > 1)
            {
                Projectile.damage = (int)(Projectile.damage * .8f);
            }
            target.immune[Projectile.owner] = 4;
        }
        bool hittile = false;
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
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor, .02f);
            return true;
        }
    }
}
