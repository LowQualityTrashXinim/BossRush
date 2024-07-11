using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Common.Systems.Mutation;
internal class NPCMutation : GlobalNPC {
	public List<ModMutation> mutationList = new List<ModMutation>();
	public override bool InstancePerEntity => true;
	public float MeleeDamageReduction = 1;
	public float RangeDamageReduction = 1;
	public float MagicDamageReduction = 1;
	public float SummonDamageReduction = 1;
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		if (entity.friendly) {
			return false;
		}
		return base.AppliesToEntity(entity, lateInstantiation);
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnSpawn(npc, source);
			}
		}
	}
	public override void SetDefaults(NPC entity) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.SetDefaults(entity);
			}
		}
		if (Main.masterMode) {
			return;
		}
		float DamageReductionPoint = 0;
		if (Main.hardMode) {
			DamageReductionPoint += .25f;
		}
		if (NPC.downedMoonlord) {
			DamageReductionPoint += .5f;
		}
		if (ModContent.GetInstance<BossRushModConfig>().Nightmare) {
			DamageReductionPoint++;
		}
		if (DamageReductionPoint >= 4) {
			MeleeDamageReduction = 0;
			RangeDamageReduction = 0;
			MagicDamageReduction = 0;
			SummonDamageReduction = 0;
		}
		while (DamageReductionPoint > 0) {
			if (Main.rand.NextBool() && MeleeDamageReduction >= 0) {
				if (MeleeDamageReduction <= 0) {
					MeleeDamageReduction = 0;
					continue;
				}
				MeleeDamageReduction = DRDistribution(MeleeDamageReduction, ref DamageReductionPoint);
			}
			else if (Main.rand.NextBool() && RangeDamageReduction >= 0) {
				if (RangeDamageReduction <= 0) {
					RangeDamageReduction = 0;
					continue;
				}
				RangeDamageReduction = DRDistribution(RangeDamageReduction, ref DamageReductionPoint);
			}
			else if (Main.rand.NextBool() && MagicDamageReduction >= 0) {
				if (MagicDamageReduction <= 0) {
					MagicDamageReduction = 0;
					continue;
				}
				MagicDamageReduction = DRDistribution(MagicDamageReduction, ref DamageReductionPoint);
			}
			else if (Main.rand.NextBool() && SummonDamageReduction >= 0) {
				if (SummonDamageReduction <= 0) {
					SummonDamageReduction = 0;
					continue;
				}
				SummonDamageReduction = DRDistribution(SummonDamageReduction, ref DamageReductionPoint);
			}
		}
	}
	private float DRDistribution(float reducePointType, ref float DRpoint) {
		float reducePoint = MathF.Round(Main.rand.NextFloat(1), 2);
		if (reducePoint > DRpoint) {
			if (reducePointType <= DRpoint) {
				reducePointType = 0;
				DRpoint = 0;
				return reducePointType;
			}
			reducePointType -= DRpoint;
			reducePointType = MathF.Round(reducePointType, 2);
			DRpoint = 0;
		}
		else {
			reducePointType -= reducePoint;
			reducePointType = MathF.Round(reducePointType, 2);
			DRpoint -= reducePoint;
		}
		return reducePointType;
	}
	public override void PostAI(NPC npc) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.PostAI(npc);
			}
		}
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnHitPlayer(npc, target, hurtInfo);
			}
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnHitByItem(npc, player, item, hit, damageDone);
			}
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnHitByProjectile(npc, projectile, hit, damageDone);
			}
		}
	}
	public override void OnKill(NPC npc) {
		if (mutationList != null) {
			foreach (var mutation in mutationList) {
				mutation.OnKill(npc);
			}
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (!Main.masterMode) {
			return;
		}
		if (projectile.DamageType == DamageClass.Melee) {
			modifiers.FinalDamage *= MeleeDamageReduction;
		}
		else if (projectile.DamageType == DamageClass.Ranged) {
			modifiers.FinalDamage *= RangeDamageReduction;
		}
		else if (projectile.DamageType == DamageClass.Magic) {
			modifiers.FinalDamage *= MagicDamageReduction;
		}
		else if (projectile.DamageType == DamageClass.Summon) {
			modifiers.FinalDamage *= SummonDamageReduction;
		}
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (!Main.masterMode) {
			return;
		}
		if (item.DamageType == DamageClass.Melee) {
			modifiers.FinalDamage *= MeleeDamageReduction;
		}
		else if (item.DamageType == DamageClass.Ranged) {
			modifiers.FinalDamage *= RangeDamageReduction;
		}
		else if (item.DamageType == DamageClass.Magic) {
			modifiers.FinalDamage *= MagicDamageReduction;
		}
		else if (item.DamageType == DamageClass.Summon) {
			modifiers.FinalDamage *= SummonDamageReduction;
		}
	}
}
