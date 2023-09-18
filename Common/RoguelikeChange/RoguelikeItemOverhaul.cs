using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Contents.Projectiles;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;

namespace BossRush.Common.RoguelikeChange
{
    enum TypeItem
    {
        //generalize
        Water,
        Ice,
        Nature,
        Fire,
        Corruption,
        Crimson,
        Hallow,
        Light,
        Dark,
        Life,//Weapon that give life
        Chaotic,
        Energy,
        //Generalize type 2
        HeavyWeight,
        LightWeight,
        Technology,
        Chemical,
        Biological,
        //Sword
        Sword,
        ShortSword,
        GreatSword,
        LongSword,
        //Spear
        Pike,
        Trident,
        Glaive,
        Naginata,
        //Gun
        Pistol,
        Rifle,
        Sniper,
        Shotgun,
        //Bow
        ShortBow,
        LongBow,
        CrossBow,
        //Magic
        TomeSpell,
        Wand,
        Staff,
        //Summon
        Whip,
        SummonStaff,
        MiscSummon,
        //Special
        Synergy,
        Terra,
        True,
        Omega,
        Alpha,
        Delta,
        Beta,
        Pure
    }
    /// <summary>
    /// This is where we should modify vanilla item
    /// </summary>
    class RoguelikeItemOverhaul : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return;
            }
            if (entity.type == ItemID.Sandgun)
            {
                entity.shoot = ModContent.ProjectileType<SandProjectile>();
            }
            if (entity.type == ItemID.LifeCrystal || entity.type == ItemID.ManaCrystal)
            {
                entity.autoReuse = true;
            }

        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return;
            }
            if (item.type == ItemID.Sandgun)
            {
                tooltips.Add(new TooltipLine(Mod, "SandGunChange",
                    "The sand projectile no longer spawn upon kill" +
                    "\nDecrease damage by 55%"));
            }
            int[] armorSet = new int[] { player.armor[0].type, player.armor[1].type, player.armor[2].type };
            foreach (TooltipLine tooltipLine in tooltips)
            {
                if (tooltipLine.Name != "SetBonus")
                {
                    continue;
                }
                if (armorSet.Contains(item.type))
                {
                    tooltips.Add(new TooltipLine(Mod, ArmorSet.ConvertIntoArmorSetFormat(armorSet), GetToolTip(item.type)));
                    return;
                }
            }
        }
        private string GetToolTip(int type)
        {
            if (type == ItemID.WoodHelmet || type == ItemID.WoodBreastplate || type == ItemID.WoodGreaves)
            {
                return "When in forest biome :" +
                       "\nIncrease defense by 11" +
                       "\nIncrease movement speed by 25%" +
                       "\nYour attack have 25% chance to drop down a acorn dealing 10 damage";
            }
            if (type == ItemID.BorealWoodHelmet || type == ItemID.BorealWoodBreastplate || type == ItemID.BorealWoodGreaves)
            {
                return "When in snow biome :" +
                       "\nIncrease defense by 13" +
                       "\nIncrease movement speed by 20%" +
                       "\nYou are immune to Chilled" +
                       "\nYour attack have 10% chance to inflict frost burn for 10 second";
            }
            if (type == ItemID.RichMahoganyHelmet || type == ItemID.RichMahoganyBreastplate || type == ItemID.RichMahoganyGreaves)
            {
                return "When in jungle biome :" +
                       "\nIncrease defense by 12" +
                       "\nIncrease movement speed by 30%" +
                       "\nGetting hit release sharp leaf around you that deal 12 damage";
            }
            if (type == ItemID.ShadewoodHelmet || type == ItemID.ShadewoodBreastplate || type == ItemID.ShadewoodGreaves)
            {
                return "When in crimson biome :" +
                       "\nIncrease defense by 17" +
                       "\nIncrease movement speed by 15%" +
                       "\nIncrease critical strike chance by 5" +
                       "\nIncrease life regen by 1" +
                       "\nWhenever you strike a enemy :" +
                       "\nA ring of crimson burst out that deal fixed 10 damage and heal you for each enemy hit and debuff them with ichor";
            }
            if (type == ItemID.EbonwoodHelmet || type == ItemID.EbonwoodBreastplate || type == ItemID.EbonwoodGreaves)
            {
                return "When in corruption biome :" +
                        "\nIncrease defense by 6" +
                        "\nIncrease movement speed by 35%" +
                        "\nIncrease damage by 5%" +
                        "\nYou leave a trail of corruption that deal 3 damage and inflict cursed inferno for 2s";
            }
            if (type == ItemID.AshWoodHelmet || type == ItemID.AshWoodBreastplate || type == ItemID.AshWoodGreaves)
            {
                return "Increase defense by 16" +
                       "\nIncrease damage by 10%" +
                       "\nWhen in underworld or underground caven level :" +
                       "\nGetting hit fires a burst of flames at the attacker, dealing from 5 to 15 damage" +
                       "\nAll attacks inflicts On Fire! for 5 seconds" +
                       "\nIncreased life regen by 1";
            }
            if (type == ItemID.CactusHelmet || type == ItemID.CactusBreastplate || type == ItemID.CactusLeggings)
            {
                return "Increase defenses by 10" +
                       "\nGetting hit will drop down a rolling cactus that is friendly with 5s cool down" +
                       "\nGetting hit will shoot out 8 cactus spike that is friendly deal 15 damage";
            }
            if (type == ItemID.PalmWoodHelmet || type == ItemID.PalmWoodBreastplate || type == ItemID.PalmWoodGreaves)
            {
                return "Increase defense by 10" +
                       "\nIncrease movement speed by 17%" +
                       "\nJumping will leave a trail of sand that deal 12 damage";
            }
            if (type == ItemID.PumpkinHelmet || type == ItemID.PumpkinBreastplate || type == ItemID.PumpkinLeggings)
            {
                return "When in overworld :" +
                       "\nGrant well fed buff for 5s on getting hit" +
                       "\nhitting enemies has 25% to inflict pumpkin overdose" +
                       "\ninflicting the same debuff to an enemy who already has it " +
                       "\ncauses an explosion, dealing 5 + 5% of damage dealt" +
                       "\nWhile below 20% HP, you gain 5x health regen";
            }
            if (type == ItemID.TinHelmet || type == ItemID.TinChainmail || type == ItemID.TinGreaves)
            {
                return "Increase defense by 5" +
                        "\nIncrease movement speed by 21%" +
                        "\nVanilla tin weapon are stronger";
            }
            if (type == ItemID.LeadHelmet || type == ItemID.LeadChainmail || type == ItemID.LeadGreaves)
            {
                return "Increase defense by 7" +
                        "\nYour attack can inflict irradiation poison" +
                        "\nLead irradiation increase enemy defense by 20 but deal 50 DoT";
            }
            if (type == ItemID.CopperHelmet || type == ItemID.CopperChainmail || type == ItemID.CopperGreaves)
            {
                return "Increase movement speed by 15%" +
                       "\nEvery 50 hit to enemy grants you the over charged for 3s" +
                       "\nDuring the rain, your hit requirement reduce by half" +
                       "\nOver charged: Increases movement speed, weapon speed, damage by 10%";
            }
            if (type == ItemID.PearlwoodHelmet || type == ItemID.PearlwoodBreastplate || type == ItemID.PearlwoodGreaves)
            {
                return "Increase movement speed by 35%" +
                        "\nAttacking an enemy summons 6 hallow Swords that deals 5 damage with 4 seconds cooldown" +
                        "\nIncrease damage by 15% during day" +
                        "\nIncrease defense By 12" +
                        "\nWhen in Hallow biome:" +
                        "\n Hallow Swords deal 15 damage";
            }
            if (type == ItemID.IronHelmet || type == ItemID.IronChainmail || type == ItemID.IronGreaves)
            {
                return "Increase damage reduction 2.5%" +
                       "\nIncrease defense effectivness by 10%" +
                       "\nIncrease damage by 5%" +
                       "\nDecrease movement speed 5%" +
                       "\nWhile under 50% HP, gain 15 bonus defense, but -10% less attack speed";
            }
            if (type == ItemID.SilverHelmet || type == ItemID.SilverChainmail || type == ItemID.SilverGreaves)
            {
                return "During the day :" +
                       "\nGain 10 defense" +
                       "\nDuring the night :" +
                       "\nIncrease damage by 10%" +
                       "\nAt full life, these effects are multiply by 2";
            }
            if (type == ItemID.TungstenHelmet || type == ItemID.TungstenChainmail || type == ItemID.TungstenGreaves)
            {
                return "Increase defense by 15" +
                       "\nWhen at full hp :" +
                       "\nReduce your defense down to 0" +
                       "\nIncrease speed by 30%" +
                       "\nThe closer your enemy is, the more damage increases";
            }
            if (type == ItemID.GoldHelmet || type == ItemID.GoldChainmail || type == ItemID.GoldGreaves)
            {
                return "Your attack have 15% to inflict Midas for 10 seconds" +
                       "\nAttacking enemies with midas debuff will : " +
                       "\nDeal additional damage based on their defense";
            }
            if (type == ItemID.PlatinumHelmet || type == ItemID.PlatinumChainmail || type == ItemID.PlatinumGreaves)
            {
                return "Increase weapon uses speed by 35%" +
                       "\nAttacking too much will lit on fire";
            }
            return "";
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return "";
            }
            return new ArmorSet(head.type, body.type, legs.type).ToString();
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            GlobalItemPlayer modplayer = player.GetModPlayer<GlobalItemPlayer>();
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.WoodHelmet, ItemID.WoodBreastplate, ItemID.WoodGreaves))
            {
                if (player.ZoneForest)
                {
                    player.statDefense += 11;
                    player.moveSpeed += .25f;
                    modplayer.WoodArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.BorealWoodHelmet, ItemID.BorealWoodBreastplate, ItemID.BorealWoodGreaves))
            {
                if (player.ZoneSnow)
                {
                    player.statDefense += 13;
                    player.moveSpeed += .20f;
                    player.buffImmune[BuffID.Chilled] = true;
                    modplayer.BorealWoodArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.RichMahoganyHelmet, ItemID.RichMahoganyBreastplate, ItemID.RichMahoganyGreaves))
            {
                if (player.ZoneJungle)
                {
                    player.statDefense += 12;
                    player.moveSpeed += .30f;
                    modplayer.RichMahoganyArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.ShadewoodHelmet, ItemID.ShadewoodBreastplate, ItemID.ShadewoodGreaves))
            {
                if (player.ZoneCrimson)
                {
                    player.statDefense += 17;
                    player.lifeRegen += 1;
                    player.moveSpeed += .15f;
                    player.GetCritChance(DamageClass.Generic) += 5f;
                    modplayer.ShadewoodArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.EbonwoodHelmet, ItemID.EbonwoodBreastplate, ItemID.EbonwoodGreaves))
            {
                if (player.ZoneCorrupt)
                {
                    player.statDefense += 6;
                    player.moveSpeed += .35f;
                    player.GetDamage(DamageClass.Generic) += .05f;
                    modplayer.EbonWoodArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.AshWoodHelmet, ItemID.AshWoodBreastplate, ItemID.AshWoodGreaves))
            {
                player.statDefense += 16;
                player.GetDamage(DamageClass.Generic) += .1f;
                if (player.ZoneUnderworldHeight || player.ZoneUnderworldHeight)
                {
                    player.lifeRegen++;
                    modplayer.AshWoodArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.CactusHelmet, ItemID.CactusBreastplate, ItemID.CactusLeggings))
            {
                player.statDefense += 10;
                modplayer.CactusArmor = true;
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PalmWoodHelmet, ItemID.PalmWoodBreastplate, ItemID.PalmWoodGreaves))
            {
                player.statDefense += 10;
                player.moveSpeed += .17f;
                modplayer.PalmWoodArmor = true;
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PumpkinHelmet, ItemID.PumpkinBreastplate, ItemID.PumpkinLeggings))
            {
                if (player.ZoneOverworldHeight)
                {
                    modplayer.PumpkinArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.TinHelmet, ItemID.TinChainmail, ItemID.TinGreaves))
            {
                player.statDefense += 5;
                player.moveSpeed += .21f;
                modplayer.TinArmor = true;
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.CopperHelmet, ItemID.CopperChainmail, ItemID.CopperGreaves))
            {
                player.moveSpeed += 0.15f;
                modplayer.CopperArmor = true;
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves))
            {
                player.moveSpeed -= 0.05f;
                player.endurance += 0.05f;
                player.DefenseEffectiveness *= 1.1f;
                player.GetDamage(DamageClass.Generic) += 0.05f;
                if (player.statLife <= player.statLifeMax * 0.5f)
                {
                    player.statDefense += 15;
                    player.GetAttackSpeed(DamageClass.Generic) -= 0.10f;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.LeadHelmet, ItemID.LeadChainmail, ItemID.LeadGreaves))
            {
                player.statDefense += 7;
                modplayer.LeadArmor = true;
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.SilverHelmet, ItemID.SilverChainmail, ItemID.SilverGreaves))
            {
                if (Main.dayTime)
                    player.statDefense += player.statLife < player.statLifeMax2 ? 10 : 20;
                else
                    player.GetDamage(DamageClass.Generic) += player.statLife < player.statLifeMax2 ? .1f : .2f;
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.TungstenHelmet, ItemID.TungstenChainmail, ItemID.TungstenGreaves))
            {
                player.statDefense += 15;
                if (player.statLife >= player.statLifeMax2)
                {
                    player.moveSpeed += .3f;
                    modplayer.TungstenArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.GoldHelmet, ItemID.GoldChainmail, ItemID.GoldGreaves))
            {
                modplayer.GoldArmor = true;
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PearlwoodHelmet, ItemID.PearlwoodBreastplate, ItemID.PearlwoodGreaves))
            {
                player.moveSpeed += 0.35f;
                player.statDefense += 12;
                modplayer.pearlWoodArmor = true;
                if (Main.dayTime)
                    player.GetDamage(DamageClass.Generic) += 0.15f;

            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PlatinumHelmet, ItemID.PlatinumChainmail, ItemID.PlatinumGreaves))
            {
                modplayer.PlatinumArmor = true;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.EoCShield)
            {
                player.GetModPlayer<EvilEyePlayer>().EoCShieldUpgrade = true;
            }
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            VanillaChange(item, player, ref damage);
        }
        private void VanillaChange(Item item, Player player, ref StatModifier damage)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return;
            }
            if (item.type == ItemID.Sandgun)
            {
                damage -= .55f;
            }

        }
    }
    public class GlobalItemPlayer : ModPlayer
    {
        public bool WoodArmor = false;
        public bool BorealWoodArmor = false;
        public bool RichMahoganyArmor = false;
        public bool ShadewoodArmor = false;
        int ShadewoodArmorCD = 0;
        public bool EbonWoodArmor = false;
        int EbonWoodArmorCD = 0;
        public bool CactusArmor = false;
        int CactusArmorCD = 0;
        public bool PalmWoodArmor = false;
        public bool PumpkinArmor = false;
        public bool AshWoodArmor = false;
        public bool CopperArmor = false;
        int CopperArmorChargeCounter = 0;
        public bool GoldArmor = false;
        public bool pearlWoodArmor = false;
        int pearlWoodArmorCD = 0;
        public bool TinArmor = false;
        public int TinArmorCountEffect = 0;
        public bool LeadArmor = false;
        public bool TungstenArmor = false;
        public bool PlatinumArmor = false;
        int PlatinumArmorCountEffect = 0;
        public override void ResetEffects()
        {
            WoodArmor = false;
            BorealWoodArmor = false;
            RichMahoganyArmor = false;
            ShadewoodArmor = false;
            EbonWoodArmor = false;
            CactusArmor = false;
            PalmWoodArmor = false;
            PumpkinArmor = false;
            AshWoodArmor = false;
            CopperArmor = false;
            GoldArmor = false;
            pearlWoodArmor = false;
            TinArmor = false;
            LeadArmor = false;
            TungstenArmor = false;
            PlatinumArmor = false;
        }
        public override void PreUpdate()
        {
            ShadewoodArmorCD = BossRushUtils.CoolDown(ShadewoodArmorCD);
            EbonWoodArmorCD = BossRushUtils.CoolDown(EbonWoodArmorCD);
            CactusArmorCD = BossRushUtils.CoolDown(CactusArmorCD);
            pearlWoodArmorCD = BossRushUtils.CoolDown(pearlWoodArmorCD);
            if (EbonWoodArmor)
                if (EbonWoodArmorCD <= 0 && Player.velocity != Vector2.Zero)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2Circular(10, 10), -Player.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<CorruptionTrail>(), 3, 0, Player.whoAmI);
                    EbonWoodArmorCD = 45;
                }
            if (PalmWoodArmor)
                if (Player.justJumped)
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 vec = new Vector2(-Player.velocity.X, Player.velocity.Y).Vector2RotateByRandom(20).LimitedVelocity(Main.rand.NextFloat(2, 3));
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, vec, ModContent.ProjectileType<SandProjectile>(), 12, 1f, Player.whoAmI);
                    }
            if (PlatinumArmor)
            {
                if (Player.ItemAnimationActive)
                    PlatinumArmorCountEffect++;
                else
                    PlatinumArmorCountEffect = BossRushUtils.CoolDown(PlatinumArmorCountEffect);
            }

        }
        public override void PostUpdate()
        {
            if (TungstenArmor)
            {
                Player.statDefense *= 0;
            }
            if (PlatinumArmorCountEffect >= 600)
            {
                Player.AddBuff(BuffID.OnFire, 300);
                Dust.NewDust(Player.Center, 0, 0, DustID.Torch, 0, 0, 0, default, Main.rand.NextFloat(1, 1.5f));
            }
        }
        public override float UseSpeedMultiplier(Item item)
        {
            if (PlatinumArmor)
            {
                return 1.35f;
            }
            return base.UseSpeedMultiplier(item);
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (TinArmor)
            {
                if (item.type == ItemID.TinBow)
                {
                    Vector2 pos = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(position, 50, 50);
                    Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * velocity.Length();
                    Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<TinOreProjectile>(), damage, knockback, Player.whoAmI);
                    TinArmorCountEffect++;
                    if (TinArmorCountEffect >= 5)
                    {
                        Projectile.NewProjectile(source, position, velocity * 1.15f, ModContent.ProjectileType<TinBarProjectile>(), (int)(damage * 1.5f), knockback, Player.whoAmI);
                        TinArmorCountEffect = 0;
                    }
                }
                if (item.type == ItemID.TopazStaff)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vec = velocity.Vector2DistributeEvenly(3, 10, i);
                        int proj = Projectile.NewProjectile(source, position, vec, type, damage, knockback, Player.whoAmI);
                        Main.projectile[proj].extraUpdates = 10;
                    }
                    return false;
                }
                if (item.type == ItemID.TinShortsword)
                {
                    Vector2 pos = position + Main.rand.NextVector2Circular(50, 50);
                    Projectile.NewProjectile(source, pos, Main.MouseWorld - pos, ModContent.ProjectileType<TinShortSwordProjectile>(), damage, knockback, Player.whoAmI);
                }
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (TinArmor)
            {
                if (item.type == ItemID.TinBow)
                {
                    velocity *= 2;
                }
                if (item.type == ItemID.TopazStaff)
                {
                    position = position.PositionOFFSET(velocity, 50);
                }
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (TinArmor)
                if (item.type == ItemID.TinBroadsword)
                    scale += .5f;
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (item.type == ItemID.WaspGun && !NPC.downedPlantBoss)
            {
                damage *= .5f;
            }
            if (TinArmor)
                switch (item.type)
                {
                    case ItemID.TinBow:
                        damage += .85f;
                        break;
                    case ItemID.TinBroadsword:
                        damage += 1.75f;
                        break;
                    case ItemID.TinShortsword:
                        damage += 1.25f;
                        break;
                    case ItemID.TopazStaff:
                        damage += .15f;
                        break;
                }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            OnHitEffect_RichMahoganyArmor(proj);
            OnHitEffect_CactusArmor(proj);
            OnHitEffect_AshWoodArmor(proj);
            OnHitEffect_PumpkinArmor();
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            OnHitEffect_RichMahoganyArmor(npc);
            OnHitEffect_CactusArmor(npc);
            OnHitEffect_AshWoodArmor(npc);
            OnHitEffect_PumpkinArmor();
        }
        private void OnHitEffect_PumpkinArmor()
        {
            if (PumpkinArmor)
                Player.AddBuff(BuffID.WellFed3, 300);
        }
        private void OnHitEffect_AshWoodArmor(Entity entity)
        {
            if (AshWoodArmor)
            {
                int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, (entity.Center - Player.Center).SafeNormalize(Vector2.UnitX) * 10, ProjectileID.Flames, Main.rand.Next(5, 15), 1f, Player.whoAmI);
                Main.projectile[proj].penetrate = -1;
            }
        }
        private void OnHitEffect_RichMahoganyArmor(Entity entity)
        {
            if (RichMahoganyArmor)
                for (int i = 0; i < 10; i++)
                {
                    Vector2 spread = Vector2.One.Vector2DistributeEvenly(10f, 360, i);
                    int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, spread * 2f, ProjectileID.BladeOfGrass, 12, 1f, Player.whoAmI);
                    Main.projectile[proj].penetrate = -1;
                }
        }
        private void OnHitEffect_CactusArmor(Entity entity)
        {
            if (CactusArmor)
            {
                if (CactusArmorCD <= 0)
                {
                    bool manualDirection = Player.Center.X < entity.Center.X;
                    Vector2 AbovePlayer = Player.Center + new Vector2(Main.rand.NextFloat(-500, 500), -1000);
                    int projectile = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), AbovePlayer, Vector2.UnitX * .1f * manualDirection.BoolOne(), ProjectileID.RollingCactus, 150, 0, Player.whoAmI);
                    Main.projectile[projectile].friendly = true;
                    Main.projectile[projectile].hostile = false;
                    CactusArmorCD = 300;
                }
                for (int i = 0; i < 8; i++)
                {
                    Vector2 vec = Vector2.One.Vector2DistributeEvenly(8, 360, i);
                    int projectile = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, vec, ProjectileID.RollingCactusSpike, 15, 0, Player.whoAmI);
                    Main.projectile[projectile].friendly = true;
                    Main.projectile[projectile].hostile = false;
                    Main.projectile[projectile].penetrate = -1;
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitNPC_ShadewoodArmor();
            OnHitNPC_BorealWoodArmor(target);
            OnHitNPC_WoodArmor(target, proj);
            OnHitNPC_PumpkinArmor(target, damageDone);
            OnHitNPC_AshWoodArmor(target);
            OnHitNPC_CopperArmor();
            OnHitNPC_GoldArmor(target, damageDone);
            OnHitNPC_LeadArmor(target);
            OnHitNPC_PearlWoodArmor(target);
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitNPC_ShadewoodArmor();
            OnHitNPC_BorealWoodArmor(target);
            OnHitNPC_WoodArmor(target);
            OnHitNPC_PumpkinArmor(target, damageDone);
            OnHitNPC_AshWoodArmor(target);
            OnHitNPC_CopperArmor();
            OnHitNPC_GoldArmor(target, damageDone);
            if (TinArmor)
                if (item.type == ItemID.TinBroadsword)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero), ModContent.ProjectileType<TinBroadSwordProjectile>(), 12, 1f, Player.whoAmI);
                }
            OnHitNPC_LeadArmor(target);
            OnHitNPC_PearlWoodArmor(target);
        }
        private void OnHitNPC_LeadArmor(NPC npc)
        {
            if (LeadArmor)
                npc.AddBuff(ModContent.BuffType<LeadIrradiation>(), 600);
        }
        private void OnHitNPC_WoodArmor(NPC target, Projectile proj = null)
        {
            if (WoodArmor)
                if (Main.rand.NextBool(4) && (proj is null || proj is not null && proj.ModProjectile is not AcornProjectile))
                    Projectile.NewProjectile(Player.GetSource_FromThis(),
                        target.Center - new Vector2(0, 400),
                        Vector2.UnitY * 10,
                        ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Player.whoAmI);
        }
        private void OnHitNPC_ShadewoodArmor()
        {
            if (ShadewoodArmor)
                if (ShadewoodArmorCD <= 0)
                {
                    for (int i = 0; i < 75; i++)
                    {
                        Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(300, 300), 0, 0, DustID.Crimson);
                        Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(300, 300), 0, 0, DustID.GemRuby);
                    }
                    Player.Center.LookForHostileNPC(out List<NPC> npclist, 325f);
                    foreach (var npc in npclist)
                    {
                        npc.StrikeNPC(npc.CalculateHitInfo(10, 1));
                        npc.AddBuff(BuffID.Ichor, 300);
                        Player.Heal(1);
                    }
                    ShadewoodArmorCD = 180;
                }
        }
        private void OnHitNPC_BorealWoodArmor(NPC target)
        {
            if (BorealWoodArmor)
                if (Main.rand.NextBool(10))
                    target.AddBuff(BuffID.Frostburn, 600);
        }
        private void OnHitNPC_PumpkinArmor(NPC npc, float damage)
        {
            if (PumpkinArmor && Main.rand.NextBool(3))
            {
                if (npc.HasBuff(ModContent.BuffType<pumpkinOverdose>()))
                {
                    int explosionRaduis = 75 + (int)MathHelper.Clamp(damage, 0, 125);
                    for (int i = 0; i < 35; i++)
                    {
                        Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(explosionRaduis, explosionRaduis), 0, 0, DustID.Pumpkin);
                        Dust.NewDust(npc.Center + Main.rand.NextVector2CircularEdge(explosionRaduis, explosionRaduis), 0, 0, DustID.OrangeTorch);
                    }
                    npc.Center.LookForHostileNPC(out List<NPC> npclist, explosionRaduis);
                    foreach (var i in npclist)
                    {
                        i.StrikeNPC(i.CalculateHitInfo(5 + (int)(damage * 0.05f), 1, Main.rand.NextBool(40)));
                    }
                    SoundEngine.PlaySound(SoundID.NPCDeath46);
                    npc.AddBuff(ModContent.BuffType<pumpkinOverdose>(), 240);
                }
                else npc.AddBuff(ModContent.BuffType<pumpkinOverdose>(), 240);
            }
        }
        private void OnHitNPC_AshWoodArmor(NPC npc)
        {
            if (AshWoodArmor)
                npc.AddBuff(BuffID.OnFire, 300);
        }
        private void OnHitNPC_PearlWoodArmor(NPC npc)
        {
            if (pearlWoodArmorCD <= 0 && pearlWoodArmor)
            {
                int dmg = 5;
                int projAmount = 6;
                int Cooldown = 240;
                if (Player.ZoneHallow)
                {
                    dmg += 10;
                }
                for (int i = 0; i < projAmount; i++)
                {
                    Vector2 pos = npc.Center + new Vector2(0, -20).Vector2DistributeEvenly(projAmount, 360, i) * 10;
                    Vector2 vel = npc.Center - pos;
                    Projectile.NewProjectile(Player.GetSource_OnHit(npc), pos, vel.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<pearlSwordProj>(), dmg, 1, Player.whoAmI);
                }
                pearlWoodArmorCD = Cooldown;
            }
        }
        private void OnHitNPC_CopperArmor()
        {
            if (!CopperArmor)
            {
                return;
            }
            CopperArmorChargeCounter++;
            if (Player.ZoneRain)
                CopperArmorChargeCounter++;
            if (CopperArmorChargeCounter >= 50)
            {
                Player.AddBuff(ModContent.BuffType<OverCharged>(), 180);
                CopperArmorChargeCounter = 0;
            }
        }
        private void OnHitNPC_GoldArmor(NPC npc, float damage)
        {
            if (GoldArmor)
                if (npc.HasBuff(BuffID.Midas))
                {
                    int GoldArmorBonusDamage = (int)damage + npc.defense;
                    npc.StrikeNPC(npc.CalculateHitInfo(GoldArmorBonusDamage, 1, false, 1, DamageClass.Generic, true, Player.luck));
                }
                else
                {
                    if (Main.rand.NextFloat() < .15f)
                    {
                        npc.AddBuff(BuffID.Midas, 600);
                    }
                }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (TungstenArmor)
            {
                float DamageIncrease = (target.Center - Player.Center).Length();
                modifiers.SourceDamage += MathHelper.Clamp(600 - DamageIncrease, 0, 200) * .005f;
            }
        }
        public override void NaturalLifeRegen(ref float regen)
        {
            regen += NaturalLifeRegen_pumpkinArmor();
            regen += NaturalLifeRegen_CopperArmor();
        }
        private float NaturalLifeRegen_pumpkinArmor() => Player.statLife <= Player.statLifeMax * .2f ? 5f : 1f;
        private float NaturalLifeRegen_CopperArmor()
        {
            return CopperArmor == true ? 1.25f : 1;
        }
    }
    public class GlobalItemProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            base.OnSpawn(projectile, source);
            if (projectile.type == ProjectileID.RollingCactusSpike && source is EntitySource_Parent parent && parent.Entity is Projectile parentProjectile)
            {
                projectile.friendly = parentProjectile.friendly;
                projectile.hostile = parentProjectile.hostile;
            }
        }
    }
    public class GlobalItemMod_GlobalNPC : GlobalNPC
    {
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(ModContent.BuffType<LeadIrradiation>()))
                modifiers.Defense.Base += 20;
        }
    }
    class ArmorSet
    {
        int headID, bodyID, legID;
        public ArmorSet(int headID, int bodyID, int legID)
        {
            this.headID = headID;
            this.bodyID = bodyID;
            this.legID = legID;
        }
        public static string ConvertIntoArmorSetFormat(int headID, int bodyID, int legID) => $"{headID}:{bodyID}:{legID}";
        /// <summary>
        /// Expect there is only 3 item in a array
        /// </summary>
        /// <param name="armor"></param>
        /// <returns></returns>
        public static string ConvertIntoArmorSetFormat(int[] armor) => $"{armor[0]}:{armor[1]}:{armor[2]}";
        public override string ToString() => $"{headID}:{bodyID}:{legID}";
    }
}