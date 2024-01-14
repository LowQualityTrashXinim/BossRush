using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using BossRush.Texture;

namespace BossRush.Contents.Skill;
public abstract class ModSkill : ModType {
	int Skill_CoolDown = 0;
	int Skill_Duration = 0;
	int Skill_EnergyRequire = 0;
	public virtual string Texture => BossRushTexture.MISSINGTEXTURE;
	public int CoolDown { get => Skill_CoolDown; }
	public int Duration { get => Skill_Duration; }
	public int EnergyRequire { get => Skill_EnergyRequire; }
	public int Type { get; private set; }
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.ModSkill.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.BossRush.ModSkill.{Name}.Description");
	protected sealed override void Register() {
		Type = SkillLoader.Register(this);
	}
	public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
	public virtual void Update(Player player) { }
	public virtual void OnMissingMana(Player player, Item item, int neededMana) { }
	public virtual void ModifyDamage(Player player, Item item, ref StatModifier damage) { }
	public virtual void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
	public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
	public virtual void OnHitByAnything(Player player) { }
	public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
	public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
	public virtual void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) { }
	/// <summary>
	/// Subtract will make player use weapon slower
	/// Additive will make player use weapon faster
	/// </summary>
	/// <param name="player"></param>
	/// <param name="item"></param>
	/// <param name="useSpeed">by default start at 1</param>
	public virtual void ModifyUseSpeed(Player player, Item item, ref float useSpeed) { }
}
public static class SkillLoader {
	private static readonly List<ModSkill> _skill = new();
	public static int TotalCount => _skill.Count;
	public static int Register(ModSkill enchant) {
		ModTypeLookup<ModSkill>.Register(enchant);
		_skill.Add(enchant);
		return _skill.Count - 1;
	}
	public static ModSkill GetSkill(int type) {
		return type >= 0 && type < _skill.Count ? _skill[type] : null;
	}
}
public class SkillModSystem : ModSystem {
	public static int SelectInventoryIndex = -1;
	public static int SelectSkillIndex = -1;
	public static ModKeybind SkillActivation { get; private set; }

	public override void Load() {
		SkillActivation = KeybindLoader.RegisterKeybind(Mod, "Skill activation", Keys.F);
	}
	public override void Unload() {
		SkillActivation = null;
	}
}
public class SkillHandlePlayer : ModPlayer {
	public int EnergyCap = 500;
	public int Energy = 0;
	public int EnergyRechargeCap = 0;
	public int Duration = 0;
	public int[] SkillHolder1 = new int[10];
	public int[] SkillHolder2 = new int[10];
	public int[] SkillHolder3 = new int[10];
	public int[] SkillInventory = new int[30];
	public bool Activate = false;
	int CurrentActiveHolder = 1;

	public int ActivateHolderIndex { get => CurrentActiveHolder; }
	public override void Initialize() {
		Array.Fill(SkillHolder1, -1);
		Array.Fill(SkillHolder2, -1);
		Array.Fill(SkillHolder3, -1);
	}
	public void AddSkillIntoCurrentActiveHolder(int SkillID, int whoAmI) {
		if (whoAmI < 0 || whoAmI > 9) {
			return;
		}
		switch (CurrentActiveHolder) {
			case 1:
				SkillHolder1[whoAmI] = SkillID;
				break;
			case 2:
				SkillHolder2[whoAmI] = SkillID;
				break;
			case 3:
				SkillHolder3[whoAmI] = SkillID;
				break;
		}
	}
	public void ReplaceSkillFromInvToSkillHolder(int whoAmIskill, int whoAmIInv) {
		if (whoAmIskill < 0 || whoAmIskill > 9) {
			return;
		}
		if (whoAmIInv < 0 || whoAmIInv > 30) {
			return;
		}
		int cache;
		switch (CurrentActiveHolder) {
			case 1:
				cache = SkillHolder1[whoAmIskill];
				SkillHolder1[whoAmIskill] = SkillInventory[whoAmIInv];
				SkillInventory[whoAmIInv] = cache;
				break;
			case 2:
				cache = SkillHolder2[whoAmIskill];
				SkillHolder2[whoAmIskill] = SkillInventory[whoAmIInv];
				SkillInventory[whoAmIInv] = cache;
				break;
			case 3:
				cache = SkillHolder3[whoAmIskill];
				SkillHolder3[whoAmIskill] = SkillInventory[whoAmIInv];
				SkillInventory[whoAmIInv] = cache;
				break;
		}
	}
	public void ReplaceSkillFromSkillHolderToInv(int whoAmIskill, int whoAmIInv) {
		if (whoAmIskill < 0 || whoAmIskill > 9) {
			return;
		}
		if (whoAmIInv < 0 || whoAmIInv > 30) {
			return;
		}
		int cache;
		switch (CurrentActiveHolder) {
			case 1:
				cache = SkillInventory[whoAmIInv];
				SkillInventory[whoAmIInv] = SkillHolder1[whoAmIskill];
				SkillHolder1[whoAmIskill] = cache;
				break;
			case 2:
				cache = SkillInventory[whoAmIInv];
				SkillInventory[whoAmIInv] = SkillHolder2[whoAmIskill];
				SkillHolder2[whoAmIskill] = cache;
				break;
			case 3:
				cache = SkillInventory[whoAmIInv];
				SkillInventory[whoAmIInv] = SkillHolder3[whoAmIskill];
				SkillHolder3[whoAmIskill] = cache;
				break;
		}
	}
	public int[] GetCurrentActiveSkillHolder() {
		CurrentActiveHolder = Math.Clamp(CurrentActiveHolder, 1, 3);
		switch (CurrentActiveHolder) {
			case 1:
				return SkillHolder1;
			case 2:
				return SkillHolder2;
			case 3:
				return SkillHolder3;
		}
		//return null in case where somehow a very catastrophic event ever happen
		return null;
	}
	public override void ProcessTriggers(TriggersSet triggersSet) {
		if (SkillModSystem.SkillActivation.JustReleased) {
			Activate = true;
			int energyCost = 0;
			switch (CurrentActiveHolder) {
				case 1:
					for (int i = 0; i < 10; i++) {
						if (SkillHolder1[i] == -1) {
							continue;
						}
						energyCost += SkillLoader.GetSkill(SkillHolder1[i]).EnergyRequire;
						Duration += SkillLoader.GetSkill(SkillHolder1[i]).Duration;
					}
					break;
				case 2:
					for (int i = 0; i < 10; i++) {
						if (SkillHolder1[i] == -1) {
							continue;
						}
						energyCost += SkillLoader.GetSkill(SkillHolder2[i]).EnergyRequire;
						Duration += SkillLoader.GetSkill(SkillHolder2[i]).Duration;
					}
					break;
				case 3:
					for (int i = 0; i < 10; i++) {
						if (SkillHolder1[i] == -1) {
							continue;
						}
						energyCost += SkillLoader.GetSkill(SkillHolder3[i]).EnergyRequire;
						Duration += SkillLoader.GetSkill(SkillHolder3[i]).Duration;
					}
					break;
			}
			if (energyCost > Energy) {
				Duration = 0;
				return;
			}
			Energy -= energyCost;
		}
	}
	public override void PreUpdate() {
		if (Duration <= 0) {
			Activate = false;
		}
		else {
			Duration = BossRushUtils.CountDown(Duration);
		}
		CurrentActiveHolder = Math.Clamp(CurrentActiveHolder, 1, 3);
	}
	public override void PostUpdate() {
		if (!Activate) {
			return;
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).Update(Player);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).Update(Player);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					SkillLoader.GetSkill(SkillHolder3[i]).Update(Player);
				}
				break;
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!Activate) {
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).Shoot(Player, item, source, position, velocity, type, damage, knockback);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).Shoot(Player, item, source, position, velocity, type, damage, knockback);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder3[i]).Shoot(Player, item, source, position, velocity, type, damage, knockback);
				}
				break;
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnMissingMana(Item item, int neededMana) {
		if (!Activate) {
			return;
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).OnMissingMana(Player, item, neededMana);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).OnMissingMana(Player, item, neededMana);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder3[i]).OnMissingMana(Player, item, neededMana);
				}
				break;
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (!Activate) {
			return;
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).ModifyDamage(Player, item, ref damage);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).ModifyDamage(Player, item, ref damage);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder3[i]).ModifyDamage(Player, item, ref damage);
				}
				break;
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (!Activate) {
			return;
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).ModifyHitNPCWithItem(Player, item, target, ref modifiers);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).ModifyHitNPCWithItem(Player, item, target, ref modifiers);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder3[i]).ModifyHitNPCWithItem(Player, item, target, ref modifiers);
				}
				break;
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (!Activate) {
			return;
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder3[i]).ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
				}
				break;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Activate) {
			return;
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).OnHitNPCWithItem(Player, item, target, hit, damageDone);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).OnHitNPCWithItem(Player, item, target, hit, damageDone);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder3[i]).OnHitNPCWithItem(Player, item, target, hit, damageDone);
				}
				break;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Activate) {
			return;
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).OnHitNPCWithProj(Player, proj, target, hit, damageDone);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).OnHitNPCWithProj(Player, proj, target, hit, damageDone);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder3[i]).OnHitNPCWithProj(Player, proj, target, hit, damageDone);
				}
				break;
		}
	}
	public override float UseSpeedMultiplier(Item item) {
		float useSpeed = 1;
		if (!Activate) {
			return useSpeed;
		}
		switch (CurrentActiveHolder) {
			case 1:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder1[i]).ModifyUseSpeed(Player, item, ref useSpeed);
				}
				break;
			case 2:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder2[i]).ModifyUseSpeed(Player, item, ref useSpeed);
				}
				break;
			case 3:
				for (int i = 0; i < 10; i++) {
					if (SkillHolder1[i] == -1) {
						continue;
					}
					SkillLoader.GetSkill(SkillHolder3[i]).ModifyUseSpeed(Player, item, ref useSpeed);
				}
				break;
		}
		return useSpeed;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Activate) {
			Energy = Math.Clamp(Math.Clamp(damageDone + Energy, 0, EnergyRechargeCap), 0, EnergyCap);
		}
	}
	public override void UpdateDead() {
		Activate = false;
		Energy = 0;
		Duration = 0;
	}
	public override void SaveData(TagCompound tag) {
		tag.Add("SkillHolder1", SkillHolder1);
		tag.Add("SkillHolder2", SkillHolder2);
		tag.Add("SkillHolder3", SkillHolder3);
		tag.Add("SkillInventory", SkillInventory);
	}
	public override void LoadData(TagCompound tag) {
		if (tag.TryGet("SkillHolder1", out int[] SkillHolder1)) {
			Array.Copy(this.SkillHolder1, SkillHolder1, SkillHolder1.Length);
		}
		if (tag.TryGet("SkillHolder2", out int[] SkillHolder2)) {
			Array.Copy(this.SkillHolder2, SkillHolder2, SkillHolder2.Length);
		}
		if (tag.TryGet("SkillHolder3", out int[] SkillHolder3)) {
			Array.Copy(this.SkillHolder3, SkillHolder3, SkillHolder3.Length);
		}
		if (tag.TryGet("SkillInventory", out int[] SkillInventory)) {
			Array.Copy(this.SkillInventory, SkillInventory, SkillInventory.Length);
		}
	}
}
public class SkillOrb : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.width = Item.height = 30;
	}
}
