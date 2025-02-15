using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class PlatinumArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.PlatinumHelmet;
		bodyID = ItemID.PlatinumChainmail;
		legID = ItemID.PlatinumGreaves;
	}
}
public class PlatinumHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.PlatinumHelmet;
		Add_Defense = 2;
		ArmorName = "PlatinumArmor";
		TypeEquipment = Type_Head;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.04f);
	}
}
public class PlatinumChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.PlatinumChainmail;
		Add_Defense = 2;
		ArmorName = "PlatinumArmor";
		TypeEquipment = Type_Body;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.04f);
	}
}
public class PlatinumGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.PlatinumGreaves;
		Add_Defense = 2;
		ArmorName = "PlatinumArmor";
		TypeEquipment = Type_Leg;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.04f);
	}
}
public class PlatinumArmorModPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("PlatinumArmor", this);
	}
	public int GemProjectileWhoAmI = -1;
	public override void Armor_ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		//Attempt to retrieve projectile
	}
	public override void Armor_ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		MultiplyDamage(ref modifiers);
	}
	public override void Armor_ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.minion) {
			return;
		}
		MultiplyDamage(ref modifiers);
	}
	private void MultiplyDamage(ref NPC.HitModifiers modifiers) {
		if (GemProjectileWhoAmI < 0 || GemProjectileWhoAmI >= 1000) {
			return;
		}
		Projectile projectile = Main.projectile[GemProjectileWhoAmI];
		int GemID = (int)projectile.ai[0];
		switch (GemID) {
			case ItemID.Amethyst:
				modifiers.SourceDamage *= 1.3f;
				break;
			case ItemID.Topaz:
				modifiers.SourceDamage *= 1.35f;
				break;
			case ItemID.Sapphire:
				modifiers.SourceDamage *= 1.4f;
				break;
			case ItemID.Emerald:
				modifiers.SourceDamage *= 1.45f;
				break;
			case ItemID.Ruby:
				modifiers.SourceDamage *= 1.5f;
				break;
			case ItemID.Diamond:
				modifiers.SourceDamage *= 1.55f;
				break;
			case ItemID.LargeAmethyst:
				modifiers.SourceDamage *= 2;
				break;
			case ItemID.LargeTopaz:
				modifiers.SourceDamage *= 2;
				break;
			case ItemID.LargeSapphire:
				modifiers.SourceDamage *= 2.5f;
				break;
			case ItemID.LargeEmerald:
				modifiers.SourceDamage *= 2.5f;
				break;
			case ItemID.LargeRuby:
				modifiers.SourceDamage *= 3f;
				break;
			case ItemID.LargeDiamond:
				modifiers.SourceDamage *= 3f;
				break;
		}
		projectile.Kill();
	}
	public int GemTypeGet() {
		bool checkTheoddlol = Main.rand.NextFloat() <= .05f;
		bool ChoosingBetween = Main.rand.NextBool();
		if (Main.rand.NextFloat() <= .1f) {
			if (checkTheoddlol) {
				if (ChoosingBetween) {
					return ItemID.LargeRuby;
				}
				else {
					return ItemID.LargeDiamond;
				}
			}
			else {
				if (ChoosingBetween) {
					return ItemID.Ruby;
				}
				else {
					return ItemID.Diamond;
				}
			}
		}
		if (Main.rand.NextFloat() <= .3f) {
			if (checkTheoddlol) {
				if (ChoosingBetween) {
					return ItemID.LargeEmerald;
				}
				else {
					return ItemID.LargeSapphire;
				}
			}
			else {
				if (ChoosingBetween) {
					return ItemID.Emerald;
				}
				else {
					return ItemID.Sapphire;
				}
			}
		}
		if (checkTheoddlol) {
			if (ChoosingBetween) {
				return ItemID.LargeTopaz;
			}
			else {
				return ItemID.LargeAmethyst;
			}
		}
		else {
			if (ChoosingBetween) {
				return ItemID.Topaz;
			}
			else {
				return ItemID.Amethyst;
			}
		}
	}
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		Player.AddBuff(ModContent.BuffType<PlatinumDefense>(), BossRushUtils.ToSecond(4));
		float RandomRadius = Main.rand.NextFloat(200, 400);
		if (Player.ownedProjectileCounts[ModContent.ProjectileType<GemCollectible>()] < 1) {
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2CircularEdge(RandomRadius, RandomRadius), Vector2.Zero, ModContent.ProjectileType<GemCollectible>(), 0, 0, Player.whoAmI, GemTypeGet());
		}
	}
	public override void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		Player.AddBuff(ModContent.BuffType<PlatinumDefense>(), BossRushUtils.ToSecond(4));
		float RandomRadius = Main.rand.NextFloat(200, 400);
		if (Player.ownedProjectileCounts[ModContent.ProjectileType<GemCollectible>()] < 1) {
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2CircularEdge(RandomRadius, RandomRadius), Vector2.Zero, ModContent.ProjectileType<GemCollectible>(), 0, 0, Player.whoAmI, GemTypeGet());
		}
	}
	public override void Armor_ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (Player.HasBuff<PlatinumDefense>()) {
			modifiers.SourceDamage -= .66f;
		}
	}
	public override void Armor_ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Player.HasBuff<PlatinumDefense>()) {
			modifiers.SourceDamage -= .66f;
		}
	}
}
public class GemCollectible : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 900;
		Projectile.tileCollide = false;
	}
	public override bool? CanDamage() {
		return false;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		//TODO : Add dust effect here
		if (Projectile.Center.IsCloseToPosition(player.Center, 30)) {
			Projectile.ai[1] = 1;
		}
		if (Projectile.ai[1] == 1) {
			Projectile.Center = player.Center.Add(0, 25 + Projectile.width);
			PlatinumArmorModPlayer modplayer = player.GetModPlayer<PlatinumArmorModPlayer>();
			if (modplayer.GemProjectileWhoAmI == -1 || modplayer.GemProjectileWhoAmI == 0) {
				player.GetModPlayer<PlatinumArmorModPlayer>().GemProjectileWhoAmI = Projectile.whoAmI;
				Projectile.ai[1]++;
			}
		}
		if (Projectile.ai[1] == 2) {
			Projectile.velocity = player.velocity;
		}
	}
	public override void OnKill(int timeLeft) {
		Main.player[Projectile.owner].GetModPlayer<PlatinumArmorModPlayer>().GemProjectileWhoAmI = -1;
		//TODO : Add death dust effect here
	}
	public int DrawItemIDData => (int)Projectile.ai[0];
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadItem(DrawItemIDData);
		Texture2D texture = TextureAssets.Item[DrawItemIDData].Value;
		Projectile.Resize(texture.Width, texture.Height);
		Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition.Add(0, Projectile.gfxOffY) + texture.Size() * .5f, null, lightColor, 0, texture.Size() * .5f, 1f, SpriteEffects.None, 0);
		return false;
	}
}
public class PlatinumDefense : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		return true;
	}
}
