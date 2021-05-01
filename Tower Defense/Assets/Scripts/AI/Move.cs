using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// FSM state Move. It will request paths to the pathfinding system.
// </summary>
public class Move : State
{

	public Move(Base_AI _npc, Animator _anim, BuildingRange _target) : base(_npc, _anim, _target)
	{
		Name = STATE.MOVE;
	}

	public override void Enter()
	{
		Debug.Log(npc.isFeared);
		npc.pathReached = false;
		anim.SetTrigger("moving");
		base.Enter();

		PathRequestManager.RequestPath(npc.transform.position, target, npc.Range, npc.OnPathFound);
	}

	public override void Update()
	{
		if (npc.pathReached)
        {
			nextState = new Attack(npc, anim, target);
			stage = EVENT.EXIT;
		}
		if (npc.isStunned)
		{
			nextState = new Stun(npc, anim, npc.Goal);
			stage = EVENT.EXIT;
		}
		if (npc.isFeared)
		{
			nextState = new Fear(npc, anim, npc.Goal);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("moving");
		base.Exit();
	}
}
