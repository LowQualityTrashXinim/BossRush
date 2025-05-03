using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace BossRush.Contents.Items;

public class BrokenDice : ModItem {
	public override void SetStaticDefaults() {
		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 7));
		ItemID.Sets.AnimatesAsSoul[Type] = true;
	}
	public override void SetDefaults() {
		Item.width = 60;
		Item.height = 72;
	}
}
