using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Accessories.ReworkedAccessories;
public class FeralClaw : ItemReworker {

	public override int VanillaItemType => ItemID.FeralClaws;

	public override void UpdateEquip(Player player) {
		player.GetAttackSpeed(Terraria.ModLoader.DamageClass.Generic) += 0.05f;
		player.GetAttackSpeed(Terraria.ModLoader.DamageClass.Generic) += MathHelper.Lerp(0.25f, 0f, player.statLife / (float)player.statLifeMax2);
	}

}
