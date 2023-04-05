using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush
{
    public partial class BossRushUtils
    {
        public static void BossRushSetDefault(this Item item,
            int width,
            int height,
            int damage,
            float knockback,
            int useTime,
            int useAnimation,
            int useStyle,
            bool autoReuse)
        {
            item.width = width;
            item.height = height;
            item.damage = damage;
            item.knockBack = knockback;
            item.useTime = useTime;
            item.useAnimation = useAnimation;
            item.useStyle = useStyle;
            item.autoReuse = autoReuse;
        }
        public static void BossRushSetDefaultMelee
            (this Item item,
            int width,
            int height,
            int damage,
            float knockback,
            int useTime,
            int useAnimation,
            int useStyle,
            bool autoReuse
            )
        {
            item.BossRushSetDefault(width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
            item.DamageType = DamageClass.Melee;
        }
        public static void BossRushDefaultRange
            (this Item item,
            int width,
            int height,
            int damage,
            float knockback,
            int useTime,
            int useAnimation,
            int useStyle,
            int shoot,
            float shootSpeed,
            bool autoReuse,
            int useAmmo = 0
        )
        {
            item.BossRushSetDefault(width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
            item.shoot = shoot;
            item.shootSpeed = shootSpeed;
            item.useAmmo = useAmmo;
            item.noMelee = true;
            item.DamageType = DamageClass.Ranged;
        }

        public static void BossRushDefaultMagic
            (this Item item,
            int width,
            int height,
            int damage,
            float knockback,
            int useTime,
            int useAnimation,
            int useStyle,
            int shoot,
            float shootSpeed,
            int manaCost,
            bool autoReuse
            )
        {
            item.BossRushSetDefault(width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
            item.shoot = shoot;
            item.shootSpeed = shootSpeed;
            item.mana = manaCost;
            item.noMelee = true;
            item.DamageType = DamageClass.Magic;
        }
        public static bool CheckUseStyleMelee(this Item item) => item.useStyle == ItemUseStyleID.Swing
            || item.useStyle == BossRushUseStyle.GenericSwingDownImprove
            || item.useStyle == BossRushUseStyle.Swipe
            || item.useStyle == BossRushUseStyle.Poke;
    }
}
