using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Spawner
{
    public class WallOfFleshSpawner : BaseSpawnerItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.GuideVoodooDoll, 1)
            .Register();
        }
        public override int[] NPCtypeToSpawn => new int[] { NPCID.WallofFlesh };
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GuideVoodooDoll);

        public override bool CanUseItem(Player player)
        {
            if (!NPC.AnyNPCs(NPCID.WallofFlesh) && player.ZoneUnderworldHeight)
                return true;

            return false;
        }
        float positionRotateX = 0f;
        float countX = 0f;
        private void PositionHandle()
        {
            if (positionRotateX < 3 && countX == 1)
            {
                positionRotateX += .3f;
            }
            else
            {
                countX = -1;
            }
            if (positionRotateX > 0 && countX == -1)
            {
                positionRotateX -= .3f;
            }
            else
            {
                countX = 1;
            }
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            PositionHandle();
            Main.instance.LoadItem(Item.type);
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(texture, position + new Vector2(positionRotateX, positionRotateX), null, Color.Purple, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(positionRotateX, -positionRotateX), null, Color.Purple, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, positionRotateX), null, Color.Purple, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, -positionRotateX), null, Color.Purple, 0, origin, scale, SpriteEffects.None, 0);
            }
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}