using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.ArgumentsSystem;
internal class ArgumentLoader : ModSystem {
	private static readonly List<ModArgument> _arguments = new();
	public static int TotalCount => _arguments.Count;
	public static int Register(ModArgument enchant) {
		ModTypeLookup<ModArgument>.Register(enchant);
		_arguments.Add(enchant);
		return _arguments.Count - 1;
	}
	public static ModArgument GetArgument(int type) {
		return type >= 0 && type < _arguments.Count ? _arguments[type] : null;
	}
}
public class ArgumentWeapon : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.IsAWeapon();
	}
	public override bool InstancePerEntity => true;
}
public class ModArgument : ModType {
	protected override void Register() {
		ArgumentLoader.Register(this);
	}
}
