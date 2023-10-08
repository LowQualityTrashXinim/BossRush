using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ForceOfEarth {
	public class EarthPower : ModBuff {
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Power of Earth");
			// Description.SetDefault("Calm and study, yet stealthly and quite as a wind");
			Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.statDefense += 2;
			player.lifeRegen += 5;
		}
	}
}
