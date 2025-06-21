using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Common.ChallengeMode;
using BossRush.Common.WorldGenOverhaul;
using BossRush.Contents.Items.Consumable.Spawner;
using Microsoft.Xna.Framework;

namespace BossRush.Common.Mode.BossRushMode;
internal class BossRushGlobalItem : GlobalItem {
	public override bool? UseItem(Item item, Player player) {
		if (!UniversalSystem.CanAccessContent(player, UniversalSystem.BOSSRUSH_MODE)) {
			return base.UseItem(item, player);
		}
		if (item.type == ItemID.SlimeCrown) {
			Rectangle rect = ModContent.GetInstance<BossRushWorldGen>().Room[Bid.Slime][0];
			Point spawnPosotion = rect.Center().ToPoint();
			NPC.SpawnBoss(spawnPosotion.X * 16, spawnPosotion.Y * 16, NPCID.KingSlime, player.whoAmI);
			return true;
		}
		return base.UseItem(item, player);
	}

	public override bool CanUseItem(Item item, Player player) {
		if (!UniversalSystem.CanAccessContent(player, UniversalSystem.BOSSRUSH_MODE)) {
			return base.CanUseItem(item, player);
		}
		if (item.type == ItemID.SlimeCrown) {
			return BossRushWorldGen.IsInBiome(player, Bid.Slime, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.SuspiciousLookingEye) {
			bool checkThisFirst = BossRushWorldGen.IsInBiome(player, Bid.FleshRealm, ModContent.GetInstance<BossRushWorldGen>().Room);
			if (Main.netMode != NetmodeID.MultiplayerClient && checkThisFirst) {
				Main.SkipToTime(0, false);
			}
			return checkThisFirst;
		}
		if (item.type == ItemID.EmpressButterfly || item.type == ItemID.MechanicalEye || item.type == ItemID.MechanicalSkull || item.type == ItemID.MechanicalWorm) {
			bool checkThisFirst = BossRushWorldGen.IsInBiome(player, Bid.Hallow, ModContent.GetInstance<BossRushWorldGen>().Room);
			if (Main.netMode != NetmodeID.MultiplayerClient && checkThisFirst) {
				Main.SkipToTime(0, false);
			}
			return checkThisFirst;
		}
		if (item.type == ModContent.ItemType<PlanteraSpawn>() || item.type == ItemID.LihzahrdPowerCell) {

			return BossRushWorldGen.IsInBiome(player, Bid.Jungle, ModContent.GetInstance<BossRushWorldGen>().Room);
		}

		if (item.type == ItemID.WormFood) {
			return BossRushWorldGen.IsInBiome(player, Bid.Corruption, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.BloodySpine) {
			return BossRushWorldGen.IsInBiome(player, Bid.Crimson, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ModContent.ItemType<CursedDoll>() || item.type == ModContent.ItemType<LunaticTablet>()) {
			bool checkThisFirst = BossRushWorldGen.IsInBiome(player, Bid.Dungeon, ModContent.GetInstance<BossRushWorldGen>().Room);
			if (Main.netMode != NetmodeID.MultiplayerClient && checkThisFirst) {
				Main.SkipToTime(0, false);
			}
			return BossRushWorldGen.IsInBiome(player, Bid.Dungeon, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.TruffleWorm) {
			return BossRushWorldGen.IsInBiome(player, Bid.Beaches, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.Abeemination) {
			return BossRushWorldGen.IsInBiome(player, Bid.BeeNest, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.DeerThing) {
			return BossRushWorldGen.IsInBiome(player, Bid.Tundra, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ModContent.ItemType<WallOfFleshSpawner>()) {
			return BossRushWorldGen.IsInBiome(player, Bid.Underworld, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		return base.CanUseItem(item, player);
	}
}
