using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.BurningPassion;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.DarkCactus;
using BossRush.Common;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Deagle;
using Terraria.ID;
using BossRush.Contents.Items.Weapon.MagicSynergyWeapon.StarLightDistributer;
using BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff;
using BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.MothWeapon;

namespace BossRush.Contents.Items.Weapon
{
    /// <summary>
    ///This mod player should hold all the logic require for the item, if the item is shooting out the projectile, it should be doing that itself !<br/>
    ///Same with projectile unless it is a vanilla projectile then we can refer to global projectile<br/>
    ///This should only hold custom bool or data that we think should be hold/use/transfer<br/>
    ///We will name using the following format "Synergy item"_"vanilla item" to assign synergy power so that it is clear to read and easy to maintain<br/>
    ///Anything that relate to actual logic and how player interact from the item could or shoulds also go in here<br/>
    /// </summary>
    public class PlayerSynergyItemHandle : ModPlayer
    {
        public bool BurningPassion_WandofFrosting = false;

        public bool DarkCactus_BatScepter = false;
        public bool DarkCactus_BladeOfGrass = false;

        public bool EnchantedOreSword_StarFury = false;
        public bool EnchantedOreSword_Musket = false;

        public bool EnchantedStarfury_SkyFacture = false;
        public bool EnchantedStarfury_BreakerBlade = false;

        public bool IceStorm_SnowBallCannon = false;
        public bool IceStorm_FlowerofFrost = false;
        public bool IceStorm_BlizzardStaff = false;

        public bool EnergyBlade_Code1 = false;
        public bool EnergyBlade_Code2 = false;
        public int EnergyBlade_Code1_Energy = 0;

        public bool Swotaff_Spear = false;
        public int Swotaff_Spear_Counter = 0;

        public bool Deagle_PhoenixBlaster = false;
        public bool Deagle_DaedalusStormBow = false;
        public int Deagle_DaedalusStormBow_coolDown = 0;
        public bool Deagle_PhoenixBlaster_Critical = false;

        public bool OvergrownMinishark_CrimsonRod = false;
        public bool OvergrownMinishark_PlatinumShortSword = false;

        public bool StarLightDistributer_MeteorArmor = false;

        public bool StreetLamp_Firecracker = false;
        public bool StreetLamp_VampireFrogStaff = false;
        public int StreetLamp_VampireFrogStaff_HitCounter = 0;

        public override void ResetEffects()
        {
            base.ResetEffects();
            BurningPassion_WandofFrosting = false;

            DarkCactus_BatScepter = false;
            DarkCactus_BladeOfGrass = false;

            EnchantedOreSword_StarFury = false;
            EnchantedOreSword_Musket = false;

            EnchantedStarfury_SkyFacture = false;
            EnchantedStarfury_BreakerBlade = false;

            IceStorm_SnowBallCannon = false;
            IceStorm_FlowerofFrost = false;
            IceStorm_BlizzardStaff = false;

            EnergyBlade_Code1 = false;
            EnergyBlade_Code2 = false;

            Swotaff_Spear = false;

            Deagle_PhoenixBlaster = false;
            Deagle_DaedalusStormBow = false;

            OvergrownMinishark_CrimsonRod = false;
            OvergrownMinishark_PlatinumShortSword = false;

            StarLightDistributer_MeteorArmor = false;

            StreetLamp_VampireFrogStaff = false;
            StreetLamp_Firecracker = false;

        }
        int check = 1;
        public override void PostUpdate()
        {
            Item item = Player.HeldItem;
            if (item.ModItem is BurningPassion)
            {
                if (!Player.ItemAnimationActive && check == 0)
                {
                    Player.velocity *= .25f;
                    check++;
                }
                else if (Player.ItemAnimationActive && Main.mouseRight)
                {
                    Player.gravity = 0;
                    Player.velocity.Y -= 0.3f;
                    Player.ignoreWater = true;
                    check = 0;
                }
            }
            if (item.ModItem is Deagle)
            {
                if (Deagle_DaedalusStormBow)
                {
                    Deagle_DaedalusStormBow_coolDown = BossRushUtils.CoolDown(Deagle_DaedalusStormBow_coolDown);
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Swotaff_Spear)
            {
                if (Swotaff_Spear_Counter < 2)
                {
                    Swotaff_Spear_Counter++;
                }
                else
                {
                    Vector2 cirRanPos = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(position, 30, 90, velocity.ToRotation());
                    Vector2 vel = (Main.MouseWorld - cirRanPos).SafeNormalize(Vector2.Zero) * 10;
                    Projectile.NewProjectile(source, cirRanPos, vel, type, damage, knockback, Player.whoAmI, 2);
                    Swotaff_Spear_Counter = 0;
                }
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                if (Deagle_PhoenixBlaster)
                {
                    Deagle_PhoenixBlaster_Critical = true;
                }
                if (Deagle_DaedalusStormBow && Deagle_DaedalusStormBow_coolDown <= 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Vector2 positionAboveSky = target.Center + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-1100, -1000));
                        int projectile = Projectile.NewProjectile(
                            Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, Player.HeldItem.ammo),
                            positionAboveSky,
                            (target.Center - positionAboveSky).SafeNormalize(Vector2.Zero) * 20f,
                            ProjectileID.BulletHighVelocity,
                            hit.Damage,
                            0,
                            Player.whoAmI);
                        Main.projectile[projectile].usesLocalNPCImmunity = true;
                    }
                    Deagle_DaedalusStormBow_coolDown = 600;
                }
            }

        }
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (Player.ItemAnimationActive && Player.HeldItem.ModItem is BurningPassion && Player.ownedProjectileCounts[ModContent.ProjectileType<BurningPassionP>()] > 0)
            {
                return true;
            }
            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }

        public override void UpdateEquips()
        {
            if (Player.head == 6 && Player.body == 6 && Player.legs == 6)
            {
                StarLightDistributer_MeteorArmor = true;
            }
        }
        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            if (StarLightDistributer_MeteorArmor && item.ModItem is StarLightDistributer)
            {
                mult = 0f;
            }
        }
    }
    public abstract class SynergyModItem : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            ModifySynergyToolTips(ref tooltips, Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>());
            TooltipLine line = new TooltipLine(Mod, "Synergy", "Synergy Weapon");
            line.OverrideColor = BossRushUtils.MultiColor(new List<Color> { new Color(25, 150, 150), Color.White }, 5);
            tooltips.Add(line);
        }
        public virtual void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) { }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
            PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
            HoldSynergyItem(player, modplayer);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
            ModifySynergyShootStats(player, player.GetModPlayer<PlayerSynergyItemHandle>(), ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public virtual void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

        }
        /// <summary>
        /// You should use this to set condition, the condition must be pre set in <see cref="PlayerSynergyItemHandle"/> and then check condition in here
        /// </summary>
        /// <param name="player"></param>
        /// <param name="modplayer"></param>
        public virtual void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) { }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SynergyShoot(player, player.GetModPlayer<PlayerSynergyItemHandle>(), source, position, velocity, type, damage, knockback, out bool CanShootItem);
            return CanShootItem;
        }
        public virtual void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) { CanShootItem = true; }
    }
    public abstract class SynergyModProjectile : ModProjectile
    {
        public virtual void SpawnDustPostPreAI(Player player) { }
        public virtual void SpawnDustPostAI(Player player) { }
        public virtual void SpawnDustPostPostAI(Player player) { }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            SynergyPreAI(player, player.GetModPlayer<PlayerSynergyItemHandle>(), out bool stopAI);
            SpawnDustPostPreAI(player);
            return stopAI;
        }
        /// <summary>
        /// You should check the condition yourself
        /// </summary>
        /// <param name="player"></param>
        /// <param name="modplayer"></param>
        /// <param name="runAI"></param>
        public virtual void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) { runAI = true; }
        public override void AI()
        {
            base.AI();
            Player player = Main.player[Projectile.owner];
            SynergyAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
            SpawnDustPostAI(player);
        }
        /// <summary>
        /// You should check the condition yourself
        /// </summary>
        /// <param name="player"></param>
        /// <param name="modplayer"></param>
        public virtual void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) { }
        public override void PostAI()
        {
            base.PostAI();
            Player player = Main.player[Projectile.owner];
            SynergyPostAI(player, player.GetModPlayer<PlayerSynergyItemHandle>());
            SpawnDustPostPostAI(player);
        }
        /// <summary>
        /// You should check the condition yourself
        /// </summary>
        /// <param name="player"></param>
        /// <param name="modplayer"></param>
        public virtual void SynergyPostAI(Player player, PlayerSynergyItemHandle modplayer) { }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            Player player = Main.player[Projectile.owner];
            ModifyHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, ref modifiers);
        }
        public virtual void ModifyHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, ref NPC.HitModifiers modifiers) { }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Player player = Main.player[Projectile.owner];
            OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
        }
        public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) { }
        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
            Player player = Main.player[Projectile.owner];
            SynergyKill(player, player.GetModPlayer<PlayerSynergyItemHandle>(), timeLeft);
        }
        public virtual void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft)
        {
        }
    }
}