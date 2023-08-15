using Terraria;
using Terraria.UI;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Common;
using System.Linq;
using BossRush.Contents.Items.Chest;
using Terraria.ID;

namespace BossRush.Contents.Perks
{
    //Do all the check in UI state since that is where the perk actually get create and choose
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
                Perk[] perkchooser = new Perk[modplayer.PerkAmount];
                for (int i = 0; i < modplayer.PerkAmount; i++)
                {
                    if (i >= modplayer.PerkAmount - 1)
                    {
                        MaterialWeaponUIImageButton buttonWeapon = new MaterialWeaponUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
                        buttonWeapon.Width.Pixels = texture.Width;
                        buttonWeapon.Height.Pixels = texture.Height;
                        Vector2 offsetPosWeapon = Vector2.UnitY.Vector2DistributeEvenly(modplayer.PerkAmount, 360, i) * modplayer.PerkAmount * 20;
                        Vector2 drawposWeapon = player.Center + offsetPosWeapon - Main.screenPosition - origin;
                        buttonWeapon.Left.Pixels = drawposWeapon.X;
                        buttonWeapon.Top.Pixels = drawposWeapon.Y;
                        Append(buttonWeapon);
                        continue;
                    }
                    //here we will randomize and validate perk
                    Perk newperk = Main.rand.Next(modplayer.DictionaryPerk.Keys.ToArray());
                    if (!perkchooser.Contains(newperk))
                        perkchooser[i] = newperk;
                    //After that we assign perk
                    PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT), modplayer, i);
                    btn.perk = newperk;
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
        public Perk perk;
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
            //We are assuming the perk are auto handle
            if (perk is not null)
            {
                if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perk))
                    perkplayer.perks.Add(perk, 1);
                else
                    if (perkplayer.perks.ContainsKey(perk) && perk.CanBeStack)
                    perkplayer.perks.Add(perk, perkplayer.perks[perk] + 1);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
    class MaterialWeaponUIImageButton : UIImageButton
    {
        public MaterialWeaponUIImageButton(Asset<Texture2D> texture) : base(texture)
        {
        }
        public override void OnActivate()
        {
            base.OnActivate();
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            Player player = Main.LocalPlayer;
            for (int i = 0; i < 5; i++)
            {
                LootBoxBase.GetWeapon(out int weapon, out int amount);
                player.QuickSpawnItem(player.GetSource_FromThis(), weapon, amount);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D WeaponTexture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemID.IronBroadsword)).Value;
            Vector2 originWeapon = new Vector2(WeaponTexture.Width * .5f, WeaponTexture.Height * .5f);
            Vector2 drawposWeapon = new Vector2(Left.Pixels, Top.Pixels) + originWeapon * .5f;
            spriteBatch.Draw(WeaponTexture, drawposWeapon, Color.White);
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
    public class PerkPlayer : ModPlayer
    {
        public bool CanGetPerk = false;
        public int PerkAmount = 3;
        public Dictionary<Perk, int> perks = new Dictionary<Perk, int>();
        public readonly Dictionary<Perk, int> DictionaryPerk = new Dictionary<Perk, int>()
        {
            { new GenericDamageIncrease(), 3 },
            { new LifeForceOrb(), 1},
            { new ImmunityToPoison(), 1},

        };
        public bool HasPerk(Perk perk) => perks.Keys.Where(x => x == perk).Any();
        public override void ResetEffects()
        {
            foreach (Perk perk in perks.Keys) 
            {
                perk.ResetEffect();
                perk.StackAmount = perks[perk];
            }
        }
        public override void PostUpdate()
        {
            foreach (Perk perk in perks.Keys) { perk.Update(); }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            foreach (Perk perk in perks.Keys) { perk.ModifyDamage(item, ref damage); }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (Perk perk in perks.Keys) { perk.OnHitNPCWithItem(item, target, hit, damageDone); }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (Perk perk in perks.Keys) { perk.OnHitNPCWithProj(proj, target, hit, damageDone); }
        }
    }
    public abstract class Perk : ModType
    {
        protected Player player;
        protected PerkPlayer perkPlayer;
        public bool CanBeStack = false;
        /// <summary>
        /// Use this if <see cref="CanBeStack"/> is true
        /// <br/> This allow easy multiply
        /// </summary>
        public int StackAmount = 0;
        public string textureString = null;
        public string Tooltip = null;
        public int Type { get; internal set; }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode;
        }
        protected override sealed void Register()
        {
        }
        public override void Unload()
        {
            player = null;
            perkPlayer = null;
            textureString = null;
            Tooltip = null;
        }
        public override void Load()
        {
            base.Load();
            SetDefaults();
        }
        public virtual void SetDefaults()
        {

        }
        public Perk()
        {
            this.player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out PerkPlayer modplayer))
                perkPlayer = modplayer;
        }
        public Perk(Player player)
        {
            if (player is not null)
                this.player = player;
            else
                this.player = Main.LocalPlayer;
            perkPlayer = player.GetModPlayer<PerkPlayer>();
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
        public virtual void ModifyDamage(Item item, ref StatModifier damage)
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
            if (player.altFunctionUse != 2)
            {
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
            }
            else
            {
                modplayer.perks.Clear();
            }
            return base.UseItem(player);
        }
    }
}