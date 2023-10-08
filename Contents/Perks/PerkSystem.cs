using BossRush.Contents.Items.NohitReward;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace BossRush.Contents.Perks {
	class PerkUISystem : ModSystem {
		public UserInterface userInterface;
		public PerkUIState perkUIstate;
		public override void Load() {
			if (!Main.dedServ) {
				perkUIstate = new();
				userInterface = new();
			}
		}
		public override void UpdateUI(GameTime gameTime) {
			userInterface?.Update(gameTime);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1) {
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"BossRush: PerkSystem",
					delegate {
						userInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
	public class PerkItem : GlobalItem {
		public override bool? UseItem(Item item, Player player) {
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (player.ItemAnimationJustStarted)
				perkplayer.PotionExpert_perk_CanConsume = Main.rand.NextFloat() <= .35f;
			if (perkplayer.perk_PotionExpert && item.buffType > 0) {
				return perkplayer.PotionExpert_perk_CanConsume;
			}
			return base.UseItem(item, player);
		}
	}
	public class PerkPlayer : ModPlayer {
		public bool CanGetPerk = false;
		public int PerkAmount = 3;
		/// <summary>
		/// Keys : Perk type<br/>
		/// Values : Stack value
		/// </summary>
		public Dictionary<int, int> perks = new Dictionary<int, int>();

		public bool perk_PotionExpert = false;
		public bool PotionExpert_perk_CanConsume = false;

		private int[] _perks;
		public override void OnEnterWorld() {
			PerkUISystem uiSystemInstance = ModContent.GetInstance<PerkUISystem>();
			uiSystemInstance.userInterface.SetState(null);
		}
		public override void Initialize() {
			_perks = new int[ModPerkLoader.TotalCount];
			perks = new Dictionary<int, int>();
		}
		public int PerkAmountModified() {
			if (perks.ContainsKey(Perk.GetPerkType<BlessingOfPerk>())) {
				return PerkAmount + perks[Perk.GetPerkType<BlessingOfPerk>()];
			}
			return PerkAmount;
		}
		public bool HasPerk<T>() where T : Perk => _perks[Perk.GetPerkType<T>()] > 0;
		public bool HasPerk(Perk perk) => _perks[perk.Type] > 0;
		public override void ResetEffects() {
			perk_PotionExpert = false;
			PerkAmount = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count + 3;
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				ModPerkLoader.GetPerk(i).StackAmount = 0;
			}
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).ResetEffect(Player);
				ModPerkLoader.GetPerk(perk).StackAmount = perks[perk];
			}
		}
		public override void PostUpdateEquips() {
			foreach (int perk in perks.Keys) {
				ModPerkLoader.GetPerk(perk).UpdateEquip(Player);
			}
		}
		public override bool CanUseItem(Item item) {
			PerkUISystem uiSystemInstance = ModContent.GetInstance<PerkUISystem>();
			if (uiSystemInstance.userInterface.CurrentState is not null) {
				return false;
			}
			return base.CanUseItem(item);
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
		public override void SaveData(TagCompound tag) {
			tag["PlayerPerks"] = perks.Keys.ToList();
			tag["PlayerPerkStack"] = perks.Values.ToList();
		}
		public override void LoadData(TagCompound tag) {
			var PlayerPerks = tag.Get<List<int>>("PlayerPerks");
			var PlayerPerkStack = tag.Get<List<int>>("PlayerPerkStack");
			perks = PlayerPerks.Zip(PlayerPerkStack, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
		}
	}

	public abstract class Perk : ModType {
		public bool CanBeStack = false;
		/// <summary>
		/// Use this if <see cref="CanBeStack"/> is true
		/// <br/> This allow easy multiply
		/// </summary>
		public int StackAmount = 0;
		public int StackLimit = 1;
		/// <summary>
		/// Please set this texture string as if you are setting <see cref="ModItem.Texture"/>
		/// </summary>
		public string textureString = null;
		public string Tooltip = null;

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
			return Tooltip;
		}
		public virtual string ModifyName() {
			return PerkName();
		}
		public string PerkName() {
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
		public virtual void SetDefaults() {

		}
		public virtual void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

		}
		public virtual void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {

		}
		/// <summary>
		/// This will run in <see cref="ModPlayer.PostUpdate"/>
		/// </summary>
		public virtual void Update(Player player) {

		}
		public virtual void UpdateEquip(Player player) {

		}
		public virtual void ResetEffect(Player player) {

		}
		public virtual void OnMissingMana(Player player, Item item, int neededMana) {

		}
		public virtual void ModifyDamage(Player player, Item item, ref StatModifier damage) {

		}
		public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {

		}
		public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {

		}
		public virtual void OnHitByAnything(Player player) { }
		public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
		public virtual void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) { }
		public virtual void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) { }
		public virtual void ModifyItemScale(Player player, Item item, ref float scale) { }
		public virtual void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) { }
		/// <summary>
		/// Subtract will make player use weapon slower
		/// Additive will make player use weapon faster
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <param name="useSpeed">by default start at 1</param>
		public virtual void ModifyUseSpeed(Player player, Item item, ref float useSpeed) { }
		public virtual void OnChoose(Player player) { }
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
	class PerkChooser : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(32, 23);
			Item.maxStack = 999;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool? UseItem(Player player) {
			PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
			if (player.altFunctionUse != 2) {
				PerkUISystem uiSystemInstance = ModContent.GetInstance<PerkUISystem>();
				uiSystemInstance.perkUIstate.whoAmI = player.whoAmI;
				uiSystemInstance.perkUIstate.StateofState = PerkUIState.DefaultState;
				uiSystemInstance.userInterface.SetState(uiSystemInstance.perkUIstate);
			}
			else if (player.IsDebugPlayer()) {
				modplayer.perks.Clear();
			}
			return true;
		}
	}
	class StarterPerkChooser : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(32, 23);
			Item.maxStack = 999;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool? UseItem(Player player) {
			PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
			if (player.altFunctionUse != 2) {
				PerkUISystem uiSystemInstance = ModContent.GetInstance<PerkUISystem>();
				uiSystemInstance.perkUIstate.whoAmI = player.whoAmI;
				uiSystemInstance.perkUIstate.StateofState = PerkUIState.StarterPerkState;
				uiSystemInstance.userInterface.SetState(uiSystemInstance.perkUIstate);
			}
			else if (player.IsDebugPlayer()) {
				modplayer.perks.Clear();
			}
			return true;
		}
	}
}