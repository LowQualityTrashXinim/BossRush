using BossRush.Common.RoguelikeChange.NPCsOverhaul;
using BossRush.Common.Systems.ObjectSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Systems;

public class AIState : ILoadable
{
	public static AIState NewAIState(NPC npc,int type)
	{
		AIState state = (AIState)AIStates[type].MemberwiseClone();
		state.npc = npc;
		return state;
	}
	public static int StateType<T>() where T : AIState => ModContent.GetInstance<T>().type;
	public static readonly List<AIState> AIStates = new List<AIState>();
	public int counter = 0;
	public Action<AIState> changeState;
	public Player Target 
	{
		get
		{
			npc.TargetClosest();
			if(npc.HasValidTarget)
				return Main.player[npc.target];
			return null;
		}

	}
	public void ChangeState(int state)
	{
		npc.aiAction = state;
		changeState.Invoke(this);
	}
	public NPC npc = null;
	public virtual void OnEntered(int oldState) { }
    public virtual void OnStateUpdate(CommonNPCInfo info) { }
    public virtual void OnExited(int newState) { }
	public int type {  get; private set; }
	public void Load(Mod mod) {
		type = AIStates.Count;
		AIStates.Add(this);
	}
	public void Unload() {
	}

	public bool ChangeStateIfTargetNull(int state)
	{
		
		if(Target == null)
		{
			ChangeState(state);
			return true;
		}
			
		return false;
	}
}

public class NPCAIStates
{ 
	public NPCAIStates(NPC npc, params int[] states)
	{
		this.npc = npc;
		foreach (int type in states) 
		{
			this.states[type] = AIState.NewAIState(npc,type);
			this.states[type].changeState += OnStateChange;
		}
		currentState = this.states[states[0]];
	}
	private Dictionary<int,AIState> states = [];
	public bool justHitTheGround = false;
	public bool inGround = false;
	public bool justMovedAwayFromTheGround = false;
	public bool afterimages = false;
	public void Update()
	{	
		
		if (justHitTheGround) 
		{
			justHitTheGround = false;

		}

		if (npc.collideY && !inGround) 
		{
		
			justHitTheGround = true;
			inGround = true;
		}

		if(justMovedAwayFromTheGround) 
		{
			justMovedAwayFromTheGround = false;
		}

		if(!npc.collideY && inGround) 
		{
			inGround = false;
			justMovedAwayFromTheGround = true;
		}

		CommonNPCInfo info = new CommonNPCInfo();
		info.justMovedAwayFromTheGround = justMovedAwayFromTheGround;
		info.inGround = inGround;
		info.justHitTheGround = justHitTheGround;
		currentState.OnStateUpdate(info);
		currentState.counter++;
	}
	public void OnStateChange(AIState oldState)
	{
		oldState.counter = 0;
		oldState.OnExited(npc.aiAction);
		AIState newState = states[npc.aiAction];
		currentState = newState;
		currentState.OnEntered(oldState.type);
	}

	public AIState currentState = null;
	public NPC npc = null;

}

public struct CommonNPCInfo
{
	public bool justHitTheGround;
	public bool inGround;
	public bool justMovedAwayFromTheGround;
}

public class Boss_Despawn : AIState 
{

	public override void OnStateUpdate(CommonNPCInfo info) {
		npc.position.Y += 30;
		npc.velocity = Vector2.Zero;
		npc.EncourageDespawn(30);
	}

}
