using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class MagicBow : SynergyModItem
    {
        int DustType = 0;
        public override void SetDefaults()
        {
            base.SetDefaults();
            MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType);
            Item.BossRushSetDefault(18, 32, damage, 1f, useTime, useTime, ItemUseStyleID.Shoot, true);
            DustType = dustType;
            Item.shoot = shoot;
            Item.shootSpeed = shootspeed;
            Item.mana = mana;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item75;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 CircularRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20)) + Main.rand.NextVector2Circular(3f, 3f);
                Dust.NewDustPerfect(position, DustType, CircularRan, 100, default, 0.5f);
            }
            position -= new Vector2(0, 5);
        }
        public virtual void MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType)
        {
            mana = 1;
            shoot = 1;
            shootspeed = 1;
            damage = 1;
            useTime = 1;
            dustType = 1;
        }
    }
}