using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.Card
{
    abstract class Card : ModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(1, 1);
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item35;
            PostCardSetDefault();
        }
        public virtual void PostCardSetDefault() { }
        public override bool? UseItem(Player player)
        {
            PlayerCardHandle modplayer = player.GetModPlayer<PlayerCardHandle>();
            OnUseItem(player, modplayer);
            return true;
        }
        public virtual bool CanBeCraft => true;
        public virtual void OnUseItem(Player player, PlayerCardHandle modplayer)
        {

        }
        public override void AddRecipes()
        {
            if(CanBeCraft)
            {
                CreateRecipe()
                    .AddIngredient(ModContent.ItemType<EmptyCard>())
                    .Register();
            }
        }
    }
    class PlayerCardHandle : ModPlayer
    {
        public ChestLootDropPlayer ChestLoot => Player.GetModPlayer<ChestLootDropPlayer>();
        public float DamageMultiply = 1;
        public float MeleeDamageMultiply = 1;
        public float RangeDamageMultiply = 1;
        public float MagicDamageMultiply = 1;
        public float SummonDamageMultiply = 1;
        public int CritStrikeChance = 0;
        public float CritDamage = 1;
        public float ManaMaxMulti = 1;
        public float HPMaxMulti = 1;
        public float MovementMulti = 1;
        public int DefenseBase = 0;
    }
}