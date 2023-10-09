using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class LeadIrradiation : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 50;
        }
    }
}