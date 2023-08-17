using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.Potion;

namespace BossRush.Contents.Perks
{
    public class GenericDamageIncrease : Perk
    {
        public override void SetDefaults()
        {
            Tooltip =
                    "+ Increase damage by 10%";
            CanBeStack = true;
            StackLimit = 5;
        }
        public override void ModifyDamage(Player player, Item item, ref StatModifier damage)
        {
            damage += .1f * StackAmount;
        }
    }
    public class LifeForceOrb : Perk
    {
        public override void SetDefaults()
        {
            Tooltip = "+ Attacking enemy will periodically create a life orb that heal you";
            CanBeStack = false;
        }
        public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            LifeForceSpawn(player, target);
        }
        public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            LifeForceSpawn(player, target);
        }
        private void LifeForceSpawn(Player player, NPC target)
        {
            if (Main.rand.NextBool(20))
                Projectile.NewProjectile(player.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(100, 100), Vector2.Zero, ModContent.ProjectileType<LifeOrb>(), 0, 0, player.whoAmI);
        }
    }
    public class ImmunityToPoison : Perk
    {
        public override void SetDefaults()
        {
            Tooltip = "+ Give you immunity to poison";
            CanBeStack = false;
        }
        public override void Update(Player player)
        {
            player.buffImmune[BuffID.Poisoned] = true;
        }
    }
    public class IllegalTrading : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = true;
            Tooltip =
                "+ Increase amount of weapon drop from chest by 1 !" +
                "\n- Decrease your damage by 10%";
            StackLimit = 5;
        }
        public override void ResetEffect(Player player)
        {
            player.GetModPlayer<ChestLootDropPlayer>().WeaponAmountAddition += 1 * StackAmount;
        }
        public override void ModifyDamage(Player player, Item item, ref StatModifier damage)
        {
            damage -= -.1f * StackAmount;
        }
    }
    public class BackUpMana : Perk
    {
        public override void SetDefaults()
        {
            Tooltip =
                  "+ You can fire magic weapon forever" +
                  "\n- When you are out of mana, you will use your life instead";
            CanBeStack = false;
        }
        public override void OnMissingMana(Player player, Item item, int neededMana)
        {
            player.statMana += neededMana;
            player.statLife = Math.Clamp(player.statLife - neededMana, 0, player.statLifeMax2);
        }
    }
    public class PeaceWithGod : Perk
    {
        public override void SetDefaults()
        {
            Tooltip =
                "+ God no longer angry at you and now opening lootbox give you syngery weapon" +
                "\n- Synergy bonus no longer available";
            CanBeStack = false;
        }
        public override void ResetEffect(Player player)
        {
            player.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonusBlock = true;
            player.GetModPlayer<ChestLootDropPlayer>().CanDropSynergyEnergy = true;
        }
    }
    public class Alchemistknowledge : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = true;
            Tooltip = "+ Mysterious potion are better than before";
            StackLimit = 3;
        }
        public override void ResetEffect(Player player)
        {
            player.GetModPlayer<MysteriousPotionPlayer>().PotionPointAddition += 3;
        }
    }
}