using BossRush.Common.Global;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.SakuraKatana;
internal class SakuraKatana : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		DataStorer.AddContext("Synergy_Sakura", new(600, Vector2.Zero, false, Color.Pink));
	}
	public override void SetDefaults() {
		Item.BossRushSetDefault(48, 92, 48, 6f, 20, 20, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
			meleeItem.SwingType = BossRushUseStyle.Swipe;
			meleeItem.UseSwipeTwo = true;
		}
		Item.UseSound = SoundID.Item1;
		Item.Set_InfoItem();
	}
	int delayBetweenHit = 0;
	public override void Synergy_ModifyWeaponDamage(Player player, ref StatModifier damage) {
		int sakuraArtCount = player.GetModPlayer<SakuraKatana_ModPlayer>().SakuraArt;
		damage += .05f * sakuraArtCount;
	}
	public override void Synergy_ModifyWeaponCrit(Player player, ref float crit) {
		int sakuraArtCount = player.GetModPlayer<SakuraKatana_ModPlayer>().SakuraArt;
		crit += 2 * sakuraArtCount;
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		float randomrotation = Main.rand.NextFloat(90);
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		for (int i = 0; i < 4; i++) {
			Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * Main.rand.NextFloat(5, 7);
			for (int l = 0; l < 4; l++) {
				float multiplier = Main.rand.NextFloat();
				float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
				int dust = Dust.NewDust(target.Center + randomPosOffset, 0, 0, DustID.Enchanted_Pink, 0, 0, 0, Color.White, scale);
				Main.dust[dust].velocity = Toward * multiplier;
				Main.dust[dust].noGravity = true;
			}
		}
		if (delayBetweenHit > 0) {
			return;
		}
		SakuraKatana_ModPlayer sakuraplayer = player.GetModPlayer<SakuraKatana_ModPlayer>();
		sakuraplayer.Add_SakuraArt();
		int strikecount = ++sakuraplayer.StrikeCount;
		if (strikecount > 30) {
			sakuraplayer.CherryBlossomAura_CoolDown = BossRushUtils.ToSecond(20);
			sakuraplayer.CherryBlossomAura_Duration = BossRushUtils.ToSecond(30);
			sakuraplayer.CherryBlossomAura_Position = player.Center;
		}
		float Rotation = MathHelper.ToRadians(Main.rand.NextFloat(90));
		Vector2 pos = target.Center + Main.rand.NextVector2Circular(target.width, target.height);
		for (int i = 0; i < 5; i++) {
			Vector2 vel = Vector2.One.Vector2DistributeEvenly(5, 360, i).RotatedBy(Rotation);
			Projectile proj = Projectile.NewProjectileDirect(Item.GetSource_OnHit(target), pos, vel * .1f, ModContent.ProjectileType<SakuraLeaf_Projectile_2>(), Item.damage, Item.knockBack, player.whoAmI);
			proj.frame = 1;
		}
		player.GetModPlayer<SakuraKatana_ModPlayer>().DelayHit = 60;
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		if (player.ItemAnimationJustStarted) {
			int flip = player.GetModPlayer<MeleeOverhaulPlayer>().ComboNumber == 0 ? 1 : -1;
			int amount = Main.rand.Next(3, 6);
			for (int i = 0; i < amount; i++) {
				Vector2 limited = player.velocity.LimitedVelocity(10);
				Vector2 vel = Vector2.UnitX * Main.rand.NextFloat(5, 10) * player.direction + limited;
				Vector2 pos = Main.rand.NextVector2FromRectangle(player.Hitbox);
				Projectile proj = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), pos.Add(0, Item.height * flip), vel, ModContent.ProjectileType<SakuraLeaf_Projectile_1>(), (int)(Item.damage * .35f), Item.knockBack, player.whoAmI);
				proj.penetrate = 1;
				proj.ai[0] = flip * -player.direction;
			}
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Katana)
			.AddIngredient(ItemID.AbigailsFlower)
			.Register();
	}
}
public class SakuraKatana_ModPlayer : ModPlayer {
	public int SakuraArt = 0;
	public int SakuraArt_CountDown = 0;
	public int StrikeCount = 0;
	public int DelayHit = 0;
	public Vector2 CherryBlossomAura_Position = Vector2.Zero;
	public int CherryBlossomAura_Duration = 0;
	public int CherryBlossomAura_CoolDown = 0;
	public void Add_SakuraArt() {
		SakuraArt = Math.Clamp(SakuraArt + 1, 0, 5);
	}
	public override void ResetEffects() {
		DelayHit = BossRushUtils.CountDown(DelayHit);
		if (CherryBlossomAura_Duration > 0) {
			CherryBlossomAura_Position += (Player.Center - CherryBlossomAura_Position).SafeNormalize(Vector2.Zero) * (Player.Center - CherryBlossomAura_Position).Length() / 64f;
			CherryBlossomAura_Duration = BossRushUtils.CountDown(CherryBlossomAura_Duration);
			DataStorer.ActivateContext(CherryBlossomAura_Position, "Synergy_Sakura");
		}
		else {
			CherryBlossomAura_CoolDown = BossRushUtils.CountDown(CherryBlossomAura_CoolDown);
		}
		if (CherryBlossomAura_CoolDown > 0 || CherryBlossomAura_Duration > 0) {
			StrikeCount = 0;
		}
		if (SakuraArt <= 0) {
			return;
		}
		if (--SakuraArt_CountDown <= 0) {
			SakuraArt = BossRushUtils.CountDown(SakuraArt);
			SakuraArt_CountDown = BossRushUtils.ToSecond(7);
		}
	}
	public override void UpdateEquips() {
		if (CherryBlossomAura_Duration <= 0) {
			return;
		}
		int damage = (int)(Player.HeldItem.damage * .65f);
		if (CherryBlossomAura_Duration % 60 == 0) {
			float Rotation = MathHelper.ToRadians(Main.rand.NextFloat(90));
			Vector2 pos = CherryBlossomAura_Position + Main.rand.NextVector2Circular(500, 500);
			for (int i = 0; i < 5; i++) {
				Vector2 vel = Vector2.One.Vector2DistributeEvenly(5, 360, i).RotatedBy(Rotation);
				Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), pos, vel * .2f, ModContent.ProjectileType<SakuraLeaf_Projectile_2>(), damage, 0, Player.whoAmI);
				proj.frame = 1;
				proj.penetrate = 1;
			}
		}
		if (CherryBlossomAura_Duration % 20 == 0) {
			int amount = Main.rand.Next(3, 6);
			for (int i = 0; i < amount; i++) {
				Vector2 vel = Vector2.UnitX * Main.rand.NextFloat(5, 10) * Main.rand.NextBool().ToDirectionInt();
				Vector2 pos = CherryBlossomAura_Position + Main.rand.NextVector2Circular(500, 500);
				Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), pos, vel, ModContent.ProjectileType<SakuraLeaf_Projectile_1>(), (int)damage, 0, Player.whoAmI);
				proj.penetrate = 1;
				proj.ai[0] = Main.rand.NextBool().ToDirectionInt();
			}
		}
		if (CherryBlossomAura_Position.IsCloseToPosition(Player.Center, 600f)) {
			PlayerStatsHandle statplayer = Player.GetModPlayer<PlayerStatsHandle>();
			statplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.3f);
			statplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 10);
			statplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.5f);
			statplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 10);
			statplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: 5);
			statplayer.EnergyRegen.Base += 5;
		}
	}
}
public class SakuraLeaf_Projectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 14;
		Projectile.timeLeft = 180;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 3;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 50;
		Projectile.ContinuouslyUpdateDamageStats = true;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		SakuraAI();
	}
	public virtual void SakuraAI() { }
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Main.instance.LoadGore(GoreID.TreeLeaf_VanityTreeSakura);
		Texture2D texture = TextureAssets.Gore[GoreID.TreeLeaf_VanityTreeSakura].Value;
		Vector2 origin = texture.Size();
		origin.X /= 32;
		origin.Y /= 8;
		Vector2 pos = Projectile.position - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, pos, texture.Frame(32, 8, 0, Projectile.frame), Color.White * (1 - Projectile.alpha / 255f), Projectile.rotation, origin * .5f, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
public class SakuraLeaf_Projectile_1 : SakuraLeaf_Projectile {
	public override void SakuraAI() {
		if (++Projectile.frameCounter >= 6) {
			Projectile.frameCounter = 0;
			Projectile.frame += 1;
			if (Projectile.frame >= 8) {
				Projectile.frame = 0;
			}
		}
		Projectile.ProjectileAlphaDecay(30);
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		Projectile.velocity += Projectile.rotation.ToRotationVector2() * -Projectile.ai[0] * .5f;
		Projectile.velocity.Y += .01f;
	}
}
public class SakuraLeaf_Projectile_2 : SakuraLeaf_Projectile {
	public override void SakuraAI() {
		Projectile.ProjectileAlphaDecay(30);
		if (Projectile.velocity != Vector2.Zero) {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		Projectile.velocity += Projectile.velocity * .025f;
		Projectile.velocity = Projectile.velocity.LimitedVelocity(2);
	}
}
