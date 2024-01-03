using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.WeaponEnchantment {
	//Todo : turn out modplayer is much better than global item, how funny
	public abstract class ModEnchantment : ModType {
		public int Type { get; private set; }
		public int ItemIDType = ItemID.None;
		/// <summary>
		/// This will clean your counter automatically upon changing weapon
		/// </summary>
		public bool ForcedCleanCounter = false;
		public string DisplayName => Language.GetTextValue($"Mods.BossRush.ModEnchantment.{Name}.DisplayName");
		public string Description => Language.GetTextValue($"Mods.BossRush.ModEnchantment.{Name}.Description");
		protected sealed override void Register() {
			SetDefaults();
			Type = EnchantmentLoader.Register(this);
		}
		/// <summary>
		/// This will run before the counter from globalItem be wiped clean
		/// Only use it if <see cref="ForcedCleanCounter"/> is true
		/// </summary>
		/// <param name="index"></param>
		/// <param name="player"></param>
		/// <param name="globalItem"></param>
		/// <param name="item"></param>
		public virtual void PreCleanCounter(int index, Player player, EnchantmentGlobalItem globalItem, Item item) { }
		/// <summary>
		/// This is actually SetStaticDefault but uhh, I forgot
		/// All the varible in this will be set upon Load
		/// </summary>
		public virtual void SetDefaults() { }
		//I couldn't figure out a way to implement WhoAmI type of stuff and can't even use globalItem to transfer the index over here to use, so I settle with this
		public virtual void ModifyShootStat(int index,Player player, EnchantmentGlobalItem globalItem, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
		public virtual void Shoot(int index,Player player, EnchantmentGlobalItem globalItem, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
		public virtual void Update(Player player) { }
		public virtual void UpdateHeldItem(int index,Item item, EnchantmentGlobalItem globalItem, Player player) { }
		public virtual void OnMissingMana(int index,Player player, EnchantmentGlobalItem globalItem, Item item, int neededMana) { }
		public virtual void ModifyDamage(int index,Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) { }
		public virtual void OnHitNPCWithItem(int index,Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitNPCWithProj(int index,Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitByAnything(Player player) { }
		public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
		public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
		public virtual void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) { }
		public virtual void ModifyCriticalStrikeChance(int index,Player player, EnchantmentGlobalItem globalItem, Item item, ref float crit) { }
		public virtual void ModifyItemScale(int index,Player player, EnchantmentGlobalItem globalItem, Item item, ref float scale) { }
		public virtual void ModifyManaCost(int index,Player player, EnchantmentGlobalItem globalItem, Item item, ref float reduce, ref float multi) { }
		/// <summary>
		/// Subtract will make player use weapon slower
		/// Additive will make player use weapon faster
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <param name="useSpeed">by default start at 1</param>
		public virtual void ModifyUseSpeed(int index,Player player, EnchantmentGlobalItem globalItem, Item item, ref float useSpeed) { }
		public virtual void OnKill(Player player) { }
	}
	public static class EnchantmentLoader {
		private static readonly List<ModEnchantment> _enchantment = new();
		private static readonly List<int> _enchantmentcacheID = new();
		public static int TotalCount => _enchantment.Count;
		public static int Register(ModEnchantment enchant) {
			ModTypeLookup<ModEnchantment>.Register(enchant);
			_enchantment.Add(enchant);
			_enchantmentcacheID.Add(enchant.ItemIDType);
			return _enchantment.Count - 1;
		}
		public static ModEnchantment GetEnchantment(int type) {
			return type >= 0 && type < _enchantment.Count ? _enchantment[type] : null;
		}
		public static ModEnchantment GetEnchantmentItemID(int ItemID) {
			int index = _enchantmentcacheID.IndexOf(ItemID);
			return index >= 0 && index < _enchantmentcacheID.Count ? _enchantment[index] : null;
		}
	}
}
