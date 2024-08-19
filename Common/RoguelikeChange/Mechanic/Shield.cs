using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using System;

namespace BossRush.Common.RoguelikeChange.Mechanic;
public class Shield_GlobalItem : GlobalItem {
	public int ShieldPoint = 0;
	public float ShieldRes = 0;
	public override bool InstancePerEntity => true;
	public static bool IsAShield(int type) =>
			type == ItemID.SquireShield ||
			type == ItemID.EoCShield ||
			type == ItemID.CobaltShield ||
			type == ItemID.ObsidianShield ||
			type == ItemID.PaladinsShield ||
			type == ItemID.AnkhShield ||
			type == ItemID.FrozenShield ||
			type == ItemID.HeroShield;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return IsAShield(entity.type);
	}
	public override void SetDefaults(Item entity) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return;
		}
		switch (entity.type) {
			case ItemID.SquireShield:
				ShieldPoint = 125;
				ShieldRes = .35f;
				break;
			case ItemID.CobaltShield:
				ShieldPoint = 175;
				ShieldRes = .4f;
				break;
			case ItemID.ObsidianShield:
				ShieldPoint = 200;
				ShieldRes = .45f;
				break;
			case ItemID.AnkhShield:
				ShieldPoint = 200;
				ShieldRes = .5f;
				break;
			case ItemID.EoCShield:
				ShieldPoint = 225;
				ShieldRes = .5f;
				break;
			case ItemID.PaladinsShield:
				ShieldPoint = 250;
				ShieldRes = .55f;
				break;
			case ItemID.FrozenShield:
				ShieldPoint = 500;
				ShieldRes = .65f;
				break;
			case ItemID.HeroShield:
				ShieldPoint = 500;
				ShieldRes = .65f;
				break;
		}
	}
	public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return;
		}
		Shield_ModPlayer shieldplayer = player.GetModPlayer<Shield_ModPlayer>();
		if (IsAShield(item.type)) {
			shieldplayer.Shield_MaxHealth += ShieldPoint;
			shieldplayer.Shield_ResPoint += ShieldRes;
		}
	}
}
public class Shield_ModPlayer : ModPlayer {
	public int Shield_Health = 0;
	public int Shield_MaxHealth = 0;
	public int Shield_CoolDown = 0;
	public float Shield_ResPoint = 1;
	public bool Shield_IsUp = false;
	public override void PreUpdate() {
		Shield_ResPoint = 1;
	}
	public override void PostUpdate() {
		if (Shield_MaxHealth <= 0 || Shield_Health <= 0 && !Shield_IsUp) {
			return;
		}
		if (Shield_CoolDown <= 0) {
			Shield_IsUp = true;
			Player.AddBuff(ModContent.BuffType<Shield_ModBuff>(), 9999999);
		}
	}
	public override void ResetEffects() {
		if (!Player.HasBuff(ModContent.BuffType<Shield_ModBuff>()) && Shield_IsUp) {
			Shield_IsUp = false;
			Shield_CoolDown = 0;
			Shield_Health = 0;
		}
		if (Shield_CoolDown <= 0 && Shield_Health <= 0) {
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			Shield_Health = (int)modplayer.ShieldHealth.ApplyTo(Shield_MaxHealth);
		}
		Shield_MaxHealth = 0;
		Shield_CoolDown = BossRushUtils.CountDown(Shield_CoolDown);
	}
	public override void UpdateEquips() {
		if (Shield_IsUp) {
			Player.DefenseEffectiveness *= Shield_ResPoint;
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		DamageShield(hurtInfo.Damage);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		DamageShield(hurtInfo.Damage);
	}
	private void DamageShield(int damagevalue) {
		if (Shield_IsUp) {
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			int damageDone = (int)(damagevalue / modplayer.ShieldEffectiveness.ApplyTo(1));
			Shield_Health -= damageDone;
			BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.RoyalBlue, $"{damageDone}", Main.rand.Next(30, 40), 5);
			if (Shield_Health <= 0) {
				Shield_IsUp = false;
				Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<Shield_ModBuff>()));
				Shield_CoolDown = BossRushUtils.ToSecond(10);
			}
		}
	}
}
public class Shield_ModBuff : ModBuff {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = false;
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		string hexColor;
		Player player = Main.LocalPlayer;
		Shield_ModPlayer shieldplayer = player.GetModPlayer<Shield_ModPlayer>();
		int MaxHealth = shieldplayer.Shield_MaxHealth;
		int healthPoint = shieldplayer.Shield_Health;
		if (healthPoint >= MaxHealth * .55f) {
			hexColor = Color.LawnGreen.Hex3();
		}
		else if (healthPoint >= MaxHealth * .2f) {
			hexColor = Color.Yellow.Hex3();
		}
		else {
			hexColor = Color.Red.Hex3();
		}
		tip += $"\nShield remain Health : [c/{hexColor}:{healthPoint}]" +
			$"\nDefenses effectiveness increases : {MathF.Round(shieldplayer.Shield_ResPoint, 2)}";
	}
}
