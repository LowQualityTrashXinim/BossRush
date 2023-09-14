using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
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
            StackLimit = 3;
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
            textureString = BossRushUtils.GetTheSameTextureAsEntity<LifeForceOrb>();
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
    //Upgrade poison perk immunity
    public class ImmunityToPoison : Perk
    {
        public override void SetDefaults()
        {
            Tooltip = 
                "+ Give you immunity to poison" +
                "+ Make a poison aura around player";
            CanBeStack = false;
        }
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.Poisoned] = true;
        }
        public override void Update(Player player)
        {
            BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, 100);
            if (npclist.Count > 0)
            {
                foreach (NPC npc in npclist)
                {
                    npc.AddBuff(BuffID.Poisoned, 1);
                }
            }
        }
    }
    //Upgrade fire perk immunity
    public class ImmunityToOnFire : Perk
    {
        public override void SetDefaults()
        {
            Tooltip = "+ Give you immunity to On Fire !";
            CanBeStack = false;
        }
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.OnFire] = true;
        }
        public override void Update(Player player)
        {

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
            damage -= .1f * StackAmount;
        }
    }
    public class BackUpMana : Perk
    {
        public override void SetDefaults()
        {
            textureString = BossRushUtils.GetTheSameTextureAsEntity<BackUpMana>();
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
            Tooltip = "+ Mysterious potion are slightly better than before";
            StackLimit = 3;
        }
        public override void ResetEffect(Player player)
        {
            player.GetModPlayer<MysteriousPotionPlayer>().PotionPointAddition += 1 * StackAmount;
        }
    }
    public class Dirt : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            Tooltip = "+ Having a single dirt in your inventory increase defense by 5";
        }
        public override void ResetEffect(Player player)
        {
            base.ResetEffect(player);
            if (player.HasItem(ItemID.DirtBlock))
                player.statDefense += 5;
        }
    }
    public class PotionExpert : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            Tooltip =
                "+ Potion have 35% to not be consumed";
        }
        public override void ResetEffect(Player player)
        {
            player.GetModPlayer<PerkPlayer>().perk_PotionExpert = true;
        }
    }
    public class SniperCharge : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            Tooltip =
                "+ Range weapon can deal 2x damage when it is ready";
        }
        int RandomCountDown = 0;
        int OpportunityWindow = 0;
        public override void Update(Player player)
        {
            if (!player.ItemAnimationActive)
                RandomCountDown = BossRushUtils.CoolDown(RandomCountDown);
            if (RandomCountDown <= 0)
            {
                if (OpportunityWindow == 0)
                {
                    BossRushUtils.CombatTextRevamp(player.Hitbox, Color.ForestGreen, "!");
                    SoundEngine.PlaySound(SoundID.MaxMana);
                }
                OpportunityWindow++;
                if (OpportunityWindow >= 600 || player.ItemAnimationActive)
                {
                    OpportunityWindow = 0;
                    RandomCountDown = Main.rand.Next(150, 210);
                }
            }
        }
        public override void ModifyDamage(Player player, Item item, ref StatModifier damage)
        {
            if (item.DamageType == DamageClass.Ranged && RandomCountDown <= 0 && OpportunityWindow < 600)
            {
                damage *= 2;
            }
        }
    }
    public class SelfExplosion : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = true;
            Tooltip =
                "+ When a enemy hit you, you will do self explosion that deal 100 damage to surrounding enemies";
            StackLimit = 5;
        }
        public override void OnHitByAnything(Player player)
        {
            player.Center.LookForHostileNPC(out List<NPC> npclist, 300);
            foreach (NPC npc in npclist)
            {
                int direction = player.Center.X - npc.Center.X > 0 ? -1 : 1;
                npc.StrikeNPC(npc.CalculateHitInfo(100 * StackAmount, direction, false, 10));
            }
        }
    }
    //public class GodGiveDice : Perk
    //{
    //    public override void SetDefaults()
    //    {
    //        CanBeStack = false;
    //        Tooltip =
    //            "+ God give you a dice";
    //    }
    //    public override void OnChoose(Player player)
    //    {
    //        player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<GodDice>());
    //    }
    //}
    public class SpeedArmor : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = true;
            Tooltip =
                "+ Gain 10% movement speed in exchange for -2 defenses";
            StackLimit = 5;
        }
        public override void ResetEffect(Player player)
        {
            player.moveSpeed += .1f * StackAmount;
            player.statDefense -= 2 * StackAmount;
        }
    }
    public class CelestialRage : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            Tooltip =
                "+ A gift from celestial being";
        }
        public override void OnChoose(Player player)
        {
            player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<CelestialWrath>());
        }
    }
}