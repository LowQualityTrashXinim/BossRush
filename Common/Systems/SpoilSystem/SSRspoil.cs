using BossRush.Contents.Items.aDebugItem;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Perks;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			LootBoxBase.GetRelic(itemsource, player, 2);
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
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<CombatV2Template>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<CombatV3Template>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<CombatV4Template>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<HealthV2Template>(), 3);
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<HealthV3Template>(), 3);
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
