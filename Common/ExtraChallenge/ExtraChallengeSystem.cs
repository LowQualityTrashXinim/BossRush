using BossRush.Contents.Perks;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace BossRush.Common.ExtraChallenge {
	internal class ExtraChallengerChooserUI : UIState {
		public int whoAmI = -1;
		public override void OnActivate() {
			base.OnActivate();
			Elements.Clear();
			if (whoAmI == -1)
				return;
			Player player = Main.player[whoAmI];
			if (player.TryGetModPlayer(out PerkPlayer modplayer)) {
				List<int> listOfPerk = new List<int>();
				for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
					if (modplayer.perks.ContainsKey(i)) {
						if ((!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0) ||
							modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
							continue;
						}
					}
					listOfPerk.Add(i);
				}
				Texture2D textureDefault = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT).Value;
				Vector2 originDefault = new Vector2(textureDefault.Width * .5f, textureDefault.Height * .5f);
				for (int i = 0; i < 3; i++) {
					int newperk = Main.rand.Next(listOfPerk);
					Asset<Texture2D> texture;
					if (ModPerkLoader.GetPerk(newperk).textureString is not null)
						texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
					else
						texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
					Vector2 origin = new Vector2(26, 26);
					listOfPerk.Remove(newperk);
					//After that we assign perk
					PerkUIImageButton btn = new PerkUIImageButton(texture, modplayer);
					btn.perkType = newperk;
					btn.Width.Pixels = 52;
					btn.Height.Pixels = 52;
					Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(modplayer.PerkAmount, 180, i) * 150;
					Vector2 drawpos = player.Center + offsetPos - Main.screenPosition - origin;
					btn.Left.Pixels = drawpos.X;
					btn.Top.Pixels = drawpos.Y;
					Append(btn);
				}
			}
		}
	}
	class ExtraChallengerChooserUIButton : UIImageButton {
		PerkPlayer perkplayer;
		public int perkType;
		private UIText toolTip;
		public ExtraChallengerChooserUIButton(Asset<Texture2D> texture, PerkPlayer perkPlayer) : base(texture) {
			Width.Pixels = texture.Value.Width;
			Height.Pixels = texture.Value.Height;
			perkplayer = perkPlayer;
		}
		public override void OnActivate() {
			base.OnActivate();
			toolTip = new UIText("");
			toolTip.HAlign = .5f;
			Append(toolTip);
		}
		public override void LeftClick(UIMouseEvent evt) {
			base.LeftClick(evt);
			//We are assuming the perk are auto handle

			if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perkType))
				perkplayer.perks.Add(perkType, 1);
			else
				if (perkplayer.perks.ContainsKey(perkType) && ModPerkLoader.GetPerk(perkType).CanBeStack)
				perkplayer.perks[perkType] = perkplayer.perks[perkType] + 1;

		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (toolTip is null) {
				return;
			}
			if (IsMouseHovering) {
				toolTip.Left.Pixels = Main.MouseScreen.X - Left.Pixels;
				toolTip.Top.Pixels = Main.MouseScreen.Y - Top.Pixels - 20;
				toolTip.SetText(ModPerkLoader.GetPerk(perkType).Tooltip);
			}
			else {
				toolTip.SetText("");
			}
		}
	}
	internal class ExtraChallengeGlobalNPCModifier : GlobalNPC {
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
			if (ExtraChallengeSystem.ListChallengeID.Contains(1)) {
				spawnRate = (int)(spawnRate * .5f);
			}
		}
		public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
			OnHitEffectNPC(npc);
		}
		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
			OnHitEffectNPC(npc);
		}
		private void OnHitEffectNPC(NPC npc) {
			if (ExtraChallengeSystem.ListChallengeID.Contains(7)) {
				if (Main.projectile.Where(x => x.ModProjectile is ExChallengeProjectile_BossShootHomeIn && x.active).Any()) {
					return;
				}
				int amount = Main.rand.Next(1, 6);
				for (int i = 0; i < amount; i++) {
					Vector2 vel = Vector2.One.Vector2DistributeEvenly(amount, 360, i);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel, ModContent.ProjectileType<ExChallengeProjectile_BossShootHomeIn>(), npc.damage, 0, -1, npc.target);
				}
			}
		}
		public override void PostAI(NPC npc) {
			base.PostAI(npc);
			if (ExtraChallengeSystem.ListChallengeID.Contains(2)) {
				Player player = Main.player[npc.target];
				if (npc.boss && (npc.type != NPCID.EaterofWorldsBody
					&& npc.type != NPCID.EaterofWorldsHead
					&& npc.type != NPCID.EaterofWorldsTail)
					&& player.GetModPlayer<ExtraChallengePlayer>().spawnPos == Vector2.Zero) {
					player.GetModPlayer<ExtraChallengePlayer>().spawnPos = player.Center;
				}
			}
		}
		public override void OnKill(NPC npc) {
			if (!ModContent.GetInstance<BossRushModConfig>().ExtraChallenge) {
				return;
			}
			if (npc.boss) {
				if (ExtraChallengeSystem.ListChallengeID.Count != ExtraChallengeSystem.readOnlyList.Count) {
					List<int> list = ExtraChallengeSystem.readOnlyList;
					if (ExtraChallengeSystem.ListChallengeID.Count < 1)
						ExtraChallengeSystem.ListChallengeID.Add(Main.rand.Next(list));
					else {
						foreach (var item in ExtraChallengeSystem.ListChallengeID) {
							if (list.Contains(item))
								list.Remove(item);
						}
						ExtraChallengeSystem.ListChallengeID.Add(Main.rand.Next(list));
					}
				}
			}
			if (ExtraChallengeSystem.ListChallengeID.Contains(2)) {
				if (npc.boss && !BossRushUtils.IsAnyVanillaBossAlive()) {
					Player player = Main.player[npc.target];
					player.GetModPlayer<ExtraChallengePlayer>().spawnPos = Vector2.Zero;
				}
			}
		}
	}
	enum ExChallenge {
		HighStress,//Done
		Horde,//Done
		LimitedArena,//Done
		KeepMoving,//Done
		StopMoving,//Done
		EverythingIsClassless,//Done
		BeingHuntDown,//Done
		BossShootHomeIn,//Done
	}
	internal class ExtraChallengeSystem : ModSystem {
		public static readonly List<int> readOnlyList = new List<int>
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
		public override void PostUpdateNPCs() {
			base.PostUpdateNPCs();
		}
		public override void PostUpdatePlayers() {
			base.PostUpdatePlayers();
		}
		public override void PostUpdateWorld() {
			ExChallenge_BeingHuntDown();
		}
		private void ExChallenge_BeingHuntDown() {
			if (ListChallengeID.Contains(6)) {
				if (Main.projectile.Where(x => x.ModProjectile is ExChallengeProjectile_BeingHuntDown).Any()) {
					return;
				}
				Projectile.NewProjectile(
					Entity.GetSource_NaturalSpawn(),
					Main.LocalPlayer.Center + Main.rand.NextVector2CircularEdge(1000, 1000),
					Vector2.Zero,
					ModContent.ProjectileType<ExChallengeProjectile_BeingHuntDown>(),
					100, 0, -1, Main.LocalPlayer.whoAmI
					);
			}
		}
		public static List<int> ListChallengeID = new List<int>();
		public override void ClearWorld() {
			ListChallengeID.Clear();
		}
		public override void SaveWorldData(TagCompound tag) {
			if (ListChallengeID.Count > 0)
				tag.Add("ListChallengeID", ListChallengeID);
		}
		public override void LoadWorldData(TagCompound tag) {
			ListChallengeID = tag.Get<List<int>>("ListChallengeID");
		}
	}
	public class ExtraChallengePlayer : ModPlayer {
		public Vector2 spawnPos = Vector2.Zero;
		public override void PostUpdate() {
			if (ExtraChallengeSystem.ListChallengeID.Contains(2)) {
				if (spawnPos != Vector2.Zero) {
					if (!spawnPos.IsCloseToPosition(Player.Center, 2000)) {
						Player.statLife -= 10;
					}
					for (int i = 0; i < 100; i++) {
						int dust = Dust.NewDust(spawnPos + Main.rand.NextVector2CircularEdge(2000, 2000), 0, 0, DustID.AncientLight);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].fadeIn = 2;
						Main.dust[dust].scale = Main.rand.NextFloat(1, 1.25f);
					}
				}
			}
			if (ExtraChallengeSystem.ListChallengeID.Contains(3)) {
				if (Player.velocity == Vector2.Zero)
					Player.statLife -= 1;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (ExtraChallengeSystem.ListChallengeID.Contains(4)) {
				if (Player.velocity != Vector2.Zero)
					damage *= Main.rand.NextFloat(0.15f, 1.15f);
			}
		}
		public override void PostItemCheck() {
			if (ExtraChallengeSystem.ListChallengeID.Contains(5)) {
				Player.HeldItem.DamageType = DamageClass.Generic;
			}
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			base.OnHitByNPC(npc, hurtInfo);
			OnHitByAnything();
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			base.OnHitByProjectile(proj, hurtInfo);
			OnHitByAnything();
		}
		private void OnHitByAnything() {
			if (ExtraChallengeSystem.ListChallengeID.Contains(0)) {
				if (Player.statLife >= 50)
					Player.statLife = 1;
			}
		}
	}
	public class ExChallengeProjectile_BossShootHomeIn : ModProjectile {
		public override string Texture => BossRushTexture.SMALLWHITEBALL;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		}
		Color color;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 10;
			color = new Color(Main.rand.Next(0, 256), Main.rand.Next(0, 256), Main.rand.Next(0, 256));
		}
		public override void AI() {
			Lighting.AddLight(Projectile.Center, new Vector3(1, 1, 1));
			if (Projectile.ai[0] >= 0) {
				Projectile.velocity += (Main.player[(int)Projectile.ai[0]].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .0025f;
			}
			Projectile.velocity = Projectile.velocity.LimitedVelocity(10);
			if (Main.rand.NextBool(4)) {
				int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(15, 15), 0, 0, DustID.GemDiamond);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = color;
			}
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 50; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond, 0, 0, 0, color, Main.rand.NextFloat(.5f, 2f));
				Vector2 vel = Main.rand.NextVector2Circular(3, 3);
				Main.dust[dust].velocity = vel;
				Main.dust[dust].color = color;
			}
		}
		public override Color? GetAlpha(Color lightColor) {
			return color;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrailWithoutColorAdjustment(color, .01f);
			return true;
		}
	}
	public class ExChallengeProjectile_BeingHuntDown : ModProjectile {
		public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 30;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 10;
		}
		public override void AI() {
			Lighting.AddLight(Projectile.Center, new Vector3(1, 1, 1));
			Projectile.velocity = Projectile.velocity.LimitedVelocity(10);
			Projectile.velocity += (Main.player[(int)Projectile.ai[0]].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .0025f;
			if (Main.rand.NextBool(4)) {
				int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(30, 30), 0, 0, DustID.Granite);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Color.Black;
			}
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 150; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(.5f, 2f));
				Vector2 vel = Main.rand.NextVector2Circular(5, 5);
				Main.dust[dust].velocity = vel;
				Main.dust[dust].color = Color.Black;
			}
			base.OnKill(timeLeft);
		}
		public override Color? GetAlpha(Color lightColor) {
			return Color.Black;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrailWithoutColorAdjustment(Color.Black, .02f);
			return true;
		}
	}
}