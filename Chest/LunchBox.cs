using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace BossRush.Chest
{
    internal class LunchBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Made with love \n-From your mom and your crush "+$"[i:{58}]");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 46;
            Item.rare = 5;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            var entitysource = player.GetSource_OpenItem(Type);
            int WeakDrink = Main.rand.Next(new int[] {ItemID.Teacup, ItemID.TropicalSmoothie,ItemID.SmoothieofDarkness, ItemID.PinaColada,ItemID.PeachSangria,ItemID.Lemonade, ItemID.FruitJuice,ItemID.BananaDaiquiri, ItemID.AppleJuice,ItemID.BloodyMoscato, ItemID.MilkCarton });
            int Smallmeal = Main.rand.Next(new int[] {ItemID.CookedMarshmallow,ItemID.RoastedBird, ItemID.SauteedFrogLegs,ItemID.GrilledSquirrel, ItemID.FruitSalad, ItemID.CookedFish, ItemID.BunnyStew, ItemID.PotatoChips,ItemID.ShuckedOyster });
            int fruit = Main.rand.Next(new int[] { ItemID.Lemon,ItemID.Peach, ItemID.Apple, ItemID.Apricot,ItemID.BlackCurrant,ItemID.Elderberry,ItemID.Grapefruit,ItemID.Mango,ItemID.Mango,ItemID.Plum,ItemID.Rambutan,ItemID.Coconut,ItemID.BloodOrange,ItemID.Grapes, ItemID.Dragonfruit, ItemID.Starfruit});

            int MediumMeal = Main.rand.Next(new int[] {ItemID.Sashimi,ItemID.PumpkinPie,ItemID.GrubSoup,ItemID.CookedShrimp,ItemID.BowlofSoup,ItemID.RoastedDuck,ItemID.MonsterLasagna, ItemID.LobsterTail,ItemID.FroggleBunwich,ItemID.Escargot, ItemID.Nachos, ItemID.ShrimpPoBoy,ItemID.Pho,ItemID.PadThai,ItemID.Fries,ItemID.Hotdog,ItemID.FriedEgg, ItemID.BananaSplit,ItemID.ChickenNugget,ItemID.ChocolateChipCookie });
            int MediumDrink = Main.rand.Next(new int[] { ItemID.PrismaticPunch, ItemID.IceCream, ItemID.CreamSoda,ItemID.CoffeeCup });

            int BigMeal = Main.rand.Next(new int[] { ItemID.SeafoodDinner,ItemID.GoldenDelight,ItemID.ApplePie,ItemID.BBQRibs,ItemID.Burger, ItemID.Pizza,ItemID.Spaghetti,ItemID.Steak,ItemID.Bacon,ItemID.ChristmasPudding,ItemID.GingerbreadCookie,ItemID.SugarCookie});
            int StrongDrink = Main.rand.Next(new int[] { ItemID.Milkshake, ItemID.Grapefruit });

            for (int i = 0; i < 10; i++)
            {
                int amount = Main.rand.Next(1, 12);
                int Chooser = Main.rand.Next(new int[] { WeakDrink, Smallmeal, fruit, MediumDrink, MediumMeal, BigMeal, StrongDrink });
                player.QuickSpawnItem(entitysource, Chooser,amount);
                if(Main.getGoodWorld && Main.masterMode && Main.rand.NextBool(5))
                {
                    player.QuickSpawnItem(entitysource, ItemID.RedPotion);
                }
            }
        }
    }
}
