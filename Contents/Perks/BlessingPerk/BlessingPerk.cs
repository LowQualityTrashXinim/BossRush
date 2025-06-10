using BossRush.Common.Global;
using BossRush.Contents.Items;
using BossRush.Contents.Items.Chest;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BossRush.Contents.Perks.BlessingPerk;

public class BlessingOfSolar : Perk {
	public override void SetDefaults() {
		textureString = BossRushUtils.GetTheSameTextureAsEntity<BlessingOfSolar>();
		list_category.Add(PerkCategory.Starter);
		CanBeStack = true;
		StackLimit = 3;
	}
	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) >= 2) {
			return DescriptionIndex(3);
		}
		return base.ModifyToolTip();
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<ChestLootDropPlayer>().UpdateMeleeChanceMutilplier += 1f;
	}
	public override void ModifyItemScale(Player player, Item item, ref float scale) {
		if (item.DamageType == DamageClass.Melee)
			scale += .12f * StackAmount(player);
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.DamageType != DamageClass.Melee && item.DamageType != DamageClass.MeleeNoSpeed) {
			return;
		}
		if (Main.rand.NextFloat() <= .07f * StackAmount(player)) {
			Item.NewItem(item.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Heart));
		}
		if (Main.rand.NextBool(10)) {
			target.AddBuff(ModContent.BuffType<MeltingDefense>(), BossRushUtils.ToSecond(3.5f));
		}
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.DamageType == DamageClass.Melee && Main.rand.NextBool(10)) {
			target.AddBuff(ModContent.BuffType<MeltingDefense>(), BossRushUtils.ToSecond(3.5f));
		}
	}
	public override void OnPickUp(Player player, Item item) {
		if (item.type == ItemID.Heart && StackAmount(player) >= 3 && Main.rand.NextBool(5)) {
			player.Heal(100);
		}
	}
}
public class MeltingDefense : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= Math.Clamp(npc.defense, 0, 40);
	}
}
public class BlessingOfVortex : Perk {
	public override void SetDefaults() {
		textureString = BossRushUtils.GetTheSameTextureAsEntity<BlessingOfVortex>();
		list_category.Add(PerkCategory.Starter);
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RangeCritDmg, Additive: 1.5f);
		player.GetModPlayer<ChestLootDropPlayer>().UpdateRangeChanceMutilplier += 1f;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .01f * StackAmount(player) && proj.DamageType == DamageClass.Ranged) {
			modifiers.SourceDamage *= 4;
		}
		if (StackAmount(player) >= 3 && player.GetModPlayer<BlessingOfVortexPlayer>().VortexCounter >= 5) {
			modifiers.SetCrit();
		}
	}
	public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (StackAmount(player) >= 3) {
			player.GetModPlayer<BlessingOfVortexPlayer>().VortexCounter++;
		}
	}
	public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
		if (item.DamageType == DamageClass.Ranged) {
			crit += 7 * StackAmount(player);
		}
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.DamageType == DamageClass.Ranged && hit.Crit) {
			player.AddBuff<Buff_VortexBlessing>(BossRushUtils.ToSecond(2));
		}
	}
}
public class Buff_VortexBlessing : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void Update(Player player, ref int buffIndex) {
		BlessingOfVortexPlayer vortex = player.GetModPlayer<BlessingOfVortexPlayer>();
		player.ModPlayerStats().AddStatsToPlayer(PlayerStats.RangeDMG, 1 + .05f * vortex.VortexStack);
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		BlessingOfVortexPlayer vortex = player.GetModPlayer<BlessingOfVortexPlayer>();
		vortex.VortexStack++;
		return true;
	}
}
public class BlessingOfVortexPlayer : ModPlayer {
	public int VortexStack = 0;
	public int VortexCounter = 0;
	public override void ResetEffects() {
		if (!Player.HasBuff<Buff_VortexBlessing>()) {
			VortexStack = 0;
		}
	}
}
public class BlessingOfNebula : Perk {
	public override void SetDefaults() {
		textureString = BossRushUtils.GetTheSameTextureAsEntity<BlessingOfNebula>();
		list_category.Add(PerkCategory.Starter);
		CanBeStack = true;
		StackLimit = 3;
	}
	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) >= 2) {
			return DescriptionIndex(3);
		}
		return base.ModifyToolTip();
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<ChestLootDropPlayer>().UpdateMagicChanceMutilplier += 1f;
	}
	public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
		multi -= .11f * StackAmount(player);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextFloat() <= .06f * StackAmount(player) && proj.DamageType == DamageClass.Magic) {
			Item.NewItem(proj.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Star));
		}
	}
	public override void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) {
		mana.Base += 78 * StackAmount(player);
	}
	public override void OnPickUp(Player player, Item item) {
		if (item.type == ItemID.Star && StackAmount(player) >= 3 && Main.rand.NextBool(5)) {
			player.NebulaLevelup(Main.rand.Next(new int[] { BuffID.NebulaUpLife1, BuffID.NebulaUpDmg1, BuffID.NebulaUpMana1 }));
		}
	}
}
public class BlessingOfStardust : Perk {
	public override void SetDefaults() {
		textureString = BossRushUtils.GetTheSameTextureAsEntity<BlessingOfStardust>();
		list_category.Add(PerkCategory.Starter);
		CanBeStack = true;
		StackLimit = 3;
	}

	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) >= 2) {
			return DescriptionIndex(3);
		}
		return base.ModifyToolTip();
	}
	public override void UpdateEquip(Player player) {
		player.maxMinions += 1;
		player.maxTurrets += 1;
		player.GetModPlayer<ChestLootDropPlayer>().UpdateSummonChanceMutilplier += 1f;
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage.Base += (player.maxMinions + player.maxTurrets) / 2 * StackAmount(player);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (ProjectileID.Sets.IsAWhip[proj.type]) {
			target.AddBuff(ModContent.BuffType<StarGaze>(), BossRushUtils.ToSecond(Main.rand.Next(1, 4)));
			if (Main.rand.NextBool(4) && StackAmount(player) >= 3) {
				Vector2 spawnPos = target.Center + Main.rand.NextVector2CircularEdge(target.width + 32, target.height + 32);
				Projectile.NewProjectile(player.GetSource_OnHit(target), spawnPos, Main.rand.NextVector2CircularEdge(4, 4), ModContent.ProjectileType<StarDustProjectile>(), (int)(hit.Damage * .55f), 1f, player.whoAmI);
			}
		}
	}
}
public class StarGaze : ModBuff {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		return true;
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 15;
		if (Main.hardMode) {
			npc.lifeRegen -= 40;
		}
		if (npc.buffTime[buffIndex] == 0) {
			int damage = Math.Clamp((int)(npc.lifeMax * .01f), 0, 1000);
			npc.StrikeNPC(npc.CalculateHitInfo(damage, 1));
		}
	}
}
public class StarRay : ModBuff {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public static readonly int TagDamage = 10;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
		BuffID.Sets.IsATagBuff[Type] = true;
	}
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		return true;
	}
	public override void Update(NPC npc, ref int buffIndex) {
	}
}
public class StarDustProjectile : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentStardust);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 999;
		Projectile.extraUpdates = 5;
	}
	NPC targetNPC = null;
	public override bool? CanDamage() {
		return targetNPC != null && targetNPC.active;
	}
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.ai[0] <= 60) {
			if (Projectile.velocity == Vector2.Zero) {
				Projectile.ai[0]++;
			}
			else {
				if (Projectile.velocity.IsLimitReached(.1f)) {
					Projectile.velocity *= .987f;
				}
				else {
					Projectile.velocity = Vector2.Zero;
				}
			}
		}
		else {
			if (targetNPC == null) {
				Projectile.Center.LookForHostileNPC(out NPC npc, 1000);
				targetNPC = npc;
				if (targetNPC == null) {
					Projectile.ai[0] = 0;
					Projectile.ai[1] = 0;
				}
				return;
			}
			if (!targetNPC.active) {
				targetNPC = null;
				return;
			}
			if (Projectile.timeLeft < 300) {
				Projectile.timeLeft = 300;
			}
			Projectile.ai[1] = Math.Clamp(Projectile.ai[1], 0, 5);
			Projectile.velocity = (targetNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
			Projectile.ai[1] += .01f;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff<StarRay>(BossRushUtils.ToSecond(5));
		Vector2 pos = Projectile.Center;
		float randomrotation = Main.rand.NextFloat(90);
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		for (int i = 0; i < 4; i++) {
			Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * (3 + Main.rand.NextFloat());
			for (int l = 0; l < 16; l++) {
				float multiplier = Main.rand.NextFloat();
				float scale = MathHelper.Lerp(2.1f, .9f, multiplier);
				int dust = Dust.NewDust(pos + randomPosOffset, 0, 0, DustID.GemSapphire, 0, 0, 0, Main.rand.Next(new Color[] { Color.White, Color.Cyan }), scale);
				Main.dust[dust].velocity = Toward * multiplier;
				Main.dust[dust].noGravity = true;
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D mainTexture = TextureAssets.Projectile[Type].Value;
		Texture2D trail = ModContent.Request<Texture2D>(BossRushTexture.SMALLWHITEBALL).Value;
		Vector2 trailOrigin = trail.Size() * .5f;
		Vector2 mainOrigin = mainTexture.Size() * .5f;
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + mainOrigin;
			Color color = new Color(0, 90, 255) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color.A = 0;
			Main.EntitySpriteDraw(trail, drawPos, null, color, Projectile.rotation, trailOrigin, (Projectile.scale - k * .02f) * .5f, SpriteEffects.None, 0);
		}
		Vector2 mainDrawPos = Projectile.position - Main.screenPosition + mainOrigin;
		Main.EntitySpriteDraw(mainTexture, mainDrawPos, null, lightColor, Projectile.rotation, mainOrigin, Projectile.scale, SpriteEffects.None, 0);

		return false;
	}
}
public class BlessingOfSynergy : Perk {
	public override void SetDefaults() {
		list_category.Add(PerkCategory.Starter);
		textureString = BossRushTexture.ACCESSORIESSLOT;
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void OnChoose(Player player) {
		if (StackAmount(player) <= 1) {
			player.QuickSpawnItem(player.GetSource_FromThis("Perk"), ModContent.ItemType<SynergyEnergy>());
		}
		base.OnChoose(player);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().ChestLoot.WeaponAmountAddition += StackAmount(player);
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		if (player.GetModPlayer<SynergyModPlayer>().CompareOldvsNewItemType) {
			damage.Flat += 10 * StackAmount(player);
		}
	}
}
public class BlessingOfTitan : Perk {
	public override void SetDefaults() {
		list_category.Add(PerkCategory.Starter);
		textureString = BossRushTexture.ACCESSORIESSLOT;
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MaxHP, Flat: 50 * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Additive: 1.15f * StackAmount(player), Flat: 10);
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, Flat: 2f * StackAmount(player));
	}
}
public class BlessingOfPerk : Perk {
	public override void SetDefaults() {
		list_category.Add(PerkCategory.Starter);
		textureString = BossRushTexture.ACCESSORIESSLOT;
		CanBeStack = true;
		CanBeChoosen = false;
		Tooltip =
			"+ Increases perk range amount by 1";
		StackLimit = 999;
	}
	public override string ModifyToolTip() {
		if (StackAmount(Main.LocalPlayer) == 10) {
			return "don't you think it is enough now ?";
		}
		return base.ModifyToolTip();
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.LootDropIncrease, Base: 1 + StackAmount(player));
	}
}
public class BlessingOfEvasive : Perk {
	public override void SetDefaults() {
		list_category.Add(PerkCategory.Starter);
		textureString = BossRushTexture.ACCESSORIESSLOT;
		CanBeStack = true;
		StackLimit = 3;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		player.GetJumpState<SimpleExtraJump>().Enable();
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1 + .15f * StackAmount(player));
		modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1 + .25f * StackAmount(player));
		modplayer.DodgeChance += .04f * StackAmount(player);
	}
}
