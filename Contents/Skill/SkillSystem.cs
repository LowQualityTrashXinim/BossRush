using System;
using Terraria;
using System.IO;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace BossRush.Contents.Skill;
public abstract class ModSkill : ModType {
	public static int GetSkillType<T>() where T : ModSkill {
		return ModContent.GetInstance<T>().Type;
	}
	/// <summary>
	/// This is also handle itself so no worry
	/// </summary>
	protected int Skill_CoolDown = 0;
	/// <summary>
	/// This is handle automatically so no need to worry about doing it yourself
	/// </summary>
	protected int Skill_Duration = 0;
	protected int Skill_EnergyRequire = 0;
	protected float Skill_EnergyRequirePercentage = 0;
	protected bool Skill_CanBeSelect = true;
	public virtual string Texture => BossRushTexture.MissingTexture_Default;
	public int CoolDown { get => Skill_CoolDown; }
	public int Duration { get => Skill_Duration; }
	public int EnergyRequire { get => Skill_EnergyRequire; }
	public float EnergyRequirePercentage { get => Skill_EnergyRequirePercentage; }
	public bool CanBeSelect { get => Skill_CanBeSelect; }
	public int Type { get; private set; }
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.ModSkill.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.BossRush.ModSkill.{Name}.Description");
	protected sealed override void Register() {
		Type = SkillLoader.Register(this);
		SetDefault();
	}
	public virtual void ModifyNextSkillStats(out StatModifier energy, out StatModifier duration, out StatModifier cooldown) {
		energy = new();
		duration = new();
		cooldown = new();
	}
	public virtual void SetDefault() { }
	/// <summary>
	/// This method always run even during on hit, but it is best to not uses it if you aren't shifting back skill
	/// </summary>
	/// <param name="index"></param>
	/// <param name="player"></param>
	public virtual void OnCalled(Player player, SkillHandlePlayer skillplayer, ref int index) { }
	public virtual void OnTrigger(Player player) { }
	public virtual void OnEnded(Player player) { }
	public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
	public virtual void ResetEffect(Player player) { }
	public virtual void Update(Player player) { }
	public virtual void OnMissingMana(Player player, Item item, int neededMana) { }
	public virtual void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
	public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
	public virtual void OnHitByAnything(Player player) { }
	public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
	public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
	public virtual void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) { }
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
	public int EnergyCap = 1500;
	public int Energy = 0;
	public int Duration = 0;
	public int CoolDown = 0;
	int[] SkillHolder1 = new int[10];
	int[] SkillHolder2 = new int[10];
	int[] SkillHolder3 = new int[10];
	public int[] SkillInventory = new int[30];
	public bool Activate = false;
	public int Duplicate = 0;
	int CurrentActiveHolder = 1;
	public int CurrentActiveIndex { get => CurrentActiveHolder; }
	int RechargeDelay = 0;
	public int MaximumCoolDown = 0;
	public int BloodToPower = 0;
	public override void Initialize() {
		Array.Fill(SkillHolder1, -1);
		Array.Fill(SkillHolder2, -1);
		Array.Fill(SkillHolder3, -1);
		Array.Fill(SkillInventory, -1);
	}
	public void ChangeHolder(int index) {
		CurrentActiveHolder = Math.Clamp(index, 1, 3);
	}
	public bool RequestAddSkill_Inventory(int skillType, bool OnRandomizeChoose = true) {
		if (skillType < 0 && skillType >= SkillLoader.TotalCount) {
			return false;
		}
		int availableIndex = -1;
		for (int i = 0; i < SkillInventory.Length; i++) {
			if (SkillInventory[i] != -1) {
				continue;
			}
			availableIndex = i;
			break;
		}
		if (availableIndex == -1) {
			BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.IndianRed, "Fail to add a skill");
			return false;
		}
		if (!SkillLoader.GetSkill(skillType).CanBeSelect) {
			if (OnRandomizeChoose) {
				skillType = Main.rand.Next(SkillLoader.TotalCount);
			}
			else {
				SkillInventory[availableIndex] = skillType;
				BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.Aqua, $"Added skill : {SkillLoader.GetSkill(skillType).DisplayName}");
				return true;

			}
		}
		SkillInventory[availableIndex] = skillType;
		BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.Aqua, "Added a skill");
		return true;
	}
	public void SkillStatTotal(out int energy, out int duration, out int cooldown) {
		int[] active = GetCurrentActiveSkillHolder();
		energy = 0;
		duration = 0;
		cooldown = 0;
		float percentageEnergy = 1;
		StatModifier energyS = new(), durationS = new(), cooldownS = new();
		int seperateEnergy = 0;
		for (int i = 0; i < active.Length; i++) {
			ModSkill skill = SkillLoader.GetSkill(active[i]);
			if (skill == null) {
				continue;
			}
			if (skill.Type == ModSkill.GetSkillType<PowerSaver>()) {
				seperateEnergy += skill.EnergyRequire;
			}
			else {
				energy += (int)energyS.ApplyTo(skill.EnergyRequire);
			}
			duration += (int)durationS.ApplyTo(skill.Duration);
			cooldown += (int)cooldownS.ApplyTo(skill.CoolDown);
			percentageEnergy += skill.EnergyRequirePercentage;
			skill.ModifyNextSkillStats(out energyS, out durationS, out cooldownS);
		}
		energy = (int)(energy * percentageEnergy) + seperateEnergy;
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
	public ModSkill CurrentSkill(ref int currentIndex) {
		int[] active = GetCurrentActiveSkillHolder();
		ModSkill skill = SkillLoader.GetSkill(active[currentIndex]);
		if (skill == null) {
			return null;
		}
		skill.OnCalled(Player, this, ref currentIndex);
		return SkillLoader.GetSkill(active[currentIndex]);
	}
	public void SwitchSkill(int whoAmIsource, int whoAmIdestination) {
		int cache;
		switch (CurrentActiveHolder) {
			case 1:
				cache = SkillHolder1[whoAmIsource];
				SkillHolder1[whoAmIsource] = SkillHolder1[whoAmIdestination];
				SkillHolder1[whoAmIdestination] = cache;
				break;
			case 2:
				cache = SkillHolder2[whoAmIsource];
				SkillHolder2[whoAmIsource] = SkillHolder2[whoAmIdestination];
				SkillHolder2[whoAmIdestination] = cache;
				break;
			case 3:
				cache = SkillHolder2[whoAmIsource];
				SkillHolder2[whoAmIsource] = SkillHolder2[whoAmIdestination];
				SkillHolder2[whoAmIdestination] = cache;
				break;
		}
	}
	/// <summary>
	/// idk, this name sound cool, so I'm keeping it
	/// This will remove a skill from skill holder
	/// It won't actually request anything network wise
	/// </summary>
	/// <param name="whoAmI"></param>
	public void RequestSkillRemoval_SkillHolder(int whoAmI) {
		switch (CurrentActiveHolder) {
			case 1:
				SkillHolder1[whoAmI] = -1;
				break;
			case 2:
				SkillHolder2[whoAmI] = -1;
				break;
			case 3:
				SkillHolder3[whoAmI] = -1;
				break;
		}
	}
	/// <summary>
	/// idk, this name sound cool, so I'm keeping it
	/// This will remove a skill from skill inventory
	/// It won't actually request anything network wise
	/// </summary>
	/// <param name="whoAmI"></param>
	public void RequestSkillRemoval_SkillInventory(int whoAmI) {
		SkillInventory[whoAmI] = -1;
	}
	public override void ProcessTriggers(TriggersSet triggersSet) {
		if (!UniversalSystem.CanAccessContent(Player, UniversalSystem.SYNERGY_MODE)) {
			return;
		}
		if (SkillModSystem.SkillActivation.JustReleased) {
			if (CoolDown > 0) {
				BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.Red, "Skill on cool down !");
				return;
			}
			Activate = true;
			SkillStatTotal(out int energy, out int duration, out int cooldown);
			Duration = duration;
			CoolDown = cooldown;
			if (energy > Energy) {
				BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.Red, "Not Enough energy !");
				Duration = 0;
				CoolDown = 0;
				Activate = false;
				MaximumCoolDown = 0;
				return;
			}
			else {
				MaximumCoolDown = CoolDown;
				Energy -= energy;
				for (int i = 0; i < 10; i++) {
					ModSkill skill = CurrentSkill(ref i);
					if (skill == null) {
						continue;
					}
					skill.OnTrigger(Player);
				}
			}
		}
	}
	public void Modify_EnergyAmount(int amount) {
		Energy = Math.Clamp(Energy + amount, 0, EnergyCap);
	}
	public override void PreUpdate() {
		RechargeDelay = BossRushUtils.CountDown(RechargeDelay);
		if (Duration <= 0) {
			Activate = false;
			for (int i = 0; i < 10; i++) {
				ModSkill skill = CurrentSkill(ref i);
				if (skill == null) {
					continue;
				}
				skill.OnEnded(Player);
			}
		}
		else {
			Duration = BossRushUtils.CountDown(Duration);
		}
		CurrentActiveHolder = Math.Clamp(CurrentActiveHolder, 1, 3);
		if (!Activate) {
			CoolDown = BossRushUtils.CountDown(CoolDown);
		}
	}
	public override void ResetEffects() {
		if (!Activate) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			ModSkill skill = CurrentSkill(ref i);
			if (skill == null) {
				continue;
			}
			CurrentSkill(ref i).ResetEffect(Player);
		}
	}
	public override void UpdateEquips() {
		if (!Activate) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			ModSkill skill = CurrentSkill(ref i);
			if (skill == null) {
				continue;
			}
			skill.Update(Player);
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!Activate) {
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		for (int i = 0; i < 10; i++) {
			ModSkill skill = CurrentSkill(ref i);
			if (skill == null) {
				continue;
			}
			skill.Shoot(Player, item, source, position, velocity, type, damage, knockback);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnMissingMana(Item item, int neededMana) {
		if (!Activate) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			ModSkill skill = CurrentSkill(ref i);
			if (skill == null) {
				continue;
			}
			skill.OnMissingMana(Player, item, neededMana);
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (!Activate) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			ModSkill skill = CurrentSkill(ref i);
			if (skill == null) {
				continue;
			}
			skill.ModifyHitNPCWithItem(Player, item, target, ref modifiers);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (!Activate) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			ModSkill skill = CurrentSkill(ref i);
			if (skill == null) {
				continue;
			}
			skill.ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Activate) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			ModSkill skill = CurrentSkill(ref i);
			if (skill == null) {
				continue;
			}
			skill.OnHitNPCWithItem(Player, item, target, hit, damageDone);
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Activate) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			ModSkill skill = CurrentSkill(ref i);
			if (skill == null) {
				continue;
			}
			skill.OnHitNPCWithProj(Player, proj, target, hit, damageDone);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Activate && RechargeDelay <= 0 && CoolDown <= 0) {
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			Energy = Math.Clamp((int)Math.Ceiling(modplayer.EnergyRecharge.ApplyTo(hit.Damage)) + Energy, 0, EnergyCap);
			RechargeDelay = (int)modplayer.RechargeEnergyCap.ApplyTo(hit.Damage);
		}
	}
	public override void UpdateDead() {
		Activate = false;
		Energy = 0;
		Duration = 0;
		RechargeDelay = 0;
		CoolDown = 0;
	}
	public override void SaveData(TagCompound tag) {
		tag.Add("SkillHolder1", SkillHolder1);
		tag.Add("SkillHolder2", SkillHolder2);
		tag.Add("SkillHolder3", SkillHolder3);
		tag.Add("SkillInventory", SkillInventory);
	}
	public override void LoadData(TagCompound tag) {
		if (tag.TryGet("SkillHolder1", out int[] SkillHolder1)) {
			Array.Copy(SkillHolder1, this.SkillHolder1, 10);
		}
		if (tag.TryGet("SkillHolder2", out int[] SkillHolder2)) {
			Array.Copy(SkillHolder2, this.SkillHolder2, 10);
		}
		if (tag.TryGet("SkillHolder3", out int[] SkillHolder3)) {
			Array.Copy(SkillHolder3, this.SkillHolder3, 10);
		}
		if (tag.TryGet("SkillInventory", out int[] SkillInventory)) {
			Array.Copy(SkillInventory, this.SkillInventory, SkillInventory.Length);
		}
	}
	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
		ModPacket packet = Mod.GetPacket();
		packet.Write((byte)BossRush.MessageType.Skill);
		packet.Write((byte)Player.whoAmI);
		packet.Write(SkillInventory.Length);
		foreach (int item in SkillInventory) {
			packet.Write(item);
		}
		packet.Write(SkillHolder1.Length);
		foreach (int item in SkillHolder1) {
			packet.Write(item);
		}
		foreach (int item in SkillHolder2) {
			packet.Write(item);
		}
		foreach (int item in SkillHolder3) {
			packet.Write(item);
		}
		packet.Send(toWho, fromWho);
	}
	public void ReceivePlayerSync(BinaryReader reader) {
		Array.Fill(SkillInventory, -1);
		Array.Fill(SkillHolder1, -1);
		Array.Fill(SkillHolder2, -1);
		Array.Fill(SkillHolder3, -1);
		int countInventory = reader.ReadInt32();
		for (int i = 0; i < countInventory; i++) {
			SkillInventory[i] = reader.ReadInt32();
		}
		int countHolder = reader.ReadInt32();
		for (int i = 0; i < countHolder; i++)
			SkillHolder1[i] = reader.ReadInt32();
		for (int i = 0; i < countHolder; i++)
			SkillHolder2[i] = reader.ReadInt32();
		for (int i = 0; i < countHolder; i++)
			SkillHolder3[i] = reader.ReadInt32();
	}

	public override void CopyClientState(ModPlayer targetCopy) {
		SkillHandlePlayer clone = (SkillHandlePlayer)targetCopy;
		clone.SkillInventory = SkillInventory;
		clone.SkillHolder1 = SkillHolder1;
		clone.SkillHolder2 = SkillHolder2;
		clone.SkillHolder3 = SkillHolder3;
	}

	public override void SendClientChanges(ModPlayer clientPlayer) {
		SkillHandlePlayer clone = (SkillHandlePlayer)clientPlayer;
		if (SkillInventory != clone.SkillInventory
			|| SkillHolder1 != clone.SkillHolder1
			|| SkillHolder2 != clone.SkillHolder2
			|| SkillHolder3 != clone.SkillHolder3
			) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
	}
}
public class SkillOrb : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 30;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.useTime = Item.useAnimation = 15;
	}
	public override bool? UseItem(Player player) {
		return base.UseItem(player);
	}
}
