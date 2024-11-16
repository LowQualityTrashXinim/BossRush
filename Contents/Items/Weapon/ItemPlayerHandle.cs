using Terraria;
using Terraria.ID;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Contents.NPCs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.RelicItem;
using Terraria.Localization;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using ReLogic.Graphics;
using BossRush.Common.General;
using BossRush.Contents.BuffAndDebuff.PlayerDebuff;

namespace BossRush.Contents.Items.Weapon {
	/// <summary>
	/// This is synergy bonus system, this system will automatically handle most of the bonus action for you<br/>
	/// No need to manual set, nor anything, you only need to check whenever or not if a bonus is active or not
	/// </summary>
	public class SynergyBonus_System : ModSystem {
		public static Dictionary<int, Dictionary<int, bool>> Dictionary_SynergyBonus = new();
		public static Dictionary<int, Dictionary<int, List<int>>> Dictionary_SynergyGroupBonus = new();
		public override void Load() {
			Dictionary_SynergyBonus = new();
			Dictionary_SynergyGroupBonus = new();
		}
		public override void Unload() {
			Dictionary_SynergyBonus = null;
			Dictionary_SynergyGroupBonus = null;
		}
		public static void Add_SynergyBonus(int SynergyItemID, int ItemID) {
			Dictionary_SynergyBonus.Add(SynergyItemID, new() { { ItemID, false } });
		}
		public static void Add_SynergyGroupBonus(int SynergyItemID, int KeyItemID, List<int> GroupItemID) {
			Dictionary_SynergyGroupBonus.Add(SynergyItemID, new() { { KeyItemID, GroupItemID } });
		}
		/// <summary>
		/// Set your key item here, not synergy item
		/// </summary>
		/// <param name="ItemID"></param>
		/// <returns></returns>
		public static List<int> SafeGet_SynergyGroupBonus(int SynergyItemID, int KeyItemID) {
			if (Dictionary_SynergyGroupBonus.ContainsKey(SynergyItemID)) {
				if (Dictionary_SynergyGroupBonus[SynergyItemID].ContainsKey(KeyItemID))
					return Dictionary_SynergyGroupBonus[SynergyItemID][KeyItemID];
			}
			return new();
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
			if (!Dictionary_SynergyBonus[SynergyItemID].ContainsKey(ItemID)) {
				return false;
			}
			return Dictionary_SynergyBonus[SynergyItemID][ItemID];
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

		public bool SinisterBook_DemonScythe = false;
		public int SinisterBook_DemonScythe_Counter = 0;

		public bool QuadDemonBlaster = false;
		public float QuadDemonBlaster_SpeedMultiplier = 1;

		public bool MagicHandCannon_Flamelash = false;

		public bool DeathBySpark_AleThrowingGlove = false;

		public bool NatureSelection_NatureCrystal = false;

		public bool HorusEye_ResonanceScepter = false;
		public override void ResetEffects() {
			SynergyBonus = 0;
			SynergyBonusBlock = false;
			int Synergylength = SynergyBonus_System.Dictionary_SynergyBonus.Keys.Count;
			for (int i = 0; i < Synergylength; i++) {
				int synergyItem = SynergyBonus_System.Dictionary_SynergyBonus.Keys.ElementAt(i);
				if (Player.HeldItem.type != synergyItem) {
					continue;
				}
				int SynergyBonusLength = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem].Keys.Count;
				for (int l = 0; l < SynergyBonusLength; l++) {
					int itemIDBonus = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem].Keys.ElementAt(l);
					bool HasItem = Player.HasItem(itemIDBonus);
					if (HasItem) {
						SynergyBonus++;
					}
					SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][itemIDBonus] = HasItem;
					continue;
				}
				if (ContentSamples.ItemsByType[synergyItem].ModItem is SynergyModItem ModItem) {
					if (!ModItem.Contain_GroupSynergy) {
						continue;
					}
					List<int> keyItem = ModItem.Get_Key;
					foreach (var item in keyItem) {
						bool HasAnyGroupItem = SynergyBonus_System.SafeGet_SynergyGroupBonus(synergyItem, item).Where(Player.HasItem).Any();
						if (HasAnyGroupItem) {
							SynergyBonus++;
							SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][item] = HasAnyGroupItem;
						}
					}
				}
			}

			SinisterBook_DemonScythe = false;

			MagicHandCannon_Flamelash = false;

			DeathBySpark_AleThrowingGlove = false;

			NatureSelection_NatureCrystal = false;

			HorusEye_ResonanceScepter = false;
		}
	}
	public class GlobalItemHandle : GlobalItem {
		public const byte None = 0;
		public override bool InstancePerEntity => true;
		public bool LostAccessories = false;
		public bool DebugItem = false;
		public bool ExtraInfo = false;
		public bool AdvancedBuffItem = false;
		public bool RPGItem = false;
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
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
			if (item.ModItem is Relic relic && relic.relicColor != null) {
				NameLine.OverrideColor = relic.relicColor.MultiColor(5);
				tooltips.Add(new(Mod, "RelicItem", $"[Active Item]\nTier : {relic.TemplateCount}") { OverrideColor = Main.DiscoColor });
			}
			ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
			if (ExtraInfo && item.ModItem != null) {
				if (!moddedplayer.Hold_Shift) {
					tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Hold shift for more infomation]") { OverrideColor = Color.Gray });
				}
			}
			if (item.accessory && LostAccessories) {
				TooltipLine line_Tooltip0 = tooltips.Where(t => t.Name == "Tooltip0").FirstOrDefault();
				if (NameLine == null || line_Tooltip0 == null) {
					return;
				}
				NameLine.Text = Language.GetTextValue($"Mods.BossRush.LostAccessories.{item.ModItem.Name}.DisplayName");
				line_Tooltip0.Text = Language.GetTextValue($"Mods.BossRush.LostAccessories.{item.ModItem.Name}.Tooltip");
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = Color.DarkGoldenrod;
				tooltips.Add(new TooltipLine(Mod, "LostAcc_" + item.type, "Lost Accessory") { OverrideColor = Color.LightGoldenrodYellow });
			}
			if (AdvancedBuffItem && NameLine != null) {
				NameLine.Text += " [Advanced]";
			}
		}
		public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y) {
			if (item.ModItem == null) {
				return true;
			}
			if (item.ModItem.Mod != Mod) {
				return true;
			}
			ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
			if (ExtraInfo && item.ModItem != null)
				if (moddedplayer.Hold_Shift) {
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
			if (AdvancedBuffItem) {
				player.AddBuff(ModContent.BuffType<Drawback>(), BossRushUtils.ToMinute(6));
			}
			return base.UseItem(item, player);
		}
	}
	public abstract class SynergyModItem : ModItem {
		private bool _groupSynergy = false;
		private List<int> Key_GroupSynergyItem = new();
		public bool Contain_GroupSynergy => _groupSynergy;
		public List<int> Get_Key => Key_GroupSynergyItem;
		/// <summary>
		/// Set a list of your synergy key item to use along with <see cref="SynergyBonus_System.Add_SynergyGroupBonus(int, int, List{int})"/><br/>
		/// This is mandatory to make synergy bonus that contain a group of item<br/>
		/// </summary>
		/// <param name="KeyItem"></param>
		public void SetKey_SynergyBonusGroupItem(List<int> KeyItem) {
			_groupSynergy = true;
			Key_GroupSynergyItem = KeyItem;
		}
		public sealed override void SetStaticDefaults() {
			ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<SynergyEnergy>();
			CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(50, 100, 100) });
			Synergy_SetStaticDefaults();
		}
		public virtual void Synergy_SetStaticDefaults() { }
		public ColorInfo CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(100, 150, 150) });
		public override sealed void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			ModifySynergyToolTips(ref tooltips, Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>());
			if (CustomColor != null) {
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = CustomColor.MultiColor(5);
			}
		}
		public override void ModifyWeaponCrit(Player player, ref float crit) {
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			crit += 4 * modplayer.SynergyBonus;
		}
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage) {
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
		public virtual void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) { }
		public override sealed void HoldItem(Player player) {
			base.HoldItem(player);
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (modplayer.SynergyBonusBlock) {
				return;
			}
		}
		public override sealed void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
			ModifySynergyShootStats(player, player.GetModPlayer<PlayerSynergyItemHandle>(), ref position, ref velocity, ref type, ref damage, ref knockback);
		}
		public override sealed void UpdateInventory(Player player) {
			base.UpdateInventory(player);
			//Very funny that hold item happen after ModifyWeaponDamage
			//This probably will tank our mod performance, but well, it is what it is
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (player.HeldItem == Item && !modplayer.SynergyBonusBlock) {
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
			Main.instance.LoadItem(Item.type);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
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
}
