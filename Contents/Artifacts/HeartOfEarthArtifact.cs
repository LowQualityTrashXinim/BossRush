using BossRush.Common.Systems.ArtifactSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Artifacts
{
    internal class HeartOfEarthArtifact : Artifact
    {
        public override float Scale => 0.7f;
    }

    class HeartOfEarthPlayer : ModPlayer
    {
        bool Earth = false;
        public override void ResetEffects()
        {
            Earth = Player.ActiveArtifact() == Artifact.ArtifactType<HeartOfEarthArtifact>();
        }

        int EarthCD = 0;
        int ShortStanding = 0;
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            if (Earth)
            {
                health.Base = 100 + Player.statLifeMax * 1.5f;
            }
        }
        public override void PostUpdate()
        {
            if (Player.velocity == Vector2.Zero)
            {
                ShortStanding++;
                if (ShortStanding > 120)
                {
                    if (ShortStanding % Math.Clamp((10 - ShortStanding / 100), 1, 10) == 0)
                    {
                        Player.statLife = Math.Clamp(Player.statLife + 1, 0, Player.statLifeMax2);
                    }
                }
            }
            else
            {
                ShortStanding = 0;
            }
            if (Earth)
            {
                EarthCD = BossRushUtils.CoolDown(EarthCD);
                if (EarthCD > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int dust = Dust.NewDust(Player.Center + Main.rand.NextVector2Circular(10, 30), 0, 0, DustID.Blood);
                        Main.dust[dust].velocity = -Vector2.UnitX * Player.direction * 2f;
                    }
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
    }
}
