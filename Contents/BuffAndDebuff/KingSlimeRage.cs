using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.BuffAndDebuff
{
    public class KingSlimeRage : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("KingSlimeRage");
            // Description.SetDefault("You can't run away from his excellency");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.65f;
            player.maxRunSpeed = 0.65f;
            player.runAcceleration *= 0.65f;
            player.jumpSpeedBoost *= 0.65f;
            player.accRunSpeed *= 0.65f;
        }
    }
}