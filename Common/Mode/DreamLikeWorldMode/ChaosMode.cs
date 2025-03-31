using System;
using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Common.Mode.DreamLikeWorldMode;
internal class ChaosModeSystem : ModSystem {
	public static bool Chaos() => ModContent.GetInstance<ChaosModeSystem>().ChaosMode;
	public bool ChaosMode = false;
	public byte BannedItemIDCount = 0;
	public byte ChainedBuffCount = 0;
	public byte ChaosWeaponCount = 0;
	public HashSet<int> List_Ban_ItemID = new();
	public Dictionary<int, int> Dict_Chained_Buff = new();
	public Dictionary<int, ChaosItemInfo> Dict_Chaos_Weapon = new();
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (!UniversalSystem.CanAccessContent(UniversalSystem.CHAOS_MODE)) {
			return;
		}
		ChaosMode = true;
		List_Ban_ItemID.Clear();
		Dict_Chained_Buff.Clear();
		Dict_Chaos_Weapon.Clear();
		BannedItemIDCount = (byte)Main.rand.Next(40, 81);
		ChainedBuffCount = (byte)Main.rand.Next(5, 15);
		ChaosWeaponCount = (byte)Main.rand.Next(30, 121);
		int repurposeID = -1;
		while (List_Ban_ItemID.Count < BannedItemIDCount) {
			repurposeID = Main.rand.NextFromHashSet(BossRushModSystem.List_Weapon).type;
			if (List_Ban_ItemID.Contains(repurposeID)) {
				continue;
			}
			List_Ban_ItemID.Add(repurposeID);
		}
		int secondBuffToChain = 0;
		while (Dict_Chained_Buff.Keys.Count < ChainedBuffCount) {
			repurposeID = Main.rand.Next(BuffID.Count);
			secondBuffToChain = Main.rand.Next(BuffID.Count);
			if (repurposeID == secondBuffToChain) {
				continue;
			}
			if (!Dict_Chained_Buff.ContainsKey(repurposeID)) {
				Dict_Chained_Buff.Add(repurposeID, secondBuffToChain);
			}
		}
		ChaosItemInfo info = new ChaosItemInfo();
		DamageClass[] randomclass = new[] { DamageClass.Melee, DamageClass.Magic, DamageClass.Ranged, DamageClass.Summon, DamageClass.Generic, DamageClass.Default };
		HashSet<Item> listItemIDPossible = BossRushModSystem.List_Weapon.Where(i => !List_Ban_ItemID.Contains(i.type)).ToHashSet();
		while (Dict_Chaos_Weapon.Keys.Count < ChaosWeaponCount) {
			info.SetInfo(Main.rand.NextFromHashSet(listItemIDPossible));
			if (Dict_Chaos_Weapon.ContainsKey(info.assignedItemID)) {
				continue;
			}
			info.Damage += Main.rand.Next(-info.Damage + 1, info.Damage + 1);
			info.KnockBack += MathF.Round(Main.rand.NextFloat(-info.KnockBack + 1, info.KnockBack + 1), 2);
			info.useTime += Main.rand.Next(-info.useTime + 1, info.useTime + 1);
			info.useAnimation += Main.rand.Next(-info.useAnimation + 1, info.useAnimation + 1);
			if (Main.rand.NextBool()) {
				info.shoot = Main.rand.Next(ProjectileLoader.ProjectileCount);
			}
			info.shootSpeed += info.shootSpeed == 0 ? Main.rand.NextFloat(0, 50) : Main.rand.NextFloat(-info.shootSpeed + 1, info.shootSpeed);
			info.scale += MathF.Round(Main.rand.NextFloat(-info.scale + .1f, info.scale), 2);
			info.crit += Main.rand.Next(-100, 100);
			info.DamageType = Main.rand.Next(randomclass);
			Dict_Chaos_Weapon.Add(info.assignedItemID, info);
		}
	}
	public override void SaveWorldData(TagCompound tag) {
		tag.Add("ChaosMode", ChaosMode);
		tag.Add("BannedItemIDCount", BannedItemIDCount);
		tag.Add("ChainedBuffCount", ChainedBuffCount);
		tag.Add("ChaosWeaponCount", ChaosWeaponCount);
		tag.Add("List_Ban_ItemID", List_Ban_ItemID.ToList());
		tag.Add("Dict_Chained_Buff_Key", Dict_Chained_Buff.Keys.ToList());
		tag.Add("Dict_Chained_Buff_Value", Dict_Chained_Buff.Values.ToList());
		tag.Add("Dict_Chaos_Weapon_Key", Dict_Chaos_Weapon.Keys.ToList());
		tag.Add("Dict_Chaos_Weapon_Value", Dict_Chaos_Weapon.Values.ToList());
	}
	public override void LoadWorldData(TagCompound tag) {
		if (tag.TryGet("ChaosMode", out bool ChaosMode)) {
			this.ChaosMode = ChaosMode;
		}
		if (tag.TryGet("BannedItemIDCount", out byte BannedItemIDCount)) {
			this.BannedItemIDCount = BannedItemIDCount;
		}
		if (tag.TryGet("ChainedBuffCount", out byte ChainedBuffCount)) {
			this.ChainedBuffCount = ChainedBuffCount;
		}
		if (tag.TryGet("ChaosWeaponCount", out byte ChaosWeaponCount)) {
			this.ChaosWeaponCount = ChaosWeaponCount;
		}
		if (tag.TryGet("List_Ban_ItemID", out List<int> List_Ban_ItemID)) {
			this.List_Ban_ItemID = List_Ban_ItemID.ToHashSet();
		}
		var Dict_Chained_Buff_Key = tag.Get<List<int>>("Dict_Chained_Buff_Key");
		var Dict_Chained_Buff_Value = tag.Get<List<int>>("Dict_Chained_Buff_Value");
		Dict_Chained_Buff = Dict_Chained_Buff_Key.Zip(Dict_Chained_Buff_Value, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);

		var Dict_Chaos_Weapon_Key = tag.Get<List<int>>("Dict_Chaos_Weapon_Key");
		var Dict_Chaos_Weapon_Value = tag.Get<List<ChaosItemInfo>>("Dict_Chaos_Weapon_Value");
		Dict_Chaos_Weapon = Dict_Chaos_Weapon_Key.Zip(Dict_Chaos_Weapon_Value, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
	}
}
public struct ChaosItemInfo {
	public int assignedItemID;
	public int Damage;
	public int useTime;
	public int useAnimation;
	public float KnockBack;
	public float scale;
	public int crit;
	public int shoot;
	public float shootSpeed;
	public DamageClass DamageType;
	public ChaosItemInfo(Item item) {
		assignedItemID = item.type;
		Damage = item.damage;
		useTime = item.useTime;
		useAnimation = item.useAnimation;
		KnockBack = item.knockBack;
		scale = item.scale;
		crit = item.crit;
		shoot = item.shoot;
		shootSpeed = item.shootSpeed;
		DamageType = item.DamageType;
	}
	public void SetInfo(Item item) {
		assignedItemID = item.type;
		Damage = item.damage;
		useTime = item.useTime;
		useAnimation = item.useAnimation;
		KnockBack = item.knockBack;
		scale = item.scale;
		crit = item.crit;
		shoot = item.shoot;
		shootSpeed = item.shootSpeed;
		DamageType = item.DamageType;
	}
	public void ApplyInfo(ref Item item) {
		item.damage = Damage;
		item.useTime = useTime;
		item.useAnimation = useAnimation;
		item.knockBack = KnockBack;
		item.scale = scale;
		item.crit = crit;
		item.shoot = shoot;
		item.shootSpeed = shootSpeed;
		item.DamageType = DamageType;
	}
}
class ChaosItemInfoSerializable : TagSerializer<ChaosItemInfo, TagCompound> {
	public override TagCompound Serialize(ChaosItemInfo value) => new TagCompound {
		["assignedItemID"] = value.assignedItemID,
		["Damage"] = value.Damage,
		["useTime"] = value.useTime,
		["useAnimation"] = value.useAnimation,
		["KnockBack"] = MathF.Round(value.KnockBack, 2),
		["scale"] = MathF.Round(value.scale, 2),
		["crit"] = value.crit,
		["shoot"] = value.shoot,
		["shootSpeed"] = MathF.Round(value.shootSpeed, 2),
		["DamageType"] = value.DamageType.Type,
	};


	public override ChaosItemInfo Deserialize(TagCompound tag) {
		var myData = new ChaosItemInfo();
		myData.assignedItemID = tag.GetInt("assignedItemID");
		myData.Damage = tag.GetInt("Damage");
		myData.useTime = tag.GetInt("useTime");
		myData.useAnimation = tag.GetInt("useAnimation");
		myData.KnockBack = tag.GetFloat("KnockBack");
		myData.scale = tag.GetFloat("scale");
		myData.crit = tag.GetInt("crit");
		myData.shoot = tag.GetInt("shoot");
		myData.shootSpeed = tag.GetFloat("shootSpeed");
		myData.DamageType = DamageClassLoader.GetDamageClass(tag.GetInt("DamageType"));
		return myData;
	}
}
