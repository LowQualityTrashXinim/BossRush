using BossRush.Common.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Mode.DreamLikeWorldMode;
internal class ChaosModeNPC : GlobalNPC{
	public override bool InstancePerEntity => true;
	public float MeleeDamageReduction = 1;
	public float RangeDamageReduction = 1;
	public float MagicDamageReduction = 1;
	public float SummonDamageReduction = 1;
	public override void SetDefaults(NPC entity) {
		float DamageReductionPoint = .5f;
		if (Main.hardMode) {
			DamageReductionPoint += .25f;
		}
		if (NPC.downedMoonlord) {
			DamageReductionPoint += .5f;
		}
		if (ModContent.GetInstance<RogueLikeConfig>().Nightmare) {
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
		return Math.Clamp(reducePointType,.1f,.9f);
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
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
