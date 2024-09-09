using BossRush.Common.Systems.Mutation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.TrialSystem;
internal class TrialModSystem : ModSystem {
	private static readonly List<TrialData> _trial = new();
	public static int TotalCount => _trial.Count;
	public static int Register(TrialData enchant) {
		ModTypeLookup<TrialData>.Register(enchant);
		_trial.Add(enchant);
		return _trial.Count - 1;
	}
	public static TrialData GetEnchantment(int type) {
		return type >= 0 && type < _trial.Count ? _trial[type] : null;
	}
}
public abstract class TrialData : ModType {
	public int Type { get; private set; }
	protected sealed override void Register() {
		SetStaticDefaults();
		Type = TrialModSystem.Register(this);
	}
	public virtual int WaveAmount() => 0;
	public virtual List<int> NPCpool() => new();
	public virtual void Reward(Player player) { }
}
