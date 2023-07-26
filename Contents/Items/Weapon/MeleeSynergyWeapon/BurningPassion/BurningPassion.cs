using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.BurningPassion
{
    class BurningPassion : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(74, 74, 40, 6.7f, 28, 28, ItemUseStyleID.Shoot, true);
            Item.BossRushSetDefaultSpear(ModContent.ProjectileType<BurningPassionP>(), 3.7f);
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(silver: 1000);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<BurningPassionP>()] < 1;
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            base.ModifySynergyToolTips(ref tooltips, modplayer);
            if (modplayer.BurningPassion_WandofFrosting)
            {
                tooltips.Add(new TooltipLine(Mod, "WandOfFrosting", $"[i:{ItemID.WandofFrosting}] Inflict frost burn on hit"));
            }
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.WandofFrosting))
            {
                modplayer.BurningPassion_WandofFrosting = true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                player.velocity = Vector2.Zero;

                player.velocity.X = velocity.X * 5f;
                player.velocity.Y = velocity.Y * 5f;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Spear)
                .AddIngredient(ItemID.WandofSparking)
                .Register();
        }
    }
    public class BurningPassionP : SynergyModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 19;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        protected virtual float HoldoutRangeMin => 50f;
        protected virtual float HoldoutRangeMax => 200f;
        public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI)
        {
            int duration = player.itemAnimationMax;
            if (player.altFunctionUse == 2 && player.ItemAnimationJustStarted)
            {
                Projectile.width += 30;
                Projectile.height += 30;
                Projectile.damage *= 3;
            }
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
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.ToRadians(45f);
            }
            else
            {
                Projectile.rotation += MathHelper.ToRadians(135f);
            }
            runAI = false;
        }
        public override void SpawnDustPostPreAI(Player player)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.Center, (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.Torch, Projectile.velocity.X * 0.75f, -5, 0, default, Main.rand.NextFloat(0.5f, 1.2f));
                if (player.GetModPlayer<PlayerSynergyItemHandle>().BurningPassion_WandofFrosting)
                {
                    Dust.NewDust(Projectile.Center, (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.IceTorch, Projectile.velocity.X * 0.75f, -5, 0, default, Main.rand.NextFloat(0.5f, 1.2f));
                }
            }
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            if(modplayer.BurningPassion_WandofFrosting)
            {
                npc.AddBuff(BuffID.Frostburn, 90);
            }
            npc.AddBuff(BuffID.OnFire, 90);
            npc.immune[Projectile.owner] = 5;
        }
    }
}