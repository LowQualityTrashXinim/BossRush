using Terraria;
using Humanizer;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.aDebugItem.UIdebug;
using Terraria.DataStructures;
using BossRush.Contents.Items.RelicItem.RelicTemplateContent;

namespace BossRush.Common.Systems.SpoilSystem;
internal class SSRspoil {
	public class SSRPerkSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override string FinalDisplayName() {
			return DisplayName.FormatWith(ItemID.FallenStar);
		}
		public override string FinalDescription() {
			ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
			return Description.FormatWith(chestplayer.ModifyGetAmount(1), chestplayer.ModifyGetAmount(2));
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			int type = ModContent.ItemType<WorldEssence>();
			if (Main.rand.NextFloat() <= .01f) {
				type = ModContent.ItemType<PerkDebugItem>();
			}
			player.QuickSpawnItem(player.GetSource_OpenItem(itemsource), type);
			LootBoxBase.GetSkillLootbox(itemsource, player);
			IEntitySource entitySource = player.GetSource_OpenItem(itemsource);
			int amount = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(2);
			for (int i = 0; i < amount; i++) {
				Item relicitem = player.QuickSpawnItemDirect(entitySource, ModContent.ItemType<Relic>());
				if (relicitem.ModItem is Relic relic) {
					relic.AutoAddRelicTemplate(player, 3);
				}
			}
			LootBoxBase.GetAccessories(itemsource, player, true);
		}
	}
	public class LegendaryRelicSpoil : ModSpoil {
		public override void SetStaticDefault() {
			RareValue = SpoilDropRarity.SSR;
		}
		public override bool IsSelectable(Player player, Item itemsource) {
			return SpoilDropRarity.SSRDrop();
		}
		public override void OnChoose(Player player, int itemsource) {
			Item item = player.QuickSpawnItemDirect(player.GetSource_OpenItem(itemsource), ModContent.ItemType<Relic>());
			if (item.ModItem is Relic relic) {
				if (Main.rand.NextBool(20)) {
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<GenericTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<CombatLowHPTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<CombatV4Template>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<LowHealthTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<CombatHighHPTemplate>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<DebuffedHealthTemplate>(), 3);
				}
				else {
					for (int i = 0; i < 6; i++) {
						relic.AddRelicTemplate(player, Main.rand.Next(RelicTemplateLoader.TotalCount), 3);
					}
				}
			}
		}
	}
}
