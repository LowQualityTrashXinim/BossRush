using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.FrozenSwordFish
{
    internal class FrostSwordFish : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost SwordFish");
            Tooltip.SetDefault("The cursed cold cousin");
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 64;
            Item.rare = 2;

            Item.damage = 37;
            Item.crit = 5;
            Item.knockBack = 1f;

            Item.useTime = 18;
            Item.useAnimation = 18;

            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(gold: 50);
            Item.autoReuse = true;
            Item.useTurn = false;

            Item.scale += 0.25f;

            Item.UseSound = SoundID.Item1;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            Vector2 pos;
            do
            {
                pos = player.Center + Main.rand.NextVector2Circular(400f, 400f);
            }
            while (!Collision.CanHitLine(player.Center, 0, 0, pos, 0, 0));
            Projectile.NewProjectile(Item.GetSource_FromThis(),pos , Vector2.Zero, ModContent.ProjectileType<FrostDaggerFishP>(), damage, knockBack, player.whoAmI);
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
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
