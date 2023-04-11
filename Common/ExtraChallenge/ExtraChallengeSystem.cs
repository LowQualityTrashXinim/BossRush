using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace BossRush.Common.ExtraChallenge
{
    internal class ExtraChallengeSystem : ModSystem
    {
        public bool BoulderRain;
        public bool Hellfirerain;
        public bool Closecombatfight;
        public bool HostileProjectileOnTop;
        int count = 0;
        int NumberToCompare = 0;
        public const int BoulderRainCooldown = 60;
        public int BoulderRainCount = 0;
        public const int HellFireRainCoolDown = 30;
        public int HellFireRainCount = 0;
        public const int DeadFromAboveCoolDown = 45;
        public int DeadFromAboveCount = 0;
        public override void PostUpdateWorld()
        {
            if (!ModContent.GetInstance<BossRushModConfig>().ExtraChallenge)
            {
                return;
            }
            Player player = Main.LocalPlayer;
            if (NumberToCompare != player.GetModPlayer<ExtraChallengePlayer>().BossSlayedCount)
            {
                count--;
                HostileProjectileOnTop = false;
                player.GetModPlayer<ExtraChallengePlayer>().OnlyUseOneClass = false;
                player.GetModPlayer<ExtraChallengePlayer>().spawnRatex3 = false;
                BoulderRain = false;
                player.GetModPlayer<ExtraChallengePlayer>().strongerEnemy = false;
                Hellfirerain = false;
                player.GetModPlayer<ExtraChallengePlayer>().Badbuff = false;
                Closecombatfight = false;
                player.GetModPlayer<ExtraChallengePlayer>().BatJungleANDCave = false;
            }
            if (count == 0)
            {
                switch (player.GetModPlayer<ExtraChallengePlayer>().ChallengeChooser)
                {
                    case 1:
                        HostileProjectileOnTop = true;
                        Main.NewText("Death from above", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Death from above"), Colors.RarityDarkRed);
                        break;
                    case 2:
                        player.GetModPlayer<ExtraChallengePlayer>().ClassChooser = Main.rand.Next(4);
                        player.GetModPlayer<ExtraChallengePlayer>().OnlyUseOneClass = true;
                        Main.NewText("Restrict to 1 class", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Restrict to 1 class"), Colors.RarityDarkRed);
                        break;
                    case 3:
                        player.GetModPlayer<ExtraChallengePlayer>().spawnRatex3 = true;
                        Main.NewText("Increase spawn rate", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Increase spawn rate"), Colors.RarityDarkRed);
                        break;
                    case 4:
                        BoulderRain = true;//done ?
                        Main.NewText("Boulder rain time", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Boulder rain time"), Colors.RarityDarkRed);
                        break;
                    case 5:
                        player.GetModPlayer<ExtraChallengePlayer>().strongerEnemy = true;
                        Main.NewText("Enemy get stronger", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Enemy get stronger"), Colors.RarityDarkRed);
                        break;
                    case 6:
                        Hellfirerain = true; //done
                        Main.NewText("Hell fire arrow rain", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Hell fire arrow rain"), Colors.RarityDarkRed);
                        break;
                    case 7:
                        player.GetModPlayer<ExtraChallengePlayer>().Badbuff = true;
                        Main.NewText("Very nasty debuff", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Very nasty debuff"), Colors.RarityDarkRed);
                        break;
                    case 8:
                        player.GetModPlayer<ExtraChallengePlayer>().BatJungleANDCave = true;
                        Main.NewText("Annoying bat start to spawn", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Bat start to spawn"), Colors.RarityDarkRed);
                        break;
                }
                NumberToCompare = player.GetModPlayer<ExtraChallengePlayer>().BossSlayedCount;
                count++;
            }
            //Custom stuff
            FallingProjectileHandle(player);
        }
        private void FallingProjectileHandle(Player player)
        {
            if (!player.active || player.dead)
            {
                return;
            }
            Boulder(player);
            HellFireRain(player);
            CannonBallFalling(player);
        }
        private void Boulder(Player player)
        {
            if (!BoulderRain)
            {
                return;
            }
            if (BoulderRainCooldown == BoulderRainCount)
            {
                BossRushUtils.SpawnBoulderOnTopPlayer(player, 1000);
                BoulderRainCount = 0;
            }
            else
            {
                BoulderRainCount++;
            }
        }
        private void HellFireRain(Player player)
        {
            if (!Hellfirerain)
            {
                return;
            }
            if (HellFireRainCount == HellFireRainCoolDown)
            {
                for (int i = 0; i < 4; i++)
                {
                    BossRushUtils.SpawnHostileProjectileDirectlyOnPlayer(player, 1500, 0, true, Vector2.Zero, ProjectileID.HellfireArrow, 100, 1f);
                    HellFireRainCount = 0;
                }
            }
            else
            {
                HellFireRainCount++;
            }
        }
        private void CannonBallFalling(Player player)
        {
            if (!HostileProjectileOnTop)
            {
                return;
            }
            if (DeadFromAboveCoolDown == DeadFromAboveCount)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 spawn = new Vector2(player.Center.X, -1000 + player.Center.Y);
                    int projectile = Projectile.NewProjectile(null, spawn, Vector2.Zero, ProjectileID.CannonballHostile, 10000, 1f, Main.myPlayer, 0f, 0f);
                    Main.projectile[projectile].hostile = true;
                    Main.projectile[projectile].friendly = false;
                    Main.projectile[projectile].tileCollide = false;
                    Main.projectile[projectile].timeLeft = 200;
                    Main.projectile[projectile].light = 1f;
                    DeadFromAboveCount = 0;
                }
            }
            else
            {
                DeadFromAboveCount++;
            }
        }
    }
}
