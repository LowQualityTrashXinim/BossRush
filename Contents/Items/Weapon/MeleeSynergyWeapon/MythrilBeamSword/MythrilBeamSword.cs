using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;
using BossRush.Common.RoguelikeChange.ItemOverhaul;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.MythrilBeamSword;
public class MythrilBeamSword : SynergyModItem {


	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(72, 72, 88, 6f, 50, 50, ItemUseStyleID.Swing, ModContent.ProjectileType<MythrilBeam>(), 15, true);
		Item.GetGlobalItem<MeleeWeaponOverhaul>().SwingType = BossRushUseStyle.Swipe;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		MeleeOverhaulPlayer meleeplayer = player.GetModPlayer<MeleeOverhaulPlayer>();
		for (int i = 0; i < 6; i++) {
			Vector2 rotate = velocity.Vector2DistributeEvenly(5, 120 * player.direction, meleeplayer.ComboNumber % 2 == 0 ? i : 5 - i) * .5f;
			Projectile.NewProjectile(source, position.PositionOFFSET(rotate, 36f), rotate, type, damage, knockback, player.whoAmI, ai1: i);
		}
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
	}
	public override void AddRecipes() {
		CreateRecipe().AddIngredient(ItemID.MythrilSword).AddIngredient(ItemID.BeamSword).Register();
	}

}
public struct BeamTrail {
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Projectile projectile, Color color) {

		MiscShaderData miscShaderData = GameShaders.Misc["TrailEffect"];
		miscShaderData.UseImage1("Images/Extra_" + (short)193);
		miscShaderData.UseColor(color);

		miscShaderData.Apply();

		_vertexStrip.PrepareStrip(projectile.oldPos, projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + projectile.Size * 0.5f);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip) {
		Color result = new Color(255, 255, 255, MathHelper.Lerp(0, 255, progressOnStrip));
		//result.A /= 2;
		return result;
	}
	private float StripWidth(float progressOnStrip) => MathHelper.Lerp(4, 1, progressOnStrip);
}
public class MythrilBeam : SynergyModProjectile {

	bool retargeting = false;

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 35;
	}

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SwordBeam);


	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.SwordBeam);
		Projectile.aiStyle = -1;

	}

	public override void OnSpawn(IEntitySource source) {
		retargeting = false;
		Projectile.FillProjectileOldPosAndRot();
	}

	public override bool PreDraw(ref Color lightColor) {


		Asset<Texture2D> texture = TextureAssets.Projectile[ProjectileID.SwordBeam];
		Main.instance.LoadProjectile(ProjectileID.SwordBeam);

		if (!retargeting) {

			Main.EntitySpriteDraw(texture.Value, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 15 - Main.screenPosition, null, Color.Yellow, Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2f, 1f, SpriteEffects.None);
			default(BeamTrail).Draw(Projectile, Color.Yellow);


		}
		else {

			Main.EntitySpriteDraw(texture.Value, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 15 - Main.screenPosition, null, Color.MediumVioletRed, Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2f, 1f, SpriteEffects.None);
			default(BeamTrail).Draw(Projectile, Color.MediumVioletRed);

		}

		return false;
	}
	int timer = 0;
	Vector2 localOriginalvelocity;
	public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) {
		runAI = false;
		if (timer == 0) {
			localOriginalvelocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
			Projectile.rotation = localOriginalvelocity.ToRotation();
		}
		if (timer <= 20 + Projectile.ai[1] * 2) {
			Projectile.timeLeft = 200;
			Projectile.velocity -= Projectile.velocity * .1f;
			timer++;
		}
		else {
			runAI = true;
			if (!Projectile.velocity.IsLimitReached(20) && Projectile.ai[0] < 25) Projectile.velocity += localOriginalvelocity;
		}
	}

	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		Projectile.ai[0]++;
		Projectile.rotation = Projectile.velocity.ToRotation();

		int range = 1200;
		Vector2 targetPos = Projectile.Center.LookForHostileNPCPositionClosest(range);

		if (Projectile.ai[0] == 25) {
			retargeting = true;
			Projectile.damage = (int)(Projectile.damage * 1.25f);
			for (int i = 0; i < 35; i++) {
				var dust = Dust.NewDustPerfect(Projectile.position, DustID.OrangeTorch, Main.rand.NextVector2CircularEdge(10, 10));
				dust.noGravity = true;

			}
			Projectile.velocity =
			targetPos != Vector2.Zero
			? Projectile.Center.DirectionTo(targetPos) * Projectile.velocity.Length()
			: Projectile.velocity;
		}
	}
}
