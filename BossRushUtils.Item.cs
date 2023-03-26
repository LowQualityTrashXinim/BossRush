using Terraria;
using Terraria.ModLoader;
using UtfUnknown.Core.Analyzers.Chinese;

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
            item.DamageType = DamageClass.Ranged;
        }
    }
}
