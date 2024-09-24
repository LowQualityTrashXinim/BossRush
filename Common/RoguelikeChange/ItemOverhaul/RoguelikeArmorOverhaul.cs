using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Projectiles;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Common.General;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul;
class RoguelikeArmorOverhaul : GlobalItem {
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		//Note : Use look for tooltip with Defense if there are gonna be modification to defenses
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		Player player = Main.LocalPlayer;
		int[] armorSet = new int[] { player.armor[0].type, player.armor[1].type, player.armor[2].type };
		int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
		if (index != -1) {
			switch (item.type) {
				case ItemID.BeeHeadgear:
					tooltips[index].Text = Language.GetTextValue($"Mods.BossRush.ArmorSet.BeeHeadgear");
					break;
				case ItemID.BeeBreastplate:
					tooltips[index].Text = Language.GetTextValue($"Mods.BossRush.ArmorSet.BeeHeadgear");
					break;
				case ItemID.BeeGreaves:
					tooltips[index].Text = Language.GetTextValue($"Mods.BossRush.ArmorSet.BeeGreaves");
					break;
			}
		}
		int defindex = tooltips.FindIndex(line => line.Name == "Defense");
		if (defindex != -1) {
			switch (item.type) {
				case ItemID.BeeHeadgear:
					tooltips[defindex].Text = Language.GetTextValue($"Mods.BossRush.ArmorSet.BeeHeadgear_Defense");
					break;
				case ItemID.BeeBreastplate:
					tooltips[defindex].Text = Language.GetTextValue($"Mods.BossRush.ArmorSet.BeeBreastplate_Defense");
					break;
				case ItemID.BeeGreaves:
					tooltips[defindex].Text = Language.GetTextValue($"Mods.BossRush.ArmorSet.BeeGreaves_Defense");
					break;
			}
		}
		foreach (TooltipLine tooltipLine in tooltips) {
			if (tooltipLine.Name != "SetBonus") {
				continue;
			}
			if (armorSet.Contains(ItemID.BeeHeadgear) || armorSet.Contains(ItemID.BeeBreastplate) || armorSet.Contains(ItemID.BeeGreaves)) {
				tooltipLine.Text = Language.GetTextValue($"Mods.BossRush.ArmorSet.BeeArmorModified");
			}
			if (armorSet.Contains(item.type)) {
				tooltipLine.Text += "\n" + GetToolTip(item.type);
				return;
			}
		}
	}
	public override string IsArmorSet(Item head, Item body, Item legs) {
		if (!UniversalSystem.Check_RLOH()) {
			return "";
		}
		return new ArmorSet(head.type, body.type, legs.type).ToString();
	}
	//I really need to make this whole GetToolTip and UpdateArmorSet to be somehow it's own classes, maybe utilize ArmorSet class ?
	private string GetToolTip(int type) {
		if (type == ItemID.WoodHelmet || type == ItemID.WoodBreastplate || type == ItemID.WoodGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.WoodArmor");
		}
		if (type == ItemID.BorealWoodHelmet || type == ItemID.BorealWoodBreastplate || type == ItemID.BorealWoodGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.BorealWoodArmor");
		}
		if (type == ItemID.RichMahoganyHelmet || type == ItemID.RichMahoganyBreastplate || type == ItemID.RichMahoganyGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.RichMahoganyArmor");
		}
		if (type == ItemID.ShadewoodHelmet || type == ItemID.ShadewoodBreastplate || type == ItemID.ShadewoodGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.ShadewoodArmor");
		}
		if (type == ItemID.EbonwoodHelmet || type == ItemID.EbonwoodBreastplate || type == ItemID.EbonwoodGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.EbonwoodArmor");
		}
		if (type == ItemID.AshWoodHelmet || type == ItemID.AshWoodBreastplate || type == ItemID.AshWoodGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.AshWoodArmor");
		}
		if (type == ItemID.CactusHelmet || type == ItemID.CactusBreastplate || type == ItemID.CactusLeggings) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.CactusArmor");
		}
		if (type == ItemID.PalmWoodHelmet || type == ItemID.PalmWoodBreastplate || type == ItemID.PalmWoodGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.PalmWoodArmor");
		}
		if (type == ItemID.PumpkinHelmet || type == ItemID.PumpkinBreastplate || type == ItemID.PumpkinLeggings) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.PumpkinArmor");
		}
		if (type == ItemID.TinHelmet || type == ItemID.TinChainmail || type == ItemID.TinGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.TinArmor");
		}
		if (type == ItemID.LeadHelmet || type == ItemID.LeadChainmail || type == ItemID.LeadGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.LeadArmor");
		}
		if (type == ItemID.CopperHelmet || type == ItemID.CopperChainmail || type == ItemID.CopperGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.CopperArmor");
		}
		if (type == ItemID.PearlwoodHelmet || type == ItemID.PearlwoodBreastplate || type == ItemID.PearlwoodGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.PearlArmor");
		}
		if (type == ItemID.IronHelmet || type == ItemID.IronChainmail || type == ItemID.IronGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.IronArmor");
		}
		if (type == ItemID.SilverHelmet || type == ItemID.SilverChainmail || type == ItemID.SilverGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.SilverArmor");
		}
		if (type == ItemID.TungstenHelmet || type == ItemID.TungstenChainmail || type == ItemID.TungstenGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.TungstenArmor");
		}
		if (type == ItemID.GoldHelmet || type == ItemID.GoldChainmail || type == ItemID.GoldGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.GoldArmor");
		}
		if (type == ItemID.PlatinumHelmet || type == ItemID.PlatinumChainmail || type == ItemID.PlatinumGreaves) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.PlatinumArmor");
		}
		if (type == ItemID.JungleHat || type == ItemID.JungleShirt || type == ItemID.JunglePants) {
			return Language.GetTextValue($"Mods.BossRush.ArmorSet.JungleArmor");
		}
		return "";
	}
	public override void UpdateArmorSet(Player player, string set) {
		RoguelikeArmorPlayer modplayer = player.GetModPlayer<RoguelikeArmorPlayer>();
		if (WoodAndFruitTypeArmor(player, modplayer, set)) { return; }
		else if (OreTypeArmor(player, modplayer, set)) { return; }
		else if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.JungleHat, ItemID.JungleShirt, ItemID.JunglePants)) {
			modplayer.JungleArmor = true;
		}
		else if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.BeeHeadgear, ItemID.BeeBreastplate, ItemID.BeeGreaves)) {
			player.GetDamage(DamageClass.Melee) += .1f;
			player.GetDamage(DamageClass.Ranged) += .1f;
			player.GetDamage(DamageClass.Magic) += .1f;
			modplayer.BeeArmor = true;
			player.maxMinions++;
		}
	}
	private bool WoodAndFruitTypeArmor(Player player, RoguelikeArmorPlayer modplayer, string set) {
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.WoodHelmet, ItemID.WoodBreastplate, ItemID.WoodGreaves)) {
			if (player.ZoneForest) {
				player.statDefense += 11;
				player.moveSpeed += .25f;
				modplayer.WoodArmor = true;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.BorealWoodHelmet, ItemID.BorealWoodBreastplate, ItemID.BorealWoodGreaves)) {
			if (player.ZoneSnow) {
				player.statDefense += 13;
				player.moveSpeed += .20f;
				player.buffImmune[BuffID.Chilled] = true;
				player.buffImmune[BuffID.Slow] = true;
				modplayer.BorealWoodArmor = true;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.RichMahoganyHelmet, ItemID.RichMahoganyBreastplate, ItemID.RichMahoganyGreaves)) {
			if (player.ZoneJungle) {
				player.statDefense += 12;
				player.moveSpeed += .30f;
				modplayer.RichMahoganyArmor = true;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.ShadewoodHelmet, ItemID.ShadewoodBreastplate, ItemID.ShadewoodGreaves)) {
			if (player.ZoneCrimson) {
				player.statDefense += 17;
				player.moveSpeed += .15f;
				modplayer.ShadewoodArmor = true;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.EbonwoodHelmet, ItemID.EbonwoodBreastplate, ItemID.EbonwoodGreaves)) {
			if (player.ZoneCorrupt) {
				player.statDefense += 6;
				player.moveSpeed += .35f;
				player.GetDamage(DamageClass.Generic) += .05f;
				modplayer.EbonWoodArmor = true;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.AshWoodHelmet, ItemID.AshWoodBreastplate, ItemID.AshWoodGreaves)) {
			player.statDefense += 16;
			player.GetDamage(DamageClass.Generic) += .1f;
			if (player.ZoneUnderworldHeight || player.ZoneUnderworldHeight) {
				modplayer.AshWoodArmor = true;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.CactusHelmet, ItemID.CactusBreastplate, ItemID.CactusLeggings)) {
			player.statDefense += 10;
			modplayer.CactusArmor = true;
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PalmWoodHelmet, ItemID.PalmWoodBreastplate, ItemID.PalmWoodGreaves)) {
			player.statDefense += 10;
			player.moveSpeed += .17f;
			modplayer.PalmWoodArmor = true;
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PumpkinHelmet, ItemID.PumpkinBreastplate, ItemID.PumpkinLeggings)) {
			if (player.ZoneOverworldHeight) {
				modplayer.PumpkinArmor = true;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PearlwoodHelmet, ItemID.PearlwoodBreastplate, ItemID.PearlwoodGreaves)) {
			player.moveSpeed += 0.35f;
			player.statDefense += 12;
			modplayer.pearlWoodArmor = true;
			if (Main.dayTime)
				player.GetDamage(DamageClass.Generic) += 0.15f;
			return true;
		}
		return false;
	}
	private bool OreTypeArmor(Player player, RoguelikeArmorPlayer modplayer, string set) {
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.TinHelmet, ItemID.TinChainmail, ItemID.TinGreaves)) {
			player.statDefense += 5;
			player.moveSpeed += .21f;
			modplayer.TinArmor = true;
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.CopperHelmet, ItemID.CopperChainmail, ItemID.CopperGreaves)) {
			player.moveSpeed += 0.25f;
			modplayer.CopperArmor = true;
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves)) {
			player.endurance += 0.1f;
			player.DefenseEffectiveness *= 1.25f;
			if (player.statLife <= player.statLifeMax * 0.5f) {
				player.statDefense += 25;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.LeadHelmet, ItemID.LeadChainmail, ItemID.LeadGreaves)) {
			player.statDefense += 7;
			modplayer.LeadArmor = true;
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.SilverHelmet, ItemID.SilverChainmail, ItemID.SilverGreaves)) {
			bool IsAbover = player.statLife < player.statLifeMax2 * .75f;
			if (Main.dayTime) {
				player.statDefense += IsAbover ? 10 : 20;
			}
			else {
				player.GetDamage(DamageClass.Generic) += IsAbover ? .1f : .2f;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.TungstenHelmet, ItemID.TungstenChainmail, ItemID.TungstenGreaves)) {
			player.statDefense += 15;
			if (player.statLife >= player.statLifeMax2) {
				player.moveSpeed += .3f;
				modplayer.TungstenArmor = true;
			}
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.GoldHelmet, ItemID.GoldChainmail, ItemID.GoldGreaves)) {
			modplayer.GoldArmor = true;
			return true;
		}
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PlatinumHelmet, ItemID.PlatinumChainmail, ItemID.PlatinumGreaves)) {
			modplayer.PlatinumArmor = true;
			return true;
		}
		return false;
	}
	public override void UpdateEquip(Item item, Player player) {
		BeeArmorRework(player, item);
		if (item.type == ItemID.NightVisionHelmet) {
			player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify -= .25f;
		}
		if (item.type == ItemID.VikingHelmet) {
			player.GetModPlayer<GlobalItemPlayer>().RoguelikeOverhaul_VikingHelmet = true;
		}
		if (item.type == ItemID.ObsidianRose || item.type == ItemID.ObsidianSkullRose) {
			player.buffImmune[BuffID.OnFire] = true;
		}
	}
	private void BeeArmorRework(Player player, Item item) {
		if (item.type == ItemID.BeeHeadgear) {
			player.GetDamage(DamageClass.Melee) += .04f;
			player.GetDamage(DamageClass.Ranged) += .04f;
			player.GetDamage(DamageClass.Magic) += .04f;
			player.GetCritChance(DamageClass.Generic) += 3;
			player.statDefense += 6;
		}
		if (item.type == ItemID.BeeBreastplate) {
			player.GetDamage(DamageClass.Melee) += .05f;
			player.GetDamage(DamageClass.Ranged) += .05f;
			player.GetDamage(DamageClass.Magic) += .05f;
			player.GetAttackSpeed(DamageClass.Melee) += .06f;
			player.statDefense += 6;
		}
		if (item.type == ItemID.BeeGreaves) {
			player.GetDamage(DamageClass.Melee) += .05f;
			player.GetDamage(DamageClass.Ranged) += .05f;
			player.GetDamage(DamageClass.Magic) += .05f;
			player.manaCost -= .16f;
			player.statDefense += 5;
		}
	}
}
class RoguelikeArmorPlayer : ModPlayer {

	public const int DashRight = 2;
	public const int DashLeft = 3;

	public const int DashCooldown = 50;
	public const int DashDuration = 35;

	public const float DashVelocity = 12.5f;

	public int DashDir = -1;

	public int DashDelay = 0;
	public int DashTimer = 0;

	public bool WoodArmor = false;
	public bool BorealWoodArmor = false;
	public bool RichMahoganyArmor = false;
	public bool ShadewoodArmor = false;
	int ShadewoodArmorCD = 0;
	public bool EbonWoodArmor = false;
	int EbonWoodArmorCD = 0;
	public bool CactusArmor = false;
	int CactusArmorCD = 0;
	public bool PalmWoodArmor = false;
	public int PalmWoodArmor_SandCounter = 0;
	public bool PumpkinArmor = false;
	public bool AshWoodArmor = false;
	public bool CopperArmor = false;
	int CopperArmorChargeCounter = 0;
	public bool GoldArmor = false;
	public bool pearlWoodArmor = false;
	int pearlWoodArmorCD = 0;
	public bool TinArmor = false;
	public int TinArmorCountEffect = 0;
	public bool LeadArmor = false;
	public bool TungstenArmor = false;
	public bool PlatinumArmor = false;
	int PlatinumArmorCountEffect = 0;
	public bool JungleArmor = false;
	public bool BeeArmor = false;

	public override void ResetEffects() {
		WoodArmor = false;
		BorealWoodArmor = false;
		RichMahoganyArmor = false;
		ShadewoodArmor = false;
		EbonWoodArmor = false;
		CactusArmor = false;
		PalmWoodArmor = false;
		PumpkinArmor = false;
		AshWoodArmor = false;
		CopperArmor = false;
		GoldArmor = false;
		pearlWoodArmor = false;
		TinArmor = false;
		LeadArmor = false;
		TungstenArmor = false;
		PlatinumArmor = false;
		JungleArmor = false;
		BeeArmor = false;
		// ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
		// When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
		// If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
		if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15) {
			DashDir = DashRight;
		}
		else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15) {
			DashDir = DashLeft;
		}
		else {
			DashDir = -1;
		}
	}
	public override void UpdateDead() {
		WoodArmor = false;
		BorealWoodArmor = false;
		RichMahoganyArmor = false;
		ShadewoodArmor = false;
		EbonWoodArmor = false;
		CactusArmor = false;
		PalmWoodArmor = false;
		PumpkinArmor = false;
		AshWoodArmor = false;
		CopperArmor = false;
		GoldArmor = false;
		pearlWoodArmor = false;
		TinArmor = false;
		LeadArmor = false;
		TungstenArmor = false;
		PlatinumArmor = false;
		JungleArmor = false;
		BeeArmor = false;
	}
	public override void PreUpdateMovement() {
		if (CanUseDash() && DashDir != -1 && DashDelay == 0 && BeeArmor && Player.HeldItem.DamageType == DamageClass.Melee) {
			Vector2 newVelocity = Player.velocity;

			switch (DashDir) {
				case DashLeft when Player.velocity.X > -DashVelocity:
				case DashRight when Player.velocity.X < DashVelocity: {
						float dashDirection = DashDir == DashRight ? 1 : -1;
						newVelocity.X = dashDirection * DashVelocity;
						break;
					}
				default:
					return;
			}
			DashDelay = DashCooldown;
			DashTimer = DashDuration;
			Player.velocity = newVelocity;
		}

		if (DashDelay > 0)
			DashDelay--;

		if (DashTimer > 0) {
			Player.eocDash = DashTimer;
			Player.armorEffectDrawShadowEOCShield = true;
			DashTimer--;
		}
	}
	private bool CanUseDash() {
		return Player.dashType == DashID.None
			&& !Player.setSolar
			&& !Player.mount.Active;
	}
	public override void PreUpdate() {
		ShadewoodArmorCD = BossRushUtils.CountDown(ShadewoodArmorCD);
		EbonWoodArmorCD = BossRushUtils.CountDown(EbonWoodArmorCD);
		CactusArmorCD = BossRushUtils.CountDown(CactusArmorCD);
		pearlWoodArmorCD = BossRushUtils.CountDown(pearlWoodArmorCD);
		if (EbonWoodArmor) {
			if (EbonWoodArmorCD <= 0 && Player.velocity != Vector2.Zero) {
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2Circular(10, 10), -Player.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<CorruptionTrail>(), 3, 0, Player.whoAmI);
				EbonWoodArmorCD = 45;
			}
		}
		if (PalmWoodArmor) {
			if (Player.justJumped) {
				for (int i = 0; i < 4; i++) {
					Vector2 vec = new Vector2(-Player.velocity.X, Player.velocity.Y).Vector2RotateByRandom(20).LimitedVelocity(Main.rand.NextFloat(2, 3));
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, vec, ModContent.ProjectileType<SandProjectile>(), 12, 1f, Player.whoAmI);
				}
			}
		}
		if (PlatinumArmor) {
			if (Player.ItemAnimationActive) {
				PlatinumArmorCountEffect = Math.Clamp(PlatinumArmorCountEffect + 1, 0, 1200);
			}
			else {
				PlatinumArmorCountEffect = BossRushUtils.CountDown(PlatinumArmorCountEffect);
			}
		}
	}
	public override void PostUpdate() {
		if (TungstenArmor) {
			Player.statDefense *= 0;
		}
		if (PlatinumArmorCountEffect >= 600) {
			Player.AddBuff(BuffID.OnFire, 300);
			Dust.NewDust(Player.Center, 0, 0, DustID.Torch, 0, 0, 0, default, Main.rand.NextFloat(1, 1.5f));
		}
	}
	public override float UseSpeedMultiplier(Item item) {
		if (PlatinumArmor) {
			return 1.35f;
		}
		return base.UseSpeedMultiplier(item);
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (TinArmor) {
			if (item.useAmmo == AmmoID.Arrow) {
				velocity *= 2;
			}
		}
	}
	public float[] Projindex = new float[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (TinArmor) {
			if (item.useAmmo == AmmoID.Arrow) {
				Vector2 pos = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(position, 50, 50);
				Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * velocity.Length();
				Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<TinOreProjectile>(), damage, knockback, Player.whoAmI);
				TinArmorCountEffect++;
				if (TinArmorCountEffect >= 5) {
					Projectile.NewProjectile(source, position, velocity * 1.15f, ModContent.ProjectileType<TinBarProjectile>(), (int)(damage * 1.5f), knockback, Player.whoAmI);
					TinArmorCountEffect = 0;
				}
			}
			if (item.mana > 0 && Item.staff[item.type]) {
				for (int i = 0; i < 3; i++) {
					Vector2 vec = velocity.Vector2DistributeEvenly(3, 10, i);
					int proj = Projectile.NewProjectile(source, position, vec, type, damage, knockback, Player.whoAmI);
					Main.projectile[proj].extraUpdates = 10;
				}
				return false;
			}
			if (item.useStyle == ItemUseStyleID.Rapier) {
				Vector2 pos = position + Main.rand.NextVector2Circular(50, 50);
				Projectile.NewProjectile(source, pos, Main.MouseWorld - pos, ModContent.ProjectileType<TinShortSwordProjectile>(), damage, knockback, Player.whoAmI);
			}
		}
		if (JungleArmor) {
			if (item.DamageType == DamageClass.Magic) {

				float indexThatIsMissing = 0;
				for (int i = 0; i < Projindex.Length; i++) {
					if (Projindex[i] != -1)
						continue;
					indexThatIsMissing = i;
					Projindex[i] = 1;
					break;
				}
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<LeafProjectile>()] < 10) {
					Projectile.NewProjectile(source, Player.Center, Vector2.Zero, ModContent.ProjectileType<LeafProjectile>(), (int)(damage * 1.25f), knockback, Player.whoAmI, indexThatIsMissing);
				}
			}
		}
		if (PalmWoodArmor) {
			if (++PalmWoodArmor_SandCounter >= 7) {
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SandProjectile>(), (int)(damage * .5f), knockback, Player.whoAmI);
				if (PalmWoodArmor_SandCounter >= 10) {
					Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * 20f, ModContent.ProjectileType<CoconutProjectile>(), (int)(damage * 1.25f), knockback, Player.whoAmI);
					PalmWoodArmor_SandCounter = 0;
				}
			}
		}
		if (BeeArmor) {
			if (item.DamageType == DamageClass.Ranged) {
				int proj = Projectile.NewProjectile(source, position, velocity, ProjectileID.Stinger, damage, knockback, Player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].penetrate = 1;
			}
			if (item.DamageType == DamageClass.Magic) {
				int proj = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), ProjectileID.QueenBeeStinger, damage, knockback, Player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
			}
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitEffect_RichMahoganyArmor(proj);
		OnHitEffect_CactusArmor(proj);
		OnHitEffect_AshWoodArmor(proj);
		OnHitEffect_PumpkinArmor();
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		OnHitEffect_RichMahoganyArmor(npc);
		OnHitEffect_CactusArmor(npc);
		OnHitEffect_AshWoodArmor(npc);
		OnHitEffect_PumpkinArmor();
	}
	private void OnHitEffect_PumpkinArmor() {
		if (PumpkinArmor) {
			Player.AddBuff(BuffID.WellFed3, 300);
		}
	}
	private void OnHitEffect_AshWoodArmor(Entity entity) {
		if (AshWoodArmor) {
			int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, (entity.Center - Player.Center).SafeNormalize(Vector2.UnitX) * 10, ProjectileID.Flames, Main.rand.Next(5, 15), 1f, Player.whoAmI);
			Main.projectile[proj].penetrate = -1;
		}
	}
	private void OnHitEffect_RichMahoganyArmor(Entity entity) {
		if (RichMahoganyArmor) {
			for (int i = 0; i < 10; i++) {
				Vector2 spread = Vector2.One.Vector2DistributeEvenly(10f, 360, i);
				int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, spread * 2f, ProjectileID.BladeOfGrass, 12, 1f, Player.whoAmI);
				Main.projectile[proj].penetrate = -1;
			}
		}
	}
	private void OnHitEffect_CactusArmor(Entity entity) {
		if (CactusArmor) {
			if (CactusArmorCD <= 0) {
				bool manualDirection = Player.Center.X < entity.Center.X;
				Vector2 AbovePlayer = Player.Center + new Vector2(Main.rand.NextFloat(-500, 500), -1000);
				int projectile = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), AbovePlayer, Vector2.UnitX * .1f * manualDirection.ToDirectionInt(), ProjectileID.RollingCactus, 150, 0, Player.whoAmI);
				Main.projectile[projectile].friendly = true;
				Main.projectile[projectile].hostile = false;
				CactusArmorCD = 300;
			}
			for (int i = 0; i < 8; i++) {
				Vector2 vec = Vector2.One.Vector2DistributeEvenly(8, 360, i);
				int projectile = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, vec, ProjectileID.RollingCactusSpike, 15, 0, Player.whoAmI);
				Main.projectile[projectile].friendly = true;
				Main.projectile[projectile].hostile = false;
				Main.projectile[projectile].penetrate = -1;
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_ShadewoodArmor();
		OnHitNPC_BorealWoodArmor(target);
		OnHitNPC_WoodArmor(target, proj);
		OnHitNPC_PumpkinArmor(target, damageDone);
		OnHitNPC_AshWoodArmor(target);
		OnHitNPC_CopperArmor();
		OnHitNPC_GoldArmor(target, damageDone);
		OnHitNPC_LeadArmor(target);
		OnHitNPC_PearlWoodArmor(target);
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_ShadewoodArmor();
		OnHitNPC_BorealWoodArmor(target);
		OnHitNPC_WoodArmor(target);
		OnHitNPC_PumpkinArmor(target, damageDone);
		OnHitNPC_AshWoodArmor(target);
		OnHitNPC_CopperArmor();
		OnHitNPC_GoldArmor(target, damageDone);
		if (TinArmor)
			if (item.DamageType == DamageClass.Melee) {
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero), ModContent.ProjectileType<TinBroadSwordProjectile>(), 12, 1f, Player.whoAmI);
			}
		OnHitNPC_LeadArmor(target);
		OnHitNPC_PearlWoodArmor(target);
	}
	private void OnHitNPC_LeadArmor(NPC npc) {
		if (LeadArmor) {
			npc.AddBuff(ModContent.BuffType<LeadIrradiation>(), 600);
		}
	}
	private void OnHitNPC_WoodArmor(NPC target, Projectile proj = null) {
		if (!WoodArmor) {
			return;
		}
		if (Main.rand.NextBool(4) && (proj is null || proj is not null && proj.ModProjectile is not AcornProjectile)) {
			Projectile.NewProjectile(Player.GetSource_FromThis(),
				target.Center - new Vector2(0, 400),
				Vector2.UnitY * 10,
				ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Player.whoAmI);
		}
	}
	private void OnHitNPC_ShadewoodArmor() {
		if (!ShadewoodArmor) {
			return;
		}
		if (ShadewoodArmorCD <= 0) {
			float radius = Player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(300);
			for (int i = 0; i < 75; i++) {
				Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(radius, radius), 0, 0, DustID.Crimson);
				Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(radius, radius), 0, 0, DustID.GemRuby);
			}
			Player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
			foreach (var npc in npclist) {
				Player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)Player.GetDamage(DamageClass.Generic).ApplyTo(30), 1));
				npc.AddBuff(BuffID.Ichor, 300);
				Player.Heal(1);
			}
			ShadewoodArmorCD = BossRushUtils.ToSecond(3);
		}
	}
	private void OnHitNPC_BorealWoodArmor(NPC target) {
		if (!BorealWoodArmor) {
			return;
		}
		if (Main.rand.NextFloat() <= .3f) {
			target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(10));
		}
	}
	private void OnHitNPC_PumpkinArmor(NPC npc, float damage) {
		if (!PumpkinArmor || !Main.rand.NextBool(3)) {
			return;
		}
		if (npc.HasBuff(ModContent.BuffType<pumpkinOverdose>())) {
			int explosionRaduis = 75 + (int)MathHelper.Clamp(damage, 0, 125);
			for (int i = 0; i < 35; i++) {
				Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(explosionRaduis, explosionRaduis), 0, 0, DustID.Pumpkin);
				Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(explosionRaduis, explosionRaduis), 0, 0, DustID.OrangeTorch);
			}
			npc.Center.LookForHostileNPC(out List<NPC> npclist, explosionRaduis);
			foreach (var i in npclist) {
				Player.StrikeNPCDirect(npc, i.CalculateHitInfo(5 + (int)(damage * 0.05f), 1, Main.rand.NextBool(40)));
			}
			SoundEngine.PlaySound(SoundID.NPCDeath46);
			npc.AddBuff(ModContent.BuffType<pumpkinOverdose>(), 240);
		}
		else {
			npc.AddBuff(ModContent.BuffType<pumpkinOverdose>(), 240);
		}
	}
	private void OnHitNPC_AshWoodArmor(NPC npc) {
		if (AshWoodArmor) {
			npc.AddBuff(BuffID.OnFire, 300);
		}
	}
	private void OnHitNPC_PearlWoodArmor(NPC npc) {
		if (pearlWoodArmorCD <= 0 && pearlWoodArmor) {
			int dmg = 12;
			if (Player.ZoneHallow) {
				dmg += 35;
			}
			for (int i = 0; i < 6; i++) {
				Vector2 pos = npc.Center + new Vector2(0, -20).Vector2DistributeEvenly(6, 360, i) * 10;
				Vector2 vel = npc.Center - pos;
				Projectile.NewProjectile(Player.GetSource_OnHit(npc), pos, vel.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<pearlSwordProj>(), dmg, 1, Player.whoAmI);
			}
			pearlWoodArmorCD = 240;
		}
	}
	private void OnHitNPC_CopperArmor() {
		if (!CopperArmor) {
			return;
		}
		CopperArmorChargeCounter++;
		if (Player.ZoneRain)
			CopperArmorChargeCounter++;
		if (CopperArmorChargeCounter >= 50) {
			Player.AddBuff(ModContent.BuffType<OverCharged>(), 300);
			CopperArmorChargeCounter = 0;
		}
	}
	private void OnHitNPC_GoldArmor(NPC npc, float damage) {
		if (GoldArmor)
			if (npc.HasBuff(BuffID.Midas)) {
				int GoldArmorBonusDamage = (int)damage + npc.defense;
				npc.StrikeNPC(npc.CalculateHitInfo(GoldArmorBonusDamage, 1, false, 1, DamageClass.Generic, true, Player.luck));
			}
			else {
				if (Main.rand.NextFloat() < .15f) {
					npc.AddBuff(BuffID.Midas, 600);
				}
			}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (TungstenArmor) {
			float DamageIncrease = (target.Center - Player.Center).Length();
			modifiers.SourceDamage += MathHelper.Clamp(600 - DamageIncrease, 0, 200) * .005f;
		}
	}
	public override void NaturalLifeRegen(ref float regen) {
		regen += NaturalLifeRegen_pumpkinArmor();
	}
	private float NaturalLifeRegen_pumpkinArmor() => Player.statLife <= Player.statLifeMax * .2f ? 5f : 1f;
}
public class ArmorSet {
	public int headID, bodyID, legID;
	protected string ArmorSetBonusToolTip = "";
	public ArmorSet(int headID, int bodyID, int legID) {
		this.headID = headID;
		this.bodyID = bodyID;
		this.legID = legID;
	}
	public static string ConvertIntoArmorSetFormat(int headID, int bodyID, int legID) => $"{headID}:{bodyID}:{legID}";
	/// <summary>
	/// Expect there is only 3 item in a array
	/// </summary>
	/// <param name="armor"></param>
	/// <returns></returns>
	public static string ConvertIntoArmorSetFormat(int[] armor) => $"{armor[0]}:{armor[1]}:{armor[2]}";
	public override string ToString() => $"{headID}:{bodyID}:{legID}";

	public bool ContainAnyOfArmorPiece(int type) => type == headID || type == bodyID || type == legID;
}
