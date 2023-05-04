using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using BossRush.Common;

namespace BossRush.Contents.Items.Spawner
{
    public class KingSlimeCall : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know this is a boss summoning item.
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            NPCID.Sets.MPAllowedEnemies[NPCID.KingSlime] = true;
        }

        public override void SetDefaults()
        {
            Item.height = 78;
            Item.width = 66;
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
                player.GetModPlayer<ModdedPlayer>().KingSlimeEnraged = true;
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = NPCID.KingSlime;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                    Main.StartSlimeRain();
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SlimeCrown)
                .AddIngredient(ModContent.ItemType<PowerEnergy>())
                .Register();
        }
    }
}
