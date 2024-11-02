using Terraria;

namespace BossRush.Common.Systems.CursesSystem;

public class WeakeningI : ModCurse {
	public override void SetDefault() {
		AddCatagory(CursesCatagory.Taboo);
		Value = 1;
	}
	public override void Update(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, -.05f * GetValue(player));
		modplayer.AddStatsToPlayer(PlayerStats.Defense, -.05f * GetValue(player));
	}
}

public class WeakeningII : ModCurse {
	public override void SetDefault() {
		AddCatagory(CursesCatagory.Taboo);
		Value = 3;
	}
	public override void Update(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, -.1f * GetValue(player));
		modplayer.AddStatsToPlayer(PlayerStats.Defense, -.1f * GetValue(player));
	}
}

public class Prideful : ModCurse {
	public override void SetDefault() {
		AddCatagory(CursesCatagory.Taboo);
		Value = 1;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() <= .55f) {
			modifiers.SourceDamage -= .4f - .1f * GetValue(player);
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() <= .55f) {
			modifiers.SourceDamage -= .4f - .1f * GetValue(player);
		}
	}
}

public class SoulFlame : ModCurse {
	public override void SetDefault() {
		AddCatagory(CursesCatagory.Taboo);
		Value = 2;
	}
	public override void Update(Player player) {
		if (player.ComparePlayerHealthInPercentage(.75f)) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Flat: -10);
		}
	}
}
public class WitheringI : ModCurse {
	public override void SetDefault() {
		AddCatagory(CursesCatagory.Taboo);
		Value = 1;
	}
	public override void Update(Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: -1);
	}
}

public class WitheringII : ModCurse {
	public override void SetDefault() {
		AddCatagory(CursesCatagory.Taboo);
		Value = 3;
	}
	public override void Update(Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: -5);
	}
}

