using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.WeaponModification
{
    /// <summary>
    /// This is the UI where we will handle all the stupid logic<br/>
    /// it should only be working on client side only and always load so make sure to use TryGet"X" here<br/>
    /// I will work with UI state as that allow us to change state of UI whenever active and deactive
    /// </summary>
    public class WeaponModificationUI: UIState
    {
        internal static Texture2D slotSprite = TextureAssets.InventoryBack2.Value;
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
        public void updateSlot()
        {
            Vector2 origin = new Vector2(24, 24);

            this.UISetPosition(Main.LocalPlayer.Center + new Vector2(space * slotID, -96), origin);
            this.UISetWidthHeight(48, 48);
        }

        //apprently this is needed to make everthing work     
        public override int CompareTo(object obj)
        {
            WeaponModificationUI otherItem = obj as WeaponModificationUI;
            return slotID.CompareTo(otherItem.slotID);
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
        }
    }
    public class WeaponModificationSystem : ModSystem
    {
        internal UserInterface userInterface;
        internal WeaponModificationUI WM_uiState;

        public static ModKeybind WeaponModificationKeybind { get; private set; }

        public override void Load()
        {
            WeaponModificationKeybind = KeybindLoader.RegisterKeybind(Mod, "Weapon Modification", "P");

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