using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;

namespace BossRush.Common.Global
{
    class GlobalItemMod : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.EoCShield)
            {
                player.GetModPlayer<EvilEyePlayer>().EoCShieldUpgrade = true;
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (ModContent.GetInstance<BossRushModConfig>().Enraged && player.whoAmI == Main.myPlayer && item.type == ItemID.SuspiciousLookingEye)
            {
                player.GetModPlayer<ModdedPlayer>().Enraged = true;
                int type = NPCID.EyeofCthulhu;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                    Main.bloodMoon = true;
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
                return true;
            }
            return default;
        }

    }
}