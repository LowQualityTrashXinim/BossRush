using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Common.Global;

namespace BossRush.Common.RoguelikeChange.Prefixes;

public abstract class BaseAccPrefix : ModPrefix {
	public override PrefixCategory Category => PrefixCategory.Accessory;
	public virtual float PowerLevel => 1;
	public sealed override bool CanRoll(Item item) {
		return item.accessory;
	}
	public sealed override bool AllStatChangesHaveEffectOn(Item item) {
		return base.AllStatChangesHaveEffectOn(item);
	}
	public sealed override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {
		base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
	}
	public override void ModifyValue(ref float valueMult) {
		valueMult *= 1f + 0.05f * PowerLevel;
	}

}
public class Evasive : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 4;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.DodgeChance += PowerLevel * .01f;
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+4% dodge chance") {
			IsModifier = true,
		};
	}
}
public class Vital : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 2;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MaxHP, Base: PowerLevel * 10);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+20 maximum health") {
			IsModifier = true,
		};
	}
}

public class Cunning : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 2;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1 + PowerLevel * .07f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+14% critical damage") {
			IsModifier = true,
		};
	}
}

public class Stealthy : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 2;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.FullHPDamage,1 + PowerLevel * .14f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+28% First strike damage") {
			IsModifier = true,
		};
	}
}
public class Spiky : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, Base: PowerLevel * .06f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+6% thorn damage") {
			IsModifier = true,
		};
	}
}
public class Vampiric : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 2;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.LifeSteal += .01f * PowerLevel;
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+2% life steal") {
			IsModifier = true,
		};
	}
}

public class Energetic : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 2;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.EnergyCap, Base: PowerLevel * 25);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+50 maximum energy") {
			IsModifier = true,
		};
	}
}

public class Alchemic : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 2;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.DebuffDamage, 1 + PowerLevel * .05f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+10% debuff damage") {
			IsModifier = true,
		};
	}
}

public class Jumpy : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 4;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1 + PowerLevel * .01f);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+4% jump boost") {
			IsModifier = true,
		};
	}
}
public class Holy : BaseAccPrefix {
	public override float PowerLevel => base.PowerLevel * 2;
	public override void ApplyAccessoryEffects(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Iframe, Base: 5 + PowerLevel);
	}
	public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
		yield return new TooltipLine(Mod, $"Tooltip_{Name}", "+10 invincibility frame") {
			IsModifier = true,
		};
	}
}
