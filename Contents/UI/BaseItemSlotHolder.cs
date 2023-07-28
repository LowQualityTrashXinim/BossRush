//using Microsoft.Xna.Framework.Graphics;
//using System;
//using Terraria.GameInput;
//using Terraria;
//using Terraria.UI;
//using Microsoft.Xna.Framework;

//namespace BossRush.Contents.UI
//{
//    internal class BaseItemSlotHolder : UIElement
//    {
//        internal Item Item;
//        private readonly int _context;
//        private readonly float _scale;
//        internal Func<Item, bool> ValidItemFunc;

//        public BaseItemSlotHolder(int context = ItemSlot.Context.BankItem, float scale = 1f)
//        {
//            _context = context;
//            _scale = scale;
//            Item = new Item();
//            Item.SetDefaults(0);

//            Width.Set(Main.inventoryScale * scale, 0f);
//            Height.Set(Main.inventoryScale * scale, 0f);
//        }

//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            float oldScale = Main.inventoryScale;
//            Main.inventoryScale = _scale;
//            Rectangle rectangle = GetDimensions().ToRectangle();

//            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
//            {
//                Main.LocalPlayer.mouseInterface = true;
//                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
//                {
//                    // Handle handles all the click and hover actions based on the context.
//                    ItemSlot.Handle(ref Item, _context);
//                }
//            }
//            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
//            ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
//            Main.inventoryScale = oldScale;
//        }
//    }
//}