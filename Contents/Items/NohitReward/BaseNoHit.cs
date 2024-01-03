using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Items.NohitReward {
	abstract class BaseNoHit : ModItem {
		public const int HP = 50;
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.rare = ItemRarityID.Expert;
			Item.value = Item.sellPrice(platinum: 5, gold: 0, silver: 0, copper: 0);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			tooltips.Add(new TooltipLine(Mod, "NoHitReward",
				"Overcoming a small challenge \n" +
				"Reward for not getting hit \n" +
				"Increase max HP by 50 " +
				"Increase perk range by 1\n" +
				"Can only be uses once \n"
				));
			foreach (TooltipLine line in tooltips) {
				if (line.Text == "challenge") line.OverrideColor = Main.DiscoColor;
			}
		}
		public virtual int Data => Item.type;
		public override bool? UseItem(Player player) {
			player.statLifeMax2 += HP;
			player.statLife += HP;
			if (Main.myPlayer == player.whoAmI) {
				player.HealEffect(HP);
			}
			player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Add(Data);
			player.GetModPlayer<NoHitPlayerHandle>().ListIsChange = true;
			return true;
		}
		public override bool CanUseItem(Player player) {
			NoHitPlayerHandle modplayer = player.GetModPlayer<NoHitPlayerHandle>();
			return !modplayer.BossNoHitNumber.Contains(Data);
		}
		private int countX = 0;
		private int countY = 0;
		private float positionRotateX = 0;
		private float positionRotateY = 0;
		private void PositionHandle() {
			if (positionRotateX < 3 && countX == 1) {
				positionRotateX += .3f;
			}
			else {
				countX = -1;
			}
			if (positionRotateX > 0 && countX == -1) {
				positionRotateX -= .3f;
			}
			else {
				countX = 1;
			}
			if (positionRotateY < 3 && countY == 1) {
				positionRotateY += .3f;
			}
			else {
				countY = -1;
			}
			if (positionRotateY > 0 && countY == -1) {
				positionRotateY -= .3f;
			}
			else {
				countY = 1;
			}
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			Color color = new Color(255, 255, 0, 50);
			Item.DrawAuraEffect(spriteBatch, position, positionRotateX, positionRotateY, color, 0, scale);
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			Color color = new Color(255, 255, 0, 50);
			Item.DrawAuraEffect(spriteBatch, positionRotateX, positionRotateY, color, 0, scale);
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
	}
	public class NoHitPlayerHandle : ModPlayer {
		public List<int> BossNoHitNumber;
		public bool ListIsChange = false;
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			health = StatModifier.Default;
			mana = StatModifier.Default;
			health.Base = BossNoHitNumber.Count * BaseNoHit.HP;
		}
		public override void Initialize() {
			BossNoHitNumber = new List<int>();
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.NoHitBossNum);
			packet.Write((byte)Player.whoAmI);
			packet.Write(BossNoHitNumber.Count);
			foreach (int item in BossNoHitNumber)
				packet.Write(item);
			packet.Send(toWho, fromWho);
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			BossNoHitNumber.Clear();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
				BossNoHitNumber.Add(reader.ReadInt32());
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			NoHitPlayerHandle clone = (NoHitPlayerHandle)targetCopy;
			clone.BossNoHitNumber = BossNoHitNumber;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			if (ListIsChange) {
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
				ListIsChange = false;
			}
		}

		public override void LoadData(TagCompound tag) {
			BossNoHitNumber = tag.Get<List<int>>("BossNoHitNumber");
		}
		public override void SaveData(TagCompound tag) {
			tag["BossNoHitNumber"] = BossNoHitNumber;
		}
	}
}
