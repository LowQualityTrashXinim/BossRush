using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.WeaponEnchantment {
	//ToDo : implement most of this stuff into Global Weapon instead of ModPlayer, modplayer should only hold data
	public abstract class ModEnchantment : ModType {
		public int Type { get; private set; }
		public int ItemIDType = ItemID.None;
		protected sealed override void Register() {
			SetDefaults();
			Type = EnchantmentLoader.Register(this);
		}
		public virtual void SetDefaults() { }
		public virtual void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
		public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
		public virtual void Update(Player player) { }
		public virtual void OnMissingMana(Player player, Item item, int neededMana) { }
		public virtual void ModifyDamage(Player player, Item item, ref StatModifier damage) { }
		public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitByAnything(Player player) { }
		public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
		public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
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
			return _enchantment[_enchantmentcacheID.IndexOf(ItemID)];
		}
	}
}
