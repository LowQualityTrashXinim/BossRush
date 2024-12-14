using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.WeaponEnchantment;
using Terraria.ID;
using BossRush.Contents.Perks;

namespace BossRush.Contents.Artifacts {
	internal class TokenOfPrideArtifact : Artifact {
		public override int Frames => 10;
		public override Color DisplayNameColor => Color.PaleGreen;
	}
	public class PridePlayer : ModPlayer {
		bool Pride = false;
		protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
		public override void ResetEffects() {
			Pride = Player.HasArtifact<TokenOfPrideArtifact>();
		}
		public override void PostUpdate() {
			if (Pride) {
				chestmodplayer.DropModifier *= 0;
			}
		}
		public override void PreUpdate() {
			if (!Pride) {
				return;
			}
			Item item = Player.HeldItem;
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				if (globalitem.EnchantmenStlot == null || globalitem.EnchantmenStlot.Length < 1 && EnchantmentGlobalItem.CanBeEnchanted(item)
					|| globalitem.EnchantmenStlot[3] != ItemID.None || globalitem.EnchantmenStlot[3] == -1) {
					return;
				}
				EnchantmentSystem.EnchantItem(ref item, 3);
			}
		}
	}
	public class BlidedPride : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .24f + .1f * StackAmount(player);
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			crit += 5 + 5 * StackAmount(player);
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Vector2.DistanceSquared(player.Center, target.Center) <= (300f * 300f) + 10000 * StackAmount(player)) {
				return;
			}
			modifiers.FinalDamage *= .5f;
		}
	}
}
