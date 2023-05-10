using System.Collections.Generic;
using BossRush.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Items.NohitReward
{
    internal class TrueEyeOfCthulhuTrophy : BaseNoHit
    {
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
        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<EoCNoHit>().EoC0hit < 1;
        }
    }

    class EoCNoHit : ModPlayer
    {
        public int EoC0hit = 0;
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;
            health.Base = EoC0hit * BaseNoHit.HP;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.EoCNoHit);
            packet.Write((byte)Player.whoAmI);
            packet.Write(EoC0hit);
            packet.Send(toWho, fromWho);
        }
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
