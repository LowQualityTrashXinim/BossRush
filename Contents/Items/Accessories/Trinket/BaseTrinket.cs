using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.Trinket;
//TODO : in the future, there are chance that trinket will get even more connect to each other or there are something that will affect all trinket
//and because of so,this base class is create to handle that problem, but at the moment, there are no need for this
//but still, this is planned in the future
public abstract class BaseTrinket : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public virtual void UpdateTrinket(Player player, TrinketPlayer modplayer) { }
	public sealed override void UpdateEquip(Player player) {
		base.UpdateEquip(player);
		UpdateTrinket(player, player.GetModPlayer<TrinketPlayer>());
	}
}
//This will store all the information about the trinket and how they will interact with player
public class TrinketPlayer : ModPlayer {
	public StatModifier HPstats;
	public StatModifier ManaStats;
	public StatModifier DamageStats;
	public override void ResetEffects() {
		Trinket_of_Swift_Health = false;
		Trinket_of_Perpetuation = false;
		HPstats = new StatModifier();
		ManaStats = new StatModifier();
		DamageStats = new StatModifier();
	}
	public override void PreUpdate() {
		Trinket_of_Swift_Health_DelayBetweenEachHit = BossRushUtils.CoolDown(Trinket_of_Swift_Health_DelayBetweenEachHit);
		if (!Player.HasBuff(ModContent.BuffType<SwiftSteal_Buff>())) {
			Trinket_of_Swift_Health_PointCounter = 0;
			Trinket_of_Swift_Health_CoolDown = BossRushUtils.CoolDown(Trinket_of_Swift_Health_CoolDown);
		}
		Trinket3_PointTimeLeft = BossRushUtils.CoolDown(Trinket3_PointTimeLeft);
		Trinket3_CoolDown = BossRushUtils.CoolDown(Trinket3_CoolDown);
		if (Trinket3_PointTimeLeft <= 0 && Trinket3_PointCounter > 0) {
			Trinket3_PointCounter--;
			Trinket3_PointTimeLeft = BossRushUtils.ToSecond(7);
		}
	}
	public override void PostUpdate() {
	}
	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
		base.ModifyMaxStats(out health, out mana);
		health.CombineWith(HPstats);
		mana.CombineWith(ManaStats);
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		damage = damage.CombineWith(DamageStats);
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Swift_Health_OnHitEffect();
		Trinket_of_Perpetuation_OnHitNPCEffect(target, hit);
		Trinket3_OnHitNPCEffect(hit);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Swift_Health_OnHitEffect();
		Trinket_of_Perpetuation_OnHitNPCEffect(target, hit);
		Trinket3_OnHitNPCEffect(hit);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		base.OnHitByNPC(npc, hurtInfo);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		base.OnHitByProjectile(proj, hurtInfo);
	}
	public bool Trinket_of_Swift_Health = false;
	public int Trinket_of_Swift_Health_PointCounter = 0;
	public int Trinket_of_Swift_Health_CoolDown = 0;
	public int Trinket_of_Swift_Health_DelayBetweenEachHit = 0;
	private void Trinket_of_Swift_Health_OnHitEffect() {
		if (Trinket_of_Swift_Health_DelayBetweenEachHit > 0)
			return;
		if (Trinket_of_Swift_Health_CoolDown > 0) {
			return;
		}
		if (!Trinket_of_Swift_Health)
			return;
		if (Player.HasBuff(ModContent.BuffType<SwiftSteal_Buff>())) {
			Trinket_of_Swift_Health_DelayBetweenEachHit = BossRushUtils.ToSecond(2.5f);
			Trinket_of_Swift_Health_PointCounter = Math.Clamp(++Trinket_of_Swift_Health_PointCounter, 0, 6);
		}
		else {
			Player.AddBuff(ModContent.BuffType<SwiftSteal_Buff>(), BossRushUtils.ToSecond(30));
		}
	}

	public bool Trinket_of_Perpetuation = false;
	private void Trinket_of_Perpetuation_OnHitNPCEffect(NPC target, NPC.HitInfo hit) {
		if (!Trinket_of_Perpetuation)
			return;
		target.AddBuff(ModContent.BuffType<Samsara_of_Retribution>(), BossRushUtils.ToSecond(1));
		if (hit.Crit) {
			NPC.HitInfo hitExtra = hit;
			hitExtra.Crit = false;
			hitExtra.Damage += (int)(hitExtra.Damage * target.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack * .1f);
			Player.StrikeNPCDirect(target, hitExtra);
		}
	}

	public bool Trinket3 = false;
	public int Trinket3_PointCounter = 0;
	public int Trinket3_PointTimeLeft = 0;
	public int Trinket3_CoolDown = 0;
	private void Trinket3_OnHitNPCEffect(NPC.HitInfo hit) {
		if (!hit.Crit)
			return;
		if (Trinket3_CoolDown > 0)
			return;
		Trinket3_PointCounter = Math.Clamp(++Trinket3_PointCounter, 0, 4);
		Trinket3_PointTimeLeft = BossRushUtils.ToSecond(7);
		Trinket3_CoolDown = BossRushUtils.ToSecond(2);
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
			OnEnded(player, player.GetModPlayer<TrinketPlayer>());
		}
	}
	public virtual void OnEnded(Player player, TrinketPlayer modplayer) { }
	public virtual void OnEnded(NPC npc) { }
	public virtual void UpdateTrinketPlayer(Player player, TrinketPlayer modplayer, ref int buffIndex) { }
}
