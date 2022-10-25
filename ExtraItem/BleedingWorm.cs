using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace BossRush.ExtraItem
{
	public class BleedingWorm : ModItem
	{
        public override string Texture => "BossRush/MissingTexture";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BleedingWorm");
			Tooltip.SetDefault("Actract a certain monster from ocean");
			ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know this is a boss summoning item.
			NPCID.Sets.MPAllowedEnemies[NPCID.BloodNautilus] = true;
		}

		public override void SetDefaults()
		{
			Item.height = 55;
			Item.width = 53;
			Item.maxStack = 999;
			Item.value = 100;
			Item.rare = ItemRarityID.LightRed;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return Main.bloodMoon;
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = NPCID.BloodNautilus;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.SpawnOnPlayer(player.whoAmI,type);
				}
				else
				{
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}
	}
}

