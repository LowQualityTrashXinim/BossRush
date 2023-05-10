using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Artifact;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.BuffAndDebuff;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Common.Global
{
    internal class ArtifactSystem : ModSystem
    {
        public override void AddRecipes()
        {
            ArtifactRecipe();
        }
        private static void ArtifactRecipe()
        {
            foreach (var itemSample in ContentSamples.ItemsByType)
            {
                ModItem item = itemSample.Value.ModItem;
                if (item is IArtifactItem)
                {
                    if (item is SkillIssuedArtifact)
                    {
                        item.CreateRecipe()
                        .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                        .AddIngredient(ModContent.ItemType<PowerEnergy>())
                        .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                        .AddIngredient(ModContent.ItemType<WoodenTreasureChest>())
                        .Register();
                        continue;
                    }
                    item.CreateRecipe()
                        .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                        .Register();
                }
            }
        }
    }
    class ArtifactGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    return player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID < 1;
                }
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Only 1 artifact can be consume"));
                }
                if (Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID != 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "ArtifactAlreadyConsumed", "You can't no longer consume anymore artifact");
                    line.OverrideColor = Color.DarkRed;
                    tooltips.Add(line);
                }
            }
        }
    }
    class ArtifactPlayerHandleLogic : ModPlayer
    {
        public int ArtifactDefinedID = 0;
        bool Greed = false;//ID = 1
        bool Pride = false;//ID = 2
        bool Vampire = false;//ID = 3
        bool Earth = false;
        int EarthCD = 0;
        int VampireMultiplier = 0;
        int EarthMultiplier = 0;
        public override void PreUpdate()
        {
            VampireMultiplier = 0;
            EarthMultiplier = 0;
            Greed = false;
            Pride = false;
            Vampire = false;
            Earth = false;
            switch (ArtifactDefinedID)
            {
                case 1:
                    Greed = true;
                    break;
                case 2:
                    Pride = true;
                    break;
                case 3:
                    VampireMultiplier = 1;
                    Vampire = true;
                    break;
                case 4:
                    Earth = true;
                    EarthMultiplier = 1;
                    break;
            }
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            if (Vampire)
            {
                health.Base = -(Player.statLifeMax * 0.65f);
            }
            if(Earth)
            {
                health.Base = 100 + Player.statLifeMax * 2;
            }
        }
        public override void PostUpdate()
        {
            if (Greed)
            {
                Player.GetModPlayer<ChestLootDropPlayer>().amountModifier += 4;
            }
            if (Pride)
            {
                Player.GetModPlayer<ChestLootDropPlayer>().multiplier = true;
                Player.GetModPlayer<ChestLootDropPlayer>().amountModifier = .5f;
            }
            if (Vampire)
            {
            }
            if (Earth)
            {
                bool isOnCoolDown = EarthCD > 0;
                EarthCD -= isOnCoolDown ? 1 : 0;
                if (isOnCoolDown)
                {
                    int dust = Dust.NewDust(Player.Center, 0, 0, DustID.Blood);
                    Main.dust[dust].velocity = -Vector2.UnitY * 2f + Main.rand.NextVector2Circular(2, 2);
                }
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Greed)
            {
                damage *= .65f;
            }
            if (Pride)
            {
                damage += .45f;
            }
        }
        int countRange = 0;
        private void LifeSteal(NPC target, int rangeMin = 1, int rangeMax = 3, float multiplier = 1)
        {
            if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy)
            {
                int HP = (int)(Main.rand.Next(rangeMin, rangeMax) * multiplier);
                int HPsimulation = Player.statLife + HP;
                if (HPsimulation < Player.statLifeMax2)
                {
                    Player.Heal(HP);
                }
                else
                {
                    Player.statLife = Player.statLifeMax2;
                }
            }
        }
        public override bool CanUseItem(Item item)
        {
            if (Earth)
            {
                return EarthCD <= 0;
            }
            return base.CanUseItem(item);
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Earth)
            {
                EarthCD = 300;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Vampire)
            {
                LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Vampire)
            {
                if (proj.DamageType == DamageClass.Melee)
                {
                    LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
                    return;
                }
                countRange++;
                if (countRange >= 3)
                {
                    LifeSteal(target);
                    countRange = 0;
                }
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (!Player.HasBuff(ModContent.BuffType<SecondChance>()) && Vampire)
            {
                Player.Heal(Player.statLifeMax2);
                Player.AddBuff(ModContent.BuffType<SecondChance>(), 10800);
                return false;
            }
            return true;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.ArtifactRegister);
            packet.Write((byte)Player.whoAmI);
            packet.Write(ArtifactDefinedID);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["ArtifactDefinedID"] = ArtifactDefinedID;
        }
        public override void LoadData(TagCompound tag)
        {
            ArtifactDefinedID = (int)tag["ArtifactDefinedID"];
        }
    }
}