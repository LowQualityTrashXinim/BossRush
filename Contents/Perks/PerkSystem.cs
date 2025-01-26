using System;
using Terraria;
using System.IO;
using Terraria.ID;
using System.Linq;
using Terraria.UI;
using Terraria.Audio;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Transfixion.Artifacts;
using BossRush.Contents.Items.Consumable.SpecialReward;

namespace BossRush.Contents.Perks {
	public class PerkItem : GlobalItem {
		public override bool? UseItem(Item item, Player player) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perk_PotionExpert && item.buffType > 0) {
				if (player.ItemAnimationJustStarted) {
					perkplayer.PotionExpert_perk_CanConsume = Main.rand.NextFloat() <= .55f;
				}
				return perkplayer.PotionExpert_perk_CanConsume;
			}
			return base.UseItem(item, player);
		}

		// how is drinking a potion with left click works differently from quick heal?... talking about a fresh spaghetti serving right there.
		public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			StatModifier healingPotionstat = StatModifier.Default;
			if (perkplayer.perk_PotionCleanse) {
				healingPotionstat -= .5f;
			}
			if (perkplayer.perk_ImprovedPotion) {
				healingPotionstat += .7f;
			}
			healValue = (int)healingPotionstat.ApplyTo(healValue);
		}

		public override bool ConsumeItem(Item item, Player player) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perk_PotionCleanse && item.healLife > 0) {
				foreach (int i in player.buffType) {
					if (Main.debuff[i]) {
						player.ClearBuff(i);

					}
				}
			}
			if (perkplayer.perk_AlchemistPotion && item.buffType > 0 && !player.HasBuff(ModContent.BuffType<MysteriousPotionBuff>())) {
				MysteriousPotionBuff.SetBuff(7, BossRushUtils.ToMinute(4), player);
			}
			return base.ConsumeItem(item, player);
		}
	}
	class PerkModSystem : ModSystem {
		public static List<int> StarterPerkType { get; private set; } = new();
		public static List<int> WeaponUpgradeType { get; private set; } = new();
		public override void Load() {
			base.Load();
			On_Player.QuickMana += On_Player_QuickMana;
			StarterPerkType = new();
		}
		public override void Unload() {
			StarterPerkType = null;
		}
		private void On_Player_QuickMana(On_Player.orig_QuickMana orig, Player self) {
			PerkPlayer perkplayer = self.GetModPlayer<PerkPlayer>();
			if (self.HasBuff(ModContent.BuffType<ManaBlock>()) && perkplayer.perk_ImprovedPotion) {
				return;
			}
			orig(self);
		}
		public override void PostSetupContent() {
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				if (ModPerkLoader.GetPerk(i).list_category.Contains(PerkCategory.Starter)) {
					StarterPerkType.Add(i);
				}
				if (ModPerkLoader.GetPerk(i).list_category.Contains(PerkCategory.WeaponUpgrade)) {
					WeaponUpgradeType.Add(i);
				}
			}
		}
	}
	class MagicOverhaulBuff : GlobalBuff {
		public override void Update(int type, Player player, ref int buffIndex) {
			if (type == BuffID.ManaSickness && player.GetModPlayer<PerkPlayer>().perk_ImprovedPotion) {
				if (player.statMana < player.statManaMax2) {
					player.statMana += 2;
				}
				if (player.buffTime[buffIndex] <= 0) {
					player.AddBuff(ModContent.BuffType<ManaBlock>(), BossRushUtils.ToSecond(10));
				}
			}
		}
	}
	class ManaBlock : ModBuff {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			base.Update(player, ref buffIndex);
		}
	}

	public class PerkPlayer : ModPlayer {
		public bool CanGetPerk = false;
		public int PerkAmount = 4;
		private byte perk_Reroll = 1;
		public void Modify_RerollCount(byte amount, bool negative = false) {
			int simulate = perk_Reroll + amount;
			if (simulate < byte.MinValue) {
				perk_Reroll = byte.MinValue;
			}
			else if (simulate > byte.MaxValue) {
				perk_Reroll = byte.MaxValue;
			}
			if (negative) {
				perk_Reroll -= amount;
			}
			else {
				perk_Reroll += amount;
			}
		}
		public byte Reroll => perk_Reroll;
		/// <summary>
		/// Keys : Perk type<br/>
		/// Values : Stack value
		/// </summary>
		public Dictionary<int, int> perks = new Dictionary<int, int>();

		public bool perk_PotionExpert = false;
		public bool perk_PotionCleanse = false;
		public bool perk_AlchemistPotion = false;
		public bool perk_ImprovedPotion = false;
		public bool PotionExpert_perk_CanConsume = false;
		public bool perk_ScatterShot = false;
		public override void Initialize() {
			perks = new Dictionary<int, int>();
			PerkAmount = 4;
		}
		public int PerkAmountModified() {
			if (perks.ContainsKey(Perk.GetPerkType<BlessingOfPerk>())) {
				return PerkAmount + perks[Perk.GetPerkType<BlessingOfPerk>()];
			}
			return PerkAmount;
		}
		public bool HasPerk<T>() where T : Perk => perks.ContainsKey(Perk.GetPerkType<T>());
		public override void PostItemCheck() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnUseItem(Player, Player.HeldItem);
			}
		}
		public override bool CanUseItem(Item item) {
			if (item.buffType == BuffID.ManaSickness && Player.HasBuff(ModContent.BuffType<ManaBlock>())) {
				return false;
			}
			return base.CanUseItem(item);
		}
		public override void ResetEffects() {
			perk_PotionExpert = false;
			perk_PotionCleanse = false;
			perk_AlchemistPotion = false;
			perk_ImprovedPotion = false;
			perk_ScatterShot = false;
			PerkAmount = 4;
			PerkAmount = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count + PerkAmountModified();
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ResetEffect(Player);
			}
		}
		public override void PostUpdateEquips() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).UpdateEquip(Player);
			}
		}
		public override void PostUpdate() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).Update(Player);
			}
		}
		public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyShootStat(Player, item, ref position, ref velocity, ref type, ref damage, ref knockback);
			}
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).Shoot(Player, item, source, position, velocity, type, damage, knockback);
			}
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		public override void OnMissingMana(Item item, int neededMana) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnMissingMana(Player, item, neededMana);
			}
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyMaxStats(Player, ref health, ref mana);
			}
		}
		public override void ModifyWeaponCrit(Item item, ref float crit) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyCriticalStrikeChance(Player, item, ref crit);
			}
		}
		public override void ModifyItemScale(Item item, ref float scale) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyItemScale(Player, item, ref scale);
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyDamage(Player, item, ref damage);
			}
		}
		public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyKnockBack(Player, item, ref knockback);
			}
		}
		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitNPCWithItem(Player, item, target, ref modifiers);
			}
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
			}
		}
		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitByNPC(Player, npc, ref modifiers);
			}
		}
		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitByProjectile(Player, proj, ref modifiers);
			}
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitNPCWithItem(Player, item, target, hit, damageDone);
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitNPCWithProj(Player, proj, target, hit, damageDone);
			}
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitByAnything(Player);
				ModPerkLoader.GetPerk(perk).OnHitByNPC(Player, npc, hurtInfo);
			}
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitByAnything(Player);
				ModPerkLoader.GetPerk(perk).OnHitByProjectile(Player, proj, hurtInfo);
			}
		}
		public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyManaCost(Player, item, ref reduce, ref mult);
			}
		}
		public override float UseSpeedMultiplier(Item item) {
			float useSpeed = 1;
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyUseSpeed(Player, item, ref useSpeed);
			}
			return useSpeed;
		}

		public override bool FreeDodge(Player.HurtInfo info) {
			foreach (int perk in perks.Keys) {
				if (ModPerkLoader.GetPerk(perk).FreeDodge(Player, info)) {
					return true;
				}
			}
			return base.FreeDodge(info);
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
			foreach (int perk in perks.Keys) {
				if (ModPerkLoader.GetPerk(perk).PreKill(Player)) {
					return false;
				}
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
		}
		public override void SaveData(TagCompound tag) {
			tag["PlayerPerks"] = perks.Keys.ToList();
			tag["PlayerPerkStack"] = perks.Values.ToList();
			tag["perk_Reroll"] = perk_Reroll;
		}
		public override void LoadData(TagCompound tag) {
			var PlayerPerks = tag.Get<List<int>>("PlayerPerks");
			var PlayerPerkStack = tag.Get<List<int>>("PlayerPerkStack");
			perks = PlayerPerks.Zip(PlayerPerkStack, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

			int count = perks.Count;
			for (int i = count - 1; i >= 0; i--) {
				if (ModPerkLoader.GetPerk(perks.Keys.ElementAt(i)) == null) {
					perks.Remove(perks.Keys.ElementAt(i));
				}
			}
			if (tag.TryGet("perk_Reroll", out byte va)) {
				perk_Reroll = va;
			}
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.Perk);
			packet.Write((byte)Player.whoAmI);
			packet.Write(perks.Keys.Count);
			foreach (int item in perks.Keys) {
				packet.Write(item);
				packet.Write(perks[item]);
			}
			packet.Write(perk_Reroll);
			packet.Send(toWho, fromWho);
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			perks.Clear();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
				perks.Add(reader.ReadInt32(), reader.ReadInt32());
			perk_Reroll = reader.ReadByte();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			PerkPlayer clone = (PerkPlayer)targetCopy;
			clone.perks = perks;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			PerkPlayer clone = (PerkPlayer)clientPlayer;
			if (perks != clone.perks) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
	public enum PerkCategory : byte {
		None,
		Starter,
		WeaponUpgrade,
		ArtifactExclusive
	}
	public abstract class Perk : ModType {
		public string DisplayName => Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.DisplayName");
		public string Description => Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.Description");
		public string DescriptionIndex(byte index) => Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.Description{index}");
		public bool CanBeStack = false;
		/// <summary>
		/// This will get the value from Mod Perk player itself<br/>
		/// it is recommend to use this rather than get it yourself cause what it doing is pretty much the same<br/>
		/// This is prefer over the previous method as this do not update and isntead just get it from the source<br/>
		/// In whatever case it is, it is highly recommend to not cached it cause the performance increases is very minimal<br/>
		/// </summary>
		public int StackAmount(Player player) {
			if (player.TryGetModPlayer(out PerkPlayer perkplayer)) {
				if (perkplayer.perks.ContainsKey(Type))
					return perkplayer.perks[Type];
			}
			return 0;
		}
		/// <summary>
		/// This is where you set limit to amount of stack should a perk have<br/>
		/// <see cref="StackAmount"/> will always start at 0 and increase by 1 ( regardless if <see cref="CanBeStack"/> true or false )<br/>
		/// The next time this perk get choosen, it will increase by 1<br/>
		/// The perk will no longer show up if the stack amount reach the limit, for more info see <see cref="PerkUIState.ActivateNormalPerkUI(PerkPlayer, Player)"/><br/>
		/// If you are modifying tooltip base on <see cref="StackAmount"/> then you should substract stack amount by 1
		/// </summary>
		public int StackLimit = 1;
		/// <summary>
		/// Please set this texture string as if you are setting <see cref="ModItem.Texture"/>
		/// </summary>
		public string textureString = null;
		public string Tooltip = null;
		/// <summary>
		/// This will prevent from perk being able to be choose
		/// </summary>
		public bool CanBeChoosen = true;
		public int Type { get; private set; }
		public List<PerkCategory> list_category = new() { };
		protected sealed override void Register() {
			Type = ModPerkLoader.Register(this);
		}
		public static int GetPerkType<T>() where T : Perk {
			return ModContent.GetInstance<T>().Type;
		}
		public string PerkNameToolTip => ModifyName() + "\n" + ModifyToolTip();
		public virtual string ModifyToolTip() {
			if (Description != null)
				return Description;
			return Tooltip;
		}
		public virtual string ModifyName() {
			return PerkName();
		}
		public string PerkName() {
			if (DisplayName != null)
				return DisplayName;
			string Name = ModPerkLoader.GetPerk(Type).Name;
			for (int i = Name.Length - 1; i > 0; i--) {
				if (char.IsUpper(Name[i])) {
					Name = Name.Substring(0, i) + " " + Name.Substring(i);
				}
			}
			return Name;
		}
		public sealed override void Unload() {
			base.Unload();
			textureString = null;
			Tooltip = null;
		}
		public Perk() {
			SetDefaults();
			if (CanBeStack)
				Tooltip += "\n( Can be stack ! )";
		}
		/// <summary>
		/// This act different to <see cref="CanBeChoosen"/><br/>
		/// if this is set to false then it won't be add into perk pool, otherwise if <see cref="CanBeChoosen"/> is false and this is true then it still won't be add<br/>
		/// This only run on client side so it is recommend to uses <see cref="Main.LocalPlayer"/> to check player condition
		/// </summary>
		/// <returns></returns>
		public virtual bool SelectChoosing() => true;
		public virtual void SetDefaults() { }
		public virtual void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
		public virtual void OnUseItem(Player player, Item item) { }
		public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
		public virtual void Update(Player player) { }
		public virtual void UpdateEquip(Player player) { }
		public virtual void ResetEffect(Player player) { }
		public virtual void OnMissingMana(Player player, Item item, int neededMana) { }
		public virtual void ModifyDamage(Player player, Item item, ref StatModifier damage) { }
		public virtual void ModifyKnockBack(Player player, Item item, ref StatModifier knockback) { }
		public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitByAnything(Player player) { }
		public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
		public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
		public virtual void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
		public virtual void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
		public virtual void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) { }
		public virtual void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) { }
		public virtual void ModifyItemScale(Player player, Item item, ref float scale) { }
		public virtual void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) { }
		public virtual void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) { }
		public virtual void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) { }
		/// <summary>
		/// Subtract will make player use weapon slower
		/// Additive will make player use weapon faster
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <param name="useSpeed">by default start at 1</param>
		public virtual void ModifyUseSpeed(Player player, Item item, ref float useSpeed) { }
		public virtual void OnChoose(Player player) { }
		public virtual bool FreeDodge(Player player, Player.HurtInfo hurtInfo) => false;
		public virtual bool PreKill(Player player) => false;
	}
	public static class ModPerkLoader {
		private static readonly List<Perk> _perks = new();
		public static int TotalCount => _perks.Count;
		public static int Register(Perk perk) {
			ModTypeLookup<Perk>.Register(perk);
			_perks.Add(perk);
			if (perk.list_category.Count < 1) {
				perk.list_category.Add(PerkCategory.None);
			}
			return _perks.Count - 1;
		}
		public static Perk GetPerk(int type) {
			return type >= 0 && type < _perks.Count ? _perks[type] : null;
		}
	}
	internal class PerkUIState : UIState {
		public string Info = "";
		public const short DefaultState = 0;
		public const short StarterPerkState = 1;
		public const short DebugState = 2;
		public const short GamblerState = 3;
		public const short WeaponUpgradeState = 4;
		public short StateofState = 0;
		public UIText toolTip;
		public Roguelike_UIImageButton reroll = null;
		List<PerkUIImageButton> list_perkbtn = new();
		public override void OnInitialize() {
			reroll = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
			reroll.OnLeftClick += Reroll_OnLeftClick;
			reroll.OnUpdate += Reroll_OnUpdate;
			reroll.UISetWidthHeight(52, 52);
			reroll.HAlign = .5f;
			reroll.VAlign = .5f;
			Append(reroll);

			list_perkbtn = new();
			toolTip = new UIText("");
			Append(toolTip);
		}

		private void Reroll_OnUpdate(UIElement affectedElement) {
			if (Main.LocalPlayer.GetModPlayer<PerkPlayer>().Reroll == 0) {
				reroll.Hide = true;
			}
			if (affectedElement.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			if (affectedElement.IsMouseHovering) {
				Main.instance.MouseText("Reroll Perk !");
			}
		}

		private void Reroll_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 1 });
			List<int> listOfPerk = new List<int>();
			Player player = Main.LocalPlayer;
			player.TryGetModPlayer(out PerkPlayer modplayer);
			if (StateofState == DefaultState) {
				for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
					if (modplayer.perks.ContainsKey(i)) {
						if ((!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0)
							|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
							continue;
						}
					}
					if (!ModPerkLoader.GetPerk(i).SelectChoosing()) {
						continue;
					}
					if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
						continue;
					}
					listOfPerk.Add(i);
				}
			}
			if (StateofState == StarterPerkState) {
				listOfPerk = PerkModSystem.StarterPerkType;
			}
			foreach (var item in list_perkbtn) {
				if (listOfPerk.Count < 1) {
					item.ChangePerkType(Main.rand.Next(new int[] { Perk.GetPerkType<SuppliesDrop>(), Perk.GetPerkType<GiftOfRelic>() }));
				}
				else {
					item.ChangePerkType(Main.rand.Next(listOfPerk));
				}
			}
			modplayer.Modify_RerollCount(1, true);
		}

		public override void OnActivate() {
			list_perkbtn.Clear();
			for (int i = Elements.Count - 1; i >= 0; i--) {
				if (Elements[i].UniqueId != reroll.UniqueId) {
					Elements[i].Remove();
				}
			}
			Player player = Main.LocalPlayer;
			if (player.TryGetModPlayer(out PerkPlayer modplayer)) {
				if (StateofState == DefaultState) {
					ActivateNormalPerkUI(modplayer, player);
				}
				if (StateofState == StarterPerkState) {
					ActivateStarterPerkUI(modplayer, player);
				}
				if (StateofState == DebugState) {
					ActivateDebugPerkUI(player);
				}
				if (StateofState == GamblerState) {
					ActivateGamblerUI(modplayer, player);
				}
				if (StateofState == WeaponUpgradeState) {
					ActivateWeaponUpgradeUI(modplayer, player);
				}
			}
		}
		private void ActivateWeaponUpgradeUI(PerkPlayer modplayer, Player player) {
			reroll.Hide = false;
			Vector2 originDefault = new Vector2(26, 26);
			List<int> starterPerk = new(PerkModSystem.WeaponUpgradeType);
			int limit = 3;
			for (int i = 0; i < limit; i++) {
				Perk choosenperk = ModPerkLoader.GetPerk(Main.rand.Next(starterPerk));
				starterPerk.Remove(choosenperk.Type);
				Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(limit, 360, i) * 120;
				//After that we assign perk
				if (modplayer.perks.ContainsKey(choosenperk.Type)) {
					if (modplayer.perks[choosenperk.Type] >= choosenperk.StackLimit) {
						continue;
					}
				}
				PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(choosenperk.textureString));
				btn.UISetWidthHeight(52, 52);
				btn.UISetPosition(player.Center + offsetPos, originDefault);
				btn.perkType = choosenperk.Type;
				list_perkbtn.Add(btn);
				Append(btn);
			}
		}
		private void ActivateDebugPerkUI(Player player) {
			int amount = ModPerkLoader.TotalCount;
			Vector2 originDefault = new Vector2(26, 26);
			for (int i = 0; i < amount; i++) {
				Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenlyPlus(amount + 1, 360, i) * Math.Clamp(amount * 20, 0, 460);
				Asset<Texture2D> texture;
				if (ModPerkLoader.GetPerk(i).textureString is not null)
					texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(i).textureString);
				else
					texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
				//After that we assign perk
				PerkUIImageButton btn = new PerkUIImageButton(texture);
				btn.UISetWidthHeight(52, 52);
				btn.UISetPosition(player.Center + offsetPos, originDefault);
				btn.perkType = i;
				Append(btn);
				ModPerkLoader.GetPerk(i);
			}
			reroll.Hide = true;
		}
		private void ActivateNormalPerkUI(PerkPlayer modplayer, Player player) {
			reroll.Hide = false;
			List<int> listOfPerk = new List<int>();
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				if (modplayer.perks.ContainsKey(i)) {
					if ((!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0)
						|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
						continue;
					}
				}
				if (!ModPerkLoader.GetPerk(i).SelectChoosing()) {
					continue;
				}
				if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
					continue;
				}
				listOfPerk.Add(i);
			}
			Vector2 originDefault = new Vector2(26, 26);
			int amount = listOfPerk.Count;
			int perkamount = modplayer.PerkAmountModified();
			for (int i = 0; i < perkamount; i++) {
				Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(perkamount, 360, i) * Math.Clamp(perkamount * 20, 0, 200);
				int newperk = Main.rand.Next(listOfPerk);
				Asset<Texture2D> texture;
				if (ModPerkLoader.GetPerk(newperk).textureString is not null)
					texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
				else
					texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
				if (i >= amount) {
					newperk = Main.rand.Next(new int[] { Perk.GetPerkType<SuppliesDrop>(), Perk.GetPerkType<GiftOfRelic>() });
					if (ModPerkLoader.GetPerk(newperk).textureString is not null)
						texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
					else
						texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
					PerkUIImageButton buttonWeapon = new PerkUIImageButton(texture);
					buttonWeapon.perkType = newperk;
					buttonWeapon.UISetWidthHeight(52, 52);
					buttonWeapon.UISetPosition(player.Center + offsetPos, originDefault);
					buttonWeapon.Info = Info;
					list_perkbtn.Add(buttonWeapon);
					Append(buttonWeapon);
					continue;
				}
				listOfPerk.Remove(newperk);
				//After that we assign perk
				PerkUIImageButton btn = new PerkUIImageButton(texture);
				btn.UISetWidthHeight(52, 52);
				btn.UISetPosition(player.Center + offsetPos, originDefault);
				btn.perkType = newperk;
				btn.Info = Info;
				list_perkbtn.Add(btn);
				Append(btn);
			}
		}
		private void ActivateStarterPerkUI(PerkPlayer modplayer, Player player) {
			reroll.Hide = false;
			Vector2 originDefault = new Vector2(26, 26);
			List<int> starterPerk = new(PerkModSystem.StarterPerkType);
			int limit = 3;
			for (int i = 0; i < limit; i++) {
				Perk choosenperk = ModPerkLoader.GetPerk(Main.rand.Next(starterPerk));
				starterPerk.Remove(choosenperk.Type);
				Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(limit, 360, i) * 120;
				//After that we assign perk
				if (modplayer.perks.ContainsKey(choosenperk.Type)) {
					if (modplayer.perks[choosenperk.Type] >= choosenperk.StackLimit) {
						continue;
					}
				}
				PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(choosenperk.textureString));
				btn.UISetWidthHeight(52, 52);
				btn.UISetPosition(player.Center + offsetPos, originDefault);
				btn.perkType = choosenperk.Type;
				list_perkbtn.Add(btn);
				Append(btn);
			}
		}
		private void ActivateGamblerUI(PerkPlayer modplayer, Player player) {
			Vector2 originDefault = new Vector2(26, 26);
			int[] starterPerk = new int[]
			{ Perk.GetPerkType<UncertainStrike>(),
			Perk.GetPerkType<StrokeOfLuck>(),
			Perk.GetPerkType<BlessingOfPerk>()
			};
			for (int i = 0; i < starterPerk.Length; i++) {
				Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(starterPerk.Length, 360, i) * starterPerk.Length * 20;
				//After that we assign perk
				if (modplayer.perks.ContainsKey(starterPerk[i])) {
					if (modplayer.perks[starterPerk[i]] >= ModPerkLoader.GetPerk(starterPerk[i]).StackLimit) {
						continue;
					}
				}
				PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(starterPerk[i]).textureString));
				btn.UISetWidthHeight(52, 52);
				btn.UISetPosition(player.Center + offsetPos, originDefault);
				btn.perkType = starterPerk[i];
				Append(btn);
			}
			reroll.Hide = true;
		}
	}
	//Do all the check in UI state since that is where the perk actually get create and choose
	class PerkUIImageButton : UIImageButton {
		public int perkType;
		public string Info = "";
		private Asset<Texture2D> texture;
		private Asset<Texture2D> ahhlookingassdefaultbgsperktexture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
		public PerkUIImageButton(Asset<Texture2D> texture) : base(texture) {
			this.texture = texture;
		}
		public void ChangePerkType(int type) {
			perkType = type;
			Perk perk = ModPerkLoader.GetPerk(perkType);
			if (perk != null && perk.textureString != null) {
				texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(perkType).textureString);
			}
			else {
				texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
			}
			SetImage(texture);
			this.UISetWidthHeight(52, 52);
		}
		public override void LeftClick(UIMouseEvent evt) {
			SoundEngine.PlaySound(SoundID.Item35 with { Pitch = -1 });
			UniversalSystem.AddPerk(perkType);
			if (Info == "Glitch") {
				Perk perk = ModPerkLoader.GetPerk(perkType);
				int stack = 0;
				if (perk.CanBeStack) {
					stack = Main.LocalPlayer.GetModPlayer<PerkPlayer>().perks[perkType];
				}
				int length = Math.Clamp(perk.StackLimit - stack, 0, 999999);
				for (int i = 0; i < length; i++) {
					UniversalSystem.AddPerk(perkType);
				}
			}
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			if (IsMouseHovering && ModPerkLoader.GetPerk(perkType) != null) {
				Main.instance.MouseText(ModPerkLoader.GetPerk(perkType).DisplayName + "\n" + ModPerkLoader.GetPerk(perkType).ModifyToolTip());
			}
			else {
				if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
					Main.instance.MouseText("");
				}
			}
			if (IsMouseHovering && Switch == 0) {
				Switch = BossRushUtils.Safe_SwitchValue(Switch, 100);
			}
			if (Switch != 0) {
				Switch = BossRushUtils.Safe_SwitchValue(Switch, 100, extraspeed: 1);
			}
		}
		int Switch = 0;
		public override void Draw(SpriteBatch spriteBatch) {
			if (Info == "Glitch") {
				spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, this.GetInnerDimensions().Position() + new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4)), null, Color.Red * .5f);
				spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, this.GetInnerDimensions().Position() + new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4)), null, Color.Blue * .5f);
			}
			spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, this.GetInnerDimensions().Position(), null, Color.White * .45f);
			if (Switch != 0) {
				float alpha = (100 - Switch) * 0.01f;
				float size = 1 + Switch * .01f * .75f;
				Vector2 origin = ahhlookingassdefaultbgsperktexture.Value.Size() * .5f;
				Vector2 adjustment = origin - origin * size;
				spriteBatch.Draw(ahhlookingassdefaultbgsperktexture.Value, this.GetInnerDimensions().Position() + adjustment, null, Color.White * alpha, 0, Vector2.Zero, size, SpriteEffects.None, 0f);
			}
			base.Draw(spriteBatch);
		}
	}
}
