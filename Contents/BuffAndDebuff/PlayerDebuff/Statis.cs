using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Texture;

namespace BossRush.Contents.BuffAndDebuff.PlayerDebuff;
internal class Stasis : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = false;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<StasisPlayer>().IsInStasis = true;
	}
}
class StasisPlayer : ModPlayer {
	public bool IsInStasis = false;
	Vector2 lastpos = Vector2.Zero;
	int lasthealth = 0;
	public override void ResetEffects() {
		IsInStasis = false;
	}
	public override void PostUpdate() {
		if (IsInStasis) {
			if (lastpos == Vector2.Zero) {
				lastpos = Player.Center;
			}
			if(lasthealth == 0) {
				lasthealth = Player.statLife;
			}
			Player.Center = lastpos;
			Player.statLife = lasthealth;
		}
		else {
			lastpos = Vector2.Zero;
			lasthealth = 0;
		}
	}
	public override void SetControls() {
		if (IsInStasis) {
			Player.controlInv = false;
			Player.controlMap = false;
			Player.controlMount = false;
			Player.controlQuickHeal = false;
			Player.controlQuickMana = false;
			Player.controlHook = false;
			Player.controlTorch = false;
			Player.controlSmart = false;
		}
	}
	public override bool CanBeHitByProjectile(Projectile proj) {
		if (IsInStasis) {
			return false;
		}
		return base.CanBeHitByProjectile(proj);
	}
	public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) {
		if (IsInStasis) {
			return false;
		}
		return base.CanBeHitByNPC(npc, ref cooldownSlot);
	}
	public override bool CanUseItem(Item item) {
		if (IsInStasis) {
			return false;
		}
		return base.CanUseItem(item);
	}
}

