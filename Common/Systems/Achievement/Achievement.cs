using BossRush.Texture;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.UI;

namespace BossRush.Common.Systems.Achievement;

/// <summary>
/// This should and will be run on client side only, this should never work in multiplayer no matter what
/// </summary>
public abstract class ModAchievement {
	public bool Achieved { get; set; }
	public bool AdditionalConditionTipAfterAchieve = false;
	public virtual string Texture => BossRushTexture.ACCESSORIESSLOT;
	public virtual bool SpecialDraw => false;
	public virtual bool CanSeeReward => true;
	public virtual void Draw(UIElement element, SpriteBatch spriteBatch) { }
	public string Name => GetType().Name;
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.Description");
	public string ConditionTip => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTip");
	public string ConditionTipAfterAchieve => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.ConditionTipAfterAchieve");
	public string Reward => Language.GetTextValue($"Mods.BossRush.Achievement.{Name}.Reward");

	public virtual void SetStaticDefault() { }
	public virtual bool Condition() {
		return false;
	}
	public virtual void SpecialEffectOnAchieved() { }
}
