using Terraria;
using Terraria.UI;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

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
                    PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT), modplayer, i);
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
        Perk perk;
        int index;
        public PerkUIImageButton(Asset<Texture2D> texture, PerkPlayer perkPlayer, int index) : base(texture)
        {
            Width.Pixels = texture.Value.Width;
            Height.Pixels = texture.Value.Height;
            perkplayer = perkPlayer;
            this.index = index;
        }
        public override void OnActivate()
        {
            base.OnActivate();
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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
                    "BossRush: PerkSystem",
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
    class PerkPlayer : ModPlayer
    {
        public bool CanGetPerk = false;
        public int PerkAmount = 3;
        public List<Perk> perks = new List<Perk>();
        public override void ResetEffects()
        {
            foreach (Perk perk in perks)
            {
                perk.ResetEffect();
            }
        }
        public override void PostUpdate()
        {
            foreach (Perk perk in perks)
            {
                perk.Update();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (Perk perk in perks)
            {
                perk.OnHitNPCWithItem(item, target, hit, damageDone);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (Perk perk in perks)
            {
                perk.OnHitNPCWithProj(proj, target, hit, damageDone);
            }
        }
    }
    class Perk
    {
        public Texture2D texture;
        public short type = PerkID.None;
        Player player;
        public Perk()
        {
        }
        public Perk(Player player, Texture2D texture)
        {
            this.texture = texture;
            this.player = player;
        }
        public virtual int NewPerk(int type, int whoAmI)
        {
            Perk perk = new Perk();
            return type;
        }
        /// <summary>
        /// This will run in <see cref="ModPlayer.ResetEffects"/>
        /// </summary>
        public virtual void ResetEffect()
        {

        }
        /// <summary>
        /// This will run in <see cref="ModPlayer.PostUpdate"/>
        /// </summary>
        public virtual void Update()
        {

        }
        /// <summary>
        /// This will run in <see cref="ModPlayer.OnHitNPCWithItem(Item, NPC, NPC.HitInfo, int)"/>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        /// <param name="hit"></param>
        /// <param name="damageDone"></param>
        public virtual void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        /// <summary>
        /// This will run in <see cref="ModPlayer.OnHitNPCWithProj(Projectile, NPC, NPC.HitInfo, int)"/>
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="target"></param>
        /// <param name="hit"></param>
        /// <param name="damageDone"></param>
        public virtual void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
    }
    //This should be use to decide which perk will get spawn or get choosen
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
            if (player.ItemAnimationJustStarted)
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
    static class PerkID
    {
        public const short None = 0;

        public const short IllegalTrading = 1;

        public const short AlchemistKnowledge = 2;

        public const short IncreaseUniversalDamage = 3;

        public const short LifeForceParticle = 4;

        public const short ImmunityToPoison = 5;

        public const short YouMadePeaceWithGod = 6;

        public const short BackUpMana = 7;

        public const short SuppliesDrop = 8;

        public const short CardCollection = 9;
    }
}