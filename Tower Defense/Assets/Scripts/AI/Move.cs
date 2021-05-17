using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// FSM state Move. It will request paths to the pathfinding system.
// </summary>
public class Move : State
{

	public Move(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		Name = STATE.MOVE;
	}

	public override void Enter()
	{
		npc.pathReached = false;
		anim.SetTrigger("moving");
		base.Enter();

		float buildingRange = Target.GetComponent<BuildingRange>() != null ? Target.GetComponent<BuildingRange>().Range : 0;
		PathData newTarget = new PathData(Target.transform.position, buildingRange);
		PathRequestManager.RequestPath(npc.transform.position, newTarget, npc.Range, npc.OnPathFound);
	}

	public override void Update()
	{
		if (npc.pathReached && !npc.isStunned && !npc.isFeared)
        {
			nextState = new Attack(npc, anim, Target);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("moving");
		base.Exit();
	}
}
