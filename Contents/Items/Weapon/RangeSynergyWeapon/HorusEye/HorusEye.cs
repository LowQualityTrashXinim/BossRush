using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HorusEye;
/// <summary>
///  Uwaaa blue archive weapon but not really
/// </summary>
internal class HorusEye : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(45, 120, 60, 7f, 28, 28, ItemUseStyleID.Shoot, ProjectileID.Bullet, 6f, false, AmmoID.Bullet);
		Item.crit = 12;
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 50);
		Item.scale = 0.7f;
		Item.UseSound = SoundID.Item38 with {
			Pitch = -.7f,
			PitchVariance = .2f
		};
		Item.scale = .7f;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-33, 5f);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 70);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		for (int i = 0; i < 30; i++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Firework_Pink);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(1.5f, 10f).RotatedBy(velocity.ToRotation());
			Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
		}
		for (int i = 0; i < 30; i++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Firework_Pink);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(velocity.ToRotation()) * Main.rand.NextFloat(7f, 19f);
			Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
		}
		player.velocity += -velocity * .35f;
		CanShootItem = false;
		for (int i = 0; i < 10; i++) {
			Projectile.NewProjectile(source, position, Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(velocity.ToRotation()) * Main.rand.NextFloat(7f, 21f), type, damage, knockback, player.whoAmI);
		}
	}
}
class HorusEye_ShieldBuff : SynergyBuff {
	public override void SynergySetStaticDefaults() {
		Main.debuff[Type] = false;
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		string hexColor;
		PlayerSynergyItemHandle modplayer = Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>();
		if (modplayer.HoruseEye_ShieldHealthPoint >= 550) {
			hexColor = Color.LawnGreen.Hex3();
		}
		else if (modplayer.HoruseEye_ShieldHealthPoint >= 250) {
			hexColor = Color.Yellow.Hex3();
		}
		else {
			hexColor = Color.Red.Hex3();
		}
		tip += $"\nShield remain Health : [c/{hexColor}:{modplayer.HoruseEye_ShieldHealthPoint}]";
	}
	public override void UpdatePlayer(Player player, ref int buffIndex) {
		player.DefenseEffectiveness *= 1;
		player.statDefense += 10;
	}
}
