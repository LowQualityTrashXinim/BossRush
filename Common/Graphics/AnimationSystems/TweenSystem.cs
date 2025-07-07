using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Graphics.AnimationSystems;

public enum TweenEaseType : byte {

	None = 0,
	InSine = 1,
	OutSine = 2,
	InExpo = 3,
	OutExpo = 4,

}

public enum TweenState : byte {
	Stopped,
	Running,
	Paused


}
/// <summary>
/// A Tween, when started, use its currentProgress field to get the running value, Also must be updated manually using the Update method, at least for now untill i figure out the best way to update it automatically
/// </summary>
/// <typeparam name="T"></typeparam>
public class Tween<T> where T : struct {
	public int currentDuration = 0;
	public T currentProgress;
	private float currentProgressPercentage = 0;
	public T start;
	public T finish;
	private TweenEaseType easeType;
	public TweenState state;
	private float endDuration = 0;
	public Action<Tween<T>> onFinsihed;
	public delegate T lerpFunction(T value1, T value2, float amount);
	public lerpFunction lerp;
	public bool pingpongEnabled = false;
	public Tween(lerpFunction lerpFunc, bool pingpong = false) {

		lerp = lerpFunc;
		pingpongEnabled = pingpong;
	}
	public Tween<T> Start(T start, T finish, TweenEaseType type, int duration) {
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

	public Tween<T> SetProperties(T start, T finish, TweenEaseType type, int duration) {

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
	public void Update() {
		if (state == TweenState.Paused || state == TweenState.Stopped)
			return;


		if (currentDuration < endDuration)
			currentDuration++;

		switch (easeType) {
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
				currentProgressPercentage = BossRushUtils.InExpo(currentDuration / endDuration,11f);
				break;
			case TweenEaseType.OutExpo:
				currentProgressPercentage = BossRushUtils.OutExpo(currentDuration / endDuration,11f);
				break;
		}
		if (pingpongEnabled)
			Terraria.Utils.PingPongFrom01To010(currentProgressPercentage);
		currentProgress = lerp(start, finish, currentProgressPercentage);
		if (currentDuration == endDuration) {
			onFinsihed?.Invoke(this);
			state = TweenState.Stopped;
		}
	}
}

/// <summary>
/// Holds A sequance of Tweens
/// </summary>
/// <typeparam name="T"></typeparam>
public class TweenHandler<T> where T : struct {

	public List<Tween<T>> tweens = new List<Tween<T>>();
	public Tween<T> currentTween;
	public void PlayTweens() {
	
		foreach(var t in tweens)
		{
			t.onFinsihed += LinkTween;
		}

		currentTween = tweens[0].Start();
	}

	public void LinkTween(Tween<T> finishedTween)
	{	
		int nextIndex = tweens.IndexOf(finishedTween) + 1;
		if(nextIndex < tweens.Count)
			currentTween = tweens[nextIndex].Start();
	}

	public void Pause() => currentTween.Pause();
	public void Resume() => currentTween.Resume();

	public void Update() {
		foreach (var t in tweens) {
			t.Update();
		}
	}
}
