using Terraria;
using Terraria.ID;
using Terraria.UI;
using System.Linq;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.NohitReward;

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
                List<int> listOfPerk = new List<int>();
                for (int i = 0; i < ModPerkLoader.TotalCount; i++)
                {
                    if (modplayer.perks.ContainsKey(i))
                    {
                        if ((!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0) ||
                            modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit)
                        {
                            continue;
                        }
                    }
                    listOfPerk.Add(i);
                }
                Texture2D texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT).Value;
                Vector2 origin = new Vector2(texture.Width * .5f, texture.Height * .5f);
                int amount = listOfPerk.Count;
                for (int i = 0; i < modplayer.PerkAmount; i++)
                {
                    if (i >= amount || i >= modplayer.PerkAmount - 1)
                    {
                        UIImageButton buttonWeapon = Main.rand.Next(new UIImageButton[]
                        { new MaterialPotionUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
                         new MaterialCardUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
                         new MaterialWeaponUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT))
                        });
                        buttonWeapon.Width.Pixels = texture.Width;
                        buttonWeapon.Height.Pixels = texture.Height;
                        Vector2 offsetPosWeapon = Vector2.UnitY.Vector2DistributeEvenly(modplayer.PerkAmount, 360, i) * modplayer.PerkAmount * 20;
                        Vector2 drawposWeapon = player.Center + offsetPosWeapon - Main.screenPosition - origin;
                        buttonWeapon.Left.Pixels = drawposWeapon.X;
                        buttonWeapon.Top.Pixels = drawposWeapon.Y;
                        Append(buttonWeapon);
                        continue;
                    }
                    int newperk = Main.rand.Next(listOfPerk);
                    // The above code will ensure that perk randomizer and perk chooser will never dupe and will never goes infinite
                    // Here we will randomize and validate perk
                    listOfPerk.Remove(newperk);
                    //After that we assign perk
                    PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT), modplayer);
                    btn.perkType = newperk;
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
        public int perkType;
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

            if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perkType))
                perkplayer.perks.Add(perkType, 1);
            else
                if (perkplayer.perks.ContainsKey(perkType) && ModPerkLoader.GetPerk(perkType).CanBeStack)
                perkplayer.perks[perkType] = perkplayer.perks[perkType] + 1;

            UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
            uiSystemInstance.userInterface.SetState(null);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(toolTip is null)
            {
                return;
            }
            if (IsMouseHovering)
            {
                toolTip.Left.Pixels = Main.MouseScreen.X - Left.Pixels;
                toolTip.Top.Pixels = Main.MouseScreen.Y - Top.Pixels - 20;
                toolTip.SetText(ModPerkLoader.GetPerk(perkType).Tooltip);
            }
            else
            {
                toolTip.SetText("");
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (ModPerkLoader.GetPerk(perkType).textureString == null)
            {
                return;
            }
            Texture2D WeaponTexture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(perkType).textureString).Value;
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
    public class PerkItem : GlobalItem
    {
        public override bool? UseItem(Item item, Player player)
        {
            PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
            if (player.ItemAnimationJustStarted)
                perkplayer.PotionExpert_perk_CanConsume = !Main.rand.NextBool(4);
            if (perkplayer.perk_PotionExpert && item.buffType > 0)
            {
                return perkplayer.PotionExpert_perk_CanConsume;
            }
            return base.UseItem(item, player);
        }
    }
    public class PerkPlayer : ModPlayer
    {
        public bool CanGetPerk = false;
        public int PerkAmount = 3;
        public Dictionary<int, int> perks = new Dictionary<int, int>();

        public bool perk_PotionExpert = false;
        public bool PotionExpert_perk_CanConsume = false;

        private int[] _perks;
        public override void Initialize()
        {
            _perks = new int[ModPerkLoader.TotalCount];
            perks = new Dictionary<int, int>();
        }
        public bool HasPerk<T>() where T : Perk => _perks[Perk.GetPerkType<T>()] > 0;
        public bool HasPerk(Perk perk) => _perks[perk.Type] > 0;
        public override void ResetEffects()
        {
            perk_PotionExpert = false;
            PerkAmount = Player.GetModPlayer<NoHitPlayerHandle>().BossNoHitNumber.Count + 3;
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).ResetEffect(Player);
                ModPerkLoader.GetPerk(perk).StackAmount = perks[perk];
            }
        }
        public override bool CanUseItem(Item item)
        {
            UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
            if (uiSystemInstance.userInterface.CurrentState is not null)
            {
                return false;
            }
            return base.CanUseItem(item);
        }
        public override void PostUpdate()
        {
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).Update(Player);
            }
        }
        public override void OnMissingMana(Item item, int neededMana)
        {
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).OnMissingMana(Player, item, neededMana);
            }
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).ModifyMaxStats(Player, ref health, ref mana);
            }
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).ModifyCriticalStrikeChance(Player, item, ref crit);
            }
        }
        public override void ModifyItemScale(Item item, ref float scale)
        {
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).ModifyItemScale(Player, item, ref scale);
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).ModifyDamage(Player, item, ref damage);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).OnHitNPCWithItem(Player, item, target, hit, damageDone);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (int perk in perks.Keys)
            {
                ModPerkLoader.GetPerk(perk).OnHitNPCWithProj(Player, proj, target, hit, damageDone);
            }
        }
        //TODO : make a override for item
        public override void SaveData(TagCompound tag)
        {
            tag["PlayerPerks"] = perks.Keys.ToList();
            tag["PlayerPerkStack"] = perks.Values.ToList();
        }
        public override void LoadData(TagCompound tag)
        {
            var PlayerPerks = tag.Get<List<int>>("PlayerPerks");
            var PlayerPerkStack = tag.Get<List<int>>("PlayerPerkStack");
            perks = PlayerPerks.Zip(PlayerPerkStack, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
        }
    }
    
    public abstract class Perk : ModType
    {
        public bool CanBeStack = false;
        /// <summary>
        /// Use this if <see cref="CanBeStack"/> is true
        /// <br/> This allow easy multiply
        /// </summary>
        public int StackAmount = 0;
        public int StackLimit = 1;
        public string textureString = null;
        public string Tooltip = null;
        public int Type { get; private set; }
        protected sealed override void Register()
        {
            Type = ModPerkLoader.Register(this);
        }
        public static int GetPerkType<T>() where T : Perk
        {
            return ModContent.GetInstance<T>().Type;
        }
        public sealed override void Unload()
        {
            base.Unload();
            textureString = null;
            Tooltip = null;
        }
        public Perk()
        {
            SetDefaults();
            if (CanBeStack)
                Tooltip += "\n( Can be stack ! )";
        }
        public virtual void SetDefaults()
        {

        }
        /// <summary>
        /// This will run in <see cref="ModPlayer.PostUpdate"/>
        /// </summary>
        public virtual void Update(Player player)
        {

        }
        public virtual void ResetEffect(Player player)
        {

        }
        public virtual void OnMissingMana(Player player, Item item, int neededMana)
        {

        }
        public virtual void ModifyDamage(Player player, Item item, ref StatModifier damage)
        {

        }
        public virtual void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public virtual void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) { }
        public virtual void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) { }
        public virtual void ModifyItemScale(Player player, Item item, ref float scale) { }
    }
    public static class ModPerkLoader
    {
        private static readonly List<Perk> _perks = new();
        public static int TotalCount => _perks.Count;
        public static int Register(Perk perk)
        {
            ModTypeLookup<Perk>.Register(perk);
            _perks.Add(perk);
            return _perks.Count - 1;
        }
        public static Perk GetPerk(int type)
        {
            return type >= 0 && type < _perks.Count ? _perks[type] : null;
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
            return true;
        }
    }
}