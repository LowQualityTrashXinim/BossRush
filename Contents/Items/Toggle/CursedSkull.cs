using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common;

namespace BossRush.Contents.Items.Toggle
{
    public class CursedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Make every boss when you fight instant kill you\n" +
                "\"The only way to make god acknowledge you\""); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.height = 60;
            Item.width = 56;
            Item.value = 0;
            Item.rare = ItemRarityID.Purple;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ChallengeGod", "A gift from God of challenge" + $"[i:{ModContent.ItemType<CursedSkull>()}]"));
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.Name == "ChallengeGod")
                {
                    line2.OverrideColor = BossRushColor.ChallangeGodColor;
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            return !BossRushUtils.IsAnyVanillaBossAlive();
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
