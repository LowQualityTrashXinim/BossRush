using BossRush.Common;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Items.NohitReward
{
    internal class TrueBrainOfCthulhuTrophy : BaseNoHit
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override bool? UseItem(Player player)
        {
            player.statLifeMax2 += HP;
            player.statLife += HP;
            if (Main.myPlayer == player.whoAmI)
            {
                player.HealEffect(HP);
            }
            player.GetModPlayer<BoCNoHit>().BoC0Hit++;
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<BoCNoHit>().BoC0Hit < 1;
        }
    }

    class BoCNoHit : ModPlayer
    {
        public int BoC0Hit = 0;
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;
            health.Base = BoC0Hit * BaseNoHit.HP;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.BoCNoHit);
            packet.Write((byte)Player.whoAmI);
            packet.Write(BoC0Hit);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["BoC0Hit"] = BoC0Hit;
        }

        public override void LoadData(TagCompound tag)
        {
            BoC0Hit = (int)tag["BoC0Hit"];
        }
    }
}
