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
using Terraria.DataStructures;
using System.Security.Permissions;
using System.Linq;
using Steamworks;

namespace BossRush.Contents.WeaponModification
{

    public class WeaponModificationUI: UIElement
    {
        internal static Texture2D slotSprite = TextureAssets.InventoryBack2.Value;
        internal Item item;
        internal float slotItemScale = .5f;
        internal int slotID = 0;
        internal int space = 64;
        internal bool isHidden = false;

        //this is for intianlaizing the UI upon loading.
        public WeaponModificationUI(int slotOrder = 0)
        {
            slotID = slotOrder;
            item = new Item();
        }

        //extract the infos about the item to make the UI update its data accourding the player's storedItems
        public void updateSlot(Item newItem)
        {
            //var player = Main.LocalPlayer.GetModPlayer<weaponMod_modPlayer>();
            //if (slotID > player.modplayer_selectedItem.GetGlobalItem<weaponMod_GlobalItem>().availableWeaponModSlots)
            //    isHidden = true;
            //else
            //    isHidden = false;

            Vector2 origin = new Vector2(24, 24);
            item.SetDefaults(newItem.type);

            this.UISetPosition(Main.LocalPlayer.Center + new Vector2(space * slotID, -96), origin);
            this.UISetWidthHeight(48, 48);
        }

        //apprently this is needed to make everthing work     
        public override int CompareTo(object obj)
        {
            WeaponModificationUI otherItem = obj as WeaponModificationUI;
            return slotID.CompareTo(otherItem.slotID);
        }

        //interaction
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);

            //if (IsMouseHovering && player.Player.itemAnimation == 0)
            //{
            //    //if(Main.mouseItem is not null)
            //    //    Main.mouseItem = new Item(player.storedItems[slotID]);


            //    //terraria for some reason uses the item at the same frame as picking it up, why? idk, this is banadage fix... maybe
            //    player.Player.reuseDelay = 30;
            //}


        }



        protected override void DrawSelf(SpriteBatch spriteBatch)
        {

            if (isHidden)
                return;


            CalculatedStyle innerDimensions = GetDimensions();
            innerDimensions = GetDimensions();
            innerDimensions.X += slotSprite.Width * slotItemScale / 2f;
            innerDimensions.Y += slotSprite.Height * slotItemScale / 2f;

            //draw the slot sprite first
            spriteBatch.Draw(slotSprite, GetDimensions().Position(), Color.White);
            // check if theres item to show an item sprite
            if (item != null)
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
    // this class serves as the middle man between the player's stored items and the slots themself, and its the holder (parent) of all the UIElements it creates (children).
    public class weaponMod_UI : UIState
    {
        public WeaponModificationUI itemSlot;
        public const int MAXSLOTS = 5;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //here is where we update the existing UIELEMETS that are inside the WM_ItemSlot itemSlot above.


            foreach (WeaponModificationUI i in Children)
            {

                //This is broken and will crash tmod if they open inventory or open cheatsheet/hero item picker, so I comment it
                //i.updateSlot(player.modplayer_selectedItem.GetGlobalItem<weaponMod_GlobalItem>().weaponMod_slot[i.slotID]);

            }
        }

        //this method creates the slots, and updates them if the player has items stored.
        public override void OnInitialize()
        {

            for (int i = 0; i < MAXSLOTS; i++)
            {
                itemSlot = new WeaponModificationUI(i);

                //the itemslot is basically a list that contains instances of WM_ItemSlot types, or atleast this is what i understand
                Append(itemSlot);


            }
        }
    }

    public class weaponMod_ModSystem : ModSystem
    {
        internal UserInterface userInterface;
        internal weaponMod_UI slot;
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
            return;
            if (Main.playerInventory)
                userInterface?.SetState(slot);
            else
                userInterface?.SetState(null);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            return;
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
    public class weaponMod_GlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int availableWeaponModSlots;
        public Item[] weaponMod_slot;


        public void Initialize_weaponModSlots()
        {

            availableWeaponModSlots = Main.rand.Next(0, 5);
            weaponMod_slot = new Item[availableWeaponModSlots];

            for (int i = 0; i < weaponMod_slot.Length; i++)
            {

                weaponMod_slot[i] = new Item(ItemID.AdamantiteBar);

            }

        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (availableWeaponModSlots == null)
                Initialize_weaponModSlots();
        }

        //public override void OnCreated(Item item, ItemCreationContext context)
        //{


        //}

        //public override void SaveData(Item item, TagCompound tag)
        //{
        //    base.SaveData(item, tag);
        //}
        //public override void LoadData(Item item, TagCompound tag)
        //{
        //    base.LoadData(item, tag);
        //}
    }
    public abstract class BaseModifierParticle : ModItem
    {
        public float DamageIncrease;
    }
    public class DamageIncreaseModifier : BaseModifierParticle
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            DamageIncrease = .1f;
        }
    }
}
