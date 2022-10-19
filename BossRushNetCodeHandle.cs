using System.IO;
using Terraria;
using BossRush.CustomPotion;
using Terraria.ModLoader.Utilities;

namespace BossRush
{
	partial class BossRushNetCodeHandle
	{
		internal enum MessageType : byte
		{
			DrugSyncPlayer,
			KingSlimeNoHit,
			EoCNoHit,
			GambleAddiction
		}
		public void HandlePacket(BinaryReader reader, int whoAmI)
		{
			MessageType msgType = (MessageType)reader.ReadByte();

			if (msgType == MessageType.DrugSyncPlayer)
			{
				// This message syncs ExamplePlayer.exampleLifeFruits
				byte playernumber = reader.ReadByte();
				WonderDrugPlayer DrugDealing = Main.player[playernumber].GetModPlayer<WonderDrugPlayer>();
				DrugDealing.DrugDealer = reader.ReadInt32();
				// SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
			}
		}
	}
}
