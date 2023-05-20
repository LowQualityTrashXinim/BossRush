using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.NohitReward
{
    internal class KSNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.KingSlimePetItem);
    }
    internal class EoCNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.EyeOfCthulhuPetItem);
    }
    internal class EoWNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WormFood);
    }
    internal class BoCNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BrainOfCthulhuPetItem);
    }
    internal class SkeletronNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SkeletronPrimePetItem);
    }
    internal class QueenBeeNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.QueenBeePetItem);
    }
    internal class DeerclopNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.DeerclopsPetItem);
    }
    internal class WallOfFleshNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WallOfFleshGoatMountItem);
    }
}