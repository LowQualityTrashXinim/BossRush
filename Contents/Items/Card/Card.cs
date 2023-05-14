using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;
using System.Data.Common;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;

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
        private int ChooseID;
        public override void OnSpawn(IEntitySource source)
        {
            switch (Tier)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 1 = Copper<br/>
        /// 2 = Silver<br/>
        /// 3 = Gold<br/>
        /// 4 = Platinum<br/>
        /// </summary>
        public virtual int Tier => 0;
        public virtual bool CanBeCraft => true;
        public virtual void OnUseItem(Player player, PlayerCardHandle modplayer)
        {

        }
        public override void AddRecipes()
        {
            if (CanBeCraft)
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
        //Copper tier
        public float MeleeDamageMultiply = 0;
        public float RangeDamageMultiply = 0;
        public float MagicDamageMultiply = 0;
        public float SummonDamageMultiply = 0;

        public float MovementMulti = 1;
        public float HPMaxMulti = 0;
        public float LifeRegenMulti = 0;
        public float ManaMaxMulti = 0;
        public int DefenseBase = 0;
        //Silver Tier
        public float DamageMultiply = 0;
        public int CritStrikeChance = 0;
        /// <summary>
        /// Not implemented
        /// </summary>
        public float CritDamage = 1;
        public float ManaRegenMulti = 0;
        /// <summary>
        /// Not implemented
        /// </summary>
        public float ChanceToNoConsumeAmmo = 0;
        public float DefenseEffectiveness = 1;
        //Gold
        /// <summary>
        /// Not implemented
        /// </summary>
        public int DropAmountIncrease = 0;
        public float DamageReduction = 0;
        public int MinionSlot = 0;
        public int SentrySlot = 0;
        //Platinum
        //public float LuckIncrease = 0;
        public int ID;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            Player.GetDamage(DamageClass.Melee) += MeleeDamageMultiply;
            Player.GetDamage(DamageClass.Ranged) += RangeDamageMultiply;
            Player.GetDamage(DamageClass.Magic) += MagicDamageMultiply;
            Player.GetDamage(DamageClass.Summon) += SummonDamageMultiply;

            Player.GetDamage(DamageClass.Generic) += DamageMultiply;
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            crit += CritStrikeChance;
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;

            health.Base = HPMaxMulti * Player.statLifeMax;
            mana.Base = ManaMaxMulti * Player.statManaMax;
        }
        public override void ResetEffects()
        {
            Player.accRunSpeed *= MovementMulti;
            Player.lifeRegen = (int)(LifeRegenMulti * Player.lifeRegen);
            Player.manaRegen = (int)(ManaRegenMulti * Player.manaRegen);
            Player.statDefense += DefenseBase;
            Player.DefenseEffectiveness *= DefenseEffectiveness;
            Player.maxMinions += MinionSlot;
            Player.maxTurrets += SentrySlot;
            Player.endurance += DamageReduction;
        }

        //_Copper : ( 1 - 3 )%

        //_Silver : ( 3 - 8 )%

        //_Gold : ( 6 - 16 )%

        //_Platinum : ( 8 - 20)%
    }
    class CardNPCdrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {

        }
    }
}