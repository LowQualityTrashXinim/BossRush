using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Common.Global
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
    class GlobalItemMod : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            if (entity.type == ItemID.Sandgun)
            {
                entity.shoot = ModContent.ProjectileType<SandProjectile>();
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
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
                    "\nYou leave a trail of corruption that deal 15 damage and inflict cursed inferno";
            }
            if (type == ItemID.CactusHelmet || type == ItemID.CactusBreastplate || type == ItemID.CactusLeggings)
            {
                return "Increase defenses by 10" +
                    "\nWhen in desert biome :" +
                    "\nGetting hit will drop down a rolling cactus that is friendly with 5s cool down" +
                    "\nGetting hit will shoot out 8 cactus spike that is friendly deal 15 damage";
            }
            if (type == ItemID.PalmWoodHelmet || type == ItemID.PalmWoodBreastplate || type == ItemID.PalmWoodGreaves)
            {
                return "When in desert or ocean biome :" +
                    "\nIncrease defense by 16" +
                    "\nIncrease movement speed by 17%" +
                    "\nJumping will leave a trail of sand that deal 12 damage";
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
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.CactusHelmet, ItemID.CactusBreastplate, ItemID.CactusLeggings))
            {
                player.statDefense += 10;
                if (player.ZoneDesert)
                {
                    modplayer.CactusArmor = true;
                }
            }
            if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PalmWoodHelmet, ItemID.PalmWoodBreastplate, ItemID.PalmWoodGreaves))
            {
                if (player.ZoneBeach || player.ZoneDesert)
                {
                    player.statDefense += 16;
                    player.moveSpeed += .17f;
                    modplayer.PalmWoodArmor = true;
                }
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
            if (item.type == ItemID.Sandgun)
            {
                damage.Base -= .55f;
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
        public override void ResetEffects()
        {
            WoodArmor = false;
            BorealWoodArmor = false;
            RichMahoganyArmor = false;
            ShadewoodArmor = false;
            EbonWoodArmor = false;
            CactusArmor = false;
            PalmWoodArmor = false;
        }
        public override void PreUpdate()
        {
            base.PreUpdate();
            ShadewoodArmorCD = BossRushUtils.CoolDown(ShadewoodArmorCD);
            EbonWoodArmorCD = BossRushUtils.CoolDown(EbonWoodArmorCD);
            CactusArmorCD = BossRushUtils.CoolDown(CactusArmorCD);
            if (EbonWoodArmor)
                if (EbonWoodArmorCD <= 0 && Player.velocity != Vector2.Zero)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2Circular(10, 10), -Player.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<CorruptionTrail>(), 15, 0, Player.whoAmI);
                    EbonWoodArmorCD = 15;
                }
            if (PalmWoodArmor)
            {
                if (Player.justJumped)
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 vec = new Vector2(-Player.velocity.X, Player.velocity.Y).NextVector2RotatedByRandom(20).LimitedVelocity(Main.rand.NextFloat(2, 3));
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, vec, ModContent.ProjectileType<SandProjectile>(), 12, 1f, Player.whoAmI);
                    }
            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            OnHitEffect_RichMahoganyArmor(proj);
            OnHitEffect_CactusArmor(proj);
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            OnHitEffect_RichMahoganyArmor(npc);
            OnHitEffect_CactusArmor(npc);
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
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitNPC_ShadewoodArmor();
            OnHitNPC_BorealWoodArmor(target);
            OnHitNPC_WoodArmor(target);
        }
        private void OnHitNPC_WoodArmor(NPC target, Projectile proj = null)
        {
            if (WoodArmor)
                if (Main.rand.NextBool(4) && (proj is null || (proj is not null && proj.ModProjectile is not AcornProjectile)))
                    Projectile.NewProjectile(Player.GetSource_FromThis(),
                        target.Center - new Vector2(0, 400),
                        Vector2.UnitY * 5,
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
                    Player.Center.LookForHostileNPC(out List<NPC> npclist, 300f);
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