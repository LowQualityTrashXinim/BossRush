using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
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
            if (Item.ModItem is BrokenArtifact)
            {
                return base.UseItem(player);
            }
            player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = Type;
            if (Item.ModItem is RandomArtifactChooser)
            {
                player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID = Main.rand.Next(new int[]
                {
                    ModContent.ItemType<TokenofGreed>(),
                    ModContent.ItemType<TokenofPride>(),
                    ModContent.ItemType<FateDecider>(),
                    ModContent.ItemType<HeartOfEarth>(),
                    ModContent.ItemType<NormalizeArtifact>(),
                    ModContent.ItemType<VampirismCrystal>(),
                    ModContent.ItemType<BootOfSpeedManipulation>()
                });
                //BossRushUtils.CombatTextRevamp(player.Hitbox, Main.DiscoColor, player.GetModPlayer<ArtifactPlayerHandleLogic>().ToStringArtifact());
                return true;
            }
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
    public class ArtifactPlayerHandleLogic : ModPlayer
    {
        public const int ArtifactDefaultID = 999;
        public int ArtifactDefinedID = ArtifactDefaultID;//setting to 999 mean it just do nothing
        public override void PreUpdate()
        {
            if (!ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                ArtifactDefinedID = ModContent.ItemType<NormalizeArtifact>();
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
                FateDeciderPlayer modplayer = player.GetModPlayer<FateDeciderPlayer>();
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