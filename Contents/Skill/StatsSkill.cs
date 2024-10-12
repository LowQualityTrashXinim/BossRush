using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace BossRush.Contents.Skill;

public class Increases_3xDamage : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 530;
		Skill_Duration = 8;
		Skill_CoolDown = BossRushUtils.ToSecond(15);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 4);
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<PowerBankDebuff>(), BossRushUtils.ToMinute(1));
	}
	public class PowerBankDebuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.EnergyRecharge, -.5f);
		}
	}
}
public class PowerSaver : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 560;
		Skill_Duration = 0;
		Skill_CoolDown = BossRushUtils.ToMinute(1);
		Skill_EnergyRequirePercentage = -.5f;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
}
public class FastForward : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 200;
		Skill_Duration = 0;
		Skill_CoolDown = BossRushUtils.ToSecond(20);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration, out StatModifier cooldown) {
		energy = new();
		duration = new();
		cooldown = new();
		duration -= .5f;
		cooldown -= .5f;
	}
}
public class PowerCord : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 100;
		Skill_EnergyRequirePercentage = .25f;
		Skill_Duration = 0;
		Skill_CoolDown = 0;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<PowerCordBuff>(), BossRushUtils.ToSecond(12));
	}
	public class PowerCordBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.EnergyRecharge, 2.5f);
		}
	}
}
public class Procrastination : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 350;
		Skill_Duration = BossRushUtils.ToSecond(4);
		Skill_CoolDown = BossRushUtils.ToSecond(17);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player) {
		player.AddBuff(BuffID.Stoned, 2);
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 2.25f);
		player.endurance += .75f;
	}
}
public class SpeedDemon : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 330;
		Skill_Duration = 30;
		Skill_CoolDown = BossRushUtils.ToSecond(9);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 2.5f);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.5f);
	}
}
public class TranquilMind : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 400;
		Skill_Duration = 5;
		Skill_CoolDown = BossRushUtils.ToSecond(60);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
}
public class InfiniteManaSupply : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 220;
		Skill_Duration = BossRushUtils.ToSecond(.5f);
		Skill_CoolDown = BossRushUtils.ToSecond(6);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player) {
		if (player.statMana < player.statManaMax2) {
			player.statMana += 10;
		}
	}
}
public class GuaranteedCrit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 425;
		Skill_Duration = BossRushUtils.ToSecond(.5f);
		Skill_CoolDown = BossRushUtils.ToSecond(5);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		player.GetModPlayer<PlayerStatsHandle>().ModifyHit_OverrideCrit = true;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		player.GetModPlayer<PlayerStatsHandle>().ModifyHit_OverrideCrit = true;
	}
}
public class RapidHealing : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 345;
		Skill_Duration = BossRushUtils.ToSecond(2);
		Skill_CoolDown = BossRushUtils.ToSecond(30);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 6 != 0) {
			return;
		}
		player.Heal(6);
	}
}
public class AdAstra : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 450;
		Skill_Duration = BossRushUtils.ToSecond(3);
		Skill_CoolDown = BossRushUtils.ToSecond(5);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Additive: 6f);
	}
	public override void OnEnded(Player player) {
		player.AddBuff(ModContent.BuffType<AdAstraDebuff>(), BossRushUtils.ToSecond(15));
	}
	public class AdAstraDebuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Multiplicative: .1f);
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: .1f);
			modplayer.AddStatsToPlayer(PlayerStats.Defense, Multiplicative: .1f);
		}
	}
}
public class BloodToPower : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 570;
		Skill_Duration = BossRushUtils.ToSecond(2);
		Skill_CoolDown = BossRushUtils.ToSecond(9);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnTrigger(Player player) {
		int blood = player.statLife / 2;
		player.statLife -= blood;
		player.GetModPlayer<SkillHandlePlayer>().BloodToPower = blood;
	}
	public override void Update(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 + player.GetModPlayer<SkillHandlePlayer>().BloodToPower * .01f);
	}
}
public class Overclock : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 635;
		Skill_Duration = BossRushUtils.ToSecond(1);
		Skill_CoolDown = BossRushUtils.ToSecond(9);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player) {
		SkillHandlePlayer skillplayer = player.GetModPlayer<SkillHandlePlayer>();
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, 1 + Math.Clamp(.5f * (skillplayer.MaximumDuration - skillplayer.Duration) / 50, 0, 7.5f));
	}
}
public class TerrorForm : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 900;
		Skill_Duration = BossRushUtils.ToSecond(4);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void Update(Player player) {
		for (int i = 0; i < 2; i++) {
			Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, Main.rand.Next(new int[] { DustID.Shadowflame, DustID.Wraith, DustID.UltraBrightTorch }));
			dust.noGravity = true;
			dust.velocity = Vector2.UnitY * -Main.rand.NextFloat(3);
			dust.scale = Main.rand.NextFloat(0.75f, 1.25f);
		}
		player.statLife = Math.Clamp(player.statLife - 1, 1, player.statLifeMax2);
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		float percentage = (1 - player.statLife / (float)player.statLifeMax2);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.5f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Multiplicative: 1 + percentage, Base: 25);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 2f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.5f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.35f, Multiplicative: 1 + percentage);
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.35f, Multiplicative: 1 + percentage);
	}
}
public class AllOrNothing : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequirePercentage = 1;
		Skill_Duration = 1;
		Skill_CoolDown = BossRushUtils.ToMinute(15);
		Skill_CanBeSelect = false;
		Skill_Type = SkillTypeID.Skill_Stats;
	}
	public override void OnTrigger(Player player) {
		player.AddBuff(ModContent.BuffType<AllOrNothingBuff>(), BossRushUtils.ToSecond(5));
	}
	public class AllOrNothingBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			if (player.buffTime[buffIndex] <= 0) {
				player.Center.LookForHostileNPC(out List<NPC> npclist, 1000);
				foreach (NPC npc in npclist) {
					int direction = player.Center.X > npc.Center.X ? 1 : -1;
					int originDmg = (int)(npc.lifeMax * .1f);
					int dmg = originDmg;
					for (int i = 2; i < 17; i++) {
						if (Main.rand.NextBool(i)) {
							dmg += (int)(originDmg * Main.rand.NextFloat(.85f, 1.15f));
						}
					}
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo(dmg, direction));
				}
			}
		}
	}
}
