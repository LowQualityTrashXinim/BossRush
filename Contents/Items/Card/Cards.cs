using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Card {
	internal class StatsResetCard : ModItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.LunarBar);
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(30, 24);
			Item.UseSound = SoundID.Item35;
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 99;
		}
		public override bool? UseItem(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
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
			modplayer.CritDamage = 0;
			modplayer.DefenseEffectiveness = 0;
			modplayer.DropAmountIncrease = 0;
			modplayer.MinionSlot = 0;
			modplayer.SentrySlot = 0;
			modplayer.Thorn = 0;
			modplayer.CardLuck = 0;
			return true;
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
		public override sealed void ModifyTooltips(List<TooltipLine> tooltips) {
			tooltips.Add(new TooltipLine(Mod, "FunCardItem", $"[c/{Main.DiscoColor.Hex3()}:Fun Item]"));
		}
		public virtual void CardSetDefault() {

		}
		public override bool? UseItem(Player player) {
			OnUseCard(player, player.GetModPlayer<PlayerStatsHandle>(), out bool Consumeable);
			return Consumeable;
		}
		public virtual void OnUseCard(Player player, PlayerStatsHandle modplayer, out bool Consumeable) {
			Consumeable = true;
		}
	}
	internal class AuraDamageCard : BaseCard {
		public override string Texture => BossRushTexture.EMPTYCARD;
		public override void CardSetDefault() {
			Item.useAnimation = Item.useTime = 25;
			Item.rare = ItemRarityID.White;
		}
		public override void OnUseCard(Player player, PlayerStatsHandle modplayer, out bool Consumeable) {
			float radius = modplayer.AuraRadius;
			player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
			foreach (var npc in npclist) {
				npc.StrikeNPC(npc.CalculateHitInfo(20, 0));
			}
			for (int i = 0; i < 100; i++) {
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
			Item.rare = ItemRarityID.White;
		}
		public override void OnUseCard(Player player, PlayerStatsHandle modplayer, out bool Consumeable) {
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
			Item.rare = ItemRarityID.White;
		}
		public override void OnUseCard(Player player, PlayerStatsHandle modplayer, out bool Consumeable) {
			player.Heal(1);
			Consumeable = true;
		}
	}
	internal class HealingCard : BaseCard {
		public override string Texture => BossRushTexture.EMPTYCARD;
		public override void CardSetDefault() {
			Item.useAnimation = Item.useTime = 10;
			Item.rare = ItemRarityID.Blue;
		}
		public override void OnUseCard(Player player, PlayerStatsHandle modplayer, out bool Consumeable) {
			player.Heal(10);
			Consumeable = true;
		}
	}
}
