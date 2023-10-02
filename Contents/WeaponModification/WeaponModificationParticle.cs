using Terraria.ModLoader;
using System.Collections.Generic;

namespace BossRush.Contents.WeaponModification
{
    public abstract class ModWeaponParticle : ModType
    {
        public int Type { get; private set; }
        public short WeaponModType = -1;
        protected sealed override void Register()
        {
            Type = ModWeapoParticleLoader.Register(this);
        }
        public static int GetWeaponParticleType<T>() where T : ModWeaponParticle
        {
            return ModContent.GetInstance<T>().Type;
        }
    }
    public static class ModWeapoParticleLoader
    {
        private static readonly List<ModWeaponParticle> _weaponParticle= new();
        public static int TotalCount => _weaponParticle.Count;
        public static int Register(ModWeaponParticle weaponParticle)
        {
            ModTypeLookup<ModWeaponParticle>.Register(weaponParticle);
            _weaponParticle.Add(weaponParticle);
            return _weaponParticle.Count - 1;
        }
        public static ModWeaponParticle GetPerk(int type)
        {
            return type >= 0 && type < _weaponParticle.Count ? _weaponParticle[type] : null;
        }
    }
}
