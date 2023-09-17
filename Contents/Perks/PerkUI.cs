﻿using Terraria;
using Terraria.UI;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Perks
{
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
                int amount = listOfPerk.Count;
                Vector2 originDefault = new Vector2(26, 26);
                for (int i = 0; i < modplayer.PerkAmount; i++)
                {
                    Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(modplayer.PerkAmount, 360, i) * modplayer.PerkAmount * 20;
                    if (i >= amount || i >= modplayer.PerkAmount - 1)
                    {
                        UIImageButton buttonWeapon = Main.rand.Next(new UIImageButton[]
                        { new MaterialPotionUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
                         new MaterialCardUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
                         new MaterialWeaponUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT))
                        });
                        buttonWeapon.UISetWidthHeight(52, 52);
                        buttonWeapon.UISetPosition(player.Center + offsetPos, originDefault);
                        Append(buttonWeapon);
                        continue;
                    }
                    int newperk = Main.rand.Next(listOfPerk);
                    Asset<Texture2D> texture;
                    if (ModPerkLoader.GetPerk(newperk).textureString is not null)
                        texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
                    else
                        texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
                    listOfPerk.Remove(newperk);
                    //After that we assign perk
                    PerkUIImageButton btn = new PerkUIImageButton(texture, modplayer);
                    btn.UISetWidthHeight(52, 52);
                    btn.UISetPosition(player.Center + offsetPos, originDefault);
                    btn.perkType = newperk;
                    Append(btn);
                }
            }
        }
    }
    //Do all the check in UI state since that is where the perk actually get create and choose


    class PerkUIImageButton : UIImageButton
    {
        PerkPlayer perkplayer;
        public int perkType;
        private UIText toolTip;
        public PerkUIImageButton(Asset<Texture2D> texture, PerkPlayer perkPlayer) : base(texture)
        {
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
            if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perkType))
                perkplayer.perks.Add(perkType, 1);
            else
                if (perkplayer.perks.ContainsKey(perkType) && ModPerkLoader.GetPerk(perkType).CanBeStack)
                perkplayer.perks[perkType] = perkplayer.perks[perkType] + 1;
            ModPerkLoader.GetPerk(perkType).OnChoose(perkplayer.Player);
            UISystem uiSystemInstance = ModContent.GetInstance<UISystem>();
            uiSystemInstance.userInterface.SetState(null);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (toolTip is null)
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
}
