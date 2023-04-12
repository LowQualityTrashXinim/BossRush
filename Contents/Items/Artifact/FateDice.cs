using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.Artifact
{
    internal class FateDice : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fate Decider");
            Tooltip.SetDefault("Increase amount weapon get drop from Treasure Chest by 4" +
                "\nRandom effect may occur ..." +
                "\n\"Replacement of god dice\"");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = 9;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ChestLootDropPlayer>().amountModifier += 4;
            player.GetModPlayer<FateDicePlayer>().FateDice = true;
        }
    }
    class FateDicePlayer : ModPlayer
    {
        public bool FateDice = false;
        int RNGdecide;
        int effectlasting = 0;

        bool CanCrit = false;
        bool CanBeHit = true;
        bool NPCdeal5TimeDamage = false;
        bool UnableToUseWeapon = false;
        bool ExtendingEffect = false;
        public override void ResetEffects()
        {
            FateDice = false;
        }
        public override void PreUpdate()
        {
            effectlasting -= effectlasting > 0 ? 1 : 0;
            if (!FateDice)
            {
                return;
            }
            if (effectlasting == 0)
            {
                RNGdecide = -1;
                CanCrit = false;
                CanBeHit = true;
                NPCdeal5TimeDamage = false;
                UnableToUseWeapon = false;
                ExtendingEffect = false;
            }
        }
        public override void PostUpdate()
        {
            if (!FateDice)
            {
                return;
            }
            if (effectlasting == 0 & Main.rand.NextBool(100))
            {
                RNGdecide = Main.rand.Next(16);
            }
            switch (RNGdecide)
            {
                case 0:
                    Player.velocity = Vector2.Zero;
                    Player.AddBuff(BuffID.Frozen, 60);
                    effectlasting = 5;
                    break;
                case 1:
                    Player.AddBuff(BuffID.Weak, 300);
                    Player.AddBuff(BuffID.Slow, 300);
                    Player.AddBuff(BuffID.BrokenArmor, 300);
                    Player.AddBuff(BuffID.ManaSickness, 300);
                    Player.AddBuff(BuffID.Obstructed, 300);
                    effectlasting = 30;
                    break;
                case 2:
                    if (effectlasting % 10 == 0)
                    {
                        BossRushUtils.SpawnBoulderOnTopPlayer(Player, 0, false);
                    }
                    effectlasting = 400;
                    break;
                case 3:
                    Player.AddBuff(BuffID.Confused, 3000);
                    effectlasting = 40;
                    break;
                case 4:
                    NPCdeal5TimeDamage = true;
                    effectlasting = 210;
                    break;
                case 5:
                    if (effectlasting % 4 == 0)
                    {
                        NPC.SpawnOnPlayer(Player.whoAmI, Main.rand.Next(TerrariaArrayID.BAT));
                    }
                    effectlasting = 100;
                    break;
                case 6:
                    UnableToUseWeapon = true;
                    effectlasting = 120;
                    break;
                case 7:
                    for (int i = 0; i < Player.CountBuffs(); i++)
                    {
                        Player.DelBuff(i);
                    }
                    effectlasting = 1;
                    break;
                case 8:
                    if (Main.dayTime)
                    {
                        if (Main.hardMode)
                        {
                            Main.eclipse = true;
                        }
                    }
                    else
                    {
                        Main.bloodMoon = true;
                    }
                    RNGdecide = Main.rand.Next(8);
                    effectlasting = 100;
                    break;
                case 9:
                    Effect9();
                    effectlasting = 1;
                    break;
                case 10:
                    Effect10();
                    effectlasting = 1;
                    break;
                case 11:
                    Effect11();
                    effectlasting = 300;
                    break;
                case 12:
                    Effect12();
                    effectlasting = 1;
                    break;
                case 13:
                    Effect13();
                    effectlasting = 180;
                    break;
                case 14:
                    Effect14();
                    effectlasting = 60;
                    break;
                case 15:
                    effectlasting = 30;
                    Effect9();
                    Effect10();
                    Effect11();
                    Effect12();
                    Effect13();
                    Effect14();
                    Effect16(effectlasting);
                    break;
                case 16:
                    effectlasting = 600;
                    Effect16(effectlasting);
                    break;
                default:
                    break;
            }
            if (Main.rand.NextBool(50) && !ExtendingEffect)
            {
                ExtendingEffect = true;
                effectlasting *= 5;
            }
        }
        private void Effect9()
        {
            ChestLootDrop.GetWeapon(out int Weapon, out int amount);
            Item.NewItem(Player.GetSource_FromThis(), Player.Center + new Vector2(0, -900), Weapon, amount);
        }
        private void Effect10()
        {
            Player.Center.LookForHostileNPC(out List<NPC> npc, 1000);
            for (int i = 0; i < npc.Count; i++)
            {
                npc[i].StrikeNPC(1000, 0, 0);
            }
        }
        private void Effect11()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 position = new Vector2(Player.Center.X + Main.rand.NextFloat(-500, 500), Player.Center.Y - 1000);
                Vector2 velocity = new Vector2(0, 20).RotatedBy(MathHelper.ToRadians(30));
                Projectile.NewProjectile(Player.GetSource_FromThis(), position, velocity, ProjectileID.MeteorShot, 100, 10, Player.whoAmI);
            }
        }
        private void Effect12()
        {
            Player.statLife += (int)Math.Clamp(Player.statLifeMax2 * .25f, 0, Player.statLifeMax2);
        }
        private void Effect13()
        {
            CanCrit = true;
        }
        private void Effect14()
        {
            CanBeHit = false;
        }
        private void Effect16(int effectlasting)
        {
            Player.AddBuff(BuffID.NebulaUpLife3, effectlasting);
            Player.AddBuff(BuffID.NebulaUpDmg3, effectlasting);
            Player.AddBuff(BuffID.NebulaUpMana3, effectlasting);
            Player.AddBuff(BuffID.Endurance, effectlasting);
            Player.AddBuff(BuffID.Wrath, effectlasting);
            Player.AddBuff(BuffID.Rage, effectlasting);
            Player.AddBuff(BuffID.Lifeforce, effectlasting);
            Player.AddBuff(BuffID.Regeneration, effectlasting);
            Player.AddBuff(BuffID.RapidHealing, effectlasting);
            Player.AddBuff(BuffID.Honey, effectlasting);
            Player.AddBuff(BuffID.BeetleEndurance3, effectlasting);
            Player.AddBuff(BuffID.BeetleMight3, effectlasting);
            Player.AddBuff(BuffID.BrainOfConfusionBuff, effectlasting);
        }
        public override bool CanUseItem(Item item)
        {
            if (!FateDice)
            {
                return base.CanUseItem(item);
            }
            return !UnableToUseWeapon;
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (!FateDice)
            {
                return;
            }
            if (CanCrit)
            {
                crit = true;
                damage *= 5;
            }
            base.OnHitNPC(item, target, damage, knockback, crit);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (!FateDice)
            {
                return;
            }
            if (CanCrit)
            {
                crit = true;
                damage *= 5;
            }
            base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (!FateDice)
            {
                return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
            }
            return CanBeHit;
        }
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (FateDice & NPCdeal5TimeDamage)
            {
                damage *= 5;
            }
            base.Hurt(pvp, quiet, damage, hitDirection, crit, cooldownCounter);
        }
    }
}