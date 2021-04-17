using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// FSM state Stun. Enemies on this state won't be able to move or attack
// </summary>
public class Stun : State
{
	float stunDuration;

	public Stun(Base_AI _npc, Animator _anim, BuildingRange _target) : base(_npc, _anim, _target)
	{
		Name = STATE.STUN;
	}

	public override void Enter()
	{
		anim.SetTrigger("stunned");
		base.Enter();

		stunDuration = npc.stunDuration;
	}

	public override void Update()
	{
		stunDuration -= Time.deltaTime;
		if(stunDuration <= 0)
        {
			Debug.Log("Stun finished");
			nextState = new Move(npc, anim, target);
			stage = EVENT.EXIT;
		}
        else
        {
			Debug.Log("Time remaining stunned: " + stunDuration);
		}
	}

	public override void Exit()
	{
		anim.ResetTrigger("stunned");
		npc.isStunned = false;
		base.Exit();
	}
}
