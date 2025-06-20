using Terraria;
using Terraria.ID;
using Terraria.Audio;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.Mutation;
using BossRush.Common.Systems.ObjectSystem;
using BossRush.Common.RoguelikeChange.ItemOverhaul;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SkullRevolver;
internal class SkullRevolver : SynergyModItem {
	int counter = 0;
	public override void SetDefaults() {
		Item.BossRushDefaultRange(26, 52, 25, 3f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.Bullet, 20f, false, AmmoID.Bullet);
		Item.rare = ItemRarityID.Orange;
		Item.UseSound = SoundID.Item11;
		Item.crit = 15;
		Item.value = Item.buyPrice(gold: 50);
		Item.UseSound = SoundID.Item41;
		if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
			weapon.SpreadAmount = 5;
			weapon.OffSetPost = 50;
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		if (++counter >= 10) {
			Projectile.NewProjectile(source, position + Main.rand.NextVector2CircularEdge(200, 200) * Main.rand.NextFloat(.75f, 1.25f), Vector2.Zero, ModContent.ProjectileType<ShadowFlameSkullPortalProjectile>(), damage, knockback, player.whoAmI);
			counter = 0;
		}
		if (Main.rand.NextFloat() <= .02f) {
			Projectile.NewProjectile(source, position + Main.rand.NextVector2CircularEdge(200, 200) * Main.rand.NextFloat(.75f, 1.25f), Vector2.Zero, ModContent.ProjectileType<CursedSkullPortalProjectile>(), damage, knockback, player.whoAmI);
		}
		CanShootItem = true;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-3, 0);
	}
	public override void AddRecipes() {
		CreateRecipe()
		.AddIngredient(ItemID.Revolver)
		.AddIngredient(ItemID.BookofSkulls)
		.Register();
	}
}
public class SkullRevolverModPlayer : ModPlayer {
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(ModContent.ItemType<SkullRevolver>())) {
			target.AddBuff(ModContent.BuffType<CursedStatus>(), BossRushUtils.ToSecond(5));
			if (target.HasBuff<CursedStatus>()
				&& target.GetGlobalNPC<RoguelikeGlobalNPC>().CursedSkullStatus >= 3
				&& proj.type != ProjectileID.BookOfSkullsSkull
				&& proj.type != ProjectileID.ClothiersCurse) {
				int amountRandom = Main.rand.Next(1, 4);
				for (int i = 0; i < amountRandom; i++) {
					Vector2 newpos = Player.Center + Main.rand.NextVector2CircularEdge(50, 50) * Main.rand.NextFloat(.9f, 1.25f);
					Projectile projectile = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(Player.HeldItem), newpos, (target.Center - newpos).SafeNormalize(Vector2.Zero) * 15, ProjectileID.BookOfSkullsSkull, Player.GetWeaponDamage(Player.HeldItem) + 1, 3f, Player.whoAmI);
					projectile.tileCollide = false;
					projectile.timeLeft = BossRushUtils.ToSecond(5);
					projectile.penetrate = 1;
				}
			}
		}
	}
}
public class CursedStatus : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		if (npc.buffTime[buffIndex] <= 0) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().CursedSkullStatus = 0;
		}
		if (npc.HasBuff<CursedStatus>() && npc.GetGlobalNPC<RoguelikeGlobalNPC>().CursedSkullStatus > 0) {
			npc.lifeRegen -= 10 + 5 * npc.GetGlobalNPC<RoguelikeGlobalNPC>().CursedSkullStatus;
			Color[] colorArr = new Color[] { Color.Purple, Color.Red, Color.White };
			for (int i = 0; i < npc.GetGlobalNPC<RoguelikeGlobalNPC>().CursedSkullStatus; i++) {
				Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.WhiteTorch);
				dust.noGravity = true;
				dust.velocity = Vector2.UnitY * -Main.rand.NextFloat(3);
				dust.scale = Main.rand.NextFloat(0.75f, 1.25f);
				Color color = Main.rand.Next(colorArr);
				color.A = 0;
				dust.color = color;
			}
		}
	}
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		if (npc.buffTime[buffIndex] <= 0) {
			return false;
		}
		return true;
	}
}
public class ShadowFlameSkullPortalProjectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 100;
	}
	public override bool? CanDamage() {
		return false;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		float progress = Projectile.timeLeft;
		for (int i = 0; i < 4; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Shadowflame);
			dust.velocity = Vector2.One.RotatedBy(MathHelper.ToRadians(progress + 90 * i)) * 4;
			dust.noGravity = true;
			dust.scale += 1;
			Dust dust2 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Shadowflame);
			dust2.velocity = Main.rand.NextVector2CircularEdge(3, 3);
			dust2.noGravity = true;
			dust2.scale += Main.rand.NextFloat();
		}
		Lighting.AddLight(Projectile.Center, Color.MediumPurple.ToVector3());
		if (progress > Projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().InitialTimeLeft * .7f) {
			return;
		}
		if (progress % 10 == 0) {
			Vector2 vel = (Main.MouseWorld - Projectile.position).SafeNormalize(Vector2.Zero) * 15;
			Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, vel, ProjectileID.ClothiersCurse, Projectile.damage, 3f, Main.LocalPlayer.whoAmI);
			proj.tileCollide = false;
			proj.timeLeft = BossRushUtils.ToSecond(4);
			proj.penetrate = 1;
			proj.maxPenetrate = 1;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		return false;
	}
}
public class CursedSkullPortalProjectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = BossRushUtils.ToSecond(3);
	}
	public override bool? CanDamage() {
		return false;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		float progress = Projectile.timeLeft;
		Color[] colorArr = new Color[] { Color.Purple, Color.Red, Color.White };
		for (int i = 0; i < 4; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.WhiteTorch);
			dust.velocity = Vector2.One.RotatedBy(MathHelper.ToRadians(progress + 90 * i)) * 4;
			dust.noGravity = true;
			dust.scale += 1;
			Color color = Main.rand.Next(colorArr);
			color.A = 0;
			dust.color = color;
			Dust dust2 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.WhiteTorch);
			dust2.velocity = Main.rand.NextVector2CircularEdge(3, 3);
			dust2.noGravity = true;
			dust2.scale += Main.rand.NextFloat();
			dust2.color = color;
		}

		Lighting.AddLight(Projectile.Center, Color.MediumPurple.ToVector3());
		if (progress % 10 == 0) {
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(10, 10), ProjectileID.BookOfSkullsSkull, Projectile.damage, 3f, Main.LocalPlayer.whoAmI);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		return false;
	}
}
