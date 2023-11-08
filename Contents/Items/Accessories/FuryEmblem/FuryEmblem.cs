using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Items.Weapon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.FuryEmblem {
	class FuryEmblem : SynergyModItem {
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 40;
			Item.width = 40;
			Item.rare = ItemRarityID.Lime;
			Item.value = 10000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage<GenericDamageClass>() += 0.05f;
			player.GetCritChance<GenericDamageClass>() += 5;
			player.statLifeMax2 += (int)(player.statLifeMax2 * 0.25f);
			player.GetModPlayer<FuryPlayer>().Furious2 = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
		   .AddIngredient(ItemID.DestroyerEmblem, 1)
		   .AddIngredient(ItemID.AvengerEmblem, 1)
		   .AddIngredient(ItemID.Vitamins, 1)
		   .Register();
		}
	}
	class FuryPlayer : ModPlayer {
		public bool Furious2;
		public bool CooldownFurious;
		public override void ResetEffects() {
			Furious2 = false;
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			if (Furious2 && !CooldownFurious) {
				Player.AddBuff(ModContent.BuffType<Furious>(), 600);
			}
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			if (Furious2 && !CooldownFurious) {
				Player.AddBuff(ModContent.BuffType<Furious>(), 600);
			}
		}
	}
}
