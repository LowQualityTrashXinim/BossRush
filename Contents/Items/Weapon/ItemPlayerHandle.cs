using Terraria;
using Terraria.ID;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Contents.NPCs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.RelicItem;
using Terraria.Localization;
using BossRush.Common;
using System.Collections.ObjectModel;
using Terraria.UI.Chat;
using ReLogic.Graphics;

namespace BossRush.Contents.Items.Weapon {
	/// <summary>
	///This mod player should hold all the logic require for the item, if the item is shooting out the projectile, it should be doing that itself !<br/>
	///Same with projectile unless it is a vanilla projectile then we can refer to global projectile<br/>
	///This should only hold custom bool or data that we think should be hold/use/transfer<br/>
	/// </summary>
	public class PlayerSynergyItemHandle : ModPlayer {
		public bool SynergyBonusBlock = false;
		public int SynergyBonus = 0;

		public bool BurningPassion_WandofFrosting = false;
		public bool BurningPassion_SkyFracture = false;

		public bool DarkCactus_BatScepter = false;
		public bool DarkCactus_BladeOfGrass = false;

		public bool EnchantedOreSword_StarFury = false;
		public bool EnchantedOreSword_EnchantedSword = false;

		public bool EnchantedStarfury_SkyFacture = false;
		public bool EnchantedStarfury_BreakerBlade = false;

		public bool IceStorm_SnowBallCannon = false;
		public bool IceStorm_FlowerofFrost = false;
		public bool IceStorm_BlizzardStaff = false;

		public bool EnergyBlade_Code1 = false;
		public bool EnergyBlade_Code2 = false;
		public int EnergyBlade_Code1_Energy = 0;

		public bool AmberBoneSpear_MandibleBlade = false;

		public bool Deagle_PhoenixBlaster = false;
		public bool Deagle_DaedalusStormBow = false;
		public int Deagle_DaedalusStormBow_coolDown = 0;
		public bool Deagle_PhoenixBlaster_Critical = false;

		public bool OvergrownMinishark_CrimsonRod = false;
		public bool OvergrownMinishark_DD2ExplosiveTrapT1Popper = false;

		public bool StreetLamp_Firecracker = false;
		public bool StreetLamp_VampireFrogStaff = false;
		public int StreetLamp_VampireFrogStaff_HitCounter = 0;

		public bool OrbOfEnergy_BookOfSkulls = false;
		public bool OrbOfEnergy_DD2LightningAuraT1Popper = false;

		public bool SinisterBook_DemonScythe = false;
		public int SinisterBook_DemonScythe_Counter = 0;

		public bool StarLightDistributer_MeteorArmor = false;
		public bool StarLightDistributer_MagicMissile = false;
		public bool StarlightDistributer_StarCannon = false;

		public bool BloodyShoot_AquaScepter = false;

		public bool RectangleShotgun_QuadBarrelShotgun = false;

		public bool SharpBoomerang_EnchantedBoomerang = false;

		public bool SuperFlareGun_Phaseblade = false;

		public bool QuadDemonBlaster = false;
		public float QuadDemonBlaster_SpeedMultiplier = 1;

		public bool MagicHandCannon_Flamelash = false;

		public bool ZapSnapper_WeatherPain = false;
		public bool ZapSnapper_ThunderStaff = false;

		public bool MagicGrenade_MagicMissle = false;

		public bool DeathBySpark_AleThrowingGlove = false;

		public bool Swotaff_Spear = false;

		public bool NatureSelection_NatureCrystal = false;

		public bool HorusEye_ResonanceScepter = false;
		public override void PreUpdate() {
		}
		public override void ResetEffects() {
			SynergyBonus = 0;
			SynergyBonusBlock = false;

			BurningPassion_WandofFrosting = false;
			BurningPassion_SkyFracture = false;

			DarkCactus_BatScepter = false;
			DarkCactus_BladeOfGrass = false;

			EnchantedOreSword_StarFury = false;
			EnchantedOreSword_EnchantedSword = false;

			EnchantedStarfury_SkyFacture = false;
			EnchantedStarfury_BreakerBlade = false;

			IceStorm_SnowBallCannon = false;
			IceStorm_FlowerofFrost = false;
			IceStorm_BlizzardStaff = false;

			EnergyBlade_Code1 = false;
			EnergyBlade_Code2 = false;

			Swotaff_Spear = false;

			AmberBoneSpear_MandibleBlade = false;

			Deagle_PhoenixBlaster = false;
			Deagle_DaedalusStormBow = false;

			OvergrownMinishark_CrimsonRod = false;
			OvergrownMinishark_DD2ExplosiveTrapT1Popper = false;

			StarLightDistributer_MeteorArmor = false;

			StreetLamp_Firecracker = false;
			StreetLamp_VampireFrogStaff = false;

			OrbOfEnergy_BookOfSkulls = false;
			OrbOfEnergy_DD2LightningAuraT1Popper = false;

			SinisterBook_DemonScythe = false;

			StarLightDistributer_MagicMissile = false;
			StarlightDistributer_StarCannon = false;

			BloodyShoot_AquaScepter = false;

			RectangleShotgun_QuadBarrelShotgun = false;

			SharpBoomerang_EnchantedBoomerang = false;

			SuperFlareGun_Phaseblade = false;

			MagicHandCannon_Flamelash = false;

			ZapSnapper_WeatherPain = false;
			ZapSnapper_ThunderStaff = false;

			MagicGrenade_MagicMissle = false;

			DeathBySpark_AleThrowingGlove = false;

			NatureSelection_NatureCrystal = false;

			HorusEye_ResonanceScepter = false;
		}
	}
	public class GlobalItemHandle : GlobalItem {
		public override bool InstancePerEntity => true;
		public bool LostAccessories = false;
		public bool DebugItem = false;
		public bool ExtraInfo = false;
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if(item.ModItem == null) {
				return;
			}
			if(item.ModItem.Mod != Mod) {
				return;
			}
			if (DebugItem) {
				TooltipLine line = tooltips.Where(t => t.Name == "ItemName").FirstOrDefault();
				line.Text += " [Debug]";
				line.OverrideColor = Color.MediumPurple;
				return;
			}
			if (item.ModItem is Relic relic && relic.relicColor != null) {
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = relic.relicColor.MultiColor(5);
				tooltips.Add(new(Mod, "RelicItem", "[Active Item]") { OverrideColor = Main.DiscoColor});
			}
			ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
			if (ExtraInfo && item.ModItem != null) {
				if (!moddedplayer.Hold_Shift) {
					tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Hold shift for more infomation]") { OverrideColor = Color.Gray });
				}
			}
			if (item.accessory && LostAccessories) {
				TooltipLine line_Name = tooltips.Where(t => t.Name == "ItemName").FirstOrDefault();
				TooltipLine line_Tooltip0 = tooltips.Where(t => t.Name == "Tooltip0").FirstOrDefault();
				if (line_Name == null || line_Tooltip0 == null) {
					return;
				}
				line_Name.Text = Language.GetTextValue($"Mods.BossRush.LostAccessories.{item.ModItem.Name}.DisplayName");
				line_Tooltip0.Text = Language.GetTextValue($"Mods.BossRush.LostAccessories.{item.ModItem.Name}.Tooltip");
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = Color.DarkGoldenrod;
				tooltips.Add(new TooltipLine(Mod, "LostAcc_" + item.type, "Lost Accessory") { OverrideColor = Color.LightGoldenrodYellow });
			}
		}
		public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y) {
			if (item.ModItem == null) {
				return true;
			}
			if (item.ModItem.Mod != Mod) {
				return true;
			}
			ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
			if (ExtraInfo && item.ModItem != null)
				if (moddedplayer.Hold_Shift) {
					float width;
					float height = -16;
					Vector2 pos;

					string value = $"Mods.BossRush.Items.{item.ModItem.Name}.ExtraInfo";
					string ExtraInfo = Language.GetTextValue(value);

					DynamicSpriteFont font = FontAssets.MouseText.Value;

					if (Main.MouseScreen.X < Main.screenWidth / 2) {
						string widest = lines.OrderBy(n => ChatManager.GetStringSize(font, n.Text, Vector2.One).X).Last().Text;
						width = ChatManager.GetStringSize(font, widest, Vector2.One).X;

						pos = new Vector2(x, y) + new Vector2(width + 30, 0);
					}
					else {
						width = ChatManager.GetStringSize(font, ExtraInfo, Vector2.One).X + 20;

						pos = new Vector2(x, y) - new Vector2(width + 30, 0);
					}

					width = ChatManager.GetStringSize(font, ExtraInfo, Vector2.One).X + 20;

					height += ChatManager.GetStringSize(font, ExtraInfo, Vector2.One).Y + 16;


					Utils.DrawInvBG(Main.spriteBatch, new Rectangle((int)pos.X - 10, (int)pos.Y - 10, (int)width + 20, (int)height + 20), new Color(25, 100, 55) * 0.85f);


					Utils.DrawBorderString(Main.spriteBatch, ExtraInfo, pos, Color.White);
					pos.Y += ChatManager.GetStringSize(font, ExtraInfo, Vector2.One).Y + 16;
				}
			return base.PreDrawTooltip(item, lines, ref x, ref y);
		}
	}
	public abstract class SynergyModItem : ModItem {
		public override void SetStaticDefaults() {
			ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<SynergyEnergy>();
			CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(50, 100, 100) });
		}
		public ColorInfo CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(100, 150, 150) });
		public override sealed void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			ModifySynergyToolTips(ref tooltips, Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>());
			if (CustomColor != null) {
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = CustomColor.MultiColor(5);
			}
		}
		public override void ModifyWeaponCrit(Player player, ref float crit) {
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			crit += 4 * modplayer.SynergyBonus;
		}
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage) {
			float damageIncreasement = 0;
			float damageMultiplier = 0;
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (modplayer.SynergyBonus > 0) {
				damageMultiplier += 0.025f * modplayer.SynergyBonus;
			}
			else {
				damageMultiplier += 0.01f;
			}
			for (int i = 0; player.inventory.Length > 0; i++) {
				if (i > 50) {
					break;
				}
				Item item = player.inventory[i];
				if (!item.IsAWeapon() || item == Item || item.ModItem is SynergyModItem) {
					continue;
				}
				damageIncreasement += player.inventory[i].damage * damageMultiplier;
			}
			damage += damageIncreasement;
		}
		public virtual void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) { }
		public override sealed void HoldItem(Player player) {
			base.HoldItem(player);
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (modplayer.SynergyBonusBlock) {
				return;
			}
		}
		public override sealed void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
			ModifySynergyShootStats(player, player.GetModPlayer<PlayerSynergyItemHandle>(), ref position, ref velocity, ref type, ref damage, ref knockback);
		}
		public override sealed void UpdateInventory(Player player) {
			base.UpdateInventory(player);
			//Very funny that hold item happen after ModifyWeaponDamage
			//This probably will tank our mod performance, but well, it is what it is
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (player.HeldItem == Item && !modplayer.SynergyBonusBlock) {
				HoldSynergyItem(player, modplayer);
			}
			SynergyUpdateInventory(player, modplayer);
		}
		public virtual void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {

		}
		public virtual void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

		}
		/// <summary>
		/// You should use this to set condition, the condition must be pre set in <see cref="PlayerSynergyItemHandle"/> and then check condition in here
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			SynergyShoot(player, player.GetModPlayer<PlayerSynergyItemHandle>(), source, position, velocity, type, damage, knockback, out bool CanShootItem);
			return CanShootItem;
		}
		public virtual void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) { CanShootItem = true; }
		public override sealed void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			base.OnHitNPC(player, target, hit, damageDone);
			OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
		}
		public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) { }

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
			switch (Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus) {
				case 1:
					auraColor = new Color(255, 50, 0, 30);
					break;
				case 2:
					auraColor = new Color(255, 255, 0, 30);
					break;
				case 3:
					auraColor = new Color(0, 255, 255, 30);
					break;
				default:
					auraColor = new Color(255, 255, 255, 30);
					break;
			}
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			ColorHandle();
			if (ItemID.Sets.AnimatesAsSoul[Type] || Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus < 1) {
				return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
			}
			Main.instance.LoadItem(Item.type);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(1.5f, 1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(1.5f, -1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, 1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, -1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
	public abstract class SynergyModProjectile : ModProjectile {
		public virtual void SpawnDustPostPostAI(Player player) { }
		public override sealed bool PreAI() {
			Player player = Main.player[Projectile.owner];
			SynergyPreAI(player, player.GetModPlayer<PlayerSynergyItemHandle>(), out bool stopAI);
			return stopAI;
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		/// <param name="runAI"></param>
		public virtual void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) { runAI = true; }
		public override sealed void AI() {
			Player player = Main.player[Projectile.owner];
			SynergyAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed void PostAI() {
			Player player = Main.player[Projectile.owner];
			SynergyPostAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
			SpawnDustPostPostAI(player);
		}
		/// <summary>
		/// You should check the condition yourself
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void SynergyPostAI(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			Player player = Main.player[Projectile.owner];
			ModifyHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, ref modifiers);
		}
		public virtual void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) { }
		public override sealed void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Player player = Main.player[Projectile.owner];
			OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
		}
		public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) { }
		public override sealed void OnKill(int timeLeft) {
			base.OnKill(timeLeft);
			Player player = Main.player[Projectile.owner];
			SynergyKill(player, player.GetModPlayer<PlayerSynergyItemHandle>(), timeLeft);
		}
		public virtual void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		}
	}
	public abstract class SynergyBuff : ModBuff {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override sealed void SetStaticDefaults() {
			base.SetStaticDefaults();
			SynergySetStaticDefaults();
		}
		public virtual void SynergySetStaticDefaults() {

		}
		public override sealed void Update(Player player, ref int buffIndex) {
			base.Update(player, ref buffIndex);
			UpdatePlayer(player, ref buffIndex);
		}
		public virtual void UpdatePlayer(Player player, ref int buffIndex) {

		}
		public override sealed void Update(NPC npc, ref int buffIndex) {
			base.Update(npc, ref buffIndex);
			UpdateNPC(npc, ref buffIndex);
		}
		public virtual void UpdateNPC(NPC npc, ref int buffIndex) {

		}
	}
	public class SynergyModSystem : ModSystem {
		public bool GodAreEnraged = false;
		public int CooldownCheck = 999;
		private void SynergyEnergyCheckPlayer(Player player) {
			int synergyCounter = 0;
			synergyCounter += player.CountItem(ModContent.ItemType<SynergyEnergy>(), 2);
			synergyCounter += player.inventory.Where(itemInv => itemInv.ModItem is SynergyModItem).Count();
			int maxCount = NPC.GetActivePlayerCount() + 1;
			if (synergyCounter >= maxCount) {
				GodAreEnraged = true;
			}
		}
		private void GodDecision(Player player) {
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			if (NPC.AnyNPCs(ModContent.NPCType<Guardian>()) || player.GetModPlayer<ChestLootDropPlayer>().CanDropSynergyEnergy)
				return;
			if (player.IsDebugPlayer())
				return;
			CooldownCheck = BossRushUtils.CountDown(CooldownCheck);
			//Main.NewText(CooldownCheck);
			if (CooldownCheck <= 0) {
				SynergyEnergyCheckPlayer(player);
			}
			if (GodAreEnraged) {
				Vector2 randomSpamLocation = Main.rand.NextVector2CircularEdge(1500, 1500) + player.Center;
				NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)randomSpamLocation.X, (int)randomSpamLocation.Y, ModContent.NPCType<Guardian>());
				BossRushUtils.CombatTextRevamp(player.Hitbox, Color.Red, "You have anger the God!");
				CooldownCheck = 999;
				GodAreEnraged = false;
			}
		}
		public override void PostUpdateWorld() {
		}
	}
}
