using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BossRush.Contents.WeaponModification
{
    /// <summary>
    /// This should be where all the effect of Weapon Modification be handle and activate
    /// </summary>
    internal class WeaponModificationPlayer : ModPlayer
    {
        public int[] WeaponModification_inventory = new int[20];
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (WeaponModificationSystem.WeaponModificationKeybind.JustPressed)
            {
                WeaponModificationSystem uiSystemInstance = ModContent.GetInstance<WeaponModificationSystem>();
                //if ((uiSystemInstance.userInterface.CurrentState == null)
                //{
                //    BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.Red, "You must hold a weapon !");
                //    return;
                //}
                if (uiSystemInstance.userInterface.CurrentState is null)
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
        public override bool CanUseItem(Item item)
        {
            WeaponModificationSystem uiSystemInstance = ModContent.GetInstance<WeaponModificationSystem>();
            if (uiSystemInstance.userInterface.CurrentState is not null)
            {
                return false;
            }
            return base.CanUseItem(item);
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
