using Terraria;

namespace BossRush.Contents.Items.Weapon.DupeSynergy {
	internal class ComboHandle {
		int Time = 0;
		int[] ComboGet = new int[2];

		public void GetAmount(int amount) {
			Time = amount;
		}

		public void CheckCombo(int MouseClick, out int attackTypeEnhancer) {
			attackTypeEnhancer = 0;
			if (Time % 2 == 0) {
				if (Time % 2 == 0 || Time % 2 == 1) {
					if (MouseClick == 1) {
						ComboGet[Time % 2] = 1;
					}
					else if (MouseClick == 2) {
						ComboGet[Time % 2] = 2;
					}
				}
				if (ComboGet[0] != 0 && ComboGet[1] != 0) {
					if (ComboGet[0] == 1 && ComboGet[1] == 1) {
						attackTypeEnhancer = 1;
					}
					else if (ComboGet[0] == 1 && ComboGet[1] == 2) {
						attackTypeEnhancer = 2;
					}
					else if (ComboGet[1] == 1 && ComboGet[0] == 2) {
						attackTypeEnhancer = 3;
					}
					else if (ComboGet[0] == 2 && ComboGet[1] == 2) {
						attackTypeEnhancer = 4;
					}
				}
				else {
					attackTypeEnhancer = 0;
				}
			}
		}
		public void Transfer(Player player, out int AttackType) {
			CheckCombo(player.altFunctionUse, out int Type1);
			AttackType = Type1;
		}
	}
}
