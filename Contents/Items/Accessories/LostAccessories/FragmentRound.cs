using BossRush.Common.Global;
using BossRush.Contents.Transfixion.Arguments;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class FragmentRound : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<FragmentRound_Player>().FragmentRound = true;
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AddStatsToPlayer(PlayerStats.RangeDMG, Additive: 1.1f, Flat: 3);
	}
}
public class FragmentRound_Player : ModPlayer {
	public bool FragmentRound = false;
	public override void ResetEffects() {
		FragmentRound = false;
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (FragmentRound) {
			if (type == ProjectileID.Bullet) {
				type = ModContent.ProjectileType<FragmentRound_Projectile>();
			}
		}
	}
}
public class FragmentRound_Projectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Bullet);
	public override void SetDefaults() {
		Projectile.height = Projectile.width = 4;
		Projectile.friendly = true;
		Projectile.timeLeft = 30;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
		Projectile.light = 0.5f;
		Projectile.extraUpdates = 1;
		Projectile.alpha = 255;
	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.timeLeft += Main.rand.Next(-10, 11);
	}
	public override void AI() {
		Projectile.alpha -= 20;
		if (Projectile.timeLeft <= 4) {
			int amount = Main.rand.Next(2, 4);
			for (int i = 0; i < amount; i++) {
				Projectile bullet = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.Vector2RotateByRandom(30).Vector2RandomSpread(2, Main.rand.NextFloat(.8f, 1.1f)), ProjectileID.Bullet, (int)(Projectile.damage * .45f), Projectile.knockBack * .5f, Projectile.owner);
				bullet.scale -= .5f;
				bullet.Resize(2, 2);
				Dust smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Scale: Main.rand.NextFloat(.95f, 1.25f));
				smoke.velocity = Main.rand.NextVector2Circular(2, 2);
				smoke.noGravity = true;
				Dust spark = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Scale: Main.rand.NextFloat(.75f, 1.25f));
				spark.velocity = Main.rand.NextVector2Circular(2, 2);
				spark.noGravity = true;
			}
			Projectile.Kill();
		}
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
	}
}
