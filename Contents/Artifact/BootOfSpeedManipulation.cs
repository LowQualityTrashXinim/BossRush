using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifact
{
    internal class BootOfSpeedManipulation : ArtifactModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void ArtifactSetDefault()
        {
            width = height = 32;
            Item.rare = ItemRarityID.Cyan;
        }
    }
    class BootSpeedPlayer : ArtifactPlayerHandleLogic
    {
        bool BootofSpeed = false;
        public override void ResetEffects()
        {
            BootofSpeed = ArtifactDefinedID == ModContent.ItemType<BootOfSpeedManipulation>();
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
            if (BootofSpeed)
            {
                Player.wingTime *= 0;
                Player.wingAccRunSpeed *= 0;
                Player.wingRunAccelerationMult *= 0;
                Player.wingTimeMax = 0;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (BootofSpeed)
                if (Player.velocity.IsLimitReached(5))
                    damage *= Main.rand.NextFloat(.3f, 1f);
        }
    }
}