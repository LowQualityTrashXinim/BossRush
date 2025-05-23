using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.FrostSwordFish {
	internal class FrostSwordFish : SynergyModItem {
		public override void SetDefaults() {
			BossRushUtils.BossRushSetDefault(Item, 60, 64, 37, 6f, 18, 18, ItemUseStyleID.Swing, true);
			Item.DamageType = DamageClass.Melee;

			Item.rare = ItemRarityID.Green;
			Item.crit = 5;
			Item.value = Item.buyPrice(gold: 50);
			Item.useTurn = false;
			Item.scale += 0.25f;
			Item.UseSound = SoundID.Item1;

			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
				meleeItem.SwingType = BossRushUseStyle.Swipe2;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			base.ModifySynergyToolTips(ref tooltips, modplayer);
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			base.HoldSynergyItem(player, modplayer);
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
			Vector2 pos = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(player.Center, 400, 400);
			Projectile.NewProjectile(Item.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<FrostDaggerFishP>(), hit.Damage, hit.Knockback, player.whoAmI);
			target.AddBuff(BuffID.Frostburn, 180);
		}
		public override void MeleeEffects(Player player, Rectangle hitbox) {
			Vector2 hitboxCenter = new Vector2(hitbox.X, hitbox.Y);
			Dust.NewDust(hitboxCenter, hitbox.Width, hitbox.Height, DustID.IceRod, 0, 0, 0, Color.Aqua, .75f);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.IceBlade)
				.AddIngredient(ItemID.FrostDaggerfish, 100)
				.Register();
		}
	}
}
