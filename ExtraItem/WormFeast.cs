using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;

namespace BossRush.ExtraItem
{
	public class WormFeast : ModItem
	{
        public override string Texture => "BossRush/ExtraItem/MissingTexture";
        public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Caution : Using this will gather unwanted attention");
			ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know this is a boss summoning item.
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			NPCID.Sets.MPAllowedEnemies[NPCID.EaterofWorldsHead] = true;
			NPCID.Sets.MPAllowedEnemies[NPCID.EaterofWorldsBody] = true;
			NPCID.Sets.MPAllowedEnemies[NPCID.EaterofWorldsTail] = true;
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
				SoundEngine.PlaySound(SoundID.Roar, player.position);
				player.GetModPlayer<ModdedPlayer>().EaterOfWorldEnraged = true;
				int type = NPCID.EaterofWorldsHead;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.WormFood)
				.AddIngredient(ModContent.ItemType<PowerEnergy>())
				.Register();
		}
	}
}

