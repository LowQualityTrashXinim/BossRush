using Terraria;
using Terraria.ModLoader;

namespace BossRush.BuffAndDebuff
{
    internal class AbsoluteStunMovement : ModBuff
    {
        public override string Texture => "BossRush/EmptyBuff";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Absolute Stun");
            Description.SetDefault("Presence of world eating worm make you feel weak");
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