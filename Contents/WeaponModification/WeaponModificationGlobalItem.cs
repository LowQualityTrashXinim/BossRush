using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace BossRush.Contents.WeaponModification
{
    internal class WeaponModificationGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int availableWeaponModSlots;
        public Item[] weaponMod_slot;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            base.OnCreated(item, context);
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            base.SaveData(item, tag);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            base.LoadData(item, tag);
        }
    }
}
