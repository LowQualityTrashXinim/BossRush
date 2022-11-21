using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;

namespace BossRush.Items.ExtraItem
{
    public class MoonLordEnrage : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MoonLordFullPower");
            Tooltip.SetDefault("Be fear, be scare");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know this is a boss summoning item.
            NPCID.Sets.MPAllowedEnemies[NPCID.MoonLordCore] = true;
            NPCID.Sets.MPAllowedEnemies[NPCID.MoonLordFreeEye] = true;
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
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // If the player using the item is the client
                // (explicitely excluded serverside here)
                SoundEngine.PlaySound(SoundID.MoonLord, player.position);

                int type = NPCID.MoonLordCore;
                int type2 = NPCID.MoonLordFreeEye;
                player.GetModPlayer<ModdedPlayer>().MoonLordEnraged = true;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                    for (int i = 0; i < 3; i++)
                    {
                        NPC.SpawnOnPlayer(player.whoAmI, type2);
                    }
                }
                else
                {
                    // If the player is in multiplayer, request a spawn
                    // This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in this class above
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
                    for (int i = 0; i < 3; i++)
                    {
                        NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type2);
                    }
                }
            }
            return true;
        }
    }
}
