using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Contents.Items.Accessories.Scabbard {
	internal class ParryScabbard : SynergyModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			Player player = Main.LocalPlayer;
			if (player.GetModPlayer<SwordPlayer>().SwordSlash) {
				tooltips.Add(new TooltipLine(Mod, "SwordBrother", $"[i:{ModContent.ItemType<SwordScabbard>()}] Increase parry duration and increase wind slash speed"));
			}
		}
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 30;
			Item.width = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = 1000000;
		}

		public override void UpdateEquip(Player player) {
			player.GetModPlayer<ParryPlayer>().Parry = true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddRecipeGroup("Wood Sword")
				.AddRecipeGroup("OreBroadSword")
				.Register();
		}
	}
	public class ParryPlayer : ModPlayer {
		public bool Parry;
		public override void ResetEffects() {
			Parry = false;
		}
		public override void ModifyHurt(ref Player.HurtModifiers modifiers)/* tModPorter Override ImmuneTo, FreeDodge or ConsumableDodge instead to prevent taking damage */
		{
			Item item = Player.HeldItem;
			if (!Player.HasBuff(ModContent.BuffType<Parried>())
				&& item.DamageType == DamageClass.Melee
				&& Player.ItemAnimationActive
				&& item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckVanillaSwingWithModded)
				&& !Player.HasBuff(ModContent.BuffType<CoolDownParried>())
				&& Parry) {
				int duration = Player.GetModPlayer<SwordPlayer>().SwordSlash ? 240 : 120;
				Player.AddBuff(ModContent.BuffType<Parried>(), duration);
			}
		}
		public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable) {
			if (Player.HasBuff(ModContent.BuffType<Parried>())) {
				return true;
			}
			return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
		}
	}
}
