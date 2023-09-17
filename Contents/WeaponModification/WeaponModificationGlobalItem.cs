using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;

namespace BossRush.Contents.WeaponModification
{
    internal class WeaponModificationGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public int Delay = 0;
        public int Recharge = 0;
        public int[] ModWeaponSlotType;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (item.damage > 0)
            {
                Delay = Main.rand.Next(0, 400);
                Recharge = Main.rand.Next(0, 400);
                ModWeaponSlotType = new int[Main.rand.Next(2, 26)];
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (item.damage > 0)
            {
                if (ModWeaponSlotType == null)
                {
                    Delay = Main.rand.Next(0, 400);
                    Recharge = Main.rand.Next(0, 400);
                    ModWeaponSlotType = new int[Main.rand.Next(2, 26)];
                }
            }
        }
        public override void HoldItem(Item item, Player player)
        {
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.damage > 0 && ModWeaponSlotType != null)
            {
                tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Delay : {Math.Round(Delay / 60f, 2)}s"));
                tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Recharge : {Math.Round(Recharge / 60f, 2)}s"));
                //tooltips.Add(new TooltipLine(Mod, "ItemDelay", $"Slot : {ModWeaponSlotType.Length}"));
            }
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("ItemDelay", Delay);
            tag.Add("ItemRecharge", Recharge);
            tag.Add("ModWeaponSlotType", ModWeaponSlotType);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("ItemDelay", out int DelayValue))
                Delay = DelayValue;
            if (tag.TryGet("ItemRecharge", out int RechargeValue))
                Recharge = RechargeValue;
            if (tag.TryGet("ModWeaponSlotType", out int[] TypeValue))
                ModWeaponSlotType = TypeValue;
        }
    }
}
