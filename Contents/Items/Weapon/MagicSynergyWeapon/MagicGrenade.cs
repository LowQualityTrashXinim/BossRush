using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon
{
    internal class MagicGrenade : SynergyModItem, ISynergyItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(10, 10, 75, 3f, 25, 25, ItemUseStyleID.Swing, ModContent.ProjectileType<MagicGrenadeProjectile>(), 12, 30, true);
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Grenade)
                .AddIngredient(ItemID.AleThrowingGlove)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
    public class MagicGrenadeProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
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
                    Main.dust[dust].velocity = (Toward.RotatedByRandom(MathHelper.ToRadians(randomrotate)) * multiplier * 15);
                    Main.dust[dust].noGravity = true;
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
            Projectile.Center.LookForHostileNPC(out List<NPC> npc, 50);
            if (npc.Count < 1)
            {
                return;
            }
            for (int i = 0; i < npc.Count; i++)
            {
                npc[i].StrikeNPC(npc[i].CalculateHitInfo(Projectile.damage, (Projectile.Center.X > npc[i].Center.X).BoolOne(), Main.rand.NextBool(Projectile.CritChance), Projectile.knockBack, Projectile.DamageType, true, player.luck));
            }
        }
    }
    class CompoundGrenadeProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
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
            if (!Main.rand.NextBool(5))
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
                }
            }
            base.SynergyAI(player, modplayer);
        }
    }
}