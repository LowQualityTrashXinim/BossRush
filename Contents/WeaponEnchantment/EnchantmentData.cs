using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.WeaponEnchantment {
	public abstract class CommonGunEnchantment : ModEnchantment {
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .1f;
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			crit += 5;
		}
	}
	public class Musket : CommonGunEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.Musket;
		}
	}
	public class FlintlockPistol : CommonGunEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.FlintlockPistol;
		}
	}
	public class Minishark : CommonGunEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.Minishark;
		}
		public override void ModifyUseSpeed(Player player, Item item, ref float useSpeed) {
			base.ModifyUseSpeed(player, item, ref useSpeed);
			useSpeed += .25f;
		}
	}
	public class TheUndertaker : CommonGunEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.TheUndertaker;
		}
	}
	public class Boomstick : CommonGunEnchantment {
		public override void SetDefaults() {
			ItemIDType = ItemID.Boomstick;
		}
	}
}
