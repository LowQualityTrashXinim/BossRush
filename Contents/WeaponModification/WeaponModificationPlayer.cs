using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using BossRush.Contents.Perks;

namespace BossRush.Contents.WeaponModification
{
    /// <summary>
    /// This should be where all the effect of Weapon Modification be handle and activate
    /// </summary>
    internal class WeaponModificationPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (WeaponModificationSystem.WeaponModificationKeybind.JustPressed)
            {
                WeaponModificationSystem uiSystemInstance = ModContent.GetInstance<WeaponModificationSystem>();
                if (uiSystemInstance.userInterface is null)
                {
                    uiSystemInstance.WM_uiState.whoAmI = Player.whoAmI;
                    uiSystemInstance.userInterface.SetState(uiSystemInstance.WM_uiState);
                }
                else
                {
                    uiSystemInstance.userInterface.SetState(null);
                }
            }
        }
        //public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        //{
        //    foreach (int CustomParticle in item.GetGlobalItem<weaponMod_GlobalItem>().weaponMod_slot)
        //    {
        //        if(ItemLoader.GetItem(CustomParticle) is DamageIncreaseModifier particle)
        //        {
        //            damage += particle.DamageIncrease;
        //        }
        //    }
        //}
    }
}
