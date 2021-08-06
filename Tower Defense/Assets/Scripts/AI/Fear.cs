using UnityEngine;

// <summary>
// FSM state Fear. Enemies on this state will run away in the opposite direction
// </summary>
public class Fear : State
{
	float fearDuration;

	public Fear(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		name = STATE.FEAR;
	}

	public override void Enter()
	{
		npc.PathReached = false;
		anim.SetFloat("animSpeed", 0.5f);
		anim.SetTrigger("moving");
		base.Enter();

		PathData fearPosition = new PathData(-npc.transform.forward * 10.0f);
		fearDuration = npc.FearDuration;
		PathRequestManager.RequestPath(npc.transform.position, fearPosition, npc.Info.Range, npc.OnPathFound);
	}

	public override void Update()
	{
		fearDuration -= Time.deltaTime;
		if (fearDuration <= 0)
		{
			nextState = new Move(npc, anim, npc.Goal);
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