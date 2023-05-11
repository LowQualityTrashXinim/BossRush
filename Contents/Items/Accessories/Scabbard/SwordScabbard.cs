using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Accessories.Scabbard
{
    internal class SwordScabbard : ModItem, ISynergyItem
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
                Vector2 speed = Player.direction == 1 ? new Vector2(Player.GetModPlayer<ParryPlayer>().Parry ? 15 : 5, 0) : new Vector2(Player.GetModPlayer<ParryPlayer>().Parry ? -15 : -5, 0);
                if(Player.HeldItem.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded))
                {
                    speed = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX) * 15f;
                }
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, speed, ModContent.ProjectileType<SwordSlash>(), (int)(Player.HeldItem.damage * .75f), 2f, Player.whoAmI);
            }
        }
    }

    public class SwordSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void AI()
        {
            Projectile.alpha += 255 / 50;
            Projectile.scale += 0.03f;
            Projectile.Size += new Vector2(0.05f, 0.05f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 4;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor, .02f);
            return true;
        }
    }
}
