using BossRush.Contents.WeaponEnchantment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics.AnimationSystem;

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

public class Tween<T> : IEnumerable where T : struct
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
	}
	public Tween<T> Start(T start, T finish, TweenEaseType type, int duration) 
	{
		this.start = start;
		this.finish = finish;
		easeType = type;
		currentDuration = 0;
		endDuration = duration;
		state = TweenState.Running;
		return this;


	}
	public Tween<T> Start() {

		currentDuration = 0;
		state = TweenState.Running;
		return this;
	}

	public Tween<T> SetProperties(T start, T finish, TweenEaseType type, int duration) 
	{

		this.start = start;
		this.finish = finish;
		easeType = type;
		currentDuration = 0;
		endDuration = duration;
		state = TweenState.Paused;
		return this;
	}

	public void Pause() => state = TweenState.Paused;
	public void Resume() => state = TweenState.Running;
	public void Update() 
	{
		if (state == TweenState.Paused || state == TweenState.Stopped)
			return;


		if(currentDuration < endDuration)
			currentDuration++;

		switch (easeType) 
		{
			case TweenEaseType.None:
				currentProgressPercentage = currentDuration / endDuration;
				break;
			case TweenEaseType.InSine:
				currentProgressPercentage = BossRushUtils.InSine(currentDuration / endDuration); 
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
			
			onFinsihed?.Invoke();
			state = TweenState.Stopped;

		}
			

	}

	public IEnumerator GetEnumerator() {
		return null;
	}
}

public class TweenHandler<T>  where T : struct {

	public List<Tween<T>> tweens = new List<Tween<T>>();
	public bool justStarted = true;
	public Tween<T> currentTween;
	public void PlayTweens() {
		int i = 0;
		foreach (var t in tweens) {
			i++;

			t.onFinsihed += () => {
				currentTween = tweens.ElementAt(i - 1).Start();
			};

		}

		currentTween = tweens.First().Start();
	}
	public void Pause() => currentTween.Pause();
	public void Resume() => currentTween.Resume();

	public void Update() 
	{

		foreach (var t in tweens) 
		{
		
			t.Update();
		
		}
	
	}


}

