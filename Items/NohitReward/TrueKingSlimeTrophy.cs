using BossRush.Common;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Items.NohitReward
{
    internal class TrueKingSlimeTrophy : BaseNoHit
    {
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
        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<KingSlimeNoHit>().KS0hit < 1;
        }
    }

    class KingSlimeNoHit : ModPlayer
    {
        public int KS0hit = 0;
        public override void ResetEffects()
        {
            Player.statLifeMax2 += KS0hit * BaseNoHit.HP;
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
