using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// FSM state Fear. Enemies on this state will run away in the opposite direction
// </summary>
public class Fear : State
{
	float fearDuration;

	public Fear(Base_AI _npc, Animator _anim, BuildingRange _target) : base(_npc, _anim, _target)
	{
		Name = STATE.FEAR;
	}

	public override void Enter()
	{
		anim.SetFloat("animSpeed", 2f);
		anim.SetTrigger("moving");
		base.Enter();

		fearDuration = npc.fearDuration;
	}

	public override void Update()
	{
		npc.Flee();

		fearDuration -= Time.deltaTime;
		if (fearDuration <= 0)
		{
			Debug.Log("Fear finished");
			nextState = new Move(npc, anim, target);
			stage = EVENT.EXIT;
		}
		else
		{
			Debug.Log("Time remaining running away: " + fearDuration);
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
