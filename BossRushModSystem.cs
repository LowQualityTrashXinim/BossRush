using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
//Require Nuclear level of organizing
using BossRush.Items.Weapon.MeleeSynergyWeapon.SuperShortSword;
using BossRush.Items.Weapon.RangeSynergyWeapon.SnowballShotgunCannon;
using BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow;
using BossRush.Items.Weapon.MeleeSynergyWeapon.ShadowTrick;
using BossRush.Items.Weapon.MeleeSynergyWeapon.EverlastingCold;
using BossRush.Items.Weapon.MeleeSynergyWeapon.FlameingWoodSword;
using BossRush.Items.Weapon.MeleeSynergyWeapon.YinYang;
using BossRush.Items.Weapon.MeleeSynergyWeapon.BurningPassion;
using BossRush.Items.Weapon.MeleeSynergyWeapon.Katana;
using BossRush.Items.Weapon.SummonerSynergyWeapon.StickySlime;
using BossRush.Items.Weapon.RangeSynergyWeapon.Bowmarang;
using BossRush.Items.Weapon.RangeSynergyWeapon.OvergrownMinishark;
using BossRush.Items.Weapon.RangeSynergyWeapon.SkullRevolver;
using BossRush.Items.Weapon.RangeSynergyWeapon.KnifeRevolver;
using BossRush.Items.Weapon.MeleeSynergyWeapon.SuperEnchantedSword;
using BossRush.Items.Weapon.RangeSynergyWeapon.BloodyShot;
using BossRush.Items.Weapon.RangeSynergyWeapon.RectangleShotgun;
using BossRush.Items.Weapon.RangeSynergyWeapon.ChaosMiniShark;
using BossRush.Items.Weapon.RangeSynergyWeapon.RifleShotgun;
using BossRush.Items.Weapon.RangeSynergyWeapon.HeartPistol;
using BossRush.Items.Weapon.MeleeSynergyWeapon.ManaStarfury;
using BossRush.Items.Weapon.RangeSynergyWeapon.Merciless;
using BossRush.Items.Weapon.MeleeSynergyWeapon.BloodStar;
using BossRush.Items.Weapon.MeleeSynergyWeapon.DarkCactus;
using BossRush.Items.Weapon.MeleeSynergyWeapon.EnchantedOreSword;
using BossRush.Items.Weapon.RangeSynergyWeapon.DeathBySpark;
using BossRush.Items.Weapon.RangeSynergyWeapon.NatureSelection;
using BossRush.Items.Weapon.RangeSynergyWeapon.SnowballRifle;
using BossRush.Items.Accessories;
using BossRush.Items.Weapon.RangeSynergyWeapon.PaintRifle;
using BossRush.Items.Weapon.RangeSynergyWeapon.ForceOfEarth;
using BossRush.Items.Weapon.RangeSynergyWeapon.QuadDemonBlaster;
using BossRush.Items.Weapon.MeleeSynergyWeapon.FrozenSwordFish;
using BossRush.Items.Weapon.MagicSynergyWeapon;

namespace BossRush
{
    internal class BossRushModSystem : ModSystem
    {
        public override void AddRecipeGroups()
        {// => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.MagicMirror)}", ItemID.IceMirror, ItemID.MagicMirror);
            RecipeGroup AnySynergyWeapon = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} { "Synergy Weapon" }", new int[]
                {
                //Melee
                ModContent.ItemType<EnchantedLongSword>(),
                ModContent.ItemType<EnchantedStarfury>(),
                ModContent.ItemType<FrozenEnchantedSword>(),
                ModContent.ItemType<TrueEnchantedSword>(),
                ModContent.ItemType<DarkCactus>(),
                ModContent.ItemType<SuperShortSword>(),
                ModContent.ItemType<EverlastingCold>(),
                ModContent.ItemType<ManaStarFury>(),
                ModContent.ItemType<YinYang>(),
                ModContent.ItemType<EnchantedSilverSword>(),
                ModContent.ItemType<PlatinumKatana>(),
                ModContent.ItemType<BloodStar>(),
                ModContent.ItemType<BurningPassion>(),
                ModContent.ItemType<FlamingWoodSword>(),
                ModContent.ItemType<FrostSwordFish>(),
                ModContent.ItemType<ShadowTrick>(),
                //Range
                ModContent.ItemType<ChaosMiniShark>(),
                ModContent.ItemType<DeathBySpark>(),
                ModContent.ItemType<BloodyShot>(),
                ModContent.ItemType<QuadDemonBlaster>(),
                ModContent.ItemType<HeartPistol>(),
                ModContent.ItemType<RectangleShotgun>(),
                ModContent.ItemType<RifleShotgun>(),
                ModContent.ItemType<SkullRevolver>(),
                ModContent.ItemType<SnowballRifle>(),
                ModContent.ItemType<SnowballShotgunCannon>(),
                ModContent.ItemType<NatureSelection>(),
                ModContent.ItemType<ForceOfEarth>(),
                ModContent.ItemType<AmethystBow>(),
                ModContent.ItemType<DiamondBow>(),
                ModContent.ItemType<EmeraldBow>(),
                ModContent.ItemType<RubyBow>(),
                ModContent.ItemType<TopazBow>(),
                ModContent.ItemType<SapphireBow>(),
                ModContent.ItemType<Merciless>(),
                ModContent.ItemType<KnifeRevolver>(),
                ModContent.ItemType<OvergrownMinishark>(),
                ModContent.ItemType<Bowmarang>(),
                ModContent.ItemType<PaintRifle>(),
                //Magic
                ModContent.ItemType<StarLightDistributer>(),
                ModContent.ItemType<AmethystSwotaff>(),
                ModContent.ItemType<EmeraldSwotaff>(),
                ModContent.ItemType<SapphireSwotaff>(),
                ModContent.ItemType<TopazSwotaff>(),
                ModContent.ItemType<RubySwotaff>(),
                ModContent.ItemType<DiamondSwotaff>(),
                ModContent.ItemType<ZapRifle>(),
                //Summon
                ModContent.ItemType<StickyFlower>(),
                //Accessory
                ModContent.ItemType<GuideToMasterNinja>()
            });
            RecipeGroup.RegisterGroup("Synergy Item", AnySynergyWeapon);

            RecipeGroup WoodSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} { Lang.GetItemNameValue(ItemID.WoodenSword) }", new int[]
            {
                ItemID.WoodenSword,
                ItemID.BorealWoodSword,
                ItemID.RichMahoganySword,
                ItemID.ShadewoodSword,
                ItemID.EbonwoodSword,
                ItemID.PalmWoodSword,
                ItemID.PearlwoodSword,
            });
            RecipeGroup.RegisterGroup("Wood Sword", WoodSword);

            RecipeGroup WoodBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} { Lang.GetItemNameValue(ItemID.WoodenBow) }", new int[]
            {
                ItemID.WoodenBow,
                ItemID.BorealWoodBow,
                ItemID.RichMahoganyBow,
                ItemID.ShadewoodBow,
                ItemID.EbonwoodBow,
                ItemID.PalmWoodBow,
                ItemID.PearlwoodBow,
            });
            RecipeGroup.RegisterGroup("Wood Bow", WoodBow);

            RecipeGroup OreShortSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} { Lang.GetItemNameValue(ItemID.CopperShortsword) }", new int[]
            {
                ItemID.CopperShortsword,
                ItemID.TinShortsword,
                ItemID.IronShortsword,
                ItemID.LeadShortsword,
                ItemID.SilverShortsword,
                ItemID.TungstenShortsword,
                ItemID.GoldShortsword,
                ItemID.PlatinumShortsword,
            });
            RecipeGroup.RegisterGroup("OreShortSword", OreShortSword);

            RecipeGroup OreBroadSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} { Lang.GetItemNameValue(ItemID.CopperBroadsword) }", new int[]
            {
                ItemID.CopperBroadsword,
                ItemID.TinBroadsword,
                ItemID.IronBroadsword,
                ItemID.LeadBroadsword,
                ItemID.SilverBroadsword,
                ItemID.TungstenBroadsword,
                ItemID.GoldBroadsword,
                ItemID.PlatinumBroadsword,
            });
            RecipeGroup.RegisterGroup("OreBroadSword", OreBroadSword);
        }
    }
}
