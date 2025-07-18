﻿using BossRush.Contents.Perks;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.ObjectSystem;

/// <summary>
/// This is a object system where it handle player created object<br/>
/// While we can use Projectile, NPC to handle some event, because of certain system and mechanic within the mod<br/>
/// It can be broken or have unintended consequence, as such Object system is created to take care of that<br/>
/// <br/>
/// For more detail on what Object class can do, check the actual object class
/// </summary>
public class ObjectSystem : ModSystem {
	public const int MaxObjects = 1000;
	public static ModObject[] Objects = new ModObject[MaxObjects];
	private static readonly List<ModObject> ModObjectSample = new();
	public static int TotalCount => ModObjectSample.Count;
	public static ModObject GetModObject(int type) {
		return type >= 0 && type < ModObjectSample.Count ? ModObjectSample[type].Clone() : null;
	}
	public static int Register(ModObject obj) {
		ModTypeLookup<ModObject>.Register(obj);
		ModObjectSample.Add(obj);
		return ModObjectSample.Count - 1;
	}
	public override void Load() {
		Objects = new ModObject[MaxObjects];
	}
	public override void Unload() {
		for (int i = 0; i < ModObjectSample.Count; i++) {
			ModObjectSample[i].Unload();
		}
		Objects = null;
		ModObjectSample.Clear();
	}
	public override void PostSetupContent() {
		for (int i = 0; i < ModObjectSample.Count; i++) {
			ModObjectSample[i].Load();
		}
	}
	public static bool AnyModObjects(int type) {
		for (int i = 0; i < Objects.Length; i++) {
			ModObject modobject = Objects[i];
			if (modobject == null) {
				continue;
			}
			if (modobject.Type == type && modobject.active) {
				return true;
			}
		}
		return false;
	}
	public override void PostUpdateWorld() {
		for (int i = 0; i < Objects.Length; i++) {
			ModObject modobject = Objects[i];
			if (modobject == null) {
				continue;
			}
			if (modobject.active) {
				modobject.AI();
				modobject.position += modobject.velocity;
				if (modobject.timeLeft > 0) {
					modobject.timeLeft--;
				}
				else {
					modobject.OnKill();
					modobject.active = false;
					continue;
				}
			}
		}
	}
	public override void PostDrawTiles() {
		Main.spriteBatch.Begin();
		for (int i = 0; i < Objects.Length; i++) {
			ModObject modobject = Objects[i];
			if (modobject == null) {
				continue;
			}
			if (modobject.active) {
				modobject.Draw(Main.spriteBatch);
			}
		}
		Main.spriteBatch.End();
	}
	public override void OnWorldUnload() {
		for (int i = 0; i < Objects.Length; i++) {
			ModObject modobject = Objects[i];
			if (modobject == null) {
				continue;
			}
			modobject.Kill();
		}
	}
}
/// <summary>
/// This system is completely client side so using <see cref="Main.LocalPlayer"/> is completely valid<br/>
/// If you however want to check for collision with Projectile or NPC, do a manual radious check using <see cref="Main.ActiveProjectiles"/>, <see cref="Main.ActiveNPCs"/><br/>
/// This system is not to be confused with particle system, as this system is not uses to be as such
/// </summary>
public class ModObject : Entity, IModType, ILoadable {
	public ModObject() {
		whoAmI = -1;
		timeLeft = 3600;
		rotation = 0;
		velocity = Vector2.Zero;
		position = Vector2.Zero;
	}
	/// <summary>
	/// Existing time of a object, default at 60s<br/>
	/// Note : the <see cref="AI"/> will still run at timeleft hitting 0 for 1 tick
	/// </summary>
	public int timeLeft = 3600;
	/// <summary>
	/// Type of the Mod Object
	/// </summary>
	public int Type = -1;
	public float rotation = 0;
	public Mod Mod { get; internal set; }
	public string Name => GetType().Name;
	public string FullName => $"{Mod?.Name ?? "Terraria"}/{Name}";
	public static ModObject NewModObject(Vector2 position, Vector2 velocity, int type) {
		int whoAmI = -1;
		for (int i = 0; i < ObjectSystem.MaxObjects; i++) {
			if (ObjectSystem.Objects[i] == null || !ObjectSystem.Objects[i].active) {
				whoAmI = i;
				break;
			}
		}
		if (whoAmI == ObjectSystem.MaxObjects || whoAmI == -1) {
			whoAmI = 0;
		}
		ObjectSystem.Objects[whoAmI] = ObjectSystem.GetModObject(type);
		ModObject obj = ObjectSystem.Objects[whoAmI];
		obj.SetDefaults();
		obj.active = true;
		obj.position = position;
		obj.velocity = velocity;
		obj.whoAmI = whoAmI;
		return obj;
	}
	public static int GetModObjectType<T>() where T : ModObject {
		return ModContent.GetInstance<T>().Type;
	}
	void ILoadable.Load(Mod mod) {
		this.Mod = mod;
		Register();
	}
	protected void Register() {
		Type = ObjectSystem.Register(this);
	}
	public virtual void SetDefaults() {

	}
	/// <summary>
	/// Kill the object from active
	/// </summary>
	public void Kill() {
		OnKill();
		active = false;
		timeLeft = 0;
	}
	/// <summary>
	/// The AI of the object, can be used to spawning NPC, projectile etc
	/// </summary>
	public virtual void AI() {

	}
	/// <summary>
	/// When the object is on its last tick or during the AI call <see cref="Kill"/>
	/// </summary>
	public virtual void OnKill() {

	}
	/// <summary>
	/// This is to draw the object in the game world if needed
	/// </summary>
	public virtual void Draw(SpriteBatch spritebatch) {

	}

	public virtual void Load() {
	}

	public void Unload() {
	}
	public ModObject Clone() {
		ModObject clone = (ModObject)MemberwiseClone();
		clone.whoAmI = -1;
		clone.timeLeft = 3600;
		clone.rotation = 0;
		clone.velocity = Vector2.Zero;
		clone.position = Vector2.Zero;
		return clone;
	}
}
