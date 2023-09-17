using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Perks;
using BossRush.Texture;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Contents.WeaponModification
{
    /// <summary>
    /// This is the UI where we will handle all the stupid logic<br/>
    /// it should only be working on client side only and always load so make sure to use TryGet"X" here<br/>
    /// I will work with UI state as that allow us to change state of UI whenever active and deactive
    /// </summary>
    public class WeaponModificationUI: UIState
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
                for (int i = 0; i < 10; i++)
                {
                    Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(10, 360, i) * 10 * 20;
                    UIImageButton btn = new UIImageButton(TextureAssets.InventoryBack2);
                    btn.UISetWidthHeight(52, 52);
                    btn.UISetPosition(player.Center + offsetPos, originDefault);
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