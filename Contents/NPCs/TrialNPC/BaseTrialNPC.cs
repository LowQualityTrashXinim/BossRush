using BossRush.Common.Systems.TrialSystem;
using BossRush.Contents.NPCs.TrialNPC.NPCHeldItem;
using BossRush.Contents.Perks;
using BossRush.Contents.Transfixion.WeaponEnchantment;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.NPCs.TrialNPC;
public abstract class BaseTrialNPC : ModNPC {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public int ItemTypeHold = 0;
	public int AttackCoolDown = 0;
	public bool Attacking = false;
	public float helditemlength = 0;
	public void Attack(int cd, int atkspd) {
		Attacking = true;
		if (AttackCoolDown <= 0) {
			AttackCoolDown = cd + atkspd;
		}
		else {
			return;
		}
		NPC.TargetClosest();
		Player player = Main.player[NPC.target];
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, (player.Center - NPC.Center).SafeNormalize(Vector2.Zero), ItemTypeHold, NPC.damage, 3f, -1, NPC.whoAmI, atkspd);
	}
	public override void ResetEffects() {
		AttackCoolDown = BossRushUtils.CountDown(AttackCoolDown);
	}
	public sealed override void SetDefaults() {
		TrialNPCDefaults();
	}
	public virtual void TrialNPCDefaults() { }
	public sealed override void AI() {
		TrialAI();
	}
	public virtual void TrialAI() { }
	public override void OnKill() {
		int SmokeAmount = Main.rand.Next(30, 41);
		for (int i = 0; i < SmokeAmount; i++) {
			Dust dust = Dust.NewDustDirect(NPC.Center + Main.rand.NextVector2Circular(NPC.width * .5f, NPC.height * .5f), 0, 0, DustID.Smoke);
			dust.noGravity = true;
			dust.velocity = NPC.velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.5f, 1.5f);
		}
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		return base.PreDraw(spriteBatch, screenPos, drawColor);
	}
}
