using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using BossRush.Contents.BuffAndDebuff;
using System;

namespace BossRush.Contents.Items.Artifact
{
    internal class VampirismCrystal : ModItem, IArtifactItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unpure, relentess crystal" +
                "\nPassive : Attack heal player " +
                "\nEffect : reduce max life by 65%" +
                "\nYou can survive 1 fatal attack ( 3 minute cooldown )");
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
            player.statLifeMax2 -= (int)(player.statLifeMax * 0.65f);
        }
    }
    public class VampirePlayer : ModPlayer
    {
        public bool VampireLifeSuck;
        public bool CoolDown;
        int countRange = 0;
        public override void ResetEffects()
        {
            VampireLifeSuck = false;
            CoolDown = false;
        }

        private void LifeSteal(NPC target, int rangeMin = 1, int rangeMax = 3, float multiplier = 1)
        {
            if (VampireLifeSuck && target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy)
            {
                int HP = (int)(Main.rand.Next(rangeMin, rangeMax) * multiplier);
                if (Player.statLife != Player.statLifeMax2)
                {
                    Player.Heal(HP);
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            countRange++;
            if (countRange >= 3)
            {
                LifeSteal(target);
                countRange = 0;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (!CoolDown && !Player.HasBuff(ModContent.BuffType<SecondChance>()) && VampireLifeSuck)
            {
                Player.Heal(Player.statLifeMax2);
                Player.AddBuff(ModContent.BuffType<SecondChance>(), 10800);
                return false;
            }
            return true;
        }
    }
}
