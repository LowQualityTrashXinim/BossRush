using BossRush.Texture;
using System;
using System.Collections.Generic;
using Terraria;
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
		DamageStats = default;
	}
	public override void PreUpdate() {
		if (!Player.HasBuff(ModContent.BuffType<Trinket1_Buff>())) {
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
		damage.CombineWith(DamageStats);
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
	int Trinket1_Delay = 0;
	private void Trinket1Effect() {
		if (!Trinket1)
			return;
		if (Player.HasBuff(ModContent.BuffType<Trinket1_Buff>())) {
			Trinket1_Point = Math.Clamp(++Trinket1_Point, 0, 6);
			Trinket1_Delay = 1500;
			DamageStats.Base += Player.statLife * .05f;
		}
		else {
			if (Trinket1_Delay > 0) {
				return;
			}
			Player.AddBuff(ModContent.BuffType<Trinket1_Buff>(), 900);
		}
	}
	public bool Trinket_of_Perpetuation = false;
	private void Trinket_of_Perpetuation_Effect(NPC target, NPC.HitInfo hit) {
		if (!Trinket_of_Perpetuation)
			return;
		if(hit.Crit && target.HasBuff(ModContent.BuffType<Samsara_of_Retribution>())) {
			target.Center.LookForHostileNPC(out List<NPC> npclist, 300);
			foreach (NPC npc in npclist) {
				npc.AddBuff(ModContent.BuffType<Samsara_of_Retribution>(), 240);
			}
		}
		target.AddBuff(ModContent.BuffType<Samsara_of_Retribution>(), 60);
	}
}
public class Trinket_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int Trinket_of_Perpetuation_PointStack = 0;
	public override void OnKill(NPC npc) {
		if(npc.HasBuff(ModContent.BuffType<Samsara_of_Retribution>())) {

		}
	}
}
public abstract class TrinketBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public sealed override void SetStaticDefaults() {
		TrinketSetStaticDefaults();
	}
	public virtual void TrinketSetStaticDefaults() { }
	public override void Update(NPC npc, ref int buffIndex) {
		base.Update(npc, ref buffIndex);
	}
	public sealed override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
		UpdateTrinketPlayer(player, player.GetModPlayer<Trinketplayer>(), ref buffIndex);
	}
	public virtual void UpdateTrinketPlayer(Player player, Trinketplayer modplayer, ref int buffIndex) { }
}
