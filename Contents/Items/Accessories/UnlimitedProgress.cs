using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories
{
    internal class UnlimitedProgress : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Untapped Potential");
            // Tooltip.SetDefault("To touch the limitless growth, you become stronger each time kill a boss, simply amazing");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 28;
            Item.width = 28;
            Item.rare = 7;
            Item.value = 10000000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NPC.downedBoss1)
            {
                player.statDefense += 1;
                player.GetDamage(DamageClass.Generic) += 0.1f;
                player.GetCritChance(DamageClass.Generic) += 1;
            }
            if (NPC.downedBoss2)
            {
                player.statDefense += 2;
                player.GetDamage(DamageClass.Generic) += 0.1f;
                player.GetCritChance(DamageClass.Generic) += 1;
            }
            if (NPC.downedBoss3)
            {
                player.statDefense += 3;
                player.GetDamage(DamageClass.Generic) += 0.1f;
                player.GetCritChance(DamageClass.Generic) += 1;
            }
            if (NPC.downedDeerclops)
            {
                player.statDefense += 5;
                player.GetDamage(DamageClass.Generic) += 0.1f;
                player.GetCritChance(DamageClass.Generic) += 1;
            }
            if (NPC.downedQueenBee)
            {
                player.statDefense += 5;
                player.GetDamage(DamageClass.Generic) += 0.1f;
                player.GetCritChance(DamageClass.Generic) += 3;
            }
            if (NPC.downedSlimeKing)
            {
                player.statDefense += 2;
                player.GetDamage(DamageClass.Generic) += 0.1f;
                player.GetCritChance(DamageClass.Generic) += 1;
            }
            if (NPC.downedQueenSlime)
            {
                player.statDefense += 4;
                player.GetDamage(DamageClass.Generic) += 0.2f;
                player.GetCritChance(DamageClass.Generic) += 2;
            }
            if (NPC.downedPlantBoss)
            {
                player.statDefense += 10;
                player.GetDamage(DamageClass.Generic) += 0.5f;
                player.GetCritChance(DamageClass.Generic) += 6;
            }
            if (NPC.downedAncientCultist)
            {
                player.statDefense += 10;
                player.GetDamage(DamageClass.Generic) += .5f;
                player.GetCritChance(DamageClass.Generic) += 7;
            }
            if (NPC.downedEmpressOfLight)
            {
                player.statDefense += 20;
                player.GetDamage(DamageClass.Generic) += 1.5f;
                player.GetCritChance(DamageClass.Generic) += 10;
            }
            if (NPC.downedGolemBoss)
            {
                player.statDefense += 15;
                player.GetDamage(DamageClass.Generic) += .5f;
                player.GetCritChance(DamageClass.Generic) += 8;
            }
            if (NPC.downedMechBoss1)
            {
                player.statDefense += 10;
                player.GetDamage(DamageClass.Generic) += .3f;
                player.GetCritChance(DamageClass.Generic) += 5;
            }
            if (NPC.downedMechBoss2)
            {
                player.statDefense += 10;
                player.GetDamage(DamageClass.Generic) += .3f;
                player.GetCritChance(DamageClass.Generic) += 5;
            }
            if (NPC.downedMechBoss3)
            {
                player.statDefense += 10;
                player.GetDamage(DamageClass.Generic) += .3f;
                player.GetCritChance(DamageClass.Generic) += 5;
            }
            if (NPC.downedMoonlord)
            {
                player.statDefense += 30;
                player.GetDamage(DamageClass.Generic) += 1.5f;
                player.GetCritChance(DamageClass.Generic) += 20;
            }
        }
    }
}