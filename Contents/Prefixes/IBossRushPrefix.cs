using Terraria.ModLoader;

namespace BossRush.Contents.Prefixes {
	interface IBossRushPrefix {
		public PrefixCategory WeaponType { get; }
		public float WeaponRollchance { get; }
	}
}
