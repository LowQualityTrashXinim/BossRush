using System;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using System.Linq;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Common.Mode.DreamLikeWorldMode;

namespace BossRush.Contents.WeaponEnchantment;
public class EnchantmentSystem : ModSystem {
	public static void EnchantmentRNG(Player self, Item item) {
		if (item == null || !EnchantmentGlobalItem.CanBeEnchanted(item)) {
			return;
		}
		EnchantmentModplayer modplayer = self.GetModPlayer<EnchantmentModplayer>();
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
			if (Main.rand.NextFloat() <= randomizedchance + modplayer.RandomizeChanceEnchantment) {
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
		if (EnchantmenStlot.Length < 4 || clone.EnchantmenStlot.Length < 4) {
			Array.Resize(ref EnchantmenStlot, 4);
			Array.Resize(ref clone.EnchantmenStlot, 4);
		}
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
	public float RandomizeChanceEnchantment = .2f;
	public void SafeRequest_EnchantItem(int requestAmount, int amountEnchant) {
		Request_EnchantedItem = requestAmount;
		Request_EnchantedAmount = amountEnchant;
	}
	public int Request_EnchantedItem = 0;
	public int Request_EnchantedAmount = 1;
	public override void ResetEffects() {
		RandomizeChanceEnchantment = 0;
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
	public override void UpdateEquips() {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).Update(Player);
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
internal class EnchantmentUIState : UIState {
	UIPanel panel;
	WeaponEnchantmentUIslot weaponEnchantmentUIslot;
	ExitUI weaponEnchantmentUIExit;
	bool isMousePressed = false;
	Vector2 position = Main.ScreenSize.ToVector2() / 2f;
	Vector2 panelSize = new Vector2(70 * 3 - 8, 62 * 2 + 8);
	Vector2 UIclampOffset = new Vector2(60, 60);
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.UISetPosition(Main.LocalPlayer.Center, panelSize / 2f);
		panel.OnLeftMouseDown += mousePressed;
		panel.OnLeftMouseUp += mouseUp;
		panel.UISetWidthHeight(panelSize.X, panelSize.Y);

		Append(panel);

		weaponEnchantmentUIslot = new WeaponEnchantmentUIslot(TextureAssets.InventoryBack);
		weaponEnchantmentUIslot.UISetWidthHeight(52, 52);
		weaponEnchantmentUIslot.UISetPosition(position + Vector2.UnitX * 120, new Vector2(26, 26));
		Append(weaponEnchantmentUIslot);
		weaponEnchantmentUIExit = new ExitUI(TextureAssets.InventoryBack13);
		weaponEnchantmentUIExit.UISetWidthHeight(52, 52);
		weaponEnchantmentUIExit.UISetPosition(position + Vector2.UnitX * 178, new Vector2(26, 26));
		Append(weaponEnchantmentUIExit);
	}

	public override void Update(GameTime gameTime) {
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		position = Vector2.Clamp(position, Vector2.Zero + UIclampOffset * Main.UIScale, Main.ScreenSize.ToVector2() - UIclampOffset * Main.UIScale);
		if (isMousePressed)
			this.position = Vector2.Clamp(Main.MouseScreen, Vector2.Zero + UIclampOffset * Main.UIScale, Main.ScreenSize.ToVector2() - UIclampOffset * Main.UIScale);
		for (int i = 0; i < Children.Count(); i++) {
			var children = Children.ElementAt(i);
			if (children is MoveableUIImage) {
				var child = children as MoveableUIImage;
				child.UISetPosition(position + child.positionOffset);
				child.position = position;
			}
		}
		panel.UISetPosition(position);
		weaponEnchantmentUIExit.UISetPosition(position + new Vector2(60, 0));
	}

	private void mousePressed(UIMouseEvent evt, UIElement listeningElement) {
		isMousePressed = true;
	}


	private void mouseUp(UIMouseEvent evt, UIElement listeningElement) {
		isMousePressed = false;
	}

	public override void OnDeactivate() {
		int count = Children.Count();
		for (int i = count - 1; i >= 0; i--) {
			UIElement child = Children.ElementAt(i);
			if (child is EnchantmentUIslot wmslot) {
				if (wmslot.itemOwner == null) {
					continue;
				}
				else {
					child.Deactivate();
					child.Remove();
				}
			}
			if (child is UIText) {
				child.Deactivate();
				child.Remove();
			}
		}
	}
}
public class MoveableUIImage : UIImage {
	public MoveableUIImage(Asset<Texture2D> texture) : base(texture) {
	}

	public Vector2 positionOffset = Vector2.Zero;
	public Vector2 position = Vector2.Zero;

}

public class WeaponEnchantmentUIslot : MoveableUIImage {
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
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			Item itemcached;
			if (item != null && item.type != ItemID.None) {
				itemcached = item.Clone();
				item = Main.mouseItem.Clone();
				Main.mouseItem = itemcached.Clone();
				player.inventory[58] = itemcached.Clone();
			}
			else {
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
				UniversalSystem.EnchantingState = true;
			}
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				int length = globalItem.EnchantmenStlot.Length - 1;
				for (int i = 0; i < length; i++) {
					EnchantmentUIslot slot = new EnchantmentUIslot(TextureAssets.InventoryBack);
					slot.positionOffset = Vector2.UnitY * 60 + Vector2.UnitX * 60 * i;
					slot.UISetWidthHeight(52, 52);
					slot.WhoAmI = i;
					slot.itemOwner = item;
					slot.itemType = globalItem.EnchantmenStlot[i];
					Parent.Append(slot);
					UIText text = new UIText($"{i + 1}");
					text.UISetPosition(positionOffset + Vector2.UnitY * 56, new Vector2(26, 26));
					textUqID.Add(text.UniqueId);
					Parent.Append(text);
				}
			}
		}
		else {
			if (item == null)
				return;
			UniversalSystem.EnchantingState = false;
			Main.mouseItem = item;
			item = null;
			int count = Parent.Children.Count();
			for (int i = count - 1; i >= 0; i--) {
				UIElement child = Parent.Children.ElementAt(i);
				if (child is EnchantmentUIslot wmslot) {
					if (wmslot.itemOwner == null)
						continue;
				}
				if (child is EnchantmentUIslot { itemOwner: not null }) {
					child.Deactivate();
					child.Remove();
				}
				if (child is UIText text && textUqID.Contains(text.UniqueId)) {
					textUqID.Remove(text.UniqueId);
					child.Deactivate();
					child.Remove();
				}
			}
		}
	}
	public override void OnDeactivate() {
		textUqID.Clear();
		Player player = Main.LocalPlayer;
		UniversalSystem.EnchantingState = false;
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
	public override void Draw(SpriteBatch spriteBatch) {
		Vector2 drawpos = position + positionOffset + texture.Size() * .5f;
		base.Draw(spriteBatch);
		if (item != null) {
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
public class EnchantmentUIslot : MoveableUIImage {
	public int itemType = 0;
	public int WhoAmI = -1;

	public Item itemOwner = null;
	private Texture2D texture;
	public EnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	public override void LeftClick(UIMouseEvent evt) {
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
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		try {
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
				Main.instance.LoadItem(itemType);
				Texture2D texture1 = TextureAssets.Item[itemType].Value;
				Vector2 origin = texture1.Size() * .5f;
				spriteBatch.Draw(texture1, drawpos, null, Color.White, 0, origin, .87f, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (itemType == ItemID.None)
			return;
		if (IsMouseHovering) {
			string tooltipText = "No enchantment can be found";
			if (EnchantmentLoader.GetEnchantmentItemID(itemType) != null) {
				tooltipText = EnchantmentLoader.GetEnchantmentItemID(itemType).Description;
			}
			Main.instance.MouseText(tooltipText);
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
}
