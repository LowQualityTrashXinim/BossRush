using BossRush.Common.Systems.TrialSystem;
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
	public void Attack(int cd) {
		Attacking = true;
		AttackCoolDown = cd;

	}
	public override void ResetEffects() {
		Attacking = false;
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
		if (Attacking) {
			ModHeldItem helditem = HeldItemModSystem.GetHeldItem(ItemTypeHold);
			if (helditem != null) {
				helditem.DrawItem(spriteBatch, screenPos, Vector2.Zero);
			}
		}
		return base.PreDraw(spriteBatch, screenPos, drawColor);
	}
}
public class HeldItemModSystem : ModSystem {
	private static readonly List<ModHeldItem> _heldItem = new();
	public static readonly List<int> _heldItemID = new();
	public static int TotalCount => _heldItem.Count;
	public static int Register(ModHeldItem helditem) {
		ModTypeLookup<ModHeldItem>.Register(helditem);
		_heldItem.Add(helditem);
		_heldItemID.Add(helditem.ItemType);
		return _heldItem.Count - 1;
	}
	public static ModHeldItem GetHeldItem(int type) {
		return type >= 0 && type < _heldItem.Count ? _heldItem[type] : null;
	}
	public static ModHeldItem GetHeldItemID(int ItemID) {
		int index = _heldItemID.IndexOf(ItemID);
		return index >= 0 && index < _heldItemID.Count ? _heldItem[index] : null;
	}
}
public abstract class ModHeldItem : ModType {
	public int Type = 0;
	public int ItemType = 0;
	public bool IsASword = false;
	protected sealed override void Register() {
		Type = HeldItemModSystem.Register(this);
	}
	public static int GetType<T>() where T : ModHeldItem {
		return ModContent.GetInstance<T>().Type;
	}
	public override void SetStaticDefaults() {
		base.SetStaticDefaults();
	}
	public virtual void Shoot(IEntitySource source, Vector2 position, Vector2 velocity, int damage, int knockback) { }
	public virtual void DrawItem(SpriteBatch spriteBatch, Vector2 position, Vector2 offset) { }
}
