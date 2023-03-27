using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Items.Weapon.RangeSynergyWeapon.IceStorm;
using System.ComponentModel.Design;
using System.Linq;
using BossRush.Items.Accessories.GuideToMasterNinja;

namespace BossRush.Items
{
    abstract class BaseSynergyHandleItem : GlobalItem
    {
        public virtual bool PreHoldItem(Item item, Player player)
        {
            return true;
        }
        public virtual void NormalHoldItem(Item item, Player player)
        {

        }
        public virtual void PostHoldItem(Item item, Player player)
        {

        }
        public override void HoldItem(Item item, Player player)
        {
            if (PreHoldItem(item, player))
            {
                NormalHoldItem(item, player);
            }
            PostHoldItem(item, player);
        }
        public virtual bool PreShoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public virtual void ShootNormal(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

        }
        public virtual void PostShoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (PreShoot(item, player, source, position, velocity, type, damage, knockback)) ;
            {
                ShootNormal(item, player, source, position, velocity, type, damage, knockback);
            }
            PostShoot(item, player, source, position, velocity, type, damage, knockback);
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
    class SynergyBehaviorHandleItem : BaseSynergyHandleItem
    {
        public override void ShootNormal(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ModContent.ItemType<IceStorm>())
            {
                IceStormSynergy(player, source, position, velocity, damage, knockback);
            }
        }
        public override void NormalHoldItem(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<IceStorm>())
            {

                if (player.HasItem(ItemID.SnowballCannon))
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<IceStormSnowBallCannonMinion>()] < 1)
                    {
                        Projectile.NewProjectile(
                            item.GetSource_FromThis(),
                            player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<IceStormSnowBallCannonMinion>(),
                            player.GetWeaponDamage(item),
                            player.GetWeaponKnockback(item),
                            player.whoAmI);
                    }
                }
                if (player.HasItem(ItemID.FlowerofFrost))
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<IceStormFrostFlowerMinion>()] < 1)
                    {
                        Projectile.NewProjectile(
                            item.GetSource_FromThis(),
                            player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<IceStormFrostFlowerMinion>(),
                            player.GetWeaponDamage(item),
                            player.GetWeaponKnockback(item),
                            player.whoAmI);
                    }
                }
            }
        }
        private void HasSnowBall(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            int projectile4 = (int)(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .5f);
            for (int i = 0; i < projectile4; i++)
            {
                float ToRa = 0;
                if (projectile4 != 1)
                {
                    ToRa = projectile4 * 7;
                }
                Projectile.NewProjectile(source, position, velocity.RotateRandom(ToRa).RandomSpread(7) * 1.5f, ProjectileID.SnowBallFriendly, damage, knockback, player.whoAmI);
            }
        }
        private void HasFrostFlower(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            int projectile5 = (int)(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .1666667f);
            for (int i = 0; i < projectile5; i++)
            {
                float ToRa = 0;
                if (projectile5 != 1)
                {
                    ToRa = projectile5 * 5;
                }
                Projectile.NewProjectile(source, position, velocity.RotateRandom(ToRa).RandomSpread(12), ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
            }
        }
        private void HasBlizzardStaff(Player player, EntitySource_ItemUse_WithAmmo source, int damage, float knockback)
        {
            if (player.GetModPlayer<IceStormPlayer>().SpeedMultiplier < 8)
            {
                return;
            }
            Vector2 SkyPos = new Vector2(player.Center.X, player.Center.Y - 800);
            Vector2 SkyVelocity = (Main.MouseWorld - SkyPos).SafeNormalize(Vector2.UnitX);
            for (int i = 0; i < 5; i++)
            {
                SkyPos += Main.rand.NextVector2Circular(200, 200);
                int FinalCharge = Projectile.NewProjectile(source, SkyPos, SkyVelocity * 30, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
                Main.projectile[FinalCharge].tileCollide = false;
                Main.projectile[FinalCharge].timeLeft = 100;
            }
        }
        private void IceStormSynergy(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            if (player.HasItem(ItemID.SnowballCannon))
            {
                HasSnowBall(player, source, position, velocity, damage, knockback);
            }
            if (player.HasItem(ItemID.FlowerofFrost))
            {
                HasFrostFlower(player, source, position, velocity, damage, knockback);
            }
            if (player.HasItem(ItemID.BlizzardStaff))
            {
                HasBlizzardStaff(player, source, damage, knockback);
            }
        }
    }
    //GOOD FUCKING LUCK FUTURE BRO
    // Don't worry bro, i got the better system now
    abstract class SynergyBehaviorHandlePlayer : ModPlayer
    {
    }
    abstract class SynergyModItem : ModItem
    {

    }
    #region Guide to Master Ninja synergy
    class GuideToMasterNinjaSynergy : SynergyBehaviorHandlePlayer
    {
        public bool GuidetoMasterNinjaBook;
        public bool GuidetoMasterNinjaBook2;
        public bool NinjaWeeb;
        int GTMNcount = 0;
        int GTMNlimitCount = 15;
        int TimerForUltimate = 0;
        public override void ResetEffects()
        {
            GuidetoMasterNinjaBook = false;
            GuidetoMasterNinjaBook2 = false;
            NinjaWeeb = false;
        }
        public override void UpdateEquips()
        {
            if (Player.head == 22 && Player.body == 14 && Player.legs == 14)
            {
                NinjaWeeb = true;
                Player.GetAttackSpeed(DamageClass.Melee) += .15f;
            }
            if(GuidetoMasterNinjaBook2)
            {
                Player.GetAttackSpeed(DamageClass.Melee) += .35f;
            }
        }
        public override void PostUpdate()
        {
            if (GuidetoMasterNinjaBook && GuidetoMasterNinjaBook2)
            {
                SpawnNinjaProjectile();
            }
        }
        private void SpawnNinjaProjectile()
        {
            int ThrowingKnife = ModContent.ProjectileType<ThrowingKnifeCustom>();
            int Shuriken = ModContent.ProjectileType<ShurikenCustom>();
            if (TimerForUltimate < 60)
            {
                if (Player.ownedProjectileCounts[ThrowingKnife] > 0 || Player.ownedProjectileCounts[Shuriken] > 0)
                {
                    TimerForUltimate = 0;
                }
                TimerForUltimate++;
                return;
            }
            if (Player.ownedProjectileCounts[ThrowingKnife] < 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 1 + i), ModContent.ProjectileType<ThrowingKnifeCustom>(), Player.HeldItem.damage + 10, 0, Player.whoAmI);
                }
            }
            if (Player.ownedProjectileCounts[Shuriken] < 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 1 + i), ModContent.ProjectileType<ShurikenCustom>(), Player.HeldItem.damage + 10, 0, Player.whoAmI);
                }
            }
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (GuidetoMasterNinjaBook)
            {
                ThrowNinjaProjectile();
            }
        }
        private void ThrowNinjaProjectile()
        {
            int[] GTMNcontain = new int[] { ProjectileID.Shuriken, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife, ProjectileID.FrostDaggerfish, ProjectileID.BoneDagger };
            if (Player.HasItem(ItemID.ThrowingKnife) &&
                Player.HasItem(ItemID.PoisonedKnife) &&
                Player.HasItem(ItemID.FrostDaggerfish) &&
                Player.HasItem(ItemID.BoneDagger))
            {
                GTMNcount++;
            }
            GTMNcount++;
            if (GTMNcount < GTMNlimitCount)
            {
                return;
            }
            int StaticDamage = 5;
            for (int i = 0; i < GTMNcontain.Length; i++)
            {
                if (Player.HasItem(GTMNcontain[i]))
                {
                    StaticDamage += 5;
                }
            }
            if (NinjaWeeb)
            {
                StaticDamage = (int)(StaticDamage * 1.2f);
            }
            Vector2 Aimto = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX);
            int proj = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Aimto * 20, Main.rand.Next(GTMNcontain), StaticDamage, 1f, Player.whoAmI);
            Main.projectile[proj].penetrate = 1;
            GTMNcount = 0;

        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (GuidetoMasterNinjaBook)
            {
                FirstBookOnHitEffect(target);
            }
            if (GuidetoMasterNinjaBook2)
            {
                SecondBookOnHitEffect(target, damage, knockback);
            }
        }
        private void FirstBookOnHitEffect(NPC target)
        {
            if (Player.HasItem(ItemID.FrostDaggerfish))
            {
                target.AddBuff(BuffID.Frostburn, 150);
            }
            if (Player.HasItem(ItemID.BoneDagger))
            {
                target.AddBuff(BuffID.OnFire, 150);
            }
        }
        private void SecondBookOnHitEffect(NPC target, int damage, float knockback)
        {
            List<int> NinjaBag = new List<int>();
            int[] RandomThrow = new int[] { ProjectileID.Shuriken, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife };
            if ((Player.HasItem(ItemID.Shuriken) || Player.HasItem(ItemID.ThrowingKnife) || Player.HasItem(ItemID.PoisonedKnife)) && Main.rand.NextBool(10))
            {
                NinjaBag.AddRange(RandomThrow);
                if (Player.HasItem(ItemID.BoneDagger))
                {
                    NinjaBag.Add(ProjectileID.BoneDagger);
                }
                if (Player.HasItem(ItemID.FrostDaggerfish))
                {
                    NinjaBag.Add(ProjectileID.FrostDaggerfish);
                }
                Vector2 SpawnProjPos = target.Center + new Vector2(0, -200);
                for (int i = 0; i < 12; i++)
                {
                    Vector2 randomSpeed = Main.rand.NextVector2Circular(1, 1);
                    Dust.NewDust(SpawnProjPos, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0, default, Main.rand.NextFloat(2f, 3.5f));
                }
                int proj1 = Projectile.NewProjectile(Player.GetSource_FromThis(), SpawnProjPos, Vector2.Zero, Main.rand.NextFromCollection(NinjaBag), damage, knockback, Player.whoAmI);
                Main.projectile[proj1].penetrate = 1;
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (GuidetoMasterNinjaBook2 && item.type == ItemID.Katana)
            {
                scale += .5f;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (GuidetoMasterNinjaBook && NinjaWeeb && item.type == ItemID.Katana)
            {
                damage += .25f;
            }
            if (!GuidetoMasterNinjaBook2)
            {
                return;
            }
            if (item.type == ItemID.Shuriken || item.type == ItemID.ThrowingKnife || item.type == ItemID.PoisonedKnife)
            {
                damage += .5f;
                if (Player.HasItem(ItemID.BoneDagger))
                {
                    damage += .1f;
                }
                if (Player.HasItem(ItemID.FrostDaggerfish))
                {
                    damage += .1f;
                }
            }
            if (item.type == ItemID.Katana)
            {
                damage += .5f;
            }
        }
    }
    #endregion
}