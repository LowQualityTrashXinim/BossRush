using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.FrostSwordFish
{
    internal class FrostSwordFish : SynergyModItem
    {
        public override void SetDefaults()
        {
            BossRushUtils.BossRushSetDefault(Item, 60, 64, 37, 1f, 18, 18, BossRushUseStyle.GenericSwingDownImprove, true);
            Item.DamageType = DamageClass.Melee;

            Item.rare = 2;
            Item.crit = 5;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;
            Item.scale += 0.25f;
            Item.UseSound = SoundID.Item1;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 pos;
            do
            {
                pos = player.Center + Main.rand.NextVector2Circular(400f, 400f);
            }
            while (!Collision.CanHitLine(player.Center, 0, 0, pos, 0, 0));
            Projectile.NewProjectile(Item.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<FrostDaggerFishP>(), hit.Damage, hit.Knockback, player.whoAmI);
            target.AddBuff(BuffID.Frostburn, 180);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Vector2 hitboxCenter = new Vector2(hitbox.X, hitbox.Y);
            Dust.NewDust(hitboxCenter, hitbox.Width, hitbox.Height, DustID.IceRod, 0, 0, 0, Color.Aqua, .75f);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ItemID.FrostDaggerfish, 100)
                .Register();
        }
    }
}