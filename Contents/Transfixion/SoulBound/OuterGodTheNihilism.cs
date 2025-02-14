using BossRush.Common.RoguelikeChange;
using BossRush.Common.Systems;
using BossRush.Contents.Transfixion.Arguments;
using BossRush.Contents.Transfixion.SoulBound.SoulBoundMaterial;
using BossRush.Texture;
using Humanizer;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Transfixion.SoulBound;
internal class OuterGodTheNihilism : ModSoulBound {
	public override void SetStaticDefaults() {
		Rarity = SoulBoundLoader.Primordial;
	}
	public override string ModifiedToolTip(Item item) {
		int level = GetLevel(item);
		return Description.FormatWith(new string[] {
			Math.Round((.2f + .05f * level) * 100).ToString(),
			Math.Round((2f + .2f * level) * 100).ToString(),
			(20 + 5 * level).ToString(),
			Math.Round((.05f + .02f * level) * 100).ToString(),
			Math.Round((.5f + .1f * level) * 100).ToString(),
			Math.Round(2 + .2f * level,1).ToString(),
			Math.Round((.005f + .001f * level) * 100,1).ToString(),
		});
	}
	public override void MeleeEffect(Player player, Item acc, Item item, Rectangle hitbox) {
		if (Main.rand.NextFloat() >= .05f + .02f * GetLevel(acc)) {
			Projectile.NewProjectile(player.GetSource_ItemUse(item), Main.rand.NextVector2FromRectangle(hitbox), Vector2.Zero, ModContent.ProjectileType<VoidTrail>(), 1, 4f, player.whoAmI, 20 + 5 * GetLevel(acc));
		}
	}
	public override void Shoot(Player player, Item acc, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextFloat() >= .05f + .02f * GetLevel(acc)) {
			Projectile.NewProjectile(player.GetSource_ItemUse(item), position, velocity, ModContent.ProjectileType<VoidTrail>(), 1, 4f, player.whoAmI, 20 + 5 * GetLevel(acc));
		}
	}
	public override void UpdateEquip(Player player, Item item) {
		if (player.immune) {
			if (Main.rand.NextBool(10)) {
				Projectile.NewProjectile(player.GetSource_ItemUse(item), Main.rand.NextVector2FromRectangle(player.Hitbox), Vector2.Zero, ModContent.ProjectileType<VoidParticle>(), 1, 4f, player.whoAmI, 20 + 5 * GetLevel(item));
			}
		}
		if (player.ItemAnimationActive && player.itemAnimation == player.itemAnimationMax / 2) {
			if (Main.rand.NextFloat() >= .005f + .001f * GetLevel(item)) {
				NPC npc = NPC.NewNPCDirect(Entity.GetSource_NaturalSpawn(), player.Center + Main.rand.NextVector2CircularEdge(1000, 1000), NPCID.Wraith);
				npc.defense += 30;
				npc.lifeMax *= 3;
				npc.life = npc.lifeMax;
				npc.GetGlobalNPC<RoguelikeGlobalNPC>().static_velocityMultiplier += .35f;
			}
		}
	}
	public override void ModifyHitNPCWithItem(Player player, Item acc, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += .5f + .1f * GetLevel(acc);
		player.AddBuff(ModContent.BuffType<Vulnerable>(), BossRushUtils.ToSecond(2 + .2f * GetLevel(acc)));
	}
	public override void ModifyHitNPCWithProj(Player player, Item acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage += .5f + .1f * GetLevel(acc);
		player.AddBuff(ModContent.BuffType<Vulnerable>(), BossRushUtils.ToSecond(2 + .2f * GetLevel(acc)));
	}
	public override void OnHitByNPC(Player player, Item acc, NPC npc, Player.HurtInfo info) {
		if (player.HeldItem.IsAWeapon(true)) {
			Item item = player.HeldItem;

			NPC.HitInfo hitinfo = new();
			hitinfo.Damage = (int)(player.GetWeaponDamage(item) * (.6f + .05f * GetLevel(acc)));
			hitinfo.HitDirection = BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X);
			hitinfo.Crit = Main.rand.Next(1, 101) >= player.GetWeaponCrit(item);
			hitinfo.Knockback = player.GetWeaponKnockback(item);
			player.StrikeNPCDirect(npc, hitinfo);
		}
	}
	public override bool FreeDodge(Player player, Player.HurtInfo info, Item acc) {
		if (Main.rand.NextFloat() >= .2f + .05f * GetLevel(acc)) {
			if (player.HeldItem.IsAWeapon(true)) {
				Item item = player.HeldItem;
				if (player.Center.LookForHostileNPC(out NPC npc, 9999)) {
					NPC.HitInfo hitinfo = new();
					hitinfo.Damage = (int)(player.GetWeaponDamage(item) * (2f + .2f * GetLevel(acc)));
					hitinfo.HitDirection = BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X);
					hitinfo.Crit = Main.rand.Next(1, 101) >= player.GetWeaponCrit(item);
					hitinfo.Knockback = player.GetWeaponKnockback(item);
					player.StrikeNPCDirect(npc, hitinfo);
				}
			}
			player.AddImmuneTime(info.CooldownCounter, 44);
		}
		return base.FreeDodge(player, info, acc);
	}
}
public class OuterGodTheNihilismItem : BaseSoulBoundItem {
	public override short SoulBoundType => ModSoulBound.GetSoulBoundType<OuterGodTheNihilism>();
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SoulofSight, 77)
			.AddIngredient(ItemID.SoulofFright, 77)
			.AddIngredient(ItemID.SoulofMight, 77)
			.AddIngredient(ItemID.SoulofNight, 77)
			.AddIngredient(ItemID.SoulofLight, 77)
			.AddIngredient(ItemID.SoulofFlight, 77)
			.AddIngredient(ItemID.GoldBar, 40)
			.AddIngredient(ItemID.PlatinumBar, 40)
			.AddIngredient(ItemID.TitaniumBar, 40)
			.AddIngredient(ItemID.AdamantiteBar, 40)
			.AddIngredient(ItemID.HellstoneBar, 40)
			.AddIngredient(ItemID.HallowedBar, 40)
			.AddIngredient(ItemID.ChlorophyteBar, 40)
			.AddIngredient(ItemID.ShroomiteBar, 40)
			.AddIngredient(ItemID.SpectreBar, 40)
			.AddIngredient(ItemID.LunarBar, 40)
			.AddTile(TileID.LunarCraftingStation)
			.Register();
	}
}
public class Vulnerable : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(true);
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		player.buffTime[buffIndex] += time;
		return true;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, player.buffTime[buffIndex] / -60);
	}
}
public class VoidParticle : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 5;
		Projectile.timeLeft = 300;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hide = true;
	}
	public override void AI() {
		int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Granite, newColor: Color.Black);
		Main.dust[dust].noGravity = true;
		Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 300)) {
			Projectile.timeLeft = 100;
			Vector2 distance = npc.Center - Projectile.Center;
			float length = distance.Length();
			if (length > 1) {
				length = 1;
			}
			Projectile.velocity -= Projectile.velocity * .1f;
			Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
			return;
		}
		Projectile.velocity.Y = 1;
		Projectile.velocity -= Projectile.velocity * .01f;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.FinalDamage *= 0;
		modifiers.FinalDamage.Flat += Projectile.ai[0];
	}
}
public class VoidTrail : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 5;
		Projectile.timeLeft = 300;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hide = true;
	}
	public override void AI() {
		int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Granite, newColor: Color.Black);
		Main.dust[dust].noGravity = true;
		Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 300)) {
			Projectile.timeLeft = 100;
			Vector2 distance = npc.Center - Projectile.Center;
			float length = distance.Length();
			if (length > 1) {
				length = 1;
			}
			Projectile.velocity -= Projectile.velocity * .1f;
			Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
			return;
		}
		Projectile.velocity.Y = 1;
		Projectile.velocity -= Projectile.velocity * .01f;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.FinalDamage *= 0;
		modifiers.FinalDamage.Flat += Projectile.ai[0];
	}
}
