using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace BossRush.Contents.WeaponModification
{
    public abstract class ModWeaponParticle : ModType
    {
        public int Type { get; private set; }
        public short WeaponModType = 0;
        public int ProjectileType = 0;
        public int RealDamage = 0;
        public float ModifierableDamage = 0;
        public float KnockBack = 0;
        public float shootspeed = 0;
        protected sealed override void Register()
        {
            Type = ModifierWeaponLoader.Register(this);
        }
        public static int GetWeaponModType<T>() where T : ModWeaponParticle
        {
            return ModContent.GetInstance<T>().Type;
        }
        /// <summary>
        /// This is to set basic stats of your modifier
        /// </summary>
        public virtual void SetDefault()
        {

        }
        public virtual void PreUpdate(Player player)
        {

        }
        public virtual void PostUpdate(Player player)
        {

        }
        public virtual void ModifyModificationDelay(Player player, ref float delay, ref float recharge, ref int castAmount)
        {

        }
        public virtual void ModifyAttack(Player player, ref float damage, ref float knockback, ref float shootspeed)
        {

        }
        public virtual void ModifyCritAttack(Player player, ref int critChance, ref float critDamage)
        {

        }
        public int ShootAmount = 0;
        public virtual Projectile Shoot(Player player, int i)
        {
            return null;
        }
    }
    public static class ModifierWeaponLoader
    {
        private static readonly List<ModWeaponParticle> _weaponParticle= new();
        public static int TotalCount => _weaponParticle.Count;
        public static int Register(ModWeaponParticle weaponParticle)
        {
            ModTypeLookup<ModWeaponParticle>.Register(weaponParticle);
            _weaponParticle.Add(weaponParticle);
            return _weaponParticle.Count - 1;
        }
        public static ModWeaponParticle GetWeaponMod(int type)
        {
            return type >= 0 && type < _weaponParticle.Count ? _weaponParticle[type] : null;
        }
    }
}
