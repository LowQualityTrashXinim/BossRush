using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Common.ChallengeMode;
using BossRush.Common.WorldGenOverhaul;
using BossRush.Contents.Items.Spawner;

namespace BossRush.Common.BossRushMode;
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
			if (Main.dayTime && checkThisFirst) {
				Main.time = Main.dayLength;
			}
			return checkThisFirst;
		}
		if (item.type == ItemID.WormFood) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Corruption, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ItemID.BloodySpine) {
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Crimson, ModContent.GetInstance<BossRushWorldGen>().Room);
		}
		if (item.type == ModContent.ItemType<CursedDoll>()) {
			bool checkThisFirst = BossRushWorldGen.IsInBiome(player, BiomeAreaID.Dungeon, ModContent.GetInstance<BossRushWorldGen>().Room);
			if (Main.dayTime && checkThisFirst) {
				Main.time = Main.dayLength;
			}
			return BossRushWorldGen.IsInBiome(player, BiomeAreaID.Dungeon, ModContent.GetInstance<BossRushWorldGen>().Room);
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
