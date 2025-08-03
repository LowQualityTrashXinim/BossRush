using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.RelicItem;
public class RelicPrefixSystem : ModSystem {
	public static readonly List<RelicPrefix> relic_prefixes = new List<RelicPrefix>();
	public static int TotalCount => relic_prefixes.Count;
	public static RelicPrefix GetRelicPrefix(int type) {
		return type >= 0 && type < relic_prefixes.Count ? relic_prefixes[type] : null;
	}
	public static short Register(RelicPrefix template) {
		ModTypeLookup<RelicPrefix>.Register(template);
		relic_prefixes.Add(template);
		return (short)(relic_prefixes.Count - 1);
	}
	public override void Load() {
		base.Load();
	}
	public override void Unload() {
		relic_prefixes.Clear();
	}
}
public abstract class RelicPrefix : ModType {
	public short Type;
	public string Description => DisplayName + " : " + Language.GetTextValue($"Mods.BossRush.RelicPrefix.{Name}.Description");
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.RelicPrefix.{Name}.DisplayName");
	public string TextureString = "";
	public static short GetRelicType<T>() where T : RelicPrefix {
		return ModContent.GetInstance<T>().Type;
	}
	protected override void Register() {
		SetStaticDefaults();
		Type = RelicPrefixSystem.Register(this);
	}
	/// <summary>
	/// This act as a StatModifier for <see cref="Relic"/>, but you can also treat it as update function
	/// </summary>
	/// <param name="player"></param>
	/// <param name="relic"></param>
	/// <param name="value"></param>
	/// <param name="TemplateType"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public virtual StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) { return value; }
	public virtual void Update(Player player, Relic relic, int index) { }
}

public class Hearty : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("HeartyRelic");
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (index != 0) {
			return value;
		}
		float percentageSet = player.statLife / (player.statLifeMax2 * .75f);
		percentageSet = Math.Clamp(percentageSet, .8f, 1.2f);
		return value * percentageSet;
	}
}
public class Arcane : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("ArcaneRelic");
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (index != 0) {
			return value;
		}
		float percentageSet = player.statMana / (player.statMana * .55f);
		percentageSet = Math.Clamp(percentageSet, .8f, 1.2f);
		return value * percentageSet;
	}
}
public class Alpha : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("AlphaRelic");
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (index != 0) {
			return value;
		}
		return value * 1.5f;
	}
}
public class Zeta : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("ZetaRelic");
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (relic.RelicTier == 1) {
			return value;
		}
		if (index != relic.RelicTier - 1) {
			return value;
		}
		return value * 1.5f;
	}
}
public class Omega : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("OmegaRelic");
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (RelicTemplateLoader.GetTemplate(TemplateType).relicType == RelicType.Projectile) {
			return value + .2f;
		}
		return value;
	}
}
public class Phi : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("PhiRelic");
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (RelicTemplateLoader.GetTemplate(TemplateType).relicType == RelicType.Stat) {
			return value + .2f;
		}
		return value;
	}
}
public class Archer : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("ArcherRelic");
	}
	public override void Update(Player player, Relic relic, int index) {
		player.arrowDamage += .06f;
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (player.HeldItem.useAmmo == AmmoID.Arrow && index == 0) {
			value.Base += 5;
			return value;
		}
		return value;
	}
}
public class Defense : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("DefenseRelic");
	}
	public override void Update(Player player, Relic relic, int index) {
		player.ModPlayerStats().UpdateDefenseBase.Base += 5;
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (player.statDefense >= 50 && index == 0) {
			value.Base += 5;
			return value;
		}
		return value;
	}
}
public class Melee : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("MeleeRelic");
	}
	public override void Update(Player player, Relic relic, int index) {
		player.ModPlayerStats().DirectItemDamage.Base += 10;
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (player.HeldItem.DamageType == DamageClass.Melee && index == 0) {
			return value + .1f;
		}
		return value;
	}
}
public class Staff : RelicPrefix {
	public override void SetStaticDefaults() {
		TextureString = BossRushUtils.GetTheSameTextureAs<Relic>("StaffRelic");
	}
	public override void Update(Player player, Relic relic, int index) {
		player.manaCost -= .05f;
	}
	public override StatModifier StatsModifier(Player player, Relic relic, StatModifier value, int TemplateType, int index) {
		if (Item.staff[player.HeldItem.type] && index == 0) {
			return value + .2f;
		}

		return value;
	}
}
