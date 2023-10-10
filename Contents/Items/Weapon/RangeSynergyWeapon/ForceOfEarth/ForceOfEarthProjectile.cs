using Terraria;
using Terraria.ID;
namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ForceOfEarth {
	internal class CopperBowP : BaseFOE {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperBow);
		public override float OffsetBehavior => 315f;
	}
	internal class TinBowP : BaseFOE {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinBow);
		public override float OffsetBehavior => 270f;
	}
	internal class IronBowP : BaseFOE {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.IronBow);
		public override float OffsetBehavior => 225f;
	}
	internal class LeadBowP : BaseFOE {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.LeadBow);
		public override float OffsetBehavior => 180f;
	}
	internal class SilverBowP : BaseFOE {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SilverBow);
		public override float OffsetBehavior => 135f;
	}
	internal class TungstenBowP : BaseFOE {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TungstenBow);
		public override float OffsetBehavior => 90f;
	}
	internal class GoldBowP : BaseFOE {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldBow);
		public override float OffsetBehavior => 45f;
	}
	internal class PlatinumBowP : BaseFOE {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumBow);
	}
}
