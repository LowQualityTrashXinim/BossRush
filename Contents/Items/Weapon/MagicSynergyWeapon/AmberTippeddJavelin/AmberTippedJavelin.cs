using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.AmberTippeddJavelin
{
    public class AmberTippedJavelin : SynergyModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(42, 42, 30, 5, 20, 20, ItemUseStyleID.Shoot, true);
            Item.BossRushSetDefaultSpear(ModContent.ProjectileType<AmberTippedSpearProj>(), 25);
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 5;
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
                if (!boltFired && player.altFunctionUse != 2)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(i * 5 * Projectile.direction)) * 18, ProjectileID.BoneJavelin, Projectile.damage, 5);

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(i * 12 * Projectile.direction)) * 10 * i, ProjectileID.AmberBolt, Projectile.damage, 5);
                    }
                    boltFired = true;
                }
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-(chooseRotation * 2)));
            }
            if (player.altFunctionUse != 2)
            {
                Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            }
            else
            {
                Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax * 3, progress);
                Dust.NewDustPerfect(Projectile.Center, DustID.AmberBolt, Projectile.velocity.RotatedBy(MathHelper.ToRadians(90f)));
                Dust.NewDustPerfect(Projectile.Center, DustID.AmberBolt, Projectile.velocity.RotatedBy(MathHelper.ToRadians(-90f)));
            }
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.ToRadians(45f);
            else
                Projectile.rotation += MathHelper.ToRadians(135f);
            runAI = false;
        }
    }
}