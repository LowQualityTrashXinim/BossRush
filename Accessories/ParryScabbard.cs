using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;
using Terraria.DataStructures;

namespace BossRush.Accessories
{
    internal class ParryScabbard : ModItem
    {
        public override string Texture => "BossRush/MissingTexture";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"Made for display of skill\"" +
                "\nGetting hit during the attack swing will grant you 2s of immunity of damage" +
                "\nHave 6s cool down before you able to parry again" +
                "\nDuring parry period, you gain 10% damage" +
                "\nOnly work if weapon is a sword");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 28;
            Item.rare = 2;
            Item.value = 1000000;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<ParryPlayer>().Parry = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood Sword")
                .AddRecipeGroup("OreBroadSword")
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
    public class ParryPlayer : ModPlayer
    {
        public bool Parry;
        public override void ResetEffects()
        {
            Parry = false;
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (!Player.HasBuff(ModContent.BuffType<Parried>()) && Player.HeldItem.DamageType == DamageClass.Melee && Player.ItemAnimationActive && Player.HeldItem.useStyle == ItemUseStyleID.Swing && !Player.HasBuff(ModContent.BuffType<CoolDownParried>()) && Parry)
            {
                Player.AddBuff(ModContent.BuffType<Parried>(), 120);
            }
            if (Player.HasBuff(ModContent.BuffType<Parried>()))
            {
                return false;
            }
            return true;
        }
    }
}
