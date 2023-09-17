using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Contents.WeaponModification
{
    /// <summary>
    /// This is the UI where we will handle all the stupid logic<br/>
    /// it should only be working on client side only and always load so make sure to use TryGet"X" here<br/>
    /// I will work with UI state as that allow us to change state of UI whenever active and deactive
    /// </summary>
    public class WeaponModificationUI : UIState
    {
        internal float slotItemScale = .5f;
        internal int slotID = 0;
        internal int space = 64;
        internal bool isHidden = false;

        public int whoAmI = -1;

        //this is for intianlaizing the UI upon loading.
        public WeaponModificationUI(int slotOrder = 0)
        {
            slotID = slotOrder;
        }
        public override void OnActivate()
        {
            Elements.Clear();
            if (whoAmI == -1)
                return;
            Player player = Main.player[whoAmI];
            if (player.TryGetModPlayer(out WeaponModificationPlayer modplayer))
            {
                Vector2 originDefault = new Vector2(26, 26);
                Item item = player.HeldItem;
                int maxLengthX = 550;
                if (item.TryGetGlobalItem(out WeaponModificationGlobalItem globalItem))
                {
                    for (int i = 0; i < globalItem.ModWeaponSlotType.Length; i++)
                    {
                        Vector2 offsetPos = player.Center + new Vector2(MathHelper.Lerp(-maxLengthX, 20 * globalItem.ModWeaponSlotType.Length - maxLengthX, i / (globalItem.ModWeaponSlotType.Length - 1f)), -100);
                        offsetPos.X += 31 * i;
                        UIImageButton btn = new UIImageButton(TextureAssets.InventoryBack2);
                        btn.UISetWidthHeight(52, 52);
                        btn.UISetPosition(offsetPos, originDefault);
                        Append(btn);
                    }
                }
                for (int i = 0; i < modplayer.WeaponModification_inventory.Length; i++)
                {
                    Vector2 offset = new Vector2(MathHelper.Lerp(-maxLengthX, maxLengthX, i / (modplayer.WeaponModification_inventory.Length - 1f)), 100);
                    Vector2 position = player.Center + offset;
                    if (i >= (modplayer.WeaponModification_inventory.Length - 1) * .5f)
                    {
                        position -= new Vector2(maxLengthX, -55);
                    }
                    UIImageButton btn = new UIImageButton(TextureAssets.InventoryBack);
                    btn.UISetWidthHeight(52, 52);
                    btn.UISetPosition(position, originDefault);
                    Append(btn);
                }
            }
        }
        public override void OnDeactivate()
        {
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
        }
    }
    public class WeaponModificationSystem : ModSystem
    {
        internal UserInterface userInterface;
        internal WeaponModificationUI WM_uiState;

        public static ModKeybind WeaponModificationKeybind { get; private set; }

        public override void Load()
        {
            WeaponModificationKeybind = KeybindLoader.RegisterKeybind(Mod, "WeaponModification", "P");

            //UI stuff
            if (!Main.dedServ)
            {
                WM_uiState = new();
                userInterface = new();
            }
        }
        public override void Unload()
        {
            WeaponModificationKeybind = null;
        }
        public override void UpdateUI(GameTime gameTime)
        {
            userInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (InventoryIndex != -1)
            {
                layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
                    "BossRush: Weapon Modification",
                    delegate
                    {
                        userInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}