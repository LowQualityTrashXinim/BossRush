using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using ReLogic.Graphics;
using BossRush.Texture;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Localization;
using BossRush.Contents.NPCs;
using BossRush.Common.Global;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using BossRush.Contents.Perks;
using Terraria.GameContent.UI;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Transfixion.Arguments;
using BossRush.Contents.BuffAndDebuff.PlayerDebuff;
using BossRush.Contents.Transfixion.WeaponEnchantment;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Annihiliation;
using BossRush.Common.Systems.IOhandle;
using BossRush.Common.ChallengeMode;

namespace BossRush.Contents.Items.Weapon {
	public struct SynergyBonus {
		public int ItemID;
		public bool Active;
		public string Tooltip = "";

		public SynergyBonus(int id) {
			ItemID = id;
		}
		public SynergyBonus(int id, string tooltip) {
			ItemID = id;
			Tooltip = tooltip;
		}
	}
	/// <summary>
	/// This is synergy bonus system, this system will automatically handle most of the bonus action for you<br/>
	/// No need to manual set, nor anything, you only need to check whenever or not if a bonus is active or not
	/// </summary>
	public class SynergyBonus_System : ModSystem {
		public static Dictionary<int, List<SynergyBonus>> Dictionary_SynergyBonus = new();
		public override void Load() {
			Dictionary_SynergyBonus = new();
		}
		public override void Unload() {
			Dictionary_SynergyBonus = null;
		}
		public static void Add_SynergyBonus(int SynergyItemID, int ItemID, string tooltip = "") {
			if (Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				if (Dictionary_SynergyBonus[SynergyItemID].Select(b => b.ItemID).ToArray().Contains(ItemID)) {
					return;
				}
				Dictionary_SynergyBonus[SynergyItemID].Add(new(ItemID, tooltip));
				return;
			}
			Dictionary_SynergyBonus.Add(SynergyItemID, new() { { new(ItemID, tooltip) } });
		}
		/// <summary>
		/// Check if the synergy bonus is active or not<br/>
		/// <b>Note :</b> If you are checking a item group, check the key item instead
		/// </summary>
		/// <param name="SynergyItemID"></param>
		/// <param name="ItemID"></param>
		/// <returns></returns>
		public static bool Check_SynergyBonus(int SynergyItemID, int ItemID) {
			if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				return false;
			}
			for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
				SynergyBonus bonus = Dictionary_SynergyBonus[SynergyItemID][i];
				if (bonus.ItemID == ItemID) {
					return bonus.Active;
				}
			}
			return false;
		}
		public static string Get_SynergyBonusTooltip(int SynergyItemID, int itemID) {
			if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				return "Synergy item not found !";
			}
			for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
				SynergyBonus bonus = Dictionary_SynergyBonus[SynergyItemID][i];
				if (bonus.ItemID == itemID) {
					return bonus.Tooltip;
				}
			}
			return "Synergy bonus item not found !";
		}
		public static void Write_SynergyTooltip(ref List<TooltipLine> lines, SynergyModItem moditem, int itemID) {
			int SynergyItemID = moditem.Type;
			if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				return;
			}
			SynergyBonus bonus = new();
			for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
				if (Dictionary_SynergyBonus[SynergyItemID][i].ItemID == itemID) {
					bonus = Dictionary_SynergyBonus[SynergyItemID][i];
				}
			}
			if (bonus.Active)
				lines.Add(new(moditem.Mod, moditem.Set_TooltipName(itemID), bonus.Tooltip));
		}

		public bool GodAreEnraged = false;
		public int CooldownCheck = 999;
		private void SynergyEnergyCheckPlayer(Player player) {
			int synergyCounter = 0;
			synergyCounter += player.CountItem(ModContent.ItemType<SynergyEnergy>(), 2);
			synergyCounter += player.inventory.Where(itemInv => itemInv.ModItem is SynergyModItem).Count();
			int maxCount = NPC.GetActivePlayerCount() + 1;
			if (synergyCounter >= maxCount) {
				GodAreEnraged = true;
			}
		}
		private void GodDecision(Player player) {
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			if (NPC.AnyNPCs(ModContent.NPCType<Guardian>()) || player.GetModPlayer<ChestLootDropPlayer>().CanDropSynergyEnergy)
				return;
			if (player.IsDebugPlayer())
				return;
			CooldownCheck = BossRushUtils.CountDown(CooldownCheck);
			//Main.NewText(CooldownCheck);
			if (CooldownCheck <= 0) {
				SynergyEnergyCheckPlayer(player);
			}
			if (GodAreEnraged) {
				Vector2 randomSpamLocation = Main.rand.NextVector2CircularEdge(1500, 1500) + player.Center;
				NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)randomSpamLocation.X, (int)randomSpamLocation.Y, ModContent.NPCType<Guardian>());
				BossRushUtils.CombatTextRevamp(player.Hitbox, Color.Red, "You have anger the God!");
				CooldownCheck = 999;
				GodAreEnraged = false;
			}
		}
		public override void PostUpdateWorld() {
		}
	}
	/// <summary>
	///This mod player should hold all the logic require for the item, if the item is shooting out the projectile, it should be doing that itself !<br/>
	///Same with projectile unless it is a vanilla projectile then we can refer to global projectile<br/>
	///This should only hold custom bool or data that we think should be hold/use/transfer<br/>
	/// </summary>
	public class PlayerSynergyItemHandle : ModPlayer {
		public bool SynergyBonusBlock = false;
		public int SynergyBonus = 0;

		public int EnergyBlade_Code1_Energy = 0;

		public int StreetLamp_VampireFrogStaff_HitCounter = 0;
		public int Annihiliation_Counter = 0;

		public int SinisterBook_DemonScythe_Counter = 0;

		public bool QuadDemonBlaster = false;
		public float QuadDemonBlaster_SpeedMultiplier = 1;
		public override void ResetEffects() {
			SynergyBonus = 0;
			SynergyBonusBlock = false;

			if (Player.HeldItem.type != ModContent.ItemType<Annihiliation>()) {
				Annihiliation_Counter = 0;
			}
			if (!BossRushModSystem.SynergyItem.Select(i => i.type).Contains(Player.HeldItem.type)) {
				return;
			}
			int synergyItem = Player.HeldItem.type;
			if (!SynergyBonus_System.Dictionary_SynergyBonus.ContainsKey(synergyItem)) {
				return;
			}
			int SynergyBonusLength = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem].Count;
			for (int l = 0; l < SynergyBonusLength; l++) {
				int itemIDBonus = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l].ItemID;
				bool HasItem = Player.HasItem(itemIDBonus);
				if (HasItem) {
					SynergyBonus++;
				}
				SynergyBonus bonus = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l];
				bonus.Active = HasItem;
				SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l] = bonus;
			}
		}
	}
	/// <summary>
	/// This class hold mainly tooltip information<br/>
	/// However this doesn't handle overhaul information
	/// </summary>
	public class GlobalItemHandle : GlobalItem {
		public const byte None = 0;
		public override bool InstancePerEntity => true;
		public bool LostAccessories = false;
		public bool DebugItem = false;
		public bool ExtraInfo = false;
		public bool AdvancedBuffItem = false;
		public bool RPGItem = false;
		public bool OverrideVanillaEffect = false;
		public override void OnCreated(Item item, ItemCreationContext context) {
			if (item.ModItem == null) {
				return;
			}
			if (item.ModItem is SynergyModItem && context is RecipeItemCreationContext) {
				LootBoxBase.AmmoForWeapon(Main.LocalPlayer, item.type);
			}
		}
		public override void SetDefaults(Item entity) {
		}
		public float CriticalDamage;
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (UniversalSystem.EnchantingState) {
				return;
			}
			//tooltips.Add(new(Mod, "Debug", $"Item width : {item.width} | height {item.height}"));
			if (item.IsAWeapon(true)) {
				for (int i = 0; i < tooltips.Count; i++) {
					TooltipLine line = tooltips[i];
					if (line.Name == "CritChance") {
						tooltips.Insert(i + 1, new(Mod, "CritDamage", $"{Math.Round(CriticalDamage, 2) * 100}% bonus critical damage"));
						tooltips.Insert(i + 2, new(Mod, "ArmorPenetration", $"{item.ArmorPenetration} Armor penetration"));
					}
					else if (line.Name == "Damage") {
						line.Text = line.Text + $" | Base : {item.OriginalDamage}";
					}
					else if (line.Name == "Knockback") {
						line.Text = line.Text + $" | Base : {Math.Round(ContentSamples.ItemsByType[item.type].knockBack, 2)} | Modified : {Math.Round(Main.LocalPlayer.GetWeaponKnockback(item), 2)}";
					}
				}
			}
			if (ModContent.GetInstance<UniversalSystem>().user2ndInterface.CurrentState == ModContent.GetInstance<UniversalSystem>().transmutationUI) {
				tooltips.Add(new(Mod, "RarityValue", $"Rarity : [c/{ItemRarity.GetColor(item.OriginalRarity).Hex3()}:{item.OriginalRarity}]"));
			}
			if (item.ModItem == null) {
				return;
			}
			if (item.ModItem.Mod != Mod) {
				return;
			}
			TooltipLine NameLine = tooltips.Where(t => t.Name == "ItemName").FirstOrDefault();
			if (DebugItem && NameLine != null) {
				NameLine.Text += " [Debug]";
				NameLine.OverrideColor = Color.MediumPurple;
				return;
			}
			ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
			if (ExtraInfo && item.ModItem != null) {
				if (!moddedplayer.Shift_Option()) {
					tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for more infomation]") { OverrideColor = Color.Gray });
				}
			}
			if (item.accessory && LostAccessories) {
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = Color.DarkGoldenrod;
				tooltips.Add(new TooltipLine(Mod, "LostAcc_" + item.type, "Lost Accessory") { OverrideColor = Color.LightGoldenrodYellow });
			}
			if (AdvancedBuffItem && NameLine != null) {
				NameLine.Text += " [Advanced]";
			}
		}
		public override void PostUpdate(Item item) {
			if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE) && ModContent.GetInstance<BossRushWorldGen>().BossRushWorld) {
				if (!Main.LocalPlayer.dead && item.type != ItemID.Heart && item.type != ItemID.Star && item.position.IsCloseToPosition(Main.LocalPlayer.Center, 1000)) {
					item.velocity = (Main.LocalPlayer.Center - item.Center).SafeNormalize(Vector2.Zero) * 5;
				}
			}
		}
		public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y) {
			if (item.ModItem == null) {
				return true;
			}
			//Prevent possible conflict, basically hardcoding to make it so that it only work for item belong to this mod
			if (item.ModItem.Mod != Mod) {
				return true;
			}
			ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
			if (ExtraInfo && item.ModItem != null)
				if (moddedplayer.Shift_Option()) {
					float width;
					float height = -16;
					Vector2 pos;
					string value = $"Mods.BossRush.Items.{item.ModItem.Name}.ExtraInfo";
					string ExtraInfo = Language.GetTextValue(value);
					DynamicSpriteFont font = FontAssets.MouseText.Value;
					if (Main.MouseScreen.X < Main.screenWidth / 2) {
						string widest = lines.OrderBy(n => ChatManager.GetStringSize(font, n.Text, Vector2.One).X).Last().Text;
						width = ChatManager.GetStringSize(font, widest, Vector2.One).X;
						pos = new Vector2(x, y) + new Vector2(width + 30, 0);
					}
					else {
						width = ChatManager.GetStringSize(font, ExtraInfo, Vector2.One).X + 20;
						pos = new Vector2(x, y) - new Vector2(width + 30, 0);
					}
					width = ChatManager.GetStringSize(font, ExtraInfo, Vector2.One).X + 20;
					height += ChatManager.GetStringSize(font, ExtraInfo, Vector2.One).Y + 16;
					Utils.DrawInvBG(Main.spriteBatch, new Rectangle((int)pos.X - 10, (int)pos.Y - 10, (int)width + 20, (int)height + 20), new Color(25, 100, 55) * 0.85f);
					Utils.DrawBorderString(Main.spriteBatch, ExtraInfo, pos, Color.White);
					pos.Y += ChatManager.GetStringSize(font, ExtraInfo, Vector2.One).Y + 16;
				}
			return base.PreDrawTooltip(item, lines, ref x, ref y);
		}
		public override bool? UseItem(Item item, Player player) {
			if (AdvancedBuffItem && !UniversalSystem.CanAccessContent(player, UniversalSystem.BOSSRUSH_MODE)) {
				player.AddBuff(ModContent.BuffType<Drawback>(), BossRushUtils.ToMinute(6));
			}
			return base.UseItem(item, player);
		}
	}
	public abstract class SynergyModItem : ModItem {
		public string Set_TooltipName(int ItemID) => $"{Name}_{ContentSamples.ItemsByType[ItemID].Name}";
		public sealed override void SetStaticDefaults() {
			ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<SynergyEnergy>();
			CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(50, 100, 100) });
			Synergy_SetStaticDefaults();
		}
		public virtual void Synergy_SetStaticDefaults() { }
		public ColorInfo CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(100, 150, 150) });
		public override sealed void ModifyTooltips(List<TooltipLine> tooltips) {
			ModifySynergyToolTips(ref tooltips, Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>());
			if (CustomColor != null) {
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = CustomColor.MultiColor(5);
			}
		}
		public override sealed void ModifyWeaponCrit(Player player, ref float crit) {
			Synergy_ModifyWeaponCrit(player, ref crit);
			if (!player.HasPerk<UntappedPotential>()) {
				return;
			}
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			crit += 4 * modplayer.SynergyBonus;
		}
		public virtual void Synergy_ModifyWeaponCrit(Player player, ref float crit) { }
		public override sealed void ModifyWeaponDamage(Player player, ref StatModifier damage) {
			Synergy_ModifyWeaponDamage(player, ref damage);
			if (!player.HasPerk<UntappedPotential>()) {
				return;
			}
			float damageIncreasement = 0;
			float damageMultiplier = 0;
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (modplayer.SynergyBonus > 0) {
				damageMultiplier += 0.025f * modplayer.SynergyBonus;
			}
			else {
				damageMultiplier += 0.01f;
			}
			for (int i = 0; player.inventory.Length > 0; i++) {
				if (i > 50) {
					break;
				}
				Item item = player.inventory[i];
				if (!item.IsAWeapon() || item == Item || item.ModItem is SynergyModItem) {
					continue;
				}
				damageIncreasement += player.inventory[i].damage * damageMultiplier;
			}
			damage += damageIncreasement;
		}
		public virtual void Synergy_ModifyWeaponDamage(Player player, ref StatModifier damage) { }
		public virtual void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) { }
		public override sealed void HoldItem(Player player) {
			string internalItemName = Item.ModItem.Name;
			if (!SynergyBonus_System.Dictionary_SynergyBonus.ContainsKey(Type)) {
				return;
			}
			List<SynergyBonus> listBonus = SynergyBonus_System.Dictionary_SynergyBonus[Type];
			if (!RoguelikeData.SynergyProgressTracker.ContainsKey(internalItemName)) {
				RoguelikeData.SynergyProgressTracker.Add(internalItemName, new());
				foreach (SynergyBonus bonus in listBonus) {
					SynergyBonus defaultBonus = new(bonus.ItemID);
					RoguelikeData.SynergyProgressTracker[internalItemName].Add(defaultBonus);
				}
			}
			else {
				if (RoguelikeData.SynergyProgressTracker[internalItemName].Count != listBonus.Count) {
					RoguelikeData.SynergyProgressTracker[internalItemName].Clear();
					foreach (SynergyBonus bonus in listBonus) {
						SynergyBonus defaultBonus = new(bonus.ItemID);
						RoguelikeData.SynergyProgressTracker[internalItemName].Add(defaultBonus);
					}
				}
				for (int i = 0; i < listBonus.Count; i++) {
					if (listBonus[i].Active) {
						SynergyBonus bonus = listBonus[i];
						bonus.Tooltip = "";
						RoguelikeData.SynergyProgressTracker[internalItemName][i] = bonus;
					}
				}
			}
		}
		public override sealed void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			ModifySynergyShootStats(player, player.GetModPlayer<PlayerSynergyItemHandle>(), ref position, ref velocity, ref type, ref damage, ref knockback);
		}
		public override sealed void UpdateInventory(Player player) {
			base.UpdateInventory(player);
			//Very funny that hold item happen after ModifyWeaponDamage
			//This probably will tank our mod performance, but well, it is what it is
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (player.HeldItem == Item) {
				HoldSynergyItem(player, modplayer);
			}
			SynergyUpdateInventory(player, modplayer);
		}
		public virtual void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {

		}
		public virtual void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

		}
		/// <summary>
		/// You should use this to set condition, the condition must be pre set in <see cref="PlayerSynergyItemHandle"/> and then check condition in here
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			SynergyShoot(player, player.GetModPlayer<PlayerSynergyItemHandle>(), source, position, velocity, type, damage, knockback, out bool CanShootItem);
			return CanShootItem;
		}
		public virtual void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) { CanShootItem = true; }
		public override sealed void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			base.OnHitNPC(player, target, hit, damageDone);
			OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
		}
		public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) { }

		private int countX = 0;
		private float positionRotateX = 0;
		private void PositionHandle() {
			if (positionRotateX < 3.5f && countX == 1) {
				positionRotateX += .2f;
			}
			else {
				countX = -1;
			}
			if (positionRotateX > 0 && countX == -1) {
				positionRotateX -= .2f;
			}
			else {
				countX = 1;
			}
		}
		Color auraColor;
		private void ColorHandle() {
			switch (Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus) {
				case 1:
					auraColor = new Color(255, 50, 0, 30);
					break;
				case 2:
					auraColor = new Color(255, 255, 0, 30);
					break;
				case 3:
					auraColor = new Color(0, 255, 255, 30);
					break;
				default:
					auraColor = new Color(255, 255, 255, 30);
					break;
			}
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			ColorHandle();
			if (ItemID.Sets.AnimatesAsSoul[Type] || Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus < 1) {
				return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
			}
			Main.instance.LoadItem(Type);
			Texture2D texture = TextureAssets.Item[Type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(1.5f, 1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(1.5f, -1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, 1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, -1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
	public abstract class SynergyModProjectile : ModProjectile {
		public virtual void SpawnDustPostPostAI(Player player) { }
		public override sealed bool PreAI() {
			Player player = Main.player[Projectile.owner];
			SynergyPreAI(player, player.GetModPlayer<PlayerSynergyItemHandle>(), out bool stopAI);
			return stopAI;
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		/// <param name="runAI"></param>
		public virtual void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) { runAI = true; }
		public override sealed void AI() {
			Player player = Main.player[Projectile.owner];
			SynergyAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed void PostAI() {
			Player player = Main.player[Projectile.owner];
			SynergyPostAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
			SpawnDustPostPostAI(player);
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void SynergyPostAI(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			Player player = Main.player[Projectile.owner];
			ModifyHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, ref modifiers);
		}
		public virtual void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) { }
		public override sealed void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Player player = Main.player[Projectile.owner];
			OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
		}
		public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) { }
		public override sealed void OnKill(int timeLeft) {
			base.OnKill(timeLeft);
			Player player = Main.player[Projectile.owner];
			SynergyKill(player, player.GetModPlayer<PlayerSynergyItemHandle>(), timeLeft);
		}
		public virtual void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		}
	}
	public abstract class SynergyBuff : ModBuff {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override sealed void SetStaticDefaults() {
			base.SetStaticDefaults();
			SynergySetStaticDefaults();
		}
		public virtual void SynergySetStaticDefaults() {

		}
		public override sealed void Update(Player player, ref int buffIndex) {
			base.Update(player, ref buffIndex);
			UpdatePlayer(player, ref buffIndex);
		}
		public virtual void UpdatePlayer(Player player, ref int buffIndex) {

		}
		public override sealed void Update(NPC npc, ref int buffIndex) {
			base.Update(npc, ref buffIndex);
			UpdateNPC(npc, ref buffIndex);
		}
		public virtual void UpdateNPC(NPC npc, ref int buffIndex) {

		}
	}
	class ItemHandleSystem : ModSystem {
		public override void Load() {
			On_Player.QuickSpawnItemDirect_IEntitySource_Item_int += On_Player_QuickSpawnItemDirect_IEntitySource_Item_int;
			On_Player.QuickSpawnItemDirect_IEntitySource_int_int += On_Player_QuickSpawnItemDirect_IEntitySource_int_int;
			On_Player.QuickSpawnItem_IEntitySource_int_int += On_Player_QuickSpawnItem_IEntitySource_int_int;
			On_Player.QuickSpawnItem_IEntitySource_Item_int += On_Player_QuickSpawnItem_IEntitySource_Item_int;
		}
		private int On_Player_QuickSpawnItem_IEntitySource_int_int(On_Player.orig_QuickSpawnItem_IEntitySource_int_int orig, Player self, IEntitySource source, int item, int stack) {
			int whoamI = orig(self, source, item, stack);
			if (whoamI < 0 && whoamI >= Main.item.Length) {
				return whoamI;
			}
			Item worlditem = Main.item[whoamI];
			EnchantmentSystem.EnchantmentRNG(self, worlditem);
			AugmentsWeapon.AddAugments(self, ref worlditem);
			return whoamI;
		}
		private int On_Player_QuickSpawnItem_IEntitySource_Item_int(On_Player.orig_QuickSpawnItem_IEntitySource_Item_int orig, Player self, IEntitySource source, Item item, int stack) {
			int whoamI = orig(self, source, item, stack);
			if (whoamI < 0 && whoamI >= Main.item.Length) {
				return whoamI;
			}
			Item worlditem = Main.item[whoamI];
			EnchantmentSystem.EnchantmentRNG(self, worlditem);
			AugmentsWeapon.AddAugments(self, ref worlditem);
			return whoamI;
		}
		private Item On_Player_QuickSpawnItemDirect_IEntitySource_int_int(On_Player.orig_QuickSpawnItemDirect_IEntitySource_int_int orig, Player self, IEntitySource source, int type, int stack) {
			Item worlditem = orig(self, source, type, stack);
			EnchantmentSystem.EnchantmentRNG(self, worlditem);
			AugmentsWeapon.AddAugments(self, ref worlditem);
			return worlditem;
		}
		private Item On_Player_QuickSpawnItemDirect_IEntitySource_Item_int(On_Player.orig_QuickSpawnItemDirect_IEntitySource_Item_int orig, Player self, IEntitySource source, Item item, int stack) {
			Item worlditem = orig(self, source, item, stack);
			EnchantmentSystem.EnchantmentRNG(self, worlditem);
			AugmentsWeapon.AddAugments(self, ref worlditem);
			return worlditem;
		}

	}
}
