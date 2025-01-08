using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.FragmentGrenade;
internal class FragmentGrenade : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.MoltenFury);
	}
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void SetDefaults() {
		Item.BossRushDefaultRange(30, 30, 40, 10f, 40, 40, ItemUseStyleID.Swing, ModContent.ProjectileType<FragmentGrenadeProjectile>(), 15, false);
		Item.noUseGraphic = true;
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.MoltenFury)) {
			tooltips.Add(new TooltipLine(Mod, "FragmentGrenade_MoltenFury", $"[i:{ItemID.MoltenFury}] On explode, rain down hell fire arrow"));
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Grenade)
			.AddIngredient(ItemID.Boomstick)
			.Register();
	}
}
public class FragmentGrenadeProjectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = BossRushUtils.ToSecond(10);
	}
	int bouncecount = 0;
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (bouncecount < 10) {
			Projectile.netUpdate = true;
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = (int)(-oldVelocity.X * 0.9f);
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = (int)(-oldVelocity.Y * 0.75f);
			bouncecount++;
		}
		else {
			if (Projectile.velocity.IsLimitReached(.1f)) {
				Projectile.position += Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
			}
		}
		Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
		return false;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation += MathHelper.ToRadians(Projectile.timeLeft * 30 * -Projectile.direction);
			Projectile.velocity.X *= 0.98f;
			Projectile.velocity.Y += 0.5f;
		}
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		for (int l = 0; l < 53; l++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(10, 10);
			int dus1t = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, Scale: Main.rand.NextFloat(2, 4));
			Main.dust[dus1t].noGravity = true;
			Main.dust[dus1t].velocity = Main.rand.NextVector2Circular(15, 15);
		}
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 150f);
		if (npclist.Count > 0) {
			foreach (NPC npc in npclist) {
				player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage, BossRushUtils.DirectionFromPlayerToNPC(Projectile.Center.X, npc.Center.X), Main.rand.Next(1, 101) <= Projectile.CritChance, Projectile.knockBack));
			}
		}
		int amount = 10;
		int type = ProjectileID.Bullet;
		int damage = (int)(Projectile.damage * .65f);
		float knockback = Projectile.knockBack * .65f;
		for (int i = 0; i < amount; i++) {
			Vector2 vel = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(3, 4);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, type, damage, knockback, Projectile.owner);
		}
		if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<FragmentGrenade>(), ItemID.MoltenFury)) {
			amount /= 2;
			for (int i = 0; i < amount; i++) {
				Vector2 vel = Vector2.UnitY * Main.rand.NextFloat(20, 24);
				Vector2 pos = Projectile.Center - Vector2.UnitY.Add(Main.rand.Next(-50, 50), -Main.rand.Next(700, 900));
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, vel, ProjectileID.HellfireArrow, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].tileCollide = true;
				Main.projectile[proj].timeLeft = 180;
				for (int l = 0; l < 2; l++) {
					int dust = Dust.NewDust(pos, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(3, 4));
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
				}
			}
		}
	}
}
