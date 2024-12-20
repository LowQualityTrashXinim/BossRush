﻿using Terraria;
using System.IO;
using Terraria.ID;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Contents.Items.Consumable.SpecialReward;

namespace BossRush.Contents.Perks {
	public class PerkItem : GlobalItem {
		public override bool? UseItem(Item item, Player player) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perk_PotionExpert && item.buffType > 0) {
				if (player.ItemAnimationJustStarted) {
					perkplayer.PotionExpert_perk_CanConsume = Main.rand.NextFloat() <= .55f;
				}
				return perkplayer.PotionExpert_perk_CanConsume;
			}
			return base.UseItem(item, player);
		}

		// how is drinking a potion with left click works differently from quick heal?... talking about a fresh spaghetti serving right there.
		public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			StatModifier healingPotionstat = StatModifier.Default;
			if (perkplayer.perk_PotionCleanse) {
				healingPotionstat -= .5f;
			}
			if (perkplayer.perk_ImprovedPotion) {
				healingPotionstat += .7f;
			}
			healValue = (int)healingPotionstat.ApplyTo(healValue);
		}

		public override bool ConsumeItem(Item item, Player player) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perk_PotionCleanse && item.healLife > 0) {
				foreach (int i in player.buffType) {
					if (Main.debuff[i]) {
						player.ClearBuff(i);

					}
				}
			}
			if (perkplayer.perk_AlchemistPotion && item.buffType > 0 && !player.HasBuff(ModContent.BuffType<MysteriousPotionBuff>())) {
				MysteriousPotionBuff.SetBuff(7, BossRushUtils.ToMinute(4), player);
			}
			return base.ConsumeItem(item, player);
		}
	}
	class PerkModSystem : ModSystem {
		public override void Load() {
			base.Load();
			On_Player.QuickMana += On_Player_QuickMana;
		}
		private void On_Player_QuickMana(On_Player.orig_QuickMana orig, Player self) {
			PerkPlayer perkplayer = self.GetModPlayer<PerkPlayer>();
			if (self.HasBuff(ModContent.BuffType<ManaBlock>()) && perkplayer.perk_ImprovedPotion) {
				return;
			}
			orig(self);
		}
	}
	class MagicOverhaulBuff : GlobalBuff {
		public override void Update(int type, Player player, ref int buffIndex) {
			if (type == BuffID.ManaSickness && player.GetModPlayer<PerkPlayer>().perk_ImprovedPotion) {
				if (player.statMana < player.statManaMax2) {
					player.statMana += 2;
				}
				if (player.buffTime[buffIndex] <= 0) {
					player.AddBuff(ModContent.BuffType<ManaBlock>(), BossRushUtils.ToSecond(10));
				}
			}
		}
	}
	class ManaBlock : ModBuff {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			base.Update(player, ref buffIndex);
		}
	}

	public class PerkPlayer : ModPlayer {
		public bool CanGetPerk = false;
		public int PerkAmount = 4;
		private byte perk_Reroll = 1;
		public void Modify_RerollCount(byte amount, bool negative = false) {
			int simulate = perk_Reroll + amount;
			if (simulate < byte.MinValue) {
				perk_Reroll = byte.MinValue;
			}
			else if (simulate > byte.MaxValue) {
				perk_Reroll = byte.MaxValue;
			}
			if (negative) {
				perk_Reroll -= amount;
			}
			else {
				perk_Reroll += amount;
			}
		}
		public byte Reroll => perk_Reroll;
		/// <summary>
		/// Keys : Perk type<br/>
		/// Values : Stack value
		/// </summary>
		public Dictionary<int, int> perks = new Dictionary<int, int>();

		public bool perk_PotionExpert = false;
		public bool perk_PotionCleanse = false;
		public bool perk_AlchemistPotion = false;
		public bool perk_ImprovedPotion = false;
		public bool PotionExpert_perk_CanConsume = false;
		public bool perk_ScatterShot = false;
		public override void Initialize() {
			perks = new Dictionary<int, int>();
			PerkAmount = 4;
		}
		public int PerkAmountModified() {
			if (perks.ContainsKey(Perk.GetPerkType<BlessingOfPerk>())) {
				return PerkAmount + perks[Perk.GetPerkType<BlessingOfPerk>()];
			}
			return PerkAmount;
		}
		public bool HasPerk<T>() where T : Perk => perks.ContainsKey(Perk.GetPerkType<T>());
		public override void PostItemCheck() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnUseItem(Player, Player.HeldItem);
			}
		}
		public override bool CanUseItem(Item item) {
			if (item.buffType == BuffID.ManaSickness && Player.HasBuff(ModContent.BuffType<ManaBlock>())) {
				return false;
			}
			return base.CanUseItem(item);
		}
		public override void ResetEffects() {
			perk_PotionExpert = false;
			perk_PotionCleanse = false;
			perk_AlchemistPotion = false;
			perk_ImprovedPotion = false;
			perk_ScatterShot = false;
			PerkAmount = 4;
			PerkAmount = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count + PerkAmountModified();
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ResetEffect(Player);
			}
		}
		public override void PostUpdateEquips() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).UpdateEquip(Player);
			}
		}
		public override void PostUpdate() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).Update(Player);
			}
		}
		public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyShootStat(Player, item, ref position, ref velocity, ref type, ref damage, ref knockback);
			}
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).Shoot(Player, item, source, position, velocity, type, damage, knockback);
			}
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		public override void OnMissingMana(Item item, int neededMana) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnMissingMana(Player, item, neededMana);
			}
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyMaxStats(Player, ref health, ref mana);
			}
		}
		public override void ModifyWeaponCrit(Item item, ref float crit) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyCriticalStrikeChance(Player, item, ref crit);
			}
		}
		public override void ModifyItemScale(Item item, ref float scale) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyItemScale(Player, item, ref scale);
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyDamage(Player, item, ref damage);
			}
		}
		public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyKnockBack(Player, item, ref knockback);
			}
		}
		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitNPCWithItem(Player, item, target, ref modifiers);
			}
		}
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
			}
		}
		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitByNPC(Player, npc, ref modifiers);
			}
		}
		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyHitByProjectile(Player, proj, ref modifiers);
			}
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitNPCWithItem(Player, item, target, hit, damageDone);
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitNPCWithProj(Player, proj, target, hit, damageDone);
			}
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitByAnything(Player);
				ModPerkLoader.GetPerk(perk).OnHitByNPC(Player, npc, hurtInfo);
			}
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).OnHitByAnything(Player);
				ModPerkLoader.GetPerk(perk).OnHitByProjectile(Player, proj, hurtInfo);
			}
		}
		public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyManaCost(Player, item, ref reduce, ref mult);
			}
		}
		public override float UseSpeedMultiplier(Item item) {
			float useSpeed = 1;
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ModifyUseSpeed(Player, item, ref useSpeed);
			}
			return useSpeed;
		}

		public override bool FreeDodge(Player.HurtInfo info) {
			foreach (int perk in perks.Keys) {
				if (ModPerkLoader.GetPerk(perk).FreeDodge(Player, info)) {
					return true;
				}
			}
			return base.FreeDodge(info);
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
			foreach (int perk in perks.Keys) {
				if (ModPerkLoader.GetPerk(perk).PreKill(Player)) {
					return false;
				}
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
		}
		public override void SaveData(TagCompound tag) {
			tag["PlayerPerks"] = perks.Keys.ToList();
			tag["PlayerPerkStack"] = perks.Values.ToList();
			tag["perk_Reroll"] = perk_Reroll;
		}
		public override void LoadData(TagCompound tag) {
			var PlayerPerks = tag.Get<List<int>>("PlayerPerks");
			var PlayerPerkStack = tag.Get<List<int>>("PlayerPerkStack");
			perks = PlayerPerks.Zip(PlayerPerkStack, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

			int count = perks.Count;
			for (int i = count - 1; i >= 0; i--) {
				if (ModPerkLoader.GetPerk(perks.Keys.ElementAt(i)) == null) {
					perks.Remove(perks.Keys.ElementAt(i));
				}
			}
			if (tag.TryGet("perk_Reroll", out byte va)) {
				perk_Reroll = va;
			}
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.Perk);
			packet.Write((byte)Player.whoAmI);
			packet.Write(perks.Keys.Count);
			foreach (int item in perks.Keys) {
				packet.Write(item);
				packet.Write(perks[item]);
			}
			packet.Write(perk_Reroll);
			packet.Send(toWho, fromWho);
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			perks.Clear();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
				perks.Add(reader.ReadInt32(), reader.ReadInt32());
			perk_Reroll = reader.ReadByte();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			PerkPlayer clone = (PerkPlayer)targetCopy;
			clone.perks = perks;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			PerkPlayer clone = (PerkPlayer)clientPlayer;
			if (perks != clone.perks) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
	public abstract class Perk : ModType {
		public string DisplayName => Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.DisplayName");
		public string Description => Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.Description");
		public bool CanBeStack = false;
		/// <summary>
		/// This will get the value from Mod Perk player itself<br/>
		/// it is recommend to use this rather than get it yourself cause what it doing is pretty much the same<br/>
		/// This is prefer over the previous method as this do not update and isntead just get it from the source<br/>
		/// In whatever case it is, it is highly recommend to not cached it cause the performance increases is very minimal<br/>
		/// </summary>
		public int StackAmount(Player player) {
			if (player.TryGetModPlayer(out PerkPlayer perkplayer)) {
				if (perkplayer.perks.ContainsKey(Type))
					return perkplayer.perks[Type];
			}
			return 0;
		}
		/// <summary>
		/// This is where you set limit to amount of stack should a perk have<br/>
		/// <see cref="StackAmount"/> will always start at 0 and increase by 1 ( regardless if <see cref="CanBeStack"/> true or false )<br/>
		/// The next time this perk get choosen, it will increase by 1<br/>
		/// The perk will no longer show up if the stack amount reach the limit, for more info see <see cref="PerkUIState.ActivateNormalPerkUI(PerkPlayer, Player)"/><br/>
		/// If you are modifying tooltip base on <see cref="StackAmount"/> then you should substract stack amount by 1
		/// </summary>
		public int StackLimit = 1;
		/// <summary>
		/// Please set this texture string as if you are setting <see cref="ModItem.Texture"/>
		/// </summary>
		public string textureString = null;
		public string Tooltip = null;
		/// <summary>
		/// This will prevent from perk being able to be choose
		/// </summary>
		public bool CanBeChoosen = true;
		public int Type { get; private set; }
		protected sealed override void Register() {
			Type = ModPerkLoader.Register(this);
		}
		public static int GetPerkType<T>() where T : Perk {
			return ModContent.GetInstance<T>().Type;
		}
		public string PerkNameToolTip => ModifyName() + "\n" + ModifyToolTip();
		public virtual string ModifyToolTip() {
			if (Description != null)
				return Description;
			return Tooltip;
		}
		public virtual string ModifyName() {
			return PerkName();
		}
		public string PerkName() {
			if (DisplayName != null)
				return DisplayName;
			string Name = ModPerkLoader.GetPerk(Type).Name;
			for (int i = Name.Length - 1; i > 0; i--) {
				if (char.IsUpper(Name[i])) {
					Name = Name.Substring(0, i) + " " + Name.Substring(i);
				}
			}
			return Name;
		}
		public sealed override void Unload() {
			base.Unload();
			textureString = null;
			Tooltip = null;
		}
		public Perk() {
			SetDefaults();
			if (CanBeStack)
				Tooltip += "\n( Can be stack ! )";
		}
		/// <summary>
		/// This act different to <see cref="CanBeChoosen"/><br/>
		/// if this is set to false then it won't be add into perk pool, otherwise if <see cref="CanBeChoosen"/> is false and this is true then it still won't be add<br/>
		/// This only run on client side so it is recommend to uses <see cref="Main.LocalPlayer"/> to check player condition
		/// </summary>
		/// <returns></returns>
		public virtual bool SelectChoosing() => true;
		public virtual void SetDefaults() { }
		public virtual void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
		public virtual void OnUseItem(Player player, Item item) { }
		public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
		public virtual void Update(Player player) { }
		public virtual void UpdateEquip(Player player) { }
		public virtual void ResetEffect(Player player) { }
		public virtual void OnMissingMana(Player player, Item item, int neededMana) { }
		public virtual void ModifyDamage(Player player, Item item, ref StatModifier damage) { }
		public virtual void ModifyKnockBack(Player player, Item item, ref StatModifier knockback) { }
		public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitByAnything(Player player) { }
		public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
		public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
		public virtual void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
		public virtual void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
		public virtual void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) { }
		public virtual void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) { }
		public virtual void ModifyItemScale(Player player, Item item, ref float scale) { }
		public virtual void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) { }
		public virtual void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) { }
		public virtual void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) { }
		/// <summary>
		/// Subtract will make player use weapon slower
		/// Additive will make player use weapon faster
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <param name="useSpeed">by default start at 1</param>
		public virtual void ModifyUseSpeed(Player player, Item item, ref float useSpeed) { }
		public virtual void OnChoose(Player player) { }
		public virtual bool FreeDodge(Player player, Player.HurtInfo hurtInfo) => false;
		public virtual bool PreKill(Player player) => false;
	}
	public static class ModPerkLoader {
		private static readonly List<Perk> _perks = new();
		public static int TotalCount => _perks.Count;
		public static int Register(Perk perk) {
			ModTypeLookup<Perk>.Register(perk);
			_perks.Add(perk);
			return _perks.Count - 1;
		}
		public static Perk GetPerk(int type) {
			return type >= 0 && type < _perks.Count ? _perks[type] : null;
		}
	}
}
