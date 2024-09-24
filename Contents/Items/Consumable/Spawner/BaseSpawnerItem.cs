using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
namespace BossRush.Contents.Items.Consumable.Spawner {
	public abstract class BaseSpawnerItem : ModItem {
		public virtual int[] NPCtypeToSpawn => new int[] { };
		public virtual bool UseSpecialSpawningMethod => false;
		public override void SetStaticDefaults() {
			ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know this is a boss summoning item.
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			for (int i = 0; i < NPCtypeToSpawn.Length; i++) 				
				NPCID.Sets.MPAllowedEnemies[NPCtypeToSpawn[i]] = true;
			PostSetStaticDefaults();
		}
		public virtual void PostSetStaticDefaults() {

		}
		public virtual void SetSpawnerDefault(out int width, out int height) {
			width = 1; height = 1;
		}
		public override void SetDefaults() {
			SetSpawnerDefault(out int width, out int height);
			Item.height = width;
			Item.width = height;
			Item.maxStack = 999;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}
		public virtual void SpecialSpawningLogic(Player player) {
		}
		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				// If the player using the item is the client
				// (explicitely excluded serverside here)
				SoundEngine.PlaySound(SoundID.Roar, player.position);
				for (int i = 0; i < NPCtypeToSpawn.Length; i++) 					
					if (Main.netMode != NetmodeID.MultiplayerClient) 						
						if (UseSpecialSpawningMethod) 							
							SpecialSpawningLogic(player);
						else 							
							NPC.SpawnOnPlayer(player.whoAmI, NPCtypeToSpawn[i]);
					else 						
						NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: NPCtypeToSpawn[i]);
			}
			return true;
		}
	}
}
