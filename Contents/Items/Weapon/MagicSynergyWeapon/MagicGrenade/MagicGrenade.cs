using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.MagicGrenade
{
    internal class MagicGrenade : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(10, 10, 75, 3f, 25, 25, ItemUseStyleID.Swing, ModContent.ProjectileType<MagicGrenadeProjectile>(), 12, 30, true);
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            if (modplayer.MagicGrenade_MagicMissle)
                tooltips.Add(new TooltipLine(Mod, "MagicGrenade_MagicMissle", $"[i:{ItemID.MagicMissile}] Grenade's explosion will be accompany by magical bolt that explode shortly after"));
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.MagicMissile))
            {
                modplayer.SynergyBonus++;
                modplayer.MagicGrenade_MagicMissle = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Grenade)
                .AddIngredient(ItemID.AleThrowingGlove)
                .Register();
        }
    }
    public class MagicGrenadeProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<MagicGrenade>();
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 54;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3());
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length() * .5f) * (Projectile.velocity.X > 0 ? 1 : -1);
            }
            for (int i = 0; i < 4; i++)
            {
                Vector2 vel = (Projectile.rotation + MathHelper.ToRadians(90 * i)).ToRotationVector2();
                int dust = Dust.NewDust(Projectile.Center.PositionOFFSET(vel, 25), 0, 0, DustID.GemAmethyst, 0, 0, 0, Color.White, Main.rand.NextFloat(.75f, 1));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = vel * .25f;
            }
            int dustnovel = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst, 0, 0, 0, Color.White, 2);
            Main.dust[dustnovel].noGravity = true;
            Main.dust[dustnovel].velocity = Vector2.Zero;
            Main.dust[dustnovel].fadeIn = 2;
            if (Projectile.ai[0] <= 15)
            {
                Projectile.ai[0]++;
                return;
            }
            Projectile.velocity.Y += .5f;
        }
        public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft)
        {
            float randomrotation = Main.rand.NextFloat(90);
            Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation));
                for (int l = 0; l < 150; l++)
                {
                    float multiplier = Main.rand.NextFloat();
                    float scale = MathHelper.Lerp(1.1f, .1f, multiplier) + 1f;
                    float randomrotate = MathHelper.Lerp(50f, 1f, BossRushUtils.InOutSine(multiplier));
                    int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemAmethyst, 0, 0, 0, Color.White, scale);
                    Main.dust[dust].velocity = Toward.RotatedByRandom(MathHelper.ToRadians(randomrotate)) * multiplier * 15;
                    Main.dust[dust].noGravity = true;
                    if (l % 3 == 0)
                    {
                        int dust2 = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemAmethyst, 0, 0, 0, Color.White, scale);
                        Main.dust[dust2].velocity = Main.rand.NextVector2CircularEdge(10, 10);
                        Main.dust[dust2].noGravity = true;
                    }
                }
            }
            for (int i = 0; i < 7; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    Projectile.Center,
                    new Vector2(Main.rand.NextFloat(-2, 2), -Main.rand.NextFloat(1, 2)) * Main.rand.NextFloat(.85f, 2f),
                    ModContent.ProjectileType<CompoundGrenadeProjectile>(),
                    (int)(Projectile.damage * .5f),
                    Projectile.knockBack,
                    Projectile.owner);
            }
            if (modplayer.MagicGrenade_MagicMissle)
            {
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.Vector2DistributeEvenly(4, 360, i).Vector2RotateByRandom(120), ModContent.ProjectileType<MagicalExplosionBolt>(), Projectile.damage, 1, Projectile.owner, 0, Main.rand.NextBool().BoolOne());
                }
            }
            Projectile.Center.LookForHostileNPC(out List<NPC> npc, 200);
            if (npc.Count < 1)
            {
                return;
            }
            for (int i = 0; i < npc.Count; i++)
            {
                npc[i].StrikeNPC(npc[i].CalculateHitInfo(Projectile.damage, (Projectile.Center.X < npc[i].Center.X).BoolOne(), Main.rand.NextBool(Projectile.CritChance), Projectile.knockBack * 2, Projectile.DamageType, true, player.luck));
                player.dpsDamage += Projectile.damage;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
    }
    class CompoundGrenadeProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 100;
            Projectile.penetrate = 1;
            Projectile.hide = true;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Lighting.AddLight(Projectile.Center, .1f, 0, .1f);
            if (Projectile.ai[0] <= 15)
            {
                Projectile.ai[0]++;
                return;
            }
            Projectile.velocity.X -= Projectile.velocity.X * .01f;
            if (Projectile.velocity.Y < 5)
            {
                Projectile.velocity.Y += .1f;
            }
            if (!Main.rand.NextBool(4))
            {
                return;
            }
            float randomrotation = Main.rand.NextFloat(90);
            Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation));
                for (int l = 0; l < 8; l++)
                {
                    float multiplier = Main.rand.NextFloat();
                    float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
                    int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemAmethyst, 0, 0, 0, Main.rand.Next(new Color[] { Color.White, Color.Purple }), scale);
                    Main.dust[dust].velocity = Toward * multiplier;
                    Main.dust[dust].noGravity = true;
                }
            }
            base.SynergyAI(player, modplayer);
        }
        public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft)
        {
            float randomrotation = Main.rand.NextFloat(90);
            Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation));
                for (int l = 0; l < 30; l++)
                {
                    float multiplier = Main.rand.NextFloat();
                    float scale = MathHelper.Lerp(1.1f, .1f, multiplier) + 1f;
                    float randomrotate = MathHelper.Lerp(50f, 1f, BossRushUtils.InOutSine(multiplier));
                    int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemAmethyst, 0, 0, 0, Color.White, scale);
                    Main.dust[dust].velocity = Toward.RotatedByRandom(MathHelper.ToRadians(randomrotate)) * multiplier * 6;
                    Main.dust[dust].noGravity = true;
                    if (l % 3 == 0)
                    {
                        int dust2 = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemAmethyst, 0, 0, 0, Color.White, scale);
                        Main.dust[dust2].velocity = Main.rand.NextVector2CircularEdge(5, 5);
                        Main.dust[dust2].noGravity = true;
                    }
                }
            }
            Projectile.Center.LookForHostileNPC(out List<NPC> npc, 150);
            if (npc.Count < 1)
            {
                return;
            }
            for (int i = 0; i < npc.Count; i++)
            {
                player.StrikeNPCDirect(npc[i], npc[i].CalculateHitInfo(Projectile.damage, (Projectile.Center.X < npc[i].Center.X).BoolOne(), Main.rand.NextBool(Projectile.CritChance), Projectile.knockBack, Projectile.DamageType, true, player.luck));
            }
        }
    }
    class MagicalExplosionBolt : ModProjectile
    {
        public override string Texture => BossRushTexture.SMALLWHITEBALL;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 800;
            Projectile.extraUpdates = 10;
            Projectile.penetrate = 1;
        }
        int direction = 0;
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(5, 5), 0, 0, DustID.GemAmethyst);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].scale = Main.rand.NextFloat(.55f, .85f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].fadeIn = .5f;
            if (Projectile.timeLeft > 300)
            {
                Projectile.timeLeft = 300;
            }
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(.5f * Projectile.ai[1]));
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center.LookForHostileNPC(out List<NPC> npc, 50);
            for (int i = 0; i < 4; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.Vector2DistributeEvenly(4, 360, i), ModContent.ProjectileType<SmallerMagicalExplosionBolt>(), (int)(Projectile.damage * .5f), 1, Projectile.owner, 0, Main.rand.NextBool().BoolOne());
            }
            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(5, 5), 0, 0, DustID.GemAmethyst);
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                Main.dust[dust].scale = Main.rand.NextFloat(.55f, .75f);
                Main.dust[dust].fadeIn = 2f;
                Main.dust[dust].noGravity = true;
            }
            if (npc.Count < 1)
            {
                return;
            }
            for (int i = 0; i < npc.Count; i++)
            {
                player.StrikeNPCDirect(npc[i], npc[i].CalculateHitInfo(Projectile.damage, (Projectile.Center.X < npc[i].Center.X).BoolOne(), Main.rand.NextBool(Projectile.CritChance), Projectile.knockBack, Projectile.DamageType, true, player.luck));
            }
        }
    }
    class SmallerMagicalExplosionBolt : ModProjectile
    {
        public override string Texture => BossRushTexture.SMALLWHITEBALL;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 5;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 150;
            Projectile.extraUpdates = 5;
            Projectile.penetrate = 1;
        }
        int direction = 0;
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(2, 2), 0, 0, DustID.GemAmethyst);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].scale = Main.rand.NextFloat(.35f, .55f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].fadeIn = .5f;
            if (Projectile.timeLeft > 100)
            {
                Projectile.timeLeft = 100;
            }
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(1 * Projectile.ai[1]));
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 25; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(2, 2), 0, 0, DustID.GemAmethyst);
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                Main.dust[dust].scale = Main.rand.NextFloat(.55f, .75f);
                Main.dust[dust].fadeIn = 1.5f;
                Main.dust[dust].noGravity = true;
            }
            Projectile.Center.LookForHostileNPC(out List<NPC> npc, 20);
            if (npc.Count < 1)
            {
                return;
            }
            for (int i = 0; i < npc.Count; i++)
            {
                player.StrikeNPCDirect(npc[i], npc[i].CalculateHitInfo(Projectile.damage, (Projectile.Center.X < npc[i].Center.X).BoolOne(), Main.rand.NextBool(Projectile.CritChance), Projectile.knockBack, Projectile.DamageType, true, player.luck));
            }
        }
    }
}