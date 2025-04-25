using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;
using System.Linq;
using BossRush.Common.Systems.ObjectSystem;

namespace BossRush.Common.Systems.Mutation;
public abstract class ModMutation : ModType {
	public bool NewGamePlus = false;
	public virtual bool MutationCondition(NPC npc, Player player) {
		return false;
	}
	public int Type { get; private set; }
	protected sealed override void Register() {
		SetStaticDefaults();
		Type = ModMutationLoader.Register(this);
	}
	public static int GetMutationType<T>() where T : ModMutation {
		return ModContent.GetInstance<T>().Type;
	}
	public virtual void OnSpawn(NPC npc, IEntitySource source) { }
	public virtual void PostAI(NPC npc) { }
	public virtual void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) { }
	public virtual void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) { }
	public virtual void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) { }
	public virtual void OnKill(NPC npc) { }
}
public static class ModMutationLoader {
	private static readonly List<ModMutation> _mutation = new();
	public static int TotalCount => _mutation.Count;
	public static int Register(ModMutation enchant) {
		ModTypeLookup<ModMutation>.Register(enchant);
		_mutation.Add(enchant);
		return _mutation.Count - 1;
	}
	public static ModMutation GetMutation(int type) {
		return type >= 0 && type < _mutation.Count ? _mutation[type] : null;
	}
}
public class MutationSystem : ModSystem {
	public float MutationChance = 0;
	public bool NewGamePlusMutation = false;
	public override void PreUpdateNPCs() {
		MutationChance = 0;
	}
	public override void Load() {
		On_NPC.SetDefaults += On_NPC_SetDefaults;
		list_mutationType = new();
	}
	public override void Unload() {
		list_mutationType = null;
	}
	private static HashSet<int> list_mutationType = new();
	/// <summary>
	/// This method will store a mutation into a temporary list and then add them into the NPC<br/>
	/// Add this before calling <see cref="NPC.NewNPCDirect(IEntitySource, int, int, int, int, float, float, float, float, int)"/> or any alternative
	/// </summary>
	/// <param name="mutationType"></param>
	public static void AddMutation(int mutationType) {
		list_mutationType.Add(mutationType);
	}
	private void On_NPC_SetDefaults(On_NPC.orig_SetDefaults orig, NPC self, int Type, NPCSpawnParams spawnparams) {
		orig(self, Type, spawnparams);
		if (self.TryGetGlobalNPC(out NPCMutation global)) {
			HashSet<ModMutation> mut = list_mutationType.Select(i => ModMutationLoader.GetMutation(i)).ToHashSet();
			global.mutationList.AddRange(mut);
		}
		list_mutationType.Clear();
	}
}
