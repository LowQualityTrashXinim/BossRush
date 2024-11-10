using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.SuperShortSword {
	public class SuperShortSwordPower : ModBuff {
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false; 
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.statDefense += 8;
		}
	}
}
