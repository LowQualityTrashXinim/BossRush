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
using Terraria.DataStructures;

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
    public class ImmunityToPoison : Perk
    {
        public override void SetDefaults()
        {
            Tooltip =
                "+ Give you immunity to poison" +
                "\n+ Make a poison aura around player";
            CanBeStack = false;
        }
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.Poisoned] = true;
        }
        public override void Update(Player player)
        {
            BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, 150);
            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(100, 100), 0, 0, DustID.Poisoned);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Vector2.Zero;
                Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
            }
            if (npclist.Count > 0)
            {
                foreach (NPC npc in npclist)
                {
                    npc.AddBuff(BuffID.Poisoned, 1);
                }
            }
        }
    }
    public class ImmunityToOnFire : Perk
    {
        public override void SetDefaults()
        {
            Tooltip =
                "+ Give you immunity to On Fire !" +
                "\n+ Make a poison aura around player";
            CanBeStack = false;
        }
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.OnFire] = true;
        }
        public override void Update(Player player)
        {
            BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, 150);
            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(100, 100), 0, 0, DustID.Torch);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = -Vector2.UnitY * 4f;
                Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
            }
            if (npclist.Count > 0)
            {
                foreach (NPC npc in npclist)
                {
                    npc.AddBuff(BuffID.OnFire, 1);
                }
            }
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
                  "\n- When you are out of mana, mana cost reduce is by 50% and you use your life instead";
            CanBeStack = false;
        }
        public override void OnMissingMana(Player player, Item item, int neededMana)
        {
            player.statMana += neededMana;
            player.statLife = Math.Clamp(player.statLife - (int)(neededMana * .5f), 0, player.statLifeMax2);
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
            textureString = BossRushUtils.GetTheSameTextureAsEntity<PotionExpert>();
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
            for (int i = 0; i < 150; i++)
            {
                int smokedust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke);
                Main.dust[smokedust].noGravity = true;
                Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(25, 25);
                Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
                int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(25, 25);
                Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
            }
        }
    }
    public class SpecialPotion : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            Tooltip =
                "+ Grant you 1 random special potion";
        }
        public override void OnChoose(Player player)
        {
            int type = Main.rand.Next(new int[] { ModContent.ItemType<TitanElixir>(), ModContent.ItemType<BerserkerElixir>(), ModContent.ItemType<GunslingerElixir>(), ModContent.ItemType<CommanderElixir>(), ModContent.ItemType<SageElixir>(), });
            player.QuickSpawnItem(player.GetSource_FromThis(), type);
        }
    }
    public class ProjectileProtection : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = true;
            Tooltip =
                "+ You are 15% more resistant to projectile";
            StackLimit = 5;
        }
        public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo)
        {
            hurtInfo.SourceDamage = (int)(hurtInfo.SourceDamage * (1 - .15f * StackAmount));
        }
    }
    public class ProjectileDuplication : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = true;
            Tooltip =
                "+ Your weapon have a chance to shoot out duplicate projectile ( warning : may work weirdly on many weapon due to terraria code )";
            StackLimit = 5;
        }
        public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextFloat() <= .1f * StackAmount)
                Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), type, damage, knockback, player.whoAmI);
        }
    }
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
    public class SolarGladiator : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            CanBeChoosen = false;
            Tooltip =
                "+ 58% increased odds for melee" +
                "\n+ 5% melee speed" +
                "\n+ 5% melee size increases";
        }
        public override void Update(Player player)
        {
            player.GetModPlayer<ChestLootDropPlayer>().MeleeChanceMutilplier += .58f;
            player.GetAttackSpeed(DamageClass.Melee) += .05f;
        }
        public override void ModifyItemScale(Player player, Item item, ref float scale)
        {
            if (item.DamageType == DamageClass.Melee)
                scale += .05f;
        }
    }
    public class VortexRanger : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            CanBeChoosen = false;
            Tooltip =
                "+ 58% increased odds for range" +
                "\n+ 5% range critical chance increases";
        }
        public override void Update(Player player)
        {
            player.GetModPlayer<ChestLootDropPlayer>().RangeChanceMutilplier += .58f;
            player.GetAttackSpeed(DamageClass.Melee) += .05f;
        }
        public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit)
        {
            if (item.DamageType == DamageClass.Ranged)
                crit += 5;
        }
    }
    public class NebulaMage : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            CanBeChoosen = false;
            Tooltip =
                "+ 58% increased odds for magic" +
                "\n+ 5% magic cost reduction";
        }
        public override void Update(Player player)
        {
            player.GetModPlayer<ChestLootDropPlayer>().MagicChanceMutilplier += .58f;
        }
        public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi)
        {
            reduce -= item.mana * .05f;
        }
    }
    public class StardustTamer : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            CanBeChoosen = false;
            Tooltip =
                "+ 58% increased odds for summoner" +
                "\n+ 5% magic cost reduction";
        }
        public override void Update(Player player)
        {
            player.GetModPlayer<ChestLootDropPlayer>().SummonChanceMutilplier += .58f;
        }
        public override void ModifyDamage(Player player, Item item, ref StatModifier damage)
        {
            if (item.DamageType == DamageClass.Summon || item.DamageType == DamageClass.SummonMeleeSpeed)
                damage += .05f;
        }
    }
    public class IncreasePerkSelectionRange : Perk
    {
        public override void SetDefaults()
        {
            CanBeStack = false;
            CanBeChoosen = false;
            Tooltip =
                "+Increases perk range amount by 1";
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
}