using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.AmberTippeddJavelin
{
    public class AmberTippedJavelin : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(42, 42, 30, 5, 20, 20, ItemUseStyleID.Shoot, true);
            Item.BossRushSetDefaultSpear(ModContent.ProjectileType<AmberTippedSpearProj>(), 25);
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 5;
            Item.rare = 3;
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            base.ModifySynergyToolTips(ref tooltips, modplayer);
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.HoldSynergyItem(player, modplayer);
        }
        public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            base.ModifySynergyShootStats(player, modplayer, ref position, ref velocity, ref type, ref damage, ref knockback);
            if(player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<AmberTippedProjectile>();
                velocity *= .5f;
                damage = (int)(damage * .75f);
            }
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                SoundEngine.PlaySound(SoundID.Item71);
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            return true;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BoneJavelin)
                .AddIngredient(ItemID.AmberStaff)
                .Register();
        }
    }
    class AmberTippedProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<AmberTippedJavelin>();
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 999;
            base.SetDefaults();
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.SynergyAI(player, modplayer);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 10)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ProjectileID.AmberBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].timeLeft = 100;
                Projectile.ai[0] = 0;
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 10)
            {
                Projectile.velocity.Y += Projectile.velocity.Y > 20 ? 0 : .5f;
            }
        }
        public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.AmberBolt);
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(8f, 8f);
                Main.dust[dust].noGravity = true;
            }
            base.SynergyKill(player, modplayer, timeLeft);
        }
    }
    internal class AmberTippedSpearProj : SynergyModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Spear);
            Projectile.width = 30;
            Projectile.height = 30;
        }
        bool boltFired = false;
        float chooseRotation = Main.rand.NextFloat(-0.6f, 0.6f);
        float HoldoutRangeMin => 50f;
        float HoldoutRangeMax => 100f;

        public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI)
        {
            int duration = player.itemAnimationMax;
            player.heldProj = Projectile.whoAmI;
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            float halfDuration = duration * 0.5f;
            float progress;
            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(chooseRotation));
                if (!boltFired)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 10f, ProjectileID.AmberBolt, Projectile.damage, 5);
                    boltFired = true;
                }
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-(chooseRotation * 2)));
            }
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.ToRadians(45f);
            else
                Projectile.rotation += MathHelper.ToRadians(135f);
            runAI = false;
        }
    }
}