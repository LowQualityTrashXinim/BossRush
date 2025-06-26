using BossRush.Common.Global;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Contents.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Transfixion.WeaponEnchantment.ShortSwordEnchantments;
public class ModPlayer_ShortSwordEnchantment : ModPlayer {
	public int ShortSwordValue = 0;
	public int ShortSwordCapacity = 3;
	public int ShortSwordCoolDownHit = 0;
	public float ShortSwordIndexesGotHit = -1;
	public override void ResetEffects() {
		ShortSwordCapacity = 3;
		ShortSwordValue = BossRushUtils.CountDown(ShortSwordValue);
		ShortSwordCoolDownHit = BossRushUtils.CountDown(ShortSwordCoolDownHit);
	}
	public override void UpdateDead() {
		ShortSwordValue = 0;
		ShortSwordCapacity = 3;
		ShortSwordCoolDownHit = 0;
		ShortSwordIndexesGotHit = -1;
	}
}
public class Enchantment_ShortSwordProjectile : ModProjectile {
	public int ItemTextureID = -1;
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperShortsword);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 100;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
	}
	public override bool? CanDamage() {
		return Main.player[Projectile.owner].GetModPlayer<ModPlayer_ShortSwordEnchantment>().ShortSwordCoolDownHit <= 0;
	}
	public override void AI() {
		var player = Main.player[Projectile.owner];
		if (player.GetModPlayer<ModPlayer_ShortSwordEnchantment>().ShortSwordCoolDownHit == 59) {
			float index = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>().ShortSwordIndexesGotHit;
			if (Projectile.ai[0] >= index) {
				Projectile.ai[0]--;
			}
		}
		if (Projectile.timeLeft <= 50) {
			Projectile.timeLeft = 50;
		}
		float amount = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
		float Degree;
		if (amount > 0) {
			Degree = 360f / amount * Projectile.ai[0];
		}
		else {
			Degree = 0;
		}
		var toward = Vector2.One.RotatedBy(MathHelper.ToRadians(Degree + player.GetModPlayer<BossRushUtilsPlayer>().counterToFullPi * 2));
		Projectile.Center = player.Center + toward * (100 - Projectile.timeLeft);
		Projectile.rotation = toward.ToRotation() + MathHelper.PiOver4;
	}
	public override void OnKill(int timeLeft) {
		var player = Main.player[Projectile.owner];
		player.GetModPlayer<ModPlayer_ShortSwordEnchantment>().ShortSwordIndexesGotHit = Projectile.ai[0];
		if (player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= 1) {
			player.GetModPlayer<ModPlayer_ShortSwordEnchantment>().ShortSwordCoolDownHit = 0;
		}
		else {
			player.GetModPlayer<ModPlayer_ShortSwordEnchantment>().ShortSwordCoolDownHit = 60;
		}

	}
	public override bool PreDraw(ref Color lightColor) {
		if (Main.player[Projectile.owner].GetModPlayer<ModPlayer_ShortSwordEnchantment>().ShortSwordCoolDownHit % 5 != 0) {
			return false;
		}
		if (ItemTextureID == -1) {
			return base.PreDraw(ref lightColor);
		}
		Main.instance.LoadItem(ItemTextureID);
		Main.instance.LoadProjectile(Type);
		var texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemTextureID)).Value;
		var origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
		var drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
public class CopperShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.CopperShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritDamage, 1.35f);
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritDamage, Base: 5);
		player.GetModPlayer<GlobalItemPlayer>().ShortSword_ThrownCD *= 0;
		globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		modplayer.ShortSwordCapacity++;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool() && hit.Crit && globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var position = target.Center + Main.rand.NextVector2CircularEdge(50 + target.width, 50 + target.height);
			var vel = (target.Center - position).SafeNormalize(Vector2.Zero) * 10;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
				position,
				vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI, ai2: -120);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile2 shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool() && hit.Crit && globalItem.Item_Counter1[index] <= 0) {
			globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 60);
			var position = target.Center + Main.rand.NextVector2CircularEdge(50 + target.width, 50 + target.height);
			var vel = (target.Center - position).SafeNormalize(Vector2.Zero) * 10;
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
				position,
				vel, ModContent.ProjectileType<SwordProjectile2>(), hit.Damage, hit.Knockback, player.whoAmI, ai2: -120);
			if (Main.projectile[projectile].ModProjectile is SwordProjectile2 shortproj)
				shortproj.ItemIDtextureValue = ItemIDType;
		}
	}
}
public class TinShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TinShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		modplayer.ShortSwordCapacity++;
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f * player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 300;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage * 3, 0, player.whoAmI, indexProj);
			if (Main.projectile[proj].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& proj.type != ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 300;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage * 3, 0, player.whoAmI, indexProj);
			if (Main.projectile[projectile].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
	}
}
public class IronShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.IronShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		modplayer.ShortSwordCapacity++;
		var statplayer = player.GetModPlayer<PlayerStatsHandle>();
		statplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 3 * player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()]);
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 30;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, indexProj);
			if (Main.projectile[proj].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& proj.type != ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 300;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, indexProj);
			if (Main.projectile[projectile].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
	}
}
public class LeadShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.LeadShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		modplayer.ShortSwordCapacity++;
		player.GetModPlayer<PlayerStatsHandle>().DebuffDamage += .08f * player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 300;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, indexProj);
			if (Main.projectile[proj].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& proj.type != ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 300;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, indexProj);
			if (Main.projectile[projectile].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
	}
}
public class SilverShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.SilverShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		modplayer.ShortSwordCapacity++;
		player.GetModPlayer<PlayerStatsHandle>().UpdateFullHPDamage += .4f;
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.CheckFirstStrike()) {
			var vel = (target.Center - player.Center).SafeNormalize(Vector2.Zero) * 10;
			Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ThrowShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, ai2: ItemIDType);
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.CheckFirstStrike()) {
			var vel = (target.Center - player.Center).SafeNormalize(Vector2.Zero) * 10;
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, vel, ModContent.ProjectileType<ThrowShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, ai2: ItemIDType);
		}
	}
}
public class TungstenShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.TungstenShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		modplayer.ShortSwordCapacity++;
		if (player.ItemAnimationActive) {
			globalItem.Item_Counter1[index] = BossRushUtils.CountDown(globalItem.Item_Counter1[index]);
			if (globalItem.Item_Counter1[index] <= 0) {
				globalItem.Item_Counter1[index] = PlayerStatsHandle.WE_CoolDown(player, 24);
				var vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 5;
				var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.Center + Main.rand.NextVector2Circular(20, 20), vel, ModContent.ProjectileType<ThrowShortSwordProjectile>(), (int)(item.damage * .34f), item.knockBack, player.whoAmI, ai2: ItemIDType);
				proj.penetrate = 1;
				proj.aiStyle = -1;
			}
		}
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += 3;
	}
}
public class GoldShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.GoldShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		modplayer.ShortSwordCapacity++;
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f * player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 30;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, indexProj);
			if (Main.projectile[proj].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
		if (Main.rand.NextBool(10)) {
			target.AddBuff(BuffID.Midas, BossRushUtils.ToSecond(Main.rand.Next(4, 6)));
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& proj.type != ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 30;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, indexProj);
			if (Main.projectile[projectile].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
		if (Main.rand.NextBool(10)) {
			target.AddBuff(BuffID.Midas, BossRushUtils.ToSecond(Main.rand.Next(4, 6)));
		}
	}
}
public class PlatinumShortSword : ModEnchantment {
	public override void SetDefaults() {
		ItemIDType = ItemID.PlatinumShortsword;
	}
	public override void UpdateHeldItem(int index, Item item, EnchantmentGlobalItem globalItem, Player player) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		modplayer.ShortSwordCapacity++;
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .25f * player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
	}
	public override void ModifyDamage(int index, Player player, EnchantmentGlobalItem globalItem, Item item, ref StatModifier damage) {
		damage.Base += player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
	}
	public override void OnHitNPCWithItem(int index, Player player, EnchantmentGlobalItem globalItem, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 30;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, indexProj);
			if (Main.projectile[proj].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
	}
	public override void OnHitNPCWithProj(int index, Player player, EnchantmentGlobalItem globalItem, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		var modplayer = player.GetModPlayer<ModPlayer_ShortSwordEnchantment>();
		if (modplayer.ShortSwordValue <= 0
			&& player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()] <= modplayer.ShortSwordCapacity
			&& proj.type != ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()
			&& modplayer.ShortSwordCoolDownHit <= 0) {
			modplayer.ShortSwordValue = 30;
			float indexProj = player.ownedProjectileCounts[ModContent.ProjectileType<Enchantment_ShortSwordProjectile>()];
			int projectile = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
				player.Center,
				Vector2.Zero, ModContent.ProjectileType<Enchantment_ShortSwordProjectile>(), hit.Damage, hit.Knockback, player.whoAmI, indexProj);
			if (Main.projectile[projectile].ModProjectile is Enchantment_ShortSwordProjectile shortproj) {
				shortproj.ItemTextureID = ItemIDType;
			}
		}
	}
}
