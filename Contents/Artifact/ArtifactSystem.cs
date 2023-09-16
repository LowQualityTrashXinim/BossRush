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
using BossRush.Common;
using BossRush.Contents.Projectiles;
using BossRush.Texture;

namespace BossRush.Contents.Artifact
{
    public abstract class ArtifactModItem : ModItem
    {
        public int width, height;
        public virtual void ArtifactSetDefault() { }
        public override void SetDefaults()
        {
            ArtifactSetDefault();
            Item.BossRushDefaultToConsume(width, height);
            Item.UseSound = SoundID.Zombie105;
        }
        protected virtual bool CanBeCraft => true;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ArtifactPlayerHandleLogic artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>();
            tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Only 1 artifact can be consume"));
            if (artifactplayer.ArtifactDefinedID != ArtifactPlayerHandleLogic.ArtifactDefaultID)
            {
                TooltipLine line = new TooltipLine(Mod, "ArtifactAlreadyConsumed", "You can't no longer consume anymore artifact");
                line.OverrideColor = Color.DarkRed;
                tooltips.Add(line);
            }
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = Type;
            //if (item.ModItem is RandomArtifactChooser)
            //{
            //    BossRushUtils.CombatTextRevamp(player.Hitbox, Main.DiscoColor, player.GetModPlayer<ArtifactPlayerHandleLogic>().ToStringArtifact());
            //}
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            ArtifactPlayerHandleLogic artifactplayer = player.GetModPlayer<ArtifactPlayerHandleLogic>();

            return artifactplayer.ArtifactDefinedID == ArtifactPlayerHandleLogic.ArtifactDefaultID;
        }
        public override void AddRecipes()
        {
            if (CanBeCraft)
            {
                CreateRecipe()
                    .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                    .Register();
            }
        }
    }

    //foreach (var itemSample in ContentSamples.ItemsByType)
    //{
    //    ModItem item = itemSample.Value.ModItem;
    //    if (item is IArtifactItem)
    //    {
    //        if (item is EternalWealth)
    //            continue;
    //        if (item is MagicalCardDeck && !ModContent.GetInstance<BossRushModConfig>().Nightmare)
    //            continue;
    //    }

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
    public abstract class ArtifactPlayerHandleLogic : ModPlayer
    {
        public const int ArtifactDefaultID = 999;
        public int ArtifactDefinedID = ArtifactDefaultID;//setting to 999 mean it just do nothing
        //ID = 1
        //ID = 2
        //ID = 3
        // ID = 4
        // ID = 5
        // ID = 6
        // ID = 7

        bool EternalWealth = false;

        int timer = 0;
        Vector2[] oldPos = new Vector2[5];
        int counterOldPos = 0;
        int MidasInfection = 0;

        protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
        public override void PreUpdate()
        {
            if (!ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                ArtifactDefinedID = 0;
            }
        }
        public override void PostUpdate()
        {
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
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.minion)
            {
                Player player = Main.player[projectile.owner];
                ArtifactPlayerHandleLogic modplayer = player.GetModPlayer<ArtifactPlayerHandleLogic>();
                if (modplayer.GoodBuffIndex == 4)
                {
                    if (Main.rand.NextBool(10))
                        player.Heal(Main.rand.Next(1, 5));
                }
                if (modplayer.BadBuffIndex == 4)
                {
                    if (Main.rand.NextBool(10))
                        Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center, (player.Center - projectile.Center).SafeNormalize(Vector2.Zero) * 5, ModContent.ProjectileType<NegativeLifeProjectile>(), 20, 0);
                }
            }
        }
    }
    class ArtifactSwordSlashProjectile : ModProjectile
    {
        public override string Texture => BossRushTexture.SMALLWHITEBALL;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 10;
        }
        public override void AI()
        {
            Projectile.alpha = (int)MathHelper.Lerp(0, 255, (50 - Projectile.timeLeft) / 50);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor, .02f);
            return base.PreDraw(ref lightColor);
        }
    }
}