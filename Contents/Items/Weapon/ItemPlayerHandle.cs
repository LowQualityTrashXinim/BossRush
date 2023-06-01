using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.BurningPassion;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.DarkCactus;

namespace BossRush.Contents.Items.Weapon
{
    //This mod player should hold all the logic require for the item, if the item is shooting out the projectile, it should be doing that itself !
    //Same with projectile unless it is a vanilla projectile then we can refer to global projectile
    //This should only hold custom bool or data that we think should be transfer
    //We will name using the following format "Synergy item"_"vanilla item"
    //Anything that relate to actual logic and how player interact from the item could also go in here
    public class PlayerSynergyItemHandle : ModPlayer
    {
        public bool BurningPassion_WandofFrosting = false;

        public bool DarkCactus_BatScepter = false;
        public bool DarkCactus_BladeOfGrass = false;

        public bool EnchantedOreSword_StarFury = false;
        public bool EnchantedOreSword_Musket = false;

        public bool EnchantedStarfury_SkyFacture = false;
        public bool EnchantedStarfury_BreakerBlade = false;
        public override void ResetEffects()
        {
            base.ResetEffects();
            BurningPassion_WandofFrosting = false;
            DarkCactus_BatScepter = false;
            DarkCactus_BladeOfGrass = false;
            EnchantedOreSword_StarFury = false;
            EnchantedOreSword_Musket = false;
            EnchantedStarfury_SkyFacture = false; 
            EnchantedStarfury_BreakerBlade = false;
        }
        int check = 1;
        public override void PostUpdate()
        {
            Item item = Player.HeldItem;
            if (item.ModItem is BurningPassion)
            {
                if (!Player.ItemAnimationActive && check == 0)
                {
                    Player.velocity *= .25f;
                    check++;
                }
                else if (Player.ItemAnimationActive && Main.mouseRight)
                {
                    Player.gravity = 0;
                    Player.velocity.Y -= 0.3f;
                    Player.ignoreWater = true;
                    check = 0;
                }
            }
        }
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (Player.ItemAnimationActive && Player.HeldItem.ModItem is BurningPassion && Player.ownedProjectileCounts[ModContent.ProjectileType<BurningPassionP>()] > 0)
            {
                return true;
            }
            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }
    }
    public abstract class SynergyModItem : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            ModifySynergyToolTips(ref tooltips, Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>());
        }
        public virtual void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) { }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
            PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
            HoldSynergyItem(player, modplayer);
        }
        public virtual void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) { }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SynergyShoot(player, player.GetModPlayer<PlayerSynergyItemHandle>(), source, position, velocity, type, damage, knockback, out bool CanShootItem);
            return CanShootItem;
        }
        public virtual void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) { CanShootItem = true; }
    }
    public abstract class SynergyModProjectile : ModProjectile
    {
        public virtual void SpawnDustPostPreAI(Player player) { }
        public virtual void SpawnDustPostAI(Player player) { }
        public virtual void SpawnDustPostPostAI(Player player) { }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            SynergyPreAI(player, player.GetModPlayer<PlayerSynergyItemHandle>(), out bool stopAI);
            SpawnDustPostPreAI(player);
            return stopAI;
        }
        /// <summary>
        /// You should check the condition yourself
        /// </summary>
        /// <param name="player"></param>
        /// <param name="modplayer"></param>
        /// <param name="runAI"></param>
        public virtual void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) { runAI = true; }
        public override void AI()
        {
            base.AI();
            Player player = Main.player[Projectile.owner];
            SynergyAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
            SpawnDustPostAI(player);
        }
        /// <summary>
        /// You should check the condition yourself
        /// </summary>
        /// <param name="player"></param>
        /// <param name="modplayer"></param>
        public virtual void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) { }
        public override void PostAI()
        {
            base.PostAI();
            Player player = Main.player[Projectile.owner];
            SynergyPostAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
            SpawnDustPostPostAI(player);
        }
        /// <summary>
        /// You should check the condition yourself
        /// </summary>
        /// <param name="player"></param>
        /// <param name="modplayer"></param>
        public virtual void SynergyPostAI(Player player, PlayerSynergyItemHandle modplayer) { }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            Player player = Main.player[Projectile.owner];
            ModifyHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, ref modifiers);
        }
        public virtual void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc,ref NPC.HitModifiers modifiers) { }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Player player = Main.player[Projectile.owner];
            OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
        }
        public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) { }
    }
}