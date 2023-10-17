using BossRush.Common.Enraged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Spawner {
	public abstract class EnragedSpawner : ModItem {
		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			PostSetStaticDefaults();
		}
		public virtual void PostSetStaticDefaults() { }
		public virtual void SetDefaultWidthHeight(out int width, out int height) { width = 10; height = 10; }
		public override void SetDefaults() {
			SetDefaultWidthHeight(out int width, out int height);
			Item.BossRushDefaultToConsume(width, height);
			Item.rare = ItemRarityID.Blue;
		}
		public virtual int BossToSpawn => NPCID.GreenSlime;
		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				player.GetModPlayer<EnragedPlayer>().Enraged = true;
				SoundEngine.PlaySound(SoundID.Roar, player.position);
				int type = BossToSpawn;
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else {
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
				OnUseItem(player);
			}
			return true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			Item.DrawAuraEffect(spriteBatch,position, 2, 2, new Color(255, 0, 0, 30), 0, scale);
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			//if (Item.whoAmI != whoAmI)
			//{
			//    return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
			//}
			Item.DrawAuraEffect(spriteBatch, 2, 2, new Color(255, 0, 0, 30), 0, scale);
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		/// <summary>
		/// This is called in the check if player whoAmI is myPlayer
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnUseItem(Player player) { }
	}
}
