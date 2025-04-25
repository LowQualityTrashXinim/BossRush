using BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossRush.Contents.Items;
public class ItemReworker : ModItem {
	

	public virtual int VanillaItemType => ItemID.None;
	public override string Texture => "Terraria/Images/Item_"+VanillaItemType;
	public override void SetDefaults() {


		Item.CloneDefaults(VanillaItemType);
		

	}
}

