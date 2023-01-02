using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;

namespace BossRush.Items.Spawner
{
    public class GitGudToggle : ModItem
    {
        public override string Texture => "BossRush/MissingTexture";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Skill Gem");
            Tooltip.SetDefault("Make every boss when you fight instant kill you\n" +
                "\"Make for people want to prove their skill to the god\"");
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
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<ModdedPlayer>().LookingForBoss();
        }
        int count = 0;
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (count == 0)
                    {
                        player.GetModPlayer<ModdedPlayer>().gitGud = true;
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Have fun"), Colors.RarityDarkRed);
                        count++;
                    }
                    else
                    {
                        player.GetModPlayer<ModdedPlayer>().gitGud = false;
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Skill issue"), new Microsoft.Xna.Framework.Color(0, 110, 225));
                        count = 0;
                    }
                }
            }
            return true;
        }
    }
}
