﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Toggle {
	public class CursedSkull : ModItem {
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		ColorInfo CustomColor;
		public override void SetDefaults() {
			Item.height = 60;
			Item.width = 56;
			Item.value = 0;
			Item.rare = ItemRarityID.Purple;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.scale = .5f;
			CustomColor = new ColorInfo(new List<Color> { new Color(255, 50, 255), new Color(100, 50, 100) });
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			TooltipLine line = new TooltipLine(Mod, "ChallengeGod", "A gift from God of challenge" + $"[i:{ModContent.ItemType<CursedSkull>()}]");
			line.OverrideColor = CustomColor.MultiColor(2);
			tooltips.Add(line);
		}
		public override bool CanUseItem(Player player) {
			return !BossRushUtils.IsAnyVanillaBossAlive();
		}
		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				SoundEngine.PlaySound(SoundID.Roar, player.position);
			}
			return true;
		}
	}
}
