using UnityEngine;

// <summary>
// FSM state Move. It will request paths to the pathfinding system.
// </summary>
public class Move : State
{
	public Move(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.MOVE;
	}

	public override void Enter()
	{
		npc.PathReached = false;
		anim.SetTrigger("moving");
		base.Enter();

		PathData newTarget = new PathData(Target.position, Target);
		PathRequestManager.RequestPath(npc.transform.position, newTarget, npc.Range, npc.OnPathFound);
	}

	public override void Update()
	{
		if (npc.PathReached && !npc.IsStunned && !npc.IsFeared && npc.PathSuccessful)
        {
			nextState = new Attack(npc, anim, Target);
			stage = EVENT.EXIT;
		}
		else if (npc.PathReached && !npc.IsStunned && !npc.IsFeared && !npc.PathSuccessful) // If PathSuccessful = false, then the path didn't reach the objective. So, try again
		{
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("moving");
		base.Exit();
	}
}