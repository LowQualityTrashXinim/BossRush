using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.NohitReward
{
    internal class TrueEyeOfCthulhuTrophy : ModItem
    {
        public const int HP = 50;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"Overcoming a small challenge, tho sadly not place-able\"\nReward for not getting hit\nIncrease max HP by 50\nCan only be used once");
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeCrystal);
            Item.value = Item.sellPrice(platinum: 5, gold: 0, silver: 0, copper:0);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if(line.Name == "ItemName") line.OverrideColor = Main.DiscoColor;
            }
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<EoCNoHit>().EoC0hit < 1;
        }
        public override bool? UseItem(Player player)
        {
            player.statLifeMax2 += HP;
            player.statLife += HP;
            if (Main.myPlayer == player.whoAmI)
            {
                player.HealEffect(HP);
            }
            player.GetModPlayer<EoCNoHit>().EoC0hit++;
            return true;
        }
    }

    class EoCNoHit : ModPlayer
    {
        public int EoC0hit = 0;
        public override void ResetEffects()
        {
            Player.statLifeMax2 += EoC0hit * TrueEyeOfCthulhuTrophy.HP;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.EoCNoHit);
            packet.Write((byte)Player.whoAmI);
            packet.Write(EoC0hit);
            packet.Send(toWho, fromWho);
        }

        // NOTE: The tag instance provided here is always empty by default.
        // Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
        public override void SaveData(TagCompound tag)
        {
            tag["EoCnoHit"] = EoC0hit;
        }

        public override void LoadData(TagCompound tag)
        {
            EoC0hit = (int)tag["EoCnoHit"];
        }
    }
}
