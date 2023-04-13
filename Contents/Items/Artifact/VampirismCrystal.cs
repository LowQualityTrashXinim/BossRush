﻿using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Contents.Items.Artifact
{
    internal class VampirismCrystal : ModItem, IArtifactItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unpure, relentess crystal" +
                "\nAttack heal player but reduce max life by 55%" +
                "\nYou can survive 1 fatal attack ( 1m30s cooldown )");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 7));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 58;
            Item.accessory = true;
            Item.rare = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<VampirePlayer>().VampireLifeSuck = true;
            player.statLifeMax2 -= (int)(player.statLifeMax * 0.55f);
        }
    }
    public class VampirePlayer : ModPlayer
    {
        public bool VampireLifeSuck;
        public bool CoolDown;

        public override void ResetEffects()
        {
            VampireLifeSuck = false;
            CoolDown = false;
        }

        private void LifeSteal(NPC target)
        {
            if (VampireLifeSuck && target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy)
            {
                int HP = Main.rand.Next(1, 6);
                if (Player.statLife != Player.statLifeMax2)
                {
                    Player.Heal(HP);
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            LifeSteal(target);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            LifeSteal(target);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (!CoolDown && !Player.HasBuff(ModContent.BuffType<SecondChance>()) && VampireLifeSuck)
            {
                Player.Heal(1);
                Player.AddBuff(ModContent.BuffType<SecondChance>(), 5400);
                return false;
            }
            return true;
        }
    }
}