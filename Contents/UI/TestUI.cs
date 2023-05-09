using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Contents.UI
{
    class UISystem : ModSystem
    {
        UserInterface userInterface;
        CardUIstate cardUIstate;
        public override void Load()
        {
            if(!Main.dedServ)
            {
                userInterface = new UserInterface();
                cardUIstate = new CardUIstate();
                cardUIstate.Activate();
            }
        }
        public override void Unload()
        {
            cardUIstate = null;
        }
        GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (userInterface?.CurrentState != null)
            {
                userInterface.Update(gameTime);
            }
        }

    }
    internal class CardUIstate : UIState
    {
        public UIImageButton buttonImage;
        public override void OnInitialize()
        {
            buttonImage = new UIImageButton(ModContent.Request<Texture2D>("Terraria/UI/ButtonPlay"));
            base.OnInitialize();
        }
    }
}