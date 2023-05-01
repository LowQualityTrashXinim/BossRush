using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using BossRush.Common;

namespace BossRush.Contents.Items.Spawner
{
    public class BloodyEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("WHAT DID YOU SHOW TO THE EYE OF CTHULHU");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know this is a boss summoning item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            NPCID.Sets.MPAllowedEnemies[NPCID.EyeofCthulhu] = true;
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
            return !Main.dayTime;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.GetModPlayer<ModdedPlayer>().EoCEnraged = true;
                // If the player using the item is the client
                // (explicitely excluded serverside here)
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = NPCID.EyeofCthulhu;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    for (int i = 0; i < 2; i++)
                    {
                        NPC.SpawnOnPlayer(player.whoAmI, type);
                    }
                    Main.bloodMoon = true;
                }
                else
                {
                    // If the player is in multiplayer, request a spawn
                    // This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in this class above
                    for (int i = 0; i < 2; i++)
                    {
                        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                    }
                }
            }
            return true;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SuspiciousLookingEye)
                .AddIngredient(ModContent.ItemType<PowerEnergy>())
                .Register();
        }
    }
}
