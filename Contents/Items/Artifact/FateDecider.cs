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
using Terraria.WorldBuilding;

namespace BossRush.Contents.Items.Artifact
{
    internal class FateDecider : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fate Decider");
            /* Tooltip.SetDefault("\"Replacement of god dice\"" +
                "\nOn Equip : Increase amount weapon get drop from Treasure Chest by 4" +
                "\nEffect : Random effect may occur ..."); */
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
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
        int RNGdecide = -1;
        int effectlasting = 0;

        bool CanCrit = false;
        bool CanBeHit = true;
        bool NPCdeal5TimeDamage = false;
        bool UnableToUseWeapon = false;
        bool ExtendingEffect = false;
        int OPEFFECT = 0;
        public override void ResetEffects()
        {
            FateDice = false;
        }
        public override void PostUpdate()
        {
            effectlasting -= effectlasting > 0 ? 1 : 0;
            if (effectlasting <= 0)
            {
                RNGdecide = -1;
                CanCrit = false;
                CanBeHit = true;
                NPCdeal5TimeDamage = false;
                UnableToUseWeapon = false;
                ExtendingEffect = false;
                OPEFFECT = 0;
            }
            if (!FateDice)
            {
                return;
            }
            if (effectlasting <= 0 && Main.rand.NextBool(600))
            {
                RNGdecide = Main.rand.Next(17);
            }
            SpamTextToScare();
            switch (RNGdecide)
            {
                case 0:
                    Player.velocity = Vector2.Zero;
                    Player.AddBuff(BuffID.Frozen, 60);
                    effectlasting = effectlasting > 0 ? effectlasting : 5;
                    break;
                case 1:
                    Player.AddBuff(BuffID.Weak, 300);
                    Player.AddBuff(BuffID.Slow, 300);
                    Player.AddBuff(BuffID.BrokenArmor, 300);
                    Player.AddBuff(BuffID.ManaSickness, 300);
                    Player.AddBuff(BuffID.Obstructed, 300);
                    effectlasting = effectlasting > 0 ? effectlasting : 30;
                    break;
                case 2:
                    if (effectlasting % 50 == 0)
                    {
                        BossRushUtils.SpawnBoulderOnTopPlayer(Player, 0, false);
                    }
                    if (effectlasting % 20 == 0)
                    {
                        BossRushUtils.SpawnHostileProjectileDirectlyOnPlayer(Player, 1000, 50, true, Vector2.Zero, ProjectileID.HellfireArrow, 100, 10f);
                    }
                    effectlasting = effectlasting > 0 ? effectlasting : 900;
                    break;
                case 3:
                    Player.AddBuff(BuffID.Confused, 600);
                    effectlasting = effectlasting > 0 ? effectlasting : 40;
                    break;
                case 4:
                    NPCdeal5TimeDamage = true;
                    effectlasting = effectlasting > 0 ? effectlasting : 210;
                    break;
                case 5:
                    if (effectlasting % 4 == 0)
                    {
                        NPC.SpawnOnPlayer(Player.whoAmI, Main.rand.Next(TerrariaArrayID.BAT));
                    }
                    effectlasting = effectlasting > 0 ? effectlasting : 100;
                    break;
                case 6:
                    UnableToUseWeapon = true;
                    int dust = Dust.NewDust(Player.Center, 0, 0, DustID.Blood);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                    effectlasting = effectlasting > 0 ? effectlasting : 120;
                    break;
                case 7:
                    for (int i = 0; i < Player.CountBuffs(); i++)
                    {
                        Player.DelBuff(i);
                    }
                    effectlasting = effectlasting > 0 ? effectlasting : 1;
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
                    effectlasting = effectlasting > 0 ? effectlasting : 100;
                    break;
                case 9:
                    Effect9();
                    effectlasting = effectlasting > 0 ? effectlasting : 1;
                    break;
                case 10:
                    Effect10();
                    effectlasting = effectlasting > 0 ? effectlasting : 1;
                    break;
                case 11:
                    Effect11();
                    effectlasting = effectlasting > 0 ? effectlasting : 300;
                    break;
                case 12:
                    Effect12();
                    effectlasting = effectlasting > 0 ? effectlasting : 1;
                    break;
                case 13:
                    Effect13();
                    effectlasting = effectlasting > 0 ? effectlasting : 180;
                    break;
                case 14:
                    Effect14();
                    effectlasting = effectlasting > 0 ? effectlasting : 60;
                    break;
                case 15:
                    effectlasting = effectlasting > 0 ? effectlasting : 30;
                    if (OPEFFECT == 0)
                    {
                        Effect9();
                        Effect10();
                        Effect11();
                        Effect12();
                        Effect13();
                        Effect14();
                        Effect16(effectlasting);
                        OPEFFECT = 10;
                    }
                    OPEFFECT -= OPEFFECT > 0 ? 1 : 0;
                    break;
                case 16:
                    effectlasting = effectlasting > 0 ? effectlasting : 600;
                    Effect16(effectlasting);
                    break;
                default:
                    break;
            }
            if (Main.rand.NextBool(300) && !ExtendingEffect)
            {
                ExtendingEffect = true;
                effectlasting *= 5;
            }
        }
        private void SpamTextToScare()
        {
            if (Main.rand.NextBool(750))
            {
                int RandomTextChooser = Main.rand.Next(11);
                Color color = Color.White;
                string TextString;
                switch (RandomTextChooser)
                {
                    case 0:
                        TextString = "You feel an evil presence watching you...";
                        color = new Color(50, 255, 130);
                        break;
                    case 1:
                        TextString = "You feel vibrations from deep below...";
                        color = new Color(50, 255, 130);
                        break;
                    case 2:
                        TextString = "This is going to be a terrible night...";
                        color = new Color(50, 255, 130);
                        break;
                    case 3:
                        TextString = "The air is getting colder around you...";
                        color = new Color(50, 255, 130);
                        break;
                    case 4:
                        TextString = "Pirates are approaching from the west!";
                        color = new Color(175, 75, 255);
                        break;
                    case 5:
                        TextString = RandomName() + " has joined.";
                        break;
                    case 6:
                        TextString = RandomNameExit() + " has left";
                        break;
                    case 7:
                        TextString = "The weather forecast that there will be boulder rain soon !";
                        color = new Color(10, 10, 255);
                        break;
                    case 8:
                        TextString = "I see you, we gonna be together forever, forever chopping tree " + $"[i:{ItemID.LucyTheAxe}]" + $"[i:{ItemID.Heart}]";
                        color = new Color(100, 0, 0);
                        break;
                    case 9:
                        TextString = "Rare life form detected that there is a furry near by !";
                        color = new Color(10, 10, 255);
                        break;
                    case 10:
                        TextString = "To give yourself a zenith, please write in chat : \\Player::give[i:Zenith][1]";
                        color = new Color(10, 10, 255);
                        break;
                    default:
                        TextString = "The weather forecast that there will be boulder rain soon";
                        color = new Color(10, 10, 255);
                        break;
                }
                Main.NewText(TextString, color);
            }
        }
        private string RandomName()
        {
            switch (Main.rand.Next(10))
            {
                case 0:
                    return "God";
                case 1:
                    return Player.name;
                case 2:
                    return "Your mom";
                case 3:
                    return "Red";
                case 4:
                    return "skillissue";
                case 5:
                    return "FBI";
                case 6:
                    return "Guide";
                case 7:
                    return "Sans";
                case 8:
                    return "LQTXinim";
                case 9:
                    return "drugaddict";
                default:
                    return "asgfgfagasdf";
            }
        }
        private string RandomNameExit()
        {
            switch (Main.rand.Next(10))
            {
                case 0:
                    return "Your lover";
                case 1:
                    return Player.name;
                case 2:
                    return "Your father";
                case 3:
                    return "Anime";
                case 4:
                    return "Luck";
                case 5:
                    return "LQTMinix";
                case 6:
                    return "Ninja";
                case 7:
                    return "Sans";
                case 8:
                    return "FeelingLucky";
                case 9:
                    return "ImNotGud";
                default:
                    return "asgfgfagasdf";
            }
        }
        private void Effect9()
        {
            ChestLootDrop.GetWeapon(out int Weapon, out int amount);
            Player.QuickSpawnItem(null, Weapon, amount);
        }
        private void Effect10()
        {
            Player.Center.LookForHostileNPC(out List<NPC> npc, 1000);
            for (int i = 0; i < npc.Count; i++)
            {
                npc[i].StrikeNPC(npc[i].CalculateHitInfo(1000,0));
                NetMessage.SendStrikeNPC(npc[i], npc[i].CalculateHitInfo(1000, 0));
            }
        }
        private void Effect11()
        {
            Vector2 position = new Vector2(Player.Center.X + Main.rand.NextFloat(-1000, 1000), Player.Center.Y - 1000);
            Vector2 velocity = new Vector2(0, 20);
            Projectile.NewProjectile(Player.GetSource_FromThis(), position, velocity, ProjectileID.StarWrath, Player.HeldItem.damage * 3, 10, Player.whoAmI);
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
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (!FateDice)
            {
                return;
            }
            if (CanCrit)
            {
                hit.Crit = true;
                hit.Damage *= 5;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (!FateDice)
            {
                return;
            }
            if (CanCrit)
            {
                hit.Crit = true;
                hit.Damage *= 5;
            }
        }
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (!FateDice)
            {
                return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
            }
            return CanBeHit;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (FateDice & NPCdeal5TimeDamage)
            {
                info.Damage *= 5;
            }
        }
    }
}