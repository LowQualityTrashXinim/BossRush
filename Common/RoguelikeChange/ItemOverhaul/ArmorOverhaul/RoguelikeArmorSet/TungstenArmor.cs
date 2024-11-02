using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class TungstenArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.TungstenHelmet;
		bodyID = ItemID.TungstenChainmail;
		legID = ItemID.TungstenGreaves;
	}
}
public class TungstenHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TungstenHelmet;
		Add_Defense = 4;
	}
}
public class TungstenChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TungstenChainmail;
		Add_Defense = 5;
	}
}

public class TungstenGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TungstenGreaves;
		Add_Defense = 4;
	}
}
public class TungstenArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("TungstenArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.statLife >= Player.statLifeMax2) {
			Player.moveSpeed += .3f;
			Player.statDefense *= 0;
		}
	}
	public override void Armor_ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		float DamageIncrease = (target.Center - Player.Center).Length();
		modifiers.SourceDamage += MathHelper.Clamp(600 - DamageIncrease, 0, 200) * .005f;
	}
}
