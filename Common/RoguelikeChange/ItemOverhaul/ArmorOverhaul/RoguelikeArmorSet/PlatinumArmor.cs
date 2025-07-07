using BossRush.Common.Global;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

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
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.06f);
		player.endurance += .03f;
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
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.06f);
		player.endurance += .04f;
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
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.06f);
		player.endurance += .03f;
	}
}
public class PlatinumArmorModPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("PlatinumArmor", this);
	}
	public int GemProjectileWhoAmI = -1;
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
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2CircularEdge(RandomRadius, RandomRadius), Vector2.Zero, ModContent.ProjectileType<GemCollectible>(), Player.GetWeaponDamage(Player.HeldItem), 0, Player.whoAmI, GemTypeGet());
		}
	}
	public override void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		Player.AddBuff(ModContent.BuffType<PlatinumDefense>(), BossRushUtils.ToSecond(4));
		float RandomRadius = Main.rand.NextFloat(200, 400);
		if (Player.ownedProjectileCounts[ModContent.ProjectileType<GemCollectible>()] < 1) {
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2CircularEdge(RandomRadius, RandomRadius), Vector2.Zero, ModContent.ProjectileType<GemCollectible>(), Player.GetWeaponDamage(Player.HeldItem), 0, Player.whoAmI, GemTypeGet());
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
	public override void OnSpawn(IEntitySource source) {
		int dustID = DustID.GemAmethyst;
		int amount = 10;
		int GemID = (int)Projectile.ai[0];
		switch (GemID) {
			case ItemID.Amethyst:
				dustID = DustID.GemAmethyst;
				break;
			case ItemID.LargeAmethyst:
				amount *= 3;
				dustID = DustID.GemAmethyst;
				break;
			case ItemID.Topaz:
				dustID = DustID.GemTopaz;
				break;
			case ItemID.LargeTopaz:
				amount *= 3;
				dustID = DustID.GemTopaz;
				break;
			case ItemID.Sapphire:
				dustID = DustID.GemSapphire;
				break;
			case ItemID.LargeSapphire:
				amount *= 3;
				dustID = DustID.GemSapphire;
				break;
			case ItemID.Emerald:
				dustID = DustID.GemEmerald;
				break;
			case ItemID.LargeEmerald:
				amount *= 3;
				dustID = DustID.GemEmerald;
				break;
			case ItemID.Ruby:
				dustID = DustID.GemRuby;
				break;
			case ItemID.LargeRuby:
				amount *= 3;
				dustID = DustID.GemRuby;
				break;
			case ItemID.Diamond:
				dustID = DustID.GemDiamond;
				break;
			case ItemID.LargeDiamond:
				amount *= 3;
				dustID = DustID.GemDiamond;
				break;
		}
		dust = dustID;
		dustAmount = amount;
	}
	public int dust = -1;
	public int dustAmount = 0;
	public override bool? CanDamage() {
		return Projectile.ai[1] >= 3;
	}
	public override void AI() {
		for (int i = 0; i < dustAmount / 10; i++) {
			Dust dustEffect = Dust.NewDustDirect(Projectile.Center, 0, 0, dust);
			dustEffect.velocity = Main.rand.NextVector2Circular(10, 10);
			dustEffect.noGravity = true;
			dustEffect.scale = Main.rand.NextFloat(.8f, 1.2f);
		}
		Player player = Main.player[Projectile.owner];
		//TODO : Add dust effect here
		if (Projectile.ai[1] == 0) {
			if (Projectile.Center.IsCloseToPosition(player.Center, 30)) {
				Projectile.ai[1] = 1;
			}
			else {
				if (Projectile.Center.IsCloseToPosition(player.Center, 200)) {
					Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * ++Projectile.ai[2];
					if (Projectile.ai[2] >= 20) {
						Projectile.ai[2] = 20;
					}
				}

			}
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
			if (player.ItemAnimationActive) {
				Projectile.ai[1]++;
			}
		}
		if (Projectile.ai[1] >= 3) {
			if (Projectile.ai[1] == 3) {
				Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 20;
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.ai[1]++;
				Projectile.timeLeft = 180;
			}
		}
	}
	public override void OnKill(int timeLeft) {
		Main.player[Projectile.owner].GetModPlayer<PlatinumArmorModPlayer>().GemProjectileWhoAmI = -1;
		for (int i = 0; i < dustAmount; i++) {
			Dust dustEffect = Dust.NewDustDirect(Projectile.Center, 0, 0, dust);
			dustEffect.velocity = Main.rand.NextVector2Circular(10, 10);
			dustEffect.noGravity = true;
			dustEffect.scale = Main.rand.NextFloat(.8f, 1.2f);
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		int GemID = (int)Projectile.ai[0];
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
		player.buffTime[buffIndex] = time;
		return true;
	}
}
