using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace BossRush.Contents.Items.aDebugItem
{
    internal class BiomeToggle : ModItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WorldGlobe);
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                tooltips.Add(new TooltipLine(Mod, "netcodeshit", "This do not work in multiplayer"));
                return;
            }
            tooltips.Add(new TooltipLine(Mod, "currentZone", $"You are in {Main.LocalPlayer.GetModPlayer<BiomeTogglePlayer>().BiomeText}"));
        }
        public override bool? UseItem(Player player)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                BiomeTogglePlayer modplayer = player.GetModPlayer<BiomeTogglePlayer>();
                modplayer.Zoneindex++;
                return true;
            }
            return true;
        }
    }
    public class BiomeTogglePlayer : ModPlayer
    {
        public int Zoneindex = 0;
        public string BiomeText = "";
        public override void PostUpdate()
        {
            base.PostUpdate();
            if (!Player.HasItem(ModContent.ItemType<BiomeToggle>()))
            {
                return;
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                return;
            }
            switch (Zoneindex)
            {
                case 0:
                    Player.ZoneOverworldHeight = true;
                    BiomeText = nameof(Player.ZoneOverworldHeight);
                    break;
                case 1:
                    Player.ZoneCorrupt = true;
                    BiomeText = nameof(Player.ZoneCorrupt);
                    break;
                case 2:
                    Player.ZoneCrimson = true;
                    BiomeText = nameof(Player.ZoneCrimson);
                    break;
                case 3:
                    Player.ZoneSnow = true;
                    BiomeText = nameof(Player.ZoneSnow);
                    break;
                case 4:
                    Player.ZoneJungle = true;
                    BiomeText = nameof(Player.ZoneJungle);
                    break;
                case 5:
                    Player.ZoneRockLayerHeight = true;
                    BiomeText = nameof(Player.ZoneRockLayerHeight);
                    break;
                case 6:
                    Player.ZoneSnow = true;
                    Player.ZoneRockLayerHeight = true;
                    BiomeText = $"{nameof(Player.ZoneSnow)} and {nameof(Player.ZoneRockLayerHeight)}";
                    break;
                case 7:
                    Player.ZoneJungle = true;
                    Player.ZoneRockLayerHeight = true;
                    BiomeText = $"{nameof(Player.ZoneJungle)} and {nameof(Player.ZoneRockLayerHeight)}";
                    break;
                default:
                    Zoneindex = 0;
                    break;
            }
        }
    }
}
