using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace BossRush.Common.Systems.Mutation;
public abstract class ModMutation : ModType {
	public bool NewGamePlus = false;
	public bool MutationCondition(NPC npc, Player player) {
		return false;
	}
	public int Type { get; private set; }
	protected sealed override void Register() {
		SetStaticDefaults();
		Type = ModMutationLoader.Register(this);
	}
	public virtual void OnSpawn(NPC npc, IEntitySource source) { }
	public virtual void SetDefaults(NPC npc) { }
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
