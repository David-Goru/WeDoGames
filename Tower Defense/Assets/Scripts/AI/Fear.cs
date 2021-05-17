using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// FSM state Fear. Enemies on this state will run away in the opposite direction
// </summary>
public class Fear : State
{
	float fearDuration;

	public Fear(Base_AI _npc, Animator _anim, Transform _target) : base(_npc, _anim, _target)
	{
		Name = STATE.FEAR;
	}

	public override void Enter()
	{
		anim.SetFloat("animSpeed", 2f);
		anim.SetTrigger("moving");
		base.Enter();

		npc.currentTurret = null;
		fearDuration = npc.fearDuration;
	}

	public override void Update()
	{
		npc.Flee();

		fearDuration -= Time.deltaTime;
		if (fearDuration <= 0)
		{
			nextState = new Move(npc, anim, Target);
			stage = EVENT.EXIT;
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("moving");
		anim.SetFloat("animSpeed", 1f);
		npc.isFeared = false;

		base.Exit();
	}
}
