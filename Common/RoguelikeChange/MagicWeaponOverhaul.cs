using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange;
class MagicOverhaulPlayer : ModPlayer {
	int TimeSinceActive = 0;
	public override void PostUpdate() {
		if (Player.ItemAnimationJustStarted) {
			TimeSinceActive = Math.Clamp(TimeSinceActive + 1, 0, 50);
		}
		if (!Player.ItemAnimationActive) {
			TimeSinceActive = 0;
		}
	}
	public override bool CanUseItem(Item item) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return base.CanUseItem(item);
		}
		if (item.buffType == BuffID.ManaSickness && Player.HasBuff(ModContent.BuffType<ManaBlock>())) {
			return false;
		}
		return base.CanUseItem(item);
	}
	public override float UseSpeedMultiplier(Item item) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul || item.mana == 0) {
			return base.UseSpeedMultiplier(item);
		}
		int manacost = Player.GetManaCost(item);
		float speed = base.UseSpeedMultiplier(item);
		float multiplier = speed - (manacost - item.mana) / (float)item.mana;
		if (multiplier <= 1) {
			return base.UseSpeedMultiplier(item);
		}
		return multiplier;
	}
	public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return;
		}
		mult += TimeSinceActive * .01f;
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return;
		}
		int manacost = Player.GetManaCost(item);
		if (item.mana != 0) {
			float multiplier = (manacost - item.mana) / (float)item.mana;
			damage *= multiplier + Player.manaCost;
		}
		else {
			damage *= Player.manaCost;
		}
	}
}
class MagicOverhaulSystem : ModSystem {
	public override void Load() {
		base.Load();
		On_Player.QuickMana += On_Player_QuickMana;
	}

	private void On_Player_QuickMana(On_Player.orig_QuickMana orig, Player self) {
		if (self.HasBuff(ModContent.BuffType<ManaBlock>()) && ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return;
		}
		orig(self);
	}
}
class MagicOverhaulBuff : GlobalBuff {
	public override void Update(int type, Player player, ref int buffIndex) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return;
		}
		if (type == BuffID.ManaSickness) {
			if (player.statMana < player.statManaMax2) {
				player.statMana++;
			}
			if (player.buffTime[buffIndex] <= 0) {
				player.AddBuff(ModContent.BuffType<ManaBlock>(), BossRushUtils.ToSecond(30));
			}
		}
	}
}
class ManaBlock : ModBuff {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
	public override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
	}
}
