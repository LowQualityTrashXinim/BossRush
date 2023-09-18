using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.ZapSnapper
{
    internal class ZapSnapper : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(56, 16, 12, 2f, 5, 5, ItemUseStyleID.Shoot, ProjectileID.ThunderSpearShot, 22, 4, true);

            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item9;
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            if (modplayer.ZapSnapper_Blowpipe)
                tooltips.Add(new TooltipLine(Mod, "ZapSnapper_Blowpipe", $"[i:{ItemID.Blowpipe}] shoot out seed"));
            if (modplayer.ZapSnapper_WeatherPain)
                tooltips.Add(new TooltipLine(Mod, "ZapSnapper_WeatherPain", $"[i:{ItemID.WeatherPain}] You sometime shoot out a super charge thunder shot"));
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.Blowpipe))
            {
                modplayer.SynergyBonus++;
                modplayer.ZapSnapper_Blowpipe = true;
            }
            if (player.HasItem(ItemID.WeatherPain))
            {
                modplayer.SynergyBonus++;
                modplayer.ZapSnapper_WeatherPain = true;
            }
        }
        public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = position.PositionOFFSET(velocity, 30);
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 newVec = velocity.Vector2RotateByRandom(10);
                int proj = Projectile.NewProjectile(source, position, newVec, ProjectileID.ThunderSpearShot, damage, knockback, player.whoAmI);
                Main.projectile[proj].DamageType = DamageClass.Magic;
                if (modplayer.ZapSnapper_Blowpipe && Main.rand.NextBool(5))
                    Projectile.NewProjectile(source, position, newVec.Vector2RotateByRandom(5), ProjectileID.Seed, damage, knockback, player.whoAmI);
            }
            if (modplayer.ZapSnapper_WeatherPain && Main.rand.NextBool(7))
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LightningStrike>(), damage * 4, knockback, player.whoAmI);
            }
            CanShootItem = false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 2);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RedRyder)
                .AddIngredient(ItemID.ThunderSpear)
                .Register();
        }
    }
    public class LightningStrike : ModProjectile
    {
        public override string Texture => BossRushTexture.SMALLWHITEBALL;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.hide = true;
            Projectile.timeLeft = 999;
            Projectile.penetrate = -1;
        }
        Vector2[] lightningPreSetPath = new Vector2[10];
        Vector2 finalPosition = Vector2.Zero;
        public override bool? CanDamage() => null;
        public override void AI()
        {
            if (Projectile.timeLeft > 1)
            {
                Projectile.timeLeft = 1;
                Vector2 path = Projectile.Center;
                for (int i = 0; i < lightningPreSetPath.Length; i++)
                {
                    lightningPreSetPath[i] = path;
                    Projectile.velocity = Projectile.velocity.Vector2RotateByRandom(Main.rand.NextFloat(25, 35));
                    path = path.PositionOFFSET(Projectile.velocity, Main.rand.NextFloat(75, 100));

                    float length = Vector2.Distance(lightningPreSetPath[i], path);
                    for (int l = 0; l < 50; l++)
                    {
                        int dust = Dust.NewDust(lightningPreSetPath[i].PositionOFFSET(Projectile.velocity, Main.rand.NextFloat(length)), 0, 0, DustID.Electric);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
                        Main.dust[dust].fadeIn = .1f;
                        Main.dust[dust].velocity = Vector2.Zero;
                    }
                    if (i == lightningPreSetPath.Length - 1)
                    {
                        finalPosition = path;
                    }
                }
                Projectile.velocity = Vector2.Zero;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < lightningPreSetPath.Length; i++)
            {
                if (i == 0)
                {
                    if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, lightningPreSetPath[i]))
                        return true;
                    continue;
                }
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lightningPreSetPath[i - 1], lightningPreSetPath[i]))
                    return true;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void Kill(int timeLeft)
        {
            finalPosition.LookForHostileNPC(out List<NPC> npclist, 100);
            for (int i = 0; i < 150; i++)
            {
                int dust = Dust.NewDust(finalPosition, 0, 0, DustID.Electric);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = .1f;
                Main.dust[dust].scale = Main.rand.NextFloat(.5f, 1.5f);
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(10, 10);
            }
            foreach (var npc in npclist)
            {
                npc.StrikeNPC(npc.CalculateHitInfo(Projectile.damage * 2, 0));
                Main.player[Projectile.owner].addDPS(Projectile.damage * 2);
            }
        }
    }
}