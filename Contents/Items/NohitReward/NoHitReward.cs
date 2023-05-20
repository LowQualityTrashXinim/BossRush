using Terraria;
using Terraria.ID;
using BossRush.Texture;

namespace BossRush.Contents.Items.NohitReward
{
    internal class KSNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.KingSlimePetItem);
        public override int Data => NPCID.KingSlime;
    }
    internal class EoCNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.EyeOfCthulhuPetItem);
        public override int Data => NPCID.EyeofCthulhu;
    }
    internal class EoWNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WormFood);
        public override int Data => NPCID.EaterofWorldsBody;
    }
    internal class BoCNoHitReward : BaseNoHit
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BrainOfCthulhuPetItem);
        public override int Data => NPCID.BrainofCthulhu;
    }
}
