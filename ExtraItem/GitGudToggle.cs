using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;

namespace BossRush.ExtraItem
{
	public class GitGudToggle : ModItem
	{
        public override string Texture => "BossRush/MissingTexture";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Confident Mode");
			Tooltip.SetDefault("Make every boss when you fight instant kill you\nShow me how confident you are on beating boss with given item");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
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
			return !NPC.AnyNPCs(NPCID.KingSlime)
				&& !NPC.AnyNPCs(NPCID.EyeofCthulhu)
				&& !NPC.AnyNPCs(NPCID.BrainofCthulhu)
				&& !NPC.AnyNPCs(NPCID.EaterofWorldsHead)
				&& !NPC.AnyNPCs(NPCID.SkeletronHead)
				&& !NPC.AnyNPCs(NPCID.QueenBee)
				&& !NPC.AnyNPCs(NPCID.WallofFlesh)
				&& !NPC.AnyNPCs(NPCID.QueenSlimeBoss)
				&& !NPC.AnyNPCs(NPCID.Spazmatism)
				&& !NPC.AnyNPCs(NPCID.Retinazer)
				&& !NPC.AnyNPCs(NPCID.TheDestroyer)
				&& !NPC.AnyNPCs(NPCID.SkeletronPrime)
				&& !NPC.AnyNPCs(NPCID.Plantera)
				&& !NPC.AnyNPCs(NPCID.Golem)
				&& !NPC.AnyNPCs(NPCID.CultistBoss)
				&& !NPC.AnyNPCs(NPCID.HallowBoss)
				&& !NPC.AnyNPCs(NPCID.DukeFishron)
				&& !NPC.AnyNPCs(NPCID.MoonLordCore);
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
						ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Git Gud Time"), Colors.RarityDarkRed);
						count++;
					}
                    else
                    {
						player.GetModPlayer<ModdedPlayer>().gitGud = false;
						ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Skill issue"), new Microsoft.Xna.Framework.Color(0,110,225));
						count = 0;
					}
				}
			}
			return true;
		}
	}
}
