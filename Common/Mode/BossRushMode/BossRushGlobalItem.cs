using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Common.ChallengeMode;
using BossRush.Common.WorldGenOverhaul;
using BossRush.Contents.Items.Consumable.Spawner;

namespace BossRush.Common.Mode.BossRushMode;
internal class BossRushGlobalItem : GlobalItem {
	public override bool CanUseItem(Item item, Player player) {

		if (!UniversalSystem.CanAccessContent(player, UniversalSystem.BOSSRUSH_MODE)) {
			return base.CanUseItem(item, player);
		}
		if (item.type == ItemID.SlimeCrown) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Slime, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.SuspiciousLookingEye) {
			bool checkThisFirst = BossRushWorldGen.IsInBiome(player, BiomeAreaID.FleshRealm, ModContent.GetInstance<BossRushWorldGen>().Room);
			if (Main.netMode != NetmodeID.MultiplayerClient && checkThisFirst) {
				Main.SkipToTime(0, false);
			}
			return checkThisFirst;
		}
		if (item.type == ItemID.EmpressButterfly || item.type == ItemID.MechanicalEye || item.type == ItemID.MechanicalSkull || item.type == ItemID.MechanicalWorm) {
			bool checkThisFirst = BossRushWorldGen.IsInBiome(player, BiomeAreaID.Hallow, ModContent.GetInstance<BossRushWorldGen>().Room);
			if (Main.netMode != NetmodeID.MultiplayerClient && checkThisFirst) {
				Main.SkipToTime(0, false);
			}
			return checkThisFirst;
		}
		if (item.type == ModContent.ItemType<PlanteraSpawn>() || item.type == ItemID.LihzahrdPowerCell) {

			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Jungle, ModContent.GetInstance<BossRushWorldGen>().Room);
		}

		if (item.type == ItemID.WormFood) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Corruption, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.BloodySpine) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Crimson, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ModContent.ItemType<CursedDoll>() || item.type == ModContent.ItemType<LunaticTablet>()) {
			bool checkThisFirst = BossRushWorldGen.IsInBiome(player, BiomeAreaID.Dungeon, ModContent.GetInstance<BossRushWorldGen>().Room);
			if (Main.netMode != NetmodeID.MultiplayerClient && checkThisFirst) {
				Main.SkipToTime(0, false);
			}
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Dungeon, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.TruffleWorm) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Beaches, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.Abeemination) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.BeeNest, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.DeerThing) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Tundra, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ModContent.ItemType<WallOfFleshSpawner>()) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Underground, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		return base.CanUseItem(item, player);
	}
}
