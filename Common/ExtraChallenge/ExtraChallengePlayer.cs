using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Common.ExtraChallenge
{
    public class ExtraChallengePlayer : ModPlayer
    {
        public int ChallengeChooser = 0;
        public int ClassChooser = 0;
        public int BossSlayedCount = 0;
        public bool OnlyUseOneClass = false;
        public bool Badbuff = false;
        public bool spawnRatex3 = false;
        public bool strongerEnemy = false;
        public bool BatJungleANDCave = false;
        public override void PostUpdate()
        {
            if (Badbuff)
            {
                Player.AddBuff(BuffID.Darkness, 10);
                Player.AddBuff(BuffID.Confused, 10);
                Player.AddBuff(BuffID.Weak, 10);
                Player.AddBuff(BuffID.ManaSickness, 10);
                Player.AddBuff(BuffID.BrokenArmor, 10);
                Player.AddBuff(BuffID.Bleeding, 10);
                Player.AddBuff(BuffID.WitheredArmor, 10);
                Player.AddBuff(BuffID.WitheredWeapon, 10);
                Player.AddBuff(BuffID.NoBuilding, 10);
            }
        }

        public override bool CanUseItem(Item item)
        {
            if (OnlyUseOneClass)
            {
                if (item.damage > 0)
                {
                    if (item.DamageType == DamageClass.Melee && ClassChooser == 0)
                    {
                        return true;
                    }
                    if (item.DamageType == DamageClass.Ranged && ClassChooser == 1)
                    {
                        return true;
                    }
                    if (item.DamageType == DamageClass.Magic && ClassChooser == 2)
                    {
                        return true;
                    }
                    if (item.DamageType == DamageClass.Summon && ClassChooser == 3)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.ExtraChallenge);
            packet.Write((byte)Player.whoAmI);
            packet.Write(ChallengeChooser);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["ExtraChallenge"] = ChallengeChooser;
        }

        public override void LoadData(TagCompound tag)
        {
            ChallengeChooser = (int)tag["ExtraChallenge"];
        }
    }
}
