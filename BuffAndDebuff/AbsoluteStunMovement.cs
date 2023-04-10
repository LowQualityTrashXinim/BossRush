using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class AbsoluteStunMovement : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified");
            Description.SetDefault("The might of gods weakens your very soul...");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity = Microsoft.Xna.Framework.Vector2.Zero;
            player.controlUseItem = false;
            player.controlHook = false;
            player.controlQuickHeal = false;
            player.controlQuickMana = false;
            player.controlJump = false;
            player.controlLeft = false;
            player.controlRight = false;
            player.controlInv = false;
        }
    }
}