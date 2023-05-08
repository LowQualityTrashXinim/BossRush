using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Artifact
{
    internal class HeartOfEarth : ModItem, IArtifactItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 58;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<HeartOfEarthPlayer>().HeartOfEarth = true;
            player.statLifeMax2 *= 2;
            player.statLifeMax2 += 100;
        }
    }
    class HeartOfEarthPlayer : ModPlayer
    {
        public bool HeartOfEarth = false;
        int coolDown = 0;
        public override void ResetEffects()
        {
            HeartOfEarth = false;
        }
        public override void PostUpdate()
        {
            bool isOnCoolDown = coolDown > 0;
            coolDown -= isOnCoolDown ? 1 : 0;
            if (isOnCoolDown)
            {
                int dust = Dust.NewDust(Player.Center, 0, 0, DustID.Blood);
                Main.dust[dust].velocity = -Vector2.UnitY * 2f + Main.rand.NextVector2Circular(2, 2);
            }
        }
        public override bool CanUseItem(Item item)
        {
            if (!HeartOfEarth)
            {
                return base.CanUseItem(item);
            }
            return coolDown <= 0;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            coolDown = 300;
        }
    }
}
