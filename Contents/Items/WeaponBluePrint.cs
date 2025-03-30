using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace BossRush.Contents.Items {
	class WeaponBluePrint : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.width = Item.height = 32;
			Item.material = true;
		}
	}
}
