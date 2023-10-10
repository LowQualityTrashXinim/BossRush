using BossRush.Texture;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.WeaponModification {
	public abstract class ModWeaponParticle : ModType {
		public virtual string ParticleTexture => BossRushTexture.MISSINGTEXTURE;
		public ModWeaponParticle() {
			SetDefault();
		}
		public int Type { get; private set; }
		public int ProjectileType = 0;
		/// <summary>
		/// This is the real damage where projectile damage is actually set
		/// </summary>
		public int ProjectileDamage = 0;
		/// <summary>
		/// This is set outside of <see cref="Shoot(Player, int)"/> method
		/// </summary>
		public float KnockBack = 0;
		/// <summary>
		/// This is shoot speed which will be set outside of <see cref="Shoot(Player, int)"/> method
		/// </summary>
		public float ShootSpeed = 0;
		/// <summary>
		/// This is to make <see cref="Shoot(Player, int)"/> run multiple time <br/>
		/// default to 1
		/// </summary>
		public int ShootAmount = 1;
		protected sealed override void Register() {
			Type = ModifierWeaponLoader.Register(this);
		}
		public static int GetWeaponModType<T>() where T : ModWeaponParticle {
			return ModContent.GetInstance<T>().Type;
		}
		/// <summary>
		/// This is to set basic stats of your modifier
		/// </summary>
		public virtual void SetDefault() {

		}
		public virtual void PreUpdate(Player player) {

		}
		public virtual void PostUpdate(Player player) {

		}
		/// <summary>
		/// Use this if your mod type can change how delay, recharge, and amount to cast
		/// </summary>
		/// <param name="player"></param>
		/// <param name="delay">1 second = 60 tick</param>
		/// <param name="recharge">1 second = 60 tick</param>
		/// <param name="castAmount">Amount that can be cast</param>
		public virtual void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount) {

		}
		/// <summary>
		/// Use this if your mod type can modify other mod type damage
		/// Please also include <see cref="ModifyModificationDelay(Player, ref float, ref float, ref int)"/> if this a modification type
		/// </summary>
		/// <param name="player"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="shootspeed"></param>
		public virtual void ModifyAttack(Player player, ref StatModifier damage, ref float knockback, ref float shootspeed) {

		}
		/// <summary>
		/// Use this if your mod type can modify other mod type critical related
		/// Please also include <see cref="ModifyModificationDelay(Player, ref float, ref float, ref int)"/> if this a modification type
		/// </summary>
		/// <param name="player"></param>
		/// <param name="critChance"></param>
		/// <param name="critDamage"></param>
		public virtual void ModifyCritAttack(Player player, ref int critChance, ref float critDamage) {

		}
		/// <summary>
		/// This is by default is set to shoot out a projectile, modify it if you want it to shoot multiple projectile in a arch<br/>
		/// If so, then use <see cref="ShootAmount"/> in <see cref="SetDefault"/>
		/// </summary>
		/// <param name="player"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		public virtual Projectile Shoot(Player player, int i) {
			return Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.Center, Vector2.Zero, ProjectileType, ProjectileDamage, KnockBack, player.whoAmI);
		}
	}
	public static class ModifierWeaponLoader {
		private static readonly List<ModWeaponParticle> _weaponParticle = new();
		public static int TotalCount => _weaponParticle.Count;
		public static int Register(ModWeaponParticle weaponParticle) {
			ModTypeLookup<ModWeaponParticle>.Register(weaponParticle);
			_weaponParticle.Add(weaponParticle);
			return _weaponParticle.Count - 1;
		}
		public static ModWeaponParticle GetWeaponMod(int type) {
			return type >= 0 && type < _weaponParticle.Count ? _weaponParticle[type] : null;
		}
	}
}
