using Terraria;
using Terraria.ID;
using Terraria.UI;
using System.Linq;
using BossRush.Common;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Card;

namespace BossRush.Contents.Perks
{
    //Do all the check in UI state since that is where the perk actually get create and choose
    internal class PerkUIState : UIState
    {
        public int whoAmI = -1;
        public override void OnActivate()
        {
            base.OnActivate();
            Elements.Clear();
            if (whoAmI == -1)
                return;
            Player player = Main.player[whoAmI];
            if (player.TryGetModPlayer(out PerkPlayer modplayer))
            {
                Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT).Value;
                Vector2 origin = new Vector2(texture.Width * .5f, texture.Height * .5f);
                Perk[] perkchooser = new Perk[modplayer.PerkAmount];
                for (int i = 0; i < modplayer.PerkAmount; i++)
                {
                    Perk newperk = Main.rand.Next(modplayer.DictionaryPerk.Keys.ToArray());
                    if (i >= 2 && (i >= ModPerkLoader.TotalCount
                        || i >= modplayer.PerkAmount - 3
                        || (modplayer.perks.ContainsKey(newperk) && !newperk.CanBeStack & modplayer.perks[newperk] >= modplayer.DictionaryPerk[newperk])
                        ))
                    {
                        UIImageButton buttonWeapon = Main.rand.Next(new UIImageButton[]
                        { new MaterialPotionUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
                         new MaterialCardUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
                         new MaterialWeaponUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT))
                        });
                        if (i >= modplayer.PerkAmount - 3)
                        {
                            buttonWeapon.Width.Pixels = texture.Width;
                            buttonWeapon.Height.Pixels = texture.Height;
                            Vector2 offsetPosWeapon = Vector2.UnitY.Vector2DistributeEvenly(modplayer.PerkAmount, 360, i) * modplayer.PerkAmount * 20;
                            Vector2 drawposWeapon = player.Center + offsetPosWeapon - Main.screenPosition - origin;
                            buttonWeapon.Left.Pixels = drawposWeapon.X;
                            buttonWeapon.Top.Pixels = drawposWeapon.Y;
                            Append(buttonWeapon);
                            continue;
                        }
                    }
                    // The above code will ensure that perk randomizer and perk chooser will never dupe and will never goes infinite
                    // Here we will randomize and validate perk

                    while (perkchooser.Contains(newperk) ||
                        modplayer.perks.ContainsKey(newperk) ||
                        (!newperk.CanBeStack && modplayer.perks[newperk] > 0) ||
                        modplayer.perks[newperk] >= modplayer.DictionaryPerk[newperk])
                    {
                        newperk = Main.rand.Next(modplayer.DictionaryPerk.Keys.ToArray());
                    }
                    perkchooser[i] = newperk;
                    //After that we assign perk
                    PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT), modplayer);
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
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
        private UIText toolTip;
        public PerkUIImageButton(Asset<Texture2D> texture, PerkPlayer perkPlayer) : base(texture)
        {
            Width.Pixels = texture.Value.Width;
            Height.Pixels = texture.Value.Height;
            perkplayer = perkPlayer;
        }
        public override void OnActivate()
        {
            base.OnActivate();
            toolTip = new UIText("");
            toolTip.HAlign = .5f;
            Append(toolTip);
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            //We are assuming the perk are auto handle
            perk.player = Main.LocalPlayer;
            perk.perkPlayer = perkplayer;
            if (perk is not null)
            {
                if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perk))
                    perkplayer.perks.Add(perk, 1);
                else
                    if (perkplayer.perks.ContainsKey(perk) && perk.CanBeStack)
                    perkplayer.perks[perk] = perkplayer.perks[perk] + 1;
            }
            UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
            uiSystemInstance.userInterface.SetState(null);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMouseHovering)
            {
                toolTip.Left.Pixels = Main.MouseScreen.X - Left.Pixels;
                toolTip.Top.Pixels = Main.MouseScreen.Y - Top.Pixels - 20;
                toolTip.SetText(perk.Tooltip);
            }
            else
            {
                toolTip.SetText("");
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (perk.textureString == null)
            {
                return;
            }
            Texture2D WeaponTexture = ModContent.Request<Texture2D>(perk.textureString).Value;
            Vector2 originWeapon = new Vector2(WeaponTexture.Width * .5f, WeaponTexture.Height * .5f);
            Vector2 drawposWeapon = new Vector2(Left.Pixels, Top.Pixels) + originWeapon * .5f;
            spriteBatch.Draw(WeaponTexture, drawposWeapon, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
    class MaterialWeaponUIImageButton : UIImageButton
    {
        private UIText toolTip;
        public MaterialWeaponUIImageButton(Asset<Texture2D> texture) : base(texture)
        {
        }
        public override void OnActivate()
        {
            base.OnActivate();
            toolTip = new UIText("");
            toolTip.HAlign = .5f;
            Append(toolTip);
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            Player player = Main.LocalPlayer;
            LootBoxBase.GetWeapon(out int weapon, out int amount);
            player.QuickSpawnItem(player.GetSource_FromThis(), weapon, amount);
            UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
            uiSystemInstance.userInterface.SetState(null);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Texture2D WeaponTexture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemID.IronBroadsword)).Value;
            Vector2 originWeapon = new Vector2(WeaponTexture.Width * .5f, WeaponTexture.Height * .5f);
            Vector2 drawposWeapon = new Vector2(Left.Pixels, Top.Pixels) + originWeapon * .5f;
            spriteBatch.Draw(WeaponTexture, drawposWeapon, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMouseHovering)
            {
                toolTip.Left.Pixels = Main.MouseScreen.X - Left.Pixels;
                toolTip.Top.Pixels = Main.MouseScreen.Y - Top.Pixels - 20;
                toolTip.SetText("Give you 1 randomize weapon based on progression");
            }
            else
            {
                toolTip.SetText("");
            }
        }
    }
    class MaterialCardUIImageButton : UIImageButton
    {
        private UIText toolTip;
        public MaterialCardUIImageButton(Asset<Texture2D> texture) : base(texture)
        {
        }
        public override void OnActivate()
        {
            base.OnActivate();
            toolTip = new UIText("");
            toolTip.HAlign = .5f;
            Append(toolTip);
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            Player player = Main.LocalPlayer;
            player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<BigCardPacket>());
            UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
            uiSystemInstance.userInterface.SetState(null);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Texture2D WeaponTexture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemID.Chest)).Value;
            Vector2 originWeapon = new Vector2(WeaponTexture.Width * .5f, WeaponTexture.Height * .5f);
            Vector2 drawposWeapon = new Vector2(Left.Pixels, Top.Pixels) + originWeapon * .5f;
            spriteBatch.Draw(WeaponTexture, drawposWeapon, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMouseHovering)
            {
                toolTip.Left.Pixels = Main.MouseScreen.X - Left.Pixels;
                toolTip.Top.Pixels = Main.MouseScreen.Y - Top.Pixels - 20;
                toolTip.SetText("Give you a big card packet");
            }
            else
            {
                toolTip.SetText("");
            }
        }
    }
    class MaterialPotionUIImageButton : UIImageButton
    {
        private UIText toolTip;
        public MaterialPotionUIImageButton(Asset<Texture2D> texture) : base(texture)
        {
        }
        public override void OnActivate()
        {
            base.OnActivate();
            toolTip = new UIText("");
            toolTip.HAlign = .5f;
            Append(toolTip);
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            Player player = Main.LocalPlayer;
            for (int i = 0; i < 5; i++)
            {
                player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<MysteriousPotion>());
            }
            UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
            uiSystemInstance.userInterface.SetState(null);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Texture2D WeaponTexture = ModContent.Request<Texture2D>(BossRushTexture.MISSINGTEXTUREPOTION).Value;
            Vector2 originWeapon = new Vector2(WeaponTexture.Width * .5f, WeaponTexture.Height * .5f);
            Vector2 drawposWeapon = new Vector2(Left.Pixels, Top.Pixels) + originWeapon * .5f;
            spriteBatch.Draw(WeaponTexture, drawposWeapon, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMouseHovering)
            {
                toolTip.Left.Pixels = Main.MouseScreen.X - Left.Pixels;
                toolTip.Top.Pixels = Main.MouseScreen.Y - Top.Pixels - 20;
                toolTip.SetText("Give you 5 mysterious potions");
            }
            else
            {
                toolTip.SetText("");
            }
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
        public Dictionary<int, int> perks = new Dictionary<int, int>();

        private int[] _perks;
        public override void Initialize()
        {
            _perks = new int[ModPerkLoader.TotalCount];
        }

        public bool HasPerk<T>() where T : Perk => _perks[Perk.GetPerkType<T>()] > 0;
        public bool HasPerk(Perk perk) => _perks[perk.Type] > 0;
        public override void ResetEffects()
        {
            PerkAmount = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count + 3;
            foreach (Perk perk in perks.Keys)
            {
                perk.ResetEffect();
                perk.StackAmount = perks[perk];
            }
        }
        public override void PreUpdate()
        {
            foreach (Perk perk in perks.Keys) { perk.player = Player; perk.perkPlayer = Player.GetModPlayer<PerkPlayer>(); }
        }
        public override void PostUpdate()
        {
            foreach (Perk perk in perks.Keys) { perk.Update(); }
        }
        public override void OnMissingMana(Item item, int neededMana)
        {
            foreach (Perk perk in perks.Keys) { perk.OnMissingMana(item, neededMana); }
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            foreach (Perk perk in perks.Keys) { perk.ModifyMaxStats(ref health, ref mana); }
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            foreach (Perk perk in perks.Keys) { perk.ModifyCriticalStrikeChance(item, ref crit); }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            foreach (Perk perk in perks.Keys) { perk.ModifyItemScale(item, ref scale); }
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
        public Player player;
        public PerkPlayer perkPlayer;
        public bool CanBeStack = false;
        /// <summary>
        /// Use this if <see cref="CanBeStack"/> is true
        /// <br/> This allow easy multiply
        /// </summary>
        public int StackAmount = 0;
        public int StackLimit = 1;
        public string textureString = null;
        public string Tooltip = null;
        public int Type { get; internal set; }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode;
        }
        public static int GetPerkType<T>() where T : Perk
        {
            return ModContent.GetInstance<T>().Type;
        }
        protected sealed override void Register()
        {
            ModTypeLookup<Perk>.Register(this);

            ModPerkLoader.Register(this);
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
        public Perk(int whoAmI)
        {
            UseThisForConstructor(whoAmI);
            SetDefaults();
            if (CanBeStack)
                Tooltip += "\n( Can be stack ! )";
        }
        public void UseThisForConstructor(int whoAmI)
        {
            if (whoAmI == -1)
                player = Main.LocalPlayer;
            else
                player = Main.player[whoAmI];
            if (player.TryGetModPlayer(out PerkPlayer modplayer))
            {
                perkPlayer = modplayer;
            }
        }
        public virtual void SetDefaults()
        {

        }
        /// <summary>
        /// This will run in <see cref="ModPlayer.PostUpdate"/>
        /// </summary>
        public virtual void Update()
        {

        }
        public virtual void ResetEffect()
        {

        }
        public virtual void OnMissingMana(Item item, int neededMana)
        {

        }
        public virtual void ModifyDamage(Item item, ref StatModifier damage)
        {

        }
        public virtual void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public virtual void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public virtual void ModifyMaxStats(ref StatModifier health, ref StatModifier mana) { }
        public virtual void ModifyCriticalStrikeChance(Item item, ref float crit) { }
        public virtual void ModifyItemScale(Item item, ref float scale) { }
    }
    public static class ModPerkLoader
    {
        internal static Dictionary<int, Perk> perks = new();
        internal static int NextTypeID = 3;
        public static int Count => NextTypeID;
        internal static void RegisterFluid(Perk perk)
        {
            perks.Add(perk.Type, perk);
        }
        public static int PerkType<T>() where T : Perk => ModContent.GetInstance<T>()?.Type ?? -1;
        public static Perk GetPerk(int type) => perks[type];
        public static int TotalCount { get; private set; }
        internal static int Register(Perk value)
        {
            ModTypeLookup<Perk>.Register(value);
            return TotalCount++;
        }
    }
    class PerkChooser : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(32, 23);
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool? UseItem(Player player)
        {
            PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
            if (player.altFunctionUse != 2)
            {
                UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
                uiSystemInstance.perkUIstate.whoAmI = player.whoAmI;
                uiSystemInstance.userInterface.SetState(uiSystemInstance.perkUIstate);
            }
            else
            {
                modplayer.perks.Clear();
            }
            return base.UseItem(player);
        }
    }
}