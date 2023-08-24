using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.OrbOfEnergy
{
    internal class OrbOfEnergy : SynergyModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(1, 1, 100, 10, 5, 5, ItemUseStyleID.HoldUp, ModContent.ProjectileType<OrbOfEnergyBolt>(), 5, 20, true);
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            base.ModifySynergyToolTips(ref tooltips, modplayer);
            if (modplayer.OrbOfEnergy_BookOfSkulls)
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.BookofSkulls}] Energy lighting can home in toward enemy"));
            if (modplayer.OrbOfEnergy_DD2LightningAuraT1Popper)
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.DD2LightningAuraT1Popper}] Energy lighting are much more deadly"));
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.BookofSkulls))
                modplayer.OrbOfEnergy_BookOfSkulls = true;
            if (player.HasItem(ItemID.DD2LightningAuraT1Popper))
                modplayer.OrbOfEnergy_DD2LightningAuraT1Popper = true;

        }
        public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = position.PositionOFFSET(velocity, 20);
            position.Y -= 20;
            velocity = velocity.NextVector2RotatedByRandom(10);
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DD2LightningAuraT1Popper)
                .AddIngredient(ItemID.ThunderSpear)
                .Register();
        }
    }
    class OrbOfEnergyBolt : SynergyModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hide = true;
            Projectile.extraUpdates = 10;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, Main.rand.Next(new int[] { DustID.Electric, DustID.GemSapphire }));
            Main.dust[dust].scale = Main.rand.NextFloat(.3f, .75f);
            Main.dust[dust].velocity = Vector2.Zero;
            if (Projectile.timeLeft % 10 == 0)
            {
                Projectile.velocity = Projectile.velocity.NextVector2RotatedByRandom(90);
                Projectile.damage += 5;
            }
            if (modplayer.OrbOfEnergy_BookOfSkulls)
                if (Projectile.Center.LookForHostileNPC(out NPC npc, 900))
                    Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .25f;
            Projectile.velocity = Projectile.velocity.LimitedVelocity(10);
            if (Projectile.timeLeft % 20 != 0)
                return;
            if (modplayer.OrbOfEnergy_DD2LightningAuraT1Popper)
            {
                Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 100f);
                foreach (var npc in npclist)
                {
                    npc.StrikeNPC(npc.CalculateHitInfo((int)(Projectile.damage * .2f), 1));
                }
                Projectile.damage += 10;
            }
        }
    }
}