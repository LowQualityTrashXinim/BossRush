using BossRush.Contents.Items.Weapon;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Items.Accessories.LostAccessories {
	internal class GodDice : ModItem {
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 14));
		}
		public override void SetDefaults() {
			Item.width = 46;
			Item.height = 52;
			Item.accessory = true;
			Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
		}
		public override void UpdateEquip(Player player) {
			GamblePlayer gamblePlayer = player.GetModPlayer<GamblePlayer>();
			gamblePlayer.GodDice = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			var player = Main.LocalPlayer;
			var gamblePlayer = player.GetModPlayer<GamblePlayer>();
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Damage", "Damage multiply : " + gamblePlayer.GambleDamage + ""));
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Defense", "Defense multiply : " + gamblePlayer.GambleDef + ""));
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Speed", "Speed multiply : " + gamblePlayer.GambleSpeed + ""));
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Max life", "Max life multiply : " + gamblePlayer.GambleHP + ""));
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Life regen", "Life regenaration multiply : " + gamblePlayer.GambleLifeRegen + ""));
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Mana", "Mana multiply : " + gamblePlayer.GambleMana + ""));
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Mana regen", "Mana regenaration multiply : " + gamblePlayer.GambleManaRegen + ""));
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Minion", "Extra minion slot : " + gamblePlayer.GambleMinionSlot + ""));
			tooltips.Add(new TooltipLine(Mod, "GodDice_" + "Crit", "Crit chance increases : " + gamblePlayer.GambleCrit + ""));
		}
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		public override bool? UseItem(Player player) {
			var gamblePlayer = player.GetModPlayer<GamblePlayer>();
			if (player.IsDebugPlayer() && player.altFunctionUse == 2) {
				gamblePlayer.Roll++;
				gamblePlayer.GambleDamage = 1;
				gamblePlayer.GambleDef = 0;
				gamblePlayer.GambleSpeed = 1;
				gamblePlayer.GambleHP = 1;
				gamblePlayer.GambleLifeRegen = 1;
				gamblePlayer.GambleMana = 1;
				gamblePlayer.GambleManaRegen = 1;
				gamblePlayer.GambleMinionSlot = 0;
				gamblePlayer.GambleCrit = 0;
				return true;
			}
			return true;
		}
	}
	class GamblePlayer : ModPlayer {
		public float GambleDamage = 1;
		public float GambleSpeed = 1;
		public float GambleHP = 1;
		public float GambleLifeRegen = 1;
		public float GambleMana = 1;
		public float GambleManaRegen = 1;
		public int GambleDef = 0;
		public int GambleMinionSlot = 0;
		public int GambleCrit = 0;
		public int Roll = 0;
		public bool GodDice = false;
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (GodDice) {
				damage *= GambleDamage;
			}
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			if (!GodDice)
				return;
			health *= GambleHP;
			mana *= GambleMana;
		}
		public override void ResetEffects() {
			if (!GodDice)
				return;
			GodDice = false;
			if (Roll > 0) {
				GambleDamage = (float)Math.Round(Main.rand.NextFloat(.85f, 1.15f), 2);
				GambleDef = Main.rand.Next(-25, 25);
				GambleSpeed = (float)Math.Round(Main.rand.NextFloat(.8f, 1.2f), 2);
				GambleHP = (float)Math.Round(Main.rand.NextFloat(.85f, 1.15f), 2);
				GambleLifeRegen = (float)Math.Round(Main.rand.NextFloat(.85f, 1.15f), 2);
				GambleMana = (float)Math.Round(Main.rand.NextFloat(.85f, 1.15f), 2);
				GambleManaRegen = (float)Math.Round(Main.rand.NextFloat(.85f, 1.15f), 2);
				GambleMinionSlot = Main.rand.Next(-2, 2);
				GambleCrit = Main.rand.Next(-15, 15);
				Roll--;
			}
			Player.statDefense += GambleDef;
			Player.accRunSpeed *= GambleSpeed;
			Player.lifeRegen = (int)(GambleLifeRegen * Player.lifeRegen);
			Player.manaRegen = (int)(Player.manaRegen * GambleManaRegen);
			Player.maxMinions = Math.Clamp(GambleMinionSlot + Player.maxMinions, 0, 999999999);
			Player.maxTurrets = Math.Clamp(GambleMinionSlot + Player.maxTurrets, 0, 999999999);
			Player.GetCritChance(DamageClass.Generic) += GambleCrit;
		}
		public override void NaturalLifeRegen(ref float regen) {
			if (!GodDice)
				return;
			regen *= GambleLifeRegen;
		}
		public override void Initialize() {
			GambleDamage = 1;
			GambleDef = 0;
			GambleSpeed = 1;
			GambleHP = 1;
			GambleLifeRegen = 1;
			GambleMana = 1;
			GambleManaRegen = 1;
			GambleMinionSlot = 0;
			GambleCrit = 0;
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			var packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.GambleAddiction);
			packet.Write((byte)Player.whoAmI);
			packet.Write(GambleDamage);
			packet.Write(GambleDef);
			packet.Write(GambleSpeed);
			packet.Write(GambleHP);
			packet.Write(GambleLifeRegen);
			packet.Write(GambleMana);
			packet.Write(GambleManaRegen);
			packet.Write(GambleMinionSlot);
			packet.Write(GambleCrit);
			packet.Send(toWho, fromWho);
		}
		public override void SaveData(TagCompound tag) {
			tag["GambleDmg"] = GambleDamage;
			tag["GambleDef"] = GambleDef;
			tag["GambleSpeed"] = GambleSpeed;
			tag["GambleHP"] = GambleHP;
			tag["GambleLifeRegen"] = GambleLifeRegen;
			tag["GambleMana"] = GambleMana;
			tag["GambleManaRegen"] = GambleManaRegen;
			tag["GambleMinionSlot"] = GambleMinionSlot;
			tag["GambleCrit"] = GambleCrit;
		}

		public override void LoadData(TagCompound tag) {
			GambleDamage = tag.GetFloat("GambleDmg");
			if (tag.ContainsKey("GambleDef")) {
				object GambleDefData = tag["GambleDef"];
				if (GambleDefData is float GambleDefFloat)
					GambleDef = (int)GambleDefFloat;
				if (GambleDefData is int GambleDefInt)
					GambleDef = GambleDefInt;
			}
			GambleSpeed = tag.GetFloat("GambleSpeed");
			GambleHP = tag.GetFloat("GambleHP");
			GambleLifeRegen = tag.GetFloat("GambleLifeRegen");
			GambleMana = tag.GetFloat("GambleMana");
			GambleManaRegen = tag.GetFloat("GambleManaRegen");
			GambleMinionSlot = tag.GetInt("GambleMinionSlot");
			GambleCrit = tag.GetInt("GambleCrit");
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			GambleDamage = reader.ReadSingle();
			GambleDef = reader.ReadInt32();
			GambleSpeed = reader.ReadSingle();
			GambleHP = reader.ReadSingle();
			GambleLifeRegen = reader.ReadSingle();
			GambleMana = reader.ReadSingle();
			GambleManaRegen = reader.ReadSingle();
			GambleMinionSlot = reader.ReadInt32();
			GambleCrit = reader.ReadInt32();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			var clone = (GamblePlayer)targetCopy;
			clone.GambleDamage = GambleDamage;
			clone.GambleDef = GambleDef;
			clone.GambleSpeed = GambleSpeed;
			clone.GambleHP = GambleHP;
			clone.GambleLifeRegen = GambleLifeRegen;
			clone.GambleMana = GambleMana;
			clone.GambleManaRegen = GambleManaRegen;
			clone.GambleMinionSlot = GambleMinionSlot;
			clone.GambleCrit = GambleCrit;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			var clone = (GamblePlayer)clientPlayer;
			if (GambleDamage != clone.GambleDamage
			|| GambleDef != clone.GambleDef
			|| GambleSpeed != clone.GambleSpeed
			|| GambleHP != clone.GambleHP
			|| GambleLifeRegen != clone.GambleLifeRegen
			|| GambleMana != clone.GambleMana
			|| GambleManaRegen != clone.GambleManaRegen
			|| GambleMinionSlot != clone.GambleMinionSlot
			|| GambleCrit != clone.GambleCrit) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}
