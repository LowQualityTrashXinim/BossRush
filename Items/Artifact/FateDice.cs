using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Items.Chest;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using BossRush.Texture;
using System.Collections.Generic;

namespace BossRush.Items.Artifact
{
    internal class FateDice : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fate Decider");
            Tooltip.SetDefault("Increase amount weapon get drop by 4" +
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
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                .Register();
        }
    }
    class FateDicePlayer : ModPlayer
    {
        public bool FateDice;
        int RNGdecide;

        bool CanCrit;
        public override void PostUpdate()
        {
            if (!FateDice)
            {
                return;
            }
            switch (RNGdecide)
            {
                case 0:
                    Player.velocity = Vector2.Zero;
                    Player.AddBuff(BuffID.Frozen, 60);
                    break;
                case 1:
                    Player.AddBuff(BuffID.Weak, 300);
                    Player.AddBuff(BuffID.Slow, 300);
                    Player.AddBuff(BuffID.BrokenArmor, 300);
                    Player.AddBuff(BuffID.ManaSickness, 300);
                    Player.AddBuff(BuffID.Obstructed, 300);
                    break;
                case 2:
                    BossRushUtils.SpawnBoulderOnTopPlayer(Player,0, false);
                    break;
                case 3:
                    Player.AddBuff(BuffID.Confused, 3000);
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    Player.statLife += (int)Math.Clamp(Player.statLifeMax2 * .25f, 0, Player.statLifeMax2);
                    break;
                case 8:
                    CanCrit = true;
                    break;
                case 9:
                    ChestLootDrop.GetWeapon(out int Weapon, out int amount);
                    Item.NewItem(Player.GetSource_FromThis(), Player.Center + new Vector2(0,-900), Weapon, amount);
                    break;
                case 10:
                    BossRushUtils.LookForHostileNPC(Player.Center, out List<NPC> npc,2000);
                    for (int i = 0; i < npc.Count; i++)
                    {
                        npc[i].StrikeNPC(1000, 0, 0);
                    }
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;
                default:
                    break;
            }
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if(!FateDice)
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
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (FateDice & Main.rand.NextBool(10))
            {
                damage *= 2;
            }
            base.Hurt(pvp, quiet, damage, hitDirection, crit, cooldownCounter);
        }
    }
}