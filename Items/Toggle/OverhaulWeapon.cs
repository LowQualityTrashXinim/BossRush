using BossRush.Texture;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Items.Toggle
{
    internal class OverhaulWeapon : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("World's gift");
            Tooltip.SetDefault("Change all existing weapon in the game" +
                "\n-Range weapon now have spread and have chance to not consume ammo");
        }

        public override void SetDefaults()
        {
            Item.height = 55;
            Item.width = 53;
            Item.maxStack = 999;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.downedSlimeKing;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    player.GetModPlayer<OverhaulWeaponPlayer>().OverhaulWeapon = true;
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("The world grant their gift to you"), Colors.RarityGreen);
                }
            }
            return true;
        }
    }
    public class OverhaulWeaponPlayer : ModPlayer
    {
        public bool OverhaulWeapon = false;
    }
}
