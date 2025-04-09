using BossRush.Common.Global;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.RelicItem.RelicTemplateContent;
/// <summary>
/// This is my example on how to make a custom template that have your own stats<br/>
/// </summary>
public class SlimeSpikeTemplate : RelicTemplate {
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Relic_SlimeSpike", new(375, Vector2.Zero, false, Color.Blue));
	}
	//we can return whatever we want since this doesn't matter to what we are making,
	//however we could also still use this to indicate what damageclass the projectile should deal
	public override PlayerStats StatCondition(Relic relic, Player player) {
		return Main.rand.Next([
			PlayerStats.MeleeDMG,
			PlayerStats.RangeDMG,
			PlayerStats.MagicDMG,
			PlayerStats.SummonDMG,
			PlayerStats.PureDamage
		]);
	}
	public override StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) {
		//We are randomizing the base damage that our friendly slime spike gonna deal
		return new(1, 1, 0, 15 + Main.rand.Next(0, 6));
	}
	public override string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) {
		//This is to get the name of the stats which we set
		string Name = Enum.GetName(stat) ?? string.Empty;
		//we use localization and our localization string look like this
		//SlimeSpikeTemplate.Description: Shoot [c/{0}:{1} friendly slime spike] dealing [c/{2}:{3}] [c/{4}:{5}] when enemy are near
		//As such it is important to format these string so that custom value can be shown on relic
		return string.Format(Description, [
			//Terraria have a feature that allow we to add color to text, they uses hex3 for custom text coloring
				Color.Blue.Hex3(),
				relic.RelicTier.ToString(),
				Color.Red.Hex3(),
				//This is my custom method that convert float number to string
				RelicTemplateLoader.RelicValueToNumber(value.Base * (1 + .1f * (relic.RelicTier - 1))),
				Color.Yellow.Hex3(),
				Name
		]);
	}
	//This where our relic effect take place, it is think of this as a UpdateEquip hook in ModPlayer
	public override void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {
		DataStorer.ActivateContext(player, "Relic_SlimeSpike");

		//Look for any NPC near the player
		if (!player.Center.LookForAnyHostileNPC(375f) || modplayer.synchronize_Counter % 30 != 0) {
			return;
		}
		//We gonna make this template strength base from the relic Tier
		int Tier = relic.RelicTier;
		//Set damage type base on PlayerStats
		DamageClass dmgclass = PlayerStatsHandle.PlayerStatsToDamageClass(stat);
		//Spawn the projectiles base on Relic Tier
		for (int i = 0; i < Tier; i++) {
			Projectile proj = Projectile.NewProjectileDirect(
				player.GetSource_ItemUse(relic.Item, Type.ToString()),
				player.Center,
				Main.rand.NextVector2CircularEdge(7, 7),
				ModContent.ProjectileType<FriendlySlimeProjectile>(),
				(int)(value.Base * (1 + .1f * (Tier - 1))),
				2 + .5f * Tier,
				player.whoAmI);
			//Setting projectile travel distance before killing
			proj.Set_ProjectileTravelDistance(375);
			//Setting projectile damage type
			proj.DamageType = dmgclass;
			//Set the projectile to ignore tile collision
			proj.tileCollide = false;
		}
	}
}
