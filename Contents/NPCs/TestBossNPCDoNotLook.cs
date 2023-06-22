using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.NPCs
{
    internal class TestBossNPCDoNotLook : ModNPC
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
    }
}
