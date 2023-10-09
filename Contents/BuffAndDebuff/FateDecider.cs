using BossRush.Contents.Artifact;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    internal class FateDeciderBuff : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Player player = Main.LocalPlayer;
            FateDeciderPlayer modplayer = player.GetModPlayer<FateDeciderPlayer>();
            tip = modplayer.GoodEffectString() + "\n" + modplayer.BadEffectString();
        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}