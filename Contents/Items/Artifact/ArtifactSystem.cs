using System;
using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.BuffAndDebuff;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Common.Utils;
using BossRush.Common;
using BossRush.Contents.Items.Toggle;
using BossRush.Contents.Items.NohitReward;

namespace BossRush.Contents.Items.Artifact
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
                        .AddIngredient(ModContent.ItemType<WoodenLootBox>())
                        .Register();
                        continue;
                    }
                    if (item is BrokenArtifact || item is EternalWealth)
                        continue;
                    if (item is MagicalCardDeck && !ModContent.GetInstance<BossRushModConfig>().Nightmare)
                        continue;
                    item.CreateRecipe()
                        .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                        .Register();
                }
            }
        }
    }
    class ArtifactGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            if (entity.ModItem is IArtifactItem)
            {
                entity.UseSound = SoundID.Zombie105;
            }
        }
        public override bool? UseItem(Item item, Player player)
        {
            if (item.ModItem is IArtifactItem moditem)
            {
                player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = moditem.ArtifactID;
                if (item.ModItem is RandomArtifactChooser)
                {
                    BossRushUtils.CombatTextRevamp(player.Hitbox, Main.DiscoColor, player.GetModPlayer<ArtifactPlayerHandleLogic>().ToStringArtifact());
                }
                return true;
            }
            return base.UseItem(item, player);
        }
        public override bool CanUseItem(Item item, Player player)
        {
            ArtifactPlayerHandleLogic artifactplayer = player.GetModPlayer<ArtifactPlayerHandleLogic>();
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    return artifactplayer.ArtifactDefinedID == ArtifactPlayerHandleLogic.ArtifactDefaultID;
                }
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            ArtifactPlayerHandleLogic artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>();
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Only 1 artifact can be consume"));
                }
                if (artifactplayer.ArtifactDefinedID != ArtifactPlayerHandleLogic.ArtifactDefaultID)
                {
                    TooltipLine line = new TooltipLine(Mod, "ArtifactAlreadyConsumed", "You can't no longer consume anymore artifact");
                    line.OverrideColor = Color.DarkRed;
                    tooltips.Add(line);
                }
            }
        }
    }
    class ArtifactItemID
    {
        public const short TokenOfGreed = 1;
        public const short TokenOfPride = 2;
        public const short VampirismCrystal = 3;
        public const short HeartOfEarth = 4;
        public const short FateDecider = 5;
        public const short BootOfSpeedManipulation = 6;
        public const short MagicalCardDeck = 7;

        public const short EternalWealth = 998;
    }
    class ArtifactPlayerHandleLogic : ModPlayer
    {
        public const int ArtifactDefaultID = 999;
        public int ArtifactDefinedID = ArtifactDefaultID;//setting to 999 mean it just do nothing
        bool Greed = false;//ID = 1
        bool Pride = false;//ID = 2
        bool Vampire = false;//ID = 3
        bool Earth = false;// ID = 4
        bool FateDice = false;// ID = 5
        bool BootofSpeed = false;// ID = 6
        public bool MagicalCardDeck = false;// ID = 7
        int EarthCD = 0;
        public bool EternalWealth = false;

        int timer = 0;
        Vector2[] oldPos = new Vector2[5];
        int counterOldPos = 0;
        int MidasInfection = 0;

        bool IsBuffCurrentlyActive = false;
        int GoodBuffIndex = -1;
        int BadBuffIndex = -1;
        public string ToStringArtifact()
        {
            switch (ArtifactDefinedID)
            {
                case 1:
                    return "Token of Greed";
                case 2:
                    return "Token of Pride";
                case 3:
                    return "Vampirism Crystal";
                case 4:
                    return "Heart of earth";
                case 5:
                    return "Fate Decider";
                case 6:
                    return "Boot of speed manipulation";
                case 7:
                    return "Magical card deck";
                default:
                    return "no artifact active";
            }
        }
        ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
        public override void PreUpdate()
        {
            if (!ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                ArtifactDefinedID = 0;
            }
            Greed = false;
            Pride = false;
            Vampire = false;
            Earth = false;
            FateDice = false;
            BootofSpeed = false;
            MagicalCardDeck = false;
            switch (ArtifactDefinedID)
            {
                case ArtifactItemID.TokenOfGreed:
                    Greed = true;
                    break;
                case ArtifactItemID.TokenOfPride:
                    Pride = true;
                    break;
                case ArtifactItemID.VampirismCrystal:
                    Vampire = true;
                    break;
                case ArtifactItemID.HeartOfEarth:
                    Earth = true;
                    break;
                case ArtifactItemID.FateDecider:
                    FateDice = true;
                    break;
                case ArtifactItemID.BootOfSpeedManipulation:
                    BootofSpeed = true;
                    break;
                case ArtifactItemID.MagicalCardDeck:
                    MagicalCardDeck = true;
                    break;
                case ArtifactItemID.EternalWealth:
                    EternalWealth = true;
                    break;
            }
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            if (Vampire)
            {
                health.Base = -(Player.statLifeMax * 0.55f);
            }
            if (Earth)
            {
                health.Base = 100 + Player.statLifeMax * 2;
            }
        }
        public override void ResetEffects()
        {
            if (BootofSpeed)
            {
                Player.moveSpeed += 1f;
                Player.maxFallSpeed += 2f;
                Player.runAcceleration += .5f;
                Player.jumpSpeed += 3f;
                Player.noFallDmg = true;
            }
        }
        public override void PostUpdate()
        {
            if (Vampire)
            {
                Player.AddBuff(BuffID.PotionSickness, 600);
            }
            if (Pride)
            {
                chestmodplayer.finalMultiplier -= .5f;
            }
            if (Greed)
            {
                chestmodplayer.amountModifier += 4;
            }
            if (EternalWealth)
            {
                chestmodplayer.finalMultiplier += 2;
                timer = BossRushUtils.CoolDown(timer);
                if (timer <= 0)
                {
                    if (counterOldPos >= oldPos.Length - 1)
                    {
                        counterOldPos = 0;
                    }
                    else
                    {
                        counterOldPos++;
                    }
                    oldPos[counterOldPos] = Player.Center;
                    timer = 600;
                }
                float distance = 500;
                bool IsInField = false;
                foreach (Vector2 vec in oldPos)
                {
                    if (Player.Center.IsCloseToPosition(vec, distance))
                    {
                        IsInField = true;
                        MidasInfection++;
                        if (MidasInfection >= 180)
                            Player.statLife = Math.Clamp(Player.statLife - 1, 1, Player.statLifeMax2);
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        int dust = Dust.NewDust(vec + Main.rand.NextVector2Circular(distance, distance), 0, 0, DustID.GoldCoin);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        int dust = Dust.NewDust(vec + Main.rand.NextVector2CircularEdge(distance, distance), 0, 0, DustID.GoldCoin);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
                    }
                }
                if (!IsInField)
                    MidasInfection = BossRushUtils.CoolDown(MidasInfection);
            }
            if (Earth)
            {
                bool isOnCoolDown = EarthCD > 0;
                EarthCD -= isOnCoolDown ? 1 : 0;
                if (isOnCoolDown)
                {
                    int dust = Dust.NewDust(Player.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.Blood);
                    Main.dust[dust].velocity = -Vector2.UnitY * 2f;
                }
            }
            if (BootofSpeed)
            {
                Player.wingTime *= 0;
                Player.wingAccRunSpeed *= 0;
                Player.wingRunAccelerationMult *= 0;
                Player.wingTimeMax = 0;
            }
            if (FateDice)
            {
                FateDeciderEffect();
            }
        }
        bool ArrowBuff, SwordProjectileBuff, BulletBuff, MageBuff, SummonerBuff;
        bool ArrowDebuff, SwordProjectileDebuff, BulletDeBuff, MageDebuff, SummonerDebuff;
        private void FateDeciderEffect()
        {
            if (Player.HasBuff(ModContent.BuffType<FateDeciderBuff>()))
            {
                IsBuffCurrentlyActive = true;
            }
            else
            {
                Player.AddBuff(ModContent.BuffType<FateDeciderBuff>(), 18000);
                IsBuffCurrentlyActive = false;
            }
            if (IsBuffCurrentlyActive)
            {
                if (GoodBuffIndex == -1)
                    GoodBuffIndex = Main.rand.Next(0, 3);
                if (BadBuffIndex == -1)
                    do
                    {
                        BadBuffIndex = Main.rand.Next(0, 3);
                    }
                    while (BadBuffIndex == GoodBuffIndex);
            }
            else
            {
                GoodBuffIndex = -1;
                BadBuffIndex = -1;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (BootofSpeed)
                if (Player.velocity.IsLimitReached(5))
                    damage *= Main.rand.NextFloat(.3f, 1f);
            if (Greed)
                damage *= .65f;
            if (Pride)
            {
                float reward = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count * .1f;
                damage += .45f + reward;
            }
        }
        int vampirecountRange = 0;
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
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.useAmmo == AmmoID.Arrow)
            {
                if (BadBuffIndex == 0)
                {
                    type = ProjectileID.WoodenArrowFriendly;
                    velocity *= .5f;
                    damage = (int)(damage * .5f);
                }
            }
            if (item.useAmmo == AmmoID.Bullet)
            {
                if (BadBuffIndex == 1)
                {
                    velocity = velocity.NextVector2RotatedByRandom(360);
                    position = position.PositionOFFSET(velocity, Main.rand.NextFloat(-500, 500));
                }
            }
            if (!item.noMelee && !item.noUseGraphic && item.DamageType == DamageClass.Melee)
            {
                if (BadBuffIndex == 2)
                {
                    velocity = -velocity;
                    position = Player.Center + Player.Center - Main.MouseWorld;
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.useAmmo == AmmoID.Arrow)
            {
                if (GoodBuffIndex == 0)
                {
                    velocity *= 2;
                    int proj = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), type, damage, knockback, Player.whoAmI);
                    if (Main.projectile[proj].ModProjectile is null)
                    {
                        Main.projectile[proj].aiStyle = -1;
                        Main.projectile[proj].ai[0] = -1;
                        Main.projectile[proj].ai[1] = AmmoID.Arrow;
                    }
                }
            }
            if (item.useAmmo == AmmoID.Bullet)
            {
                if (GoodBuffIndex == 1)
                {
                    velocity *= .25f;
                    int proj = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), type, damage, knockback, Player.whoAmI);
                    if (Main.projectile[proj].ModProjectile is null)
                    {
                        Main.projectile[proj].aiStyle = -1;
                        Main.projectile[proj].ai[0] = -1;
                        Main.projectile[proj].ai[1] = AmmoID.Bullet;
                    }
                }
            }
            if (!item.noMelee && !item.noUseGraphic && item.DamageType == DamageClass.Melee)
            {
                if (GoodBuffIndex == 2)
                {
                    int proj = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), type, damage, knockback, Player.whoAmI);
                    if (Main.projectile[proj].ModProjectile is null)
                    {
                        Main.projectile[proj].penetrate = 1;
                    }
                }
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
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
                vampirecountRange++;
                if (vampirecountRange >= 3)
                {
                    LifeSteal(target, 1, 5);
                    vampirecountRange = 0;
                }
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (!Player.HasBuff(ModContent.BuffType<SecondChance>()) && Vampire)
            {
                Player.Heal(Player.statLifeMax2);
                Player.AddBuff(ModContent.BuffType<SecondChance>(), 18000);
                return false;
            }
            return true;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRush.MessageType.ArtifactRegister);
            packet.Write((byte)Player.whoAmI);
            packet.Write(ArtifactDefinedID);
            packet.Send(toWho, fromWho);
        }
        public override void Initialize()
        {
            ArtifactDefinedID = ArtifactDefaultID;
        }
        public override void SaveData(TagCompound tag)
        {
            tag["ArtifactDefinedID"] = ArtifactDefinedID;
        }
        public override void LoadData(TagCompound tag)
        {
            ArtifactDefinedID = tag.GetInt("ArtifactDefinedID");
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            ArtifactDefinedID = reader.ReadInt32();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            ArtifactPlayerHandleLogic clone = (ArtifactPlayerHandleLogic)targetCopy;
            clone.ArtifactDefinedID = ArtifactDefinedID;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            ArtifactPlayerHandleLogic clone = (ArtifactPlayerHandleLogic)clientPlayer;
            if (ArtifactDefinedID != clone.ArtifactDefinedID) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
    public class ArtifactGlobalProjectile : GlobalProjectile
    {
        //projectile.ai[2] will act as a timer
        public override void PostAI(Projectile projectile)
        {
            if (projectile.aiStyle == -1 && projectile.ai[0] == -1)
            {
                if (projectile.ai[1] == AmmoID.Arrow)
                {
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    if (projectile.Center.LookForHostileNPC(out NPC npc, 600))
                    {
                        Vector2 distance = npc.Center - projectile.Center;
                        float length = distance.Length();
                        if (length > 5)
                        {
                            length = 5;
                        }
                        projectile.velocity -= projectile.velocity * .08f;
                        projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
                        projectile.velocity = projectile.velocity.LimitedVelocity(20);
                        return;
                    }
                }
                if (projectile.ai[1] == AmmoID.Bullet)
                {
                    projectile.velocity += projectile.velocity * .02f;
                    projectile.ai[2]++;
                    if (projectile.ai[2] % 10 == 0)
                        projectile.damage += 1;
                }
            }
        }
    }
}