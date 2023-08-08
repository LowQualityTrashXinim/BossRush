using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Common.Utils;
using BossRush.Texture;
using System.IO;

namespace BossRush.Contents.UI
{
    internal class BossRushAchievementUI : UIState
    {
        public override void OnInitialize()
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            //Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT).Value;
            //Vector2 origin = new Vector2(texture.Width * .5f, texture.Height * .5f);
            //float perkAmount = 7f;
            //for (int i = 0; i < perkAmount; i++)
            //{
            //    Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(perkAmount, 360, i) * perkAmount * 20;
            //    Vector2 drawpos = (player.Center + offsetPos) - Main.screenPosition;
            //    spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            //}
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
    class UISystem : ModSystem
    {
        private UserInterface userInterface;
        internal BossRushAchievementUI BRAbtn;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                BRAbtn = new();
                userInterface = new();
                userInterface.SetState(BRAbtn);
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            userInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "BossRush: AchievementButton",
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