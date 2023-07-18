using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Common.ExtraChallenge
{
    internal class ExtraChallengeSystem : ModSystem
    {
        int count = 0;
        int NumberToCompare = 0;
        public const int BoulderRainCooldown = 45;
        public int BoulderRainCount = 0;
        public const int HellFireRainCoolDown = 25;
        public int HellFireRainCount = 0;
        public const int DeadFromAboveCoolDown = 90;
        public int DeadFromAboveCount = 0;
        public override void PostUpdateWorld()
        {
            if (!ModContent.GetInstance<BossRushModConfig>().ExtraChallenge)
            {
                return;
            }
            Player player = Main.LocalPlayer;
            ExtraChallengePlayer modplayer = player.GetModPlayer<ExtraChallengePlayer>();
            if (NumberToCompare != modplayer.BossSlayedCount)
            {
                count--;
                modplayer.HostileProjectileOnTop = false;
                modplayer.OnlyUseOneClass = false;
                modplayer.spawnRatex3 = false;
                modplayer.BoulderRain = false;
                modplayer.strongerEnemy = false;
                modplayer.Hellfirerain = false;
                modplayer.Badbuff = false;
                modplayer.Closecombatfight = false;
                modplayer.BatJungleANDCave = false;
            }
            if (count == 0)
            {
                switch (modplayer.ChallengeChooser)
                {
                    case 1:
                        modplayer.HostileProjectileOnTop = true;
                        Main.NewText("Death from above", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Death from above"), Colors.RarityDarkRed);
                        break;
                    case 2:
                        modplayer.ClassChooser = Main.rand.Next(4);
                        modplayer.OnlyUseOneClass = true;
                        Main.NewText("Restrict to 1 class", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Restrict to 1 class"), Colors.RarityDarkRed);
                        break;
                    case 3:
                        modplayer.spawnRatex3 = true;
                        Main.NewText("Increase spawn rate", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Increase spawn rate"), Colors.RarityDarkRed);
                        break;
                    case 4:
                        modplayer.BoulderRain = true;//done ?
                        Main.NewText("Boulder rain time", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Boulder rain time"), Colors.RarityDarkRed);
                        break;
                    case 5:
                        modplayer.strongerEnemy = true;
                        Main.NewText("Enemy get stronger", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Enemy get stronger"), Colors.RarityDarkRed);
                        break;
                    case 6:
                        modplayer.Hellfirerain = true; //done
                        Main.NewText("Hell fire arrow rain", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Hell fire arrow rain"), Colors.RarityDarkRed);
                        break;
                    case 7:
                        modplayer.Badbuff = true;
                        Main.NewText("Very nasty debuff", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Very nasty debuff"), Colors.RarityDarkRed);
                        break;
                    case 8:
                        modplayer.BatJungleANDCave = true;
                        Main.NewText("Annoying bat start to spawn", Colors.RarityDarkRed);
                        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Bat start to spawn"), Colors.RarityDarkRed);
                        break;
                    case 9:
                        modplayer.InvertShoot = true;
                        Main.NewText("Where are you aiming at ?", Colors.RarityDarkRed);
                        break;
                    default:
                        if (modplayer.ChallengeChooser > 8)
                        {
                            modplayer.ChallengeChooser = 0;
                            Main.NewText("We run into a problem where we run out of challenge, are you using a debug item ? For now we gonna reset it back to 0", Colors.RarityDarkRed);
                            break;
                        }
                        Main.NewText("the dev is appear to be too lazy to implement a fix for this, grab Xinim and screenshot this message and explain how you encounter this ");
                        break;
                }
                NumberToCompare = modplayer.BossSlayedCount;
                count++;
            }
            //Custom stuff
            FallingProjectileHandle(modplayer, player);
        }
        private void FallingProjectileHandle(ExtraChallengePlayer modplayer, Player player)
        {
            if (!player.active || player.dead)
            {
                return;
            }
            Boulder(modplayer, player);
            HellFireRain(modplayer, player);
            CannonBallFalling(modplayer, player);
        }
        private void Boulder(ExtraChallengePlayer modplayer, Player player)
        {
            if (!modplayer.BoulderRain)
            {
                return;
            }
            if (BoulderRainCount >= BoulderRainCooldown)
            {
                BossRushUtils.SpawnBoulderOnTopPlayer(player, 1000);
                BoulderRainCount = 0;
            }
            else
            {
                BoulderRainCount++;
            }
        }
        private void HellFireRain(ExtraChallengePlayer modplayer, Player player)
        {
            if (!modplayer.Hellfirerain)
            {
                return;
            }
            if (HellFireRainCount >= HellFireRainCoolDown)
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
        private void CannonBallFalling(ExtraChallengePlayer modplayer, Player player)
        {
            if (!modplayer.HostileProjectileOnTop)
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
