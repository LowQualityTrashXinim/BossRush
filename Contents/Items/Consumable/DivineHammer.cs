using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable;
internal class DivineHammer : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.noUseGraphic = true;
		Item.UseSound = SoundID.Item37;
	}
}
