using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Items.NohitReward
{
    internal class TrueEaterOfWorldTrophy : BaseNoHit
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
            player.GetModPlayer<EoWNoHit>().EoW0Hit++;
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<EoWNoHit>().EoW0Hit < 1;
        }
    }

    class EoWNoHit : ModPlayer
    {
        public int EoW0Hit = 0;
        public override void ResetEffects()
        {
            Player.statLifeMax2 += EoW0Hit * BaseNoHit.HP;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.EoWNoHit);
            packet.Write((byte)Player.whoAmI);
            packet.Write(EoW0Hit);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["EoW0Hit"] = EoW0Hit;
        }

        public override void LoadData(TagCompound tag)
        {
            EoW0Hit = (int)tag["EoW0Hit"];
        }
    }
}
