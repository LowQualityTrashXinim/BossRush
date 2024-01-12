using Terraria;
using Terraria.ID;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.NohitReward;

namespace BossRush.Contents.Perks {
	public class PerkItem : GlobalItem {
		public override bool? UseItem(Item item, Player player) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perk_PotionExpert && item.buffType > 0) {
				if (player.ItemAnimationJustStarted) {
					perkplayer.PotionExpert_perk_CanConsume = Main.rand.NextFloat() <= .35f;
				}
				return perkplayer.PotionExpert_perk_CanConsume;
			}
			return base.UseItem(item, player);
		}

		// how is drinking a potion with left click works differently from quick heal?... talking about a fresh spaghetti serving right there.
		public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perk_PotionCleanse) {
				healValue /= 2;
			}
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
			return base.ConsumeItem(item, player);
		}
	}
	class PerkModSystem : ModSystem {
		public override void Load() {
			base.Load();
			On_Player.QuickMana += On_Player_QuickMana;
		}
		private void On_Player_QuickMana(On_Player.orig_QuickMana orig, Player self) {
			PerkPlayer perkplayer = self.GetModPlayer<PerkPlayer>();
			if (self.HasBuff(ModContent.BuffType<ManaBlock>()) && perkplayer.HasPerk<ImprovedManaPotion>()) {
				return;
			}
			orig(self);
		}
	}
	class MagicOverhaulBuff : GlobalBuff {
		public override void Update(int type, Player player, ref int buffIndex) {
			if (type == BuffID.ManaSickness && player.GetModPlayer<PerkPlayer>().HasPerk<ImprovedManaPotion>()) {
				if (player.statMana < player.statManaMax2) {
					player.statMana++;
				}
				if (player.buffTime[buffIndex] <= 0) {
					player.AddBuff(ModContent.BuffType<ManaBlock>(), BossRushUtils.ToSecond(30));
				}
			}
		}
	}
	class ManaBlock : ModBuff {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			base.Update(player, ref buffIndex);
		}
	}
	public class PerkGlobalNpc : GlobalNPC {


		public override void ModifyActiveShop(NPC npc, string shopName, Item[] items) {

			PerkPlayer perkPlayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();


			// when talking to npc with the shopPerk
			if (perkPlayer.perks.ContainsKey(Perk.GetPerkType<ShopPerk>())) {


				items[^1] = getExtraItemValue(npc.type);

				//check if the npc already talked to with shopPerk, if not, generate an item for that specific npc
				foreach (int npcID in perkPlayer.hasExtraWeapon.Keys) {
					if (npcID == npc.type)
						return;
				}

				LootBoxBase.GetWeapon(out int weapon, out int amount);

				// store the npc and their item in a dict, which the dict gets saved and loaded inside perkPlayer
				perkPlayer.hasExtraWeapon.Add(npc.type, weapon);
				items[^1] = getExtraItemValue(npc.type);
			}
		}
		private Item getExtraItemValue(int key) {
			return new Item(Main.LocalPlayer.GetModPlayer<PerkPlayer>().hasExtraWeapon.GetValueOrDefault(key)) { shopCustomPrice = Item.buyPrice(gold: 25) };
		}

	}

	public class PerkPlayer : ModPlayer {
		public bool CanGetPerk = false;
		public int PerkAmount = 4;
		/// <summary>
		/// Keys : Perk type<br/>
		/// Values : Stack value
		/// </summary>
		public Dictionary<int, int> perks = new Dictionary<int, int>();

		public bool perk_PotionExpert = false;
		public bool perk_PotionCleanse = false;
		public bool PotionExpert_perk_CanConsume = false;

		// ShopPerk
		public Dictionary<int, int> hasExtraWeapon = new Dictionary<int, int>();
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
		public override bool CanUseItem(Item item) {
			if (item.buffType == BuffID.ManaSickness && Player.HasBuff(ModContent.BuffType<ManaBlock>())) {
				return false;
			}
			return base.CanUseItem(item);
		}
		public override void ResetEffects() {
			perk_PotionExpert = false;
			perk_PotionCleanse = false;
			PerkAmount = 4;
			PerkAmount = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count + PerkAmountModified();
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				ModPerkLoader.GetPerk(i).StackAmount = 0;
			}
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).StackAmount = perks[perk];
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
		public override void SaveData(TagCompound tag) {
			tag["PlayerPerks"] = perks.Keys.ToList();
			tag["PlayerPerkStack"] = perks.Values.ToList();
			//save the dict hasExtraWeapon
			tag["npcShop"] = hasExtraWeapon.Keys.ToList();
			tag["npcWeapon"] = hasExtraWeapon.Values.ToList();
		}
		public override void LoadData(TagCompound tag) {
			var PlayerPerks = tag.Get<List<int>>("PlayerPerks");
			var PlayerPerkStack = tag.Get<List<int>>("PlayerPerkStack");
			perks = PlayerPerks.Zip(PlayerPerkStack, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

			var npcShop = tag.Get<List<int>>("npcShop");
			var npcWeapon = tag.Get<List<int>>("npcWeapon");
			hasExtraWeapon = npcShop.Zip(npcWeapon, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
		}
	}
	public abstract class Perk : ModType {
		public string DisplayName => Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.DisplayName");
		public string Description => Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.Description");
		public bool CanBeStack = false;
		/// <summary>
		/// Use this if <see cref="CanBeStack"/> is true
		/// <br/> This allow easy multiply
		/// </summary>
		public int StackAmount = 0;
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
		public virtual void SetDefaults() { }
		public virtual void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
		public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
		public virtual void Update(Player player) { }
		public virtual void UpdateEquip(Player player) { }
		public virtual void ResetEffect(Player player) { }
		public virtual void OnMissingMana(Player player, Item item, int neededMana) { }
		public virtual void ModifyDamage(Player player, Item item, ref StatModifier damage) { }
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
		/// <summary>
		/// Subtract will make player use weapon slower
		/// Additive will make player use weapon faster
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <param name="useSpeed">by default start at 1</param>
		public virtual void ModifyUseSpeed(Player player, Item item, ref float useSpeed) { }
		public virtual void OnChoose(Player player) { }
	}
	public static class ModPerkLoader {
		private static readonly List<Perk> _perks = new();
		public static int TotalCount => _perks.Count;
		public static int Register(Perk perk) {
			ModTypeLookup<Perk>.Register(perk);
			_perks.Add(perk);
			return _perks.Count - 1;
		}
		public static Perk GetPerk(int type) {
			return type >= 0 && type < _perks.Count ? _perks[type] : null;
		}
	}
	class PerkChooser : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(32, 23);
			Item.maxStack = 999;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool? UseItem(Player player) {
			PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
			if (player.altFunctionUse != 2) {
				UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
				uiSystemInstance.perkUIstate.whoAmI = player.whoAmI;
				uiSystemInstance.perkUIstate.StateofState = PerkUIState.DefaultState;
				uiSystemInstance.userInterface.SetState(uiSystemInstance.perkUIstate);
			}
			else if (player.IsDebugPlayer()) {
				modplayer.perks.Clear();
			}
			return true;
		}
	}
	class StarterPerkChooser : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(32, 23);
			Item.maxStack = 999;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool? UseItem(Player player) {
			PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
			if (player.altFunctionUse != 2) {
				UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
				uiSystemInstance.perkUIstate.whoAmI = player.whoAmI;
				uiSystemInstance.perkUIstate.StateofState = PerkUIState.StarterPerkState;
				uiSystemInstance.userInterface.SetState(uiSystemInstance.perkUIstate);
			}
			else if (player.IsDebugPlayer()) {
				modplayer.perks.Clear();
			}
			return true;
		}
	}
}
