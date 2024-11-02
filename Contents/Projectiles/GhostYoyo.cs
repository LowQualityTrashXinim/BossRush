using BossRush.Common.Utils;
using BossRush.Contents.WeaponEnchantment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles;
internal class GhostYoyo : ModProjectile {

	int yoyoID;
	float yoyoOffset;
	float yoyoSpinSpeed;

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.WoodYoyo);

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;

	}
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.WoodYoyo);
		Projectile.aiStyle = -1;
		Projectile.usesLocalNPCImmunity = true;

	}

	public override bool PreDraw(ref Color lightColor) {

		Main.instance.LoadProjectile(ContentSamples.ItemsByType[(int)Projectile.ai[0]].shoot);
		Texture2D yoyoTexture = TextureAssets.Projectile[ContentSamples.ItemsByType[(int)Projectile.ai[0]].shoot].Value;

		Vector2 origin = yoyoTexture.Size() * .5f;

		Vector2 drawpos = Projectile.position - Main.screenPosition + origin;

		for (float i = 0; i < Projectile.oldPos.Length; i++) {

			Main.EntitySpriteDraw(yoyoTexture, Projectile.oldPos[(int)i] - Main.screenPosition + yoyoTexture.Size() / 2f, null, lightColor * MathHelper.Lerp(1f, 0f, i / Projectile.oldPos.Length), 0, yoyoTexture.Size() / 2f, 1f, SpriteEffects.None);


		}
		Main.EntitySpriteDraw(yoyoTexture, drawpos, null, lightColor, 0, origin, 1f, SpriteEffects.None);



		return false;
	}


	public override void AI() {

		Player player = Main.player[Projectile.owner];

		Projectile.ai[1] += Projectile.ai[2];

		if (Main.myPlayer == player.whoAmI)
			Projectile.position = (new Vector2(MathF.Sin(Projectile.ai[1]) * 125, MathF.Cos(Projectile.ai[1]) * 125) + Main.MouseWorld);


		if (player.HeldItem.type != ItemID.None && player.HeldItem.IsAWeapon() && !player.HeldItem.consumable)
			foreach (int enchantment in player.HeldItem.GetGlobalItem<EnchantmentGlobalItem>().EnchantmenStlot)
				if (enchantment == Projectile.ai[0])
					Projectile.timeLeft = 2;


	}
}
