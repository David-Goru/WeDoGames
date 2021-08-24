using UnityEngine;

public class Move : State
{
	public Move(BaseAI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.MOVE;
	}

	public override void Enter()
	{
		npc.PathReached = false;
		anim.SetTrigger("MOVE");
		anim.SetFloat("animSpeed", 1.0f);
		base.Enter();

		if (Target == null)
		{
			if (npc.CurrentTurret != null) Target = npc.CurrentTurret;
			else Target = npc.Goal;
		}

		PathData newTarget = new PathData(Target.position, Target);
		PathRequestManager.RequestPath(npc.transform.position, newTarget, npc.Info.Range, npc.OnPathFound);
	}

	public override void Update()
	{
		if (npc.PathReached && !npc.IsStunned && !npc.IsFeared && !npc.IsKnockbacked && npc.PathSuccessful)
        {
			nextState = new Attack(npc, anim, Target);
			stage = EVENT.EXIT;
		}
		else if (npc.PathReached && !npc.IsStunned && !npc.IsFeared && !npc.IsKnockbacked && !npc.PathSuccessful) // If PathSuccessful = false, then the path didn't reach the objective. So, try again
		{
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("MOVE");
		base.Exit();
	}
}