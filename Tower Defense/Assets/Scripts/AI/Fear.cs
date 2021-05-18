﻿using System.Collections;
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
		npc.pathReached = false;
		anim.SetFloat("animSpeed", 2f);
		anim.SetTrigger("moving");
		base.Enter();

		FearPosition = new PathData(-npc.transform.forward * 10.0f, 0);
		npc.currentTurret = null;
		fearDuration = npc.fearDuration;
		PathRequestManager.RequestPath(npc.transform.position, FearPosition, npc.Range, npc.OnPathFound);
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
		npc.isFeared = false;

		base.Exit();
	}
}