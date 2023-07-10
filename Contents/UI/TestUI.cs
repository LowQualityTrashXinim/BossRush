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

namespace BossRush.Contents.UI
{
    internal class BossRushAchievementButton : UIState
    {
        private UIText text;
        private UIElement area;
        private UIImageButton btn;
        private UIAchievementPanel achievementMenu;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Width.Set(BossRushUtils.ScreenWidth, 0);
            area.Height.Set(BossRushUtils.ScreenHeight, 0);

            ButtonCreate();
            TextGoesWithButtonCreate();
            AchievementMenuCreate();

            btn.Append(text);
            area.Append(btn);
            area.Append(achievementMenu);
            Append(area);
        }
        private void ButtonCreate()
        {
            btn = new UIImageButton(ModContent.Request<Texture2D>("BossRush/icon"));
            btn.Width.Set(80, 0f);
            btn.Height.Set(80, 0f);
            btn.Left.Set(0, 0f);
            btn.Top.Set(0, 0f);
            btn.HAlign = .7f;
            btn.VAlign = .05f;
            btn.OnLeftClick += new MouseEvent(OpenAchievementUI);
        }
        private void TextGoesWithButtonCreate()
        {
            text = new UIText("Mod Achievement", 0.8f);
            text.Width.Set(138, 0f);
            text.Height.Set(34, 0f);
            text.Top.Set(btn.Height.Pixels, 0f);
            text.Left.Set(btn.Width.Pixels * .5f - text.Width.Pixels * .5f, 0f);
        }
        private void AchievementMenuCreate()
        {
            achievementMenu = new UIAchievementPanel();
            achievementMenu.HAlign = .1f;
            achievementMenu.VAlign = .1f;
            area.Append(achievementMenu);
        }
        private void OpenAchievementUI(UIMouseEvent evt, UIElement listeningElement)
        {
            achievementMenu.hide = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
    class UIAchievementPanel : UIPanel
    {
        private UIImage barFrame;
        private UIImageButton btn;
        public bool hide = true;
        public override void OnInitialize()
        {
            barFrame = new UIImage(ModContent.Request<Texture2D>(BossRushTexture.ACHIEVEMENTMENUBLUE));
            barFrame.Width.Set(1204, 0);
            barFrame.Height.Set(984, 0);
            barFrame.ImageScale = .7f;
            Append(barFrame);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (hide)
            {
                return;
            }
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
        internal BossRushAchievementButton BRAbtn;
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