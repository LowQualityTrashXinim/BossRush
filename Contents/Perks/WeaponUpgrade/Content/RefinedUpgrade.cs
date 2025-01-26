using BossRush.Common.RoguelikeChange;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Perks;
using BossRush.Contents.Perks.WeaponUpgrade;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Perks.WeaponUpgrade.Content;
internal class RefinedUpgrade_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.RefinedUpgrade)) {
			return;
		}
		if (entity.axe <= 0 || entity.noMelee) {
			return;
		}
		if (entity.TryGetGlobalItem(out GlobalItemHandle glo)) {
			glo.CriticalDamage += 2f;
		}
		entity.damage += 10;

	}
	public override void HoldItem(Item item, Player player) {
		if (UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.RefinedUpgrade)) {
			if (item.axe <= 0 || item.noMelee) {
				return;
			}
			player.GetModPlayer<MeleeOverhaulPlayer>().DelayReuse -= .5f;
		}
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.RefinedUpgrade)) {
			if (item.axe <= 0 || item.noMelee) {
				return;
			}
			if (Main.rand.NextBool(3)) {
				target.AddBuff(ModContent.BuffType<Axe_BleedDebuff>(), BossRushUtils.ToSecond(Main.rand.Next(3, 8)));
			}
		}
	}
}
class Axe_BleedDebuff : ModBuff {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeOverhaulNPC>().StatDefense -= .2f;
		npc.lifeRegen -= 10;
	}
}
public class RefinedUpgrade : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.RefinedUpgrade);
		Mod.Reflesh_GlobalItem(player);
		int[] Orestaff = {
		ItemID.CopperAxe,
		ItemID.TinAxe,
		ItemID.IronAxe,
		ItemID.LeadAxe,
		ItemID.SilverAxe,
		ItemID.TungstenAxe,
		ItemID.GoldAxe,
		ItemID.PlatinumAxe
		}; 
		player.QuickSpawnItem(player.GetSource_Misc("WeaponUpgrade"), Main.rand.Next(Orestaff));
	}
}
