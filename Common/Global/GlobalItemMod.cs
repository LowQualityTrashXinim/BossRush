using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;
using System.Linq;

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
                entity.shoot = ModContent.ProjectileType<SandInYourFace>();
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (item.type == ItemID.Sandgun)
            {
                tooltips.Add(new TooltipLine(Mod, "SandGunChange",
                    "The sand projectile no longer spawn upon kill" +
                    "\nDecrease damage by 35%"));
            }
            int[] armorSet = new int[] { player.armor[0].type, player.armor[1].type, player.armor[2].type };
            foreach (TooltipLine tooltipLine in tooltips)
            {
                if(tooltipLine.Name != "SetBonus")
                {
                    continue;
                }
                if(armorSet.Contains(item.type))
                {
                    tooltips.Add(new TooltipLine(Mod, ArmorSet.ConvertIntoArmorSetFormat(armorSet), GetToolTip(item.type)));
                    return;
                }
            }
        }
        private string GetToolTip(int type)
        {
            if(type == ItemID.WoodHelmet || type == ItemID.WoodBreastplate || type == ItemID.WoodGreaves)
            {
                return "When in forest" +
                    "\nIncrease defense by 15" +
                    "\nIncrease movement speed by 25%" +
                    "\nIncrease flat melee damage by 10";
            }
            return "";
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if(!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return "";
            }
            return new ArmorSet(head.type, body.type, legs.type).ToString();
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            if (set.Equals(ArmorSet.ConvertIntoArmorSetFormat(ItemID.WoodHelmet, ItemID.WoodBreastplate, ItemID.WoodGreaves)))
            {
                if (player.ZoneForest)
                {
                    player.statDefense += 15;
                    player.moveSpeed += .25f;
                    player.GetDamage(DamageClass.Melee).Base += 10;
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
                damage.Base -= .35f;
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
    class SandInYourFace : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SandBallGun);
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SandBallGun);
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            base.AI();
            if (Projectile.ai[0] >= 50)
            {
                if (Projectile.velocity.Y < 16)
                {
                    Projectile.velocity.Y += .25f;
                }
            }
            Projectile.ai[0]++;
        }
    }
}