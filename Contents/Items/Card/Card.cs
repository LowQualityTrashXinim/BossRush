using System;
using Terraria;
using System.IO;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Contents.Artifacts;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;

namespace BossRush.Contents.Items.Card {
	abstract class CardItem : ModItem {
		public const int PlatinumCardDropChance = 40;
		public const int GoldCardDropChance = 20;
		public const int SilverCardDropChance = 10;
		public float Multiplier = 1f;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(30, 24);
			Item.UseSound = SoundID.Item35;
			PostCardSetDefault();
		}
		public virtual void PostCardSetDefault() { }
		public virtual void ModifyCardToolTip(ref List<TooltipLine> tooltips, PlayerCardHandle modplayer) {

		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			PlayerCardHandle modplayer = Main.LocalPlayer.GetModPlayer<PlayerCardHandle>();
			ModifyCardToolTip(ref tooltips, modplayer);
			if (Tier > 0) {
				tooltips.Add(new TooltipLine(Mod, "HelpfulText", "Use the card to get choose from 1 of 3 stats bonus" +
					"\nThe more cards you uses, the higher the chance of getting bad stats will be"));
			}
		}

		public override void OnSpawn(IEntitySource source) {
		}
		/// <summary>
		/// Use this if <see cref="Tier"/> value set within the card item have value larger than 0
		/// </summary>
		public virtual void OnTierItemSpawn() { }

		/// <summary>
		/// 1 = Copper<br/>
		/// 2 = Silver<br/>
		/// 3 = Gold<br/>
		/// 4 = Platinum<br/>
		/// </summary>
		public virtual int Tier => 0;
		public virtual int PostTierModify => Main.LocalPlayer.HasArtifact<MagicalCardDeckArtifact>() ? Tier + 1 : Tier;
		public override bool CanUseItem(Player player) {
			return !BossRushUtils.IsAnyVanillaBossAlive();
		}
		public virtual void OnUseItem(Player player, PlayerCardHandle modplayer) { }
		public override bool? UseItem(Player player) {
			PlayerCardHandle modplayer = player.GetModPlayer<PlayerCardHandle>();
			OnUseItem(player, modplayer);
			if (Tier <= 0)
				return true;
			CardSystem uiSystemInstance = ModContent.GetInstance<CardSystem>();
			uiSystemInstance.cardUIstate.Tier = Tier;
			uiSystemInstance.userInterface.SetState(uiSystemInstance.cardUIstate);
			return true;
		}
		private int countX = 0;
		private float positionRotateX = 0;
		private void PositionHandle() {
			if (positionRotateX < 3.5f && countX == 1) {
				positionRotateX += .2f;
			}
			else {
				countX = -1;
			}
			if (positionRotateX > 0 && countX == -1) {
				positionRotateX -= .2f;
			}
			else {
				countX = 1;
			}
		}
		Color auraColor;
		private void ColorHandle() {
			switch (Tier) {
				case 1:
					auraColor = new Color(255, 100, 0, 30);
					break;
				case 2:
					auraColor = new Color(200, 200, 200, 30);
					break;
				case 3:
					auraColor = new Color(255, 255, 0, 30);
					break;
				default:
					auraColor = new Color(255, 255, 255, 30);
					break;
			}
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			ColorHandle();
			Item.DrawAuraEffect(spriteBatch, position, positionRotateX, positionRotateX, auraColor, 0, scale);
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			Item.DrawAuraEffect(spriteBatch, positionRotateX, positionRotateX, auraColor, rotation, scale);
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		public virtual bool CanBeCraft => true;
		public override void AddRecipes() {
			if (CanBeCraft) {
				CreateRecipe()
					.AddIngredient(ModContent.ItemType<EmptyCard>())
					.Register();
			}
		}
	}
	class PlayerCardHandle : ModPlayer {
		public int CardTracker = 0;
		public bool DecreaseRateOfFire = false;// ID 1
		public bool NoHealing = false;// ID 2
		public bool SluggishDamage = false;// ID 3
		public bool FiveTimeDamageTaken = false;// ID 4
		public bool LimitedResource = false;// ID 5
		public bool PlayWithConstantLifeLost = false;// ID 6
		public bool ReduceIframe = false;// ID 7
		public bool WeaponCanJammed = false; // ID 8 sometime can't use weapon
		public bool WeaponCanKick = false; // ID 9 lose life on use weapon
		public bool NegativeDamageRandomize = false;// ID 10
		public bool CritDealNoDamage = false;// ID 11
		public bool ReducePositiveCardStat = false;// ID 12
		public bool AccessoriesDisable = false; // Will be implement much later
		public List<int> listCursesID = new List<int>();
		public bool ListIsChange = false;
		//We handle no dupe curses in here
		public override void PreUpdate() {
			base.PreUpdate();
			if (item != Player.HeldItem) {
				SlowDown = 0;
				CoolDown = 0;
			}
			DecreaseRateOfFire = false;
			NoHealing = false;
			SluggishDamage = false;
			FiveTimeDamageTaken = false;
			LimitedResource = false;
			PlayWithConstantLifeLost = false;
			ReduceIframe = false;
			WeaponCanJammed = false;
			WeaponCanKick = false;
			NegativeDamageRandomize = false;
			CritDealNoDamage = false;
			ReducePositiveCardStat = false;
			AccessoriesDisable = false;
			for (int i = 0; i < listCursesID.Count; i++) {
				switch (listCursesID[i]) {
					case 1:
						DecreaseRateOfFire = true;
						break;
					case 2:
						NoHealing = true;
						break;
					case 3:
						SluggishDamage = true;
						break;
					case 4:
						FiveTimeDamageTaken = true;
						break;
					case 5:
						LimitedResource = true;
						break;
					case 6:
						PlayWithConstantLifeLost = true;
						break;
					case 7:
						ReduceIframe = true;
						break;
					case 8:
						WeaponCanJammed = true;
						break;
					case 9:
						WeaponCanKick = true;
						break;
					case 10:
						NegativeDamageRandomize = true;
						break;
					case 11:
						CritDealNoDamage = true;
						break;
					case 12:
						ReducePositiveCardStat = true;
						break;
					default:
						break;
				}
			}
		}
		public string CursedStringStats(int curseID) {
			string CursedString;
			switch (curseID) {
				case 1:
					CursedString = "Weapon have decreased fire rate";
					break;
				case 2:
					CursedString = "You can no longer heal using health potion";
					break;
				case 3:
					CursedString = "Weapon deal significantly less damage";
					break;
				case 4:
					CursedString = "Getting hit is much more fatal";
					break;
				case 5:
					CursedString = "Your wounds and your magic refuse to regenerate on their own...;";
					break;
				case 6:
					CursedString = "You always lose life leaving you with 1 hp left";
					break;
				case 7:
					CursedString = "Your immunity frame have been reduced";
					break;
				case 8:
					CursedString = "Your weapons temporarily stop working after too much use...";
					break;
				case 9:
					CursedString = "Your weapon cost life to work";
					break;
				case 10:
					CursedString = "Damage has been ramdomized to be worse";
					break;
				case 11:
					CursedString = "Critical hits deal next to no damage";
					break;
				case 12:
					CursedString = "Cards stats are halved";
					break;
				default:
					CursedString = "Error ! You shouldn't be getting this unless you done something horribly wrong";
					break;
			}
			return CursedString;
		}
		public override float UseSpeedMultiplier(Item item) {
			if (DecreaseRateOfFire) {
				return .35f;
			}
			return base.UseSpeedMultiplier(item);
		}
		float SlowDown = 1;
		int CoolDown = 0;
		public override bool CanUseItem(Item item) {
			if (WeaponCanJammed && CoolDown != 0) {
				return SlowDown <= 1;
			}
			if (WeaponCanKick && Player.statLife < Player.GetWeaponDamage(item)) {
				return false;
			}
			if (ModContent.GetInstance<CardSystem>().userInterface.CurrentState != null)
				return false;
			return base.CanUseItem(item);
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (WeaponCanKick) {
				Player.statLife = Math.Clamp(Player.statLife - Player.GetWeaponDamage(item), 1, Player.statLifeMax2);
			}
			if (WeaponCanJammed && Main.mouseLeft) {
				SlowDown += .2f;
			}
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		public ChestLootDropPlayer ChestLoot => Player.GetModPlayer<ChestLootDropPlayer>();
		public const int maxStatCanBeAchieved = 9999;
		//Copper tier
		public float MeleeDMG = 0;
		public float RangeDMG = 0;
		public float MagicDMG = 0;
		public float SummonDMG = 0;
		public float Movement = 0;
		public float JumpBoost = 0;
		public int HPMax = 0;
		public float HPRegen = 0;
		public int ManaMax = 0;
		public float ManaRegen = 0;
		public int DefenseBase = 0;
		//Silver Tier
		public float DamagePure = 0;
		public int CritStrikeChance = 0;
		public float Thorn = 0;
		public float CritDamage = 1;
		public float DefenseEffectiveness = 1;
		//Gold
		public int DropAmountIncrease = 0;
		public int MinionSlot = 0;
		public int SentrySlot = 0;
		public int CardLuck = 0;
		Item item;
		//Platinum
		//public float LuckIncrease = 0;
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (item.DamageType == DamageClass.Melee) {
				damage.Base = Math.Clamp(MeleeDMG + damage.Base, 1, maxStatCanBeAchieved);
			}
			if (item.DamageType == DamageClass.Ranged) {
				damage.Base = Math.Clamp(RangeDMG + damage.Base, 1, maxStatCanBeAchieved);
			}
			if (item.DamageType == DamageClass.Magic) {
				damage.Base = Math.Clamp(MagicDMG + damage.Base, 1, maxStatCanBeAchieved);
			}
			if (item.DamageType == DamageClass.Summon) {
				damage.Base = Math.Clamp(SummonDMG + damage.Base, 1, maxStatCanBeAchieved);
			}
			damage.Base = Math.Clamp(DamagePure + damage.Base, 1, maxStatCanBeAchieved);
			if (SluggishDamage) {
				damage *= .5f;
			}
			if (NegativeDamageRandomize) {
				damage *= Main.rand.NextFloat();
			}
		}
		public override void ModifyWeaponCrit(Item item, ref float crit) {
			crit = Math.Clamp(CritStrikeChance + crit, 0, maxStatCanBeAchieved);
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.CritDamage.Flat = Math.Clamp(CritDamage, -modifiers.CritDamage.Base + 1, 999999) * modifiers.CritDamage.Base;
			if (CritDealNoDamage) {
				modifiers.CritDamage *= 0;
			}
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			health = StatModifier.Default;
			mana = StatModifier.Default;

			health.Base = Math.Clamp(HPMax + health.Base, -100, maxStatCanBeAchieved);
			mana.Base = Math.Clamp(ManaMax + mana.Base, -20, maxStatCanBeAchieved);
		}
		public override void PostUpdate() {
			base.PostUpdate();
			item = Player.HeldItem;
			ChestLoot.amountModifier = Math.Clamp(DropAmountIncrease + ChestLoot.amountModifier, 0, maxStatCanBeAchieved);
			if (NoHealing) {
				Player.AddBuff(BuffID.PotionSickness, 999);
			}
			if (PlayWithConstantLifeLost) {
				Player.statLife = Math.Clamp(Player.statLife - 1, 1, Player.statLifeMax2);
			}
			if (WeaponCanJammed) {
				if (SlowDown >= 6) {
					SlowDown = 6;
					CoolDown = Player.itemAnimationMax * 10;
				}
				if (!Player.ItemAnimationActive) {
					CoolDown = BossRushUtils.CoolDown(CoolDown);
					SlowDown = Math.Clamp(SlowDown - .05f, 1, 6);
				}
			}
		}
		public override void ModifyHurt(ref Player.HurtModifiers modifiers) {
			base.ModifyHurt(ref modifiers);
			if (FiveTimeDamageTaken) {
				modifiers.FinalDamage *= 5;
			}
		}
		public override void PostHurt(Player.HurtInfo info) {
			base.PostHurt(info);
			if (info.PvP) {
				return;
			}
			if (ReduceIframe) {
				Player.AddImmuneTime(info.CooldownCounter, -30);
			}
		}
		public override void ResetEffects() {
			Player.statDefense += Math.Clamp(DefenseBase, -(maxStatCanBeAchieved + Player.statDefense), maxStatCanBeAchieved);
			Player.moveSpeed = Math.Clamp(Movement + Player.moveSpeed, 0, maxStatCanBeAchieved);
			Player.jumpSpeedBoost = Math.Clamp(JumpBoost + Player.jumpSpeedBoost, 0, maxStatCanBeAchieved);
			Player.lifeRegen = (int)Math.Clamp(HPRegen * Player.lifeRegen, 0, maxStatCanBeAchieved);
			Player.manaRegen = (int)Math.Clamp(ManaRegen * Player.manaRegen, 0, maxStatCanBeAchieved);
			Player.DefenseEffectiveness *= Math.Clamp(DefenseEffectiveness, 0, maxStatCanBeAchieved);
			Player.maxMinions = Math.Clamp(MinionSlot + Player.maxMinions, 0, maxStatCanBeAchieved);
			Player.maxTurrets = Math.Clamp(SentrySlot + Player.maxTurrets, 0, maxStatCanBeAchieved);
			Player.thorns += Thorn;
			if (LimitedResource) {
				Player.lifeRegen = 0;
				Player.manaRegen = 0;
			}
		}
		public override void Initialize() {
			MeleeDMG = 0;
			RangeDMG = 0;
			MagicDMG = 0;
			SummonDMG = 0;
			Movement = 0;
			JumpBoost = 0;
			HPMax = 0;
			HPRegen = 0;
			ManaMax = 0;
			ManaRegen = 0;
			DefenseBase = 0;
			DamagePure = 0;
			CritStrikeChance = 0;
			Thorn = 0;
			CritDamage = 1;
			DefenseEffectiveness = 1;
			DropAmountIncrease = 0;
			MinionSlot = 0;
			SentrySlot = 0;
			CardTracker = 0;
			CardLuck = 0;
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.CardEffect);
			packet.Write((byte)Player.whoAmI);
			packet.Write(MeleeDMG);
			packet.Write(RangeDMG);
			packet.Write(MagicDMG);
			packet.Write(SummonDMG);
			packet.Write(Movement);
			packet.Write(JumpBoost);
			packet.Write(HPMax);
			packet.Write(HPRegen);
			packet.Write(ManaMax);
			packet.Write(ManaRegen);
			packet.Write(DefenseBase);
			packet.Write(DamagePure);
			packet.Write(CritStrikeChance);
			packet.Write(CritDamage);
			packet.Write(DefenseEffectiveness);
			packet.Write(DropAmountIncrease);
			packet.Write(MinionSlot);
			packet.Write(SentrySlot);
			packet.Write(Thorn);
			packet.Write(CardTracker);
			packet.Write(CardLuck);
			packet.Write(listCursesID.Count);
			foreach (int item in listCursesID)
				packet.Write(item);
			packet.Send(toWho, fromWho);
		}
		public override void SaveData(TagCompound tag) {
			tag["MeleeDMG"] = MeleeDMG;
			tag["RangeDMG"] = RangeDMG;
			tag["MagicDMG"] = MagicDMG;
			tag["SummonDMG"] = SummonDMG;
			tag["Movement"] = Movement;
			tag["JumpBoost"] = JumpBoost;
			tag["HPMax"] = HPMax;
			tag["HPRegen"] = HPRegen;
			tag["ManaMax"] = ManaMax;
			tag["ManaRegen"] = ManaRegen;
			tag["DefenseBase"] = DefenseBase;
			tag["DamagePure"] = DamagePure;
			tag["CritStrikeChance"] = CritStrikeChance;
			tag["CritDamage"] = CritDamage;
			tag["DefenseEffectiveness"] = DefenseEffectiveness;
			tag["DropAmountIncrease"] = DropAmountIncrease;
			tag["MinionSlot"] = MinionSlot;
			tag["SentrySlot"] = SentrySlot;
			tag["Thorn"] = Thorn;
			tag["CardTracker"] = CardTracker;
			tag["CardLuck"] = CardLuck;
			tag.Add("CursesID", listCursesID);
		}
		public override void LoadData(TagCompound tag) {
			MeleeDMG = (float)tag["MeleeDMG"];
			RangeDMG = (float)tag["RangeDMG"];
			MagicDMG = (float)tag["MagicDMG"];
			SummonDMG = (float)tag["SummonDMG"];
			Movement = (float)tag["Movement"];
			JumpBoost = (float)tag["JumpBoost"];
			HPMax = (int)tag["HPMax"];
			HPRegen = (float)tag["HPRegen"];
			ManaMax = (int)tag["ManaMax"];
			ManaRegen = (float)tag["ManaRegen"];
			DefenseBase = (int)tag["DefenseBase"];
			DamagePure = (float)tag["DamagePure"];
			CritStrikeChance = (int)tag["CritStrikeChance"];
			CritDamage = (float)tag["CritDamage"];
			DefenseEffectiveness = (float)tag["DefenseEffectiveness"];
			DropAmountIncrease = (int)tag["DropAmountIncrease"];
			MinionSlot = (int)tag["MinionSlot"];
			SentrySlot = (int)tag["SentrySlot"];
			Thorn = (float)tag["Thorn"];
			CardTracker = (int)tag["CardTracker"];
			if (tag.TryGet<int>("CardLuck", out int cardluck)) {
				CardLuck = cardluck;
			}
			else {
				CardLuck = 0;
			}
			listCursesID = tag.Get<List<int>>("CursesID");
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			MeleeDMG = reader.ReadSingle();
			RangeDMG = reader.ReadSingle();
			MagicDMG = reader.ReadSingle();
			SummonDMG = reader.ReadSingle();
			Movement = reader.ReadSingle();
			JumpBoost = reader.ReadSingle();
			HPMax = reader.ReadInt32();
			HPRegen = reader.ReadSingle();
			ManaMax = reader.ReadInt32();
			ManaRegen = reader.ReadSingle();
			DefenseBase = reader.ReadInt32();
			DamagePure = reader.ReadSingle();
			CritStrikeChance = reader.ReadInt32();
			CritDamage = reader.ReadSingle();
			DefenseEffectiveness = reader.ReadSingle();
			DropAmountIncrease = reader.ReadInt32();
			MinionSlot = reader.ReadInt32();
			SentrySlot = reader.ReadInt32();
			Thorn = reader.ReadSingle();
			CardTracker = reader.ReadInt32();
			CardLuck = reader.ReadInt32();
			listCursesID.Clear();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
				listCursesID.Add(reader.ReadInt32());
		}
		public override void CopyClientState(ModPlayer targetCopy) {
			PlayerCardHandle clone = (PlayerCardHandle)targetCopy;
			clone.MeleeDMG = MeleeDMG;
			clone.RangeDMG = RangeDMG;
			clone.MagicDMG = MagicDMG;
			clone.SummonDMG = SummonDMG;
			clone.Movement = Movement;
			clone.JumpBoost = JumpBoost;
			clone.HPMax = HPMax;
			clone.HPRegen = HPRegen;
			clone.ManaMax = ManaMax;
			clone.ManaRegen = ManaRegen;
			clone.DefenseBase = DefenseBase;
			clone.DamagePure = DamagePure;
			clone.CritStrikeChance = CritStrikeChance;
			clone.CritDamage = CritDamage;
			clone.DefenseEffectiveness = DefenseEffectiveness;
			clone.DropAmountIncrease = DropAmountIncrease;
			clone.MinionSlot = MinionSlot;
			clone.SentrySlot = SentrySlot;
			clone.Thorn = Thorn;
			clone.CardTracker = CardTracker;
			clone.CardLuck = CardLuck;
			clone.listCursesID = listCursesID;
		}
		public override void SendClientChanges(ModPlayer clientPlayer) {
			PlayerCardHandle clone = (PlayerCardHandle)clientPlayer;
			if (MeleeDMG != clone.MeleeDMG
				|| RangeDMG != clone.RangeDMG
				|| MagicDMG != clone.MagicDMG
				|| SummonDMG != clone.SummonDMG
				|| Movement != clone.Movement
				|| JumpBoost != clone.JumpBoost
				|| HPMax != clone.HPMax
				|| HPRegen != clone.HPRegen
				|| ManaMax != clone.ManaMax
				|| ManaRegen != clone.ManaRegen
				|| DefenseBase != clone.DefenseBase
				|| DamagePure != clone.DamagePure
				|| CritStrikeChance != clone.CritStrikeChance
				|| CritDamage != clone.CritDamage
				|| DefenseEffectiveness != clone.DefenseEffectiveness
				|| DropAmountIncrease != clone.DropAmountIncrease
				|| MinionSlot != clone.MinionSlot
				|| SentrySlot != clone.SentrySlot
				|| Thorn != clone.Thorn
				|| CardTracker != clone.CardTracker
				|| CardLuck != clone.CardLuck
				|| ListIsChange) {
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
				ListIsChange = false;
			}
		}
	}
	class CardNPCdrop : GlobalNPC {
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			LeadingConditionRule NightmareMode = new LeadingConditionRule(new NightmareMode());
			NightmareMode.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<PremiumCardPacket>()));
			npcLoot.Add(NightmareMode);
		}
	}
}
