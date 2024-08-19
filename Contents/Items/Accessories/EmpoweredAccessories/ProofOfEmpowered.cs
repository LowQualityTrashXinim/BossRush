using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using System.Reflection;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.EmpoweredAccessories;
internal class ProofOfEmpowered : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	MethodInfo meth = null;
	public override void SetStaticDefaults() {
		meth = typeof(Player).GetMethod("SpawnHallucination", BindingFlags.NonPublic | BindingFlags.Instance);
	}
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.accessory = true;
		Item.rare = ItemRarityID.Expert;
		meth = typeof(Player).GetMethod("SpawnHallucination", BindingFlags.NonPublic | BindingFlags.Instance);
	}
	public override void UpdateAccessory(Player player, bool hideVisual) {
		player.endurance += .17f;
		player.npcTypeNoAggro[1] = true;
		player.npcTypeNoAggro[16] = true;
		player.npcTypeNoAggro[59] = true;
		player.npcTypeNoAggro[71] = true;
		player.npcTypeNoAggro[81] = true;
		player.npcTypeNoAggro[138] = true;
		player.npcTypeNoAggro[121] = true;
		player.npcTypeNoAggro[122] = true;
		player.npcTypeNoAggro[141] = true;
		player.npcTypeNoAggro[147] = true;
		player.npcTypeNoAggro[183] = true;
		player.npcTypeNoAggro[184] = true;
		player.npcTypeNoAggro[204] = true;
		player.npcTypeNoAggro[225] = true;
		player.npcTypeNoAggro[244] = true;
		player.npcTypeNoAggro[302] = true;
		player.npcTypeNoAggro[333] = true;
		player.npcTypeNoAggro[335] = true;
		player.npcTypeNoAggro[334] = true;
		player.npcTypeNoAggro[336] = true;
		player.npcTypeNoAggro[537] = true;
		player.npcTypeNoAggro[676] = true;
		player.npcTypeNoAggro[667] = true;
		player.dashType = DashID.ShieldOfCthulhu;
		player.brainOfConfusionItem = Item;
		player.boneGloveItem = Item;
		player.strongBees = true;
		if (meth != null) 			try {
				meth.Invoke(player, new object[] { Item });
			}
			catch (Exception ex) {
				Main.NewText(ex.Message);
			}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.RoyalGel)
			.AddIngredient(ItemID.EoCShield)
			.AddIngredient(ItemID.WormScarf)
			.AddIngredient(ItemID.BrainOfConfusion)
			.AddIngredient(ItemID.HiveBackpack)
			.AddIngredient(ItemID.BoneGlove)
			.AddIngredient(ItemID.BoneHelm)
			.AddIngredient(ModContent.ItemType<PowerEnergy>())
			.Register();
	}
}
