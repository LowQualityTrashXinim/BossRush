using BossRush.Common.Global;
using BossRush.Common.Mode.DreamLikeWorldMode;
using BossRush.Common.Systems;
using BossRush.Common.Systems.Achievement;
using BossRush.Contents.Perks;
using BossRush.Contents.Transfixion.Arguments;
using BossRush.Contents.Transfixion.SoulBound;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using Terraria;
using Terraria.Achievements;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace BossRush.Contents.Transfixion.WeaponEnchantment;
public class EnchantmentSystem : ModSystem {
	public static void EnchantmentRNG(Player self, ref Item item) {
		if (item == null || !EnchantmentGlobalItem.CanBeEnchanted(item)) {
			return;
		}
		EnchantmentModplayer modplayer = self.GetModPlayer<EnchantmentModplayer>();
		PlayerStatsHandle handle = self.GetModPlayer<PlayerStatsHandle>();
		if (modplayer.Request_EnchantedItem > 0) {
			int length = modplayer.Request_EnchantedAmount;
			for (int i = 0; i < length; i++) {
				EnchantItem(ref item, i);
			}
			modplayer.Request_EnchantedItem--;
		}
		float randomizedchance = 0f;
		if (!ChaosModeSystem.Chaos()) {
			if (UniversalSystem.Check_TotalRNG()) {
				randomizedchance += .2f;
			}
			if (UniversalSystem.LuckDepartment(UniversalSystem.CHECK_WWEAPONENCHANT)) {
				return;
			}
		}
		for (int i = 0; i < 3; i++) {
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				if (globalitem.EnchantmenStlot[i] != 0) {
					continue;
				}
			}
			if (Main.rand.NextFloat() <= randomizedchance + handle.RandomizeChanceEnchantment) {
				EnchantItem(ref item, i);
				continue;
			}
			break;
		}
	}
	public static void EnchantItem(ref Item item, int slot = -1, int enchantmentType = -1) {
		if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
			if (slot == -1) {
				for (int i = 0; i < globalitem.EnchantmenStlot.Length - 1; i++) {
					if (globalitem.EnchantmenStlot[i] != 0) {
						continue;
					}
					slot = i;
				}
			}
			slot = Math.Clamp(slot, 0, globalitem.EnchantmenStlot.Length - 1);
			if (enchantmentType == -1) {
				globalitem.EnchantmenStlot[slot] = Main.rand.Next(EnchantmentLoader.EnchantmentcacheID);
			}
			else {
				ModEnchantment enchant = EnchantmentLoader.GetEnchantmentItemID(enchantmentType);
				if (enchant == null) {
					return;
				}
				enchant.OnAddEnchantment(item, globalitem, enchantmentType, slot);
				globalitem.EnchantmenStlot[slot] = enchantmentType;
			}
		}
	}
}
public class EnchantmentGlobalItem : GlobalItem {
	public static bool CanBeEnchanted(Item entity) => entity.IsAWeapon() && !entity.consumable;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return CanBeEnchanted(entity);
	}
	public override bool InstancePerEntity => true;
	public int[] EnchantmenStlot = new int[4];
	public int[] Item_Counter1 = new int[4];
	public int[] Item_Counter2 = new int[4];
	public int[] Item_Counter3 = new int[4];
	public override GlobalItem Clone(Item from, Item to) {
		EnchantmentGlobalItem clone = (EnchantmentGlobalItem)base.Clone(from, to);
		if (clone == null) {
			return base.Clone(from, to);
		}
		if (EnchantmenStlot.Length < 4 || clone.EnchantmenStlot.Length < 4) {
			Array.Resize(ref EnchantmenStlot, 4);
			Array.Resize(ref clone.EnchantmenStlot, 4);
		}
		clone.EnchantmenStlot = new int[4];
		Array.Copy((int[])EnchantmenStlot?.Clone(), clone.EnchantmenStlot, 4);
		return clone;
	}
	public override GlobalItem NewInstance(Item target) {
		EnchantmenStlot = new int[4];
		Item_Counter1 = new int[4];
		Item_Counter2 = new int[4];
		Item_Counter3 = new int[4];
		return base.NewInstance(target);
	}
	public int GetValidNumberOfEnchantment() {
		int valid = 0;
		for (int i = 0; i < EnchantmenStlot.Length; i++) {
			if (EnchantmenStlot[i] != -1 && EnchantmenStlot[i] != 0 && EnchantmentLoader.GetEnchantmentItemID(EnchantmenStlot[i]) != null) {
				valid++;
			}
		}
		return valid;
	}
	public override void UpdateInventory(Item item, Player player) {
		if (EnchantmenStlot == null) {
			return;
		}
		//This is here to be consistent
		if (player.HeldItem.type == ItemID.None || item.type == ItemID.None) {
			return;
		}
		for (int l = 0; l < EnchantmenStlot.Length; l++) {
			if (EnchantmenStlot[l] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(EnchantmenStlot[l]).Update(l, item, this, player);
		}
	}
	public override void HoldItem(Item item, Player player) {
		if (EnchantmenStlot == null) {
			return;
		}
		//This is here to be consistent
		if (player.HeldItem.type == ItemID.None || item.type == ItemID.None) {
			return;
		}
		for (int i = 0; i < EnchantmenStlot.Length; i++) {
			if (EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(EnchantmenStlot[i]).UpdateHeldItem(i, item, this, player);
		}
	}
	public string GetWeaponModificationStats() => $"Item's enchantment slot : {EnchantmenStlot.Length}";
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.HARDCORE_MODE))
			return;
		if (UniversalSystem.EnchantingState)
			return;
		if (item.damage > 0 && EnchantmenStlot != null) {
			string text = "";
			for (int i = 0; i < EnchantmenStlot.Length; i++) {
				if (EnchantmenStlot[i] == ItemID.None) {
					continue;
				}
				text += $"[[i:{EnchantmenStlot[i]}]]\n{EnchantmentLoader.GetEnchantmentItemID(EnchantmenStlot[i]).Description}\n";
			}
			tooltips.Add(new TooltipLine(Mod, "", $"{text}"));
		}
	}
	public override void SaveData(Item item, TagCompound tag) {
		tag.Add("EnchantmentSlot", EnchantmenStlot);
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (tag.TryGet("EnchantmentSlot", out int[] TypeValue))
			EnchantmenStlot = TypeValue;
	}
}
public class EnchantmentModplayer : ModPlayer {
	Item item;
	EnchantmentGlobalItem globalItem;
	public int SlotUnlock = 0;
	public void SafeRequest_EnchantItem(int requestAmount, int amountEnchant) {
		Request_EnchantedItem = requestAmount;
		Request_EnchantedAmount = amountEnchant;
	}
	public int Request_EnchantedItem = 0;
	public int Request_EnchantedAmount = 1;
	public override void ResetEffects() {
		Player.GetModPlayer<PerkPlayer>().perks.TryGetValue(Perk.GetPerkType<EnchantmentSmith>(), out int value);
		SlotUnlock = value;
	}
	private bool CommonEnchantmentCheck() => !Player.HeldItem.IsAWeapon() || globalItem == null || globalItem.EnchantmenStlot == null || !UniversalSystem.CanAccessContent(Player, UniversalSystem.HARDCORE_MODE);
	public override void PostUpdate() {
		if (Player.HeldItem.type == ItemID.None)
			return;
		if (item != Player.HeldItem) {
			if (item != null && !CommonEnchantmentCheck()) {
				for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
					if (globalItem.EnchantmenStlot[i] == 0)
						continue;
					if (EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ForcedCleanCounter) {
						EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).PreCleanCounter(i, Player, globalItem, item);
						Array.Fill(globalItem.Item_Counter1, 0);
						Array.Fill(globalItem.Item_Counter2, 0);
						Array.Fill(globalItem.Item_Counter3, 0);
					}
				}
			}
			item = Player.HeldItem;
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem localglobal)) {
				globalItem = localglobal;
			}
		}
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyShootStat(i, Player, globalItem, item, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (CommonEnchantmentCheck()) {
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).Shoot(i, Player, globalItem, item, source, position, velocity, type, damage, knockback);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnMissingMana(Item item, int neededMana) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnMissingMana(i, Player, globalItem, item, neededMana);
		}
	}
	public override void ModifyWeaponCrit(Item item, ref float crit) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyCriticalStrikeChance(i, Player, globalItem, item, ref crit);
		}
	}
	public override void ModifyItemScale(Item item, ref float scale) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyItemScale(i, Player, globalItem, item, ref scale);
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyDamage(i, Player, globalItem, item, ref damage);
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyHitNPCWithItem(i, Player, globalItem, item, target, ref modifiers);
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitNPCWithItem(i, Player, globalItem, item, target, hit, damageDone);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyHitNPCWithProj(i, Player, globalItem, proj, target, ref modifiers);
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitNPCWithProj(i, Player, globalItem, proj, target, hit, damageDone);
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByAnything(Player);
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByNPC(i, globalItem, Player, npc, hurtInfo);
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByAnything(Player);
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByProjectile(i, globalItem, Player, proj, hurtInfo);
		}
	}
	public override void OnConsumeMana(Item item, int manaConsumed) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnConsumeMana(i, Player, globalItem, item, manaConsumed);
		}
	}
	public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyManaCost(i, Player, globalItem, item, ref reduce, ref mult);
		}
	}
	public override float UseSpeedMultiplier(Item item) {
		float useSpeed = base.UseSpeedMultiplier(item);
		if (CommonEnchantmentCheck()) {
			return useSpeed;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyUseSpeed(i, Player, globalItem, item, ref useSpeed);
		}
		return useSpeed;
	}
	public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnKill(Player);
		}
	}
}
public class DivineHammerUIState : UIState {
	UIPanel Mainpanel;
	Roguelike_UIPanel HeaderPanel;
	UIPanel BodyPanel;
	Roguelike_UIImage enchantment;
	Roguelike_UIImage augmentation;
	Roguelike_UIImage soulBind;
	public WeaponEnchantmentUIslot weaponEnchantmentUIslot;
	ExitUI exit;

	public ItemHolderSlot AccAugmentSlot;
	public ItemHolderSlot AccSacrificeAugmentSlot;
	public ConfirmButton confirmButton;
	public ItemHolderSlot AccAugmentResult;

	public ItemHolderSlot armorholderSlot;
	public ItemHolderSlot soulBindUIslot;
	public ConfirmButton SoulBindconfirmButton;
	public ItemHolderSlot armorResultBindUIslot;

	public EnchantmentUIslot slot1, slot2, slot3;
	Asset<Texture2D> tex = TextureAssets.InventoryBack;
	public void GeneralInit() {
		Mainpanel = new UIPanel();
		Mainpanel.HAlign = .5f;
		Mainpanel.VAlign = .5f;
		Mainpanel.UISetWidthHeight(300, 250);
		Append(Mainpanel);

		HeaderPanel = new();
		HeaderPanel.Width.Percent = 1f;
		HeaderPanel.Height.Pixels = 80;
		HeaderPanel.BackgroundColor = Color.Black;
		HeaderPanel.BorderColor = Color.Black;
		HeaderPanel.BackgroundColor.A = 0;
		HeaderPanel.BorderColor.A = 0;
		Mainpanel.Append(HeaderPanel);

		BodyPanel = new();
		BodyPanel.Width.Percent = 1f;
		BodyPanel.Height.Pixels = Mainpanel.Height.Pixels - HeaderPanel.Height.Pixels - 30;
		BodyPanel.VAlign = 1f;
		Mainpanel.Append(BodyPanel);

		exit = new ExitUI(tex);
		exit.UISetWidthHeight(52, 52);
		exit.HAlign = 1f;
		HeaderPanel.Append(exit);
	}
	public void EnchantmentInit() {
		enchantment = new(tex);
		enchantment.SetPostTex(ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<DivineHammerUIState>("WeaponEnchantment")), true);
		enchantment.UISetWidthHeight(52, 52);
		enchantment.OnLeftClick += Universal_OnLeftClick;
		enchantment.HighlightColor = enchantment.Color.ScaleRGB(0.5f);
		enchantment.Highlight = true;
		HeaderPanel.Append(enchantment);

		weaponEnchantmentUIslot = new WeaponEnchantmentUIslot(tex);
		weaponEnchantmentUIslot.UISetWidthHeight(52, 52);
		BodyPanel.Append(weaponEnchantmentUIslot);

		slot1 = new EnchantmentUIslot(tex);
		slot1.HAlign = 0;
		slot1.VAlign = 1f;
		slot1.WhoAmI = 0;
		slot1.Hide = true;
		BodyPanel.Append(slot1);

		slot2 = new EnchantmentUIslot(tex);
		slot2.HAlign = .5f;
		slot2.VAlign = 1f;
		slot2.WhoAmI = 1;
		slot2.Hide = true;
		BodyPanel.Append(slot2);

		slot3 = new EnchantmentUIslot(tex);
		slot3.HAlign = 1;
		slot3.VAlign = 1f;
		slot3.WhoAmI = 2;
		slot3.Hide = true;
		BodyPanel.Append(slot3);
	}
	public void Visual_Enchantment(bool hide) {
		weaponEnchantmentUIslot.Hide = hide;
	}
	Roguelike_UIPanel AugmentationSelection;
	Roguelike_UIPanel AugmentationSelection_Head;
	Roguelike_UIPanel AugmentationSelection_Body;
	public void AugmentationInit() {
		textlist.Clear();
		augmentation = new(tex);
		augmentation.SetPostTex(ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<DivineHammerUIState>("Augmentation")), true);
		augmentation.UISetWidthHeight(52, 52);
		augmentation.MarginLeft = enchantment.Width.Pixels + 10;
		augmentation.OnLeftClick += Universal_OnLeftClick;
		augmentation.HighlightColor = augmentation.Color.ScaleRGB(0.5f);
		HeaderPanel.Append(augmentation);

		AccAugmentSlot = new(tex);
		AccAugmentSlot.HAlign = 0;
		AccAugmentSlot.VAlign = .5f;
		AccAugmentSlot.Hide = true;
		AccAugmentSlot.SetPostTex(TextureAssets.Item[ItemID.AvengerEmblem], attemptToLoad: true);
		AccAugmentSlot.drawInfo.Opacity = .3f;
		AccAugmentSlot.OnLeftClick += AccAugmentSlot_OnLeftClick;
		BodyPanel.Append(AccAugmentSlot);

		AccSacrificeAugmentSlot = new(tex);
		AccSacrificeAugmentSlot.HAlign = .33f;
		AccSacrificeAugmentSlot.VAlign = .5f;
		AccSacrificeAugmentSlot.Hide = true;
		AccSacrificeAugmentSlot.OnLeftClick += AccAugmentSlot_OnLeftClick;
		BodyPanel.Append(AccSacrificeAugmentSlot);

		confirmButton = new(tex);
		confirmButton.HAlign = .66f;
		confirmButton.VAlign = .5f;
		confirmButton.Hide = true;
		confirmButton.OnLeftClick += ConfirmButton_OnLeftClick;
		confirmButton.SetPostTex(ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<DivineHammerUIState>("Augmentation")), true);
		confirmButton.SetVisibility(.7f, 1f);
		BodyPanel.Append(confirmButton);

		AccAugmentResult = new(tex);
		AccAugmentResult.HAlign = 1f;
		AccAugmentResult.VAlign = .5f;
		AccAugmentResult.Hide = true;
		AccAugmentResult.OnLeftClick += AccAugmentSlot_OnLeftClick;
		BodyPanel.Append(AccAugmentResult);

		Vector2 position = Mainpanel.GetOuterDimensions().Position();
		AugmentationSelection = new();
		AugmentationSelection.Top.Set(position.Y + Mainpanel.Height.Pixels, 0);
		AugmentationSelection.UISetWidthHeight(400, 400);
		AugmentationSelection.Hide = true;
		AugmentationSelection.HAlign = .5f;
		AugmentationSelection.VAlign = .5f;
		AugmentationSelection.BackgroundColor = AugmentationSelection.BackgroundColor with { A = 255 };
		AugmentationSelection.SetPadding(5);
		Append(AugmentationSelection);

		float num = 4;
		for (int x = 0; x < num; x++) {
			for (int y = 0; y < num + 2; y++) {
				int counter = (int)(x * num) + y + 1;
				ModAugments aug = AugmentsLoader.GetAugments(counter);
				string augText = "";
				Color color = Color.White;
				if (aug != null) {
					augText = aug.DisplayName;
					color = aug.tooltipColor;
				}
				AugmentationText btn = new(counter, augText, .7f);
				btn.BorderColor = color;
				btn.VAlign = MathHelper.Lerp(0f, 1f, y / (num + 1));
				btn.HAlign = MathHelper.Lerp(0f, 1f, x / (num - 1f));
				btn.UISetWidthHeight(100, 10);
				btn.OnLeftClick += Btn_OnLeftClick;
				btn.OnUpdate += Btn_OnUpdate;
				btn.SetPadding(8);
				AugmentationSelection.Append(btn);
				textlist.Add(btn);
			}
		}
	}

	private void Btn_OnUpdate(UIElement affectedElement) {
		if (affectedElement is AugmentationText btn) {
			if (btn.AugmentationType != SelectedAugmentationType) {
				btn.TextColor = Color.White;
			}
		}
	}

	int SelectedAugmentationType = 0;
	private void Btn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (listeningElement is AugmentationText btn) {
			SelectedAugmentationType = btn.AugmentationType;
			btn.TextColor = Color.Yellow;
		}
	}

	List<AugmentationText> textlist = new();
	private void ConfirmButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (AccAugmentSlot.item == null || AccAugmentSlot.item.type == 0) {
			return;
		}
		if (AccSacrificeAugmentSlot.item == null || AccSacrificeAugmentSlot.item.type == 0) {
			return;
		}
		if (AccAugmentResult.item != null && AccAugmentResult.item.type != 0) {
			return;
		}
		Item item = AccAugmentSlot.item;
		if (!item.GetGlobalItem<AugmentsWeapon>().AugmentsSlots.Contains(0)) {
			return;
		}
		int[] slots = item.GetGlobalItem<AugmentsWeapon>().AugmentsSlots;
		int counter = 0;
		for (int i = 0; i < slots.Length; i++) {
			if (slots[i] == 0) {
				continue;
			}
			counter++;
		}
		if (counter >= 1) {
			return;
		}
		if (AugmentsLoader.GetAugments(SelectedAugmentationType) == null) {
			return;
		}
		AugmentsWeapon.AddAugments(Main.LocalPlayer, ref item, SelectedAugmentationType);
		AccSacrificeAugmentSlot.item.TurnToAir();
		AccAugmentResult.item = item.Clone();
		AccAugmentSlot.item.TurnToAir();
	}

	private void AccAugmentSlot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		if (listeningElement.UniqueId == AccAugmentSlot.UniqueId) {
			Item item = Main.mouseItem;
			if (!item.accessory && item.type != ItemID.None) {
				return;
			}
			BossRushUtils.SimpleItemMouseExchange(player, ref AccAugmentSlot.item);
			if (AccAugmentSlot.item.type == ItemID.None) {
				AccAugmentSlot.drawInfo.Hide = true;
			}
			else {
				if (Main.mouseItem.type == ItemID.None) {
					AccAugmentSlot.drawInfo.Hide = false;
				}
			}
		}
		else if (listeningElement.UniqueId == AccSacrificeAugmentSlot.UniqueId) {
			Item item = Main.mouseItem;
			if (!item.accessory && item.type != ItemID.None) {
				return;
			}
			BossRushUtils.SimpleItemMouseExchange(player, ref AccSacrificeAugmentSlot.item);
		}
		else if (listeningElement.UniqueId == AccAugmentResult.UniqueId) {
			Item item = Main.mouseItem;
			if (item.type != 0) {
				return;
			}
			if (AccAugmentResult.item.type == 0) {
				return;
			}
			Main.mouseItem = AccAugmentResult.item.Clone();
			player.inventory[58] = AccAugmentResult.item.Clone();
			AccAugmentResult.item.TurnToAir();
		}
	}

	public void Visual_Augmentation(bool hide) {
		AccAugmentSlot.Hide = hide;
		AccSacrificeAugmentSlot.Hide = hide;
		confirmButton.Hide = hide;
		AccAugmentResult.Hide = hide;
		AugmentationSelection.Hide = hide;
	}
	public void SoulBindInit() {
		soulBind = new(tex);
		soulBind.SetPostTex(ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<DivineHammerUIState>("SoulBound")), true);
		soulBind.UISetWidthHeight(52, 52);
		soulBind.OnLeftClick += Universal_OnLeftClick;
		soulBind.MarginLeft = augmentation.Width.Pixels + enchantment.Width.Pixels + 20;
		soulBind.HighlightColor = soulBind.Color.ScaleRGB(0.5f);
		HeaderPanel.Append(soulBind);

		armorholderSlot = new(tex);
		armorholderSlot.HAlign = 0;
		armorholderSlot.Hide = true;
		armorholderSlot.OnLeftClick += ArmorholderSlot_OnLeftClick;
		armorholderSlot.VAlign = .5f;
		BodyPanel.Append(armorholderSlot);

		soulBindUIslot = new(tex);
		soulBindUIslot.HAlign = .33f;
		soulBindUIslot.Hide = true;
		soulBindUIslot.OnLeftClick += ArmorholderSlot_OnLeftClick;
		soulBindUIslot.VAlign = .5f;
		BodyPanel.Append(soulBindUIslot);

		SoulBindconfirmButton = new(tex);
		SoulBindconfirmButton.HAlign = .66f;
		SoulBindconfirmButton.Hide = true;
		SoulBindconfirmButton.OnLeftClick += SoulBindconfirmButton_OnLeftClick;
		SoulBindconfirmButton.VAlign = .5f;
		SoulBindconfirmButton.SetVisibility(.7f, 1f);
		BodyPanel.Append(SoulBindconfirmButton);

		armorResultBindUIslot = new(tex);
		armorResultBindUIslot.HAlign = 1;
		armorResultBindUIslot.Hide = true;
		armorResultBindUIslot.OnLeftClick += ArmorholderSlot_OnLeftClick;
		armorResultBindUIslot.VAlign = .5f;
		BodyPanel.Append(armorResultBindUIslot);
	}
	private void SoulBindconfirmButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (armorholderSlot.item == null || armorholderSlot.item.type == 0) {
			return;
		}
		if (soulBindUIslot.item == null || soulBindUIslot.item.type == 0) {
			return;
		}
		if (armorResultBindUIslot.item != null && armorResultBindUIslot.item.type != 0) {
			return;
		}
		if (soulBindUIslot.item.ModItem is BaseSoulBoundItem moditem) {
			if (SoulBoundGlobalItem.AddSoulBound(ref armorholderSlot.item, moditem.SoulBoundType)) {
				soulBindUIslot.item.TurnToAir();
				armorResultBindUIslot.item = armorholderSlot.item.Clone();
				armorholderSlot.item.TurnToAir();
			}
		}
	}
	private void ArmorholderSlot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Player player = Main.LocalPlayer;
		if (listeningElement.UniqueId == armorholderSlot.UniqueId) {
			Item item = Main.mouseItem;
			if (!item.IsThisArmorPiece() && item.type != 0) {
				return;
			}
			BossRushUtils.SimpleItemMouseExchange(player, ref armorholderSlot.item);
		}
		else if (listeningElement.UniqueId == soulBindUIslot.UniqueId) {
			Item item = Main.mouseItem;
			if ((item.ModItem == null || item.ModItem is not BaseSoulBoundItem) && item.type != 0) {
				return;
			}
			BossRushUtils.SimpleItemMouseExchange(Main.LocalPlayer, ref soulBindUIslot.item);
		}
		else if (listeningElement.UniqueId == armorResultBindUIslot.UniqueId) {
			Item item = Main.mouseItem;
			if (item.type != ItemID.None) {
				return;
			}
			if (armorResultBindUIslot.item.type == ItemID.None) {
				return;
			}
			Main.mouseItem = armorResultBindUIslot.item.Clone();
			player.inventory[58] = armorResultBindUIslot.item.Clone();
			armorResultBindUIslot.item.TurnToAir();
		}
	}
	public void Visual_SoulBound(bool hide) {
		armorholderSlot.Hide = hide;
		soulBindUIslot.Hide = hide;
		SoulBindconfirmButton.Hide = hide;
		armorResultBindUIslot.Hide = hide;
	}
	public override void OnInitialize() {
		GeneralInit();

		EnchantmentInit();

		AugmentationInit();

		SoulBindInit();
	}
	private void Universal_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		enchantment.Highlight = false;
		augmentation.Highlight = false;
		soulBind.Highlight = false;
		Visual_Augmentation(true);
		Visual_Enchantment(true);
		Visual_SoulBound(true);
		if (listeningElement.UniqueId == enchantment.UniqueId) {
			enchantment.Highlight = true;
			Visual_Enchantment(false);
		}
		else if (listeningElement.UniqueId == augmentation.UniqueId) {
			augmentation.Highlight = true;
			Visual_Augmentation(false);
		}
		else if (listeningElement.UniqueId == soulBind.UniqueId) {
			soulBind.Highlight = true;
			Visual_SoulBound(false);
		}
	}

	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (enchantment.IsMouseHovering) {
			Main.instance.MouseText("Weapon Enchantment");
		}
	}
	public override void OnDeactivate() {
		slot3.Hide = true;
		slot2.Hide = true;
		slot1.Hide = true;
	}
}
public class AugmentationText : Roguelike_UITextPanel {
	public int AugmentationType { get; private set; }
	public AugmentationText(int type, string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
		if (AugmentsLoader.GetAugments(type) == null) {
			return;
		}
		AugmentationType = type;
	}
}
public class WeaponEnchantmentUIslot : Roguelike_UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;
	private Texture2D texture;
	public WeaponEnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	List<int> textUqID = new List<int>();
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		//checking whenever or not if the mouse item is a actual weapon
		if (Main.mouseItem.type != ItemID.None) {
			if (!Main.mouseItem.IsAWeapon())
				return;
			Item itemcached;
			//Checking whenever or not the slot is empty or not
			if (item != null && item.type != ItemID.None) {
				//The slot is not empty and the the mouse have a item, we exchange item
				itemcached = item.Clone();
				item.TurnToAir();
				item = Main.mouseItem.Clone();
				Main.mouseItem = itemcached;
				player.inventory[58] = itemcached;
			}
			else {
				//The slot is empty, we clone the item into the slot
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
				UniversalSystem.EnchantingState = true;
			}
			//we are setting up the slot
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				DivineHammerUIState state = ModContent.GetInstance<UniversalSystem>().DivineHammer_uiState;
				state.slot1.WhoAmI = 0;
				state.slot1.text.SetText("1");
				state.slot1.itemOwner = item;
				state.slot1.itemType = globalItem.EnchantmenStlot[0];
				state.slot1.Hide = false;

				state.slot2.WhoAmI = 1;
				state.slot2.text.SetText("2");
				state.slot2.itemOwner = item;
				state.slot2.itemType = globalItem.EnchantmenStlot[1];
				state.slot2.Hide = false;

				state.slot3.WhoAmI = 2;
				state.slot3.text.SetText("3");
				state.slot3.itemOwner = item;
				state.slot3.itemType = globalItem.EnchantmenStlot[2];
				state.slot3.Hide = false;
			}
		}
		//In this case, it mean that the mouse item is empty
		else {
			//Checking whenver or not if the item holder is empty or not, if it is then do return and do nothing
			if (item == null)
				return;
			//It seem that it is not empty, we disable general stuff
			UniversalSystem.EnchantingState = false;
			Main.mouseItem = item;
			item = null;
			DivineHammerUIState state = ModContent.GetInstance<UniversalSystem>().DivineHammer_uiState;
			state.slot1.Hide = true;
			state.slot2.Hide = true;
			state.slot3.Hide = true;
		}
	}
	public void DropItem(Player player) {
		if (item == null)
			return;
		for (int i = 0; i < 50; i++) {
			if (player.CanItemSlotAccept(player.inventory[i], item)) {
				if (ModContent.GetInstance<UniversalSystem>().WorldState == "Exited") {
					ModContent.GetInstance<UniversalSystem>().IsAttemptingToBringItemToNewPlayer = true;
					return;
				}
				player.inventory[i] = item.Clone();
				item = null;
				return;
			}
		}
		player.DropItem(player.GetSource_DropAsItem(), player.Center, ref item);
		item = null;
	}
	public override void OnDeactivate() {
		Player player = Main.LocalPlayer;
		UniversalSystem.EnchantingState = false;
		DropItem(player);
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		Vector2 drawpos = this.GetInnerDimensions().Position() + texture.Size() * .5f;
		if (item != null) {
			if (IsMouseHovering) {
				UniversalSystem.EnchantingState = false;
				Main.HoverItem = item.Clone();
				Main.hoverItemName = item.HoverName;
			}
			else {
				UniversalSystem.EnchantingState = true;
			}
			Main.instance.LoadItem(item.type);
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Vector2 origin = texture.Size() * .5f;
			float scaling = 1;
			if (texture.Width > this.texture.Width || texture.Height > this.texture.Height) {
				scaling = ScaleCalculation(texture.Size()) * .68f;
			}
			spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
		}
		else {
			Texture2D backgroundtexture = TextureAssets.Item[ItemID.SilverBroadsword].Value;
			spriteBatch.Draw(backgroundtexture, drawpos, null, new Color(0, 0, 0, 80), 0, texture.Size() * .35f, ScaleCalculation(backgroundtexture.Size()) * .78f, SpriteEffects.None, 0);
		}
	}
	private float ScaleCalculation(Vector2 textureSize) => texture.Size().Length() / textureSize.Length();
}
public class ConfirmButton : Roguelike_UIImageButton {
	public ConfirmButton(Asset<Texture2D> texture) : base(texture) {
	}
}
public class EnchantmentUIslot : Roguelike_UIImage {
	public int itemType = 0;
	public int WhoAmI = -1;

	public Item itemOwner = null;
	private Texture2D texture;
	public UIText text;
	public EnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
		text = new((WhoAmI + 1).ToString());
		text.HAlign = .5f;
		text.VAlign = .5f;
		Append(text);
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (Main.LocalPlayer.GetModPlayer<EnchantmentModplayer>().SlotUnlock < WhoAmI) {
			return;
		}
		if (itemOwner == null)
			return;
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			if (itemType != 0)
				return;
			if (EnchantmentLoader.GetEnchantmentItemID(Main.mouseItem.type) == null)
				return;
			itemType = Main.mouseItem.type;
			Main.mouseItem.TurnToAir();
			Main.LocalPlayer.inventory[58].TurnToAir();
			EnchantmentSystem.EnchantItem(ref itemOwner, WhoAmI, itemType);
		}
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		try {
			if (Main.LocalPlayer.GetModPlayer<EnchantmentModplayer>().SlotUnlock < WhoAmI) {
				Texture2D lockTexture = ModContent.Request<Texture2D>(BossRushTexture.Lock).Value;
				Vector2 origin = lockTexture.Size() * .5f;
				Vector2 drawpos = this.GetInnerDimensions().Position() + texture.Size() * .5f;
				spriteBatch.Draw(lockTexture, drawpos, null, Color.White, 0, origin, .87f, SpriteEffects.None, 0);
				return;
			}
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Vector2 drawpos = this.GetInnerDimensions().Position() + texture.Size() * .5f;
				Main.instance.LoadItem(itemType);
				Texture2D texture1 = TextureAssets.Item[itemType].Value;
				Vector2 origin = texture1.Size() * .5f;
				spriteBatch.Draw(texture1, drawpos, null, Color.White, 0, origin, .87f, SpriteEffects.None, 0);
				if (IsMouseHovering) {
					string tooltipText = "No enchantment can be found";
					if (EnchantmentLoader.GetEnchantmentItemID(itemType) != null) {
						tooltipText = EnchantmentLoader.GetEnchantmentItemID(itemType).Description;
					}
					Item fakeItem = new Item(itemType, 1, 0);
					fakeItem.SetNameOverride(tooltipText);
					Main.HoverItem = fakeItem;
					Main.hoverItemName = tooltipText;
					Main.instance.MouseText("");
					Main.mouseText = true;
				}
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	public override void UpdateImage(GameTime gameTime) {
		if (itemType == ItemID.None)
			return;
		text.SetText("");
	}
}
