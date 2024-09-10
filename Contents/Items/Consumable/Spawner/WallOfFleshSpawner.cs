using BossRush.Common.WorldGenOverhaul;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;

namespace BossRush.Contents.Items.Consumable.Spawner {
	public class WallOfFleshSpawner : BaseSpawnerItem {
		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.GuideVoodooDoll, 1)
			.Register();
		}
		public override int[] NPCtypeToSpawn => new int[] { NPCID.WallofFlesh };
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GuideVoodooDoll);

		public override bool CanUseItem(Player player) {
			if (!NPC.AnyNPCs(NPCID.WallofFlesh) && player.ZoneUnderworldHeight)
				return true;

			return false;
		}
		public override bool UseSpecialSpawningMethod => true;
		public override void SpecialSpawningLogic(Player player) {
			var pos = player.Center;
			if (pos.Y / 16f < Main.maxTilesY - 205 || Main.wofNPCIndex >= 0 || Main.netMode == 1 || NPC.AnyNPCs(113))
				return;

			Player.FindClosest(pos, 16, 16);
			int num = 1;
			if (pos.X / 16f > Main.maxTilesX / 2)
				num = -1;

			bool flag = false;
			int num2 = (int)pos.X;
			int targetPlayerIndex = 0;
			while (!flag) {
				flag = true;
				for (int i = 0; i < 255; i++) 					if (Main.player[i].active && Main.player[i].position.X > num2 - 1200 && Main.player[i].position.X < num2 + 1200) {
						num2 -= num * 16;
						flag = false;
						targetPlayerIndex = i;
					}

				if (num2 / 16 < 20 || num2 / 16 > Main.maxTilesX - 20)
					flag = true;
			}

			int num10 = NPC.NewNPC(NPC.GetBossSpawnSource(targetPlayerIndex), num2, (int)(RogueLikeWorldGen.GridPart_Y * 23.5f * 16), NPCID.WallofFlesh);
			if (Main.netMode == 0)
				Main.NewText(Language.GetTextValue("Announcement.HasAwoken", Main.npc[num10].TypeName), 175, 75);
			else if (Main.netMode == 2)
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Main.npc[num10].GetTypeNetName()), new Color(175, 75, 255));
		}
		float positionRotateX = 0f;
		float countX = 0f;
		private void PositionHandle() {
			if (positionRotateX < 3 && countX == 1) 				positionRotateX += .3f;
			else 				countX = -1;
			if (positionRotateX > 0 && countX == -1) 				positionRotateX -= .3f;
			else 				countX = 1;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			Main.instance.LoadItem(Item.type);
			var texture = TextureAssets.Item[Item.type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(positionRotateX, positionRotateX), null, Color.Purple, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(positionRotateX, -positionRotateX), null, Color.Purple, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, positionRotateX), null, Color.Purple, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, -positionRotateX), null, Color.Purple, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
}
