using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Transfixion.Arguments;

namespace BossRush.Contents.Transfixion.SoulBound.SoulBoundMaterial;
public abstract class BaseSoulBoundItem : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public virtual short SoulBoundType => -1;
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		int index = tooltips.FindIndex(t => t.Name == "ItemName");
		if (index >= 0) {
			tooltips[index].OverrideColor = info.MultiColor(6);
		}
		ModSoulBound soulbound = SoulBoundLoader.GetSoulBound(SoulBoundType);
		if (soulbound != null) {
			tooltips.Add(new(Mod, "SoulBound", soulbound.ModifiedToolTip(Item)));
		}
	}
	ColorInfo info = new(new List<Color>() { Color.AliceBlue, Color.IndianRed, Color.MediumPurple, Color.DarkGreen, Color.LightGoldenrodYellow });
}
