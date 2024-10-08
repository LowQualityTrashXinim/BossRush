using BossRush.Texture;
using Terraria.Localization;

namespace BossRush.Common.Systems.Achievement;

/// <summary>
/// This should and will be run on client side only, this should never work in multiplayer no matter what
/// </summary>
public abstract class Achievement {
	public bool Achieved { get; set; }
	public string Texture => BossRushTexture.MissingTexture_Default;
	public string Name => GetType().Name;
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.Description");
	public string ConditionTip => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTip");
	public string ConditionTipAfterAchieve => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTipAfterAchieve");

	protected virtual void SetDefault() { }
	public virtual bool Condition() {
		return false;
	}
}
