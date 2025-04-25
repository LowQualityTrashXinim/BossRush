using Terraria;
using Humanizer;
using Terraria.ID;
using BossRush.Contents.Items.Chest;
using Terraria.DataStructures;
using Terraria.ModLoader;
using BossRush.Contents.Perks;

namespace BossRush.Common.Systems.SpoilSystem;
internal class RareSpoil {
	public class RoguelikeSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.FallenStar);
		}
		public override string FinalDescription() {
			ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
			return Description.FormatWith(
				chestplayer.ModifyGetAmount(2),
				chestplayer.ModifyGetAmount(4)
				);
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.RareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			LootBoxBase.GetSkillLootbox(itemsource, player, 2);
			LootBoxBase.GetRelic(itemsource, player, 4);
		}
	}
	public class ArmorAccessorySpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.ArmorStatue);
		}
		public override string FinalDescription() {
			ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
			return Description.FormatWith(
				chestplayer.ModifyGetAmount(1),
				chestplayer.ModifyGetAmount(2)
				);
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.RareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			int amount = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				LootBoxBase.GetAccessories(itemsource, player);
			}
			int amount2 = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(1);
			for (int i = 0; i < amount2; i++) {
				LootBoxBase.GetArmorPiece(itemsource, player);
			}
		}
	}
	public class LostAccessorySpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.RareDrop() && UniversalSystem.LuckDepartment(UniversalSystem.CHECK_LOSTACC);
		}
		public override string FinalDescription() {
			return Description.FormatWith(Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(1));
		}
		public override void OnChoose(Player player, int itemsource) {
			int amount = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(1);
			for (int i = 0; i < amount; i++) {
				player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), Main.rand.NextFromHashSet(BossRushModSystem.LostAccessories));
			}
		}
	}
	public class RareArmorPiece : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.RareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			LootBoxBase.GetArmorPiece(itemsource, player);
		}
	}
	public class StarterPerkSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.Rare;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.RareDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			IEntitySource entitySource = player.GetSource_OpenItem(itemsource);
			player.QuickSpawnItem(entitySource, ModContent.ItemType<CelestialEssence>());
		}
	}
}


