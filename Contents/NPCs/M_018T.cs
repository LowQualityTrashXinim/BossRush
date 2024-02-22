using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria.Localization;
using BossRush.Contents.Items;
using BossRush.Contents.Perks;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Contents.Items.aDebugItem;
using BossRush.Common.Systems;

namespace BossRush.Contents.NPCs;
internal class M_018T : ModNPC {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public const string ShopName = "Shop";
	public override void SetStaticDefaults() {
		NPCID.Sets.NoTownNPCHappiness[Type] = true;
	}
	public override void SetDefaults() {
		NPC.width = NPC.height = 36;
		NPC.lifeMax = 900;
		NPC.townNPC = true;
		NPC.friendly = true;
		NPC.noGravity = true;
		TownNPCStayingHomeless = true;
	}
	public override void SetChatButtons(ref string button, ref string button2) {
		button = Language.GetTextValue("LegacyInterface.28");
	}
	public override void OnChatButtonClicked(bool firstButton, ref string shopName) {
		if (firstButton) {
			shopName = ShopName;
		}
	}
	public override string GetChat() {
		Player player = Main.LocalPlayer;
		WeightedRandom<string> chat = new WeightedRandom<string>();
		chat.Add("What ?");
		chat.Add("Are you here to buy ?");
		chat.Add("I'm not here to make friend");
		chat.Add("Pick something and then begone");
		if (player.HeldItem.ModItem is SynergyModItem) {
			chat.Add("A synergy item ? Lucky ...");
		}
		if (player.HeldItem.ModItem is CelestialWrath) {
			chat.Add("Get that thing away from me");
		}
		string chosenChat = chat;
		return chosenChat;
	}
	public override void AddShops() {
		NPCShop shop = new NPCShop(Type, ShopName);
		shop.Add(new Item(ModContent.ItemType<MysteriousPotion>()) { shopCustomPrice = Item.buyPrice(gold: 1) });
		shop.Add(new Item(ModContent.ItemType<CardPacket>()) { shopCustomPrice = Item.buyPrice(gold: 20) });
		shop.Add(new Item(ModContent.ItemType<WeaponLootBox>()) { shopCustomPrice = Item.buyPrice(gold: 40) });
		shop.Add(new Item(ModContent.ItemType<SkillLootBox>()) { shopCustomPrice = Item.buyPrice(gold: 55) });
		shop.Add(new Item(ModContent.ItemType<PerkChooser>()) { shopCustomPrice = Item.buyPrice(platinum: 1) });
		shop.Add(new Item(ModContent.ItemType<StarterPerkChooser>()) { shopCustomPrice = Item.buyPrice(platinum: 1) });
		if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
			shop.Add(new Item(ModContent.ItemType<EnchantmentTablet>()) { shopCustomPrice = Item.buyPrice(platinum: 5) });
		}
		shop.Add(new Item(ModContent.ItemType<ModStatsDebugger>()) { shopCustomPrice = Item.buyPrice(gold: 50) });
		shop.Add(new Item(ModContent.ItemType<ShowPlayerStats>()) { shopCustomPrice = Item.buyPrice(gold: 50) });
		shop.Register();
	}
}
