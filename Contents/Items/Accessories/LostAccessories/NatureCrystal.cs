using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories {
	class NatureCrystal : ModItem {
		public override void SetDefaults() {
			Item.DefaultToAccessory(28, 30);
			Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
			Item.value = 1000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statLifeMax2 += 40;
			player.statManaMax2 += 40;
			player.GetModPlayer<NatureCrystalPlayer>().NatureCrystal = true;
		}
	}
	class NatureCrystalPlayer : ModPlayer {
		public bool NatureCrystal = false;
		public override void ResetEffects() {
			NatureCrystal = false;
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			HealBaseOnDamage(hurtInfo.Damage);
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			HealBaseOnDamage(hurtInfo.Damage);
		}
		public void HealBaseOnDamage(int damage) {
			if (!NatureCrystal) {
				return;
			}
			int damageReal = (int)(damage * .1f);
			Player.Heal(damageReal);
			Player.statMana += Main.rand.Next(3, 10);
			if (Player.statMana > Player.statManaMax2) {
				Player.statMana = Player.statManaMax2;
			}
			Player.ManaEffect(damageReal);
		}
	}
}
