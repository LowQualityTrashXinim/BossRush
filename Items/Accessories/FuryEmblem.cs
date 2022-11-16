using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;

namespace BossRush.Items.Accessories
{
    class FuryEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Your rage is what make battle turn tide\nIncrease 5% damage and crits\nIncrease HP by 25%\nGrant a buff upon getting hit that increase 50% damage and crits\nBut shatter your defense and cut your life regen\nThe buff will reset its time when you got hit again");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
            Item.rare = 7;
            Item.value = 10000000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.05f;
            player.GetCritChance<GenericDamageClass>() += 5;
            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.25f);
            player.GetModPlayer<FuryPlayer>().Furious2 = true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ItemID.DestroyerEmblem, 1)
           .AddIngredient(ItemID.AvengerEmblem, 1)
           .AddIngredient(ItemID.Vitamins, 1)
           .Register();
        }
    }
    class FuryPlayer : ModPlayer
    {
        public bool Furious2;
        public bool CooldownFurious;
        public override void ResetEffects()
        {
            Furious2 = false;
        }
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (Furious2 && !CooldownFurious)
            {
                Player.AddBuff(ModContent.BuffType<Furious>(), 600);
            }
        }
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (Furious2 && !CooldownFurious)
            {
                Player.AddBuff(ModContent.BuffType<Furious>(), 600);
            }
        }
    }
}
