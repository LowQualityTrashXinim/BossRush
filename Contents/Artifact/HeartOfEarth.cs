using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Artifact
{
    internal class HeartOfEarth : ArtifactModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;

        public override void ArtifactSetDefault()
        {
            width = 32; height = 58;
            Item.rare = ItemRarityID.Cyan;
        }
    }
    class HeartOfEarthPlayer : ArtifactPlayerHandleLogic
    {
        bool Earth = false;
        public override void ResetEffects()
        {
            Earth = ArtifactDefinedID == ModContent.ItemType<HeartOfEarth>();
        }
        int EarthCD = 0;
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            if (Earth)
            {
                health.Base = 100 + Player.statLifeMax * 2;
            }
        }
        public override void PostUpdate()
        {
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