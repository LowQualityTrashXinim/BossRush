using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;

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
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.EoCShield)
            {
                player.GetModPlayer<EvilEyePlayer>().EoCShieldUpgrade = true;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.Sandgun)
            {
                tooltips.Add(new TooltipLine(Mod, "SandGunChange",
                    $"The sand projectile no longer spawn upon kill\nDecrease damage by 35%"));
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