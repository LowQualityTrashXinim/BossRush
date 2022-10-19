using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.NohitReward
{
    internal class TrueKingSlimeTrophy : ModItem
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
            return player.GetModPlayer<KingSlimeNoHit>().KS0hit < 1;
        }
        public override bool? UseItem(Player player)
        {
            player.statLifeMax2 += HP;
            player.statLife += HP;
            if (Main.myPlayer == player.whoAmI)
            {
                player.HealEffect(HP);
            }
            player.GetModPlayer<KingSlimeNoHit>().KS0hit++;
            return true;
        }
    }

    class KingSlimeNoHit : ModPlayer
    {
        public int KS0hit = 0;
        public override void ResetEffects()
        {
            Player.statLifeMax2 += KS0hit * TrueKingSlimeTrophy.HP;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.KingSlimeNoHit);
            packet.Write((byte)Player.whoAmI);
            packet.Write(KS0hit);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["KSnoHit"] = KS0hit;
        }

        public override void LoadData(TagCompound tag)
        {
            KS0hit = (int)tag["KSnoHit"];
        }
    }
}
