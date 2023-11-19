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
	public virtual void UpdateTrinket(Player player, Trinketplayer modplayer) { }
	public sealed override void UpdateEquip(Player player) {
		base.UpdateEquip(player);
		UpdateTrinket(player, player.GetModPlayer<Trinketplayer>());
	}
}
//This will store all the information about the trinket and how they will interact with player
public class Trinketplayer : ModPlayer {
	public StatModifier HPstats;
	public StatModifier ManaStats;
	public StatModifier DamageStats;
	public override void ResetEffects() {
		Trinket1 = false;
		Trinket_of_Perpetuation = false;
		HPstats = default;
		ManaStats = default;
		DamageStats = new StatModifier();
	}
	public override void PreUpdate() {
		if (!Player.HasBuff(ModContent.BuffType<SwiftSteal_Buff>())) {
			Trinket1_Point = 0;
			Trinket1_Delay = BossRushUtils.CoolDown(Trinket1_Delay);
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
		Trinket1Effect();
		Trinket_of_Perpetuation_Effect(target, hit);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket1Effect();
		Trinket_of_Perpetuation_Effect(target, hit);
	}
	public bool Trinket1 = false;
	public int Trinket1_Point = 0;
	public int Trinket1_Delay = 0;
	private void Trinket1Effect() {
		if (!Trinket1)
			return;
		if (Player.HasBuff(ModContent.BuffType<SwiftSteal_Buff>())) {
			Trinket1_Point = Math.Clamp(++Trinket1_Point, 0, 6);
		}
		else {
			if (Trinket1_Delay > 0) {
				return;
			}
			Player.AddBuff(ModContent.BuffType<SwiftSteal_Buff>(), 900);
		}
	}
	public bool Trinket_of_Perpetuation = false;
	private void Trinket_of_Perpetuation_Effect(NPC target, NPC.HitInfo hit) {
		if (!Trinket_of_Perpetuation)
			return;
		target.AddBuff(ModContent.BuffType<Samsara_of_Retribution>(), 60);
		if (hit.Crit) {
			NPC.HitInfo hitExtra = hit;
			hitExtra.Crit = false;
			hitExtra.Damage += (int)(hitExtra.Damage * target.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack * .1f);
			Player.StrikeNPCDirect(target,hitExtra);
		}
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
		UpdateTrinketPlayer(player, player.GetModPlayer<Trinketplayer>(), ref buffIndex);
		if (player.buffTime[buffIndex] <= 0) {
			OnEnded(player, player.GetModPlayer<Trinketplayer>());
		}
	}
	public virtual void OnEnded(Player player, Trinketplayer modplayer) { }
	public virtual void OnEnded(NPC npc) { }
	public virtual void UpdateTrinketPlayer(Player player, Trinketplayer modplayer, ref int buffIndex) { }
}
