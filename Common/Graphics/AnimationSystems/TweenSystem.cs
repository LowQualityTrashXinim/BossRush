using BossRush.Contents.WeaponEnchantment;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics.AnimationSystem;
public class TweenSystem<T> : ModSystem where T : struct {

	public static List<Tween<T>> _tweens = new();
	public override void PreUpdateEntities() {
		for (int i = 0; i < _tweens.Count; i++) {
			_tweens[i].Update();

		}
	}
}

public enum TweenEaseType : byte 
{

	None = 0,
	InSine = 1,
	OutSine = 2,
	InExpo = 3,
	OutExpo = 4,

}

public enum TweenState : byte 
{
	Stopped,
	Running,
	Paused


}

public class Tween<T> where T : struct
{
	public int currentDuration = 0;
	public T currentProgress;
	private float currentProgressPercentage = 0;
	public T start;
	public T finish;
	private TweenEaseType easeType;
	public TweenState state;
	private float endDuration = 0;
	public Action onFinsihed;
	public delegate T lerpFunction(T value1, T value2, float amount);
	public lerpFunction lerp;
	public Tween(lerpFunction lerpFunc) 
	{

		lerp = lerpFunc;
		TweenSystem<T>._tweens.Add(this);
	}
	public void Start(T start, T finish, TweenEaseType type, int duration) 
	{
		this.start = start;
		this.finish = finish;
		easeType = type;
		currentDuration = 0;
		endDuration = duration;
		state = TweenState.Running;
		


	}

	public void Pause() => state = TweenState.Paused;
	public void Resume() => state = TweenState.Running;
	public void Update() 
	{
		if (state == TweenState.Paused || state == TweenState.Stopped)
			return;


		if(currentDuration != endDuration)
			currentDuration++;
		switch (easeType) 
		{
			case TweenEaseType.None:
				currentProgressPercentage = currentDuration / endDuration;
				break;
			case TweenEaseType.InSine:
				currentProgressPercentage = BossRushUtils.InSine(currentDuration/endDuration); 
				break;
			case TweenEaseType.OutSine:
				currentProgressPercentage = BossRushUtils.OutSine(currentDuration / endDuration);
				break;
			case TweenEaseType.InExpo:
				currentProgressPercentage = BossRushUtils.InExpo(currentDuration / endDuration);
				break;
			case TweenEaseType.OutExpo:
				currentProgressPercentage = BossRushUtils.OutExpo(currentDuration / endDuration);
				break;
		}
		
		currentProgress = lerp(start, finish, currentProgressPercentage);

		if(currentDuration == endDuration) 
		{
			
			onFinsihed.Invoke();

		}
			

	}
}


