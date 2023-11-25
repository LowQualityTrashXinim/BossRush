using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.Trinket;
public abstract class BaseTrinket : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public virtual void UpdateTrinket(Player player, TrinketPlayer modplayer) { }
	public sealed override void UpdateEquip(Player player) {
		UpdateTrinket(player, player.GetModPlayer<TrinketPlayer>());
	}
}
//This will store all the information about the trinket and how they will interact with player
public class TrinketPlayer : ModPlayer {
	public StatModifier HPstats = StatModifier.Default;
	public StatModifier ManaStats = StatModifier.Default;
	public StatModifier DamageStats = StatModifier.Default;
	public override void Initialize() {
		HPstats = StatModifier.Default;
		ManaStats = StatModifier.Default;
		DamageStats = StatModifier.Default;
	}
	public override void ResetEffects() {
		HPstats = StatModifier.Default;
		ManaStats = StatModifier.Default;
		DamageStats = StatModifier.Default;
	}
	public int counterToFullPi = 0;
	public override void PreUpdate() {
		if (++counterToFullPi >= 360)
			counterToFullPi = 0;
	}
	public override void PostUpdate() {
	}
	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
		base.ModifyMaxStats(out health, out mana);
		health = health.CombineWith(HPstats);
		mana = mana.CombineWith(ManaStats);
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		damage = damage.CombineWith(DamageStats);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		base.OnHitByNPC(npc, hurtInfo);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		base.OnHitByProjectile(proj, hurtInfo);
	}
}
public class Trinket_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int Trinket_of_Perpetuation_PointStack = 0;
}
public abstract class TrinketBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public sealed override void SetStaticDefaults() {
		TrinketSetStaticDefaults();
	}
	public virtual void TrinketSetStaticDefaults() { }
	public sealed override void Update(NPC npc, ref int buffIndex) {
		base.Update(npc, ref buffIndex);
		UpdateTrinketNPC(npc);
		if (npc.buffTime[buffIndex] <= 0) {
			OnEnded(npc);
		}
	}
	public virtual void UpdateTrinketNPC(NPC npc) { }
	public sealed override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
		UpdateTrinketPlayer(player, player.GetModPlayer<TrinketPlayer>(), ref buffIndex);
		if (player.buffTime[buffIndex] <= 0) {
			OnEnded(player);
		}
	}
	public virtual void OnEnded(Player player) { }
	public virtual void OnEnded(NPC npc) { }
	public virtual void UpdateTrinketPlayer(Player player, TrinketPlayer modplayer, ref int buffIndex) { }
}
