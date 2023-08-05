using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using BossRush.Contents.Items.Card;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Common.Utils;
using BossRush.Texture;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Events;
using ReLogic.Content;

namespace BossRush.Contents.UI.weaponModifiction
{
    //why items dosent show????

    public class WM_ItemSlot : UIElement
    {
        internal static Texture2D slotSprite = TextureAssets.InventoryBack2.Value;
        private Asset<Texture2D> _texture;
        internal Item item;
        internal float slotItemScale = .5f;
        internal int slotID = 0;
        internal CalculatedStyle pos;
        internal Vector2 screenCenter = new Vector2(Main.screenWidth + 25, Main.screenHeight - 20) / 2f;
        internal int space = 12;
        //just get item ID ONLY, and then we can get everything about it (texture, name, etc...)
        public WM_ItemSlot(int itemID,int slotOrder = 0)
        {
            slotID = itemID;
            pos.X = slotOrder * 64 + screenCenter.X + space;
            pos.Y = screenCenter.Y;
            item = new Item();
            item.SetDefaults(itemID);
            _texture = TextureAssets.Item[itemID];
            getItemInfoForUI(item);

        }
        //extract the infos about the item to make the UI use some of it
        public void getItemInfoForUI(Item item)
        {

            Height.Set(item.height * slotItemScale, 0f);
            Width.Set(item.width * slotItemScale, 0f);
        }


        public override int CompareTo(object obj)
        {
            WM_ItemSlot Test = obj as WM_ItemSlot;
            return slotID.CompareTo(Test.slotID);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {

            CalculatedStyle innerDimensions = GetInnerDimensions();
            innerDimensions = pos;
            innerDimensions.X += slotSprite.Width * slotItemScale / 2f;
            innerDimensions.Y += slotSprite.Height * slotItemScale / 2f; 

            //draw the slot sprite first
            spriteBatch.Draw(slotSprite, pos.Position(), Color.White);

            // check if theres item to show an item sprite
            
            if(item != null)
            {
                Main.instance.LoadItem(item.type);
                item.GetColor(Color.Beige);
                Color itemAlpha = item.GetAlpha(Color.White);
                Color itemColor = item.GetColor(Color.White);

                Rectangle rect = TextureAssets.Item[item.type].Frame(1, 1, 0, 0);
                spriteBatch.Draw(TextureAssets.Item[item.type].Value, innerDimensions.Position(), new Rectangle?(rect), itemAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(TextureAssets.Item[item.type].Value, innerDimensions.Position(), new Rectangle?(rect), itemColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                if (IsMouseHovering)
                    Main.hoverItemName = item.Name;



            }

        }
    }

    public class WM_UI : UIState
    {

        public WM_ItemSlot itemSlot;
        

        public override void OnInitialize()
        {
            var player = Main.LocalPlayer.GetModPlayer<WM_modPlayer>();
            


            for(int i = 0; i < player.WM_availableSlots; i++)
            {

                itemSlot = new WM_ItemSlot(player.storedItems[i], i);

                Append(itemSlot);

            }


            

        }
    }

    public class WM_ModSystem : ModSystem
    {
        internal UserInterface userInterface;
        internal WM_UI slot;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                
                slot = new();
                userInterface = new();
                


            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            userInterface?.Update(gameTime);
            if (Main.playerInventory)
            {
                userInterface?.SetState(slot);

            } 
            else 
                userInterface?.SetState(null);
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

    public class WM_modPlayer : ModPlayer
    {
        public const int WM_MAXSLOTS = 12;
        public const int WM_STARTINGSLOTS = 4;
        public int WM_availableSlots = WM_STARTINGSLOTS;
        public int[] storedItems = new int[WM_MAXSLOTS];


        public override void OnEnterWorld()
        {
            storedItems[0] = 4;
            storedItems[1] = 2;
            storedItems[2] = 98;
        }

        public override void Initialize()
        {
            base.Initialize();
            WM_availableSlots = WM_STARTINGSLOTS;

        }

        public override void SaveData(TagCompound tag)
        {
            tag["storedItems"] = storedItems;
            tag["WM_availableSlots"] = WM_availableSlots;
        }

        public override void LoadData(TagCompound tag)
        {

            storedItems = tag.GetIntArray("storedItems");
            WM_availableSlots = tag.GetInt("WM_availableSlots");

        }
    }
}
