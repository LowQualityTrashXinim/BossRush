using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Note
{
    internal class Note1 : ModItem
    {
        public override string Texture => BossRushTexture.NOTE;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Note 01");
            /* Tooltip.SetDefault(
                "Hello, welcome to the mod, in this mod, you will be opening chest to get weapon to fight boss" +
                "\nTo get you quick on the right track, let introduce you to basic mechanic to of the mod and what you should take note of" +
                "\nChest usually contain weapon, accessories, armor, potion and some stuff you can build with with all of it is random" +
                "\nTho in later game, you won't get stuff to build with as it was deemed unneeded, chest item progress with your progress" +
                "\nTo open the chest, you simply right click it like a herb bag or treasure bags and it will give you stuff to progress" +
                "\nThis mod is best played in expert mode, you would see that you get a king slime spawner in wooden chest" +
                "\nWooden chest will always drop you 1 KS spawner, boss themselves will drop the spawner for the next boss" +
                "\nIf you die while fighting a boss (do not account for boss despawn) then you will get another shot at that boss" +
                "\nThere are no timer nor any rush in this mod,tho it is best if you try to fight boss as soon as possible" +
                "\nBut that doesn't mean you shouldn't prepare a arena to fight boss as it is intended for you to beat boss" +
                "\nYou may notice the Power Energy thing to craft King Scepter" +
                "\nAll i can say about it is that you will be fighting same boss but with some special add into the fight" +
                "\nfor now, this is all i can cover, i will see you after King slime"); */
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 41;
            Item.height = 29;
            Item.material = true;
            Item.rare = 0;
        }
    }
}
