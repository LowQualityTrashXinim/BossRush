using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Card {
	internal class SolarCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentSolar);
		public override void PostCardSetDefault() {
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Red;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.ChestLoot.MeleeChanceMutilplier += .5f;
		}
	}
	internal class VortexCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentVortex);
		public override void PostCardSetDefault() {
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Red;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.ChestLoot.RangeChanceMutilplier += .5f;
		}
	}
	internal class NebulaCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentNebula);
		public override void PostCardSetDefault() {
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Red;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.ChestLoot.MagicChanceMutilplier += .5f;
		}
	}
	internal class StarDustCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentStardust);
		public override void PostCardSetDefault() {
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Red;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.ChestLoot.SummonChanceMutilplier += .5f;
		}
	}
	internal class ResetCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.LunarBar);
		public override void PostCardSetDefault() {
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Red;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.ChestLoot.MeleeChanceMutilplier = 1;
			modplayer.ChestLoot.RangeChanceMutilplier = 1;
			modplayer.ChestLoot.MagicChanceMutilplier = 1;
			modplayer.ChestLoot.SummonChanceMutilplier = 1;
			modplayer.MeleeDMG = 0;
			modplayer.RangeDMG = 0;
			modplayer.MagicDMG = 0;
			modplayer.SummonDMG = 0;
			modplayer.Movement = 0;
			modplayer.JumpBoost = 0;
			modplayer.HPMax = 0;
			modplayer.HPRegen = 0;
			modplayer.ManaMax = 0;
			modplayer.ManaRegen = 0;
			modplayer.DefenseBase = 0;
			modplayer.DamagePure = 0;
			modplayer.CritStrikeChance = 0;
			modplayer.CritDamage = 1;
			modplayer.DefenseEffectiveness = 1;
			modplayer.DropAmountIncrease = 0;
			modplayer.MinionSlot = 0;
			modplayer.SentrySlot = 0;
			modplayer.Thorn = 0;
			modplayer.CardLuck = 0;
		}
	}
	internal class CopperCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperBar);
		public override void PostCardSetDefault() {
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override int Tier => 1;
	}
	internal class SilverCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SilverBar);
		public override void PostCardSetDefault() {
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override int Tier => 2;
	}
	internal class GoldCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldBar);
		public override void PostCardSetDefault() {
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override int Tier => 3;
	}
	internal class PlatinumCard : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumBar);
		public override void PostCardSetDefault() {
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override int Tier => 4;
	}
	class CopperCardNormalizer : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperBrickWall);
		public override void PostCardSetDefault() {
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.CardLuck = Math.Clamp(modplayer.CardLuck - Main.rand.Next(11), 0, 200);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<CopperCard>(), 10)
				.Register();
		}
	}
	class SilverCardNormalizer : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SilverBrickWall);
		public override void PostCardSetDefault() {
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.CardLuck = Math.Clamp(modplayer.CardLuck - Main.rand.Next(5, 21), 0, 200);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<SilverCard>(), 10)
				.Register();
		}
	}
	class GoldCardNormalizer : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldBrickWall);
		public override void PostCardSetDefault() {
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.CardLuck = Math.Clamp(modplayer.CardLuck - Main.rand.Next(15, 36), 0, 200);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<GoldCard>(), 10)
				.Register();
		}
	}
	class PlatinumCardNormalizer : CardItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumBrickWall);
		public override void PostCardSetDefault() {
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override void OnUseItem(Player player, PlayerCardHandle modplayer) {
			modplayer.CardLuck = Math.Clamp(modplayer.CardLuck - Main.rand.Next(25, 66), 0, 200);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<PlatinumCard>(), 10)
				.Register();
		}
	}
	//This was ported from a secret mod of mine, it is badly made, but it should work most of it
	public abstract class BaseCard : ModItem {
		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 64;
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.scale = .5f;
			CardSetDefault();
		}
		public virtual void CardSetDefault() {

		}
		public override bool? UseItem(Player player) {
			OnUseCard(player, player.GetModPlayer<PlayerCardHandle>(), out bool Consumeable);
			return Consumeable;
		}
		public virtual void OnUseCard(Player player, PlayerCardHandle modplayer, out bool Consumeable) {
			Consumeable = true;
		}
	}
	internal class AuraDamageCard : BaseCard {
		public override string Texture => BossRushTexture.EMPTYCARD;
		public override void CardSetDefault() {
			Item.useAnimation = Item.useTime = 25;
			Item.rare = 0;
		}
		public override void OnUseCard(Player player, PlayerCardHandle modplayer, out bool Consumeable) {
			float radius = modplayer.AuraRadius;
			player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
			foreach (var npc in npclist) {
				npc.StrikeNPC(npc.CalculateHitInfo(20, 0));
			}
			for (int i = 0;i < 100; i++) {
				int dust = Dust.NewDust(player.Center + Vector2.One.Vector2DistributeEvenly(100, 360, i) * radius, 0, 0, DustID.GemDiamond);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
			}
			Consumeable = true;
		}
	}
	internal class ChaoticElementalCard : BaseCard {
		public override string Texture => BossRushTexture.EMPTYCARD;
		public override void CardSetDefault() {
			Item.useAnimation = Item.useTime = 25;
			Item.rare = 0;
		}
		public override void OnUseCard(Player player, PlayerCardHandle modplayer, out bool Consumeable) {
			int[] debuffArray = new int[] { BuffID.OnFire, BuffID.OnFire3, BuffID.Bleeding, BuffID.Frostburn, BuffID.Frostburn2, BuffID.ShadowFlame, BuffID.CursedInferno, BuffID.Ichor, BuffID.Venom, BuffID.Poisoned, BuffID.Confused, BuffID.Midas };
			player.Center.LookForHostileNPC(out NPC npc, modplayer.AuraRadius);
			if (npc != null) {
				npc.AddBuff(BuffID.OnFire, Main.rand.Next(debuffArray));
			}
			Consumeable = true;
		}
	}
	internal class WeakHealingCard : BaseCard {
		public override string Texture => BossRushTexture.EMPTYCARD;
		public override void CardSetDefault() {
			Item.useAnimation = Item.useTime = 10;
			Item.rare = 0;
		}
		public override void OnUseCard(Player player, PlayerCardHandle modplayer, out bool Consumeable) {
			player.Heal(1);
			Consumeable = true;
		}
	}
	internal class HealingCard : BaseCard {
		public override string Texture => BossRushTexture.EMPTYCARD;
		public override void CardSetDefault() {
			Item.useAnimation = Item.useTime = 10;
			Item.rare = 1;
		}
		public override void OnUseCard(Player player, PlayerCardHandle modplayer, out bool Consumeable) {
			player.Heal(10);
			Consumeable = true;
		}
	}
}
