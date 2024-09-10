using Terraria.ID;
using Terraria;

namespace BossRush.Contents.Items.Consumable.SpecialReward;
internal class KSDonHitReward : BaseDonHit {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.KingSlimePetItem);
}
internal class EoCDonHitReward : BaseDonHit {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.EyeOfCthulhuPetItem);
}
internal class EoWDonHitReward : BaseDonHit {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WormFood);
}
internal class BoCDonHitReward : BaseDonHit {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BrainOfCthulhuPetItem);
}
internal class SkeletronDonHitReward : BaseDonHit {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SkeletronPetItem);
}
internal class QueenBeeDonHitReward : BaseDonHit {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.QueenBeePetItem);
}
internal class DeerclopDonHitReward : BaseDonHit {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.DeerclopsPetItem);
}
internal class WallOfFleshDonHitReward : BaseDonHit {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WallOfFleshGoatMountItem);
}
