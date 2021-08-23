using UnityEngine;

public class Fear : State
{
	float fearDuration;

	public Fear(BaseAI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.FEAR;
	}

	public override void Enter()
	{
		npc.PathReached = false;
		anim.SetFloat("animSpeed", 0.5f);
		anim.SetTrigger("moving");
		base.Enter();

		PathData fearPosition = new PathData(-npc.transform.forward * 20.0f);
		fearDuration = npc.FearDuration;
		PathRequestManager.RequestPath(npc.transform.position, fearPosition, npc.Info.Range, npc.OnPathFound);
	}

	public override void Update()
	{
		fearDuration -= Time.deltaTime;
		if (fearDuration <= 0)
		{
			npc.setNewGoal();
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("moving");
		anim.SetFloat("animSpeed", 1f);
		npc.IsFeared = false;

		base.Exit();
	}
}