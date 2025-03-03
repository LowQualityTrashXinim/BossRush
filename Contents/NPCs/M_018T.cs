using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria.Localization;
using BossRush.Contents.Items;
using BossRush.Contents.Perks;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.Consumable.Potion;
using BossRush.Common.Systems;
using BossRush.Contents.Skill;
using BossRush.Contents.Items.aDebugItem.StatsInform;
using BossRush.Contents.Items.RelicItem;
using BossRush.Common.General;
using BossRush.Contents.Items.Toggle;

namespace BossRush.Contents.NPCs;
internal class M_018T : ModNPC {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public const string ShopName = "Shop";
	public override void SetStaticDefaults() {
		NPCID.Sets.NoTownNPCHappiness[Type] = true;
	}
	public override void SetDefaults() {
		NPC.width = NPC.height = 36;
		NPC.lifeMax = 900;
		NPC.townNPC = true;
		NPC.friendly = true;
		NPC.aiStyle = 7;
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
		if (player.HeldItem.ModItem is SynergyModItem) {
			chat.Add("A synergy item, are you gonna sell it to me ? Please don't, I don't want to anger god");
			chat.Add("Unlimited potential, hold the power to rival mythical weapon");
			chat.Add("Synergy item isn't what it seem on the surface ...");
			chat.Add("It hold hidden secret, you should start trying to find out the secret");
		}
		else if (player.HeldItem.ModItem is CelestialWrath) {
			chat.Add("Get that thing away from me");
			chat.Add("Do not throw it here, throw it somewhere that not close to us");
			chat.Add("You gonna throw it here ? please don't");
			chat.Add("Do not sell that thing to me, no mortal would want to buy that !");
		}
		else {
			if (UniversalSystem.Check_RLOH()) {
				chat.Add("A lot of normal item are quite good now, sadly their selling price haven't change");
			}
			chat.Add("What ?");
			chat.Add("Are you here to buy ?");
			chat.Add("I'm not here to make friend");
			chat.Add("Pick something and then begone");
		}
		string chosenChat = chat;
		return chosenChat;
	}
	public override void AddShops() {
		NPCShop shop = new NPCShop(Type, ShopName);
		shop.Add(new Item(ModContent.ItemType<RelicContainer>()) { shopCustomPrice = Item.buyPrice(gold: 7) });
		shop.Add(new Item(ModContent.ItemType<WeaponLootBox>()) { shopCustomPrice = Item.buyPrice(gold: 15) });
		shop.Add(new Item(ModContent.ItemType<SkillLootBox>()) { shopCustomPrice = Item.buyPrice(gold: 17) });
		shop.Add(new Item(ModContent.ItemType<WorldEssence>()) { shopCustomPrice = Item.buyPrice(platinum: 1) });
		shop.Add(new Item(ModContent.ItemType<CelestialEssence>()) { shopCustomPrice = Item.buyPrice(platinum: 1) });
		shop.Add(new Item(ModContent.ItemType<UserInfoTablet>()) { shopCustomPrice = Item.buyPrice(gold: 50) });
		shop.Register();
	}
}
