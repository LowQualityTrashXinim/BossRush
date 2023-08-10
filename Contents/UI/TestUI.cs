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
using BossRush.Contents.Items.NohitReward;
using ReLogic.Content;

namespace BossRush.Contents.UI
{
    internal class PerkUIState : UIState
    {
        public override void OnActivate()
        {
            base.OnActivate();
            Elements.Clear();
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out PerkPlayer modplayer))
            {
                Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT).Value;
                Vector2 origin = new Vector2(texture.Width * .5f, texture.Height * .5f);
                for (int i = 0; i < modplayer.PerkAmount; i++)
                {
                    PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT), modplayer);
                    btn.Width.Pixels = texture.Width;
                    btn.Height.Pixels = texture.Height;
                    Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(modplayer.PerkAmount, 360, i) * modplayer.PerkAmount * 20;
                    Vector2 drawpos = player.Center + offsetPos - Main.screenPosition - origin;
                    btn.Left.Pixels = drawpos.X;
                    btn.Top.Pixels = drawpos.Y;
                    Append(btn);
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
    class PerkUIImageButton : UIImageButton
    {
        PerkPlayer perkplayer;
        public PerkUIImageButton(Asset<Texture2D> texture, PerkPlayer perkPlayer) : base(texture)
        {
            Width.Pixels = texture.Value.Width;
            Height.Pixels = texture.Value.Height;
            perkplayer = perkPlayer;
        }
    }
    class UISystem : ModSystem
    {
        public UserInterface userInterface;
        public PerkUIState perkUIstate;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                perkUIstate = new();
                userInterface = new();
                userInterface.SetState(perkUIstate);
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
    public enum Perk
    {
        //Stackable Perk
        IllegalTrading,
        AlchemistKnowledge,
        IncreaseUniversalDamage,
        //Not stackable perk
        ImmunityToPoison,
        YouMadePeaceWithGod,
        BackUpMana,
        //Misc perk
        SuppliesDrop,

    }
    class PerkPlayer : ModPlayer
    {
        public bool CanGetPerk = false;
        public int PerkAmount = 3;
        public Dictionary<Perk, int> perkDictionary = new Dictionary<Perk, int>();

        //Stackable Perk
        public int TradeDamageForWeapon = 0;
        //Not stackable perk
        //Misc perk
        public override void ResetEffects()
        {
            NoHitPlayerHandle nohitplayer = Player.GetModPlayer<NoHitPlayerHandle>();
            PerkAmount = 3 + nohitplayer.BossNoHitNumber.Count;
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
        }
    }
    class PerkChooser : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 23);
        }
        bool check = false;
        public override bool? UseItem(Player player)
        {
            PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
            UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
            check = !check;
            if (check && player.ItemAnimationEndingOrEnded)
            {
                uiSystemInstance.userInterface.SetState(uiSystemInstance.perkUIstate);
            }
            else
            {
                uiSystemInstance.userInterface.SetState(null);
            }
            return base.UseItem(player);
        }
    }
}